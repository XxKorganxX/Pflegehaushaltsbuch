using System;
using System.Collections.Generic;

namespace Pflegehaushaltsbuch.Forms
{
    public interface IStatisticsFormContract
    {
        int SelectedStatisticIndex { get; set; }
        DateTime BeginDate { get; set; }
        DateTime EndDate { get; set; }
        void SetDateRange(DateTime beginDate, DateTime endDate);
        void UpdateDiagram(Dictionary<DateTime, decimal[]> values, decimal maxAmount);
        void ShowMainForm();
        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ConfirmMessage(string msg);
    }
}
