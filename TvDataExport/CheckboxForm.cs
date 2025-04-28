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
        private List<KeyItem> checkboxItems = new();

        public CheckboxForm()
        {
            InitializeComponent();
            InitCheckboxes();
        }
        private void InitCheckboxes()
        {
            _config = _configManager.GetConfiguration();
            ReloadFlow(flowLayoutPanelIni, _config.IniKeysToExport);
            ReloadFlow(flowLayoutPanelModel, _config.ModelKeysToExport);
        }
        private void LoadCheckboxes()
        {
            _config = _configManager.GetConfiguration();

            if (TabControlSettings.SelectedTab == tabPageIni)
                ReloadFlow(flowLayoutPanelIni, _config.IniKeysToExport);
            if (TabControlSettings.SelectedTab == tabPageModel)
                ReloadFlow(flowLayoutPanelModel, _config.ModelKeysToExport);
        }
        private void ReloadFlow(FlowLayoutPanel panel, List<KeyItem> items)
        {
            panel.Controls.Clear();

            foreach (var item in items)
            {
                var cb = new CheckBox
                {
                    Text = item.Label,
                    Checked = item.IsChecked,
                    Tag = item
                };

                cb.CheckedChanged += (s, e) =>
                {
                    if (cb.Tag is KeyItem checkboxItem)
                        checkboxItem.IsChecked = cb.Checked;
                };

                panel.Controls.Add(cb);
            }
        }
        private void SaveCheckboxes()
        {
            // Готовим данные с формы
            var newItems = new List<KeyItem>();
            FlowLayoutPanel panelToUse = null;
            KeysType keysType;

            if (TabControlSettings.SelectedTab == tabPageIni)
            {
                panelToUse = flowLayoutPanelIni;
                keysType = KeysType.Ini;
            }
            else if (TabControlSettings.SelectedTab == tabPageModel)
            {
                panelToUse = flowLayoutPanelModel;
                keysType = KeysType.Model;
            }
            else
            {
                // Неподдерживаемая вкладка
                return;
            }

            foreach (var control in panelToUse.Controls)
            {
                if (control is CheckBox cb && cb.Tag is KeyItem item)
                {
                    newItems.Add(new KeyItem
                    {
                        Label = cb.Text,
                        IsChecked = cb.Checked
                    });
                }
            }

            // Теперь сохраняем только нужную часть
            _configManager.SavePartialConfiguration(keysType, newItems);
        }

        private CheckBox CreateCheckboxControl(KeyItem item)
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
                if (cb.Tag is KeyItem ci)
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

            if (_lastRightClickedCheckBox.Tag is KeyItem item)
            {
                checkboxItems.Remove(item); // Удаляем из списка
                flowLayoutPanelIni.Controls.Remove(_lastRightClickedCheckBox); // Удаляем с формы
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

            var newItem = new KeyItem { Label = newLabel, IsChecked = false };
            checkboxItems.Add(newItem);

            var cb = CreateCheckboxControl(newItem);
            flowLayoutPanelIni.Controls.Add(cb);

            txtNewItem.Clear();
        }
    }
}
