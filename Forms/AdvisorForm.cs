using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Windows.Forms;
using System.Data;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Advisor Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class AdvisorForm : Form, IAdvisorFormContract
    {
        private readonly AdvisorFormPresenter presenter;

        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            insertButton.Enabled = rights.CanAccessRepresentatives && rights.CanInsert;
            changeButton.Enabled = rights.CanAccessRepresentatives && rights.CanModify;
            deleteButton.Enabled = rights.CanDelete;
        }

        /// <summary>
        /// Creates a new AdvisorForm view.
        /// </summary>
        public AdvisorForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new AdvisorFormPresenter(this, session);
            view.AutoGenerateColumns = false;
            this.Enter += clientPanel_Enter;
            this.Leave += clientPanel_Leave;
        }

        /// <summary>
        /// Handles the enter event for client Panel and updates the related state.
        /// </summary>
        private async void clientPanel_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.ConnectTableToDataBaseAsync();
        }
        /// <summary>
        /// Handles the leave event for client Panel and updates the related state.
        /// </summary>
        private void clientPanel_Leave(object sender, EventArgs e)
        {
            presenter.DisconnectTable();
        }
        /// <summary>
        /// Handles the click event for create Account Button and updates the related state.
        /// </summary>
        private async void createAccountButton_Click(object sender, EventArgs e)
        {
            await presenter.CreateAccountAsync();
        }
        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private async void changeButton_Click(object sender, EventArgs e)
        {
            await presenter.ChangeAdvisorAsync();
        }
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }
        /// <summary>
        /// Handles the closed lifecycle step and applies the related control behavior.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            presenter.Print();
        }
        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            await presenter.DeleteAsync();
        }

        /// <summary>
        /// Handles the cell Double Click event for view and updates the related state.
        /// </summary>
        private async void view_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            await presenter.ChangeSelectedAdvisorAsync(e.RowIndex);
        }

        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            await presenter.UpdateAsync();
        }

        /// <summary>
        /// Provides the default sort column value for the presenter.
        /// </summary>
        string IAdvisorFormContract.DefaultSortColumn
        {
            get { return nameColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the currently sorted column name for the presenter.
        /// </summary>
        string IAdvisorFormContract.CurrentSortColumn
        {
            get { return view.SortedColumn == null ? null : view.SortedColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the change button enabled value for the presenter.
        /// </summary>
        bool IAdvisorFormContract.ChangeButtonEnabled
        {
            get { return changeButton.Enabled; }
        }

        /// <summary>
        /// Provides the selected advisor position for dialogs.
        /// </summary>
        int IAdvisorFormContract.SelectedAdvisorPosition
        {
            get
            {
                if (view.DataSource == null)
                    return -1;

                return BindingContext[view.DataSource].Position;
            }
        }

        /// <summary>
        /// Provides the selected advisor id for the presenter.
        /// </summary>
        int? IAdvisorFormContract.SelectedAdvisorId
        {
            get
            {
                if (view.SelectedRows.Count == 0)
                    return null;

                DataRowView rowView = view.SelectedRows[0].DataBoundItem as DataRowView;
                if (rowView == null || rowView.Row[idColumn.DataPropertyName] == DBNull.Value)
                    return null;

                return Convert.ToInt32(rowView.Row[idColumn.DataPropertyName]);
            }
        }

        /// <summary>
        /// Binds the advisor list to the view controls.
        /// </summary>
        void IAdvisorFormContract.BindAdvisors(DataView advisors)
        {
            view.DataSource = advisors;
            advisorsBox.DataSource = advisors;
            advisorsBox.DisplayMember = nameColumn.DataPropertyName;
        }

        /// <summary>
        /// Clears advisor list bindings.
        /// </summary>
        void IAdvisorFormContract.ClearAdvisors()
        {
            dateBox.DataBindings.Clear();
            advisorsBox.DataSource = null;
            view.DataSource = null;
        }

        /// <summary>
        /// Binds the selected advisor date field.
        /// </summary>
        void IAdvisorFormContract.BindAdvisorDate(DataView advisors)
        {
            dateBox.DataBindings.Clear();
            dateBox.DataBindings.Add("Text", advisors, dateColumn.DataPropertyName, true, DataSourceUpdateMode.OnPropertyChanged, "", "dd/MM/yyyy");
        }

        /// <summary>
        /// Runs the show create advisor dialog view action for the presenter.
        /// </summary>
        bool IAdvisorFormContract.ShowCreateAdvisorDialog(DataTable table)
        {
            using (CreateAdvisorDialog dialog = new CreateAdvisorDialog(Session, table))
            {
                return dialog.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Runs the show change advisor dialog view action for the presenter.
        /// </summary>
        bool IAdvisorFormContract.ShowChangeAdvisorDialog(DataTable table, int position)
        {
            using (CreateAdvisorDialog dialog = new CreateAdvisorDialog(Session, table, position))
            {
                return dialog.ShowDialog(this) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Prints the advisor list.
        /// </summary>
        void IAdvisorFormContract.PrintAdvisors(DataRow[] advisors)
        {
            PrintBase printer = new PrintBase(Session, Data.Printing.LayoutEnum.advisors);
            printer.Print(Text, Text, this, advisors);
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IAdvisorFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }
    }
}
