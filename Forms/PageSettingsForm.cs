using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.Databases;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Forms.Presenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Page Settings Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class PageSettingsForm : Form, IPageSettingsFormContract
    {
        private readonly PageSettingsFormPresenter presenter;


        /// <summary>
        /// Creates a new PageSettingsForm view.
        /// </summary>
        public PageSettingsForm(SqlSession session, ILayoutSurface layout)
        {
            InitializeComponent();
            Session = session;
            presenter = new PageSettingsFormPresenter(this, session, layout);
            presenter.Initialize();
        }
        /// <summary>
        /// Runs the binding parse action.
        /// </summary>
        void binding_Parse(object sender, ConvertEventArgs e)
        {
            presenter.RefreshLayoutSize();
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            presenter.Ok();
        }
        /// <summary>
        /// Handles the validated event for create Font and updates the related state.
        /// </summary>
        private void createFont_Validated(object sender, EventArgs e)
        {
            presenter.CreateFont();
        }
        /// <summary>
        /// Handles the selected Index Changed event for paper Format Box and updates the related state.
        /// </summary>
        private void paperFormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.PaperFormatChanged();
        }
        /// <summary>
        /// Handles the checked Changed event for turn Paper Box and updates the related state.
        /// </summary>
        private void turnPaperBox_CheckedChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Handles the click event for left Text and updates the related state.
        /// </summary>
        private void leftText_Click(object sender, EventArgs e)
        {
            presenter.LeftText();
        }
        /// <summary>
        /// Handles the click event for center Text and updates the related state.
        /// </summary>
        private void centerText_Click(object sender, EventArgs e)
        {
            presenter.CenterText();
        }
        /// <summary>
        /// Handles the click event for right Text and updates the related state.
        /// </summary>
        private void rightText_Click(object sender, EventArgs e)
        {
            presenter.RightText();
        }
        /// <summary>
        /// Handles the click event for top Text and updates the related state.
        /// </summary>
        private void topText_Click(object sender, EventArgs e)
        {
            presenter.TopText();
        }
        /// <summary>
        /// Handles the click event for bottom Text and updates the related state.
        /// </summary>
        private void bottomText_Click(object sender, EventArgs e)
        {
            presenter.BottomText();
        }

        /// <summary>
        /// Provides the selected font family value for the presenter.
        /// </summary>
        FontFamily IPageSettingsFormContract.SelectedFontFamily
        {
            get { return fontFamilyBox.SelectedItem as FontFamily; }
        }

        /// <summary>
        /// Provides the selected font size value for the presenter.
        /// </summary>
        int IPageSettingsFormContract.SelectedFontSize
        {
            get { return (int)fontSizeBox.SelectedItem; }
        }

        PaperSize IPageSettingsFormContract.SelectedPaperSize
        {
            get { return paperFormatBox.SelectedItem as PaperSize; }
        }

        void IPageSettingsFormContract.BindFontFamilies(System.Collections.Generic.IEnumerable<FontFamily> fontFamilies)
        {
            fontFamilyBox.DataSource = new System.Collections.Generic.List<FontFamily>(fontFamilies);
            fontFamilyBox.DisplayMember = "Name";
        }

        void IPageSettingsFormContract.BindFontSizes(System.Collections.Generic.IEnumerable<int> fontSizes)
        {
            fontSizeBox.Items.Clear();
            foreach (int fontSize in fontSizes)
                fontSizeBox.Items.Add(fontSize);
        }

        void IPageSettingsFormContract.BindPaperSizes(System.Collections.Generic.IEnumerable<PaperSize> paperSizes)
        {
            paperFormatBox.DisplayMember = "PaperName";
            paperFormatBox.ValueMember = "Kind";
            paperFormatBox.DataSource = new System.Collections.Generic.List<PaperSize>(paperSizes);
        }

        void IPageSettingsFormContract.BindDocumentSettings(DocumentLayer document)
        {
            DocumentLayer.DocumentPageNumber pageNumber = document.PageNumber;
            turnPaperBox.DataBindings.Clear();
            paperFormatBox.DataBindings.Clear();
            printPageNumberBox.DataBindings.Clear();
            formatBox.DataBindings.Clear();
            topBox.DataBindings.Clear();
            bottomBox.DataBindings.Clear();
            leftBox.DataBindings.Clear();
            rightBox.DataBindings.Clear();

            Binding landscapeBinding = new Binding("Checked", document, "Landscape", false, DataSourceUpdateMode.OnPropertyChanged);
            landscapeBinding.Format += binding_Parse;
            turnPaperBox.DataBindings.Add(landscapeBinding);
            paperFormatBox.DataBindings.Add("SelectedValue", document, "Kind");
            printPageNumberBox.DataBindings.Add("Checked", pageNumber, "Print");
            formatBox.DataBindings.Add("Text", pageNumber, "Text");
            topBox.DataBindings.Add("Text", pageNumber.Margins, "Top");
            bottomBox.DataBindings.Add("Text", pageNumber.Margins, "Bottom");
            leftBox.DataBindings.Add("Text", pageNumber.Margins, "Left");
            rightBox.DataBindings.Add("Text", pageNumber.Margins, "Right");
        }

        void IPageSettingsFormContract.SetSelectedFont(Font font)
        {
            fontFamilyBox.SelectedItem = font.FontFamily;
            fontSizeBox.SelectedItem = (int)font.Size;
            boldButton.Checked = font.Bold;
            italicButton.Checked = font.Italic;
            strikeButton.Checked = font.Strikeout;
            underscoreButton.Checked = font.Underline;
        }

        /// <summary>
        /// Provides the bold checked value for the presenter.
        /// </summary>
        bool IPageSettingsFormContract.BoldChecked
        {
            get { return boldButton.Checked; }
        }

        /// <summary>
        /// Provides the italic checked value for the presenter.
        /// </summary>
        bool IPageSettingsFormContract.ItalicChecked
        {
            get { return italicButton.Checked; }
        }

        /// <summary>
        /// Provides the strike checked value for the presenter.
        /// </summary>
        bool IPageSettingsFormContract.StrikeChecked
        {
            get { return strikeButton.Checked; }
        }

        /// <summary>
        /// Provides the underline checked value for the presenter.
        /// </summary>
        bool IPageSettingsFormContract.UnderlineChecked
        {
            get { return underscoreButton.Checked; }
        }

        /// <summary>
        /// Runs the set horizontal selection view action for the presenter.
        /// </summary>
        void IPageSettingsFormContract.SetHorizontalSelection(StringAlignment alignment)
        {
            leftText.Checked = alignment == StringAlignment.Near;
            centerText.Checked = alignment == StringAlignment.Center;
            rightText.Checked = alignment == StringAlignment.Far;
        }

        /// <summary>
        /// Runs the set vertical selection view action for the presenter.
        /// </summary>
        void IPageSettingsFormContract.SetVerticalSelection(StringAlignment alignment)
        {
            topText.Checked = alignment == StringAlignment.Near;
            bottomText.Checked = alignment == StringAlignment.Far;
        }

        /// <summary>
        /// Runs the close view action for the presenter.
        /// </summary>
        void IPageSettingsFormContract.CloseView()
        {
            Close();
        }
    }
}
