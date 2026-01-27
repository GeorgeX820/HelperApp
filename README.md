# HelperApp - Build & Deploy Tool

A powerful WinForms application for automating .NET project builds, publishing, and Azure Blob Storage deployments.

## Features

- 🔨 **Build Automation**: Clean, Build, Rebuild, and Publish .NET projects
- 📦 **Package Creation**: Automatically create ZIP packages with version info
- ☁️ **Azure Integration**: Direct upload to Azure Blob Storage
- 🔢 **Version Management**: Update version across .csproj, version.xml, and changelog.html
- 💾 **Auto-Save**: Automatically saves Visual Studio files before operations
- 🧹 **Smart Cleanup**: Cleans old build artifacts and deploy folders
- 🔒 **Secure Settings**: Protected Azure SAS URL storage

## Requirements

- .NET 8.0 SDK
- Windows OS
- Visual Studio (optional, for auto-save feature)

## Installation

1. Clone the repository:
```bash
git clone https://github.com/GeorgeX820/HelperApp.git
cd HelperApp
```

2. Build the project:
```bash
dotnet build
```

3. Run the application:
```bash
dotnet run
```

## Usage

### Initial Setup
1. **Project Path**: Browse and select your .NET project folder
2. **Output Path**: Set the deployment output folder (default: Desktop\Deploy)
3. **Azure SAS URL**: Click 🔓 Edit, paste your Azure Blob Storage SAS URL, then click 🔒 Save

### Version Management
1. Enter new version (e.g., `1.0.0.4`)
2. Click **Apply Version** - updates .csproj, version.xml, and changelog.html

### Build Operations
- **🧹 Clean**: Remove build artifacts
- **🔨 Build**: Build in Release configuration
- **🔄 Rebuild**: Clean + Build
- **📤 Publish**: Publish to `bin\Release\net8.0-windows\publish\win-x64`
- **🗜️ Create ZIP**: Package publish output with version files
- **🚀 FULL DEPLOY**: Complete workflow (Build → Publish → Package → Azure Upload)

### Full Deploy Workflow
1. Auto-saves Visual Studio files (Ctrl+S)
2. Cleans deploy folder
3. Updates version
4. Builds project
5. Publishes application
6. Creates ZIP package
7. Deletes old Azure files
8. Uploads new files to Azure

## Configuration

Settings are automatically saved to:
```
%AppData%\HelperApp\settings.json
```

Includes:
- Last project path
- Last output path
- Last version
- Azure SAS URL (encrypted)

## Azure Setup

1. Go to Azure Portal → Storage Account → Container
2. Generate SAS token with permissions: Read, Write, Delete, List
3. Set expiry date (recommended: 1+ year)
4. Copy the Blob SAS URL
5. Paste in HelperApp Azure section

## Dependencies

- Azure.Storage.Blobs (12.27.0)
- System.Text.Json (built-in)

## License

MIT License

## Author

GeorgeX820
