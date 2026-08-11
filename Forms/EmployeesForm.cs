using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Data.Print;
using Pflegehaushaltsbuch.Forms.Presenters;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Assistants Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class EmployeesForm : Form, IEmployeesFormContract
    {
        private readonly EmployeesFormPresenter presenter;

        /// <summary>
        /// Creates a new AssistantsForm view.
        /// </summary>
        public EmployeesForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new EmployeesFormPresenter(this, session);
            view.AutoGenerateColumns = false;
            view.CellPainting += CellPainting;
            view.CellFormatting += CellFormatting;
            this.Enter += EmployeesForm_Enter;
            this.Leave += EmployeesForm_Leave;
        }

        /// <summary>
        /// Handles the user Rights lifecycle step and applies the related control behavior.
        /// </summary>
        public override void ApplyUserRights(UserRights rights)
        {
            if (rights == null)
                return;

            if (rights.IsSupervisor)
            {
                updateButton.Visible = true;
                view.AllowUserToDeleteRows = true;
            }
            createButton.Enabled = rights.CanInsert;
            changeButton.Enabled = payOutButton.Enabled = rights.CanModify;
            deleteButton.Visible = rights.CanDelete;
        }

        /// <summary>
        /// Handles the cell format event.
        /// </summary>
        void CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dateColumn.Index && e.Value != null && e.Value != DBNull.Value)
            {
                DateTime date;
                if (DateTime.TryParse(e.Value.ToString(), out date))
                {
                    e.Value = date.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
            if (e.ColumnIndex == paybackTypeColumn.Index && e.Value != null && e.Value != DBNull.Value)
            {
                e.Value = ((SQLBase.Repayment)Int32.Parse(e.Value.ToString())).GetDisplayName();
                e.FormattingApplied = true;
            }
        }

        /// <summary>
        /// Handles the cell paint event.
        /// </summary>
        void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == activeColumn.Index && e.RowIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected);
                bool isActive = false;
                if (e.Value != null && e.Value != DBNull.Value && !Boolean.TryParse(e.Value.ToString(), out isActive))
                {
                    int value = 0;
                    if (Int32.TryParse(e.Value.ToString(), out value))
                    {
                        isActive = value != 0;
                    }
                }
                Rectangle rect = e.CellBounds;
                rect.Height -= 10;
                rect.X += 5;
                rect.Y += 5;
                rect.Width = rect.Height;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle bounds = rect;
                using (var ellipsePath = new GraphicsPath())
                {
                    ellipsePath.AddEllipse(bounds);
                    using (var brush = new PathGradientBrush(ellipsePath))
                    {
                        brush.CenterPoint = new PointF(bounds.X + bounds.Width / 3f, bounds.Y + bounds.Height / 3f);
                        brush.CenterColor = Color.White;
                        if (isActive)
                            brush.SurroundColors = new[] { Color.Green };
                        else
                            brush.SurroundColors = new[] { Color.Red };
                        e.Graphics.FillEllipse(brush, rect);
                    }
                }
                e.Graphics.DrawEllipse(new Pen(Brushes.Black), rect);
                e.Graphics.SmoothingMode = SmoothingMode.Default;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the employee form enter enter event.
        /// </summary>
        async void EmployeesForm_Enter(object sender, EventArgs e)
        {
            ApplyCurrentUserRights();
            await presenter.ConnectTableToDataBaseAsync();
        }

        /// <summary>
        /// Handles the employee form leave leave event.
        /// </summary>
        void EmployeesForm_Leave(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the click event for create Button and updates the related state.
        /// </summary>
        private async void createButton_Click(object sender, EventArgs e)
        {
            await presenter.CreateAsync();
        }

        /// <summary>
        /// Handles the click event for change Button and updates the related state.
        /// </summary>
        private async void changeButton_Click(object sender, EventArgs e)
        {
            await presenter.ChangeAssistantAsync();
        }

        /// <summary>
        /// Handles the click event for delete Button and updates the related state.
        /// </summary>
        private async void deleteButton_Click(object sender, EventArgs e)
        {
            await presenter.DeleteAsync();
        }

        /// <summary>
        /// Handles the click event for pay Out Button and updates the related state.
        /// </summary>
        private async void payOutButton_Click(object sender, EventArgs e)
        {
            await presenter.PayOutAsync();
        }

        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            presenter.Back();
        }

        /// <summary>
        /// Handles the click event for update Button and updates the related state.
        /// </summary>
        private async void updateButton_Click(object sender, EventArgs e)
        {
            await presenter.UpdateAsync();
        }

        /// <summary>
        /// Handles the click event for print Button and updates the related state.
        /// </summary>
        private void printButton_Click(object sender, EventArgs e)
        {
            presenter.Print();
        }

        /// <summary>
        /// Handles the cell Content Double Click event for view and updates the related state.
        /// </summary>
        private async void view_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            await presenter.ChangeSelectedAssistantAsync(e.RowIndex);
        }

        /// <summary>
        /// Handles the click event for export Button and updates the related state.
        /// </summary>
        private void exportButton_Click(object sender, EventArgs e)
        {
            presenter.Export();
        }

        /// <summary>
        /// Handles the click event for button Import and updates the related state.
        /// </summary>
        private async void buttonImport_Click(object sender, EventArgs e)
        {
            await presenter.ImportAsync();
        }

        /// <summary>
        /// Provides the default sort column value for the presenter.
        /// </summary>
        string IEmployeesFormContract.DefaultSortColumn
        {
            get { return nameColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the currently sorted column name for the presenter.
        /// </summary>
        string IEmployeesFormContract.CurrentSortColumn
        {
            get { return view.SortedColumn == null ? null : view.SortedColumn.DataPropertyName; }
        }

        /// <summary>
        /// Provides the change button enabled value for the presenter.
        /// </summary>
        bool IEmployeesFormContract.ChangeButtonEnabled
        {
            get { return changeButton.Enabled; }
        }

        /// <summary>
        /// Provides the selected assistant id for the presenter.
        /// </summary>
        int? IEmployeesFormContract.SelectedAssistantId
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
        /// Provides the selected assistant name for the presenter.
        /// </summary>
        string IEmployeesFormContract.SelectedAssistantName
        {
            get { return nameBox.Text; }
        }

        /// <summary>
        /// Binds the employee list to the view controls.
        /// </summary>
        void IEmployeesFormContract.BindEmployees(DataView employees)
        {
            view.DataSource = employees;
            nameBox.DataSource = employees;
            nameBox.DisplayMember = nameColumn.DataPropertyName;
        }

        /// <summary>
        /// Clears employee list bindings.
        /// </summary>
        void IEmployeesFormContract.ClearEmployees()
        {
            dateBox.DataBindings.Clear();
            nameBox.DataSource = null;
            view.DataSource = null;
        }

        /// <summary>
        /// Binds the selected employee date field.
        /// </summary>
        void IEmployeesFormContract.BindEmployeeDate(DataView employees)
        {
            dateBox.DataBindings.Clear();
            dateBox.DataBindings.Add("Text", employees, dateColumn.DataPropertyName, true, DataSourceUpdateMode.OnPropertyChanged, "", "dd/MM/yyyy");
        }

        /// <summary>
        /// Shows the current total amount.
        /// </summary>
        void IEmployeesFormContract.SetTotalAmount(string totalAmount)
        {
            totalAmountBox.Text = totalAmount;
        }

        /// <summary>
        /// Prints the employee list.
        /// </summary>
        void IEmployeesFormContract.PrintEmployees(DataRow[] employees)
        {
            PrintBase printer = new PrintBase(Session, Data.Printing.LayoutEnum.employees);
            printer.Print(Text, Text, this, employees);
        }

        /// <summary>
        /// Runs the show create assistant dialog view action for the presenter.
        /// </summary>
        bool IEmployeesFormContract.ShowCreateAssistantDialog(int id, out AssistantInput input)
        {
            using (CreateEmployeesDialog dialog = new CreateEmployeesDialog(id))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new AssistantInput
                {
                    ID = dialog.ID,
                    AssistantName = dialog.AssistantName,
                    Amount = dialog.Amount,
                    Date = dialog.Date
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the show change assistant dialog view action for the presenter.
        /// </summary>
        bool IEmployeesFormContract.ShowChangeAssistantDialog(int id, string name, System.DateTime date, decimal amount, out AssistantInput input)
        {
            using (CreateEmployeesDialog dialog = new CreateEmployeesDialog(id, name, date, amount))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new AssistantInput
                {
                    ID = dialog.ID,
                    AssistantName = dialog.AssistantName,
                    Amount = dialog.Amount,
                    Date = dialog.Date
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the show ioan payback dialog view action for the presenter.
        /// </summary>
        bool IEmployeesFormContract.ShowIoanPaybackDialog(string assistantName, int assistantId, decimal amount, out AssistantPaybackInput input)
        {
            using (Dialoge.IoanPaybackDialog dialog = new Dialoge.IoanPaybackDialog(Session, assistantName, assistantId, amount))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    input = null;
                    return false;
                }

                input = new AssistantPaybackInput
                {
                    AssistantName = dialog.AssistantName,
                    AssistantId = dialog.AssistantId,
                    PaybackDate = dialog.PaybackDate,
                    Amount = dialog.Amount,
                    RepaymentIndex = dialog.RepaymentIndex,
                    Repayment = dialog.Repayment
                };
                return true;
            }
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IEmployeesFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }
    }
}
