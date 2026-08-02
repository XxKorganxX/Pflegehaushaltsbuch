namespace Pflegehaushaltsbuch.Forms
{
    partial class DesignForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignForm));
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.closeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tabControl = new Pflegehaushaltsbuch.FormControls.TabControl();
            this.generallyTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tableLayoutPanel5 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label4 = new Pflegehaushaltsbuch.FormControls.Label();
            this.documentPathBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.selectPathButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.designTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.backColorModeBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.fontSizeBox = new Pflegehaushaltsbuch.FormControls.TextBox();
            this.label2 = new Pflegehaushaltsbuch.FormControls.Label();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.languageTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.languageBox = new Pflegehaushaltsbuch.FormControls.ComboBox();
            this.label7 = new Pflegehaushaltsbuch.FormControls.Label();
            this.view = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.generallyTab.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.designTab.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.languageTab.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.view, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.closeButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.BackColor = System.Drawing.Color.Transparent;
            this.closeButton.BorderColor = System.Drawing.Color.Black;
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.closeButton.Name = "closeButton";
            this.closeButton.Radius = -1F;
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.AngleColorGradiant = 90F;
            this.tabControl.AutoSizeTabs = false;
            this.tabControl.BorderColor = System.Drawing.Color.White;
            this.tabControl.BorderWidth = 1F;
            this.tabControl.Controls.Add(this.generallyTab);
            this.tabControl.Controls.Add(this.designTab);
            this.tabControl.Controls.Add(this.languageTab);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.SelectedTabBoderColor = System.Drawing.Color.LightGreen;
            this.tabControl.SelectedTabBottomColor = System.Drawing.Color.Green;
            this.tabControl.SelectedTabForeColor = System.Drawing.Color.White;
            this.tabControl.SelectedTabTopColor = System.Drawing.Color.Green;
            this.tabControl.TabBackcolor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.tabControl.TabBorderColor = System.Drawing.Color.DarkGray;
            this.tabControl.TabBottomColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(207)))), ((int)(((byte)(207)))));
            this.tabControl.TabForeColor = System.Drawing.Color.Black;
            this.tabControl.TabTopColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.tabControl.VisibleTabs = false;
            // 
            // generallyTab
            // 
            resources.ApplyResources(this.generallyTab, "generallyTab");
            this.generallyTab.Controls.Add(this.tableLayoutPanel3);
            this.generallyTab.Name = "generallyTab";
            this.generallyTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel5, 2);
            this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.documentPathBox, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.selectPathButton, 0, 1);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.tableLayoutPanel5.SetColumnSpan(this.label4, 2);
            this.label4.Name = "label4";
            // 
            // documentPathBox
            // 
            resources.ApplyResources(this.documentPathBox, "documentPathBox");
            this.documentPathBox.Name = "documentPathBox";
            // 
            // selectPathButton
            // 
            resources.ApplyResources(this.selectPathButton, "selectPathButton");
            this.selectPathButton.BackColor = System.Drawing.Color.Transparent;
            this.selectPathButton.BorderColor = System.Drawing.Color.Black;
            this.selectPathButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectPathButton.Name = "selectPathButton";
            this.selectPathButton.UseVisualStyleBackColor = true;
            this.selectPathButton.Click += new System.EventHandler(this.selectPathButton_Click);
            // 
            // designTab
            // 
            resources.ApplyResources(this.designTab, "designTab");
            this.designTab.Controls.Add(this.tableLayoutPanel2);
            this.designTab.Name = "designTab";
            this.designTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.backColorModeBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.fontSizeBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // backColorModeBox
            // 
            resources.ApplyResources(this.backColorModeBox, "backColorModeBox");
            this.backColorModeBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.backColorModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.backColorModeBox.FormattingEnabled = true;
            this.backColorModeBox.Items.AddRange(new object[] {
            resources.GetString("backColorModeBox.Items"),
            resources.GetString("backColorModeBox.Items1"),
            resources.GetString("backColorModeBox.Items2"),
            resources.GetString("backColorModeBox.Items3")});
            this.backColorModeBox.Name = "backColorModeBox";
            // 
            // fontSizeBox
            // 
            resources.ApplyResources(this.fontSizeBox, "fontSizeBox");
            this.fontSizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fontSizeBox.Name = "fontSizeBox";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // languageTab
            // 
            resources.ApplyResources(this.languageTab, "languageTab");
            this.languageTab.Controls.Add(this.tableLayoutPanel4);
            this.languageTab.Name = "languageTab";
            this.languageTab.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.languageBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // languageBox
            // 
            resources.ApplyResources(this.languageBox, "languageBox");
            this.languageBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageBox.FormattingEnabled = true;
            this.languageBox.Items.AddRange(new object[] {
            resources.GetString("languageBox.Items"),
            resources.GetString("languageBox.Items1")});
            this.languageBox.Name = "languageBox";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Name = "label7";
            // 
            // view
            // 
            resources.ApplyResources(this.view, "view");
            this.view.Name = "view";
            this.view.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("view.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("view.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("view.Nodes2")))});
            this.view.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.view_AfterSelect);
            // 
            // DesignForm
            // 
            this.AcceptButton = this.closeButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "DesignForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.generallyTab.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.designTab.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.languageTab.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.Label label1;
        private FormControls.Label label2;
        private FormControls.Button closeButton;
        private FormControls.TextBox fontSizeBox;
        private Pflegehaushaltsbuch.FormControls.ComboBox backColorModeBox;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.TabControl tabControl;
        private System.Windows.Forms.TabPage generallyTab;
        private System.Windows.Forms.TabPage designTab;
        private FormControls.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TreeView view;
        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.Label label4;
        private FormControls.TextBox documentPathBox;
        private System.Windows.Forms.TabPage languageTab;
        private FormControls.TableLayoutPanel tableLayoutPanel4;
        private FormControls.ComboBox languageBox;
        private FormControls.Label label7;
        private FormControls.TableLayoutPanel tableLayoutPanel5;
        private FormControls.Button selectPathButton;
    }
}