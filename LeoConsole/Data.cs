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
        public string DefaultSavePath = "data/";
        public string SavePath;

        public string version = "1.0.2";
    }

    [System.Serializable]
    public class User
    {
        public string name;
        public string begrüßung;
    }
}
