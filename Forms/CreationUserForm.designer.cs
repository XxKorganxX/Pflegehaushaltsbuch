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
            this.createCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.changeCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.adminCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.handsignBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.bookCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.cancelBookingCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.cashBalanceCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.bankBalanceCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.statisticsCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.cashAuditCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.documentsCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.employeesCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.representativesCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.clientsCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.pettyCashCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
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
            // createCheckBox
            // 
            resources.ApplyResources(this.createCheckBox, "createCheckBox");
            this.createCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.createCheckBox.Checked = true;
            this.createCheckBox.CheckedImage = null;
            this.createCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.createCheckBox.Name = "createCheckBox";
            this.createCheckBox.UnCheckedImage = null;
            this.createCheckBox.UseVisualStyleBackColor = true;
            // 
            // changeCheckBox
            // 
            resources.ApplyResources(this.changeCheckBox, "changeCheckBox");
            this.changeCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.changeCheckBox.Checked = true;
            this.changeCheckBox.CheckedImage = null;
            this.changeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.changeCheckBox.Name = "changeCheckBox";
            this.changeCheckBox.UnCheckedImage = null;
            this.changeCheckBox.UseVisualStyleBackColor = true;
            // 
            // adminCheckBox
            // 
            resources.ApplyResources(this.adminCheckBox, "adminCheckBox");
            this.adminCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.adminCheckBox.CheckedImage = null;
            this.adminCheckBox.Name = "adminCheckBox";
            this.adminCheckBox.UnCheckedImage = null;
            this.adminCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // handsignBox
            // 
            this.handsignBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.handsignBox, "handsignBox");
            this.handsignBox.Name = "handsignBox";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.handsignBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.loginBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
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
            this.tableLayoutPanel2.Controls.Add(this.createCheckBox, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.changeCheckBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.bookCheckBox, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cancelBookingCheckBox, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.adminCheckBox, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.cashBalanceCheckBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.bankBalanceCheckBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.statisticsCheckBox, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.cashAuditCheckBox, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.documentsCheckBox, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.employeesCheckBox, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.representativesCheckBox, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.clientsCheckBox, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.pettyCashCheckBox, 1, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // bookCheckBox
            // 
            resources.ApplyResources(this.bookCheckBox, "bookCheckBox");
            this.bookCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.bookCheckBox.Checked = true;
            this.bookCheckBox.CheckedImage = null;
            this.bookCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bookCheckBox.Name = "bookCheckBox";
            this.bookCheckBox.UnCheckedImage = null;
            this.bookCheckBox.UseVisualStyleBackColor = true;
            // 
            // cancelBookingCheckBox
            // 
            resources.ApplyResources(this.cancelBookingCheckBox, "cancelBookingCheckBox");
            this.cancelBookingCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.cancelBookingCheckBox.Checked = true;
            this.cancelBookingCheckBox.CheckedImage = null;
            this.cancelBookingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cancelBookingCheckBox.Name = "cancelBookingCheckBox";
            this.cancelBookingCheckBox.UnCheckedImage = null;
            this.cancelBookingCheckBox.UseVisualStyleBackColor = true;
            // 
            // cashBalanceCheckBox
            // 
            resources.ApplyResources(this.cashBalanceCheckBox, "cashBalanceCheckBox");
            this.cashBalanceCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.cashBalanceCheckBox.Checked = true;
            this.cashBalanceCheckBox.CheckedImage = null;
            this.cashBalanceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cashBalanceCheckBox.Name = "cashBalanceCheckBox";
            this.cashBalanceCheckBox.UnCheckedImage = null;
            this.cashBalanceCheckBox.UseVisualStyleBackColor = true;
            // 
            // bankBalanceCheckBox
            // 
            resources.ApplyResources(this.bankBalanceCheckBox, "bankBalanceCheckBox");
            this.bankBalanceCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.bankBalanceCheckBox.Checked = true;
            this.bankBalanceCheckBox.CheckedImage = null;
            this.bankBalanceCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bankBalanceCheckBox.Name = "bankBalanceCheckBox";
            this.bankBalanceCheckBox.UnCheckedImage = null;
            this.bankBalanceCheckBox.UseVisualStyleBackColor = true;
            // 
            // statisticsCheckBox
            // 
            resources.ApplyResources(this.statisticsCheckBox, "statisticsCheckBox");
            this.statisticsCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.statisticsCheckBox.Checked = true;
            this.statisticsCheckBox.CheckedImage = null;
            this.statisticsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statisticsCheckBox.Name = "statisticsCheckBox";
            this.statisticsCheckBox.UnCheckedImage = null;
            this.statisticsCheckBox.UseVisualStyleBackColor = true;
            // 
            // cashAuditCheckBox
            // 
            resources.ApplyResources(this.cashAuditCheckBox, "cashAuditCheckBox");
            this.cashAuditCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.cashAuditCheckBox.Checked = true;
            this.cashAuditCheckBox.CheckedImage = null;
            this.cashAuditCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cashAuditCheckBox.Name = "cashAuditCheckBox";
            this.cashAuditCheckBox.UnCheckedImage = null;
            this.cashAuditCheckBox.UseVisualStyleBackColor = true;
            // 
            // documentsCheckBox
            // 
            resources.ApplyResources(this.documentsCheckBox, "documentsCheckBox");
            this.documentsCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.documentsCheckBox.Checked = true;
            this.documentsCheckBox.CheckedImage = null;
            this.documentsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.documentsCheckBox.Name = "documentsCheckBox";
            this.documentsCheckBox.UnCheckedImage = null;
            this.documentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // employeesCheckBox
            // 
            resources.ApplyResources(this.employeesCheckBox, "employeesCheckBox");
            this.employeesCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.employeesCheckBox.Checked = true;
            this.employeesCheckBox.CheckedImage = null;
            this.employeesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.employeesCheckBox.Name = "employeesCheckBox";
            this.employeesCheckBox.UnCheckedImage = null;
            this.employeesCheckBox.UseVisualStyleBackColor = true;
            // 
            // representativesCheckBox
            // 
            resources.ApplyResources(this.representativesCheckBox, "representativesCheckBox");
            this.representativesCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.representativesCheckBox.Checked = true;
            this.representativesCheckBox.CheckedImage = null;
            this.representativesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.representativesCheckBox.Name = "representativesCheckBox";
            this.representativesCheckBox.UnCheckedImage = null;
            this.representativesCheckBox.UseVisualStyleBackColor = true;
            // 
            // clientsCheckBox
            // 
            resources.ApplyResources(this.clientsCheckBox, "clientsCheckBox");
            this.clientsCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.clientsCheckBox.Checked = true;
            this.clientsCheckBox.CheckedImage = null;
            this.clientsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.clientsCheckBox.Name = "clientsCheckBox";
            this.clientsCheckBox.UnCheckedImage = null;
            this.clientsCheckBox.UseVisualStyleBackColor = true;
            // 
            // pettyCashCheckBox
            // 
            resources.ApplyResources(this.pettyCashCheckBox, "pettyCashCheckBox");
            this.pettyCashCheckBox.BorderColor = System.Drawing.Color.DimGray;
            this.pettyCashCheckBox.Checked = true;
            this.pettyCashCheckBox.CheckedImage = null;
            this.pettyCashCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pettyCashCheckBox.Name = "pettyCashCheckBox";
            this.pettyCashCheckBox.UnCheckedImage = null;
            this.pettyCashCheckBox.UseVisualStyleBackColor = true;
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
        private Pflegehaushaltsbuch.FormControls.CheckBox createCheckBox;
        private Pflegehaushaltsbuch.FormControls.CheckBox changeCheckBox;
        private Pflegehaushaltsbuch.FormControls.CheckBox adminCheckBox;
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.TextBox handsignBox;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.FlowLayoutPanel flowLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.Label label1;
        private FormControls.TextBox loginBox;
        private FormControls.CheckBox bookCheckBox;
        private FormControls.CheckBox cancelBookingCheckBox;
        private FormControls.CheckBox cashBalanceCheckBox;
        private FormControls.CheckBox bankBalanceCheckBox;
        private FormControls.CheckBox clientsCheckBox;
        private FormControls.CheckBox representativesCheckBox;
        private FormControls.CheckBox employeesCheckBox;
        private FormControls.CheckBox documentsCheckBox;
        private FormControls.CheckBox cashAuditCheckBox;
        private FormControls.CheckBox statisticsCheckBox;
        private FormControls.CheckBox pettyCashCheckBox;
    }
}
