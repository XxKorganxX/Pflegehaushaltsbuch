namespace Pflegehaushaltsbuch.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new Pflegehaushaltsbuch.FormControls.TabControl();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AngleColorGradiant = 90F;
            this.tabControl1.AutoSizeTabs = false;
            this.tabControl1.BorderColor = System.Drawing.Color.White;
            this.tabControl1.BorderWidth = 1F;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedTabBoderColor = System.Drawing.Color.LightGreen;
            this.tabControl1.SelectedTabBottomColor = System.Drawing.Color.Green;
            this.tabControl1.SelectedTabForeColor = System.Drawing.Color.White;
            this.tabControl1.SelectedTabTopColor = System.Drawing.Color.Green;
            this.tabControl1.TabBackcolor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.tabControl1.TabBorderColor = System.Drawing.Color.DarkGray;
            this.tabControl1.TabBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(207)))), ((int)(((byte)(207)))));
            this.tabControl1.TabForeColor = System.Drawing.Color.Black;
            this.tabControl1.TabStop = false;
            this.tabControl1.TabTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.tabControl1.VisibleTabs = false;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Orange;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.TabControl tabControl1;
    }
}