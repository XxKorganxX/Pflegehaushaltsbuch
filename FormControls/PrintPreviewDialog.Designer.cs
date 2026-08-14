namespace Pflegehaushaltsbuch.FormControls
{
    partial class PrintPreviewDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPreviewDialog));
            this.panel = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.zoomBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.printButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.button1 = new Pflegehaushaltsbuch.FormControls.Button();
            this.printerBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.printsBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label6 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.pageBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.rowBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.autoZoomButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.printerSettingsButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.label11 = new Pflegehaushaltsbuch.FormControls.Label();
            this.panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel.BorderColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.panel, "panel");
            this.panel.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.panel.Name = "panel";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 17);
            this.tableLayoutPanel1.Controls.Add(this.zoomBox, 1, 17);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.printerBox, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.printsBox, 1, 15);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 14);
            this.tableLayoutPanel1.Controls.Add(this.autoZoomButton, 1, 18);
            this.tableLayoutPanel1.Controls.Add(this.printerSettingsButton, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 13);
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // zoomBox
            // 
            this.zoomBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.zoomBox, "zoomBox");
            this.zoomBox.Name = "zoomBox";
            this.zoomBox.TextChanged += new System.EventHandler(this.zoomBox_TextChanged);
            this.zoomBox.Validated += new System.EventHandler(this.zoomBox_Validated);
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.printButton);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // printButton
            // 
            resources.ApplyResources(this.printButton, "printButton");
            this.printButton.BackColor = System.Drawing.Color.Transparent;
            this.printButton.BorderColor = System.Drawing.Color.Black;
            this.printButton.Name = "printButton";
            this.printButton.PaintBackGround = false;
            this.printButton.Radius = -1F;
            this.printButton.UseVisualStyleBackColor = true;
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::Pflegehaushaltsbuch.Properties.Resources.Adobe_PDF_file_icon_32x32;
            this.button1.BorderColor = System.Drawing.Color.Black;
            this.button1.Name = "button1";
            this.button1.PaintBackGround = false;
            this.button1.Radius = -1F;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.pdfButton_Click);
            // 
            // printerBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.printerBox, 2);
            resources.ApplyResources(this.printerBox, "printerBox");
            this.printerBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.printerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.printerBox.FormattingEnabled = true;
            this.printerBox.Name = "printerBox";
            this.printerBox.SelectedIndexChanged += new System.EventHandler(this.printerBox_SelectedIndexChanged_1);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Gradiant = true;
            this.label1.Name = "label1";
            // 
            // printsBox
            // 
            this.printsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.printsBox, "printsBox");
            this.printsBox.Name = "printsBox";
            this.printsBox.TextChanged += new System.EventHandler(this.zoomBox_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.pageBox);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.rowBox);
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // pageBox
            // 
            this.pageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pageBox, "pageBox");
            this.pageBox.Name = "pageBox";
            this.pageBox.ReadOnly = true;
            this.pageBox.TextChanged += new System.EventHandler(this.pageBox_TextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // rowBox
            // 
            this.rowBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.rowBox, "rowBox");
            this.rowBox.Name = "rowBox";
            this.rowBox.ReadOnly = true;
            this.rowBox.TextChanged += new System.EventHandler(this.rowBox_TextChanged);
            // 
            // autoZoomButton
            // 
            resources.ApplyResources(this.autoZoomButton, "autoZoomButton");
            this.autoZoomButton.BackColor = System.Drawing.Color.Transparent;
            this.autoZoomButton.BorderColor = System.Drawing.Color.Black;
            this.autoZoomButton.Name = "autoZoomButton";
            this.autoZoomButton.Radius = -1F;
            this.autoZoomButton.UseVisualStyleBackColor = true;
            this.autoZoomButton.Click += new System.EventHandler(this.autoZoomButton_Click);
            // 
            // printerSettingsButton
            // 
            resources.ApplyResources(this.printerSettingsButton, "printerSettingsButton");
            this.printerSettingsButton.BackColor = System.Drawing.Color.Transparent;
            this.printerSettingsButton.BorderColor = System.Drawing.Color.Black;
            this.tableLayoutPanel1.SetColumnSpan(this.printerSettingsButton, 2);
            this.printerSettingsButton.Name = "printerSettingsButton";
            this.printerSettingsButton.Radius = -1F;
            this.printerSettingsButton.UseVisualStyleBackColor = true;
            this.printerSettingsButton.Click += new System.EventHandler(this.printerSettingsButton_Click);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.tableLayoutPanel1.SetColumnSpan(this.label11, 2);
            this.label11.Gradiant = true;
            this.label11.Name = "label11";
            // 
            // PrintPreviewDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Name = "PrintPreviewDialog";
            this.Controls.SetChildIndex(this.panel, 0);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Pflegehaushaltsbuch.FormControls.ComboBox printerBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button printButton;
        private Button button1;
        private TableLayoutPanel panel;
        private Label label2;
        private TextBox zoomBox;
        private Label label4;
        private TextBox printsBox;
        private Label label1;
        private TextBox pageBox;
        private TextBox rowBox;
        private Label label6;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label3;
        private Button autoZoomButton;
        private Button printerSettingsButton;
        private Label label11;
    }
}
