using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Pflegehaushaltsbuch.Data;
using Pflegehaushaltsbuch.FormControls;
using Pflegehaushaltsbuch.Presenters.FormPresenters;
namespace Pflegehaushaltsbuch.Forms
{
    /// <summary>
    /// Represents the Page Settings Form window and coordinates its user interface behavior.
    /// </summary>
    public partial class PageSettingsForm : Pflegehaushaltsbuch.FormControls.Form, IPageSettingsFormContract
    {
        private readonly PageSettingsFormPresenter presenter;


        Layout layout;
        DocumentLayer document;
        /// <summary>
        /// Creates a new Page Settings Form instance and initializes the required state.
        /// </summary>
        public PageSettingsForm(Layout layout)
        {
            InitializeComponent();
            presenter = new PageSettingsFormPresenter(this);
            this.layout = layout;
            this.document = layout.Document;
            List<FontFamily> fontFamilys = new List<FontFamily>();
            foreach (FontFamily fontFamily in FontFamily.Families)
                fontFamilys.Add(fontFamily);
            fontFamilyBox.DataSource = fontFamilys;
            fontFamilyBox.DisplayMember = "Name"; 
            for (int i = 1; i < 100; i++)
                fontSizeBox.Items.Add(i);
            
            PrinterSettings printerSettings = new PrinterSettings();
            
            List<PaperSize> paperSizes = new List<PaperSize>();
            foreach (PaperSize test in printerSettings.PaperSizes)
                paperSizes.Add(test);
            
            paperFormatBox.DisplayMember = "PaperName";
            paperFormatBox.ValueMember = "Kind";
            paperFormatBox.DataSource = paperSizes;
            var pageNumber = document.PageNumber;
            //turnPaperBox.DataBindings.Add("Checked", document, "Landscape", false, DataSourceUpdateMode.OnPropertyChanged);
            
            Binding binding = new Binding("Checked", document, "Landscape", false, DataSourceUpdateMode.OnPropertyChanged);
            binding.Format+=binding_Parse;
            turnPaperBox.DataBindings.Add(binding);
           
            paperFormatBox.DataBindings.Add("SelectedValue", document, "Kind");
            printPageNumberBox.DataBindings.Add("Checked", pageNumber, "Print");
            formatBox.DataBindings.Add("Text", pageNumber, "Text");
            if (pageNumber.Horizontal == StringAlignment.Near)
                leftText.Checked = true;
            else if (pageNumber.Horizontal == StringAlignment.Center)
                centerText.Checked = true;
            else if (pageNumber.Horizontal == StringAlignment.Far)
                rightText.Checked = true;
            if (pageNumber.Vertical == StringAlignment.Near)
                topText.Checked = true;
            else
                bottomText.Checked = true;
            fontFamilyBox.SelectedItem = pageNumber.Font.FontFamily;
            fontSizeBox.SelectedItem = (int)pageNumber.Font.Size;
            boldButton.Checked = pageNumber.Font.Bold;
            italicButton.Checked = pageNumber.Font.Italic;
            strikeButton.Checked = pageNumber.Font.Strikeout;
            underscoreButton.Checked = pageNumber.Font.Underline;
            topBox.DataBindings.Add("Text", pageNumber.Margins, "Top");
            bottomBox.DataBindings.Add("Text", pageNumber.Margins, "Bottom");
            leftBox.DataBindings.Add("Text", pageNumber.Margins, "Left");
            rightBox.DataBindings.Add("Text", pageNumber.Margins, "Right");
        }
        void binding_Parse(object sender, ConvertEventArgs e)
        {
            layout.Size = layout.Document.GetSize();
        }
        /// <summary>
        /// Handles the click event for ok Button and updates the related state.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            Margins m = document.PageNumber.Margins;
            /*
            PrinterSettings printerSettings = new PrinterSettings();
            IQueryable<PaperSize> paperSizes = printerSettings.PaperSizes.Cast<PaperSize>().AsQueryable();
            PaperSize paperSize = paperSizes.Where(userSize => userSize.Kind == document.Kind).FirstOrDefault();
            
            document.PageNumber.Rect = new RectangleF(
                m.Left, 
                m.Top, 
                paperSize.Width - m.Right - m.Left, 
                paperSize.Height-m.Bottom-m.Top);
            */
            Close();
        }
        /// <summary>
        /// Handles the validated event for create Font and updates the related state.
        /// </summary>
        private void createFont_Validated(object sender, EventArgs e)
        {
            FontStyle style = new FontStyle();
            if(boldButton.Checked)
                style |= FontStyle.Bold;
            if(italicButton.Checked)
                style |= FontStyle.Italic;
            if(strikeButton.Checked)
                style |= FontStyle.Strikeout;
            if(underscoreButton.Checked)
                style |= FontStyle.Underline;
            document.PageNumber.Font = new Font(fontFamilyBox.SelectedItem as FontFamily, (int)fontSizeBox.SelectedItem, style, GraphicsUnit.World);
        }
        /// <summary>
        /// Handles the selected Index Changed event for paper Format Box and updates the related state.
        /// </summary>
        private void paperFormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaperSize paper = paperFormatBox.SelectedItem as PaperSize;
            if (paper == null)
                return;
            layout.Document.Size = new Size(paper.Width, paper.Height);
            layout.Size = layout.Document.GetSize();
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
            centerText.Checked = false;
            rightText.Checked = false;
            document.PageNumber.Horizontal = StringAlignment.Near;
        }
        /// <summary>
        /// Handles the click event for center Text and updates the related state.
        /// </summary>
        private void centerText_Click(object sender, EventArgs e)
        {
            leftText.Checked = false;
            rightText.Checked = false;
            document.PageNumber.Horizontal = StringAlignment.Center;
        }
        /// <summary>
        /// Handles the click event for right Text and updates the related state.
        /// </summary>
        private void rightText_Click(object sender, EventArgs e)
        {
            leftText.Checked = false;
            centerText.Checked = false;
            document.PageNumber.Horizontal = StringAlignment.Far;
        }
        /// <summary>
        /// Handles the click event for top Text and updates the related state.
        /// </summary>
        private void topText_Click(object sender, EventArgs e)
        {
            bottomText.Checked = false;
            document.PageNumber.Vertical = StringAlignment.Near;
        }
        /// <summary>
        /// Handles the click event for bottom Text and updates the related state.
        /// </summary>
        private void bottomText_Click(object sender, EventArgs e)
        {
            topText.Checked = false;
            document.PageNumber.Vertical = StringAlignment.Far;
        }
    }
}
