namespace Pflegehaushaltsbuch.Forms
{
    partial class CreateEmployeesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateEmployeesDialog));
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.amountBox = new Pflegehaushaltsbuch.FormControls.NumericUpDown();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.nameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.bookAccountBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label5 = new Pflegehaushaltsbuch.FormControls.Label();
            this.idBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.dateBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            ((System.ComponentModel.ISupportInitialize)(this.amountBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
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
            // okButton
            // 
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Name = "okButton";
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // amountBox
            // 
            this.amountBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.amountBox.DecimalPlaces = 2;
            resources.ApplyResources(this.amountBox, "amountBox");
            this.amountBox.Maximum = new decimal(new int[] {
            1874919423,
            2328306,
            0,
            0});
            this.amountBox.Name = "amountBox";
            this.amountBox.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // nameBox
            // 
            this.nameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nameBox, "nameBox");
            this.nameBox.Name = "nameBox";
            // 
            // bookAccountBox
            // 
            resources.ApplyResources(this.bookAccountBox, "bookAccountBox");
            this.bookAccountBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.bookAccountBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bookAccountBox.FormattingEnabled = true;
            this.bookAccountBox.Name = "bookAccountBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Name = "label5";
            // 
            // idBox
            // 
            this.idBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.idBox, "idBox");
            this.idBox.Name = "idBox";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bookAccountBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.amountBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.nameBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.idBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.dateBox, 1, 2);
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
            // dateBox
            // 
            resources.ApplyResources(this.dateBox, "dateBox");
            this.dateBox.Days = true;
            this.dateBox.Name = "dateBox";
            this.dateBox.ShowYear = true;
            // 
            // CreateEmployeesDialog
            // 
            this.AcceptButton = this.okButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateEmployeesDialog";
            ((System.ComponentModel.ISupportInitialize)(this.amountBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.Label label4;
        private FormControls.Button cancelButton;
        private FormControls.Button okButton;
        private Pflegehaushaltsbuch.FormControls.NumericUpDown amountBox;
        private Pflegehaushaltsbuch.FormControls.Label label3;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private Pflegehaushaltsbuch.FormControls.TextBox nameBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox bookAccountBox;
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.Label label5;
        private Pflegehaushaltsbuch.FormControls.TextBox idBox;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.DateTimeBox dateBox;
    }
}
