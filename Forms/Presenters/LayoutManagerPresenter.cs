using Pflegehaushaltsbuch.Data.Graphics;
using Pflegehaushaltsbuch.Databases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LayoutEnum = Pflegehaushaltsbuch.Data.Printing.LayoutEnum;

namespace Pflegehaushaltsbuch.Forms.Presenters
{
    public class LayoutManagerPresenter
    {
        public SqlSession session { get; private set; }

        private readonly SemaphoreSlim databaseOperationLock = new SemaphoreSlim(1, 1);
        private GraphicsItem[] selectedItems = new GraphicsItem[0];
        private ILayoutSurface currentLayout;
        private LayoutEnum currentLayoutType = LayoutEnum.cash;
        private bool initialized;

        public LayoutManagerPresenter(ILayoutManagerContract view, SqlSession session)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            View = view;
            this.session = session;
        }

        protected ILayoutManagerContract View { get; private set; }

        public virtual void InitializeControls()
        {
            View.BindStyles(Enum.GetValues(typeof(DashStyle)).Cast<DashStyle>());
            View.BindFontFamilies(FontFamily.Families.Select(a => a.Name));

            List<int> fontSizes = new List<int>();
            for (int i = 1; i < 200; i++)
                fontSizes.Add(i);
            View.BindFontSizes(fontSizes);
        }

        //public virtual void OnUserRights(int access, bool admin, bool supervisor)
        //{
        //    View.SetSaveEnabled(admin | supervisor);
        //}

        public virtual void Enter()
        {
            if (!session.IsConnected)
            {
                View.ShowDatabaseNotAvailable();
                View.ShowMainForm();
                return;
            }

            if (initialized)
                return;

            initialized = true;
            ResetDocuments();
            currentLayout = View.GetLayout(LayoutEnum.cash);
            View.SelectedPageIndex = 0;
            View.SelectedPrintPageIndex = 0;
            View.SelectedFontFamily = "Arial";
            View.SelectedFontSize = 10;

            View.RectangleChanged += OnChangeRectangle;
            View.SelectedItemsChanged += OnUpdateSelectedItem;
            View.LayoutVisibleChanged += LayoutVisibleChanged;

            foreach (ILayoutSurface layer in View.Layouts)
            {
                layer.Session = session;
            }
        }

        public virtual void LayoutVisibleChanged(ILayoutSurface layout)
        {
            if (layout == null)
                return;

            selectedItems = new GraphicsItem[0];
            layout.ClearSelectedItems();
            if (currentLayout != null)
            {
                layout.Back = currentLayout.Back;
                layout.Fore = currentLayout.Fore;
                layout.Border = currentLayout.Border;
                layout.CurrentFont = currentLayout.CurrentFont;
                layout.Horizontal = currentLayout.Horizontal;
                layout.Vertical = currentLayout.Vertical;
                layout.BorderWidth = currentLayout.BorderWidth;
                layout.PrintOnPage = currentLayout.PrintOnPage;
                layout.CurrentPage = currentLayout.CurrentPage;
            }
            else
            {
                layout.CurrentPage = View.SelectedPageIndex;
                layout.Back = View.SelectedBackColor;
                layout.Fore = View.SelectedForeColor;
                layout.Border = View.SelectedBorderColor;
            }

            currentLayout = layout;
            UpdateLayout();
        }

        public virtual void UpdateLayout()
        {
            if (currentLayout != null)
                currentLayout.Invalidate();
        }

        public virtual void Back()
        {
            View.ShowAdministrationForm();
        }

        public virtual void ChangeDrawMode(LayoutDrawElement drawElement)
        {
            foreach (ILayoutSurface layout in View.Layouts)
                layout.ChangeDrawMode(drawElement);
        }

        public virtual void Size()
        {
            ChangeDrawMode(LayoutDrawElement.Size);
        }

        public virtual void Image()
        {
            ChangeDrawMode(LayoutDrawElement.Image);
        }

        public virtual void Text()
        {
            ChangeDrawMode(LayoutDrawElement.Text);
        }

        public virtual void Line()
        {
            ChangeDrawMode(LayoutDrawElement.Line);
        }

        public virtual void Arc()
        {
            ChangeDrawMode(LayoutDrawElement.Arc);
        }

        public virtual void Join()
        {
            ChangeDrawMode(LayoutDrawElement.Join);
        }

        public virtual void Seperat()
        {
            foreach (GraphicsItem item in selectedItems)
                item.Disconnect();
        }

        public virtual void Table()
        {
            ChangeDrawMode(LayoutDrawElement.Table);
        }

        public virtual async Task SaveAsync()
        {
            if (!await databaseOperationLock.WaitAsync(0))
                return;

            try
            {
                await session.SQL.Printing.SaveDocuments(session.SQL);
                View.ShowLayoutSaved();
            }
            finally
            {
                databaseOperationLock.Release();
            }
        }

        public virtual void Import()
        {
            string selectedPath;
            if (!View.ShowFolderDialog(out selectedPath))
                return;

            session.SQL.Printing.ImportDocuments(selectedPath);
            Reset();
        }

        public virtual void Export()
        {
            string selectedPath;
            if (!View.ShowFolderDialog(out selectedPath))
                return;

            session.SQL.Printing.ExportDocuments(selectedPath);
            View.ShowLayoutsExported(selectedPath);
        }

        public virtual void ResetWithDialog()
        {
            View.ShowResetLayoutsDialog(session);
            Reset();
        }

        public virtual void Reset()
        {
            foreach (ILayoutSurface layer in View.Layouts)
                layer.ClearSelectedItems();

            ResetDocuments();
            currentLayout = View.GetLayout(LayoutEnum.cash);
            selectedItems = new GraphicsItem[0];
            UpdateLayout();
        }

        public virtual void UpdateFont()
        {
            if (currentLayout == null)
                return;

            FontStyle fontStyle = FontStyle.Regular;
            if (View.BoldChecked)
                fontStyle |= FontStyle.Bold;
            if (View.ItalicChecked)
                fontStyle |= FontStyle.Italic;
            if (View.UnderlineChecked)
                fontStyle |= FontStyle.Underline;
            if (View.StrikeOutChecked)
                fontStyle |= FontStyle.Strikeout;

            Font font = new Font(View.SelectedFontFamily, View.SelectedFontSize, fontStyle, GraphicsUnit.World);
            currentLayout.CurrentFont = font;
        }

        public virtual void UpdateFontSelectedItems()
        {
            foreach (FontItem item in selectedItems.OfType<FontItem>())
                item.Font = currentLayout.CurrentFont;
            UpdateLayout();
        }

        public virtual void FontSelectionChanged(bool focused)
        {
            if (!focused)
                return;

            UpdateFont();
            UpdateFontSelectedItems();
        }

        public virtual void FontStyleChanged()
        {
            UpdateFont();
            UpdateFontSelectedItems();
        }

        public virtual void Horizontal(StringAlignment align)
        {
            View.SetHorizontalAlignment(align);

            foreach (TextItem item in selectedItems.OfType<TextItem>())
                item.HorizontalAlignment = align;
            currentLayout.Horizontal = align;
            UpdateLayout();
        }

        public virtual void Vertical(StringAlignment align)
        {
            View.SetVerticalAlignment(align);

            foreach (TextItem item in selectedItems.OfType<TextItem>())
                item.VerticalAlignment = align;
            currentLayout.Vertical = align;
            UpdateLayout();
        }

        public virtual void StyleChanged(bool focused)
        {
            if (!focused)
                return;

            foreach (GraphicsItem item in selectedItems)
                item.Style = View.SelectedStyle;
            UpdateLayout();
        }

        public virtual void BorderWidthChanged(string text, bool focused)
        {
            if (!focused || selectedItems == null)
                return;

            float value;
            if (!float.TryParse(text, out value))
                return;

            foreach (GraphicsItem item in selectedItems)
                item.BorderWidth = value;
            currentLayout.BorderWidth = value;
            UpdateLayout();
        }

        public virtual void RectangleValueChanged(RectangleProperty property, string text, bool focused)
        {
            if (!focused || selectedItems == null)
                return;

            int value;
            if (!int.TryParse(text, out value))
                return;

            foreach (GraphicsItem item in selectedItems)
            {
                RectangleF rect = item.GetRectangle(currentLayout.CurrentPage);
                if (property == RectangleProperty.X)
                    rect.X = value;
                else if (property == RectangleProperty.Y)
                    rect.Y = value;
                else if (property == RectangleProperty.Width)
                    rect.Width = value;
                else if (property == RectangleProperty.Height)
                    rect.Height = value;

                item.SetRectangle(currentLayout.CurrentPage, rect);
            }

            UpdateLayout();
        }

        public virtual void BorderColor()
        {
            Color color;
            if (!View.ShowColorDialog(out color) || selectedItems == null)
                return;

            View.SelectedBorderColor = color;
            foreach (GraphicsItem item in selectedItems)
                item.BorderColor = color;
            currentLayout.Border = color;
            UpdateLayout();
        }

        public virtual void BackColor()
        {
            Color color;
            if (!View.ShowColorDialog(out color) || selectedItems == null)
                return;

            View.SelectedBackColor = color;
            foreach (GraphicsItem item in selectedItems)
                item.BackColor = color;
            currentLayout.Back = color;
            UpdateLayout();
        }

        public virtual void ForeColor()
        {
            Color color;
            if (!View.ShowColorDialog(out color) || selectedItems == null)
                return;

            View.SelectedForeColor = color;
            foreach (GraphicsItem item in selectedItems)
                item.ForeColor = color;
            currentLayout.Fore = color;
            UpdateLayout();
        }

        public virtual void Edit()
        {
            EditControl();
        }

        public virtual void PrintPageChanged(bool focused)
        {
            if (!focused || currentLayout == null)
                return;

            int selected = View.SelectedPrintPageIndex;
            currentLayout.PrintOnPage = selected;
            foreach (GraphicsItem item in selectedItems)
                item.PrintOn = (GraphicsItem.PrintPage)selected;
            UpdateLayout();
        }

        public virtual void PageChanged()
        {
            if (currentLayout == null)
                return;

            currentLayout.CurrentPage = View.SelectedPageIndex;
            selectedItems = new GraphicsItem[0];
            currentLayout.ClearSelectedItems();
            UpdateLayout();
        }

        public virtual void CalculateTextHeight()
        {
            bool value = View.CalculateTextHeightChecked;
            foreach (TextItem item in selectedItems.OfType<TextItem>())
                item.CalculateTextHeight = value;
        }

        public virtual void Copy()
        {
            if (currentLayout == null)
                return;

            currentLayout.CopyToClipboard();
        }

        public virtual void Insert()
        {
            if (currentLayout == null)
                return;

            currentLayout.InsertFromClipboard();
        }

        public virtual void Print()
        {
            View.PrintLayout(session, currentLayoutType);
        }

        public virtual void EditControl()
        {
            foreach (DataTableItem item in selectedItems.OfType<DataTableItem>())
            {
                DataTable tableDesign;
                if (View.ShowEditDataTableDialog(item.TableDesign, out tableDesign))
                    item.TableDesign = tableDesign;
                return;
            }

            ImageItem[] imageItems = selectedItems.OfType<ImageItem>().ToArray();
            if (imageItems.Length > 0)
            {
                string fileName;
                if (View.ShowImageFileDialog(out fileName))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(fileName);
                    foreach (ImageItem item in imageItems)
                        item.Image = image;
                    UpdateLayout();
                }
                return;
            }

            TextItem[] textItems = selectedItems.OfType<TextItem>().ToArray();
            if (textItems.Length > 0)
            {
                string text = textItems[0].Text;
                string content;
                if (!View.ShowEditTextDialog(session, text, out content))
                    return;

                foreach (TextItem textItem in textItems)
                    textItem.Text = content;
                UpdateLayout();
            }
        }

        public virtual void PageSettings()
        {
            View.ShowPageSettings(session, currentLayout);
        }

        public virtual void SelectLayout(LayoutEnum layoutType)
        {
            LayoutsOff();
            currentLayoutType = layoutType;

            if (layoutType == LayoutEnum.cash)
                currentLayout = View.GetLayout(LayoutEnum.cash);
            else if (layoutType == LayoutEnum.bank)
                currentLayout = View.GetLayout(LayoutEnum.bank);
            else if (layoutType == LayoutEnum.officecash)
                currentLayout = View.GetLayout(LayoutEnum.officecash);
            else if (layoutType == LayoutEnum.accounts)
                currentLayout = View.GetLayout(LayoutEnum.accounts);
            else if (layoutType == LayoutEnum.clients)
                currentLayout = View.GetLayout(LayoutEnum.clients);
            else if (layoutType == LayoutEnum.advisors)
                currentLayout = View.GetLayout(LayoutEnum.advisors);
            else if (layoutType == LayoutEnum.employees)
                currentLayout = View.GetLayout(LayoutEnum.employees);
            else if (layoutType == LayoutEnum.cashaudit)
                currentLayout = View.GetLayout(LayoutEnum.cashaudit);
            else if (layoutType == LayoutEnum.quittance)
                currentLayout = View.GetLayout(LayoutEnum.quittance);

            if (currentLayout != null)
                currentLayout.Visible = true;
        }

        public virtual void LayoutsOff()
        {
            foreach (ILayoutSurface layout in View.Layouts)
                layout.Visible = false;
        }

        private void ResetDocuments()
        {
            View.GetLayout(LayoutEnum.cash).Document = session.SQL.Printing.Layouts[LayoutEnum.cash];
            View.GetLayout(LayoutEnum.bank).Document = session.SQL.Printing.Layouts[LayoutEnum.bank];
            View.GetLayout(LayoutEnum.clients).Document = session.SQL.Printing.Layouts[LayoutEnum.clients];
            View.GetLayout(LayoutEnum.accounts).Document = session.SQL.Printing.Layouts[LayoutEnum.accounts];
            View.GetLayout(LayoutEnum.advisors).Document = session.SQL.Printing.Layouts[LayoutEnum.advisors];
            View.GetLayout(LayoutEnum.employees).Document = session.SQL.Printing.Layouts[LayoutEnum.employees];
            View.GetLayout(LayoutEnum.cashaudit).Document = session.SQL.Printing.Layouts[LayoutEnum.cashaudit];
            View.GetLayout(LayoutEnum.quittance).Document = session.SQL.Printing.Layouts[LayoutEnum.quittance];
            View.GetLayout(LayoutEnum.officecash).Document = session.SQL.Printing.Layouts[LayoutEnum.officecash];
        }

        private void OnUpdateSelectedItem(GraphicsItem[] items)
        {
            selectedItems = items;
            if (items == null || selectedItems.Length == 0)
                return;

            GraphicsItem item = items.Last();
            View.SelectedForeColor = currentLayout.Fore = item.ForeColor;
            View.SelectedBackColor = currentLayout.Back = item.BackColor;
            View.SelectedBorderColor = currentLayout.Border = item.BorderColor;
            View.SetBorderWidthText(item.BorderWidth.ToString("f2"));
            View.SelectedStyle = item.Style;
            currentLayout.BorderWidth = item.BorderWidth;

            RectangleF rect = item.GetRectangle(currentLayout.CurrentPage);
            View.SetRectangleValues(rect);
            currentLayout.PrintOnPage = View.SelectedPrintPageIndex = (int)item.PrintOn;

            FontItem fontItem = item as FontItem;
            if (fontItem != null)
            {
                Font font = fontItem.Font;
                currentLayout.CurrentFont = font;
                View.SelectedFontFamily = font.Name;
                View.SelectedFontSize = (int)font.Size;
                View.BoldChecked = font.Bold;
                View.ItalicChecked = font.Italic;
                View.UnderlineChecked = font.Underline;
                View.StrikeOutChecked = font.Strikeout;
            }

            TextItem textItem = item as TextItem;
            if (textItem != null)
            {
                currentLayout.Horizontal = textItem.HorizontalAlignment;
                currentLayout.Vertical = textItem.VerticalAlignment;
                currentLayout.CalculateTextHeight = textItem.CalculateTextHeight;
                SetHorizontalAlignmentSelection(textItem.HorizontalAlignment);
                SetVerticalAlignmentSelection(textItem.VerticalAlignment);
            }
        }

        private void OnChangeRectangle(RectangleF p)
        {
            View.SetRectangleValues(p);
        }

        private void SetHorizontalAlignmentSelection(StringAlignment alignment)
        {
            View.SetHorizontalAlignment(alignment);
        }

        private void SetVerticalAlignmentSelection(StringAlignment alignment)
        {
            View.SetVerticalAlignment(alignment);
        }
    }

    public enum RectangleProperty
    {
        X,
        Y,
        Width,
        Height
    }
}
