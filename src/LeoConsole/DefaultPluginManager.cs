using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using ILeoConsole.Localization;
using ILeoConsole.Plugin;
using ILeoConsole.Core;

namespace LeoConsole
{
    public class DefaultPluginManager
    {
        static string url = "https://raw.githubusercontent.com/BoettcherDasOriginal/LeoConsole/main/DefaultPkgList.txt";
        static string DirPath = Path.Combine(Commands.consoleData.SavePath, "pkg");
        static string filePKGListName = "DefaultPkgList.txt";
        static Dictionary<string,string> DefaultPlugins;

        public static void UpdateDefaultPlugins()
        {
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_dpkgUpdateStart"));

            if (!Directory.Exists(DirPath)) { Directory.CreateDirectory(DirPath); }

            DefaultPlugins = new Dictionary<string, string>();

            try
            {
                //Dwonload List
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, Path.Combine(DirPath, filePKGListName));

                //Get Default Plugins from List
                foreach (string pkgListLine in File.ReadLines(Path.Combine(DirPath, filePKGListName)))
                {
                    string[] pkgListProperties = pkgListLine.Split(' ');
                    if (pkgListProperties.Length > 0)
                    {
                        string dUrl = null;
                        string dName = null;

                        switch (pkgListProperties[0])
                        {
                            case "pkg":
                                for (int i = 1; i < pkgListProperties.Length; i++)
                                {
                                    string[] pkgProperties = pkgListProperties[i].Split(':');

                                    if (pkgProperties.Length > 0)
                                    {
                                        switch (pkgProperties[0])
                                        {
                                            case "-n":
                                                dName = pkgProperties[1];
                                                break;

                                            case "-d":
                                                dUrl = pkgProperties[1];
                                                break;
                                        }
                                    }
                                }

                                if (dUrl != null) { dUrl = "https://" + dUrl; } else { dUrl = null; }

                                DefaultPlugins.Add(dName, dUrl);

                                dUrl = null;
                                dName = null;

                                break;
                        }
                    }
                }

                //Check if the plugin is already installed
                foreach(IPlugin plugin in PluginLoader.Plugins)
                {
                    if(DefaultPlugins.Count <= 0) { break; }

                    foreach (string name in DefaultPlugins.Keys)
                    {
                        if(plugin.Name == name)
                        {
                            DefaultPlugins.Remove(name);
                        }
                    }
                }

                //Get Disabeled Plugins
                string configPath = Path.Combine(Commands.consoleData.SavePath, "var", "LeoConsole", "config");
                string filePath = Path.Combine(configPath, "dpkg.txt");
                string[] disabeledPlugins;
                if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }
                if (File.Exists(filePath))
                {
                    string configText = File.ReadAllText(filePath);
                    disabeledPlugins = Config.GetCategory(configText, "pkgDisabeled");
                }
                else
                {
                    disabeledPlugins = new string[] { };
                }

                //Download missing Plugins
                int downloadedPkgs = 0;
                foreach (string name in DefaultPlugins.Keys)
                {
                    if (!disabeledPlugins.Contains(name))
                    {
                        string url = null;
                        DefaultPlugins.TryGetValue(name, out url);
                        PKGDownload(url, name);
                        downloadedPkgs++;
                    }
                }

                //End
                if(downloadedPkgs > 0)
                {
                    Console.WriteLine(downloadedPkgs + LocalisationManager.GetLocalizationFromKey("lc_dpkgUpdateEndDown"));
                }
                else
                {
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_dpkgUpdateEndAll"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_dpkgUpdateFail404"));
            }
        }

        static void PKGDownload(string url, string name)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, Path.Combine(Commands.consoleData.SavePath, "plugins", $"{name}.dll"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
