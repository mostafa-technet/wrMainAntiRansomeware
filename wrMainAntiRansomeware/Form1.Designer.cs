namespace wrMainAntiRansomeware
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fileFormatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executableExcludePolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToblacklistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToWhitelistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.securityLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAlertsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.backupVSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.restoreAllSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.activiateNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Webroam Antiransomeware";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFormatsToolStripMenuItem,
            this.executableExcludePolicyToolStripMenuItem,
            this.addToblacklistToolStripMenuItem,
            this.addToWhitelistToolStripMenuItem,
            this.reportToolStripMenuItem,
            this.securityLevelToolStripMenuItem,
            this.alertToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.toolStripMenuItem4,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.backupVSSToolStripMenuItem,
            this.toolStripMenuItem5,
            this.restoreAllSettingsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 330);
            // 
            // fileFormatsToolStripMenuItem
            // 
            this.fileFormatsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileFormatsToolStripMenuItem.Image = global::wrMainAntiRansomeware.Properties.Resources.Treetog_I_Documents;
            this.fileFormatsToolStripMenuItem.Name = "fileFormatsToolStripMenuItem";
            this.fileFormatsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.fileFormatsToolStripMenuItem.Text = "&File Formats Settings";
            this.fileFormatsToolStripMenuItem.Click += new System.EventHandler(this.fileFormatsToolStripMenuItem_Click);
            // 
            // executableExcludePolicyToolStripMenuItem
            // 
            this.executableExcludePolicyToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.executableExcludePolicyToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.executableExcludePolicyToolStripMenuItem.Name = "executableExcludePolicyToolStripMenuItem";
            this.executableExcludePolicyToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.executableExcludePolicyToolStripMenuItem.Text = "Executable Exclude Po&licy";
            this.executableExcludePolicyToolStripMenuItem.Click += new System.EventHandler(this.executableExcludePolicyToolStripMenuItem_Click);
            // 
            // addToblacklistToolStripMenuItem
            // 
            this.addToblacklistToolStripMenuItem.Name = "addToblacklistToolStripMenuItem";
            this.addToblacklistToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addToblacklistToolStripMenuItem.Text = "Edit &Blacklist Signs";
            this.addToblacklistToolStripMenuItem.Visible = false;
            this.addToblacklistToolStripMenuItem.Click += new System.EventHandler(this.addToblacklistToolStripMenuItem_Click);
            // 
            // addToWhitelistToolStripMenuItem
            // 
            this.addToWhitelistToolStripMenuItem.Name = "addToWhitelistToolStripMenuItem";
            this.addToWhitelistToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.addToWhitelistToolStripMenuItem.Text = "Edit &Whitelist Signs";
            this.addToWhitelistToolStripMenuItem.Visible = false;
            this.addToWhitelistToolStripMenuItem.Click += new System.EventHandler(this.addToWhitelistToolStripMenuItem_Click);
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.reportToolStripMenuItem.Text = "&Report";
            this.reportToolStripMenuItem.Click += new System.EventHandler(this.reportToolStripMenuItem_Click);
            // 
            // securityLevelToolStripMenuItem
            // 
            this.securityLevelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.highToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.disableToolStripMenuItem});
            this.securityLevelToolStripMenuItem.Name = "securityLevelToolStripMenuItem";
            this.securityLevelToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.securityLevelToolStripMenuItem.Text = "&Security Level";
            // 
            // highToolStripMenuItem
            // 
            this.highToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.highToolStripMenuItem.Name = "highToolStripMenuItem";
            this.highToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.highToolStripMenuItem.Text = "&High";
            this.highToolStripMenuItem.Click += new System.EventHandler(this.highToolStripMenuItem_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.mediumToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.mediumToolStripMenuItem.Text = "&Medium";
            this.mediumToolStripMenuItem.Click += new System.EventHandler(this.mediumToolStripMenuItem_Click);
            // 
            // disableToolStripMenuItem
            // 
            this.disableToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
            this.disableToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.disableToolStripMenuItem.Text = "&Disable";
            this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
            // 
            // alertToolStripMenuItem
            // 
            this.alertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.disableAlertsToolStripMenuItem});
            this.alertToolStripMenuItem.Name = "alertToolStripMenuItem";
            this.alertToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.alertToolStripMenuItem.Text = "&Alert";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.configureToolStripMenuItem.Text = "&Settings";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // disableAlertsToolStripMenuItem
            // 
            this.disableAlertsToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.disableAlertsToolStripMenuItem.Checked = true;
            this.disableAlertsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.disableAlertsToolStripMenuItem.Name = "disableAlertsToolStripMenuItem";
            this.disableAlertsToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.disableAlertsToolStripMenuItem.Text = "Disable Alerts";
            this.disableAlertsToolStripMenuItem.Click += new System.EventHandler(this.disableAlertsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(207, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(210, 22);
            this.toolStripMenuItem1.Text = "Self &Protection";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Checked = true;
            this.toolStripMenuItem4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(210, 22);
            this.toolStripMenuItem4.Text = "Shadow C&opy Protection";
            this.toolStripMenuItem4.Visible = false;
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Checked = true;
            this.toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(210, 22);
            this.toolStripMenuItem2.Text = "Protect from &Consoles";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(207, 6);
            // 
            // backupVSSToolStripMenuItem
            // 
            this.backupVSSToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.backupVSSToolStripMenuItem.Name = "backupVSSToolStripMenuItem";
            this.backupVSSToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.backupVSSToolStripMenuItem.Text = "Bac&kup (VSS)";
            // 
            // browseToolStripMenuItem
            // 
            this.browseToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.browseToolStripMenuItem.Text = "&Browse";
            this.browseToolStripMenuItem.Click += new System.EventHandler(this.browseToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "&Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(207, 6);
            // 
            // restoreAllSettingsToolStripMenuItem
            // 
            this.restoreAllSettingsToolStripMenuItem.Name = "restoreAllSettingsToolStripMenuItem";
            this.restoreAllSettingsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.restoreAllSettingsToolStripMenuItem.Text = "Resto&re All Settings";
            this.restoreAllSettingsToolStripMenuItem.Click += new System.EventHandler(this.restoreAllSettingsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(50, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "System is Secure!";
            this.label1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.label2_PreviewKeyDown);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.SeaShell;
            this.textBox1.Location = new System.Drawing.Point(13, 67);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(267, 242);
            this.textBox1.TabIndex = 2;
            this.textBox1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.label2_PreviewKeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Protected writing Formats:";
            this.label2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.label2_PreviewKeyDown);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(65, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "&Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.label2_PreviewKeyDown);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(160, 329);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.label2_PreviewKeyDown);
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 18000000;
            timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.activiateNowToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(149, 26);
            // 
            // activiateNowToolStripMenuItem
            // 
            this.activiateNowToolStripMenuItem.Name = "activiateNowToolStripMenuItem";
            this.activiateNowToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.activiateNowToolStripMenuItem.Text = "&Activiate Now";
            this.activiateNowToolStripMenuItem.Click += new System.EventHandler(this.activiateNowToolStripMenuItem_Click);
            // 
            // timer2
            // 
            this.timer2.Interval = 300000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 3000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(292, 364);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Webroam Antiransomeware";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.label2_PreviewKeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToblacklistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToWhitelistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem securityLevelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableAlertsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileFormatsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executableExcludePolicyToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem backupVSSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem activiateNowToolStripMenuItem;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem restoreAllSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        public static System.Windows.Forms.Timer timer1;
    }
}

