using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole;
using ILeoConsole.Core;
using ILeoConsole.Plugin;
using ILeoConsole.Localization;

namespace LeoConsole
{
    public class LeoConsole
    {
        public Data data;
        User user;
        UserFunctions userFunctions;

        public static string CurrentWorkingPath;
        public static List<ICommand> commands = new List<ICommand>();
        public static List<IData> datas = new List<IData>();

        #region RESET

        public void reboot()
        {
            Console.Clear();
            
            data = new Data();
            userFunctions = new UserFunctions(data);

            Commands.currrentConsole = this;
            Commands.userFunctions = userFunctions;
            Commands.consoleData = new ConsoleData();

            commands = new List<ICommand>();
            datas = new List<IData>();

            start();
        }

        public void reloadPlugins(bool reload)
        {
            if (reload) { LConsole.WriteLine("§6reload"); }

            commands = new List<ICommand>();
            datas = new List<IData>();

            //Get Language
            string configPath = Path.Combine(data.SavePath, "var", "LeoConsole", "config");
            string filePath = Path.Combine(configPath, "user.txt");
            string lang;
            if (!Directory.Exists(configPath)) { Directory.CreateDirectory(configPath); }
            if (File.Exists(filePath))
            {
                string configText = File.ReadAllText(filePath);
                lang = Config.GetCategory(configText, "language")[0];
            }
            else { lang = "en"; }

            LocalisationManager.Language = lang;

            //reload

            string PluginLoaderPath = Path.Combine(data.SavePath, "plugins");

            if (!Directory.Exists(PluginLoaderPath))
            {
                Directory.CreateDirectory(PluginLoaderPath);
            }

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadPlugins"));

            try
            {
                PluginLoader loader = new PluginLoader(PluginLoaderPath);
                loader.LoadPlugins();

                foreach (IPlugin plugin in PluginLoader.Plugins)
                {
                    plugin.PluginInit();
                }

                foreach (IConsole console in PluginLoader.Consoles)
                {
                    console.Execute = string.Empty;
                }

                Console.WriteLine($"{PluginLoader.Plugins.Count} {LocalisationManager.GetLocalizationFromKey("lc_loadPluginsSuc")}");
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format(LocalisationManager.GetLocalizationFromKey("lc_loadPluginsFailed") + " {0}", e.Message));
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_anyKeyContinue"));
                Console.ReadKey();
            }

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadDatas"));
            datas.Add(Commands.consoleData);
            foreach (IPlugin plugin in PluginLoader.Plugins)
            {
                if (plugin.data != null)
                {
                    datas.Add(plugin.data);
                }
            }

            foreach (IData data in LeoConsole.datas)
            {
                data.SavePath = this.data.SavePath;
                data.DownloadPath = this.data.DownloadPath;
                data.Version = this.data.version;
            }
            Console.WriteLine($"{datas.Count} {LocalisationManager.GetLocalizationFromKey("lc_loadDatasSuc")}");

            foreach (IPlugin plugin in PluginLoader.Plugins)
            {
                plugin.RegisterCommands();
            }

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadCommands"));
            DefaultCommands();
            foreach (IPlugin plugin in PluginLoader.Plugins)
            {
                if (plugin.Commands.Count > 0)
                {
                    foreach (ICommand command in plugin.Commands)
                    {
                        commands.Add(command);
                    }
                }
            }
            Console.WriteLine($"{commands.Count} {LocalisationManager.GetLocalizationFromKey("lc_loadCommandsSuc")}");

            DefaultPluginManager.UpdateDefaultPlugins();

            foreach (IPlugin plugin in PluginLoader.Plugins)
            {
                plugin.PluginMain();
            }
        }

        #endregion

        #region FIRSTSTART

        public void firstStart()
        {
            if (!Directory.Exists(data.SavePath))
            {
                Directory.CreateDirectory(data.SavePath);
            }

            Console.WriteLine("First Startup: \n ");
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_firstStartHi"));
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_firstStartInfo1"));
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_firstStartInfo2"));

            do
            {
                Console.Write(">");
                string text = Console.ReadLine();

                if (text == "newAccount")
                {
                    userFunctions.newKonto(true, new List<User>());
                    break;
                }
                else if (text == "helpAccount")
                {
                    Console.WriteLine($"\n{LocalisationManager.GetLocalizationFromKey("lc_firstStartHelp1")}");
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_firstStartHelp2"));
                }
                else
                {
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_cantFindCmdFront") + text + LocalisationManager.GetLocalizationFromKey("lc_cantFindCmdBack"));
                }

            } while (true);
        }

        #endregion

        #region START

        public void start()
        {
            LocalisationManager.Localizations.Add(new ENLocalisation());
            LocalisationManager.Init();

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_starting"));
            Console.Title = "LeoConsole -> Starting...";

            CurrentWorkingPath = data.SavePath;

            if (!data.isLinuxBuild)
            {
                Update.CheckForUpdate(data, "https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", data.DownloadPath, data.version);
            }
            else
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_linuxUpdateSkip"));
            }

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadUser"));

            List<User> users = SaveLoad.LoadUsers(data.SavePath);

            if (users == null)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadUser404"));
                firstStart();
            }
            else
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_loadUserSuc"));

                reloadPlugins(false);

                Console.WriteLine($"\n--- LeoConsole v{data.version} ---\n");
                Console.Title = "LeoConsole  v" + data.version;

                prepareConsole(users);
            }
        }

        #endregion

        #region INPUT

        bool starting_anser = false;

        public void prepareConsole(List<User> users)
        {
            user = userFunctions.UserLogin(users);
            Console.WriteLine();

            Console.WriteLine(user.begrüßung + " " + user.name + "!");
            starting_anser = false;

            GetInput();
        }

        string Input;
        string[] properties;

        public void GetInput()
        {
            //Check for Plugin command request
            foreach (IConsole console in PluginLoader.Consoles)
            {
                if(console.Execute != string.Empty)
                {
                    Input = console.Execute;
                    HandleInput();
                    break;
                }
            }

            //Input
            Console.WriteLine("");
            string afterUserName = "";
            if (PluginLoader.Consoles != null)
            {
                foreach (IConsole console in PluginLoader.Consoles)
                {
                    if (console.PluginTextAfterInput != null && console.PluginTextAfterInput != "")
                    {
                        afterUserName = afterUserName + ":" + console.PluginTextAfterInput;
                    }
                }
            }

            string pathDisplay = CurrentWorkingPath;
            if(pathDisplay.Equals(data.SavePath, StringComparison.OrdinalIgnoreCase)) { pathDisplay = "\\"; }

            LConsole.Write($"§a{user.name}§r{afterUserName}:§9{pathDisplay}§r>");

            Input = LConsole.ReadLine(commands);

            HandleInput();
        }

        bool commandExists;
        public void HandleInput()
        {
            //handle

            //properties = Input.Split(' ');
            properties = Utils.HandelApostropheInput(Input);

            if (!starting_anser && properties[0] == "hi")
            {
                Console.WriteLine(":D"); starting_anser = true;
                GetInput();
            }
            else
            {
                starting_anser = true;
            }

            commandExists = false;
            foreach(ICommand command in commands)
            {
                if(command.Name == properties[0])
                {
                    commandExists = true;
                    command.Execute(properties);
                }
            }
            if (!commandExists)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_cantFindCmdFront") + properties[0] + LocalisationManager.GetLocalizationFromKey("lc_cantFindCmdBack"));
            }

            foreach(IData data in datas)
            {
                data.CurrentWorkingPath = CurrentWorkingPath;
            }

            GetInput();
        }

        #endregion

        public void DefaultCommands()
        {
            commands.Add(new EMPTY());
            commands.Add(new HELP());
            commands.Add(new UPDATE());
            commands.Add(new EXIT());
            commands.Add(new REBOOT());
            commands.Add(new RELOAD());
            commands.Add(new CREDITS());
            commands.Add(new LOGOUT());
            commands.Add(new NEWUSERC());
            commands.Add(new WHOAMI());
            commands.Add(new PLUGININFO());
            commands.Add(new LS());
            commands.Add(new CD());
            commands.Add(new MKDIR());
            commands.Add(new RMTRASH());
            commands.Add(new LANG());
            commands.Add(new DPKG());
        }
    }
}
