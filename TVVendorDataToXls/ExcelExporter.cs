using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using TvDataExport.Shared;
using TvVendorDataToXls.ExportManager;

namespace TvExportRefactoredJson
{
    public static class ExcelExporter
    {
        public static void ExportToExcel<T>(
            List<T> dataList,
            string filePath,
            Func<T, Dictionary<string, string>> selector,
            List<string>? columnOrder = null)
        {
            using var document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook);
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Sheet1"
            };
            sheets.Append(sheet);

            if (dataList.Count == 0)
                return;

            // Build columns
            var firstRow = selector(dataList[0]);
            var columns = columnOrder ?? firstRow.Keys.ToList();

            // Header
            var headerRow = new Row();
            foreach (var col in columns)
            {
                var cell = new Cell
                {
                    DataType = CellValues.String,
                    CellValue = new CellValue(col)
                };
                headerRow.AppendChild(cell);
            }
            sheetData.AppendChild(headerRow);

            // Data
            foreach (var item in dataList)
            {
                var row = new Row();
                var values = selector(item);
                foreach (var col in columns)
                {
                    var cell = new Cell
                    {
                        DataType = CellValues.String,
                        CellValue = new CellValue(values.TryGetValue(col, out var value) ? value : "NOT FOUND")
                    };
                    row.AppendChild(cell);
                }
                sheetData.AppendChild(row);
            }

            workbookPart.Workbook.Save();
        }

        public static List<string> GetExportablePropertyNames(Type type)
        {
            return type.GetProperties()
                .Where(p => p.GetCustomAttribute<ExportToXlsAttribute>()?.ToExport == true)
                .Select(p => p.Name)
                .ToList();
        }

        public static Dictionary<string, string> ExtractExportableValues(object obj, Config config)
        {
            var dict = new Dictionary<string, string>();
            var props = obj.GetType().GetProperties();

            foreach (var prop in props)
            {
                //var attr = prop.GetCustomAttribute<ExportToXlsAttribute>();
                //if (attr?.ToExport != true)
                //    continue;

                if (!config.IsModelKeyExportable(prop.Name))
                    continue;


                var value = prop.GetValue(obj);

                switch (value)
                {
                    case null:
                        dict[prop.Name] = "";
                        break;

                    case string s:
                        dict[prop.Name] = s;
                        break;

                    case IEnumerable<string> stringList:
                        dict[prop.Name] = string.Join(",", stringList);
                        break;

                    case IEnumerable<int> intList:
                        dict[prop.Name] = string.Join(",", intList);
                        break;

                    case Dictionary<string, string> strDict:
                        dict[prop.Name] = string.Join(";", strDict.Select(kv => $"{kv.Key}={kv.Value}"));
                        break;

                    case Dictionary<string, object> objDict:
                        dict[prop.Name] = JsonSerializer.Serialize(objDict);
                        break;

                    case IEnumerable<Dictionary<string, object>> dictList:
                        dict[prop.Name] = JsonSerializer.Serialize(dictList);
                        break;

                    case IEnumerable<object> list:
                        dict[prop.Name] = JsonSerializer.Serialize(list);
                        break;

                    default:
                        if (value.GetType().IsClass && value.GetType().Assembly == typeof(object).Assembly)
                            dict[prop.Name] = value.ToString();
                        else
                        {
                            // Попробуем рекурсивно пройти вложенный объект
                            var nested = ExtractExportableValues(value, config);
                            foreach (var kv in nested)
                                dict[$"{prop.Name}.{kv.Key}"] = kv.Value;
                        }
                        break;
                }
            }

            return dict;
        }

        public static List<Dictionary<string, string>> Extract<T>(T obj, Config config)
        {
            return new List<Dictionary<string, string>> { ExtractExportableValues(obj, config) };
        }

        public static Dictionary<string, string> Merge(List<Dictionary<string, string>> dicts)
        {
            var result = new Dictionary<string, string>();
            foreach (var dict in dicts)
                foreach (var kv in dict)
                    result[kv.Key] = kv.Value;
            return result;
        }
    }
}