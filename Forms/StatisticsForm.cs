using System;
using System.ComponentModel;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.Forms.Presenters;
using System.Collections.Generic;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Statistics Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class StatisticsForm : Form, IStatisticsFormContract
    {
        private readonly StatisticsFormPresenter presenter;

        /// <summary>
        /// Creates a new StatisticsForm view.
        /// </summary>
        public StatisticsForm(SqlSession session)
        {
            InitializeComponent();
            Session = session;
            presenter = new StatisticsFormPresenter(this, session);
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

            ApplyCurrentUserRights();
            await presenter.EnterAsync();
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
            presenter.Back();
        }
        /// <summary>
        /// Handles the validating event for date Change and updates the related state.
        /// </summary>
        private void dateChange_Validating(object sender, CancelEventArgs e)
        {
            presenter.UpdateDealings();
        }
        /// <summary>
        /// Handles the selected Index Changed event for combo Box and updates the related state.
        /// </summary>
        private async void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Program.DesignMode)
                return;

            await presenter.StatisticSelectionChangedAsync();
        }
        /// <summary>
        /// Handles the selected Index Changed event for month Begin Box and updates the related state.
        /// </summary>
        private void monthBeginBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.UpdateDealings();
        }
        /// <summary>
        /// Handles the value Changed event for all Date Boxes and updates the related state.
        /// </summary>
        private void allDateBoxes_ValueChanged()
        {
            presenter.DateChanged();
        }

        /// <summary>
        /// Provides the selected statistic index value for the presenter.
        /// </summary>
        int IStatisticsFormContract.SelectedStatisticIndex
        {
            get { return comboBox.SelectedIndex; }
            set { comboBox.SelectedIndex = value; }
        }

        /// <summary>
        /// Provides the begin date value for the presenter.
        /// </summary>
        DateTime IStatisticsFormContract.BeginDate
        {
            get { return dateBegin.Date; }
            set { dateBegin.Date = value; }
        }

        /// <summary>
        /// Provides the end date value for the presenter.
        /// </summary>
        DateTime IStatisticsFormContract.EndDate
        {
            get { return dateEnd.Date; }
            set { dateEnd.Date = value; }
        }

        /// <summary>
        /// Runs the update diagram view action for the presenter.
        /// </summary>
        void IStatisticsFormContract.UpdateDiagram(Dictionary<DateTime, decimal[]> values, decimal maxAmount)
        {
            barDiagram2.UpdateTable(values, maxAmount);
        }

        /// <summary>
        /// Runs the show main form view action for the presenter.
        /// </summary>
        void IStatisticsFormContract.ShowMainForm()
        {
            ShowFormEvent(Enums.Forms.Main);
        }
    }
}
