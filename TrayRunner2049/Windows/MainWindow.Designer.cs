namespace TrayRunner2049.Windows;

partial class MainWindow
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;


    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
        tlpMainLayout = new System.Windows.Forms.TableLayoutPanel();
        tlpProfileLayout = new System.Windows.Forms.TableLayoutPanel();
        lblProfile = new System.Windows.Forms.Label();
        txtProfileName = new System.Windows.Forms.TextBox();
        btnSettings = new System.Windows.Forms.Button();
        txtOutput = new TrayRunner2049.Controls.AnsiRichTextBox();
        tplInteractionLayout = new System.Windows.Forms.TableLayoutPanel();
        btnClear = new System.Windows.Forms.Button();
        txtConsoleInput = new System.Windows.Forms.TextBox();
        btnEnter = new System.Windows.Forms.Button();
        tplProcessControlLayout = new System.Windows.Forms.TableLayoutPanel();
        btnStartProcess = new System.Windows.Forms.Button();
        btnStopProcess = new System.Windows.Forms.Button();
        notifyIcon = new System.Windows.Forms.NotifyIcon(components);
        tlpMainLayout.SuspendLayout();
        tlpProfileLayout.SuspendLayout();
        tplInteractionLayout.SuspendLayout();
        tplProcessControlLayout.SuspendLayout();
        SuspendLayout();
        //
        // tlpMainLayout
        //
        tlpMainLayout.ColumnCount = 1;
        tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpMainLayout.Controls.Add(tlpProfileLayout, 0, 0);
        tlpMainLayout.Controls.Add(txtOutput, 0, 1);
        tlpMainLayout.Controls.Add(tplInteractionLayout, 0, 2);
        tlpMainLayout.Controls.Add(tplProcessControlLayout, 0, 3);
        tlpMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpMainLayout.Location = new System.Drawing.Point(0, 0);
        tlpMainLayout.Name = "tlpMainLayout";
        tlpMainLayout.Padding = new System.Windows.Forms.Padding(3);
        tlpMainLayout.RowCount = 4;
        tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpMainLayout.Size = new System.Drawing.Size(592, 373);
        tlpMainLayout.TabIndex = 0;
        //
        // tlpProfileLayout
        //
        tlpProfileLayout.ColumnCount = 3;
        tlpProfileLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tlpProfileLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpProfileLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tlpProfileLayout.Controls.Add(lblProfile, 0, 0);
        tlpProfileLayout.Controls.Add(txtProfileName, 1, 0);
        tlpProfileLayout.Controls.Add(btnSettings, 2, 0);
        tlpProfileLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpProfileLayout.Location = new System.Drawing.Point(3, 3);
        tlpProfileLayout.Margin = new System.Windows.Forms.Padding(0);
        tlpProfileLayout.Name = "tlpProfileLayout";
        tlpProfileLayout.RowCount = 1;
        tlpProfileLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpProfileLayout.Size = new System.Drawing.Size(586, 30);
        tlpProfileLayout.TabIndex = 0;
        //
        // lblProfile
        //
        lblProfile.AutoSize = true;
        lblProfile.Dock = System.Windows.Forms.DockStyle.Fill;
        lblProfile.Location = new System.Drawing.Point(3, 3);
        lblProfile.Margin = new System.Windows.Forms.Padding(3);
        lblProfile.Name = "lblProfile";
        lblProfile.Size = new System.Drawing.Size(44, 24);
        lblProfile.TabIndex = 0;
        lblProfile.Text = "Profile:";
        lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        //
        // txtProfileName
        //
        txtProfileName.Cursor = System.Windows.Forms.Cursors.Default;
        txtProfileName.Dock = System.Windows.Forms.DockStyle.Fill;
        txtProfileName.Location = new System.Drawing.Point(53, 3);
        txtProfileName.Name = "txtProfileName";
        txtProfileName.ReadOnly = true;
        txtProfileName.Size = new System.Drawing.Size(465, 23);
        txtProfileName.TabIndex = 0;
        txtProfileName.TabStop = false;
        //
        // btnSettings
        //
        btnSettings.AutoSize = true;
        btnSettings.Dock = System.Windows.Forms.DockStyle.Fill;
        btnSettings.Location = new System.Drawing.Point(524, 3);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new System.Drawing.Size(59, 24);
        btnSettings.TabIndex = 6;
        btnSettings.Text = "Settings";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        //
        // txtOutput
        //
        txtOutput.BackColor = System.Drawing.Color.Black;
        txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
        txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
        txtOutput.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)0));
        txtOutput.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)224)), ((int)((byte)224)), ((int)((byte)224)));
        txtOutput.Location = new System.Drawing.Point(6, 36);
        txtOutput.MaxLength = 0;
        txtOutput.Name = "txtOutput";
        txtOutput.ReadOnly = true;
        txtOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
        txtOutput.Size = new System.Drawing.Size(580, 271);
        txtOutput.TabIndex = 0;
        txtOutput.TabStop = false;
        txtOutput.Text = "";
        txtOutput.LinkClicked += txtOutput_LinkClicked;
        //
        // tplInteractionLayout
        //
        tplInteractionLayout.ColumnCount = 3;
        tplInteractionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tplInteractionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tplInteractionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tplInteractionLayout.Controls.Add(btnClear, 2, 0);
        tplInteractionLayout.Controls.Add(txtConsoleInput, 0, 0);
        tplInteractionLayout.Controls.Add(btnEnter, 1, 0);
        tplInteractionLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        tplInteractionLayout.Location = new System.Drawing.Point(3, 310);
        tplInteractionLayout.Margin = new System.Windows.Forms.Padding(0);
        tplInteractionLayout.Name = "tplInteractionLayout";
        tplInteractionLayout.RowCount = 1;
        tplInteractionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tplInteractionLayout.Size = new System.Drawing.Size(586, 30);
        tplInteractionLayout.TabIndex = 0;
        //
        // btnClear
        //
        btnClear.AutoSize = true;
        btnClear.Location = new System.Drawing.Point(518, 3);
        btnClear.Name = "btnClear";
        btnClear.Size = new System.Drawing.Size(65, 24);
        btnClear.TabIndex = 3;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = true;
        btnClear.Click += btnClear_Click;
        //
        // txtConsoleInput
        //
        txtConsoleInput.BackColor = System.Drawing.Color.Black;
        txtConsoleInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        txtConsoleInput.Dock = System.Windows.Forms.DockStyle.Fill;
        txtConsoleInput.ForeColor = System.Drawing.Color.FromArgb(((int)((byte)224)), ((int)((byte)224)), ((int)((byte)224)));
        txtConsoleInput.Location = new System.Drawing.Point(3, 3);
        txtConsoleInput.Name = "txtConsoleInput";
        txtConsoleInput.Size = new System.Drawing.Size(438, 23);
        txtConsoleInput.TabIndex = 1;
        txtConsoleInput.KeyDown += txtConsoleInput_KeyDown;
        //
        // btnEnter
        //
        btnEnter.AutoSize = true;
        btnEnter.Location = new System.Drawing.Point(447, 3);
        btnEnter.Name = "btnEnter";
        btnEnter.Size = new System.Drawing.Size(65, 24);
        btnEnter.TabIndex = 2;
        btnEnter.Text = "Enter";
        btnEnter.UseVisualStyleBackColor = true;
        btnEnter.Click += btnEnter_Click;
        //
        // tplProcessControlLayout
        //
        tplProcessControlLayout.ColumnCount = 3;
        tplProcessControlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tplProcessControlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tplProcessControlLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tplProcessControlLayout.Controls.Add(btnStartProcess, 1, 0);
        tplProcessControlLayout.Controls.Add(btnStopProcess, 2, 0);
        tplProcessControlLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        tplProcessControlLayout.Location = new System.Drawing.Point(3, 340);
        tplProcessControlLayout.Margin = new System.Windows.Forms.Padding(0);
        tplProcessControlLayout.Name = "tplProcessControlLayout";
        tplProcessControlLayout.RowCount = 1;
        tplProcessControlLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tplProcessControlLayout.Size = new System.Drawing.Size(586, 30);
        tplProcessControlLayout.TabIndex = 3;
        //
        // btnStartProcess
        //
        btnStartProcess.AutoSize = true;
        btnStartProcess.Location = new System.Drawing.Point(447, 3);
        btnStartProcess.Name = "btnStartProcess";
        btnStartProcess.Size = new System.Drawing.Size(65, 24);
        btnStartProcess.TabIndex = 4;
        btnStartProcess.Text = "Start";
        btnStartProcess.UseVisualStyleBackColor = true;
        btnStartProcess.Click += btnStartProcess_Click;
        //
        // btnStopProcess
        //
        btnStopProcess.AutoSize = true;
        btnStopProcess.Location = new System.Drawing.Point(518, 3);
        btnStopProcess.Name = "btnStopProcess";
        btnStopProcess.Size = new System.Drawing.Size(65, 24);
        btnStopProcess.TabIndex = 5;
        btnStopProcess.Text = "Stop";
        btnStopProcess.UseVisualStyleBackColor = true;
        btnStopProcess.Click += btnStopProcess_Click;
        //
        // notifyIcon
        //
        notifyIcon.Visible = true;
        notifyIcon.DoubleClick += notifyIcon_DoubleClick;
        //
        // MainWindow
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(592, 373);
        Controls.Add(tlpMainLayout);
        Icon = ((System.Drawing.Icon)resources.GetObject("$this.Icon"));
        FormClosing += MainWindow_FormClosing;
        Load += MainWindow_Load;
        Resize += MainWindow_Resize;
        tlpMainLayout.ResumeLayout(false);
        tlpProfileLayout.ResumeLayout(false);
        tlpProfileLayout.PerformLayout();
        tplInteractionLayout.ResumeLayout(false);
        tplInteractionLayout.PerformLayout();
        tplProcessControlLayout.ResumeLayout(false);
        tplProcessControlLayout.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button btnClear;

    private System.Windows.Forms.NotifyIcon notifyIcon;

    private System.Windows.Forms.Button btnStopProcess;

    private System.Windows.Forms.Button btnStartProcess;

    private System.Windows.Forms.TableLayoutPanel tplProcessControlLayout;

    private System.Windows.Forms.Button btnEnter;

    private System.Windows.Forms.TextBox txtConsoleInput;

    private System.Windows.Forms.TableLayoutPanel tplInteractionLayout;

    private TrayRunner2049.Controls.AnsiRichTextBox txtOutput;

    private System.Windows.Forms.Button btnSettings;

    private System.Windows.Forms.TextBox txtProfileName;

    private System.Windows.Forms.Label lblProfile;

    private System.Windows.Forms.TableLayoutPanel tlpProfileLayout;

    private System.Windows.Forms.TableLayoutPanel tlpMainLayout;

    #endregion
}