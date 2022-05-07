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
        /// <summary>
        /// The name of your plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The description of your plugin
        /// </summary>
        string Explanation { get; }

        /// <summary>
        /// An interface to the data class from LeoConsole
        /// </summary>
        IData data { get; set; }

        /// <summary>
        /// A list of all your custom commands
        /// </summary>
        List<ICommand> Commands { get; set; }

        /// <summary>
        /// PluginInit() is called when LeoConsole loads your plugin
        /// </summary>
        void PluginInit();

        /// <summary>
        /// PluginMain() is called when LeoConsole has finished the starting process
        /// </summary>
        void PluginMain();

        /// <summary>
        /// PluginShutdown() is called before LeoConsole shuts down
        /// </summary>
        void PluginShutdown();
    }
}