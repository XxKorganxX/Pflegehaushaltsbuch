namespace Pflegehaushaltsbuch.Forms
{
    partial class DataExchangeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataExchangeForm));
            this.tableLayoutPanel3 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.label19 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.FlowLayoutPanel();
            this.exportButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.backButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel1 = new Pflegehaushaltsbuch.FormControls.TableLayoutPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.clientsTabPage = new System.Windows.Forms.TabPage();
            this.clientView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.DeadlinesTabPage = new System.Windows.Forms.TabPage();
            this.deadlinesView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.representativesTabPage = new System.Windows.Forms.TabPage();
            this.advisorView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.employeesTabPage = new System.Windows.Forms.TabPage();
            this.employeesView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.pettyCashTransactionsTabPage = new System.Windows.Forms.TabPage();
            this.officeCashTransactionsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.cashTransactionsTabPage = new System.Windows.Forms.TabPage();
            this.cashTransactionsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.bankTransactionsTabPage = new System.Windows.Forms.TabPage();
            this.bankTransactionsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.clientsTransactionsTabPage = new System.Windows.Forms.TabPage();
            this.clientTransactionsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.accountsTabPage = new System.Windows.Forms.TabPage();
            this.accountsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.documentTabPage = new System.Windows.Forms.TabPage();
            this.documentsView = new Pflegehaushaltsbuch.FormControls.DataGridView();
            this.label1 = new Pflegehaushaltsbuch.FormControls.Label();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.includeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.excludeButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.resetButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.moveLftButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.moveRightButton = new Pflegehaushaltsbuch.FormControls.Button();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.clientsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientView)).BeginInit();
            this.DeadlinesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deadlinesView)).BeginInit();
            this.representativesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advisorView)).BeginInit();
            this.employeesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.employeesView)).BeginInit();
            this.pettyCashTransactionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.officeCashTransactionsView)).BeginInit();
            this.cashTransactionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cashTransactionsView)).BeginInit();
            this.bankTransactionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankTransactionsView)).BeginInit();
            this.clientsTransactionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientTransactionsView)).BeginInit();
            this.accountsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountsView)).BeginInit();
            this.documentTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentsView)).BeginInit();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BorderColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Gradiant = true;
            this.label19.Name = "label19";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.exportButton);
            this.flowLayoutPanel1.Controls.Add(this.backButton);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // exportButton
            // 
            resources.ApplyResources(this.exportButton, "exportButton");
            this.exportButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exportButton.BorderColor = System.Drawing.Color.DimGray;
            this.exportButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.exportButton.Name = "exportButton";
            this.exportButton.Radius = -1F;
            this.exportButton.UseVisualStyleBackColor = false;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // backButton
            // 
            resources.ApplyResources(this.backButton, "backButton");
            this.backButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backButton.BorderColor = System.Drawing.Color.DimGray;
            this.backButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.backButton.Name = "backButton";
            this.backButton.Radius = -1F;
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tableLayoutPanel1.Border = true;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tabControl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // tabControl
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl, 2);
            this.tabControl.Controls.Add(this.clientsTabPage);
            this.tabControl.Controls.Add(this.DeadlinesTabPage);
            this.tabControl.Controls.Add(this.representativesTabPage);
            this.tabControl.Controls.Add(this.employeesTabPage);
            this.tabControl.Controls.Add(this.pettyCashTransactionsTabPage);
            this.tabControl.Controls.Add(this.cashTransactionsTabPage);
            this.tabControl.Controls.Add(this.bankTransactionsTabPage);
            this.tabControl.Controls.Add(this.clientsTransactionsTabPage);
            this.tabControl.Controls.Add(this.accountsTabPage);
            this.tabControl.Controls.Add(this.documentTabPage);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // clientsTabPage
            // 
            this.clientsTabPage.Controls.Add(this.clientView);
            resources.ApplyResources(this.clientsTabPage, "clientsTabPage");
            this.clientsTabPage.Name = "clientsTabPage";
            this.clientsTabPage.UseVisualStyleBackColor = true;
            // 
            // clientView
            // 
            this.clientView.AllowUserToAddRows = false;
            this.clientView.AllowUserToDeleteRows = false;
            this.clientView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.clientView, "clientView");
            this.clientView.Name = "clientView";
            this.clientView.RowTemplate.Height = 24;
            this.clientView.StandardTab = true;
            // 
            // DeadlinesTabPage
            // 
            this.DeadlinesTabPage.Controls.Add(this.deadlinesView);
            resources.ApplyResources(this.DeadlinesTabPage, "DeadlinesTabPage");
            this.DeadlinesTabPage.Name = "DeadlinesTabPage";
            this.DeadlinesTabPage.UseVisualStyleBackColor = true;
            // 
            // deadlinesView
            // 
            this.deadlinesView.AllowUserToAddRows = false;
            this.deadlinesView.AllowUserToDeleteRows = false;
            this.deadlinesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.deadlinesView, "deadlinesView");
            this.deadlinesView.Name = "deadlinesView";
            this.deadlinesView.RowTemplate.Height = 24;
            this.deadlinesView.StandardTab = true;
            // 
            // representativesTabPage
            // 
            this.representativesTabPage.Controls.Add(this.advisorView);
            resources.ApplyResources(this.representativesTabPage, "representativesTabPage");
            this.representativesTabPage.Name = "representativesTabPage";
            this.representativesTabPage.UseVisualStyleBackColor = true;
            // 
            // advisorView
            // 
            this.advisorView.AllowUserToAddRows = false;
            this.advisorView.AllowUserToDeleteRows = false;
            this.advisorView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.advisorView, "advisorView");
            this.advisorView.Name = "advisorView";
            this.advisorView.RowTemplate.Height = 24;
            this.advisorView.StandardTab = true;
            // 
            // employeesTabPage
            // 
            this.employeesTabPage.Controls.Add(this.employeesView);
            resources.ApplyResources(this.employeesTabPage, "employeesTabPage");
            this.employeesTabPage.Name = "employeesTabPage";
            this.employeesTabPage.UseVisualStyleBackColor = true;
            // 
            // employeesView
            // 
            this.employeesView.AllowUserToAddRows = false;
            this.employeesView.AllowUserToDeleteRows = false;
            this.employeesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.employeesView, "employeesView");
            this.employeesView.Name = "employeesView";
            this.employeesView.RowTemplate.Height = 24;
            this.employeesView.StandardTab = true;
            // 
            // pettyCashTransactionsTabPage
            // 
            this.pettyCashTransactionsTabPage.Controls.Add(this.officeCashTransactionsView);
            resources.ApplyResources(this.pettyCashTransactionsTabPage, "pettyCashTransactionsTabPage");
            this.pettyCashTransactionsTabPage.Name = "pettyCashTransactionsTabPage";
            this.pettyCashTransactionsTabPage.UseVisualStyleBackColor = true;
            // 
            // officeCashTransactionsView
            // 
            this.officeCashTransactionsView.AllowUserToAddRows = false;
            this.officeCashTransactionsView.AllowUserToDeleteRows = false;
            this.officeCashTransactionsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.officeCashTransactionsView, "officeCashTransactionsView");
            this.officeCashTransactionsView.Name = "officeCashTransactionsView";
            this.officeCashTransactionsView.RowTemplate.Height = 24;
            this.officeCashTransactionsView.StandardTab = true;
            // 
            // cashTransactionsTabPage
            // 
            this.cashTransactionsTabPage.Controls.Add(this.cashTransactionsView);
            resources.ApplyResources(this.cashTransactionsTabPage, "cashTransactionsTabPage");
            this.cashTransactionsTabPage.Name = "cashTransactionsTabPage";
            this.cashTransactionsTabPage.UseVisualStyleBackColor = true;
            // 
            // cashTransactionsView
            // 
            this.cashTransactionsView.AllowUserToAddRows = false;
            this.cashTransactionsView.AllowUserToDeleteRows = false;
            this.cashTransactionsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.cashTransactionsView, "cashTransactionsView");
            this.cashTransactionsView.Name = "cashTransactionsView";
            this.cashTransactionsView.RowTemplate.Height = 24;
            this.cashTransactionsView.StandardTab = true;
            // 
            // bankTransactionsTabPage
            // 
            this.bankTransactionsTabPage.Controls.Add(this.bankTransactionsView);
            resources.ApplyResources(this.bankTransactionsTabPage, "bankTransactionsTabPage");
            this.bankTransactionsTabPage.Name = "bankTransactionsTabPage";
            this.bankTransactionsTabPage.UseVisualStyleBackColor = true;
            // 
            // bankTransactionsView
            // 
            this.bankTransactionsView.AllowUserToAddRows = false;
            this.bankTransactionsView.AllowUserToDeleteRows = false;
            this.bankTransactionsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.bankTransactionsView, "bankTransactionsView");
            this.bankTransactionsView.Name = "bankTransactionsView";
            this.bankTransactionsView.RowTemplate.Height = 24;
            this.bankTransactionsView.StandardTab = true;
            // 
            // clientsTransactionsTabPage
            // 
            this.clientsTransactionsTabPage.Controls.Add(this.clientTransactionsView);
            resources.ApplyResources(this.clientsTransactionsTabPage, "clientsTransactionsTabPage");
            this.clientsTransactionsTabPage.Name = "clientsTransactionsTabPage";
            this.clientsTransactionsTabPage.UseVisualStyleBackColor = true;
            // 
            // clientTransactionsView
            // 
            this.clientTransactionsView.AllowUserToAddRows = false;
            this.clientTransactionsView.AllowUserToDeleteRows = false;
            this.clientTransactionsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.clientTransactionsView, "clientTransactionsView");
            this.clientTransactionsView.Name = "clientTransactionsView";
            this.clientTransactionsView.RowTemplate.Height = 24;
            this.clientTransactionsView.StandardTab = true;
            // 
            // accountsTabPage
            // 
            this.accountsTabPage.Controls.Add(this.accountsView);
            resources.ApplyResources(this.accountsTabPage, "accountsTabPage");
            this.accountsTabPage.Name = "accountsTabPage";
            this.accountsTabPage.UseVisualStyleBackColor = true;
            // 
            // accountsView
            // 
            this.accountsView.AllowUserToAddRows = false;
            this.accountsView.AllowUserToDeleteRows = false;
            this.accountsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.accountsView, "accountsView");
            this.accountsView.Name = "accountsView";
            this.accountsView.RowTemplate.Height = 24;
            this.accountsView.StandardTab = true;
            // 
            // documentTabPage
            // 
            this.documentTabPage.Controls.Add(this.documentsView);
            resources.ApplyResources(this.documentTabPage, "documentTabPage");
            this.documentTabPage.Name = "documentTabPage";
            this.documentTabPage.UseVisualStyleBackColor = true;
            // 
            // documentsView
            // 
            this.documentsView.AllowUserToAddRows = false;
            this.documentsView.AllowUserToDeleteRows = false;
            this.documentsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.documentsView, "documentsView");
            this.documentsView.Name = "documentsView";
            this.documentsView.RowTemplate.Height = 24;
            this.documentsView.StandardTab = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // flowLayoutPanel3
            // 
            resources.ApplyResources(this.flowLayoutPanel3, "flowLayoutPanel3");
            this.flowLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel3, 2);
            this.flowLayoutPanel3.Controls.Add(this.includeButton);
            this.flowLayoutPanel3.Controls.Add(this.excludeButton);
            this.flowLayoutPanel3.Controls.Add(this.resetButton);
            this.flowLayoutPanel3.Controls.Add(this.moveLftButton);
            this.flowLayoutPanel3.Controls.Add(this.moveRightButton);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            // 
            // includeButton
            // 
            resources.ApplyResources(this.includeButton, "includeButton");
            this.includeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.includeButton.BorderColor = System.Drawing.Color.Black;
            this.includeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.includeButton.Name = "includeButton";
            this.includeButton.UseVisualStyleBackColor = true;
            this.includeButton.Click += new System.EventHandler(this.includeButton_Click);
            // 
            // excludeButton
            // 
            resources.ApplyResources(this.excludeButton, "excludeButton");
            this.excludeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.excludeButton.BorderColor = System.Drawing.Color.Black;
            this.excludeButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.excludeButton.Name = "excludeButton";
            this.excludeButton.UseVisualStyleBackColor = true;
            this.excludeButton.Click += new System.EventHandler(this.excludeButton_Click);
            // 
            // resetButton
            // 
            resources.ApplyResources(this.resetButton, "resetButton");
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.resetButton.BorderColor = System.Drawing.Color.Black;
            this.resetButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.resetButton.Name = "resetButton";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // moveLftButton
            // 
            resources.ApplyResources(this.moveLftButton, "moveLftButton");
            this.moveLftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.moveLftButton.BorderColor = System.Drawing.Color.Black;
            this.moveLftButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.moveLftButton.Name = "moveLftButton";
            this.moveLftButton.UseVisualStyleBackColor = true;
            this.moveLftButton.Click += new System.EventHandler(this.moveLeftButton_Click);
            // 
            // moveRightButton
            // 
            resources.ApplyResources(this.moveRightButton, "moveRightButton");
            this.moveRightButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.moveRightButton.BorderColor = System.Drawing.Color.Black;
            this.moveRightButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.moveRightButton.Name = "moveRightButton";
            this.moveRightButton.UseVisualStyleBackColor = true;
            this.moveRightButton.Click += new System.EventHandler(this.moveRightButton_Click);
            // 
            // DataExchangeForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "DataExchangeForm";
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.clientsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientView)).EndInit();
            this.DeadlinesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deadlinesView)).EndInit();
            this.representativesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advisorView)).EndInit();
            this.employeesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.employeesView)).EndInit();
            this.pettyCashTransactionsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.officeCashTransactionsView)).EndInit();
            this.cashTransactionsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cashTransactionsView)).EndInit();
            this.bankTransactionsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bankTransactionsView)).EndInit();
            this.clientsTransactionsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientTransactionsView)).EndInit();
            this.accountsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accountsView)).EndInit();
            this.documentTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentsView)).EndInit();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FormControls.TableLayoutPanel tableLayoutPanel3;
        private FormControls.Label label19;
        private FormControls.FlowLayoutPanel flowLayoutPanel1;
        private FormControls.Button exportButton;
        private FormControls.Button backButton;
        private FormControls.TableLayoutPanel tableLayoutPanel1;
        private FormControls.DataGridView cashTransactionsView;
        private FormControls.DataGridView bankTransactionsView;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage clientsTabPage;
        private FormControls.DataGridView clientView;
        private System.Windows.Forms.TabPage representativesTabPage;
        private FormControls.DataGridView advisorView;
        private System.Windows.Forms.TabPage employeesTabPage;
        private FormControls.DataGridView employeesView;
        private System.Windows.Forms.TabPage pettyCashTransactionsTabPage;
        private FormControls.DataGridView officeCashTransactionsView;
        private System.Windows.Forms.TabPage cashTransactionsTabPage;
        private System.Windows.Forms.TabPage bankTransactionsTabPage;
        private FormControls.Label label1;
        private FormControls.Button includeButton;
        private FormControls.Button excludeButton;
        private FormControls.Button resetButton;
        private FormControls.Button moveLftButton;
        private FormControls.Button moveRightButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.TabPage DeadlinesTabPage;
        private FormControls.DataGridView deadlinesView;
        private System.Windows.Forms.TabPage clientsTransactionsTabPage;
        private System.Windows.Forms.TabPage accountsTabPage;
        private FormControls.DataGridView clientTransactionsView;
        private FormControls.DataGridView accountsView;
        private System.Windows.Forms.TabPage documentTabPage;
        private FormControls.DataGridView documentsView;
    }
}