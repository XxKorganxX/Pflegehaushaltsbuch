namespace Pflegehaushaltsbuch.Forms
{
    partial class CreateDeadlineForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateDeadlineForm));
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.dateTimeBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.allMonthBox = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.noteBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cancelButton.BorderColor = System.Drawing.Color.DimGray;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.cancelButton, "cancelButton");
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Radius = -1F;
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // okButton
            // 
            this.okButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.okButton.BorderColor = System.Drawing.Color.DimGray;
            resources.ApplyResources(this.okButton, "okButton");
            this.okButton.Name = "okButton";
            this.okButton.Radius = -1F;
            this.okButton.UseVisualStyleBackColor = false;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dateTimeBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.allMonthBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.noteBox, 1, 1);
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
            // dateTimeBox
            // 
            resources.ApplyResources(this.dateTimeBox, "dateTimeBox");
            this.dateTimeBox.Days = true;
            this.dateTimeBox.Name = "dateTimeBox";
            this.dateTimeBox.ShowYear = true;
            // 
            // allMonthBox
            // 
            resources.ApplyResources(this.allMonthBox, "allMonthBox");
            this.allMonthBox.BorderColor = System.Drawing.Color.DimGray;
            this.allMonthBox.CheckedImage = null;
            this.allMonthBox.Name = "allMonthBox";
            this.allMonthBox.UnCheckedImage = null;
            this.allMonthBox.UseVisualStyleBackColor = true;
            // 
            // noteBox
            // 
            this.noteBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.noteBox, "noteBox");
            this.noteBox.Name = "noteBox";
            // 
            // CreateDeadlineForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CreateDeadlineForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.Button cancelButton;
        private Pflegehaushaltsbuch.FormControls.Button okButton;
        private Pflegehaushaltsbuch.FormControls.Label label4;
        private Pflegehaushaltsbuch.FormControls.TableLayoutPanel tableLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.DateTimeBox dateTimeBox;
        private FormControls.CheckBox allMonthBox;
        private FormControls.TextBox noteBox;
    }
}