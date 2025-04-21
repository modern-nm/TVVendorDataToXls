using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using TvVendorDataToXls;
using TvDataExport.Shared;

namespace TvDataExport
{
    public partial class Form1 : Form
    {
        private Config Config;
        //private TvVendorDataToXls.TvDataExportManager ExportManager = new();
        public Form1()
        {
            InitializeComponent();
            //ExportManager.Notify += DisplayMessage;
            //ExportManager.Notify += ErrorMessage;
            ConfigManager configManager = new ConfigManager();
            Config = configManager.GetConfiguration();

            if (Config.KeysToExport == null)
            {
                configManager.InitConfiguration();
                Config = configManager.GetConfiguration();
            }


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
            TvVendorDataToXls.TvDataExportManager ExportManager = new();
            progressBar1.Value = 0;
            progressBar1.Maximum = ExportManager.GetFilesCount(textBox1.Text);
            ExportManager.Notify += DisplayMessage;
            ExportManager.Notify += ErrorMessage;

            if (textBox1.Text == "")
                return;
            if (exportIniRadioButton.Checked)
            {
                //ExportManager.GetKeysToBeExported();
                ExportManager.ConvertIniToXls(textBox1.Text);
            }
            if (exportPanelRadioButton.Checked)
                ExportManager.ConvertJsonPanelToXls(textBox1.Text);
            if (exportModelRadioButton.Checked)
                ExportManager.ConvertJsonModelToXls(textBox1.Text);
        }

        private void ErrorMessage(AccountEventArgs accountEventArgs)
        {
            if (accountEventArgs.Type == ManagerEventType.Error)
            {
                logRichTextBox.Text += accountEventArgs.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new CheckboxForm();
            form.ShowDialog(); // Открывает в отдельном окне
        }
    }
}
