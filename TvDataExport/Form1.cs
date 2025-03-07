using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using TvVendorDataToXls;

namespace TvDataExport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pathButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            TvVendorDataToXls.TvDataExport tvDataExport = new();
            if (textBox1.Text == "")
                return;
            if (exportIniRadioButton.Checked)
            {
                tvDataExport.GetKeysToBeExported();
                tvDataExport.ConvertIniToXls(textBox1.Text);
            }
            if (exportPanelRadioButton.Checked)
                tvDataExport.ConvertJsonPanelToXls(textBox1.Text);
            if (exportModelRadioButton.Checked)
                tvDataExport.ConvertJsonModelToXls(textBox1.Text);
        }
    }
}
