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
        /// <summary>
        /// The current logged in user
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The default save path
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// The default download path
        /// </summary>
        public string DownloadPath { get; set; }

        /// <summary>
        /// The current workin directory
        /// </summary>
        string CurrentWorkingPath { get; set; }

        /// <summary>
        /// The current running version of LeoConsole
        /// </summary>
        public string Version { get; set; }
    }
}
