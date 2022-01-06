using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole;
using ILeoConsole.Core;
using ILeoConsole.Plugin;

namespace LeoConsole
{
    public class LeoConsole
    {
        public Data data;
        User user;
        UserFunctions userFunctions;

        public static List<ICommand> commands = new List<ICommand>();
        public static List<IData> datas = new List<IData>();

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

        #region FIRSTSTART

        public void firstStart()
        {
            if (!Directory.Exists(data.SavePath))
            {
                Directory.CreateDirectory(data.SavePath);
            }

            Console.WriteLine("First Startup: \n ");
            Console.WriteLine("Willkommen bei Leo Console!");
            Console.WriteLine("Damit du Leo Console benutzen kannst, musst du dir ein Root Konto erstellen.");
            Console.WriteLine("Gib dazu 'newKonto' ein. Du hast bereits ein Konto, aber es wurde nicht geladen? Gib 'helpKonto' ein.");

            do
            {
                Console.Write(">");
                string text = Console.ReadLine();

                if (text == "newKonto")
                {
                    userFunctions.newKonto(true, new List<User>());
                    break;
                }
                else if (text == "helpKonto")
                {
                    Console.WriteLine("\nWenn sie ihren SavePath ändern, kommt es dazu das Leo Console ihre alte Users.lcs Datei nicht mehr Findet. ");
                    Console.Write("Sie können einfach ihre alte Users.lcs Datei nach '" + data.SavePath + "' Kopieren.\n");
                }
                else
                {
                    Console.WriteLine("Der Befehl '" + text + "' ist entweder falsch geschrieben oder konnte nicht gefunden werden.");
                }

            } while (true);
        }

        #endregion

        #region START

        public void start()
        {
            Console.WriteLine("Startet...");
            Console.Title = "LeoConsole -> Starting...";

            if (!data.isLinuxBuild)
            {
                Update.CheckForUpdate(data, "https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", data.DownloadPath, data.version);
            }
            else
            {
                Console.WriteLine("Update übersprungen: LinuxBuild");
            }

            Console.WriteLine("Lädt: Users.lcs");

            List<User> users = SaveLoad.LoadUsers(data.SavePath);

            if (users == null)
            {
                Console.WriteLine("User.lcs konnte nicht gefunden werden!");
                firstStart();
            }
            else
            {
                Console.WriteLine("Users.lcs erfolgreich geladen");

                string PluginLoaderPath = Path.Combine(data.SavePath, "plugins");

                if (!Directory.Exists(PluginLoaderPath))
                {
                    Directory.CreateDirectory(PluginLoaderPath);
                }

                Console.WriteLine("Lädt: Plugins");

                try
                {
                    PluginLoader loader = new PluginLoader(PluginLoaderPath);
                    loader.LoadPlugins();

                    foreach(IPlugin plugin in PluginLoader.Plugins)
                    {
                        plugin.PluginMain();
                    }

                    Console.WriteLine($"Erfolgreich {PluginLoader.Plugins.Count} Plugins geladen!");
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Plugins konnten nicht Geladen werden: {0}", e.Message));
                    Console.WriteLine("Drücke eine Beliebige Taste um LeoConsole ohne Plugins zu starten...");
                    Console.ReadKey();
                }

                Console.WriteLine("Registriere: Datas");
                datas.Add(Commands.consoleData);
                foreach(IPlugin plugin in PluginLoader.Plugins)
                {
                    if(plugin.data != null)
                    {
                        datas.Add(plugin.data);
                    }
                }

                foreach (IData data in LeoConsole.datas)
                {
                    data.SavePath = this.data.SavePath;
                    data.DownloadPath = this.data.DownloadPath;
                }
                Console.WriteLine($"Erfolgreich {datas.Count} Datas registriert!");

                Console.WriteLine("Registriere: Commands");
                DefaultCommands();
                foreach(IPlugin plugin in PluginLoader.Plugins)
                {
                    if(plugin.Commands.Count > 0)
                    {
                        foreach(ICommand command in plugin.Commands)
                        {
                            commands.Add(command);
                        }
                    }
                }
                Console.WriteLine($"Erfolgreich {commands.Count} Commands registriert!");

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
            Console.WriteLine("");
            string afterUserName = "";
            if (PluginLoader.Consoles != null)
            {
                foreach (IConsole console in PluginLoader.Consoles)
                {
                    if (console.AfterInput != null && console.AfterInput != "")
                    {
                        afterUserName = afterUserName + ":" + console.AfterInput;
                    }
                }
            }

            LConsole.Write($"§a{user.name}§r{afterUserName}>");

            Input = Console.ReadLine();

            HandleInput();
        }

        bool commandExists;
        public void HandleInput()
        {
            //handle

            properties = Input.Split(' ');

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
                Console.WriteLine("Der Befehl '" + properties[0] + "' ist entweder falsch geschrieben oder konnte nicht gefunden werden.");
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
            commands.Add(new CREDITS());
            commands.Add(new LOGOUT());
            commands.Add(new NEWUSERC());
            commands.Add(new WHOAMI());
            commands.Add(new PKGCOMMAND());
            commands.Add(new PLUGININFO());
        }
    }
}
