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
using ILeoConsole.Localization;

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
                            Console.WriteLine($"{LocalisationManager.GetLocalizationFromKey("lc_updateMsg")} \n");
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateVCurrent") + version);
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateVNew") + updateText + "\n");
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateQ"));
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
                                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateSkip"));
                                    break;
                            }
                        }
                        else
                        {
                            if (second_updateNumber > second_versionNumber)
                            {
                                Console.WriteLine($"{LocalisationManager.GetLocalizationFromKey("lc_updateMsg")} \n");
                                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateVCurrent") + version);
                                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateVNew") + updateText + "\n");
                                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateQ"));
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
                                        Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateSkip"));
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateNone"));
                            }
                        }
                    }
                    else
                    {
                        if (first_updateNumber > first_versionNumber)
                        {
                            Console.WriteLine($"{LocalisationManager.GetLocalizationFromKey("lc_updateMsg")} \n");
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateVCurrent") + version);
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateVNew") + updateText + "\n");
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateQ"));
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
                                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateSkip"));
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateNone"));
                        }
                    }
                }
                else
                {
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateNone"));
                }

                File.Delete(filepath + "update.txt");
            }

            catch (Exception e)
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateCheck404"));
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_anyKeyContinue"));
                Console.ReadKey();
            }
        }

        public static void AutoUpdate(Data data, string newVersion)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo installDir = Directory.GetParent(Directory.GetParent(dir).FullName);

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateStart"));
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateInstallFolder") + installDir.FullName);

            try
            {
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateStartDown"));

                string zipFilePath = Path.Combine(data.DownloadPath, $"LeoConsole_v{newVersion}.zip");

                WebClient webClient = new WebClient();
                webClient.DownloadFile("https://github.com/boettcherDasOriginal/LeoConsole/releases/latest/download/LeoConsole_v" + newVersion + ".zip", zipFilePath);

                Console.WriteLine("'" + zipFilePath + LocalisationManager.GetLocalizationFromKey("lc_updateDownSuc"));
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateUnZip"));
                ZipFile.ExtractToDirectory(zipFilePath, Path.Combine(installDir.FullName, $"LeoConsole_v{newVersion}"));

                File.Delete(zipFilePath);

                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateDataInfo"));
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateDataQ"));
                do
                {
                    Console.Write(">");
                    string text = Console.ReadLine();

                    if (text == "y")
                    {
                        Tools.DirectoryCopy(data.SavePath, Path.Combine(installDir.FullName, $"LeoConsole_v{newVersion}", "data"), true);
                        Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateDataCopySuc"));
                        break;
                    }
                    else if (text == "n")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_cantFindCmdFront") + text + LocalisationManager.GetLocalizationFromKey("lc_cantFindCmdBack"));
                    }

                } while (true);

                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_complete"));

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
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_updateFail"));
                Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_anyKeyContinue"));
                Console.ReadKey();
            }

        }

    }
}
