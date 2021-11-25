using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Leo Console";

            LeoConsole leoConsole = new LeoConsole();
            leoConsole.neuStart();

            Console.WriteLine("LeoConsole wurde Beendet");
            Console.ReadLine();
        }
    }
}
