using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace TvDataExport.Shared;

public class ConfigManager
{
    private string ConfigFilename = "App.config";
    private string KeysToExportFilename = "KeysToExport.json";


    public Config GetConfiguration()
    {
        JsonSerializerOptions p_readOptions = new()
        {
            WriteIndented = true
        };


        var config = new Config();
        var fileExtsToProcess = ConfigurationManager.AppSettings["fileExtsToProcess"];

        if (fileExtsToProcess != null)
            config.FileExtsToProcess = fileExtsToProcess;
        else
            config.FileExtsToProcess = ".ini";
        bool setFieldsToBeExported = true;
        if (bool.TryParse(ConfigurationManager.AppSettings["setFieldsToBeExported"], out setFieldsToBeExported))
            config.SetFieldsToBeExported = setFieldsToBeExported;

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KeysToExport.json");
        try
        {
            string text = File.ReadAllText(path);
            var keys = JsonSerializer.Deserialize<Dictionary<KeysType,List<KeyItem>>>(text, p_readOptions);
            config.IniKeysToExport = keys[KeysType.Ini];
            config.ModelKeysToExport = keys[KeysType.Model];
            config.PanelKeysToExport = keys[KeysType.Panel];
        }
        catch (Exception)
        {
            InitConfiguration();
        }


        return config;
    }
    public void SaveConfiguration(Config config)
    {
        var keysToExport = new Dictionary<KeysType, List<KeyItem>>
        {
            [KeysType.Ini] = config.IniKeysToExport,
            [KeysType.Model] = config.ModelKeysToExport,
            [KeysType.Panel] = config.PanelKeysToExport
        };

        var text = JsonSerializer.Serialize(keysToExport, new JsonSerializerOptions() { WriteIndented = true });
        File.WriteAllText(KeysToExportFilename, text);
    }
    public void SavePartialConfiguration(KeysType keysType, List<KeyItem> updatedList)
    {
        Dictionary<KeysType, List<KeyItem>> keysToExport;

        if (File.Exists(KeysToExportFilename))
        {
            // Читаем существующий файл
            var text = File.ReadAllText(KeysToExportFilename);
            keysToExport = JsonSerializer.Deserialize<Dictionary<KeysType, List<KeyItem>>>(text)
                           ?? new Dictionary<KeysType, List<KeyItem>>();
        }
        else
        {
            // Если файла нет — создаём новый
            keysToExport = new Dictionary<KeysType, List<KeyItem>>();
        }

        // Обновляем только нужный раздел
        keysToExport[keysType] = updatedList;

        // Сохраняем весь файл заново
        var outputText = JsonSerializer.Serialize(keysToExport, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(KeysToExportFilename, outputText);
    }
    public void InitConfiguration()
    {
        var text =  @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                        <configuration>
	                        <appSettings>
		                        <add key=""setFieldsToBeExported"" value=""false""/>
		                        <add key=""fileExtsToProcess"" value="".ini""/>
	                        </appSettings>
                        </configuration>";
        File.WriteAllText(ConfigFilename, text);

        List<string> iniKeys = new List<string>()
        {
            "FILENAME",
            "PROJECT_NAME",
            "RCU_NAME",
            "PANEL_NAME",
            "PSU_NAME",
            "REGION_NAME",
            "CHASSIS_NAME",
            "MANUFACTURER_NAME",
            "TCL_LOCAL_KEYBOARD",
            "inputSource",
            "ST_AMP_SELECTION",
            "ST_AMP_SUB_SELECTION",
            "DOLBY_AUDIO",
            "DOLBY_AUDIO_FEATURE",
            "CLIENT_TYPE",
            "PowerLogoPath"
        };
        List<string> modelKeys = new()
        {
            "PANEL",
            "SOURCE_SUPPORT",
            "HARDWARE_SUPPORT",
            "AMP_CHIPS",
            "DEMOD",
            "CLIENT_TYPE",
            "PROJECT_NAME",
            "PROJECT_VERSION",
            "RCU_TYPE ",
            "PSU_TYPE",
            "MANUFACTURER_NAME",
            "CHASSIS_NAME",
            "LOGO_PATH"

        };
        List<string> panelKeys = new()
        {
            "NAME",
            "PANEL_PARAM"

        };
        var keysToExport = new Dictionary<KeysType, List<KeyItem>>
        {
            [KeysType.Ini] = iniKeys.Select(label => new KeyItem { Label = label, IsChecked = true }).ToList(),
            [KeysType.Model] = modelKeys.Select(label => new KeyItem { Label = label, IsChecked = true }).ToList(),
            [KeysType.Panel] = panelKeys.Select(label => new KeyItem { Label = label, IsChecked = true }).ToList()
        };

        text = JsonSerializer.Serialize(keysToExport, new JsonSerializerOptions() { WriteIndented = true });
        File.WriteAllText(KeysToExportFilename, text);
    }


}
public enum KeysType
{
    Model,
    Panel,
    Ini,
}


