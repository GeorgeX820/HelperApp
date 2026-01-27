namespace HelperApp;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        
        // Main Form
        this.Text = "HelperApp - Build & Deploy Tool";
        this.Size = new Size(800, 760);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.FromArgb(30, 30, 30);
        this.ForeColor = Color.White;
        this.Font = new Font("Segoe UI", 9F);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        // Title Panel
        panelTitle = new Panel();
        panelTitle.Dock = DockStyle.Top;
        panelTitle.Height = 50;
        panelTitle.BackColor = Color.FromArgb(45, 45, 45);

        lblTitle = new Label();
        lblTitle.Text = "🛠️ HelperApp - Build & Deploy Tool";
        lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(0, 150, 255);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(15, 12);
        panelTitle.Controls.Add(lblTitle);

        // Project Section
        panelProject = new Panel();
        panelProject.Location = new Point(15, 60);
        panelProject.Size = new Size(755, 90);
        panelProject.BackColor = Color.FromArgb(40, 40, 40);

        lblProjectSection = new Label();
        lblProjectSection.Text = "📁 PROJECT";
        lblProjectSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblProjectSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblProjectSection.Location = new Point(10, 8);
        lblProjectSection.AutoSize = true;

        lblProjectPathLabel = new Label();
        lblProjectPathLabel.Text = "Path:";
        lblProjectPathLabel.Location = new Point(10, 35);
        lblProjectPathLabel.AutoSize = true;

        txtProjectPath = new TextBox();
        txtProjectPath.Location = new Point(50, 32);
        txtProjectPath.Size = new Size(580, 23);
        txtProjectPath.BackColor = Color.FromArgb(50, 50, 50);
        txtProjectPath.ForeColor = Color.White;
        txtProjectPath.BorderStyle = BorderStyle.FixedSingle;
        txtProjectPath.TextChanged += TxtProjectPath_TextChanged;

        btnBrowseProject = new Button();
        btnBrowseProject.Text = "Browse...";
        btnBrowseProject.Location = new Point(640, 30);
        btnBrowseProject.Size = new Size(100, 27);
        btnBrowseProject.BackColor = Color.FromArgb(60, 60, 60);
        btnBrowseProject.FlatStyle = FlatStyle.Flat;
        btnBrowseProject.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        btnBrowseProject.Click += BtnBrowseProject_Click;

        lblProjectNameLabel = new Label();
        lblProjectNameLabel.Text = "Project:";
        lblProjectNameLabel.Location = new Point(10, 62);
        lblProjectNameLabel.AutoSize = true;

        lblProjectName = new Label();
        lblProjectName.Text = "No project selected";
        lblProjectName.ForeColor = Color.FromArgb(150, 200, 255);
        lblProjectName.Location = new Point(60, 62);
        lblProjectName.AutoSize = true;

        lblCurrentVersionLabel = new Label();
        lblCurrentVersionLabel.Text = "Current Version:";
        lblCurrentVersionLabel.Location = new Point(250, 62);
        lblCurrentVersionLabel.AutoSize = true;

        lblCurrentVersion = new Label();
        lblCurrentVersion.Text = "N/A";
        lblCurrentVersion.ForeColor = Color.FromArgb(255, 200, 100);
        lblCurrentVersion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblCurrentVersion.Location = new Point(355, 62);
        lblCurrentVersion.AutoSize = true;

        panelProject.Controls.AddRange(new Control[] { 
            lblProjectSection, lblProjectPathLabel, txtProjectPath, btnBrowseProject,
            lblProjectNameLabel, lblProjectName, lblCurrentVersionLabel, lblCurrentVersion 
        });

        // Version Section
        panelVersion = new Panel();
        panelVersion.Location = new Point(15, 158);
        panelVersion.Size = new Size(755, 55);
        panelVersion.BackColor = Color.FromArgb(40, 40, 40);

        lblVersionSection = new Label();
        lblVersionSection.Text = "🔢 VERSION";
        lblVersionSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblVersionSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblVersionSection.Location = new Point(10, 8);
        lblVersionSection.AutoSize = true;

        lblNewVersionLabel = new Label();
        lblNewVersionLabel.Text = "New Version:";
        lblNewVersionLabel.Location = new Point(120, 10);
        lblNewVersionLabel.AutoSize = true;

        txtNewVersion = new TextBox();
        txtNewVersion.Location = new Point(210, 7);
        txtNewVersion.Size = new Size(150, 23);
        txtNewVersion.BackColor = Color.FromArgb(50, 50, 50);
        txtNewVersion.ForeColor = Color.FromArgb(100, 255, 100);
        txtNewVersion.BorderStyle = BorderStyle.FixedSingle;
        txtNewVersion.Font = new Font("Consolas", 11F, FontStyle.Bold);
        txtNewVersion.TextAlign = HorizontalAlignment.Center;

        btnApplyVersion = new Button();
        btnApplyVersion.Text = "Apply Version";
        btnApplyVersion.Location = new Point(380, 5);
        btnApplyVersion.Size = new Size(120, 30);
        btnApplyVersion.BackColor = Color.FromArgb(0, 120, 200);
        btnApplyVersion.FlatStyle = FlatStyle.Flat;
        btnApplyVersion.FlatAppearance.BorderSize = 0;
        btnApplyVersion.Click += BtnApplyVersion_Click;

        lblVersionHint = new Label();
        lblVersionHint.Text = "(Updates .csproj & version.xml)";
        lblVersionHint.ForeColor = Color.Gray;
        lblVersionHint.Location = new Point(510, 12);
        lblVersionHint.AutoSize = true;

        panelVersion.Controls.AddRange(new Control[] { 
            lblVersionSection, lblNewVersionLabel, txtNewVersion, btnApplyVersion, lblVersionHint 
        });

        // Build Section
        panelBuild = new Panel();
        panelBuild.Location = new Point(15, 221);
        panelBuild.Size = new Size(755, 55);
        panelBuild.BackColor = Color.FromArgb(40, 40, 40);

        lblBuildSection = new Label();
        lblBuildSection.Text = "🔨 BUILD";
        lblBuildSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblBuildSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblBuildSection.Location = new Point(10, 15);
        lblBuildSection.AutoSize = true;

        btnClean = new Button();
        btnClean.Text = "🧹 Clean";
        btnClean.Location = new Point(120, 10);
        btnClean.Size = new Size(100, 35);
        btnClean.BackColor = Color.FromArgb(80, 80, 80);
        btnClean.FlatStyle = FlatStyle.Flat;
        btnClean.FlatAppearance.BorderSize = 0;
        btnClean.Click += BtnClean_Click;

        btnBuild = new Button();
        btnBuild.Text = "🔨 Build";
        btnBuild.Location = new Point(230, 10);
        btnBuild.Size = new Size(100, 35);
        btnBuild.BackColor = Color.FromArgb(0, 150, 80);
        btnBuild.FlatStyle = FlatStyle.Flat;
        btnBuild.FlatAppearance.BorderSize = 0;
        btnBuild.Click += BtnBuild_Click;

        btnRebuild = new Button();
        btnRebuild.Text = "🔄 Rebuild";
        btnRebuild.Location = new Point(340, 10);
        btnRebuild.Size = new Size(100, 35);
        btnRebuild.BackColor = Color.FromArgb(200, 150, 0);
        btnRebuild.FlatStyle = FlatStyle.Flat;
        btnRebuild.FlatAppearance.BorderSize = 0;
        btnRebuild.Click += BtnRebuild_Click;

        panelBuild.Controls.AddRange(new Control[] { lblBuildSection, btnClean, btnBuild, btnRebuild });

        // Publish Section
        panelPublish = new Panel();
        panelPublish.Location = new Point(15, 284);
        panelPublish.Size = new Size(755, 55);
        panelPublish.BackColor = Color.FromArgb(40, 40, 40);

        lblPublishSection = new Label();
        lblPublishSection.Text = "📦 PUBLISH";
        lblPublishSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPublishSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblPublishSection.Location = new Point(10, 15);
        lblPublishSection.AutoSize = true;

        btnPublish = new Button();
        btnPublish.Text = "📤 Publish";
        btnPublish.Location = new Point(120, 10);
        btnPublish.Size = new Size(110, 35);
        btnPublish.BackColor = Color.FromArgb(150, 80, 200);
        btnPublish.FlatStyle = FlatStyle.Flat;
        btnPublish.FlatAppearance.BorderSize = 0;
        btnPublish.Click += BtnPublish_Click;

        btnCreatePackage = new Button();
        btnCreatePackage.Text = "🗜️ Create ZIP";
        btnCreatePackage.Location = new Point(240, 10);
        btnCreatePackage.Size = new Size(120, 35);
        btnCreatePackage.BackColor = Color.FromArgb(200, 100, 50);
        btnCreatePackage.FlatStyle = FlatStyle.Flat;
        btnCreatePackage.FlatAppearance.BorderSize = 0;
        btnCreatePackage.Click += BtnCreatePackage_Click;

        btnFullDeploy = new Button();
        btnFullDeploy.Text = "🚀 FULL DEPLOY";
        btnFullDeploy.Location = new Point(380, 10);
        btnFullDeploy.Size = new Size(150, 35);
        btnFullDeploy.BackColor = Color.FromArgb(200, 50, 100);
        btnFullDeploy.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnFullDeploy.FlatStyle = FlatStyle.Flat;
        btnFullDeploy.FlatAppearance.BorderSize = 0;
        btnFullDeploy.Click += BtnFullDeploy_Click;

        panelPublish.Controls.AddRange(new Control[] { lblPublishSection, btnPublish, btnCreatePackage, btnFullDeploy });

        // Output Section
        panelOutput = new Panel();
        panelOutput.Location = new Point(15, 347);
        panelOutput.Size = new Size(755, 55);
        panelOutput.BackColor = Color.FromArgb(40, 40, 40);

        lblOutputSection = new Label();
        lblOutputSection.Text = "📂 OUTPUT";
        lblOutputSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblOutputSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblOutputSection.Location = new Point(10, 15);
        lblOutputSection.AutoSize = true;

        txtOutputPath = new TextBox();
        txtOutputPath.Location = new Point(100, 13);
        txtOutputPath.Size = new Size(430, 23);
        txtOutputPath.BackColor = Color.FromArgb(50, 50, 50);
        txtOutputPath.ForeColor = Color.White;
        txtOutputPath.BorderStyle = BorderStyle.FixedSingle;

        btnBrowseOutput = new Button();
        btnBrowseOutput.Text = "Browse...";
        btnBrowseOutput.Location = new Point(540, 10);
        btnBrowseOutput.Size = new Size(90, 30);
        btnBrowseOutput.BackColor = Color.FromArgb(60, 60, 60);
        btnBrowseOutput.FlatStyle = FlatStyle.Flat;
        btnBrowseOutput.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        btnBrowseOutput.Click += BtnBrowseOutput_Click;

        btnOpenOutput = new Button();
        btnOpenOutput.Text = "Open";
        btnOpenOutput.Location = new Point(640, 10);
        btnOpenOutput.Size = new Size(100, 30);
        btnOpenOutput.BackColor = Color.FromArgb(60, 60, 60);
        btnOpenOutput.FlatStyle = FlatStyle.Flat;
        btnOpenOutput.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        btnOpenOutput.Click += BtnOpenOutput_Click;

        panelOutput.Controls.AddRange(new Control[] { lblOutputSection, txtOutputPath, btnBrowseOutput, btnOpenOutput });

        // Azure Section
        panelAzure = new Panel();
        panelAzure.Location = new Point(15, 410);
        panelAzure.Size = new Size(755, 55);
        panelAzure.BackColor = Color.FromArgb(40, 40, 40);

        lblAzureSection = new Label();
        lblAzureSection.Text = "☁️ AZURE";
        lblAzureSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAzureSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblAzureSection.Location = new Point(10, 15);
        lblAzureSection.AutoSize = true;

        lblAzureSasLabel = new Label();
        lblAzureSasLabel.Text = "SAS URL:";
        lblAzureSasLabel.Location = new Point(80, 17);
        lblAzureSasLabel.AutoSize = true;

        txtAzureSasUrl = new TextBox();
        txtAzureSasUrl.Location = new Point(145, 13);
        txtAzureSasUrl.Size = new Size(520, 23);
        txtAzureSasUrl.BackColor = Color.FromArgb(50, 50, 50);
        txtAzureSasUrl.ForeColor = Color.FromArgb(100, 200, 255);
        txtAzureSasUrl.BorderStyle = BorderStyle.FixedSingle;
        txtAzureSasUrl.Font = new Font("Consolas", 8F);
        txtAzureSasUrl.ReadOnly = true;
        txtAzureSasUrl.UseSystemPasswordChar = true;

        btnEditAzure = new Button();
        btnEditAzure.Text = "🔓 Edit";
        btnEditAzure.Location = new Point(675, 10);
        btnEditAzure.Size = new Size(65, 30);
        btnEditAzure.BackColor = Color.FromArgb(60, 60, 60);
        btnEditAzure.FlatStyle = FlatStyle.Flat;
        btnEditAzure.FlatAppearance.BorderSize = 0;
        btnEditAzure.Click += BtnEditAzure_Click;

        panelAzure.Controls.AddRange(new Control[] { lblAzureSection, lblAzureSasLabel, txtAzureSasUrl, btnEditAzure });

        // Console Section
        panelConsole = new Panel();
        panelConsole.Location = new Point(15, 473);
        panelConsole.Size = new Size(755, 235);
        panelConsole.BackColor = Color.FromArgb(40, 40, 40);

        lblConsoleSection = new Label();
        lblConsoleSection.Text = "📋 CONSOLE OUTPUT";
        lblConsoleSection.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblConsoleSection.ForeColor = Color.FromArgb(0, 200, 150);
        lblConsoleSection.Location = new Point(10, 8);
        lblConsoleSection.AutoSize = true;

        btnClearConsole = new Button();
        btnClearConsole.Text = "Clear";
        btnClearConsole.Location = new Point(680, 5);
        btnClearConsole.Size = new Size(60, 25);
        btnClearConsole.BackColor = Color.FromArgb(60, 60, 60);
        btnClearConsole.FlatStyle = FlatStyle.Flat;
        btnClearConsole.FlatAppearance.BorderSize = 0;
        btnClearConsole.Click += BtnClearConsole_Click;

        txtConsole = new TextBox();
        txtConsole.Location = new Point(10, 35);
        txtConsole.Size = new Size(735, 190);
        txtConsole.Multiline = true;
        txtConsole.ScrollBars = ScrollBars.Vertical;
        txtConsole.ReadOnly = true;
        txtConsole.BackColor = Color.FromArgb(20, 20, 20);
        txtConsole.ForeColor = Color.FromArgb(200, 200, 200);
        txtConsole.Font = new Font("Consolas", 9F);
        txtConsole.BorderStyle = BorderStyle.None;

        progressBar = new ProgressBar();
        progressBar.Location = new Point(180, 7);
        progressBar.Size = new Size(490, 20);
        progressBar.Style = ProgressBarStyle.Marquee;
        progressBar.Visible = false;

        panelConsole.Controls.AddRange(new Control[] { lblConsoleSection, btnClearConsole, txtConsole, progressBar });

        // Add all panels to form
        this.Controls.AddRange(new Control[] { 
            panelTitle, panelProject, panelVersion, panelBuild, 
            panelPublish, panelOutput, panelAzure, panelConsole 
        });
    }

    #endregion

    private Panel panelTitle;
    private Panel panelProject;
    private Panel panelVersion;
    private Panel panelBuild;
    private Panel panelPublish;
    private Panel panelOutput;
    private Panel panelAzure;
    private Panel panelConsole;

    private Label lblTitle;
    private Label lblProjectSection;
    private Label lblProjectPathLabel;
    private Label lblProjectNameLabel;
    private Label lblProjectName;
    private Label lblCurrentVersionLabel;
    private Label lblCurrentVersion;
    private Label lblVersionSection;
    private Label lblNewVersionLabel;
    private Label lblVersionHint;
    private Label lblBuildSection;
    private Label lblPublishSection;
    private Label lblOutputSection;
    private Label lblConsoleSection;
    private Label lblAzureSection;
    private Label lblAzureSasLabel;

    private TextBox txtProjectPath;
    private TextBox txtNewVersion;
    private TextBox txtOutputPath;
    private TextBox txtConsole;
    private TextBox txtAzureSasUrl;

    private Button btnBrowseProject;
    private Button btnApplyVersion;
    private Button btnClean;
    private Button btnBuild;
    private Button btnRebuild;
    private Button btnPublish;
    private Button btnCreatePackage;
    private Button btnFullDeploy;
    private Button btnBrowseOutput;
    private Button btnOpenOutput;
    private Button btnClearConsole;
    private Button btnEditAzure;

    private ProgressBar progressBar;
}
