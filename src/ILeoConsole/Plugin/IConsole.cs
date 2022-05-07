using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Plugin
{
    public interface IConsole
    {
        string PluginTextAfterInput { get; set; }
        string Execute { get; set; }
    }
}