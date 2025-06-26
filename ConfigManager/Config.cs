using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvDataExport.Shared
{
    public class Config
    {
        public bool SetFieldsToBeExported { get; set; } = true;
        public string FileExtsToProcess { get; set; } = ".ini";
        public List<KeyItem> IniKeysToExport { get; set; }
        public List<KeyItem> ModelKeysToExport { get; set; }
        public List<KeyItem> PanelKeysToExport { get; set; }

        public Config()
        {
            IniKeysToExport = new List<KeyItem>();
            ModelKeysToExport = new List<KeyItem>();
            PanelKeysToExport = new List<KeyItem>();
        }

        public bool IsModelKeyExportable(string key) =>
            IsKeyExportable(ModelKeysToExport, key);

        public bool IsIniKeyExportable(string key) =>
            IsKeyExportable(IniKeysToExport, key);

        public bool IsPanelKeyExportable(string key) =>
            IsKeyExportable(PanelKeysToExport, key);

        private static bool IsKeyExportable(List<KeyItem> keyList, string key) =>
            keyList.Any(item => item.Label.ToLower() == key.ToLower() && item.IsChecked == true);
    }


}
