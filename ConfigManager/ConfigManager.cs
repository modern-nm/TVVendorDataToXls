using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvDataExport.Shared;

public class ConfigManager
{
    private string ConfigFilename = "App.config";
    private string KeysToExportFilename = "KeysToExport.json";


    public Config GetConfiguration()
    {
        JsonSerializerOptions p_readOptions = new()
        {
            //PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper,
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
            config.KeysToExport = JsonSerializer.Deserialize<List<CheckboxItem>>(text, p_readOptions);
        }
        catch (Exception)
        {
            InitConfiguration();
        }


        return config;
    }
    public void SaveConfiguration()
    {

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
        List<string> keys = new List<string>()
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
        var keysToExport = new List<CheckboxItem>();
        foreach (var key in keys) 
        {
            keysToExport.Add(new() { Label = key, IsChecked = true });
        }

        text = JsonSerializer.Serialize(keysToExport, new JsonSerializerOptions() { WriteIndented = true });
        File.WriteAllText(KeysToExportFilename, text);
    }
}


