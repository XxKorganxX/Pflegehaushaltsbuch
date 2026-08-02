namespace Pflegehaushaltsbuch.Forms
{
    partial class DatabaseManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseManagerForm));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.masterkeyPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.masterKeywordBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.changeMasterkeyButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.masterKeywordIIBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label9 = new Pflegehaushaltsbuch.FormControls.Label();
            this.userPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.sqlUserButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.usernameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.fromHostBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.keywordBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.connectPanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.databasesBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.connectButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.createdatabasePanel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.databaseBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.createDataBaseButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label8 = new Pflegehaushaltsbuch.FormControls.Label();
            this.changeMasterkeywordLabel = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.masterkeyPanel.SuspendLayout();
            this.userPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.connectPanel.SuspendLayout();
            this.createdatabasePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.masterkeyPanel, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.userPanel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.connectPanel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.createdatabasePanel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.changeMasterkeywordLabel, 0, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // masterkeyPanel
            // 
            resources.ApplyResources(this.masterkeyPanel, "masterkeyPanel");
            this.masterkeyPanel.Controls.Add(this.masterKeywordBox, 1, 0);
            this.masterkeyPanel.Controls.Add(this.changeMasterkeyButton, 1, 2);
            this.masterkeyPanel.Controls.Add(this.masterKeywordIIBox, 1, 1);
            this.masterkeyPanel.Controls.Add(this.label9, 0, 0);
            this.masterkeyPanel.Name = "masterkeyPanel";
            // 
            // masterKeywordBox
            // 
            resources.ApplyResources(this.masterKeywordBox, "masterKeywordBox");
            this.masterKeywordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.masterKeywordBox.Name = "masterKeywordBox";
            this.masterKeywordBox.UseSystemPasswordChar = true;
            // 
            // changeMasterkeyButton
            // 
            resources.ApplyResources(this.changeMasterkeyButton, "changeMasterkeyButton");
            this.changeMasterkeyButton.BackColor = System.Drawing.Color.Transparent;
            this.changeMasterkeyButton.BorderColor = System.Drawing.Color.Black;
            this.changeMasterkeyButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.changeMasterkeyButton.Name = "changeMasterkeyButton";
            this.changeMasterkeyButton.UseVisualStyleBackColor = true;
            this.changeMasterkeyButton.Click += new System.EventHandler(this.changeMasterkeyButton_Click_1);
            // 
            // masterKeywordIIBox
            // 
            resources.ApplyResources(this.masterKeywordIIBox, "masterKeywordIIBox");
            this.masterKeywordIIBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.masterKeywordIIBox.Name = "masterKeywordIIBox";
            this.masterKeywordIIBox.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Name = "label9";
            // 
            // userPanel
            // 
            resources.ApplyResources(this.userPanel, "userPanel");
            this.userPanel.Controls.Add(this.label2, 0, 0);
            this.userPanel.Controls.Add(this.sqlUserButton, 1, 3);
            this.userPanel.Controls.Add(this.usernameBox, 1, 0);
            this.userPanel.Controls.Add(this.fromHostBox, 1, 2);
            this.userPanel.Controls.Add(this.label3, 0, 1);
            this.userPanel.Controls.Add(this.label4, 0, 2);
            this.userPanel.Controls.Add(this.keywordBox, 1, 1);
            this.userPanel.Name = "userPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // sqlUserButton
            // 
            resources.ApplyResources(this.sqlUserButton, "sqlUserButton");
            this.sqlUserButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sqlUserButton.BorderColor = System.Drawing.Color.DimGray;
            this.sqlUserButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sqlUserButton.Name = "sqlUserButton";
            this.sqlUserButton.Radius = -1F;
            this.sqlUserButton.UseVisualStyleBackColor = false;
            this.sqlUserButton.Click += new System.EventHandler(this.sqlUserButton_Click);
            // 
            // usernameBox
            // 
            resources.ApplyResources(this.usernameBox, "usernameBox");
            this.usernameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.usernameBox.Name = "usernameBox";
            // 
            // fromHostBox
            // 
            resources.ApplyResources(this.fromHostBox, "fromHostBox");
            this.fromHostBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fromHostBox.Name = "fromHostBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // keywordBox
            // 
            resources.ApplyResources(this.keywordBox, "keywordBox");
            this.keywordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.keywordBox.Name = "keywordBox";
            this.keywordBox.UseSystemPasswordChar = true;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.Transparent;
            this.cancelButton.BorderColor = System.Drawing.Color.Black;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // connectPanel
            // 
            resources.ApplyResources(this.connectPanel, "connectPanel");
            this.connectPanel.Controls.Add(this.label6, 0, 0);
            this.connectPanel.Controls.Add(this.databasesBox, 1, 0);
            this.connectPanel.Controls.Add(this.connectButton, 1, 1);
            this.connectPanel.Name = "connectPanel";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Name = "label6";
            // 
            // databasesBox
            // 
            resources.ApplyResources(this.databasesBox, "databasesBox");
            this.databasesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.databasesBox.FormattingEnabled = true;
            this.databasesBox.Name = "databasesBox";
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
            // createdatabasePanel
            // 
            resources.ApplyResources(this.createdatabasePanel, "createdatabasePanel");
            this.createdatabasePanel.Controls.Add(this.label1, 0, 0);
            this.createdatabasePanel.Controls.Add(this.databaseBox, 1, 0);
            this.createdatabasePanel.Controls.Add(this.createDataBaseButton, 1, 1);
            this.createdatabasePanel.Name = "createdatabasePanel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // databaseBox
            // 
            resources.ApplyResources(this.databaseBox, "databaseBox");
            this.databaseBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.databaseBox.Name = "databaseBox";
            // 
            // createDataBaseButton
            // 
            resources.ApplyResources(this.createDataBaseButton, "createDataBaseButton");
            this.createDataBaseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.createDataBaseButton.BorderColor = System.Drawing.Color.DimGray;
            this.createDataBaseButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.createDataBaseButton.Name = "createDataBaseButton";
            this.createDataBaseButton.Radius = -1F;
            this.createDataBaseButton.UseVisualStyleBackColor = false;
            this.createDataBaseButton.Click += new System.EventHandler(this.createDataBaseButton_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Gradiant = true;
            this.label5.Name = "label5";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Gradiant = true;
            this.label7.Name = "label7";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Gradiant = true;
            this.label8.Name = "label8";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // changeMasterkeywordLabel
            // 
            resources.ApplyResources(this.changeMasterkeywordLabel, "changeMasterkeywordLabel");
            this.changeMasterkeywordLabel.ForeColor = System.Drawing.Color.White;
            this.changeMasterkeywordLabel.Gradiant = true;
            this.changeMasterkeywordLabel.Name = "changeMasterkeywordLabel";
            this.changeMasterkeywordLabel.Click += new System.EventHandler(this.changeMasterkeywordLabel_Click);
            // 
            // DatabaseManagerForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DatabaseManagerForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.masterkeyPanel.ResumeLayout(false);
            this.masterkeyPanel.PerformLayout();
            this.userPanel.ResumeLayout(false);
            this.userPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.connectPanel.ResumeLayout(false);
            this.connectPanel.PerformLayout();
            this.createdatabasePanel.ResumeLayout(false);
            this.createdatabasePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button connectButton;
        private FormControls.Label label1;
        private FormControls.TextBox databaseBox;
        private FormControls.Button cancelButton;
        private FormControls.Button createDataBaseButton;
        private FormControls.Label label2;
        private FormControls.TextBox usernameBox;
        private FormControls.Button sqlUserButton;
        private FormControls.Label label3;
        private FormControls.TextBox keywordBox;
        private FormControls.TextBox fromHostBox;
        private FormControls.Label label4;
        private FormControls.ComboBox databasesBox;
        private FormControls.Label label6;
        private FormControls.TableLayoutPanel createdatabasePanel;
        private FormControls.TableLayoutPanel userPanel;
        private FormControls.Button changeMasterkeyButton;
        private FormControls.TableLayoutPanel connectPanel;
        private FormControls.Label label5;
        private FormControls.Label label7;
        private FormControls.Label label8;
        private FormControls.TableLayoutPanel masterkeyPanel;
        private FormControls.TextBox masterKeywordBox;
        private FormControls.TextBox masterKeywordIIBox;
        private FormControls.Label label9;
        private FormControls.Label changeMasterkeywordLabel;
    }
}