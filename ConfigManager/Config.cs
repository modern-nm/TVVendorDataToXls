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
        public List<KeyItem>? KeysToExport { get; set; }
    }
}
