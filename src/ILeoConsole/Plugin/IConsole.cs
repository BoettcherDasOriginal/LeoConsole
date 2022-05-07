using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Plugin
{
    public interface IConsole
    {
        /// <summary>
        /// Writes the specified string value behind the user name
        /// </summary>
        string PluginTextAfterInput { get; set; }

        /// <summary>
        /// Executes the given input like user input
        /// </summary>
        string Execute { get; set; }
    }
}