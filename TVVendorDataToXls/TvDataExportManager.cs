using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace TvVendorDataToXls
{
    public class TvDataExportManager
    {
        public delegate void AccountHandler(AccountEventArgs accountEventArgs);
        public event AccountHandler Notify;

        private List<string>? keysToBeExported;
        private readonly string ExtsToProcess = ".ini";
        private readonly JsonSerializerOptions p_readOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseUpper,
            WriteIndented = true
        };
        private static readonly JsonSerializerOptions m_readOptions = new()
        {
            WriteIndented = true
        };

        public int GetFilesCount(string path)
        {
            int result = 0;
            List<Dictionary<string, Dictionary<string, string>>> pairsList = new();

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (!CheckFileExt(fi.Extension, ExtsToProcess))
                    continue;
                result++;
            }
            return result;
        }

        public void GetKeysToBeExported()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KeysToExport.json");
                keysToBeExported = JsonSerializer.Deserialize<List<string>?>(path, p_readOptions);
            }
            catch (Exception)
            {
                keysToBeExported =
                [
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
                ];
                /// implement Event doing some with errors.
                /// 


                throw;
            }
            
            
            
        }

        public void ConvertIniToXls(string path)
        {
            List<Dictionary<string, Dictionary<string, string>>> pairsList = new();

            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fi in di.GetFiles())
            {
                if (!CheckFileExt(fi.Extension, ExtsToProcess))
                    continue;
                var pairs = ParseIniFile(fi.FullName);
                pairs.First().Value.Add("FILENAME", CutPanelFilename(fi.Name));
                pairsList.Add(pairs);
            }
            WriteIniDataToXls(pairsList);


        }

        private void WriteIniDataToXls(List<Dictionary<string, Dictionary<string, string>>> pairsList)
        {
            if (keysToBeExported != null)
            {
#if DEBUG
                foreach (var item in pairsList) //list
                {
                    foreach (var pair in item) // [Ini]
                    {
                        foreach (var pair1 in pair.Value)
                        {
                            if (keysToBeExported.Contains(pair1.Key))
                                Console.WriteLine($"{pair1.Key} = {pair1.Value}");
                        }
                    }
                }
#endif
                using (SpreadsheetDocument document = SpreadsheetDocument.Create("TvData.xlsx", SpreadsheetDocumentType.Workbook))
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

                    List<String> columns = new List<string>();

                    foreach (var key in keysToBeExported) // шапка
                    {
                        columns.Add(key);
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(key);
                        headerRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(headerRow);

                    foreach (var item in pairsList) //list
                    {
                        Row row = new Row();
                        foreach (var pair in item) // [Ini]
                        {
                            foreach (var pair1 in pair.Value)
                            {
                                if (columns.Contains(pair1.Key))
                                {
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
                        sheetData.AppendChild(row);
                    }

                    workbookPart.Workbook.Save();
                }
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

        public void ConvertJsonPanelToXls(string path)
        {
            List<Exception> exceptions = new List<Exception>();
            List<string> exceptionFiles = new List<string>();

            List<PanelInfo> tvInfoList = new List<PanelInfo>();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var fi in di.GetFiles())
            {
                if (!CheckFileExt(fi.Extension, ExtsToProcess))
                    continue;
                var jsonString = ReadFile(path: fi.FullName);
                try
                {
                    PanelInfo? tvInfo = JsonSerializer.Deserialize<PanelInfo>(jsonString, p_readOptions);

                    if (tvInfo != null)
                    {
                        tvInfo.Filename = CutPanelFilename(fi.Name);
                        tvInfo.PanelParam = CutPanelParam(tvInfo.PanelParam);
#if DEBUG
                        Console.WriteLine($"{nameof(tvInfo.Name)}: {tvInfo.Name}");
                        Console.WriteLine($"{nameof(tvInfo.PanelParam)}: {tvInfo.PanelParam}");
#endif
                        tvInfoList.Add(tvInfo);
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                    exceptionFiles.Add(fi.FullName);
                    Console.WriteLine($"{fi.FullName}........{e.Message}");
                }
            }
            WritePanelXls(tvInfoList);
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var ef in exceptionFiles)
            {

                Console.WriteLine($"ERROR during processing file {ef}");

            }
            Console.ResetColor();
        }

        public void ConvertJsonModelToXls(string path)
        {
            List<Exception> exceptions = new List<Exception>();
            List<string> exceptionFiles = new List<string>();

            List<Root> tvModelInfoList = new List<Root>();
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var fi in di.GetFiles())
            {
                if (!CheckFileExt(fi.Extension, ExtsToProcess))
                    continue;
                var jsonString = ReadFile(path: fi.FullName);
                try
                {
                    Root? tvModelInfo = JsonSerializer.Deserialize<Root>(jsonString, m_readOptions);

                    if (tvModelInfo != null)
                    {
                        tvModelInfo.Filename = CutModelFilename(fi.Name);
                        tvModelInfo.PANEL = tvModelInfo.PANEL != null ? tvModelInfo.PANEL.Split("panel/").Last().Split(".ini").First() : "ERROR";
#if DEBUG
                        Console.WriteLine($"{nameof(tvModelInfo.Products.PROJECT_NAME)}: {tvModelInfo.Products.PROJECT_NAME}");
                        Console.WriteLine($"{nameof(tvModelInfo.Products.RCU_TYPE)}: {tvModelInfo.Products.RCU_TYPE}");
                        Console.WriteLine($"{nameof(tvModelInfo.Products.MANUFACTURER_NAME)}: {tvModelInfo.Products.MANUFACTURER_NAME}");
                        Console.WriteLine($"{nameof(tvModelInfo.Products.CHASSIS_NAME)}: {tvModelInfo.Products.CHASSIS_NAME}");
                        foreach (var demod in tvModelInfo.Drives.DEMOD)
                        {
                            Console.WriteLine(demod.Count);
                            foreach (var d in demod.Values)
                            {
                                Console.WriteLine($"{nameof(d.TUNER_TYPE)}: {d.TUNER_TYPE}");
                                Console.WriteLine($"{nameof(d.SUPPORT)}: {Encoding.Default.GetString(JsonSerializer.SerializeToUtf8Bytes(d.SUPPORT))}");
                            }

                        }
                        Console.WriteLine($"{nameof(tvModelInfo.Apps.SOURCE_SUPPORT)}: {Encoding.Default.GetString(JsonSerializer.SerializeToUtf8Bytes(tvModelInfo.Apps.SOURCE_SUPPORT))}");
#endif
                        tvModelInfoList.Add(tvModelInfo);
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                    exceptionFiles.Add(fi.FullName);
                    Console.WriteLine($"{fi.FullName}........{e.Message}");
                    Notify?.Invoke(new AccountEventArgs($"{fi.FullName}........{e.Message}", ManagerEventType.Error));
                }
                Notify?.Invoke(new AccountEventArgs("", ManagerEventType.Message));
            }
            WriteModelXls(tvModelInfoList);
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var ef in exceptionFiles)
            {

                Console.WriteLine($"ERROR during processing file {ef}");

            }
            Console.ResetColor();
        }

        private void WritePanelXls(List<PanelInfo> tvInfoList)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create("PanelData.xlsx", SpreadsheetDocumentType.Workbook))
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

                List<String> columns = new List<string>();



                PropertyInfo[] props = typeof(PanelInfo).GetProperties();
                foreach (MemberInfo propInfo in props) // шапка
                {
                    columns.Add(propInfo.Name);
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(propInfo.Name);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (var tvInfoItem in tvInfoList)
                {
                    Row row = new Row();
                    foreach (PropertyInfo propInfo in props)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        try
                        {
                            var propValue = propInfo.GetValue(tvInfoItem);

                            if (propValue != null)
                            {
                                cell.CellValue = new CellValue(propValue.ToString());
                            }
                        }
                        catch (Exception e)
                        {
                            cell.CellValue = new CellValue("ERROR");
                            Console.WriteLine($"{propInfo.Name} value is null");
                        }


                        row.AppendChild(cell);
                    }
                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
            }
        }
        private void WriteModelXls(List<Root> tvInfoList)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create("ModelInfo.xlsx", SpreadsheetDocumentType.Workbook))
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

                List<String> columns = new List<string>();

                #region Table header

                PropertyInfo[] props = typeof(Root).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    object[] attributes = prop.GetCustomAttributes(false);
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is ExportToXlsAttribute exportToXlsAttribute)
                        {
                            if (exportToXlsAttribute.ToExport == true)
                            {
                                columns.Add(prop.Name);
                                Cell cell = new Cell();
                                cell.DataType = CellValues.String;
                                cell.CellValue = new CellValue(prop.Name);
                                headerRow.AppendChild(cell);
                            }
                        }
                    }
                }
                #region Products
                props = typeof(Products).GetProperties();

                foreach (PropertyInfo prop in props)
                {
                    object[] attributes = prop.GetCustomAttributes(false);
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is ExportToXlsAttribute exportToXlsAttribute)
                        {
                            if (exportToXlsAttribute.ToExport == true)
                            {
                                columns.Add(prop.Name);
                                Cell cell = new Cell();
                                cell.DataType = CellValues.String;
                                cell.CellValue = new CellValue(prop.Name);
                                headerRow.AppendChild(cell);
                            }
                        }
                    }
                }
                #endregion
                #region Drives
                props = typeof(Drives).GetProperties();
                foreach (PropertyInfo prop in props)
                {

                    object[] attributes = prop.GetCustomAttributes(false);
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is ExportToXlsAttribute exportToXlsAttribute)
                        {
                            if (exportToXlsAttribute.ToExport == true)
                            {
                                columns.Add(prop.Name);
                                Cell cell = new Cell();
                                cell.DataType = CellValues.String;
                                cell.CellValue = new CellValue(prop.Name);
                                headerRow.AppendChild(cell);
                            }
                        }
                        if (attribute is HasExportedPropsAttribute hasExportedPropsAttribute)
                        {
                            var props1 = prop.GetType().GetProperties();
                            foreach (PropertyInfo prop1 in props1)
                            {
                                object[] attributes1 = prop1.GetCustomAttributes(false);
                                foreach (Attribute attribute1 in attributes1)
                                {
                                    if (attribute is ExportToXlsAttribute exportToXlsAttribute1)
                                    {
                                        if (exportToXlsAttribute1.ToExport == true)
                                        {
                                            columns.Add(prop1.Name);
                                            Cell cell = new Cell();
                                            cell.DataType = CellValues.String;
                                            cell.CellValue = new CellValue(prop1.Name);
                                            headerRow.AppendChild(cell);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Apps
                props = typeof(Apps).GetProperties();
                foreach (PropertyInfo prop in props)
                {
                    object[] attributes = prop.GetCustomAttributes(false);
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is ExportToXlsAttribute exportToXlsAttribute)
                        {
                            if (exportToXlsAttribute.ToExport == true)
                            {
                                columns.Add(prop.Name);
                                Cell cell = new Cell();
                                cell.DataType = CellValues.String;
                                cell.CellValue = new CellValue(prop.Name);
                                headerRow.AppendChild(cell);
                            }
                        }
                    }
                }

                sheetData.AppendChild(headerRow);
                #endregion
                #endregion

                #region Products
                foreach (var tvInfoItem in tvInfoList)
                {
                    Row row = new Row();
                    foreach (var col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        try
                        {
                            object? propValue = null;
                            foreach (var item in tvInfoItem.Products.GetType().GetProperties())
                                if (item.Name == col)
                                    propValue = item.GetValue(tvInfoItem.Products);
                            foreach (var item in tvInfoItem.Apps.GetType().GetProperties())
                                if (item.Name == col)
                                    propValue = Encoding.Default.GetString(JsonSerializer.SerializeToUtf8Bytes(item.GetValue(tvInfoItem.Apps)));
                            foreach (var item in tvInfoItem.Drives.GetType().GetProperties())
                                if (item.Name == col)
                                    propValue = Encoding.Default.GetString(JsonSerializer.SerializeToUtf8Bytes(item.GetValue(tvInfoItem.Drives)));
                            foreach (var item in tvInfoItem.GetType().GetProperties())
                                if (item.Name == col)
                                    propValue = item.GetValue(tvInfoItem);

                            if (propValue != null)
                            {
                                cell.CellValue = new CellValue(propValue.ToString());
                            }
                        }
                        catch (Exception e)
                        {
                            cell.CellValue = new CellValue("ERROR");
                        }
                        row.AppendChild(cell);
                    }
                    sheetData.AppendChild(row);
                }
                #endregion

                workbookPart.Workbook.Save();
            }
        }
        private string ReadFile(string path)
        {
            var task = File.ReadAllLines(path);
            for (int i = 0; i < task.Count(); i++)
            {
                if (task[i].Contains("//"))
                    task[i] = task[i].Split("//").First();
            }
            string allText = "";
            foreach (var item in task)
            {
                allText += item.ToString();
            }
            return allText;
        }
        private string CutPanelFilename(string str)
        {
            return str.Split("panel_").Last().Split(".ini").First();

        }
        private string CutModelFilename(string str)
        {
            return str.Split("model_").Last().Split(".ini").First();

        }
        private string CutPanelParam(string str)
        {
            return str.Split('/').Last().Split(".ini").First();

        }
        private bool CheckFileExt(string str, string expected)
        {
            return str.ToLower().Equals(expected.ToLower());
        }
    }

    public class AccountEventArgs
    {
        // Сообщение
        public string Message { get; }
        // Сумма, на которую изменился счет
        public ManagerEventType Type { get; }
        public AccountEventArgs(string message, ManagerEventType type)
        {
            Message = message;
            Type = type;
        }
    }

    public enum ManagerEventType
    {
        Error = 0,
        Message = 1
    }

    public class PanelInfo
    {
        public string Filename { get; set; }
        public string Name { get; set; }                // NAME
        public string PanelParam { get; set; }          // PANEL_PARAM
        /*public string PanelExtend { get; set; }         // PANEL_EXTEND
        public int PanelSize { get; set; }              // PANEL_SIZE
        public int PanelFrequency { get; set; }         // PANEL_FREQUENCY
        public string PQ { get; set; }                  // PQ
        public string WhiteBalanceDefault { get; set; } // WHITE_BALANCE_DEFAULT
        public string Gamma { get; set; }               // GAMMA

        public TvInfoDimming Dimming { get; set; }      // DIMMING
        public TvInfoLvdsSsc LvdsSsc { get; set; }      // LVDS_SSC*/
    }

    public class PanelInfoDimming
    {
        public string Type { get; set; }
        public int Lines { get; set; }
        public int Column { get; set; }
    }

    public class PanelInfoLvdsSsc
    {
        public int Span { get; set; }
        public int Step { get; set; }
    }


    public class Products
    {
        [ExportToXls(true)]
        public string CLIENT_TYPE { get; set; }

        [ExportToXls(true)]
        public string PROJECT_NAME { get; set; }

        [ExportToXls(true)]
        public string PROJECT_VERSION { get; set; }

        [ExportToXls(true)]
        public string RCU_TYPE { get; set; }

        public string PSU_TYPE { get; set; }

        [ExportToXls(true)]
        public string MANUFACTURER_NAME { get; set; }

        [ExportToXls(true)]
        public string CHASSIS_NAME { get; set; }
        [ExportToXls(true)]
        public string LOGO_PATH { get; set; }

        public int LOGO_BRIGHTER { get; set; }
    }

    public class Demod
    {
        public string TUNER_TYPE { get; set; }
        public List<string> SUPPORT { get; set; }
    }

    public class Drives
    {
        [ExportToXls(true)]
        public List<Dictionary<string, object>> AMP_CHIPS { get; set; }
        public string DSP_COEF_PATH { get; set; }

        [ExportToXls(true)]
        public List<Dictionary<string, Demod>> DEMOD { get; set; }
    }

    public class Tvos
    {
        public string DATABASE_USER_DEFAULT { get; set; }
        public string DATABASE_SATELLITE_DEFAULT { get; set; }
        public string DATABASE_DTV_CONFIG { get; set; }
        public List<int> VIDEO_MUTE_COLOR { get; set; }
        public string VIDEO_CHANNEL_CHANGE { get; set; }
        public string PICTURE_MODE { get; set; }
        public string PICTURE_CURVE { get; set; }
        public string BACKLIGHT_PARA { get; set; }
        public string OVERSCAN_PATH { get; set; }
        public string VOLUME_CURVE_PATH { get; set; }
        public string AUDIO_PARA_PATH { get; set; }
        public string EDID { get; set; }
        public List<string> DISABLE { get; set; }
    }

    public class Apps
    {
        [ExportToXls(true)]
        public Dictionary<string, string> SOURCE_SUPPORT { get; set; }
        public List<string> HARDWARE_SUPPORT { get; set; }
        public List<string> COUNTRY { get; set; }
        public List<string> LANGUAGE { get; set; }
        public string TV_SHARE_LIST { get; set; }
        public string USER { get; set; }
        public string HOTEL { get; set; }
        public string FACTORY { get; set; }
    }

    public class Root
    {
        [ExportToXls(true)]
        public string Filename { get; set; }
        public Products Products { get; set; }
        public Drives Drives { get; set; }
        public Tvos Tvos { get; set; } // Changed from "Tvos" to singular for C# naming conventions
        public Apps Apps { get; set; }
        public string SOC { get; set; }

        [ExportToXls(true)]
        public string PANEL { get; set; }
    }

    class ExportToXlsAttribute : Attribute
    {
        public bool ToExport { get; set; }
        public ExportToXlsAttribute(bool b) => ToExport = b;
    }
    class HasExportedPropsAttribute : Attribute { }
}
