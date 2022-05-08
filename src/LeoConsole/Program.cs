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

            Console.WriteLine("LeoConsole has been terminated");
            Console.ReadLine();
        }

        private static void FinishUpdate(string oldVersionPath)
        {
            try
            {
                DirectoryInfo oldDir = new DirectoryInfo(oldVersionPath);
                if (oldDir.Exists)
                {
                    Console.WriteLine("Leo Console has been updated successfully!");
                    Console.WriteLine("Old Installation: " + oldDir.FullName);
                    Console.WriteLine("Do you want to uninstall the old installation? y/n");
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
                            Console.WriteLine("The command '" + text + "' is either misspelled or could not be found.");
                        }

                    } while (true);
                }
                else
                {
                    Console.WriteLine("'" + oldVersionPath + "' could not be found!");
                    Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("'" + oldVersionPath + "' could not be found!");
                Console.WriteLine("Exception:\n" + ex.ToString());
                Console.ReadKey();
            }
        }
    }
}