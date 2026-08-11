namespace Pflegehaushaltsbuch.Forms
{
    partial class CreateClientDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateClientDialog));
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.nameBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.debitorNrBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.cancelButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.okButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.advisorsBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label12 = new Pflegehaushaltsbuch.FormControls.Label();
            this.streetBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.zipcodeBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.cityBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label13 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label14 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label15 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.useAdvisor = new Pflegehaushaltsbuch.FormControls.CheckBox();
            this.titleBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.saldoBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.bornBox = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Name = "label4";
            // 
            // debitorNrBox
            // 
            this.debitorNrBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.debitorNrBox, "debitorNrBox");
            this.debitorNrBox.Name = "debitorNrBox";
            this.debitorNrBox.TabStop = false;
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Name = "label3";
            // 
            // advisorsBox
            // 
            resources.ApplyResources(this.advisorsBox, "advisorsBox");
            this.advisorsBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.advisorsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.advisorsBox.FormattingEnabled = true;
            this.advisorsBox.Name = "advisorsBox";
            this.advisorsBox.SelectedIndexChanged += new System.EventHandler(this.advisorsBox_SelectedIndexChanged);
            this.advisorsBox.SelectionChangeCommitted += new System.EventHandler(this.advisorsBox_SelectionChangeCommitted);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Name = "label12";
            // 
            // streetBox
            // 
            this.streetBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.streetBox, "streetBox");
            this.streetBox.Name = "streetBox";
            // 
            // zipcodeBox
            // 
            this.zipcodeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.zipcodeBox, "zipcodeBox");
            this.zipcodeBox.Name = "zipcodeBox";
            // 
            // cityBox
            // 
            this.cityBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.cityBox, "cityBox");
            this.cityBox.Name = "cityBox";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Name = "label13";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Name = "label14";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Name = "label15";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // useAdvisor
            // 
            resources.ApplyResources(this.useAdvisor, "useAdvisor");
            this.useAdvisor.BorderColor = System.Drawing.Color.DimGray;
            this.useAdvisor.CheckedImage = null;
            this.useAdvisor.Name = "useAdvisor";
            this.useAdvisor.UnCheckedImage = null;
            this.useAdvisor.UseVisualStyleBackColor = true;
            this.useAdvisor.CheckedChanged += new System.EventHandler(this.useAdvisor_CheckedChanged);
            // 
            // titleBox
            // 
            this.titleBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.titleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.titleBox.FormattingEnabled = true;
            resources.ApplyResources(this.titleBox, "titleBox");
            this.titleBox.Name = "titleBox";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.advisorsBox, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.debitorNrBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.titleBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cityBox, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.zipcodeBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.streetBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label13, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.nameBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label14, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label15, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.saldoBox, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.useAdvisor, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.bornBox, 1, 3);
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
            // saldoBox
            // 
            this.saldoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.saldoBox, "saldoBox");
            this.saldoBox.Name = "saldoBox";
            // 
            // bornBox
            // 
            resources.ApplyResources(this.bornBox, "bornBox");
            this.bornBox.Days = true;
            this.bornBox.Name = "bornBox";
            // 
            // CreateClientDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.Controls.Add(this.tableLayoutPanel1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateClientDialog";
            this.Shown += new System.EventHandler(this.CreateClientForm_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.Button okButton;
        private FormControls.Button cancelButton;
        private Pflegehaushaltsbuch.FormControls.Label label1;
        private Pflegehaushaltsbuch.FormControls.TextBox nameBox;
        private Pflegehaushaltsbuch.FormControls.Label label4;
        private Pflegehaushaltsbuch.FormControls.TextBox debitorNrBox;
        private Pflegehaushaltsbuch.FormControls.Label label3;
        private Pflegehaushaltsbuch.FormControls.ComboBox advisorsBox;
        private Pflegehaushaltsbuch.FormControls.Label label12;
        private Pflegehaushaltsbuch.FormControls.TextBox streetBox;
        private Pflegehaushaltsbuch.FormControls.TextBox zipcodeBox;
        private Pflegehaushaltsbuch.FormControls.TextBox cityBox;
        private Pflegehaushaltsbuch.FormControls.Label label13;
        private Pflegehaushaltsbuch.FormControls.Label label14;
        private Pflegehaushaltsbuch.FormControls.Label label15;
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.CheckBox useAdvisor;
        private Pflegehaushaltsbuch.FormControls.ComboBox titleBox;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.TextBox saldoBox;
        private FormControls.DateTimeBox bornBox;
    }
}
