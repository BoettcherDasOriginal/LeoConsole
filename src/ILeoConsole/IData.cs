using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole.Core;
using ILeoConsole.Plugin;

namespace ILeoConsole
{
    public interface IData
    {
        public User User { get; set; }
        public string SavePath { get; set; }
        public string DownloadPath { get; set; }
        public string Version { get; set; }
    }
}
