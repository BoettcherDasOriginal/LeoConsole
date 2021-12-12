using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using ILeoConsole;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace LeoConsole
{
    class LeoConsole
    {
        Random rand = new Random();

        Data data;
        User user;

        #region NEUSTART

        public void neuStart()
        {
            Console.Clear();
            data = new Data();
            start();
        }

        #endregion

        #region FIRSTSTART

        public void firstStart()
        {
            if(!Directory.Exists(data.SavePath))
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

                if(text == "newKonto")
                {
                    newKonto(true, new List<User>());
                    break;
                }
                else if(text == "helpKonto")
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

        #region UPDATE

        public void CheckForUpdate(string url, string filepath, string version)
        {
            if(!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, filepath + "update.txt");

                string updateText = File.ReadAllText(filepath + "update.txt");

                string[] updateNumbers = updateText.Split('.');
                string[] versionNumbers = version.Split('.');

                int first_updateNumber = Int32.Parse(updateNumbers[0]);
                int first_versionNumber = Int32.Parse(versionNumbers[0]);

                int second_updateNumber = Int32.Parse(updateNumbers[1]);
                int second_versionNumber = Int32.Parse(versionNumbers[1]);

                if(first_updateNumber >= first_versionNumber)
                {
                    if(second_updateNumber >= second_versionNumber)
                    {
                        string[] updateNumber = updateNumbers[2].Split('-');
                        string[] versionNumber = versionNumbers[2].Split('-');

                        int third_updateNumber = Int32.Parse(updateNumber[0]);
                        int third_versionNumber = Int32.Parse(versionNumber[0]);

                        if(third_updateNumber > third_versionNumber)
                        {
                            Console.WriteLine("Updates verfügbar! \n");
                            Console.WriteLine("Deine Version: " + version);
                            Console.WriteLine("Neue Version: " + updateText + "\n");
                            Console.WriteLine("Soll das Update Heruntergeladen werden? y/n");
                            string anser = Console.ReadLine();
                            switch (anser)
                            {
                                case "y":
                                    AutoUpdate(updateText);
                                    break;

                                case "Y":
                                    AutoUpdate(updateText);
                                    break;

                                default:
                                    Console.WriteLine("Weiter ohne Update!");
                                    break;
                            }
                        }
                        else
                        {
                            if(second_updateNumber > second_versionNumber)
                            {
                                Console.WriteLine("Updates verfügbar! \n");
                                Console.WriteLine("Deine Version: " + version);
                                Console.WriteLine("Neue Version: " + updateText + "\n");
                                Console.WriteLine("Soll das Update Heruntergeladen werden? y/n");
                                string anser = Console.ReadLine();
                                switch (anser)
                                {
                                    case "y":
                                        AutoUpdate(updateText);
                                        break;

                                    case "Y":
                                        AutoUpdate(updateText);
                                        break;

                                    default:
                                        Console.WriteLine("Weiter ohne Update!");
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Keine Updates Gefunden!");
                            }
                        }
                    }
                    else
                    {
                        if(first_updateNumber > first_versionNumber)
                        {
                            Console.WriteLine("Updates verfügbar! \n");
                            Console.WriteLine("Deine Version: " + version);
                            Console.WriteLine("Neue Version: " + updateText + "\n");
                            Console.WriteLine("Soll das Update Heruntergeladen werden? y/n");
                            string anser = Console.ReadLine();
                            switch (anser)
                            {
                                case "y":
                                    AutoUpdate(updateText);
                                    break;

                                case "Y":
                                    AutoUpdate(updateText);
                                    break;

                                default:
                                    Console.WriteLine("Weiter ohne Update!");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Keine Updates Gefunden!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Keine Updates Gefunden!");
                }

                File.Delete(filepath + "update.txt");
            }

            catch (Exception e)
            {
                Console.WriteLine("Die Update Seite konnte nicht ereicht werden! Bitte versuche es später Nochmal");
                Console.WriteLine("Drücke eine beliebiege Taste um vortzufahren");
                Console.ReadKey();
            }
        }

        public void AutoUpdate(string newVersion)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo installDir = Directory.GetParent(Directory.GetParent(dir).FullName);

            Console.WriteLine("Starte Update...");
            Console.WriteLine("Installations Ordner: " + installDir.FullName);

            try
            {
                Console.WriteLine("Starte Download der neuen Version...");

                string zipFilePath = data.DownloadPath + "LeoConsole_v" + newVersion + ".zip";

                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/LeoConsole_v" + newVersion + ".zip", zipFilePath);

                Console.WriteLine("'" + zipFilePath + "' erfolgreich Heruntergeladen");
                Console.WriteLine("Extrahiere Zip Datei...");
                ZipFile.ExtractToDirectory(zipFilePath,installDir.FullName + "\\LeoConsole_v" + newVersion);

                File.Delete(zipFilePath);

                Console.WriteLine("'data/' könnte evtl. nicht mit der neuen Version kompatiebel sein.");
                Console.WriteLine("Soll 'data/' zur neuen Version Kopiertwerden? y/n\n");
                do
                {
                    Console.Write(">");
                    string text = Console.ReadLine();

                    if(text == "y")
                    {
                        Tools.DirectoryCopy("data", installDir.FullName + "\\LeoConsole_v" + newVersion + "\\data", true);
                        Console.WriteLine("'data/' erfolgreich Kopiert!");
                        break;
                    }
                    else if(text == "n")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Der Befehl '" + text + "' ist entweder falsch geschrieben oder konnte nicht gefunden werden.");
                    }

                } while (true);

                Console.WriteLine("Fertig!");

                Process LCProcess = new Process();

                LCProcess.StartInfo.FileName = installDir.FullName + "\\LeoConsole_v" + newVersion + "\\LeoConsole.exe";
                LCProcess.StartInfo.Arguments = "update \"" + Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory) + "\"";
                LCProcess.StartInfo.WorkingDirectory = installDir.FullName + "\\LeoConsole_v" + newVersion + "\\";
                LCProcess.Start();

                File.Delete(zipFilePath);

                Thread.Sleep(100);

                Environment.Exit(0);
            }
            catch(Exception e)
            {
                Console.WriteLine("Update konnte nicht Heruntergeladen werden!");
                Console.WriteLine("Drücke eine beliebiege Taste um vortzufahren");
                Console.ReadKey();
            }

        }

        #endregion

        #region User

        public void newKonto(bool root, List<User> users)
        {
            User newUser = new User();

            Console.WriteLine("\nUm ein neues Konto zu erstellen, geben sie bitte folgende Informationen ein:");

            Console.Write("Benutzername: ");
            newUser.name = Console.ReadLine();

            Console.Write("Begrüßungssatz: ");
            newUser.begrüßung = Console.ReadLine();

            bool passwordCheck = false;
            do
            {
                Console.Write("Password: ");
                var pass = string.Empty;
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass.Substring(0, pass.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.WriteLine("");
                Console.Write("Password bestätigen: ");
                var passCheck = string.Empty;
                ConsoleKey keyCheck;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    keyCheck = keyInfo.Key;

                    if (keyCheck == ConsoleKey.Backspace && passCheck.Length > 0)
                    {
                        Console.Write("\b \b");
                        passCheck = passCheck.Substring(0, passCheck.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        passCheck += keyInfo.KeyChar;
                    }
                } while (keyCheck != ConsoleKey.Enter);

                if(pass == passCheck)
                {
                    newUser.password = pass;
                    passwordCheck = true;
                }
                else
                {
                    Console.WriteLine("Falsches Passwort!");
                }
            } while (!passwordCheck);

            newUser.root = root;

            Console.WriteLine("\nDas wars. Drücke eine Beliebige Taste um das Konto zu Speichern.");
            Console.ReadKey();

            users.Add(newUser);
            SaveLoad.saveUsers(users, data.SavePath);
            Console.WriteLine("Drücke eine Beliebige Taste um Leo Console Neuzustarten.");
            Console.ReadKey();

            LeoConsole leoConsole = new LeoConsole();
            leoConsole.neuStart();
        }

        public User UserLogin(List<User> users)
        {
            User result = null;

            Dictionary<string, string> userInfos = new Dictionary<string, string>();
            foreach (User u in users)
            {
                userInfos.Add(u.name, u.password);
            }

            do
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                if (userInfos.ContainsKey(username))
                {
                    Console.Write("Password: ");
                    var pass = string.Empty;
                    ConsoleKey key;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        key = keyInfo.Key;

                        if (key == ConsoleKey.Backspace && pass.Length > 0)
                        {
                            Console.Write("\b \b");
                            pass = pass.Substring(0, pass.Length - 1);
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            pass += keyInfo.KeyChar;
                        }
                    } while (key != ConsoleKey.Enter);
                    for (int i = 0; i < userInfos.Count; i++)
                    {
                        if(userInfos.Contains(new KeyValuePair<string, string>(username, pass)))
                        {
                            foreach(User u in users)
                            {
                                if(u.name == username && u.password == pass) { result = u; break; }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Falsches Password!\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Der Benutzer '{0}' konnte nicht gefunden werden!\n", username);
                }

                if(result != null)
                {
                    break;
                }

            } while (true);

            return result;
        }

        #endregion

        #region START

        public void start()
        {
            Console.WriteLine("Startet...");
            Console.Title = "LeoConsole -> Starting...";

            Console.WriteLine("Suche Nach Updates...");
            CheckForUpdate("https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", data.DownloadPath, data.version);

            Console.WriteLine("SavePath: " + data.SavePath);
            Console.WriteLine("Lädt: Users.lcs");
            
            List<User> users = SaveLoad.LoadUsers(data.SavePath);

            if(users == null)
            {
                Console.WriteLine("User.lcs konnte nicht gefunden werden!");
                firstStart();
            }
            else
            {
                Console.WriteLine("Users.lcs erfolgreich geladen");
                Console.WriteLine("Einlogen...");
                user = UserLogin(users);

                Console.WriteLine("\nUser: " + user.name);

                string PluginLoaderPath = data.SavePath + "plugins";

                if (!Directory.Exists(PluginLoaderPath))
                {
                    Directory.CreateDirectory(PluginLoaderPath);
                }

                Console.WriteLine("Lädt: Plugins");
                try
                {
                    PluginLoader loader = new PluginLoader(PluginLoaderPath);
                    loader.LoadPlugins();

                    Console.WriteLine("Plugins:");

                    foreach (IPlugin plugin in PluginLoader.Plugins)
                    {
                        Console.WriteLine("{0}", plugin.Name);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Plugins konnten nicht Geladen werden: {0}", e.Message));
                    Console.WriteLine("Drücke eine Beliebige Taste um LeoConsole ohne Plugins zu starten...");
                    Console.ReadKey();
                }

                Console.WriteLine("\n--Initialisierung Abgeschlossen--\n");
                Console.Title = "LeoConsole  v" + data.version;

                consoleApp();
            }
        }

        bool starting_anser = false;

        public void consoleApp()
        {
            Console.WriteLine(user.begrüßung + " " + user.name + "!");
            starting_anser = false;

            consoleAppInput();
        }

        #endregion

        #region INPUT

        string Input;
        string[] properties;

        public void consoleAppInput()
        {
            Console.WriteLine("");
            Console.Write(">");

            Input = Console.ReadLine();

            handelInput();
        }

        public void handelInput()
        {
            properties = Input.Split(' ');

            string pluginName = GetPluginName(properties[0]);

            if(!starting_anser && properties[0] == "hi")
            {
                Console.WriteLine(":D"); starting_anser = true;
                consoleAppInput();
            }
            else
            {
                starting_anser = true;
            }

            if(properties[0] == pluginName)
            {
                _PLUGIN(properties[0], Input);
            }
            else
            {
                switch (properties[0])
                {
                    default: _DEFAULT(properties[0]); break;

                    case "": _LEER(); break;

                    case "help": _HELP(); break;

                    case "exit": _EXIT(); break;

                    case "neustart": _NEUSTART(); break;

                    case "randomNumber": _RANDOMNUMBER(properties[1], Convert.ToInt32(properties[2])); break;

                    case "newKonto": _NEWKONTO(); break;

                    case "kontoInfo": _KONTOINFO(); break;

                    case "pluginHelp": _PLUGINHELP(); break;

                    case "pluginPath": _PLUGINPATH(); break;

                    case "customPlugin": _CUSTOMPLUGIN(); break;

                    case "checkForUpdate": _UPDATE(); break;

                    case "credits": _CREDITS(); break;
                }
            }
        }

        #endregion

        #region COMMAND

        string[] commandList = 
        { 
            "help                             zeigt die Hilfe", 
            "exit                             schließt Leo Console", 
            "neustart                         startet Leo Console Neu",
            "credits                          zeigt die Credits",
            "checkForUpdate                   guckt ob Updates verfügbar sind",
            "randomNumber <bool> <length>     generiert einen zuffälligen Code",
            "newKonto                         erstellt ein neues Konto",
            "kontoInfo                        zeigt die Konto Daten",
            "pluginHelp                       zeigt die Hilfe für die Plugins",
            "pluginPath                       zeigt den PluginPath",
            "customPlugin                     gibt Informationen darüber, wie man eigene Plugins erstellt"
        };
        

        public void _COMMAND()
        {
            consoleAppInput();
        }
        public void _DEFAULT(string text)
        {
            Console.WriteLine("Der Befehl '" + text + "' ist entweder falsch geschrieben oder konnte nicht gefunden werden.");

            consoleAppInput();
        }
        public void _LEER()
        {
            Console.WriteLine("");
            
            consoleAppInput();
        }
        public void _EXIT()
        {
            Environment.Exit(0);
        }
        public void _NEUSTART()
        {
            Console.WriteLine("Bist du sicher das du Leo Console Neustarten möchtest? Y/N");
            Console.Write(">");
            string anser = Console.ReadLine();

            switch (anser)
            {
                default:
                    Console.WriteLine("'" + anser + "' ist keine gültige Antwort!");
                    consoleAppInput();
                    break;

                case "N":
                    consoleAppInput();
                    break;

                case "Y":
                    neuStart();
                    break;
            }
        }
        public void _HELP()
        {
            for(int i = 0; i < commandList.Length; i++)
            {
                Console.WriteLine(commandList[i]);
            }

            consoleAppInput();
        }
        public void _RANDOMNUMBER(string withText, int length)
        {
            int Buchstabe;
            int textOrNumber;

            Console.WriteLine("Neuer Code: ");

            for (int i = 0; i < length; i++)
            {
                if(withText == "true")
                {
                    textOrNumber = rand.Next(0, 12);

                    if(textOrNumber >= 10)
                    {
                        Buchstabe = rand.Next(1, 26);

                        switch(Buchstabe)
                        {
                            case 1:
                                Console.Write("A");
                                break;

                            case 2:
                                Console.Write("B");
                                break;

                            case 3:
                                Console.Write("C");
                                break;

                            case 4:
                                Console.Write("D");
                                break;

                            case 5:
                                Console.Write("E");
                                break;

                            case 6:
                                Console.Write("F");
                                break;

                            case 7:
                                Console.Write("G");
                                break;

                            case 8:
                                Console.Write("H");
                                break;

                            case 9:
                                Console.Write("I");
                                break;

                            case 10:
                                Console.Write("J");
                                break;

                            case 11:
                                Console.Write("K");
                                break;

                            case 12:
                                Console.Write("L");
                                break;

                            case 13:
                                Console.Write("M");
                                break;

                            case 14:
                                Console.Write("N");
                                break;

                            case 15:
                                Console.Write("O");
                                break;

                            case 16:
                                Console.Write("P");
                                break;

                            case 17:
                                Console.Write("Q");
                                break;

                            case 18:
                                Console.Write("R");
                                break;

                            case 19:
                                Console.Write("S");
                                break;

                            case 20:
                                Console.Write("T");
                                break;

                            case 21:
                                Console.Write("U");
                                break;

                            case 22:
                                Console.Write("V");
                                break;

                            case 23:
                                Console.Write("W");
                                break;

                            case 24:
                                Console.Write("X");
                                break;

                            case 25:
                                Console.Write("Y");
                                break;

                            case 26:
                                Console.Write("Z");
                                break;
                        }
                    }
                    else if(textOrNumber <= 9)
                    {
                        Console.Write(rand.Next(0, 9).ToString());
                    }
                }
                else
                {
                    Console.Write(rand.Next(0, 9).ToString());
                }
            }

            Console.WriteLine("");
            consoleAppInput();
        }
        public void _NEWKONTO()
        {
            if (user.root)
            {
                newKonto(false, SaveLoad.LoadUsers(data.SavePath));
            }
            else
            {
                Console.WriteLine("Zum erstellen eines neuen Users, benötigst du Root rechte!");
                consoleAppInput();
            }
        }
        public void _KONTOINFO()
        {
            Console.WriteLine("Benutzername: " + user.name);
            Console.WriteLine("Begrüßung: " + user.begrüßung);
            Console.WriteLine("Root: " + user.root.ToString());

            consoleAppInput();
        }
        public void _PLUGINHELP()
        {
            foreach (IPlugin plugin in PluginLoader.Plugins)
            {
                Console.WriteLine("{0}: {1}", plugin.Name, plugin.Explanation);
            }

            consoleAppInput();
        }
        public void _PLUGINPATH()
        {
            Console.WriteLine("PluginPath: " + new DirectoryInfo(data.SavePath + "plugins").FullName);

            consoleAppInput();
        }
        public void _CUSTOMPLUGIN()
        {
            Console.WriteLine("Siehe 'plugin doc' Ordner");

            consoleAppInput();
        }
        public void _UPDATE()
        {
            CheckForUpdate("https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", data.DownloadPath, data.version);

            consoleAppInput();
        }
        public void _CREDITS()
        {
            Console.WriteLine("Programmierung: Horizon");
            Console.WriteLine("Desing: Horizon");
            Console.WriteLine("Grafik: Horizon");

            consoleAppInput();
        }

        #endregion

        #region Plugins

        public string GetPluginName (string name)
        {
            IPlugin plugin = PluginLoader.Plugins.Where(p => p.Name == name).FirstOrDefault();
            if (plugin != null)
            {
                return plugin.Name;
            }
            else
            {
                return "ERROR: Fehler beim Abrufen des Plugin-Namens!";
            }
        }

        public void _PLUGIN(string name, string line)
        {
            IPlugin plugin = PluginLoader.Plugins.Where(p => p.Name == name).FirstOrDefault();
            if (plugin != null)
            {
                string parameters = line.Replace(string.Format("{0} ", name), string.Empty);
                plugin.PluginMain(parameters);

                consoleAppInput();
            }
            else
            {
                Console.WriteLine("Hopla! Es ist ein Problemm beim Laden des Plugins aufgetreten.");

                consoleAppInput();
            }
        }

        #endregion
    }
}
