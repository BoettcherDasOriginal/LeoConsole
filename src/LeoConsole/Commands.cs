using System;
using System.Collections.Generic;
using System.Linq;
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


}
