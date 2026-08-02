namespace Pflegehaushaltsbuch.Forms
{
    partial class UserLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLoginForm));
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.userNameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.passwordBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.connectButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.resetUserButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.closeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.userNameBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.passwordBox, 1, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // userNameBox
            // 
            this.userNameBox.AcceptsReturn = true;
            resources.ApplyResources(this.userNameBox, "userNameBox");
            this.userNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userNameBox.Name = "userNameBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // passwordBox
            // 
            this.passwordBox.AcceptsReturn = true;
            resources.ApplyResources(this.passwordBox, "passwordBox");
            this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.UseSystemPasswordChar = true;
            this.passwordBox.Enter += new System.EventHandler(this.passwordBox_Enter);
            // 
            // connectButton
            // 
            resources.ApplyResources(this.connectButton, "connectButton");
            this.connectButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.connectButton.BorderColor = System.Drawing.Color.DimGray;
            this.connectButton.Name = "connectButton";
            this.connectButton.Radius = -1F;
            this.connectButton.UseVisualStyleBackColor = false;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // resetUserButton
            // 
            resources.ApplyResources(this.resetUserButton, "resetUserButton");
            this.resetUserButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.resetUserButton.BorderColor = System.Drawing.Color.DimGray;
            this.resetUserButton.Name = "resetUserButton";
            this.resetUserButton.Radius = -1F;
            this.resetUserButton.UseVisualStyleBackColor = false;
            this.resetUserButton.Click += new System.EventHandler(this.resetUserButton_Click);
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.BorderColor = System.Drawing.Color.DimGray;
            this.closeButton.Name = "closeButton";
            this.closeButton.Radius = -1F;
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.closeButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.resetUserButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.connectButton, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // UserLoginForm
            // 
            this.AcceptButton = this.connectButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserLoginForm";
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.TextBox passwordBox;
        private FormControls.Label label3;
        private FormControls.TextBox userNameBox;
        private FormControls.Label label2;
        private FormControls.Button connectButton;
        private FormControls.Button resetUserButton;
        private FormControls.Button closeButton;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
    }
}