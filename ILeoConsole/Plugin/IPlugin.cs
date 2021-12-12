using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        string Explanation { get; }
        void PluginMain(string parameters);
    }
}
