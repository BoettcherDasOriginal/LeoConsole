﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole;
using ILeoConsole.Core;
using ILeoConsole.Plugin;

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
        public string Version { get { return Commands.currrentConsole.data.version; } }
    }

    #endregion

    #region Command Tenplates

    public class COMMAND : ICommand
    {
        public string Name { get { return ""; } }
        public string Description { get { return ""; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
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
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Console.WriteLine("");
        }
    }

    public class HELP : ICommand
    {
        public string Name { get { return "help"; } }
        public string Description { get { return "Zeigt die Hilfe"; } }
        public Action CommandFunktion { get { return () => Help(); } }
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
        public string Description { get { return "sucht nach Updates"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Update.CheckForUpdate(Commands.currrentConsole.data, "https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", Commands.consoleData.DownloadPath, Commands.consoleData.Version);
        }
    }

    public class EXIT : ICommand
    {
        public string Name { get { return "exit"; } }
        public string Description { get { return "Schließt Leo Console"; } }
        public Action CommandFunktion { get { return () => Exit(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Exit()
        {
            Environment.Exit(0);
        }
    }

    public class REBOOT : ICommand
    {
        public string Name { get { return "reboot"; } }
        public string Description { get { return "startet Leo Console neu"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Console.WriteLine("Bist du sicher das du Leo Console Neustarten möchtest? Y/N");
            Console.Write(">");
            string anser = Console.ReadLine();

            switch (anser.ToLower())
            {
                default:
                    Console.WriteLine("'" + anser + "' ist keine gültige Antwort!");
                    break;

                case "n":
                    break;

                case "":
                case "y":
                    Commands.currrentConsole.reboot();
                    break;
            }
        }
    }

    public class CREDITS : ICommand
    {
        public string Name { get { return "credits"; } }
        public string Description { get { return "zeigt die Credits"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            LConsole.WriteLine($"LeoConsole v{Commands.consoleData.Version}");
            LConsole.WriteLine("(c) 2021-2022, BoettcherDasOriginal");
        }
    }

    #endregion

    #region User Commands

    public class LOGOUT : ICommand
    {
        public string Name { get { return "logout"; } }
        public string Description { get { return "meldet den aktuellen Benutzer ab"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            LConsole.WriteLine("§4logout\n");

            List<User> users = SaveLoad.LoadUsers(Commands.consoleData.SavePath);

            if (users == null)
            {
                Console.WriteLine("User.lcs konnte nicht gefunden werden!");
                Commands.currrentConsole.firstStart();
            }
            else
            {
                Commands.currrentConsole.prepareConsole(users);
            }
        }
    }

    public class NEWUSERC : ICommand
    {
        public string Name { get { return "newUser"; } }
        public string Description { get { return "erstellt einen neuen User"; } }
        public Action CommandFunktion { get { return () => Command(); } }
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
                Console.WriteLine("Zum erstellen eines neuen Users, benötigst du Root rechte!");
            }
        }
    }

    public class WHOAMI : ICommand
    {
        public string Name { get { return "whoami"; } }
        public string Description { get { return "zeigt deine User Informationen"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            Console.WriteLine("Benutzername: " + Commands.consoleData.User.name);
            Console.WriteLine("Begrüßung: " + Commands.consoleData.User.begrüßung);
            Console.WriteLine("Root: " + Commands.consoleData.User.root.ToString());
        }
    }

    #endregion

    #region PKG COMMANDS

    public class PKGCOMMAND : ICommand
    {
        string url = "https://raw.githubusercontent.com/BoettcherDasOriginal/LeoConsole/main/PackageList.txt";
        string DirPath = Commands.consoleData.SavePath + "pkg/";
        string filePKGListName = "PackageList.txt";

        public string Name { get { return "pkg"; } }
        public string Description { get { return "ruft den Package Manager auf ('pkg get' oder 'pkg update')"; } }
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
            if (!File.Exists(DirPath + filePKGListName)) { Console.WriteLine("Die PackageList konnte nicht gefunden werden!\nVersuche mal 'pkg update'"); return; }

            if (_InputProperties.Length > 1)
            {
                if (!pkgIsFound)
                {
                    foreach (string pkgListLine in File.ReadLines(DirPath + filePKGListName))
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
                webClient.DownloadFile(url, Commands.consoleData.SavePath + $"plugins/{name}.dll");

                Console.WriteLine("pkg erfolgreich Heruntergeladen!");
                Console.WriteLine("Starte LeoConsole neu um es zu aktievieren!");
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
                webClient.DownloadFile(url, DirPath + filePKGListName);

                Console.WriteLine("Die PackageList ist nun aktuell!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Es ist ein Fehler beim updaten der PackageList aufgetaucht!\nBitte versuche es später nocheinmal!");
            }
        }
    }

    public class PLUGININFO : ICommand
    {
        public string Name { get { return "pkginfo"; } }
        public string Description { get { return "zeigt alle geladene Plugins"; } }
        public Action CommandFunktion { get { return () => Command(); } }
        private string[] _InputProperties;
        public string[] InputProperties { get { return _InputProperties; } set { _InputProperties = value; } }
        public void Command()
        {
            foreach(IPlugin plugin in PluginLoader.Plugins)
            {
                LConsole.WriteLine(plugin.Name + " => " + plugin.Explanation);
            }
        }
    }

    #endregion
}
