using TvVendorDataToXls.ExportManager;

public class Program
{

    public static void Main(string[] args)
    {
        ExportManager tvExport = new ExportManager();

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
                tvExport.ConvertJsonModelToXls_NEW(args[0]);
                if (args.Contains("--panel"))
                tvExport.ConvertJsonPanelToXls_NEW(args[0]);
            if (args.Contains("--ini"))
            {
                tvExport.ConvertIniToXls(args[0]);
            }

        }

    }
}