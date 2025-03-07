using System.IO;

public class Program
{
    public static readonly Dictionary<string,string> names = new() { { "BOOT1", "boot1" }, { "BOOT2", "boot2" }, { "RPMB", "rpmb" }, { "USER", "user" }, { "ext_csd", "extcsd" } };

    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine($"Are u sure want to rename all files contain boot1,boot2,rpmb,user,ext_csd in directory {Environment.CurrentDirectory}? (y/n)");
            var line = Console.ReadLine();
            switch (line)
            {
                case "y":
                    Ren(Environment.CurrentDirectory);
                    Console.ReadKey();
                    break;
                default:
                    break;
            }

        }
        if (args.Length == 1)
        {
            if (args[0] == "--help")
            {
                Console.WriteLine("Usage: MassiveRenameTool --path path/to/directory");
            }
            else
                Console.WriteLine("Wrong arguments. type --help");
        }
            if (args.Length == 2) 
            {
                if (args[0] == "--path")
                {
                    try
                    {
                        Ren(args[1]);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

        else
        {
            Console.WriteLine("Wrong arguments. type --help");
        }
    }
    static void Ren(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo fi in di.GetFiles())
        {
            string oldFilename = fi.Name;
            string newFilename = "";
            foreach (var name in names)
            {
                if (oldFilename.Contains(name.Key, StringComparison.OrdinalIgnoreCase))
                {
                    newFilename = fi.Directory.Name + "." + name.Value;
                }
            }
            if (newFilename != "" && newFilename != oldFilename)
            {
                File.Move(fi.FullName, fi.DirectoryName + "/" + newFilename);
                Console.WriteLine($"{oldFilename} renamed to {newFilename}");
            }


        }
    }
}

