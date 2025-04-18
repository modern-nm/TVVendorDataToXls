using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using TvDataExport.Shared;

namespace TvDataExport
{
    public partial class CheckboxForm : Form
    {
        private readonly ConfigManager _configManager = new ConfigManager();
        private Config _config;
        private CheckBox? _lastRightClickedCheckBox;
        //private const string FilePath = "checkboxes.json";
        private List<CheckboxItem> checkboxItems = new();

        public CheckboxForm()
        {
            InitializeComponent();
            LoadCheckboxes();
        }

        private void LoadCheckboxes()
        {
            _config = _configManager.GetConfiguration();

            if (_config?.KeysToExport == null)
                return;

            foreach (var item in _config.KeysToExport)
            {
                var cb = CreateCheckboxControl(item);
                flowLayoutPanel1.Controls.Add(cb);
            }
        }

        private void SaveCheckboxes()
        {
            var newList = new List<CheckboxItem>();

            foreach (var control in flowLayoutPanel1.Controls)
            {
                if (control is CheckBox cb && cb.Tag is CheckboxItem item)
                {
                    newList.Add(new CheckboxItem
                    {
                        Label = cb.Text,
                        IsChecked = cb.Checked
                    });
                }
            }

            _config.KeysToExport = newList;

            string json = JsonSerializer.Serialize(newList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("KeysToExport.json", json);
        }

        private CheckBox CreateCheckboxControl(CheckboxItem item)
        {
            var cb = new CheckBox
            {
                Text = item.Label,
                Checked = item.IsChecked,
                AutoSize = true,
                Tag = item,
                ContextMenuStrip = contextMenuStrip1
            };

            cb.CheckedChanged += (s, e) =>
            {
                if (cb.Tag is CheckboxItem ci)
                    ci.IsChecked = cb.Checked;
            };

            cb.MouseUp += CheckBox_MouseUp;

            return cb;
        }

        private void CheckBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && sender is CheckBox cb)
            {
                _lastRightClickedCheckBox = cb;
            }
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_lastRightClickedCheckBox == null) return;

            if (_lastRightClickedCheckBox.Tag is CheckboxItem item)
            {
                checkboxItems.Remove(item); // Удаляем из списка
                flowLayoutPanel1.Controls.Remove(_lastRightClickedCheckBox); // Удаляем с формы
                _lastRightClickedCheckBox.Dispose();
                _lastRightClickedCheckBox = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCheckboxes();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            LoadCheckboxes();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string newLabel = txtNewItem.Text.Trim();
            if (string.IsNullOrEmpty(newLabel))
            {
                MessageBox.Show("Введите название.");
                return;
            }

            if (checkboxItems.Any(i => i.Label.Equals(newLabel, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Такой чекбокс уже существует.");
                return;
            }

            var newItem = new CheckboxItem { Label = newLabel, IsChecked = false };
            checkboxItems.Add(newItem);

            var cb = CreateCheckboxControl(newItem);
            flowLayoutPanel1.Controls.Add(cb);

            txtNewItem.Clear();
        }
    }
}
