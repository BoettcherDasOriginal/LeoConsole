using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;

namespace LeoConsole
{
    internal class Update
    {
        public static void CheckForUpdate(Data data, string url, string filepath, string version)
        {
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, Path.Combine(filepath, "update.txt"));

                string updateText = File.ReadAllText(Path.Combine(filepath, "update.txt"));

                string[] updateNumbers = updateText.Split('.');
                string[] versionNumbers = version.Split('.');

                int first_updateNumber = Int32.Parse(updateNumbers[0]);
                int first_versionNumber = Int32.Parse(versionNumbers[0]);

                int second_updateNumber = Int32.Parse(updateNumbers[1]);
                int second_versionNumber = Int32.Parse(versionNumbers[1]);

                if (first_updateNumber >= first_versionNumber)
                {
                    if (second_updateNumber >= second_versionNumber)
                    {
                        string[] updateNumber = updateNumbers[2].Split('-');
                        string[] versionNumber = versionNumbers[2].Split('-');

                        int third_updateNumber = Int32.Parse(updateNumber[0]);
                        int third_versionNumber = Int32.Parse(versionNumber[0]);

                        if (third_updateNumber > third_versionNumber)
                        {
                            Console.WriteLine("Updates verfügbar! \n");
                            Console.WriteLine("Deine Version: " + version);
                            Console.WriteLine("Neue Version: " + updateText + "\n");
                            Console.WriteLine("Soll das Update Heruntergeladen werden? y/n");
                            string anser = Console.ReadLine();
                            switch (anser)
                            {
                                case "y":
                                    AutoUpdate(data, updateText);
                                    break;

                                case "Y":
                                    AutoUpdate(data, updateText);
                                    break;

                                default:
                                    Console.WriteLine("Weiter ohne Update!");
                                    break;
                            }
                        }
                        else
                        {
                            if (second_updateNumber > second_versionNumber)
                            {
                                Console.WriteLine("Updates verfügbar! \n");
                                Console.WriteLine("Deine Version: " + version);
                                Console.WriteLine("Neue Version: " + updateText + "\n");
                                Console.WriteLine("Soll das Update Heruntergeladen werden? y/n");
                                string anser = Console.ReadLine();
                                switch (anser)
                                {
                                    case "y":
                                        AutoUpdate(data, updateText);
                                        break;

                                    case "Y":
                                        AutoUpdate(data, updateText);
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
                        if (first_updateNumber > first_versionNumber)
                        {
                            Console.WriteLine("Updates verfügbar! \n");
                            Console.WriteLine("Deine Version: " + version);
                            Console.WriteLine("Neue Version: " + updateText + "\n");
                            Console.WriteLine("Soll das Update Heruntergeladen werden? y/n");
                            string anser = Console.ReadLine();
                            switch (anser)
                            {
                                case "y":
                                    AutoUpdate(data, updateText);
                                    break;

                                case "Y":
                                    AutoUpdate(data, updateText);
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

        public static void AutoUpdate(Data data, string newVersion)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo installDir = Directory.GetParent(Directory.GetParent(dir).FullName);

            Console.WriteLine("Starte Update...");
            Console.WriteLine("Installations Ordner: " + installDir.FullName);

            try
            {
                Console.WriteLine("Starte Download der neuen Version...");

                string zipFilePath = Path.Combine(data.DownloadPath, $"LeoConsole_v{newVersion}.zip");

                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/LeoConsole_v" + newVersion + ".zip", zipFilePath);

                Console.WriteLine("'" + zipFilePath + "' erfolgreich Heruntergeladen");
                Console.WriteLine("Extrahiere Zip Datei...");
                ZipFile.ExtractToDirectory(zipFilePath, Path.Combine(installDir.FullName, $"LeoConsole_v{newVersion}"));

                File.Delete(zipFilePath);

                Console.WriteLine("'data/' könnte evtl. nicht mit der neuen Version kompatiebel sein.");
                Console.WriteLine("Soll 'data/' zur neuen Version Kopiertwerden? y/n\n");
                do
                {
                    Console.Write(">");
                    string text = Console.ReadLine();

                    if (text == "y")
                    {
                        Tools.DirectoryCopy(data.SavePath, Path.Combine(installDir.FullName, $"LeoConsole_v{newVersion}", "data"), true);
                        Console.WriteLine("'data/' erfolgreich Kopiert!");
                        break;
                    }
                    else if (text == "n")
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

                LCProcess.StartInfo.FileName = Path.Combine(installDir.FullName, $"LeoConsole_v{newVersion}", "LeoConsole.exe");
                LCProcess.StartInfo.Arguments = "update \"" + Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory) + "\"";
                LCProcess.StartInfo.WorkingDirectory = Path.Combine(installDir.FullName, $"LeoConsole_v{newVersion}");
                LCProcess.Start();

                File.Delete(zipFilePath);

                Thread.Sleep(100);

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Update konnte nicht Heruntergeladen werden!");
                Console.WriteLine("Drücke eine beliebiege Taste um vortzufahren");
                Console.ReadKey();
            }

        }

    }
}
