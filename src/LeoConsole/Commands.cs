using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole;
using ILeoConsole.Core;
using ILeoConsole.Plugin;
using ILeoConsole.Localization;

namespace LeoConsole
{
    #region Features for Commands

    public static class Commands
    {
        public static LeoConsole currrentConsole;
        public static UserFunctions userFunctions;
        public static IData consoleData;
    }

    public class ConsoleData : IData
    {
        private User _User;
        public User User { get { return _User; } set { _User = value; } }
        private string _SavePath;
        public string SavePath { get { return _SavePath; } set { _SavePath = value; } }
        private string _DownloadPath;
        public string DownloadPath { get { return _DownloadPath; } set { _DownloadPath = value; } }
        private string _Version;
        public string Version { get { return _Version; } set { _Version = value; } }
        private string _CurrentWorkingPath;
        public string CurrentWorkingPath { get { return _CurrentWorkingPath; } set { _CurrentWorkingPath = value; } }
    }

    #endregion

    #region Command Tenplates

    public class COMMAND : ICommand
    {
        public string Name { get { return ""; } }
        public string Description { get { return ""; } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            
        }
        public void Help()
        {

        }
    }

    #endregion

    #region System Commands

    public class EMPTY : ICommand
    {
        public string Name { get { return ""; } }
        public string Description { get { return "Empty ;D"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Console.WriteLine("");
        }
        public void Help()
        {
            Console.WriteLine("Yes, i am a command too! ;)");
        }
    }

    public class HELP : ICommand
    {
        public string Name { get { return "help"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_helpCmdInfo"); } }
        public Action CommandFunktion { get { return () => Help(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }

        public void Help()
        {
            foreach(ICommand command in LeoConsole.commands)
            {
                if(command.Name != "")
                {
                    LConsole.WriteLine(command.Name + " => " + command.Description);
                }
            }
        }
    }

    public class UPDATE : ICommand
    {
        public string Name { get { return "update"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_updateCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Update.CheckForUpdate(Commands.currrentConsole.data, "https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", Commands.consoleData.DownloadPath, Commands.currrentConsole.data.version);
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class EXIT : ICommand
    {
        public string Name { get { return "exit"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_exitCmdInfo"); } }
        public Action CommandFunktion { get { return () => Exit(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Exit()
        {
            foreach (IPlugin plugin in PluginLoader.Plugins)
            {
                plugin.PluginShutdown();
            }

            Environment.Exit(0);
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class REBOOT : ICommand
    {
        public string Name { get { return "reboot"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_rebootCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_rebootCmdDialog"));
            Console.Write(">");
            string anser = Console.ReadLine();

            switch (anser.ToLower())
            {
                default:
                    break;

                case "n":
                    break;

                case "":
                case "y":
                    Commands.currrentConsole.reboot();
                    break;
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class RELOAD : ICommand
    {
        public string Name { get { return "reload"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_reloadCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Commands.currrentConsole.reloadPlugins(true);
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class CREDITS : ICommand
    {
        public string Name { get { return "credits"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_creditsCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            LConsole.WriteLine($"LeoConsole v{Commands.currrentConsole.data.version}");
            LConsole.WriteLine("(c) 2021-2022, BoettcherDasOriginal");
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    #endregion

    #region User Commands

    public class LOGOUT : ICommand
    {
        public string Name { get { return "logout"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_logoutCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            LConsole.WriteLine("§4logout\n");

            List<User> users = SaveLoad.LoadUsers(Commands.consoleData.SavePath);

            if (users == null)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadUser404"));
                Commands.currrentConsole.firstStart();
            }
            else
            {
                Commands.currrentConsole.prepareConsole(users);
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class NEWUSERC : ICommand
    {
        public string Name { get { return "newUser"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_newUserCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if (Commands.consoleData.User.root)
            {
                Commands.userFunctions.newKonto(false, SaveLoad.LoadUsers(Commands.consoleData.SavePath));
            }
            else
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_newUserCmdRootE"));
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class WHOAMI : ICommand
    {
        public string Name { get { return "whoami"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_whoamiCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Console.WriteLine($"{LocalisationManager.GetLocalizationFromKey("lc_username")}: " + Commands.consoleData.User.name);
            Console.WriteLine($"{LocalisationManager.GetLocalizationFromKey("lc_greeting")}: " + Commands.consoleData.User.begrüßung);
            Console.WriteLine($"Root: " + Commands.consoleData.User.root.ToString());
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    #endregion

    #region PKG COMMANDS

    #region OLD
    /*
    public class PKGCOMMAND : ICommand
    {
        string url = "https://raw.githubusercontent.com/BoettcherDasOriginal/LeoConsole/main/PackageList.txt";
        string DirPath = Path.Combine(Commands.consoleData.SavePath, "pkg");
        string filePKGListName = "PackageList.txt";

        public string Name { get { return "pkg"; } }
        public string Description { get { return "ruft den §4alten§r Package Manager auf ('pkg get' oder 'pkg update')"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if(_InputProperties.Length > 1)
            {
                switch (_InputProperties[1])
                {
                    case "get":
                        if(_InputProperties.Length > 2) { GetPKG(_InputProperties[2]); } else { Console.WriteLine("Parameter Fehlen!"); }
                        break;

                    case "update":
                        UpdatePKGList();
                        break;

                    default:
                        Console.WriteLine("Der Befehl '" + _InputProperties[1] + "' ist entweder falsch geschrieben oder konnte nicht gefunden werden.");
                        break;
                }
            }
            else { Console.WriteLine("LeoConsole PackageManager v1.0"); }
        }

        void GetPKG(string pkgNameToGet)
        {
            string pkgDownloadUrl = null;
            bool pkgIsFound = false;

            if (!Directory.Exists(DirPath)) { Directory.CreateDirectory(DirPath); }
            if (!File.Exists(Path.Combine(DirPath, filePKGListName))) { Console.WriteLine("Die PackageList konnte nicht gefunden werden!\nVersuche mal 'pkg update'"); return; }

            if (_InputProperties.Length > 1)
            {
                if (!pkgIsFound)
                {
                    foreach (string pkgListLine in File.ReadLines(Path.Combine(DirPath, filePKGListName)))
                    {
                        if (pkgIsFound) 
                        {
                            break; 
                        }

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
                                    if (dName == pkgNameToGet)
                                    {
                                        pkgIsFound = true;
                                        if (dUrl != null) { pkgDownloadUrl = "https://" + dUrl; } else { pkgDownloadUrl = null; }
                                    }

                                    dUrl = null;
                                    dName = null;

                                    break;
                            }
                        }
                    }

                    if (pkgIsFound)
                    {
                        Console.WriteLine($"pkg '{pkgNameToGet}' gefunden!");
                        PKGDownload(pkgDownloadUrl, pkgNameToGet);
                    }
                    else
                    {
                        Console.WriteLine($"pkg '{pkgNameToGet}' konnte nicht gefunden werden!");
                    }
                }
            }
        }

        void PKGDownload(string url, string name)
        {
            Console.WriteLine("Starte PKG Download...");
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, Path.Combine(Commands.consoleData.SavePath, "plugins", $"{name}.dll"));

                Console.WriteLine("pkg erfolgreich Heruntergeladen!");
                Commands.currrentConsole.reloadPlugins(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void UpdatePKGList()
        {
            if (!Directory.Exists(DirPath)) { Directory.CreateDirectory(DirPath); }

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, Path.Combine(DirPath, filePKGListName));

                Console.WriteLine("Die PackageList ist nun aktuell!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Es ist ein Fehler beim updaten der PackageList aufgetaucht!\nBitte versuche es später nocheinmal!");
            }
        }
    }*/
    #endregion

    public class PLUGININFO : ICommand
    {
        public string Name { get { return "pkginfo"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_pkginfoCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            foreach(IPlugin plugin in PluginLoader.Plugins)
            {
                LConsole.WriteLine(plugin.Name + " => " + plugin.Explanation);
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    #endregion

    #region WORKING DIRECTORY COMMANDS

    public class LS : ICommand
    {
        public string Name { get { return "ls"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_lsCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if (_InputProperties.Length < 2)
            {
                ls(LeoConsole.CurrentWorkingPath);
                return;
            }
            for (int i = 1; i < _InputProperties.Length; i++)
            {
                ls(_InputProperties[i]);
                Console.Write("\n");
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }

        private void ls(string directory)
        {
            string path = Path.Combine(LeoConsole.CurrentWorkingPath, directory);

            Console.WriteLine(path + ":");
            try
            {
                foreach (string filename in Directory.GetDirectories(path))
                {
                    string file = Path.GetFileName(filename);

                    if (file.Contains(' '))
                    {
                        LConsole.WriteLine($"§e'{ Path.GetFileName(filename) + "/"}' §r");
                    }
                    else
                    {
                        LConsole.WriteLine($"§e{ Path.GetFileName(filename) + "/"} §r");
                    }
                }
                foreach (string filename in Directory.GetFiles(path))
                {
                    string file = Path.GetFileName(filename);

                    if (file.Contains(' '))
                    {
                        LConsole.WriteLine($"§a'{Path.GetFileName(filename)}' §r");
                    }
                    else
                    {
                        LConsole.WriteLine($"§a{Path.GetFileName(filename)} §r");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
        }
    }

    public class CD : ICommand
    {
        public string Name { get { return "cd"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_cdCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if (_InputProperties.Length > 1)
            {
                string path = string.Empty;
                for (int i = 1; i < _InputProperties.Length; i++)
                {
                    if(i == 1)
                    {
                        path = path + _InputProperties[i];
                    }
                    else
                    {
                        path = path + " " + _InputProperties[i];
                    }
                }

                string newPath = Path.Combine(LeoConsole.CurrentWorkingPath, path);

                if (Directory.Exists(newPath))
                {
                    LeoConsole.CurrentWorkingPath = Path.GetFullPath(newPath);
                }
                else
                {
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_cantFindPathFront")+ path + LocalisationManager.GetLocalizationFromKey("lc_cantFindPathBack"));
                }
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class MKDIR : ICommand
    {
        public string Name { get { return "mkdir"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_mkdirCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if (_InputProperties.Length < 2)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_mkdirCmdPropE"));
                return;
            }
            for (int i = 1; i < _InputProperties.Length; i++)
            {
                mkdir(_InputProperties[i]);
                Console.Write("\n");
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }

        private void mkdir(string directory)
        {
            string path = Path.Combine(LeoConsole.CurrentWorkingPath, directory);

            try
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("created " + path);
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
        }
    }

    public class RMTRASH : ICommand
    {
        public string Name { get { return "rmtrash"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_rmtrashCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if (_InputProperties.Length < 2)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_rmtrashCmdPropE"));
                return;
            }
            for (int i = 1; i < _InputProperties.Length; i++)
            {
                rmtrash(_InputProperties[i]);
                Console.Write("\n");
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }

        private void rmtrash(string path)
        {
            string _path = Path.Combine(LeoConsole.CurrentWorkingPath, path);
            string trashPath = Path.Combine(Commands.consoleData.SavePath, "trash");
            if (!Directory.Exists(trashPath))
            {
                Directory.CreateDirectory(trashPath);
            }

            try
            {
                if (Directory.Exists(_path))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_path);
                    Console.WriteLine(directoryInfo.FullName);
                    bool yes = LConsole.YesNoDialog(LocalisationManager.GetLocalizationFromKey("lc_rmtrashCmdDialogD"), false);
                    if (yes)
                    {
                        Directory.Move(_path, Path.Combine(trashPath, directoryInfo.Name));
                        Console.WriteLine("removed " + directoryInfo.FullName);
                    }
                    else { Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_rmtrashCmdCanceled")); }
                }
                else if (File.Exists(_path))
                {
                    FileInfo fileInfo = new FileInfo(_path);
                    Console.WriteLine(fileInfo.FullName);
                    bool yes = LConsole.YesNoDialog(LocalisationManager.GetLocalizationFromKey("lc_rmtrashCmdDialogF"), false);
                    if (yes)
                    {
                        File.Move(_path, Path.Combine(trashPath, fileInfo.Name));
                        Console.WriteLine("removed " + path);
                    }
                    else { Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_rmtrashCmdCanceled")); }
                }
                else
                {
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_fodDoesNotExist"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
        }
    }

    #endregion

    #region SETTINGS

    public class LANG : ICommand
    {
        public string Name { get { return "lang"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_langCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if(InputProperties.Length > 1)
            {
                LocalisationManager.Language = InputProperties[1];
            }
            else
            {
                LocalisationManager.Language = "en";
            }

            string configPath = Path.Combine(Commands.consoleData.SavePath, "var", "LeoConsole", "config");
            string filePath = Path.Combine(configPath, "user.txt");

            string newConfigText = $"#language:\n{LocalisationManager.Language}\n#end";

            File.WriteAllText(filePath, newConfigText);
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }
    }

    public class DPKG : ICommand
    {
        static string url = "https://raw.githubusercontent.com/BoettcherDasOriginal/LeoConsole/main/DefaultPkgList.txt";
        static string DirPath = Path.Combine(Commands.consoleData.SavePath, "pkg");
        static string filePKGListName = "DefaultPkgList.txt";
        static Dictionary<string, string> DefaultPlugins;

        public string Name { get { return "dpkg"; } }
        public string Description { get { return LocalisationManager.GetLocalizationFromKey("lc_dpkgCmdInfo"); } }
        public Action CommandFunktion { get { return () => Command(); } }
        public Action HelpFunktion { get { return () => Help(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            if(InputProperties.Length > 1)
            {
                switch (InputProperties[1])
                {
                    case "disabel":
                        if(InputProperties.Length > 2) { DisabelDpkg(false,InputProperties[2]); }
                        break;

                    case "enabel":
                        if (InputProperties.Length > 2) { DisabelDpkg(true, InputProperties[2]); }
                        break;
                }
            }
            else
            {
                GetList();
            }
        }
        public void Help()
        {
            Console.WriteLine("comming soon...");
        }

        void DisabelDpkg(bool status,string name)
        {
            if (!Directory.Exists(DirPath)) { Directory.CreateDirectory(DirPath); }

            //Get Disabeled Plugins
            string configPath = Path.Combine(Commands.consoleData.SavePath, "var", "LeoConsole", "config");
            string filePath = Path.Combine(configPath, "dpkg.txt");
            List<string> disabeledPlugins;
            if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }
            if (File.Exists(filePath))
            {
                string configText = File.ReadAllText(filePath);
                disabeledPlugins = Config.GetCategory(configText, "pkgDisabeled").ToList();
            }
            else
            {
                disabeledPlugins = new List<string>();
            }

            //Enabel/Disabel
            if (status)
            {
                if (disabeledPlugins.Contains(name)) { disabeledPlugins.Remove(name); }
            }
            else
            {
                if(!disabeledPlugins.Contains(name)) { disabeledPlugins.Add(name); }
            }

            //Write
            if (File.Exists(filePath))
            {
                if(disabeledPlugins.Contains("#end")) { disabeledPlugins.Remove("#end"); }

                string configText = File.ReadAllText(filePath);
                string replacement = "#pkgDisabeled:\n";

                foreach(string plugin in disabeledPlugins)
                {
                    replacement = replacement + plugin + "\n";
                }
                replacement = replacement + "#";

                string newConfigText = Utils.ReplaceTextBetweenTags(configText, replacement, "#pkgDisabeled:", "#end");
                File.WriteAllText(filePath, newConfigText);
            }
            else
            {
                string newConfigText = "#pkgDisabeled:\n";

                foreach (string plugin in disabeledPlugins)
                {
                    newConfigText = newConfigText + plugin + "\n";
                }
                newConfigText = newConfigText + "#end";

                File.WriteAllText(filePath, newConfigText);
            }
        }

        void GetList()
        {
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

                //Get Disabeled Plugins
                string configPath = Path.Combine(Commands.consoleData.SavePath, "var", "LeoConsole", "config");
                string filePath = Path.Combine(configPath, "dpkg.txt");
                List<string> disabeledPlugins;
                if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }
                if (File.Exists(filePath))
                {
                    string configText = File.ReadAllText(filePath);
                    disabeledPlugins = Config.GetCategory(configText, "pkgDisabeled").ToList();
                }
                else
                {
                    disabeledPlugins = new List<string>();
                }

                if (disabeledPlugins.Contains("#end")) { disabeledPlugins.Remove("#end"); }

                //Print
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_dpkgCmdDpkg"));
                foreach(string name in DefaultPlugins.Keys) { Console.WriteLine(name); }
                Console.WriteLine();
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_dpkgCmdDisabeled"));
                foreach(string name in disabeledPlugins) { Console.WriteLine(name); }
            }
            catch(Exception e)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_dpkgUpdateFail404"));
            }
        }
    }

    #endregion
}
