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
        void DisplayMessage(AccountEventArgs accountEventArgs)
        {
            if (accountEventArgs.Type == ManagerEventType.Message)
                progressBar1.Increment(1);
        }
        private void exportButton_Click(object sender, EventArgs e)
        {
            TvVendorDataToXls.TvDataExportManager exportManager = new();
            progressBar1.Value = 0;
            progressBar1.Maximum = exportManager.GetFilesCount(textBox1.Text);
            exportManager.Notify += DisplayMessage;
            exportManager.Notify += ErrorMessage;
            if (textBox1.Text == "")
                return;
            if (exportIniRadioButton.Checked)
            {
                exportManager.GetKeysToBeExported();
                exportManager.ConvertIniToXls(textBox1.Text);
            }
            if (exportPanelRadioButton.Checked)
                exportManager.ConvertJsonPanelToXls(textBox1.Text);
            if (exportModelRadioButton.Checked)
                exportManager.ConvertJsonModelToXls(textBox1.Text);
        }

        private void ErrorMessage(AccountEventArgs accountEventArgs)
        {
            if (accountEventArgs.Type == ManagerEventType.Error)
            {
                logRichTextBox.Text += accountEventArgs.Message;
            }
        }
    }
}
