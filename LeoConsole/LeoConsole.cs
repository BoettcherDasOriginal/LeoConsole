using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ILeoConsole;
using System.Net;

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
            if(!Directory.Exists(data.DefaultSavePath))
            {
                Directory.CreateDirectory(data.DefaultSavePath);
            }

            Console.WriteLine("First Startup: \n ");
            Console.WriteLine("Willkommen bei Leo Console!");
            Console.WriteLine("Damit du Leo Console benutzen kannst, musst du dir ein Konto erstellen.");
            Console.WriteLine("Gib dazu 'newKonto' ein. Du hast bereits ein Konto, aber es wurde nicht geladen? Gib 'helpKonto' ein.");

            do
            {
                Console.Write(">");
                string text = Console.ReadLine();

                if(text == "newKonto")
                {
                    newKonto();
                    break;
                }
                else if(text == "helpKonto")
                {
                    Console.WriteLine("\nWenn sie ihren SavePath ändern, kommt es dazu das Leo Console ihre alte User.lcs Datei nicht mehr Findet. ");
                    Console.Write("Sie können einfach ihre alte User.lcs Datei nach '" + data.SavePath + "' Kopieren.\n");
                }
                else
                {
                    Console.WriteLine("Der Befehl '" + text + "' ist entweder falsch geschrieben oder konnte nicht gefunden werden.");
                }

            } while (true);
        }

        #endregion

        #region NEWKONTO

        public void newKonto()
        {
            User newUser = new User();

            Console.WriteLine("\nUm ein neues Konto zu erstellen, geben sie bitte folgende Informationen ein:");

            Console.Write("Benutzername: ");
            newUser.name = Console.ReadLine();

            Console.Write("Begrüßungssatz: ");
            newUser.begrüßung = Console.ReadLine();

            Console.WriteLine("\nDas wars. Drücke eine Beliebige Taste um das Konto zu Speichern.\nBeachte das dein altes Konto (Falls vorhanden) gelöscht wird!");
            Console.ReadKey();
            SaveLoad.saveUser(newUser, data.SavePath);
            Console.WriteLine("Drücke eine Beliebige Taste um Leo Console Neuzustarten.");
            Console.ReadKey();

            LeoConsole leoConsole = new LeoConsole();
            leoConsole.neuStart();
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

                if(first_updateNumber <= first_versionNumber)
                {
                    if(second_updateNumber <= second_versionNumber)
                    {
                        string[] updateNumber = updateNumbers[2].Split('-');
                        string[] versionNumber = versionNumbers[2].Split('-');

                        int third_updateNumber = Int32.Parse(updateNumber[0]);
                        int third_versionNumber = Int32.Parse(versionNumber[0]);

                        if(third_updateNumber <= third_versionNumber)
                        {
                            Console.WriteLine("Keine Updates Gefunden!");
                        }
                        else
                        {
                            Console.WriteLine("Updates verfügbar! \n");
                            Console.WriteLine("Deine Version: " + version);
                            Console.WriteLine("Neue Version: " + updateText + "\n");
                            Console.WriteLine("Du kannst die Neu version unter <NULL> Herunterladen");
                            Console.WriteLine("Drücke eine beliebiege Taste um vortzufahren");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Updates verfügbar! \n");
                        Console.WriteLine("Deine Version: " + version);
                        Console.WriteLine("Neue Version: " + updateText + "\n");
                        Console.WriteLine("Du kannst die Neu version unter 'https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/' Herunterladen");
                        Console.WriteLine("Drücke eine beliebiege Taste um vortzufahren");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Updates verfügbar! \n");
                    Console.WriteLine("Deine Version: " + version);
                    Console.WriteLine("Neue Version: " + updateText + "\n");
                    Console.WriteLine("Du kannst die Neu version unter <NULL> Herunterladen");
                    Console.WriteLine("Drücke eine beliebiege Taste um vortzufahren");
                    Console.ReadKey();
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

        #endregion

        #region START

        public void start()
        {
            Console.WriteLine("Startet...");
            Console.Title = "LeoConsole -> Starting...";

            Console.WriteLine("Suche Nach Updates...");
            CheckForUpdate("https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/version.txt", data.DefaultSavePath, data.version);

            Console.WriteLine("Lädt: SavePath.lcs");
            data.SavePath = SaveLoad.LoadPath();
            Console.WriteLine("SavePath: " + data.SavePath);
            Console.WriteLine("Lädt: User.lcs");
            
            user = SaveLoad.LoadUser(data.SavePath);

            if(user == null)
            {
                Console.WriteLine("User.lcs konnte nicht gefunden werden!");
                firstStart();
            }
            else
            {
                Console.WriteLine("User: " + user.name);

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

                    case "setSavePath": _SETSAVEPATH(properties[1]); break;

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
            "setSavePath <path>               setzt den SavePath von Leo Console",
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
        public void _SETSAVEPATH(string path)
        {
            Console.WriteLine("Bist du sicher das du den SavePath auf '" + path + "' legen möchtest? Y/N");
            Console.Write(">");
            string anser = Console.ReadLine();

            switch(anser)
            {
                default: 
                    Console.WriteLine("'" + anser + "' ist keine gültige Antwort!");
                    consoleAppInput();
                    break;

                case "N":
                    consoleAppInput();
                    break;

                case "Y":
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    SaveLoad.savePath(path);
                    Console.WriteLine("Der SavePath wurde auf '" + path + "' gesetzt.");
                    _NEUSTART();
                    break;
            }
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
            newKonto();
        }
        public void _KONTOINFO()
        {
            Console.WriteLine("Benutzername: " + user.name);
            Console.WriteLine("Begrüßung: " + user.begrüßung);

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
            Console.WriteLine("PluginPath: " + data.SavePath + "plugins");

            consoleAppInput();
        }
        public void _CUSTOMPLUGIN()
        {
            Console.WriteLine("Siehe 'plugin doc' Ordner");

            consoleAppInput();
        }
        public void _UPDATE()
        {
            CheckForUpdate("https://hgs-update.netlify.app/update/LeoConsole.txt", data.DefaultSavePath, data.version);

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
