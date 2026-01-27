using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace HelperApp;

public class BuildHelper
{
    // Windows API for sending keystrokes
    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    private const int SW_RESTORE = 9;

    public string ProjectPath { get; set; } = string.Empty;
    public string OutputPath { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0.0.0";
    public string AzureSasUrl { get; set; } = string.Empty;

    public event Action<string>? OnLog;
    public event Action<bool>? OnComplete;

    private void Log(string message)
    {
        OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");
    }

    public bool SaveVisualStudioFiles()
    {
        try
        {
            Log("💾 Saving Visual Studio files (Ctrl+S)...");
            
            // Find Visual Studio process
            var vsProcesses = Process.GetProcesses()
                .Where(p => p.ProcessName.Contains("devenv"))
                .ToList();

            if (vsProcesses.Count == 0)
            {
                Log("⚠️ Visual Studio not found - skipping auto-save");
                return true;
            }

            foreach (var vsProcess in vsProcesses)
            {
                try
                {
                    var hwnd = vsProcess.MainWindowHandle;
                    if (hwnd != IntPtr.Zero)
                    {
                        // Bring VS to foreground
                        ShowWindow(hwnd, SW_RESTORE);
                        SetForegroundWindow(hwnd);
                        Thread.Sleep(200);
                        
                        // Send Ctrl+S
                        SendKeys.SendWait("^s");
                        Thread.Sleep(300);
                        
                        Log($"✅ Saved: {vsProcess.MainWindowTitle}");
                    }
                }
                catch { }
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Log($"⚠️ Auto-save warning: {ex.Message}");
            return true;
        }
    }

    public string? FindCsprojFile()
    {
        if (string.IsNullOrEmpty(ProjectPath) || !Directory.Exists(ProjectPath))
            return null;

        var csprojFiles = Directory.GetFiles(ProjectPath, "*.csproj", SearchOption.TopDirectoryOnly);
        return csprojFiles.FirstOrDefault();
    }

    public string? GetCurrentVersion()
    {
        var csproj = FindCsprojFile();
        if (csproj == null) return null;

        try
        {
            var doc = XDocument.Load(csproj);
            var versionElement = doc.Descendants("Version").FirstOrDefault();
            return versionElement?.Value;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdateVersionAsync(string newVersion)
    {
        return await Task.Run(() =>
        {
            try
            {
                var csproj = FindCsprojFile();
                if (csproj == null)
                {
                    Log("❌ No .csproj file found!");
                    return false;
                }

                Log($"📝 Updating version to {newVersion}...");

                // Update .csproj
                var csprojContent = File.ReadAllText(csproj);
                csprojContent = Regex.Replace(csprojContent, @"<Version>[^<]*</Version>", $"<Version>{newVersion}</Version>");
                csprojContent = Regex.Replace(csprojContent, @"<AssemblyVersion>[^<]*</AssemblyVersion>", $"<AssemblyVersion>{newVersion}</AssemblyVersion>");
                csprojContent = Regex.Replace(csprojContent, @"<FileVersion>[^<]*</FileVersion>", $"<FileVersion>{newVersion}</FileVersion>");
                File.WriteAllText(csproj, csprojContent);
                Log($"✓ Updated: {Path.GetFileName(csproj)}");

                // Update version.xml if exists
                var versionXmlPath = Path.Combine(ProjectPath, "AzureSetup", "version.xml");
                if (File.Exists(versionXmlPath))
                {
                    var xmlContent = File.ReadAllText(versionXmlPath);
                    xmlContent = Regex.Replace(xmlContent, @"<version>[^<]*</version>", $"<version>{newVersion}</version>");
                    
                    // Update ZIP filename in URL
                    var projectName = Path.GetFileNameWithoutExtension(csproj);
                    xmlContent = Regex.Replace(xmlContent, 
                        $@"{projectName}-v[\d\.]+\.zip", 
                        $"{projectName}-v{newVersion}.zip");
                    
                    File.WriteAllText(versionXmlPath, xmlContent);
                    Log($"✓ Updated: version.xml");
                }

                // Update changelog.html if exists
                var changelogPath = Path.Combine(ProjectPath, "AzureSetup", "changelog.html");
                if (File.Exists(changelogPath))
                {
                    var changelogContent = File.ReadAllText(changelogPath);
                    // Update version in title or header
                    changelogContent = Regex.Replace(changelogContent, @"Version [\d\.]+", $"Version {newVersion}");
                    changelogContent = Regex.Replace(changelogContent, @"v[\d\.]+", $"v{newVersion}");
                    File.WriteAllText(changelogPath, changelogContent);
                    Log($"✓ Updated: changelog.html");
                }

                Version = newVersion;
                Log($"✅ Version updated to {newVersion}");
                return true;
            }
            catch (Exception ex)
            {
                Log($"❌ Error: {ex.Message}");
                return false;
            }
        });
    }

    public async Task<bool> CleanAsync()
    {
        return await RunDotnetCommandAsync("clean", "Cleaning");
    }

    private void CleanFolder(string folderPath, string folderName)
    {
        if (Directory.Exists(folderPath))
        {
            try
            {
                Directory.Delete(folderPath, true);
                Log($"🗑️ Deleted: {folderName}");
            }
            catch (Exception ex)
            {
                Log($"⚠️ Could not delete {folderName}: {ex.Message}");
            }
        }
    }

    private void CleanBuildFolders()
    {
        Log("🧹 Cleaning old build files...");
        var binPath = Path.Combine(ProjectPath, "bin");
        var objPath = Path.Combine(ProjectPath, "obj");
        CleanFolder(binPath, "bin folder");
        CleanFolder(objPath, "obj folder");
    }

    private void CleanPublishFolder()
    {
        Log("🧹 Cleaning old publish files...");
        var publishPath = Path.Combine(ProjectPath, "bin", "Release", "net8.0-windows", "publish");
        CleanFolder(publishPath, "publish folder");
    }

    public void CleanOutputFolder()
    {
        if (string.IsNullOrEmpty(OutputPath)) return;
        
        Log("🧹 Cleaning Deploy folder...");
        if (Directory.Exists(OutputPath))
        {
            foreach (var file in Directory.GetFiles(OutputPath))
            {
                try
                {
                    File.Delete(file);
                    Log($"🗑️ Deleted: {Path.GetFileName(file)}");
                }
                catch { }
            }
        }
    }

    // Note: This is handled in MainForm with user confirmation dialog
    // dotnet restore doesn't save VS open files - user must save manually

    public async Task<bool> BuildAsync()
    {
        await Task.Run(() => CleanBuildFolders());
        return await RunDotnetCommandAsync("build -c Release", "Building");
    }

    public async Task<bool> RebuildAsync()
    {
        Log("🔄 Rebuilding (Clean + Build)...");
        if (!await CleanAsync()) return false;
        return await BuildAsync();
    }

    public async Task<bool> PublishAsync()
    {
        await Task.Run(() => CleanPublishFolder());
        // Output to bin\Release\net8.0-windows\publish\win-x64 (matches VS profile)
        var publishOutputPath = Path.Combine(ProjectPath, "bin", "Release", "net8.0-windows", "publish", "win-x64");
        var args = $"publish -c Release -r win-x64 --self-contained false -o \"{publishOutputPath}\" -p:Version={Version} -p:AssemblyVersion={Version} -p:FileVersion={Version}";
        return await RunDotnetCommandAsync(args, "Publishing");
    }

    private async Task<bool> RunDotnetCommandAsync(string arguments, string action)
    {
        return await Task.Run(() =>
        {
            try
            {
                Log($"🔨 {action}...");

                var psi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = arguments,
                    WorkingDirectory = ProjectPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                if (process == null)
                {
                    Log("❌ Failed to start dotnet process");
                    return false;
                }

                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    foreach (var line in output.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)))
                    {
                        if (line.Contains("error") || line.Contains("warning") || line.Contains("->"))
                            Log(line.Trim());
                    }
                }

                if (process.ExitCode == 0)
                {
                    Log($"✅ {action} succeeded!");
                    return true;
                }
                else
                {
                    Log($"❌ {action} failed! Exit code: {process.ExitCode}");
                    if (!string.IsNullOrWhiteSpace(error))
                        Log(error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"❌ Error: {ex.Message}");
                return false;
            }
        });
    }

    public async Task<bool> CreateDeployPackageAsync()
    {
        return await Task.Run(() =>
        {
            try
            {
                var csproj = FindCsprojFile();
                if (csproj == null)
                {
                    Log("❌ No .csproj file found!");
                    return false;
                }

                var projectName = Path.GetFileNameWithoutExtension(csproj);
                var publishPath = Path.Combine(ProjectPath, "bin", "Release", "net8.0-windows", "publish", "win-x64");

                if (!Directory.Exists(publishPath))
                {
                    Log($"❌ Publish folder not found: {publishPath}");
                    Log("💡 Please run Publish first!");
                    return false;
                }

                Log("📦 Creating deploy package...");

                // Create output directory if needed
                if (!Directory.Exists(OutputPath))
                    Directory.CreateDirectory(OutputPath);

                // ZIP filename
                var zipFileName = $"{projectName}-v{Version}.zip";
                var zipPath = Path.Combine(OutputPath, zipFileName);

                // Delete existing ZIP if exists
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath);
                    Log($"🗑️ Deleted existing: {zipFileName}");
                }

                // Create temp folder for packaging
                var tempPath = Path.Combine(Path.GetTempPath(), $"HelperApp_{Guid.NewGuid():N}");
                Directory.CreateDirectory(tempPath);

                try
                {
                    // Copy all publish files and subdirectories
                    Log("📋 Copying all publish files...");
                    CopyDirectory(publishPath, tempPath);
                    
                    var fileCount = Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories).Length;
                    Log($"✓ Copied {fileCount} files");

                    // Create ZIP
                    Log($"🗜️ Creating ZIP: {zipFileName}");
                    ZipFile.CreateFromDirectory(tempPath, zipPath, CompressionLevel.Optimal, false);

                    var fileInfo = new FileInfo(zipPath);
                    Log($"✅ Package created: {zipFileName}");
                    Log($"📊 Size: {fileInfo.Length / 1024.0 / 1024.0:F2} MB");
                    Log($"📂 Location: {zipPath}");

                    // Also copy version.xml and changelog.html to output folder (for Azure upload)
                    var versionXmlSrc = Path.Combine(ProjectPath, "AzureSetup", "version.xml");
                    if (File.Exists(versionXmlSrc))
                    {
                        File.Copy(versionXmlSrc, Path.Combine(OutputPath, "version.xml"), true);
                        Log("✓ Copied version.xml to output folder");
                    }

                    var changelogSrc = Path.Combine(ProjectPath, "AzureSetup", "changelog.html");
                    if (File.Exists(changelogSrc))
                    {
                        File.Copy(changelogSrc, Path.Combine(OutputPath, "changelog.html"), true);
                        Log("✓ Copied changelog.html to output folder");
                    }

                    return true;
                }
                finally
                {
                    // Cleanup temp folder
                    if (Directory.Exists(tempPath))
                        Directory.Delete(tempPath, true);
                }
            }
            catch (Exception ex)
            {
                Log($"❌ Error: {ex.Message}");
                return false;
            }
        });
    }

    public async Task<bool> FullDeployAsync()
    {
        Log("🚀 Starting full deploy process...");
        Log("═══════════════════════════════════════");
        
        // Clean output/deploy folder
        await Task.Run(() => CleanOutputFolder());

        if (!await BuildAsync()) return false;
        if (!await PublishAsync()) return false;
        if (!await CreateDeployPackageAsync()) return false;
        
        // Upload to Azure if configured
        if (!string.IsNullOrEmpty(AzureSasUrl))
        {
            if (!await UploadToAzureAsync()) return false;
        }
        else
        {
            Log("⚠️ Azure SAS URL not configured - skipping upload");
        }

        Log("═══════════════════════════════════════");
        Log("🎉 Full deploy completed successfully!");
        OnComplete?.Invoke(true);
        return true;
    }

    public async Task<bool> UploadToAzureAsync()
    {
        return await Task.Run(async () =>
        {
            try
            {
                Log("☁️ Uploading to Azure Blob Storage...");
                
                if (string.IsNullOrEmpty(AzureSasUrl))
                {
                    Log("❌ Azure SAS URL not configured!");
                    return false;
                }

                var containerClient = new BlobContainerClient(new Uri(AzureSasUrl));
                
                // Delete old blobs first
                Log("🗑️ Deleting old files from Azure...");
                await foreach (var blobItem in containerClient.GetBlobsAsync())
                {
                    var blobClient = containerClient.GetBlobClient(blobItem.Name);
                    await blobClient.DeleteIfExistsAsync();
                    Log($"   Deleted: {blobItem.Name}");
                }

                // Upload new files from output folder
                Log("📤 Uploading new files...");
                if (Directory.Exists(OutputPath))
                {
                    foreach (var file in Directory.GetFiles(OutputPath))
                    {
                        var fileName = Path.GetFileName(file);
                        var blobClient = containerClient.GetBlobClient(fileName);
                        
                        using var stream = File.OpenRead(file);
                        await blobClient.UploadAsync(stream, overwrite: true);
                        Log($"   ✅ Uploaded: {fileName}");
                    }
                }

                Log("☁️ Azure upload completed!");
                return true;
            }
            catch (Exception ex)
            {
                Log($"❌ Azure Error: {ex.Message}");
                return false;
            }
        });
    }

    private void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var dir in Directory.GetDirectories(sourceDir))
        {
            var destSubDir = Path.Combine(destDir, Path.GetFileName(dir));
            CopyDirectory(dir, destSubDir);
        }
    }
}
