using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole.Core;
using ILeoConsole;

namespace ILeoConsole.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        string Explanation { get; }
        IData data { get; set; }
        List<ICommand> Commands { get; set; }
        void PluginInit();
        void PluginMain();
        void PluginShutdown();
    }
}