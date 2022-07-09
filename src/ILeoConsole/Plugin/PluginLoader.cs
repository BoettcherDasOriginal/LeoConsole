using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using ILeoConsole.Plugin;
using ILeoConsole.Localization;
using ILeoConsole.Core;

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

            GC.Collect(); // collects all unused memory
            GC.WaitForPendingFinalizers(); // wait until GC has finished its work
            GC.Collect();
        }

        public void LoadPlugin(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (!IsAssemblyLoaded(fileInfo.Name.Replace(".dll", "")))
            {
                Assembly.Load(File.ReadAllBytes(Path.GetFullPath(fileName)));
            }
            
            LoadPlugins();
        }

        void GetDlls(string path)
        {
            //Get Disabeled DLLs
            string configPath = Path.Combine(path, "..", "var","LeoConsole","config");
            string filePath = Path.Combine(configPath, "pkg.txt");
            string[] disabeledDlls;
            if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }
            if (File.Exists(filePath))
            {
                string configText = File.ReadAllText(filePath);
                disabeledDlls = Config.GetCategory(configText, "pkgDisabeled");
            }
            else
            {
                disabeledDlls = new string[] { };
            }

            //Get Dll
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (file.EndsWith(".dll"))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if (!disabeledDlls.Contains(fileInfo.Name.Replace(".dll","")))
                        {
                            if (!IsAssemblyLoaded(fileInfo.Name.Replace(".dll", "")))
                            {
                                Assembly.Load(File.ReadAllBytes(Path.GetFullPath(file)));
                            }
                        }
                    }
                }

                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    GetDlls(Path.GetFullPath(dir));
                }
            }
        }

        bool IsAssemblyLoaded(string fullName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName == fullName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}