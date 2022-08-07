using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LeoConsole
{
    public class Data
    {
        public string workingPath;
        public string SavePath;
        public string DownloadPath;

        public string version = "2.1.0";

        public Data()
        {
            workingPath = AppDomain.CurrentDomain.BaseDirectory;
            SavePath = Path.Combine(workingPath, "data");
            DownloadPath = Path.Combine(SavePath, "tmp");
        }
    }
}
