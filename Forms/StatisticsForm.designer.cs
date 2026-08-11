namespace Pflegehaushaltsbuch.Forms
{
    partial class StatisticsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsForm));
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.dateEnd = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label3 = new Pflegehaushaltsbuch.FormControls.Label();
            this.dateBegin = new Pflegehaushaltsbuch.FormControls.DateTimeBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.comboBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.barDiagram2 = new Pflegehaushaltsbuch.Forms.Dialoge.BarDiagramControl();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barDiagram2)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.Border = false;
            this.tableLayoutPanel2.BorderColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BorderWidth = 1F;
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // label7
            // 
            this.label7.AttachRegion = false;
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.DrawLine = false;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Gradiant = true;
            this.label7.LinePadding = 0;
            this.label7.Name = "label7";
            this.label7.Radius = 0F;
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backButton.BorderColor = System.Drawing.Color.DimGray;
            this.backButton.Checked = false;
            this.backButton.CheckedState = false;
            this.backButton.ForeColor = System.Drawing.Color.White;
            this.backButton.Name = "backButton";
            this.backButton.PaintBackGround = true;
            this.backButton.Radius = -1F;
            this.backButton.RoundEdges = false;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.Border = true;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.BorderWidth = 1F;
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.barDiagram2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.Border = false;
            this.tableLayoutPanel3.BorderColor = System.Drawing.Color.Empty;
            this.tableLayoutPanel3.BorderWidth = 1F;
            this.tableLayoutPanel3.Controls.Add(this.dateEnd, 7, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.dateBegin, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.comboBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // dateEnd
            // 
            resources.ApplyResources(this.dateEnd, "dateEnd");
            this.dateEnd.BackColor = System.Drawing.Color.Transparent;
            this.dateEnd.Days = false;
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.ValueChanged += new Pflegehaushaltsbuch.FormControls.DateTimeBox.UpdateDistanceDelegate(this.allDateBoxes_ValueChanged);
            // 
            // label3
            // 
            this.label3.AttachRegion = false;
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.DrawLine = false;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Gradiant = false;
            this.label3.LinePadding = 0;
            this.label3.Name = "label3";
            this.label3.Radius = 0F;
            // 
            // dateBegin
            // 
            resources.ApplyResources(this.dateBegin, "dateBegin");
            this.dateBegin.BackColor = System.Drawing.Color.Transparent;
            this.dateBegin.Days = false;
            this.dateBegin.Name = "dateBegin";
            this.dateBegin.ValueChanged += new Pflegehaushaltsbuch.FormControls.DateTimeBox.UpdateDistanceDelegate(this.allDateBoxes_ValueChanged);
            // 
            // label2
            // 
            this.label2.AttachRegion = false;
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.DrawLine = false;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Gradiant = false;
            this.label2.LinePadding = 0;
            this.label2.Name = "label2";
            this.label2.Radius = 0F;
            // 
            // comboBox
            // 
            resources.ApplyResources(this.comboBox, "comboBox");
            this.comboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox.FormattingEnabled = true;
            this.comboBox.Items.AddRange(new object[] {
            resources.GetString("comboBox.Items"),
            resources.GetString("comboBox.Items1"),
            resources.GetString("comboBox.Items2")});
            this.comboBox.Name = "comboBox";
            this.comboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AttachRegion = false;
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.DrawLine = false;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Gradiant = false;
            this.label1.LinePadding = 0;
            this.label1.Name = "label1";
            this.label1.Radius = 0F;
            // 
            // barDiagram2
            // 
            this.barDiagram2.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.barDiagram2, "barDiagram2");
            this.barDiagram2.Name = "barDiagram2";
            this.barDiagram2.TabStop = false;
            // 
            // StatisticsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.DoubleBuffered = true;
            this.Name = "StatisticsForm";
            this.Load += new System.EventHandler(this.StatisticsForm_Load);
            this.Enter += new System.EventHandler(this.StatisticsForm_Enter);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barDiagram2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.Button backButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private Dialoge.BarDiagramControl barDiagram2;
        private Pflegehaushaltsbuch.FormControls.Label label3;
        private Pflegehaushaltsbuch.FormControls.Label label2;
        private Pflegehaushaltsbuch.FormControls.ComboBox comboBox;
        private FormControls.DateTimeBox dateBegin;
        private FormControls.DateTimeBox dateEnd;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private FormControls.Label label7;
        private FormControls.Label label1;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
    }
}