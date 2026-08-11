using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class PageSettingsFormPresenter
    {
        private readonly ILayoutSurface layout;
        private readonly DocumentLayer document;

        public PageSettingsFormPresenter(IPageSettingsFormContract view, SqlSession session, ILayoutSurface layout)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            View = view;
            this.layout = layout;
            document = layout.Document;
        }

        protected IPageSettingsFormContract View { get; private set; }

        public virtual void Initialize()
        {
            List<FontFamily> fontFamilies = new List<FontFamily>();
            foreach (FontFamily fontFamily in FontFamily.Families)
                fontFamilies.Add(fontFamily);

            View.BindFontFamilies(fontFamilies);
            
            List<int> fontSizes = new List<int>();
            for (int i = 1; i < 100; i++)
                fontSizes.Add(i);
            View.BindFontSizes(fontSizes);

            PrinterSettings printerSettings = new PrinterSettings();
            List<PaperSize> paperSizes = new List<PaperSize>();
            foreach (PaperSize paperSize in printerSettings.PaperSizes)
                paperSizes.Add(paperSize);

            View.BindPaperSizes(paperSizes);

            DocumentLayer.DocumentPageNumber pageNumber = document.PageNumber;
            View.BindDocumentSettings(document);
            View.SetHorizontalSelection(pageNumber.Horizontal);
            View.SetVerticalSelection(pageNumber.Vertical);
            View.SetSelectedFont(pageNumber.Font);
        }

        public virtual void RefreshLayoutSize()
        {
            layout.RefreshDocumentSize();
        }

        public virtual void Ok()
        {
            View.CloseView();
        }

        public virtual void CreateFont()
        {
            FontStyle style = new FontStyle();
            if (View.BoldChecked)
                style |= FontStyle.Bold;
            if (View.ItalicChecked)
                style |= FontStyle.Italic;
            if (View.StrikeChecked)
                style |= FontStyle.Strikeout;
            if (View.UnderlineChecked)
                style |= FontStyle.Underline;

            document.PageNumber.Font = new Font(View.SelectedFontFamily, View.SelectedFontSize, style, GraphicsUnit.World);
        }

        public virtual void PaperFormatChanged()
        {
            PaperSize paper = View.SelectedPaperSize;
            if (paper == null)
                return;

            layout.Document.Size = new Size(paper.Width, paper.Height);
            RefreshLayoutSize();
        }

        public virtual void LeftText()
        {
            document.PageNumber.Horizontal = StringAlignment.Near;
            View.SetHorizontalSelection(StringAlignment.Near);
        }

        public virtual void CenterText()
        {
            document.PageNumber.Horizontal = StringAlignment.Center;
            View.SetHorizontalSelection(StringAlignment.Center);
        }

        public virtual void RightText()
        {
            document.PageNumber.Horizontal = StringAlignment.Far;
            View.SetHorizontalSelection(StringAlignment.Far);
        }

        public virtual void TopText()
        {
            document.PageNumber.Vertical = StringAlignment.Near;
            View.SetVerticalSelection(StringAlignment.Near);
        }

        public virtual void BottomText()
        {
            document.PageNumber.Vertical = StringAlignment.Far;
            View.SetVerticalSelection(StringAlignment.Far);
        }
    }
}
