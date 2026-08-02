namespace Pflegehaushaltsbuch.Forms
{
    partial class CreationUserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreationUserForm));
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.insertBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.changeBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.deleteBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.adminBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.nameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.phoneBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.faxBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.emailBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.loginBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            // insertBox
            // 
            resources.ApplyResources(this.insertBox, "insertBox");
            this.insertBox.BorderColor = System.Drawing.Color.DimGray;
            this.insertBox.CheckedImage = null;
            this.insertBox.Name = "insertBox";
            this.insertBox.UnCheckedImage = null;
            this.insertBox.UseVisualStyleBackColor = true;
            this.insertBox.CheckedChanged += new System.EventHandler(this.access_CheckedChanged);
            // 
            // changeBox
            // 
            resources.ApplyResources(this.changeBox, "changeBox");
            this.changeBox.BorderColor = System.Drawing.Color.DimGray;
            this.changeBox.CheckedImage = null;
            this.changeBox.Name = "changeBox";
            this.changeBox.UnCheckedImage = null;
            this.changeBox.UseVisualStyleBackColor = true;
            this.changeBox.CheckedChanged += new System.EventHandler(this.access_CheckedChanged);
            // 
            // deleteBox
            // 
            resources.ApplyResources(this.deleteBox, "deleteBox");
            this.deleteBox.BorderColor = System.Drawing.Color.DimGray;
            this.deleteBox.CheckedImage = null;
            this.deleteBox.Name = "deleteBox";
            this.deleteBox.UnCheckedImage = null;
            this.deleteBox.UseVisualStyleBackColor = true;
            this.deleteBox.CheckedChanged += new System.EventHandler(this.access_CheckedChanged);
            // 
            // adminBox
            // 
            resources.ApplyResources(this.adminBox, "adminBox");
            this.adminBox.BorderColor = System.Drawing.Color.DimGray;
            this.adminBox.CheckedImage = null;
            this.adminBox.Name = "adminBox";
            this.adminBox.UnCheckedImage = null;
            this.adminBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // nameBox
            // 
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nameBox, "nameBox");
            this.nameBox.Name = "nameBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // phoneBox
            // 
            this.phoneBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.phoneBox, "phoneBox");
            this.phoneBox.Name = "phoneBox";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // faxBox
            // 
            this.faxBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.faxBox, "faxBox");
            this.faxBox.Name = "faxBox";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // emailBox
            // 
            this.emailBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.emailBox, "emailBox");
            this.emailBox.Name = "emailBox";
            this.emailBox.Validating += new System.ComponentModel.CancelEventHandler(this.emailBox_Validating);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Controls.Add(this.nameBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.emailBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.phoneBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.faxBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.loginBox, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.cancelButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Border = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.Controls.Add(this.adminBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.insertBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.deleteBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.changeBox, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // loginBox
            // 
            this.loginBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.loginBox, "loginBox");
            this.loginBox.Name = "loginBox";
            // 
            // CreationUserForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CreationUserForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.Button okButton;
        private Pflegehaushaltsbuch.FormControls.Button cancelButton;
        private Pflegehaushaltsbuch.FormControls.CheckBox insertBox;
        private Pflegehaushaltsbuch.FormControls.CheckBox changeBox;
        private Pflegehaushaltsbuch.FormControls.CheckBox deleteBox;
        private Pflegehaushaltsbuch.FormControls.CheckBox adminBox;
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.TextBox nameBox;
        private Pflegehaushaltsbuch.FormControls.Label label3;
        private Pflegehaushaltsbuch.FormControls.TextBox phoneBox;
        private Pflegehaushaltsbuch.FormControls.Label label4;
        private Pflegehaushaltsbuch.FormControls.TextBox faxBox;
        private Pflegehaushaltsbuch.FormControls.Label label5;
        private Pflegehaushaltsbuch.FormControls.TextBox emailBox;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.FlowLayoutPanel flowLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.Label label1;
        private FormControls.TextBox loginBox;
    }
}