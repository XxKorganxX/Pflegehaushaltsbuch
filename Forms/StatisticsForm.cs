using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Statistics Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class StatisticsForm : Pflegehaushaltsbuch.FormControls.Form, IStatisticsFormContract
    {
        private readonly StatisticsFormPresenter presenter;


        /// <summary>
        /// Handles the show Form lifecycle step and applies the related control behavior.
        /// </summary>
        public delegate void OnShowForm(Enums.Forms formEnum, SQLBase sql);
        public event OnShowForm ShowForm;
        private DataTable dealings;
        /// <summary>
        /// Creates a new Statistics Form instance and initializes the required state.
        /// </summary>
        public StatisticsForm()
        {
            InitializeComponent();
            presenter = new StatisticsFormPresenter(this);
        }
        /// <summary>
        /// Handles the load event for statistics Form and updates the related state.
        /// </summary>
        private void StatisticsForm_Load(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
        }
        /// <summary>
        /// Handles the enter event for statistics Form and updates the related state.
        /// </summary>
        private async void StatisticsForm_Enter(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            comboBox.SelectedIndex = 0;
            dateBegin.Date = new DateTime(DateTime.Now.Year, 1, 1);
            dateEnd.Date = new DateTime(DateTime.Now.Year, 12, 31);
            dealings = new DataTable();
            if (comboBox.SelectedIndex == 0)
                await sql.FillAdapterAsync(SQLBase.SELECT.Books, dealings);
            if (comboBox.SelectedIndex == 1)
                await sql.FillAdapterAsync(SQLBase.SELECT.Barge, dealings);
            if (comboBox.SelectedIndex == 2)
                await sql.FillAdapterAsync(SQLBase.SELECT.Bank, dealings);
            UpdateDealings();
        }
        //{
        //    if (Program.DesignMode)
        //        return;
        //}
        /// <summary>
        /// Handles the click event for back Button and updates the related state.
        /// </summary>
        private void backButton_Click(object sender, EventArgs e)
        {
            ShowForm(Enums.Forms.Main, sql);
        }
        /// <summary>
        /// Handles the validating event for date Change and updates the related state.
        /// </summary>
        private void dateChange_Validating(object sender, CancelEventArgs e)
        {
            UpdateDealings();
        }
        /// <summary>
        /// Updates the dealings data and refreshes the related application state.
        /// </summary>
        private void UpdateDealings()
        {
            if (Program.DesignMode)
                return;
            if (dealings == null)
                return;

            DateTime current = new DateTime(dateBegin.Date.Year, dateBegin.Date.Month, 1);
            DateTime next = current.AddMonths(1);
            DateTime end = new DateTime(dateEnd.Date.Year, dateEnd.Date.Month, 1);
            if (current > end)
                return;
            decimal maxAmount = 0;
            Dictionary<DateTime, decimal[]> values = new Dictionary<DateTime, decimal[]>();
            do
            {
                DataRow[] rows = dealings.Rows.OfType<DataRow>()
                    .Where(row => row["date"] != DBNull.Value)
                    .Where(row =>
                    {
                        DateTime date = Convert.ToDateTime(row["date"]);
                        return date >= current && date < next;
                    })
                    .ToArray();
                values[current] = new decimal[] { 0, 0 };
                foreach (DataRow row in rows)
                {
                    decimal value = 0;
                    if (decimal.TryParse(row["amount"].ToString(), out value))
                    {
                        if (value > 0)
                        {
                            values[current][0] += value;
                            maxAmount = Math.Max(maxAmount, values[current][0]);
                        }
                        else if (value < 0)
                        {
                            values[current][1] += Math.Abs(value);
                            maxAmount = Math.Max(maxAmount, values[current][1]);
                        }
                    }
                }
                current = current.AddMonths(1);
                next = next.AddMonths(1);
            }
            while (current <= end);
            if (maxAmount != 0)
            {
                foreach (DateTime key in values.Keys.ToArray())
                {
                    values[key][0] /= maxAmount;
                    values[key][1] /= maxAmount;
                }
            }
            //totalAmountBox.Text = maxAmount.ToString("C");
            barDiagram2.UpdateTable(values, maxAmount);
        }
        /// <summary>
        /// Handles the selected Index Changed event for combo Box and updates the related state.
        /// </summary>
        private async void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;
            if (comboBox.SelectedIndex < 0)
                return;
            dealings = new DataTable();
            if (comboBox.SelectedIndex == 0)
                await sql.FillAdapterAsync(SQLBase.SELECT.Books, dealings);
            if (comboBox.SelectedIndex == 1)
                await sql.FillAdapterAsync(SQLBase.SELECT.Barge, dealings);
            if (comboBox.SelectedIndex == 2)
                await sql.FillAdapterAsync(SQLBase.SELECT.Bank, dealings);
            UpdateDealings();
        }
        /// <summary>
        /// Handles the selected Index Changed event for month Begin Box and updates the related state.
        /// </summary>
        private void monthBeginBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDealings();
        }
        /// <summary>
        /// Handles the value Changed event for all Date Boxes and updates the related state.
        /// </summary>
        private void allDateBoxes_ValueChanged()
        {
            UpdateDealings();
        }
    }
}
