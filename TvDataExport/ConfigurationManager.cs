using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvDataExport
{
    class ConfigManager
    {
        private string ConfigFilename = "App.config";
        private string KeysToExportFilename = "KeysToExport.json";


        public Config GetConfiguration()
        {
            var config = new Config();
            var fileExtsToProcess = ConfigurationManager.AppSettings["fileExtsToProcess"];

            if (fileExtsToProcess != null)
                config.FileExtsToProcess = fileExtsToProcess;
            else
                config.FileExtsToProcess = ".ini";
            bool setFieldsToBeExported = true;
            if (bool.TryParse(ConfigurationManager.AppSettings["setFieldsToBeExported"], out setFieldsToBeExported))
                config.SetFieldsToBeExported = setFieldsToBeExported;

            return config;
        }
        public void WriteConfiguration()
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


        }
    }

    public class Config
    {
        public bool SetFieldsToBeExported { get; set; } = true;
        public string FileExtsToProcess { get; set; } = ".ini";
    }
}


