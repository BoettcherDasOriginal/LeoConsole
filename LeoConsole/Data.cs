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
        public string SavePath = "data/";
        public string DownloadPath = "data/tmp/";

        public string version = "1.1.1";
    }

    [System.Serializable]
    public class User
    {
        public string name;
        public string begrüßung;
        public string password;
        public bool root;
    }
}
