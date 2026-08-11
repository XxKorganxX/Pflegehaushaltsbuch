namespace Pflegehaushaltsbuch.Forms.Dialoge
{
    partial class ResetLayoutsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResetLayoutsDialog));
            this.cashBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.bankBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.clientsBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.billBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.advisorsBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.employeeBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.cashCheckBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.allBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.button1 = new Pflegehaushaltsbuch.FormControls.Button();
            this.quittanceBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.officeCashBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cashBox
            // 
            resources.ApplyResources(this.cashBox, "cashBox");
            this.cashBox.BackColor = System.Drawing.Color.Transparent;
            this.cashBox.BorderColor = System.Drawing.Color.White;
            this.cashBox.CheckedImage = null;
            this.cashBox.Name = "cashBox";
            this.cashBox.UnCheckedImage = null;
            this.cashBox.UseVisualStyleBackColor = false;
            this.cashBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // bankBox
            // 
            resources.ApplyResources(this.bankBox, "bankBox");
            this.bankBox.BackColor = System.Drawing.Color.Transparent;
            this.bankBox.BorderColor = System.Drawing.Color.White;
            this.bankBox.CheckedImage = null;
            this.bankBox.Name = "bankBox";
            this.bankBox.UnCheckedImage = null;
            this.bankBox.UseVisualStyleBackColor = false;
            this.bankBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // clientsBox
            // 
            resources.ApplyResources(this.clientsBox, "clientsBox");
            this.clientsBox.BackColor = System.Drawing.Color.Transparent;
            this.clientsBox.BorderColor = System.Drawing.Color.White;
            this.clientsBox.CheckedImage = null;
            this.clientsBox.Name = "clientsBox";
            this.clientsBox.UnCheckedImage = null;
            this.clientsBox.UseVisualStyleBackColor = false;
            this.clientsBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // billBox
            // 
            resources.ApplyResources(this.billBox, "billBox");
            this.billBox.BackColor = System.Drawing.Color.Transparent;
            this.billBox.BorderColor = System.Drawing.Color.White;
            this.billBox.CheckedImage = null;
            this.billBox.Name = "billBox";
            this.billBox.UnCheckedImage = null;
            this.billBox.UseVisualStyleBackColor = false;
            this.billBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // advisorsBox
            // 
            resources.ApplyResources(this.advisorsBox, "advisorsBox");
            this.advisorsBox.BackColor = System.Drawing.Color.Transparent;
            this.advisorsBox.BorderColor = System.Drawing.Color.White;
            this.advisorsBox.CheckedImage = null;
            this.advisorsBox.Name = "advisorsBox";
            this.advisorsBox.UnCheckedImage = null;
            this.advisorsBox.UseVisualStyleBackColor = false;
            this.advisorsBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // assistantsBox
            // 
            resources.ApplyResources(this.employeeBox, "assistantsBox");
            this.employeeBox.BackColor = System.Drawing.Color.Transparent;
            this.employeeBox.BorderColor = System.Drawing.Color.White;
            this.employeeBox.CheckedImage = null;
            this.employeeBox.Name = "assistantsBox";
            this.employeeBox.UnCheckedImage = null;
            this.employeeBox.UseVisualStyleBackColor = false;
            this.employeeBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // cashCheckBox
            // 
            resources.ApplyResources(this.cashCheckBox, "cashCheckBox");
            this.cashCheckBox.BackColor = System.Drawing.Color.Transparent;
            this.cashCheckBox.BorderColor = System.Drawing.Color.White;
            this.cashCheckBox.CheckedImage = null;
            this.cashCheckBox.Name = "cashCheckBox";
            this.cashCheckBox.UnCheckedImage = null;
            this.cashCheckBox.UseVisualStyleBackColor = false;
            this.cashCheckBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // allBox
            // 
            resources.ApplyResources(this.allBox, "allBox");
            this.allBox.BackColor = System.Drawing.Color.Transparent;
            this.allBox.BorderColor = System.Drawing.Color.White;
            this.allBox.CheckedImage = null;
            this.allBox.Name = "allBox";
            this.allBox.UnCheckedImage = null;
            this.allBox.UseVisualStyleBackColor = false;
            this.allBox.CheckedChanged += new System.EventHandler(this.allBox_CheckedChanged);
            this.allBox.Click += new System.EventHandler(this.allBox_Click);
            // 
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            this.okButton.Checked = false;
            this.okButton.CheckedState = false;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.ForeColor = System.Drawing.Color.White;
            this.okButton.Name = "okButton";
            this.okButton.PaintBackGround = true;
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button1.BorderColor = System.Drawing.Color.DimGray;
            this.button1.Checked = false;
            this.button1.CheckedState = false;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.PaintBackGround = true;
            this.button1.Radius = -1F;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // quittanceBox
            // 
            resources.ApplyResources(this.quittanceBox, "quittanceBox");
            this.quittanceBox.BackColor = System.Drawing.Color.Transparent;
            this.quittanceBox.BorderColor = System.Drawing.Color.White;
            this.quittanceBox.CheckedImage = null;
            this.quittanceBox.Name = "quittanceBox";
            this.quittanceBox.UnCheckedImage = null;
            this.quittanceBox.UseVisualStyleBackColor = false;
            this.quittanceBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.Border = false;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.BorderWidth = 1F;
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.quittanceBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.allBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cashCheckBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cashBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bankBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.clientsBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.billBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.advisorsBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.employeeBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.officeCashBox, 1, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.okButton);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // officeCashBox
            // 
            resources.ApplyResources(this.officeCashBox, "officeCashBox");
            this.officeCashBox.BackColor = System.Drawing.Color.Transparent;
            this.officeCashBox.BorderColor = System.Drawing.Color.White;
            this.officeCashBox.CheckedImage = null;
            this.officeCashBox.Name = "officeCashBox";
            this.officeCashBox.UnCheckedImage = null;
            this.officeCashBox.UseVisualStyleBackColor = false;
            this.officeCashBox.Click += new System.EventHandler(this.cashBox_Click);
            // 
            // ResetLayoutsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetLayoutsDialog";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.CheckBox cashBox;
        private FormControls.CheckBox bankBox;
        private FormControls.CheckBox clientsBox;
        private FormControls.CheckBox billBox;
        private FormControls.CheckBox advisorsBox;
        private FormControls.CheckBox employeeBox;
        private FormControls.CheckBox cashCheckBox;
        private FormControls.CheckBox allBox;
        private FormControls.Button okButton;
        private FormControls.Button button1;
        private FormControls.CheckBox quittanceBox;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.CheckBox officeCashBox;
    }
}
