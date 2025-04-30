using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvVendorDataToXls.ExportManager
{
    public partial class ExportManager
    {
        public void ConvertIniToXls(string path)
        {
            DirectoryPath = path;
            List<Dictionary<string, Dictionary<string, string>>> pairsList = new();

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (!CheckFileExt(fi.Extension, Config.FileExtsToProcess))
                    continue;
                var pairs = ParseIniFile(fi.FullName);
                pairs.First().Value.Add("FILENAME", CutPanelFilename(fi.Name));
                pairsList.Add(pairs);
                Notify?.Invoke(new ExportEventArgs("", ManagerEventType.Message));
            }
            WriteIniDataToXls(pairsList);
        }

        private void WriteIniDataToXls(List<Dictionary<string, Dictionary<string, string>>> pairsList)
        {
            var keyList = GetKeylist();
            if (Config.IniKeysToExport == null)
                return;
#if DEBUG
            foreach (var item in pairsList) //list
            {
                foreach (var pair in item) // [Ini]
                {
                    foreach (var pair1 in pair.Value)
                    {
                        if (keyList.Contains(pair1.Key))
                            Console.WriteLine($"{pair1.Key} = {pair1.Value}");
                    }
                }
            }
#endif
            string filePath = Path.Combine(DirectoryPath, $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}_ini_data.xlsx");
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<string> columns = new List<string>();

                foreach (var key in Config.IniKeysToExport) // шапка
                {
                    if(!key.IsChecked)
                        continue;
                    columns.Add(key.Label);
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(key.Label);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (var item in pairsList) //list
                {
                    Row row = new Row();
                    foreach (var col in columns)
                    {
                        bool keyFound = false;
                        foreach (var pair in item) // [Ini]
                        {
                            foreach (var pair1 in pair.Value)
                            {
                                if (col == pair1.Key)
                                {
                                    keyFound = true;
                                    Cell cell = new Cell();
                                    cell.DataType = CellValues.String;
                                    try
                                    {
                                        var propValue = pair1.Value;

                                        if (propValue != null)
                                        {
                                            cell.CellValue = new CellValue(pair1.Value);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        cell.CellValue = new CellValue("ERROR");
                                        Console.WriteLine($" value is null");
                                    }
                                    row.AppendChild(cell);

                                }
                            }
                        }
                        if (!keyFound)
                        {
                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("NOT FOUND");
                            row.AppendChild(cell);
                        }
                    }
                    sheetData.AppendChild(row);
                }
                workbookPart.Workbook.Save();
            }
        }

        Dictionary<string, Dictionary<string, string>> ParseIniFile(string filePath)
        {
            var configuration = new Dictionary<string, Dictionary<string, string>>();

            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"The {filePath} specified INI file does not exist.");

                List<string> lines = File.ReadAllLines(filePath).ToList();
                int currentSectionIndex = -1;
                string sectionName = "";
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i].Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.StartsWith("[", StringComparison.OrdinalIgnoreCase) && line.EndsWith("]"))
                    {
                        sectionName = line.Substring(1, line.Length - 2).Trim();

                        if (!configuration.ContainsKey(sectionName))
                            configuration[sectionName] = new Dictionary<string, string>();

                        currentSectionIndex = i;
                    }
                    else
                    {
                        if (currentSectionIndex != -1)
                        {
                            int keyIndex = line.IndexOf('=');

                            if (keyIndex > 0)
                            {
                                string key = line.Substring(0, keyIndex).Trim();
                                string value = line.Substring(keyIndex + 1).Split(';').First().Trim();

                                if (!configuration[sectionName].ContainsKey(key))
                                    configuration[sectionName][key] = value;
                            }
                        }
                    }
                }

                return configuration;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error during processing {filePath}");
                Console.ResetColor();
                return new Dictionary<string, Dictionary<string, string>>();
            }
        }
    }
}
