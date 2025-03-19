using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using System;
using System.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TvVendorDataToXls;

public class Program
{
    //private static List<string>? keysToBeExported;
    //private static readonly string ExtsToProcess = ".ini";

    public static void Main(string[] args)
    {
        TvDataExportManager tvExport = new TvDataExportManager();

        if (args.Length == 0)
        {
            Console.WriteLine("Usage: TvVendorDataToXls path/to/directory [--model/--panel]");
            return;
        }
        if (args.Length == 1)
        {
            tvExport.ConvertJsonModelToXls(args[0]);
        }
        else
        {
            if (args.Contains("--model"))
                tvExport.ConvertJsonModelToXls(args[0]);
            if (args.Contains("--panel"))
                tvExport.ConvertJsonPanelToXls(args[0]);
            if (args.Contains("--ini"))
            {
                tvExport.GetKeysToBeExported();
                tvExport.ConvertIniToXls(args[0]);
            }

        }

    }
}