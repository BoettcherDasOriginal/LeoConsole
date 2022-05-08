using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using ILeoConsole.Plugin;
using ILeoConsole.Localization;

namespace ILeoConsole.Plugin
{
    public class PluginLoader
    {
        string PluginFolder;

        public PluginLoader(string savepath)
        {
            PluginFolder = savepath;
        }

        public static List<IPlugin> Plugins { get; set; }
        public static List<IConsole> Consoles { get; set; }

        public void LoadPlugins()
        {
            Plugins = new List<IPlugin>();
            Consoles = new List<IConsole>();

            //Load the DLLs from the Plugins directory
            GetDlls(PluginFolder);

            Type interfaceType = typeof(IPlugin);
            //Fetch all types that implement the interface IPlugin and are a class
            Type[] types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
                .ToArray();
            foreach (Type type in types)
            {
                //Create a new instance of all found types
                Plugins.Add((IPlugin)Activator.CreateInstance(type));

                if (typeof(IConsole).IsAssignableFrom(type) && type.IsClass) { Consoles.Add((IConsole)Activator.CreateInstance(type)); }
                if (typeof(ILocalization).IsAssignableFrom(type) && type.IsClass) { LocalisationManager.Localizations.Add((ILocalization)Activator.CreateInstance(type)); }
            }
        }

        void GetDlls(string path)
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        Assembly.LoadFile(Path.GetFullPath(file));
                    }
                }

                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    GetDlls(Path.GetFullPath(dir));
                }
            }
        }
    }
}