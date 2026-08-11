namespace Pflegehaushaltsbuch.Forms
{
    partial class DatabaseServerConnectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseServerConnectForm));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.databaseTypePanel = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.sqlButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.mySqlButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.sqliteButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.userNameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.passwordBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.connectButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.closeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.hostLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.hostBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.databaseTypePanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.databaseTypePanel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.userNameBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.passwordBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.hostLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.hostBox, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // databaseTypePanel
            // 
            resources.ApplyResources(this.databaseTypePanel, "databaseTypePanel");
            this.databaseTypePanel.Controls.Add(this.sqlButton);
            this.databaseTypePanel.Controls.Add(this.mySqlButton);
            this.databaseTypePanel.Controls.Add(this.sqliteButton);
            this.databaseTypePanel.Name = "databaseTypePanel";
            // 
            // sqlButton
            // 
            resources.ApplyResources(this.sqlButton, "sqlButton");
            this.sqlButton.BackColor = System.Drawing.Color.Transparent;
            this.sqlButton.BorderColor = System.Drawing.Color.Black;
            this.sqlButton.CheckedState = true;
            this.sqlButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sqlButton.Name = "sqlButton";
            this.sqlButton.Tag = Pflegehaushaltsbuch.XmlConfig.DataBaseTypes.SQL;
            this.sqlButton.UseVisualStyleBackColor = true;
            this.sqlButton.Click += new System.EventHandler(this.databaseTypeButton_Click);
            // 
            // mySqlButton
            // 
            resources.ApplyResources(this.mySqlButton, "mySqlButton");
            this.mySqlButton.BackColor = System.Drawing.Color.Transparent;
            this.mySqlButton.BorderColor = System.Drawing.Color.Black;
            this.mySqlButton.CheckedState = true;
            this.mySqlButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.mySqlButton.Name = "mySqlButton";
            this.mySqlButton.Tag = Pflegehaushaltsbuch.XmlConfig.DataBaseTypes.MySQL;
            this.mySqlButton.UseVisualStyleBackColor = true;
            this.mySqlButton.Click += new System.EventHandler(this.databaseTypeButton_Click);
            // 
            // sqliteButton
            // 
            resources.ApplyResources(this.sqliteButton, "sqliteButton");
            this.sqliteButton.BackColor = System.Drawing.Color.Transparent;
            this.sqliteButton.BorderColor = System.Drawing.Color.Black;
            this.sqliteButton.CheckedState = true;
            this.sqliteButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sqliteButton.Name = "sqliteButton";
            this.sqliteButton.Tag = Pflegehaushaltsbuch.XmlConfig.DataBaseTypes.SQLite;
            this.sqliteButton.UseVisualStyleBackColor = true;
            this.sqliteButton.Click += new System.EventHandler(this.databaseTypeButton_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // userNameBox
            // 
            this.userNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.userNameBox, "userNameBox");
            this.userNameBox.Name = "userNameBox";
            // 
            // passwordBox
            // 
            this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.passwordBox, "passwordBox");
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.connectButton);
            this.flowLayoutPanel1.Controls.Add(this.closeButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.BackColor = System.Drawing.Color.Transparent;
            this.connectButton.BorderColor = System.Drawing.Color.Black;
            this.connectButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.connectButton.Name = "connectButton";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BorderColor = System.Drawing.Color.Black;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // hostLabel
            // 
            resources.ApplyResources(this.hostLabel, "hostLabel");
            this.hostLabel.BackColor = System.Drawing.Color.Transparent;
            this.hostLabel.ForeColor = System.Drawing.Color.White;
            this.hostLabel.Name = "hostLabel";
            // 
            // hostBox
            // 
            this.hostBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.hostBox, "hostBox");
            this.hostBox.Name = "hostBox";
            // 
            // DatabaseServerConnectForm
            // 
            this.AcceptButton = this.connectButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.closeButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DatabaseServerConnectForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.databaseTypePanel.ResumeLayout(false);
            this.databaseTypePanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel databaseTypePanel;
        private FormControls.Button sqlButton;
        private FormControls.Button mySqlButton;
        private FormControls.Button sqliteButton;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button connectButton;
        private FormControls.Label hostLabel;
        private FormControls.TextBox hostBox;
        private FormControls.Button closeButton;
        private FormControls.Label label3;
        private FormControls.Label label2;
        private FormControls.TextBox userNameBox;
        private FormControls.TextBox passwordBox;
    }
}




