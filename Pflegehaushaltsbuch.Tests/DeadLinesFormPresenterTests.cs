using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pflegehaushaltsbuch.Forms.Presenters;

namespace Pflegehaushaltsbuch.Tests
{
    [TestClass]
    public class DeadLinesFormPresenterTests
    {
        [TestMethod]
        public void BuildDeadlineDatesReturnsSelectedMonthWhenForAllMonthsIsFalse()
        {
            var dates = DeadLinesFormPresenter.BuildDeadlineDates(new DateTime(2000, 8, 15), false);

            Assert.AreEqual(1, dates.Count);
            Assert.AreEqual(new DateTime(2000, 8, 15), dates[0]);
        }

        [TestMethod]
        public void BuildDeadlineDatesReturnsEveryValidMonthWhenForAllMonthsIsTrue()
        {
            var dates = DeadLinesFormPresenter.BuildDeadlineDates(new DateTime(2000, 8, 15), true);

            Assert.AreEqual(12, dates.Count);
            CollectionAssert.AreEqual(
                Enumerable.Range(1, 12).Select(month => new DateTime(2000, month, 15)).ToList(),
                dates);
        }

        [TestMethod]
        public void BuildDeadlineDatesSkipsMonthsWithoutSelectedDay()
        {
            var dates = DeadLinesFormPresenter.BuildDeadlineDates(new DateTime(2000, 8, 31), true);

            CollectionAssert.DoesNotContain(dates, new DateTime(2000, 2, 29));
            Assert.IsTrue(dates.All(date => date.Day == 31));
            Assert.AreEqual(7, dates.Count);
        }
    }
}
