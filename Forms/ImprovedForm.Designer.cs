namespace Pflegehaushaltsbuch.Forms
{
    partial class ImprovedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImprovedForm));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.sendButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.textBox = new Pflegehaushaltsbuch.FormControls.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Gradiant = true;
            this.label7.Name = "label7";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.sendButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // sendButton
            // 
            resources.ApplyResources(this.sendButton, "sendButton");
            this.sendButton.BackColor = System.Drawing.Color.Transparent;
            this.sendButton.BorderColor = System.Drawing.Color.Black;
            this.sendButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.sendButton.Name = "sendButton";
            this.sendButton.Radius = -1F;
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.BackColor = System.Drawing.Color.Transparent;
            this.backButton.BorderColor = System.Drawing.Color.Black;
            this.backButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.backButton.Name = "backButton";
            this.backButton.Radius = -1F;
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel2.Border = true;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.textBox, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // textBox
            // 
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel2.SetColumnSpan(this.textBox, 3);
            resources.ApplyResources(this.textBox, "textBox");
            this.textBox.Name = "textBox";
            this.tableLayoutPanel2.SetRowSpan(this.textBox, 2);
            // 
            // ImprovedForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ImprovedForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button sendButton;
        private FormControls.Label label7;
        private FormControls.Button backButton;
        private FormControls.RichTextBox textBox;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
    }
}