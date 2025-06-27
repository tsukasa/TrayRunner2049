using System.ComponentModel;

namespace TrayRunner2049.Windows;

partial class SettingsWindow
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
        tlpSettingsWindowLayout = new System.Windows.Forms.TableLayoutPanel();
        tlpProfileRow = new System.Windows.Forms.TableLayoutPanel();
        btnDelete = new System.Windows.Forms.Button();
        cmbProfile = new System.Windows.Forms.ComboBox();
        lblProfile = new System.Windows.Forms.Label();
        tlpFileNameRow = new System.Windows.Forms.TableLayoutPanel();
        txtFileName = new System.Windows.Forms.TextBox();
        lblFileName = new System.Windows.Forms.Label();
        tlpArgumentsRow = new System.Windows.Forms.TableLayoutPanel();
        txtArguments = new System.Windows.Forms.TextBox();
        lblArguments = new System.Windows.Forms.Label();
        tlpWorkingDirectoryRow = new System.Windows.Forms.TableLayoutPanel();
        txtWorkingDirectory = new System.Windows.Forms.TextBox();
        lblWorkingDirectory = new System.Windows.Forms.Label();
        tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        chkAutoRestart = new System.Windows.Forms.CheckBox();
        btnIcon = new System.Windows.Forms.Button();
        lblSystemTrayIcon = new System.Windows.Forms.Label();
        tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        btnCancel = new System.Windows.Forms.Button();
        btnOk = new System.Windows.Forms.Button();
        tlpEncodingPanel = new System.Windows.Forms.TableLayoutPanel();
        lblOutputEncoding = new System.Windows.Forms.Label();
        cmbOutputEncoding = new System.Windows.Forms.ComboBox();
        tlpSettingsWindowLayout.SuspendLayout();
        tlpProfileRow.SuspendLayout();
        tlpFileNameRow.SuspendLayout();
        tlpArgumentsRow.SuspendLayout();
        tlpWorkingDirectoryRow.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        tlpEncodingPanel.SuspendLayout();
        SuspendLayout();
        // 
        // tlpSettingsWindowLayout
        // 
        tlpSettingsWindowLayout.ColumnCount = 1;
        tlpSettingsWindowLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpSettingsWindowLayout.Controls.Add(tlpProfileRow, 0, 0);
        tlpSettingsWindowLayout.Controls.Add(tlpFileNameRow, 0, 1);
        tlpSettingsWindowLayout.Controls.Add(tlpArgumentsRow, 0, 2);
        tlpSettingsWindowLayout.Controls.Add(tlpWorkingDirectoryRow, 0, 3);
        tlpSettingsWindowLayout.Controls.Add(tableLayoutPanel1, 0, 5);
        tlpSettingsWindowLayout.Controls.Add(tableLayoutPanel2, 0, 6);
        tlpSettingsWindowLayout.Controls.Add(tlpEncodingPanel, 0, 4);
        tlpSettingsWindowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpSettingsWindowLayout.Location = new System.Drawing.Point(0, 0);
        tlpSettingsWindowLayout.Name = "tlpSettingsWindowLayout";
        tlpSettingsWindowLayout.Padding = new System.Windows.Forms.Padding(3);
        tlpSettingsWindowLayout.RowCount = 7;
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
        tlpSettingsWindowLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tlpSettingsWindowLayout.Size = new System.Drawing.Size(414, 230);
        tlpSettingsWindowLayout.TabIndex = 14;
        // 
        // tlpProfileRow
        // 
        tlpProfileRow.ColumnCount = 3;
        tlpSettingsWindowLayout.SetColumnSpan(tlpProfileRow, 3);
        tlpProfileRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        tlpProfileRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpProfileRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tlpProfileRow.Controls.Add(btnDelete, 0, 0);
        tlpProfileRow.Controls.Add(cmbProfile, 0, 0);
        tlpProfileRow.Controls.Add(lblProfile, 0, 0);
        tlpProfileRow.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpProfileRow.Location = new System.Drawing.Point(6, 3);
        tlpProfileRow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
        tlpProfileRow.Name = "tlpProfileRow";
        tlpProfileRow.RowCount = 1;
        tlpProfileRow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpProfileRow.Size = new System.Drawing.Size(402, 30);
        tlpProfileRow.TabIndex = 0;
        // 
        // btnDelete
        // 
        btnDelete.AutoSize = true;
        btnDelete.Location = new System.Drawing.Point(349, 3);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new System.Drawing.Size(50, 24);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;
        // 
        // cmbProfile
        // 
        cmbProfile.Dock = System.Windows.Forms.DockStyle.Fill;
        cmbProfile.FormattingEnabled = true;
        cmbProfile.Location = new System.Drawing.Point(123, 3);
        cmbProfile.Name = "cmbProfile";
        cmbProfile.Size = new System.Drawing.Size(220, 23);
        cmbProfile.TabIndex = 1;
        cmbProfile.SelectedIndexChanged += cmbProfile_SelectedIndexChanged;
        // 
        // lblProfile
        // 
        lblProfile.Dock = System.Windows.Forms.DockStyle.Fill;
        lblProfile.Location = new System.Drawing.Point(3, 0);
        lblProfile.Name = "lblProfile";
        lblProfile.Size = new System.Drawing.Size(114, 30);
        lblProfile.TabIndex = 0;
        lblProfile.Text = "Profile:";
        lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // tlpFileNameRow
        // 
        tlpFileNameRow.ColumnCount = 2;
        tlpFileNameRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        tlpFileNameRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tlpFileNameRow.Controls.Add(txtFileName, 1, 0);
        tlpFileNameRow.Controls.Add(lblFileName, 0, 0);
        tlpFileNameRow.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpFileNameRow.Location = new System.Drawing.Point(6, 33);
        tlpFileNameRow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
        tlpFileNameRow.Name = "tlpFileNameRow";
        tlpFileNameRow.RowCount = 1;
        tlpFileNameRow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpFileNameRow.Size = new System.Drawing.Size(402, 30);
        tlpFileNameRow.TabIndex = 1;
        // 
        // txtFileName
        // 
        txtFileName.Dock = System.Windows.Forms.DockStyle.Fill;
        txtFileName.Location = new System.Drawing.Point(123, 3);
        txtFileName.Name = "txtFileName";
        txtFileName.Size = new System.Drawing.Size(276, 23);
        txtFileName.TabIndex = 3;
        // 
        // lblFileName
        // 
        lblFileName.Dock = System.Windows.Forms.DockStyle.Fill;
        lblFileName.Location = new System.Drawing.Point(3, 0);
        lblFileName.Name = "lblFileName";
        lblFileName.Size = new System.Drawing.Size(114, 30);
        lblFileName.TabIndex = 0;
        lblFileName.Text = "File Name:";
        lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // tlpArgumentsRow
        // 
        tlpArgumentsRow.ColumnCount = 2;
        tlpArgumentsRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        tlpArgumentsRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tlpArgumentsRow.Controls.Add(txtArguments, 1, 0);
        tlpArgumentsRow.Controls.Add(lblArguments, 0, 0);
        tlpArgumentsRow.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpArgumentsRow.Location = new System.Drawing.Point(6, 63);
        tlpArgumentsRow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
        tlpArgumentsRow.Name = "tlpArgumentsRow";
        tlpArgumentsRow.RowCount = 1;
        tlpArgumentsRow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpArgumentsRow.Size = new System.Drawing.Size(402, 30);
        tlpArgumentsRow.TabIndex = 2;
        // 
        // txtArguments
        // 
        txtArguments.Dock = System.Windows.Forms.DockStyle.Fill;
        txtArguments.Location = new System.Drawing.Point(123, 3);
        txtArguments.Name = "txtArguments";
        txtArguments.Size = new System.Drawing.Size(276, 23);
        txtArguments.TabIndex = 4;
        // 
        // lblArguments
        // 
        lblArguments.Dock = System.Windows.Forms.DockStyle.Fill;
        lblArguments.Location = new System.Drawing.Point(3, 0);
        lblArguments.Name = "lblArguments";
        lblArguments.Size = new System.Drawing.Size(114, 30);
        lblArguments.TabIndex = 0;
        lblArguments.Text = "Arguments:";
        lblArguments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // tlpWorkingDirectoryRow
        // 
        tlpWorkingDirectoryRow.ColumnCount = 2;
        tlpWorkingDirectoryRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        tlpWorkingDirectoryRow.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tlpWorkingDirectoryRow.Controls.Add(txtWorkingDirectory, 1, 0);
        tlpWorkingDirectoryRow.Controls.Add(lblWorkingDirectory, 0, 0);
        tlpWorkingDirectoryRow.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpWorkingDirectoryRow.Location = new System.Drawing.Point(6, 93);
        tlpWorkingDirectoryRow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
        tlpWorkingDirectoryRow.Name = "tlpWorkingDirectoryRow";
        tlpWorkingDirectoryRow.RowCount = 1;
        tlpWorkingDirectoryRow.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpWorkingDirectoryRow.Size = new System.Drawing.Size(402, 30);
        tlpWorkingDirectoryRow.TabIndex = 3;
        // 
        // txtWorkingDirectory
        // 
        txtWorkingDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
        txtWorkingDirectory.Location = new System.Drawing.Point(123, 3);
        txtWorkingDirectory.Name = "txtWorkingDirectory";
        txtWorkingDirectory.Size = new System.Drawing.Size(276, 23);
        txtWorkingDirectory.TabIndex = 5;
        // 
        // lblWorkingDirectory
        // 
        lblWorkingDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
        lblWorkingDirectory.Location = new System.Drawing.Point(3, 0);
        lblWorkingDirectory.Name = "lblWorkingDirectory";
        lblWorkingDirectory.Size = new System.Drawing.Size(114, 30);
        lblWorkingDirectory.TabIndex = 0;
        lblWorkingDirectory.Text = "Working Directory:";
        lblWorkingDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 3;
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel1.Controls.Add(chkAutoRestart, 2, 0);
        tableLayoutPanel1.Controls.Add(btnIcon, 1, 0);
        tableLayoutPanel1.Controls.Add(lblSystemTrayIcon, 0, 0);
        tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel1.Location = new System.Drawing.Point(6, 153);
        tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanel1.Size = new System.Drawing.Size(402, 30);
        tableLayoutPanel1.TabIndex = 4;
        // 
        // chkAutoRestart
        // 
        chkAutoRestart.AutoSize = true;
        chkAutoRestart.Dock = System.Windows.Forms.DockStyle.Left;
        chkAutoRestart.Location = new System.Drawing.Point(264, 3);
        chkAutoRestart.Name = "chkAutoRestart";
        chkAutoRestart.Size = new System.Drawing.Size(91, 24);
        chkAutoRestart.TabIndex = 8;
        chkAutoRestart.Text = "Auto Restart";
        chkAutoRestart.UseVisualStyleBackColor = true;
        // 
        // btnIcon
        // 
        btnIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
        btnIcon.Location = new System.Drawing.Point(123, 3);
        btnIcon.Name = "btnIcon";
        btnIcon.Size = new System.Drawing.Size(24, 24);
        btnIcon.TabIndex = 7;
        btnIcon.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        btnIcon.UseVisualStyleBackColor = true;
        btnIcon.Click += btnIcon_Click;
        btnIcon.KeyUp += btnIcon_KeyUp;
        // 
        // lblSystemTrayIcon
        // 
        lblSystemTrayIcon.Dock = System.Windows.Forms.DockStyle.Fill;
        lblSystemTrayIcon.Location = new System.Drawing.Point(3, 0);
        lblSystemTrayIcon.Name = "lblSystemTrayIcon";
        lblSystemTrayIcon.Size = new System.Drawing.Size(114, 30);
        lblSystemTrayIcon.TabIndex = 0;
        lblSystemTrayIcon.Text = "System Tray Icon:";
        lblSystemTrayIcon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.ColumnCount = 3;
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel2.Controls.Add(btnCancel, 2, 0);
        tableLayoutPanel2.Controls.Add(btnOk, 1, 0);
        tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel2.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
        tableLayoutPanel2.Location = new System.Drawing.Point(6, 193);
        tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 1;
        tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel2.Size = new System.Drawing.Size(402, 57);
        tableLayoutPanel2.TabIndex = 5;
        // 
        // btnCancel
        // 
        btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        btnCancel.Location = new System.Drawing.Point(324, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new System.Drawing.Size(75, 25);
        btnCancel.TabIndex = 10;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // btnOk
        // 
        btnOk.Location = new System.Drawing.Point(243, 3);
        btnOk.Name = "btnOk";
        btnOk.Size = new System.Drawing.Size(75, 25);
        btnOk.TabIndex = 9;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += btnOk_Click;
        // 
        // tlpEncodingPanel
        // 
        tlpEncodingPanel.ColumnCount = 2;
        tlpEncodingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
        tlpEncodingPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpEncodingPanel.Controls.Add(lblOutputEncoding, 0, 0);
        tlpEncodingPanel.Controls.Add(cmbOutputEncoding, 1, 0);
        tlpEncodingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
        tlpEncodingPanel.Location = new System.Drawing.Point(6, 123);
        tlpEncodingPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
        tlpEncodingPanel.Name = "tlpEncodingPanel";
        tlpEncodingPanel.RowCount = 1;
        tlpEncodingPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tlpEncodingPanel.Size = new System.Drawing.Size(402, 30);
        tlpEncodingPanel.TabIndex = 6;
        // 
        // lblOutputEncoding
        // 
        lblOutputEncoding.AutoSize = true;
        lblOutputEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
        lblOutputEncoding.Location = new System.Drawing.Point(3, 0);
        lblOutputEncoding.Name = "lblOutputEncoding";
        lblOutputEncoding.Size = new System.Drawing.Size(114, 30);
        lblOutputEncoding.TabIndex = 0;
        lblOutputEncoding.Text = "Output Encoding:";
        lblOutputEncoding.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // cmbOutputEncoding
        // 
        cmbOutputEncoding.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
        cmbOutputEncoding.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
        cmbOutputEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
        cmbOutputEncoding.FormattingEnabled = true;
        cmbOutputEncoding.Location = new System.Drawing.Point(123, 3);
        cmbOutputEncoding.Name = "cmbOutputEncoding";
        cmbOutputEncoding.Size = new System.Drawing.Size(276, 23);
        cmbOutputEncoding.TabIndex = 6;
        // 
        // SettingsWindow
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(414, 230);
        Controls.Add(tlpSettingsWindowLayout);
        FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        Text = "Settings";
        tlpSettingsWindowLayout.ResumeLayout(false);
        tlpProfileRow.ResumeLayout(false);
        tlpProfileRow.PerformLayout();
        tlpFileNameRow.ResumeLayout(false);
        tlpFileNameRow.PerformLayout();
        tlpArgumentsRow.ResumeLayout(false);
        tlpArgumentsRow.PerformLayout();
        tlpWorkingDirectoryRow.ResumeLayout(false);
        tlpWorkingDirectoryRow.PerformLayout();
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        tableLayoutPanel2.ResumeLayout(false);
        tlpEncodingPanel.ResumeLayout(false);
        tlpEncodingPanel.PerformLayout();
        ResumeLayout(false);
    }

    private System.Windows.Forms.ComboBox cmbOutputEncoding;

    private System.Windows.Forms.Label lblOutputEncoding;

    private System.Windows.Forms.TableLayoutPanel tlpEncodingPanel;

    private System.Windows.Forms.TableLayoutPanel tlpFileNameRow;

    private System.Windows.Forms.TableLayoutPanel tlpSettingsWindowLayout;
    private System.Windows.Forms.TableLayoutPanel tlpProfileRow;

    private System.Windows.Forms.ComboBox cmbProfile;
    private System.Windows.Forms.Button btnDelete;

    private System.Windows.Forms.Label lblProfile;

    private System.Windows.Forms.Label lblFileName;

    #endregion

    private System.Windows.Forms.TextBox txtFileName;
    private System.Windows.Forms.TableLayoutPanel tlpArgumentsRow;
    private System.Windows.Forms.TextBox txtArguments;
    private System.Windows.Forms.Label lblArguments;
    private System.Windows.Forms.TableLayoutPanel tlpWorkingDirectoryRow;
    private System.Windows.Forms.TextBox txtWorkingDirectory;
    private System.Windows.Forms.Label lblWorkingDirectory;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.CheckBox chkAutoRestart;
    private System.Windows.Forms.Button btnIcon;
    private System.Windows.Forms.Label lblSystemTrayIcon;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOk;
}