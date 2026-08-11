namespace Pflegehaushaltsbuch.Forms
{
    partial class AdministrationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrationForm));
            this.backupFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openBackupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel4 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.databaseBackupButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.restoreButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.connectDatabaseButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.disconnectDatabaseButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.userRightsButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.companyButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.layoutButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.exitButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.designButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.dataExchangeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // backupFileDialog
            // 
            this.backupFileDialog.DefaultExt = "xml";
            this.backupFileDialog.DereferenceLinks = false;
            this.backupFileDialog.FileName = "Pflegehaushaltsbuch";
            resources.ApplyResources(this.backupFileDialog, "backupFileDialog");
            // 
            // openBackupFileDialog
            // 
            this.openBackupFileDialog.FileName = "Pflegehaushaltsbuch.xml";
            resources.ApplyResources(this.openBackupFileDialog, "openBackupFileDialog");
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel4, 4);
            this.tableLayoutPanel4.Controls.Add(this.databaseBackupButton, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.restoreButton, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.connectDatabaseButton, 3, 1);
            this.tableLayoutPanel4.Controls.Add(this.disconnectDatabaseButton, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.userRightsButton, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.companyButton, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.layoutButton, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.dataExchangeButton, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.designButton, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.exitButton, 1, 2);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // databaseBackupButton
            // 
            resources.ApplyResources(this.databaseBackupButton, "databaseBackupButton");
            this.databaseBackupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.databaseBackupButton.BorderColor = System.Drawing.Color.DimGray;
            this.databaseBackupButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.databaseBackupButton.Name = "databaseBackupButton";
            this.databaseBackupButton.Radius = -1F;
            this.databaseBackupButton.UseVisualStyleBackColor = false;
            this.databaseBackupButton.Click += new System.EventHandler(this.databaseBackupButton_Click);
            // 
            // restoreButton
            // 
            resources.ApplyResources(this.restoreButton, "restoreButton");
            this.restoreButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.restoreButton.BorderColor = System.Drawing.Color.DimGray;
            this.restoreButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.restoreButton.Name = "restoreButton";
            this.restoreButton.Radius = -1F;
            this.restoreButton.UseVisualStyleBackColor = false;
            this.restoreButton.Click += new System.EventHandler(this.restoreButton_Click);
            // 
            // connectDatabaseButton
            // 
            resources.ApplyResources(this.connectDatabaseButton, "connectDatabaseButton");
            this.connectDatabaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.connectDatabaseButton.BorderColor = System.Drawing.Color.DimGray;
            this.connectDatabaseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.connectDatabaseButton.Name = "connectDatabaseButton";
            this.connectDatabaseButton.Radius = -1F;
            this.connectDatabaseButton.UseVisualStyleBackColor = false;
            this.connectDatabaseButton.Click += new System.EventHandler(this.dbConnectButton_Click);
            // 
            // disconnectDatabaseButton
            // 
            resources.ApplyResources(this.disconnectDatabaseButton, "disconnectDatabaseButton");
            this.disconnectDatabaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.disconnectDatabaseButton.BorderColor = System.Drawing.Color.DimGray;
            this.disconnectDatabaseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.disconnectDatabaseButton.Name = "disconnectDatabaseButton";
            this.disconnectDatabaseButton.Radius = -1F;
            this.disconnectDatabaseButton.UseVisualStyleBackColor = false;
            this.disconnectDatabaseButton.Click += new System.EventHandler(this.resetDatabase_Click);
            // 
            // userRightsButton
            // 
            resources.ApplyResources(this.userRightsButton, "userRightsButton");
            this.userRightsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.userRightsButton.BorderColor = System.Drawing.Color.DimGray;
            this.userRightsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.userRightsButton.Name = "userRightsButton";
            this.userRightsButton.Radius = -1F;
            this.userRightsButton.UseVisualStyleBackColor = false;
            this.userRightsButton.Click += new System.EventHandler(this.userRightsButton_Click);
            // 
            // companyButton
            // 
            resources.ApplyResources(this.companyButton, "companyButton");
            this.companyButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.companyButton.BorderColor = System.Drawing.Color.DimGray;
            this.companyButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.companyButton.Name = "companyButton";
            this.companyButton.Radius = -1F;
            this.companyButton.UseVisualStyleBackColor = false;
            this.companyButton.Click += new System.EventHandler(this.companyButton_Click);
            // 
            // layoutButton
            // 
            resources.ApplyResources(this.layoutButton, "layoutButton");
            this.layoutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.layoutButton.BorderColor = System.Drawing.Color.DimGray;
            this.layoutButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.layoutButton.Name = "layoutButton";
            this.layoutButton.Radius = -1F;
            this.layoutButton.UseVisualStyleBackColor = false;
            this.layoutButton.Click += new System.EventHandler(this.layoutButton_Click);
            // 
            // exitButton
            // 
            resources.ApplyResources(this.exitButton, "exitButton");
            this.exitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exitButton.BorderColor = System.Drawing.Color.DimGray;
            this.exitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exitButton.Name = "exitButton";
            this.exitButton.Radius = -1F;
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click_1);
            // 
            // designButton
            // 
            resources.ApplyResources(this.designButton, "designButton");
            this.designButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.designButton.BorderColor = System.Drawing.Color.DimGray;
            this.designButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.designButton.Name = "designButton";
            this.designButton.Radius = -1F;
            this.designButton.UseVisualStyleBackColor = false;
            this.designButton.Click += new System.EventHandler(this.designButton_Click);
            // 
            // dataExchangeButton
            // 
            resources.ApplyResources(this.dataExchangeButton, "dataExchangeButton");
            this.dataExchangeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dataExchangeButton.BorderColor = System.Drawing.Color.DimGray;
            this.dataExchangeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dataExchangeButton.Name = "dataExchangeButton";
            this.dataExchangeButton.Radius = -1F;
            this.dataExchangeButton.UseVisualStyleBackColor = false;
            this.dataExchangeButton.Click += new System.EventHandler(this.dataExchangeButton_Click);
            // 
            // AdministrationForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "AdministrationForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog backupFileDialog;
        private System.Windows.Forms.OpenFileDialog openBackupFileDialog;
        private FormControls.Button userRightsButton;
        private FormControls.Button layoutButton;
        private FormControls.Button companyButton;
        private FormControls.Button databaseBackupButton;
        private FormControls.Button restoreButton;
        private FormControls.Button exitButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.Button disconnectDatabaseButton;
        private System.Windows.Forms.ToolTip toolTip;
        private FormControls.Button connectDatabaseButton;
        private FormControls.TableLayoutPanel tableLayoutPanel4;
        private FormControls.Button designButton;
        private FormControls.Button dataExchangeButton;
    }
}