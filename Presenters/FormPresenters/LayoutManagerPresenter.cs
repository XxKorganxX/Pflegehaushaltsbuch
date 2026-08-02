using System;
using Pflegehaushaltsbuch.Forms;

namespace Pflegehaushaltsbuch.Presenters.FormPresenters
{
    public class LayoutManagerPresenter
    {
        public LayoutManagerPresenter(ILayoutManagerContract view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            View = view;
        }

        protected ILayoutManagerContract View { get; private set; }

        public virtual void OnUserRights()
        {
        }

        public virtual void UpdateLayout()
        {
        }

        public virtual void Back()
        {
        }

        public virtual void Size()
        {
        }

        public virtual void Image()
        {
        }

        public virtual void Text()
        {
        }

        public virtual void Line()
        {
        }

        public virtual void Arc()
        {
        }

        public virtual void Join()
        {
        }

        public virtual void Seperat()
        {
        }

        public virtual void Table()
        {
        }

        public virtual void Save()
        {
        }

        public virtual void Import()
        {
        }

        public virtual void Export()
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void UpdateFont()
        {
        }

        public virtual void UpdateFonSelectedItems()
        {
        }

        public virtual void FontButtonsChanged()
        {
        }

        public virtual void Horizontal()
        {
        }

        public virtual void Vertical()
        {
        }

        public virtual void BorderColor()
        {
        }

        public virtual void BackColor()
        {
        }

        public virtual void ForeColor()
        {
        }

        public virtual void Edit()
        {
        }

        public virtual void CalculateTextHeight()
        {
        }

        public virtual void Copy()
        {
        }

        public virtual void Insert()
        {
        }

        public virtual void Print()
        {
        }

        public virtual void CopyToolStripMenuItem()
        {
        }

        public virtual void InsertToolStripMenuItem()
        {
        }

        public virtual void EditToolStripMenuItem()
        {
        }

        public virtual void EditControl()
        {
        }

        public virtual void PageSettings()
        {
        }

        public virtual void Cash()
        {
        }

        public virtual void Bank()
        {
        }

        public virtual void OfficeCash()
        {
        }

        public virtual void Statement()
        {
        }

        public virtual void Client()
        {
        }

        public virtual void Advisor()
        {
        }

        public virtual void Assistant()
        {
        }

        public virtual void CashAudit()
        {
        }

        public virtual void Quittance()
        {
        }

        public virtual void LayoutsOff()
        {
        }
    }
}
