namespace Pflegehaushaltsbuch.FormControls
{
    partial class DateTimeBox
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateTimeBox));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.yearBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.monthBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.dayBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.calendarButton = new Pflegehaushaltsbuch.FormControls.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calendarButton)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Border = false;
            this.tableLayoutPanel1.BorderColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.BorderWidth = 1F;
            this.tableLayoutPanel1.Controls.Add(this.yearBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.monthBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dayBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.calendarButton, 3, 0);
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // yearBox
            // 
            this.yearBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.yearBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.yearBox, "yearBox");
            this.yearBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.yearBox.FormattingEnabled = true;
            this.yearBox.Name = "yearBox";
            this.yearBox.SelectionChangeCommitted += new System.EventHandler(this.update_SelectionChangeCommitted);
            // 
            // monthBox
            // 
            this.monthBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.monthBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.monthBox, "monthBox");
            this.monthBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.monthBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monthBox.FormattingEnabled = true;
            this.monthBox.Items.AddRange(new object[] {
            resources.GetString("monthBox.Items"),
            resources.GetString("monthBox.Items1"),
            resources.GetString("monthBox.Items2"),
            resources.GetString("monthBox.Items3"),
            resources.GetString("monthBox.Items4"),
            resources.GetString("monthBox.Items5"),
            resources.GetString("monthBox.Items6"),
            resources.GetString("monthBox.Items7"),
            resources.GetString("monthBox.Items8"),
            resources.GetString("monthBox.Items9"),
            resources.GetString("monthBox.Items10"),
            resources.GetString("monthBox.Items11")});
            this.monthBox.Name = "monthBox";
            this.monthBox.SelectionChangeCommitted += new System.EventHandler(this.update_SelectionChangeCommitted);
            // 
            // dayBox
            // 
            this.dayBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.dayBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.dayBox, "dayBox");
            this.dayBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.dayBox.FormattingEnabled = true;
            this.dayBox.Name = "dayBox";
            this.dayBox.SelectionChangeCommitted += new System.EventHandler(this.update_SelectionChangeCommitted);
            // 
            // calendarButton
            // 
            resources.ApplyResources(this.calendarButton, "calendarButton");
            this.calendarButton.BackgroundImage = global::Pflegehaushaltsbuch.Properties.Resources.calendar;
            this.calendarButton.Name = "calendarButton";
            this.calendarButton.TabStop = false;
            this.calendarButton.Click += new System.EventHandler(this.calendarButton_Click);
            // 
            // DateTimeBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "DateTimeBox";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.calendarButton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Pflegehaushaltsbuch.FormControls.ComboBox dayBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox monthBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox yearBox;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox calendarButton;
    }
}
