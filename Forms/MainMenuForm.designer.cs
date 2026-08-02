namespace Pflegehaushaltsbuch.Forms
{
    partial class MainMenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            this.backupFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openBackupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.adminPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.userRightsButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.pictureBox6 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.statisticsPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.accountHoldingsButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.pictureBox1 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.cashCheckPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.cashOfficeControlbutton = new Pflegehaushaltsbuch.FormControls.Button();
            this.pictureBox8 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.recordPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.recordButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.pictureBox14 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.assistantsPAnel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.pictureBox4 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.creditButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.advisorPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.advisorButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.pictureBox5 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.clientsPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.pictureBox3 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.clientManagementButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.OfficeCashPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.pictureBox9 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.officeCashButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.bankingPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.bankingButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.pictureBox7 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.cashPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.pictureBox2 = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.cashButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.adminPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.statisticsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.cashCheckPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.recordPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            this.assistantsPAnel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.advisorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.clientsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.OfficeCashPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.bankingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            this.cashPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
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
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.adminPanel, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.statisticsPanel, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.cashCheckPanel, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.recordPanel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.assistantsPAnel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.advisorPanel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.clientsPanel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.OfficeCashPanel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.bankingPanel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cashPanel, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // adminPanel
            // 
            resources.ApplyResources(this.adminPanel, "adminPanel");
            this.adminPanel.Controls.Add(this.userRightsButton, 0, 1);
            this.adminPanel.Controls.Add(this.pictureBox6, 0, 0);
            this.adminPanel.Name = "adminPanel";
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
            // pictureBox6
            // 
            resources.ApplyResources(this.pictureBox6, "pictureBox6");
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.BackgroundImage = global::Pflegehaushaltsbuch.Properties.Resources.settings;
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.userRightsButton_Click);
            // 
            // statisticsPanel
            // 
            resources.ApplyResources(this.statisticsPanel, "statisticsPanel");
            this.statisticsPanel.Controls.Add(this.accountHoldingsButton, 0, 1);
            this.statisticsPanel.Controls.Add(this.pictureBox1, 0, 0);
            this.statisticsPanel.Name = "statisticsPanel";
            // 
            // accountHoldingsButton
            // 
            resources.ApplyResources(this.accountHoldingsButton, "accountHoldingsButton");
            this.accountHoldingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.accountHoldingsButton.BorderColor = System.Drawing.Color.DimGray;
            this.accountHoldingsButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.accountHoldingsButton.Name = "accountHoldingsButton";
            this.accountHoldingsButton.Radius = -1F;
            this.accountHoldingsButton.UseVisualStyleBackColor = false;
            this.accountHoldingsButton.Click += new System.EventHandler(this.accountHoldingsButton_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.accountHoldingsButton_Click);
            // 
            // cashCheckPanel
            // 
            resources.ApplyResources(this.cashCheckPanel, "cashCheckPanel");
            this.cashCheckPanel.Controls.Add(this.cashOfficeControlbutton, 0, 1);
            this.cashCheckPanel.Controls.Add(this.pictureBox8, 0, 0);
            this.cashCheckPanel.Name = "cashCheckPanel";
            // 
            // cashOfficeControlbutton
            // 
            resources.ApplyResources(this.cashOfficeControlbutton, "cashOfficeControlbutton");
            this.cashOfficeControlbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cashOfficeControlbutton.BorderColor = System.Drawing.Color.DimGray;
            this.cashOfficeControlbutton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cashOfficeControlbutton.Name = "cashOfficeControlbutton";
            this.cashOfficeControlbutton.Radius = -1F;
            this.cashOfficeControlbutton.UseVisualStyleBackColor = false;
            this.cashOfficeControlbutton.Click += new System.EventHandler(this.cashOfficeControlbutton_Click);
            // 
            // pictureBox8
            // 
            resources.ApplyResources(this.pictureBox8, "pictureBox8");
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.TabStop = false;
            this.pictureBox8.Click += new System.EventHandler(this.cashOfficeControlbutton_Click);
            // 
            // recordPanel
            // 
            resources.ApplyResources(this.recordPanel, "recordPanel");
            this.recordPanel.Controls.Add(this.recordButton, 0, 1);
            this.recordPanel.Controls.Add(this.pictureBox14, 0, 0);
            this.recordPanel.Name = "recordPanel";
            // 
            // recordButton
            // 
            resources.ApplyResources(this.recordButton, "recordButton");
            this.recordButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.recordButton.BorderColor = System.Drawing.Color.DimGray;
            this.recordButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.recordButton.Name = "recordButton";
            this.recordButton.Radius = -1F;
            this.recordButton.UseVisualStyleBackColor = false;
            this.recordButton.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // pictureBox14
            // 
            resources.ApplyResources(this.pictureBox14, "pictureBox14");
            this.pictureBox14.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.TabStop = false;
            this.pictureBox14.Click += new System.EventHandler(this.recordButton_Click);
            // 
            // assistantsPAnel
            // 
            resources.ApplyResources(this.assistantsPAnel, "assistantsPAnel");
            this.assistantsPAnel.Controls.Add(this.pictureBox4, 0, 0);
            this.assistantsPAnel.Controls.Add(this.creditButton, 0, 1);
            this.assistantsPAnel.Name = "assistantsPAnel";
            // 
            // pictureBox4
            // 
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.creditButton_Click);
            // 
            // creditButton
            // 
            resources.ApplyResources(this.creditButton, "creditButton");
            this.creditButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.creditButton.BorderColor = System.Drawing.Color.DimGray;
            this.creditButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.creditButton.Name = "creditButton";
            this.creditButton.Radius = -1F;
            this.creditButton.UseVisualStyleBackColor = false;
            this.creditButton.Click += new System.EventHandler(this.creditButton_Click);
            // 
            // advisorPanel
            // 
            resources.ApplyResources(this.advisorPanel, "advisorPanel");
            this.advisorPanel.Controls.Add(this.advisorButton, 0, 1);
            this.advisorPanel.Controls.Add(this.pictureBox5, 0, 0);
            this.advisorPanel.Name = "advisorPanel";
            // 
            // advisorButton
            // 
            resources.ApplyResources(this.advisorButton, "advisorButton");
            this.advisorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.advisorButton.BorderColor = System.Drawing.Color.DimGray;
            this.advisorButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.advisorButton.Name = "advisorButton";
            this.advisorButton.Radius = -1F;
            this.advisorButton.UseVisualStyleBackColor = false;
            this.advisorButton.Click += new System.EventHandler(this.advisorButton_Click);
            // 
            // pictureBox5
            // 
            resources.ApplyResources(this.pictureBox5, "pictureBox5");
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.advisorButton_Click);
            // 
            // clientsPanel
            // 
            resources.ApplyResources(this.clientsPanel, "clientsPanel");
            this.clientsPanel.Controls.Add(this.pictureBox3, 0, 0);
            this.clientsPanel.Controls.Add(this.clientManagementButton, 0, 1);
            this.clientsPanel.Name = "clientsPanel";
            // 
            // pictureBox3
            // 
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.clientManagementButton_Click);
            // 
            // clientManagementButton
            // 
            resources.ApplyResources(this.clientManagementButton, "clientManagementButton");
            this.clientManagementButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clientManagementButton.BorderColor = System.Drawing.Color.DimGray;
            this.clientManagementButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.clientManagementButton.Name = "clientManagementButton";
            this.clientManagementButton.Radius = -1F;
            this.clientManagementButton.UseVisualStyleBackColor = false;
            this.clientManagementButton.Click += new System.EventHandler(this.clientManagementButton_Click);
            // 
            // OfficeCashPanel
            // 
            resources.ApplyResources(this.OfficeCashPanel, "OfficeCashPanel");
            this.OfficeCashPanel.Controls.Add(this.pictureBox9, 0, 0);
            this.OfficeCashPanel.Controls.Add(this.officeCashButton, 0, 1);
            this.OfficeCashPanel.Name = "OfficeCashPanel";
            // 
            // pictureBox9
            // 
            resources.ApplyResources(this.pictureBox9, "pictureBox9");
            this.pictureBox9.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.TabStop = false;
            this.pictureBox9.Click += new System.EventHandler(this.officeCashButton_Click);
            // 
            // officeCashButton
            // 
            resources.ApplyResources(this.officeCashButton, "officeCashButton");
            this.officeCashButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.officeCashButton.BorderColor = System.Drawing.Color.DimGray;
            this.officeCashButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.officeCashButton.Name = "officeCashButton";
            this.officeCashButton.Radius = -1F;
            this.officeCashButton.UseVisualStyleBackColor = false;
            this.officeCashButton.Click += new System.EventHandler(this.officeCashButton_Click);
            // 
            // bankingPanel
            // 
            resources.ApplyResources(this.bankingPanel, "bankingPanel");
            this.bankingPanel.Controls.Add(this.bankingButton, 0, 1);
            this.bankingPanel.Controls.Add(this.pictureBox7, 0, 0);
            this.bankingPanel.Name = "bankingPanel";
            // 
            // bankingButton
            // 
            resources.ApplyResources(this.bankingButton, "bankingButton");
            this.bankingButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.bankingButton.BorderColor = System.Drawing.Color.DimGray;
            this.bankingButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.bankingButton.Name = "bankingButton";
            this.bankingButton.Radius = -1F;
            this.bankingButton.UseVisualStyleBackColor = false;
            this.bankingButton.Click += new System.EventHandler(this.bankingButton_Click);
            // 
            // pictureBox7
            // 
            resources.ApplyResources(this.pictureBox7, "pictureBox7");
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Click += new System.EventHandler(this.bankingButton_Click);
            // 
            // cashPanel
            // 
            resources.ApplyResources(this.cashPanel, "cashPanel");
            this.cashPanel.Controls.Add(this.pictureBox2, 0, 0);
            this.cashPanel.Controls.Add(this.cashButton, 0, 1);
            this.cashPanel.Name = "cashPanel";
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.cashButton_Click);
            // 
            // cashButton
            // 
            resources.ApplyResources(this.cashButton, "cashButton");
            this.cashButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cashButton.BorderColor = System.Drawing.Color.DimGray;
            this.cashButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cashButton.Name = "cashButton";
            this.cashButton.Radius = -1F;
            this.cashButton.UseVisualStyleBackColor = false;
            this.cashButton.Click += new System.EventHandler(this.cashButton_Click);
            // 
            // MainMenuForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainMenuForm";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.adminPanel.ResumeLayout(false);
            this.adminPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.statisticsPanel.ResumeLayout(false);
            this.statisticsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.cashCheckPanel.ResumeLayout(false);
            this.cashCheckPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.recordPanel.ResumeLayout(false);
            this.recordPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            this.assistantsPAnel.ResumeLayout(false);
            this.assistantsPAnel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.advisorPanel.ResumeLayout(false);
            this.advisorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.clientsPanel.ResumeLayout(false);
            this.clientsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.OfficeCashPanel.ResumeLayout(false);
            this.OfficeCashPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.bankingPanel.ResumeLayout(false);
            this.bankingPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            this.cashPanel.ResumeLayout(false);
            this.cashPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog backupFileDialog;
        private System.Windows.Forms.OpenFileDialog openBackupFileDialog;
        private FormControls.TableLayoutPanel cashPanel;
        private FormControls.PictureBox pictureBox2;
        private FormControls.Button cashButton;
        private FormControls.TableLayoutPanel bankingPanel;
        private FormControls.Button bankingButton;
        private FormControls.PictureBox pictureBox7;
        private FormControls.TableLayoutPanel clientsPanel;
        private FormControls.PictureBox pictureBox3;
        private FormControls.Button clientManagementButton;
        private FormControls.TableLayoutPanel advisorPanel;
        private FormControls.Button advisorButton;
        private FormControls.PictureBox pictureBox5;
        private FormControls.TableLayoutPanel assistantsPAnel;
        private FormControls.PictureBox pictureBox4;
        private FormControls.Button creditButton;
        private FormControls.TableLayoutPanel recordPanel;
        private FormControls.Button recordButton;
        private FormControls.PictureBox pictureBox14;
        private FormControls.TableLayoutPanel cashCheckPanel;
        private FormControls.Button cashOfficeControlbutton;
        private FormControls.PictureBox pictureBox8;
        private FormControls.TableLayoutPanel statisticsPanel;
        private FormControls.Button accountHoldingsButton;
        private FormControls.PictureBox pictureBox1;
        private FormControls.TableLayoutPanel adminPanel;
        private FormControls.Button userRightsButton;
        private FormControls.PictureBox pictureBox6;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.TableLayoutPanel OfficeCashPanel;
        private FormControls.PictureBox pictureBox9;
        private FormControls.Button officeCashButton;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
    }
}