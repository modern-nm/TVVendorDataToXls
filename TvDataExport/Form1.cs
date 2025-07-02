using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using TvDataExport.Shared;
using TvVendorDataToXls.ExportManager;

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

            if (Config.IniKeysToExport == null)
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
        void DisplayMessage(ExportEventArgs accountEventArgs)
        {
            if (accountEventArgs.Type == ManagerEventType.Message)
                progressBar1.Increment(1);
        }
        private void exportButton_Click(object sender, EventArgs e)
        {
            ExportManager ExportManager = new();
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
                ExportManager.ConvertJsonPanelToXls_NEW(textBox1.Text);
            if (exportModelRadioButton.Checked)
                ExportManager.ConvertJsonModelToXls_NEW(textBox1.Text);
        }

        private void ErrorMessage(ExportEventArgs accountEventArgs)
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

        private void button2_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/modern-nm/TVVendorDataToXls";
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // обязательно для .NET Core / .NET 5+
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть ссылку: " + ex.Message);
            }
        }
    }
}
