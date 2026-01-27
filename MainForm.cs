namespace HelperApp;

public partial class MainForm : Form
{
    private readonly AppSettings _settings;
    private readonly BuildHelper _buildHelper;
    private bool _isRunning = false;

    public MainForm()
    {
        InitializeComponent();
        _settings = AppSettings.Load();
        _buildHelper = new BuildHelper();
        _buildHelper.OnLog += OnBuildLog;
        _buildHelper.OnComplete += OnBuildComplete;
        LoadSettings();
    }

    private void LoadSettings()
    {
        if (!string.IsNullOrEmpty(_settings.LastProjectPath))
            txtProjectPath.Text = _settings.LastProjectPath;
        
        if (!string.IsNullOrEmpty(_settings.LastOutputPath))
            txtOutputPath.Text = _settings.LastOutputPath;
        else
            txtOutputPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Deploy");

        if (!string.IsNullOrEmpty(_settings.LastVersion))
            txtNewVersion.Text = _settings.LastVersion;

        if (!string.IsNullOrEmpty(_settings.AzureSasUrl))
            txtAzureSasUrl.Text = _settings.AzureSasUrl;

        UpdateProjectInfo();
    }

    private void SaveSettings()
    {
        _settings.LastProjectPath = txtProjectPath.Text;
        _settings.LastOutputPath = txtOutputPath.Text;
        _settings.LastVersion = txtNewVersion.Text;
        _settings.AzureSasUrl = txtAzureSasUrl.Text;
        _settings.Save();
    }

    private void UpdateProjectInfo()
    {
        _buildHelper.ProjectPath = txtProjectPath.Text;
        _buildHelper.OutputPath = txtOutputPath.Text;
        _buildHelper.AzureSasUrl = txtAzureSasUrl.Text;

        var csproj = _buildHelper.FindCsprojFile();
        if (csproj != null)
        {
            lblProjectName.Text = Path.GetFileNameWithoutExtension(csproj);
            var currentVersion = _buildHelper.GetCurrentVersion();
            lblCurrentVersion.Text = currentVersion ?? "N/A";
            if (string.IsNullOrEmpty(txtNewVersion.Text) && !string.IsNullOrEmpty(currentVersion))
                txtNewVersion.Text = currentVersion;
        }
        else
        {
            lblProjectName.Text = "No project found";
            lblCurrentVersion.Text = "N/A";
        }
    }

    private void OnBuildLog(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnBuildLog(message));
            return;
        }
        txtConsole.AppendText(message + Environment.NewLine);
        txtConsole.SelectionStart = txtConsole.TextLength;
        txtConsole.ScrollToCaret();
    }

    private void OnBuildComplete(bool success)
    {
        if (InvokeRequired)
        {
            Invoke(() => OnBuildComplete(success));
            return;
        }
        SetRunningState(false);
    }

    private void SetRunningState(bool running)
    {
        _isRunning = running;
        btnClean.Enabled = !running;
        btnBuild.Enabled = !running;
        btnRebuild.Enabled = !running;
        btnPublish.Enabled = !running;
        btnCreatePackage.Enabled = !running;
        btnFullDeploy.Enabled = !running;
        btnApplyVersion.Enabled = !running;
        progressBar.Visible = running;
        progressBar.Style = running ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
    }

    private void BtnBrowseProject_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select Project Folder",
            ShowNewFolderButton = false
        };

        if (!string.IsNullOrEmpty(txtProjectPath.Text) && Directory.Exists(txtProjectPath.Text))
            dialog.SelectedPath = txtProjectPath.Text;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtProjectPath.Text = dialog.SelectedPath;
            UpdateProjectInfo();
            SaveSettings();
        }
    }

    private void BtnBrowseOutput_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select Output Folder",
            ShowNewFolderButton = true
        };

        if (!string.IsNullOrEmpty(txtOutputPath.Text) && Directory.Exists(txtOutputPath.Text))
            dialog.SelectedPath = txtOutputPath.Text;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtOutputPath.Text = dialog.SelectedPath;
            _buildHelper.OutputPath = dialog.SelectedPath;
            SaveSettings();
        }
    }

    private async void BtnApplyVersion_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        if (string.IsNullOrWhiteSpace(txtNewVersion.Text))
        {
            MessageBox.Show("Please enter a version!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetRunningState(true);
        txtConsole.Clear();
        
        // Auto-save VS files first
        _buildHelper.SaveVisualStudioFiles();
        
        _buildHelper.Version = txtNewVersion.Text;
        
        var success = await _buildHelper.UpdateVersionAsync(txtNewVersion.Text);
        if (success)
        {
            SaveSettings();
            UpdateProjectInfo();
        }
        SetRunningState(false);
    }

    private async void BtnClean_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        SetRunningState(true);
        txtConsole.Clear();
        await _buildHelper.CleanAsync();
        SetRunningState(false);
    }

    private async void BtnBuild_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        SetRunningState(true);
        txtConsole.Clear();
        _buildHelper.Version = txtNewVersion.Text;
        await _buildHelper.BuildAsync();
        SetRunningState(false);
    }

    private async void BtnRebuild_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        SetRunningState(true);
        txtConsole.Clear();
        _buildHelper.Version = txtNewVersion.Text;
        await _buildHelper.RebuildAsync();
        SetRunningState(false);
    }

    private async void BtnPublish_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        SetRunningState(true);
        txtConsole.Clear();
        _buildHelper.Version = txtNewVersion.Text;
        await _buildHelper.PublishAsync();
        SetRunningState(false);
    }

    private async void BtnCreatePackage_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        SetRunningState(true);
        txtConsole.Clear();
        _buildHelper.Version = txtNewVersion.Text;
        await _buildHelper.CreateDeployPackageAsync();
        SetRunningState(false);
        SaveSettings();
    }

    private async void BtnFullDeploy_Click(object sender, EventArgs e)
    {
        if (_isRunning) return;
        
        if (string.IsNullOrWhiteSpace(txtProjectPath.Text) || !Directory.Exists(txtProjectPath.Text))
        {
            MessageBox.Show("Please select a valid project folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetRunningState(true);
        txtConsole.Clear();
        
        // Auto-save VS files first
        _buildHelper.SaveVisualStudioFiles();
        
        _buildHelper.Version = txtNewVersion.Text;
        
        // First update version
        await _buildHelper.UpdateVersionAsync(txtNewVersion.Text);
        UpdateProjectInfo();
        
        // Then full deploy
        await _buildHelper.FullDeployAsync();
        SaveSettings();
        SetRunningState(false);
    }

    private void BtnOpenOutput_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtOutputPath.Text) && Directory.Exists(txtOutputPath.Text))
        {
            System.Diagnostics.Process.Start("explorer.exe", txtOutputPath.Text);
        }
        else
        {
            MessageBox.Show("Output folder does not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnClearConsole_Click(object sender, EventArgs e)
    {
        txtConsole.Clear();
    }

    private void TxtProjectPath_TextChanged(object sender, EventArgs e)
    {
        UpdateProjectInfo();
    }

    private void BtnEditAzure_Click(object sender, EventArgs e)
    {
        if (txtAzureSasUrl.ReadOnly)
        {
            // Unlock for editing
            txtAzureSasUrl.ReadOnly = false;
            txtAzureSasUrl.UseSystemPasswordChar = false;
            btnEditAzure.Text = "🔒 Save";
            btnEditAzure.BackColor = Color.FromArgb(0, 120, 200);
            txtAzureSasUrl.Focus();
        }
        else
        {
            // Lock and save
            txtAzureSasUrl.ReadOnly = true;
            txtAzureSasUrl.UseSystemPasswordChar = true;
            btnEditAzure.Text = "🔓 Edit";
            btnEditAzure.BackColor = Color.FromArgb(60, 60, 60);
            SaveSettings();
        }
    }
}
