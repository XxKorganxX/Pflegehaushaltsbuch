using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class StatisticsFormPresenterTests
    {
        [TestMethod]
        public void BuildStatisticValuesGroupsAmountsByMonthAndNormalizesByLargestSide()
        {
            DataTable dealings = CreateDealingsTable();
            dealings.Rows.Add(new DateTime(2026, 1, 5), 100m);
            dealings.Rows.Add(new DateTime(2026, 1, 20), -50m);
            dealings.Rows.Add(new DateTime(2026, 2, 1), -200m);
            dealings.Rows.Add(new DateTime(2026, 3, 1), 25m);

            decimal maxAmount;
            var values = StatisticsFormPresenter.BuildStatisticValues(
                dealings,
                new DateTime(2026, 1, 1),
                new DateTime(2026, 3, 31),
                out maxAmount);

            Assert.AreEqual(200m, maxAmount);
            Assert.AreEqual(3, values.Count);
            Assert.AreEqual(0.5m, values[new DateTime(2026, 1, 1)][0]);
            Assert.AreEqual(0.25m, values[new DateTime(2026, 1, 1)][1]);
            Assert.AreEqual(0m, values[new DateTime(2026, 2, 1)][0]);
            Assert.AreEqual(1m, values[new DateTime(2026, 2, 1)][1]);
            Assert.AreEqual(0.125m, values[new DateTime(2026, 3, 1)][0]);
            Assert.AreEqual(0m, values[new DateTime(2026, 3, 1)][1]);
        }

        [TestMethod]
        public void BuildStatisticValuesReturnsEmptyResultWhenBeginIsAfterEnd()
        {
            decimal maxAmount;
            var values = StatisticsFormPresenter.BuildStatisticValues(
                CreateDealingsTable(),
                new DateTime(2026, 4, 1),
                new DateTime(2026, 3, 31),
                out maxAmount);

            Assert.AreEqual(0m, maxAmount);
            Assert.AreEqual(0, values.Count);
        }

        private static DataTable CreateDealingsTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("date", typeof(DateTime));
            table.Columns.Add("amount", typeof(decimal));
            return table;
        }
    }
}
