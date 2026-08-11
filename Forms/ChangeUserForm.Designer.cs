namespace Pflegehaushaltsbuch.Forms
{
    partial class ChangeUserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeUserForm));
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.pw0Box = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.pw1Box = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.usernameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.loginBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            this.okButton.Name = "okButton";
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cancelButton.BorderColor = System.Drawing.Color.DimGray;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Radius = -1F;
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // pw0Box
            // 
            this.pw0Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pw0Box, "pw0Box");
            this.pw0Box.Name = "pw0Box";
            this.pw0Box.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // pw1Box
            // 
            this.pw1Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pw1Box, "pw1Box");
            this.pw1Box.Name = "pw1Box";
            this.pw1Box.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // usernameBox
            // 
            this.usernameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.usernameBox, "usernameBox");
            this.usernameBox.Name = "usernameBox";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cancelButton, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.pw1Box, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.okButton, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.usernameBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.pw0Box, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.loginBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // loginBox
            // 
            this.loginBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.loginBox, "loginBox");
            this.loginBox.Name = "loginBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // ChangeUserForm
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChangeUserForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.Button okButton;
        private FormControls.Button cancelButton;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private Pflegehaushaltsbuch.FormControls.TextBox pw0Box;
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.TextBox pw1Box;
        private Pflegehaushaltsbuch.FormControls.Label label3;
        private Pflegehaushaltsbuch.FormControls.TextBox usernameBox;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.TextBox loginBox;
        private FormControls.Label label4;
    }
}