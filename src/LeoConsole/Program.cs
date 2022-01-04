using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LeoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                switch (args[0])
                {
                    case "update":
                        FinishUpdate(args[1]);
                        break;
                }
            }

            Console.Title = "Leo Console";

            LeoConsole leoConsole = new LeoConsole();
            leoConsole.reboot();

            Console.WriteLine("LeoConsole wurde Beendet");
            Console.ReadLine();
        }

        private static void FinishUpdate(string oldVersionPath)
        {
            try
            {
                DirectoryInfo oldDir = new DirectoryInfo(oldVersionPath);
                if (oldDir.Exists)
                {
                    Console.WriteLine("LeoConsole wurde erfolgreich aktualiesiert!");
                    Console.WriteLine("alte Installation: " + oldDir.FullName);
                    Console.WriteLine("Soll die alte Installation deinstalliert werden? y/n");
                    do
                    {
                        Console.Write(">");
                        string text = Console.ReadLine();

                        if (text == "y")
                        {
                            oldDir.Delete(true);
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
                }
                else
                {
                    Console.WriteLine("'" + oldVersionPath + "' konnte nicht gefunden werden!");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("'" + oldVersionPath + "' konnte nicht gefunden werden!");
                Console.WriteLine("Exception:\n" + ex.ToString());
                Console.ReadKey();
            }
        }
    }
}