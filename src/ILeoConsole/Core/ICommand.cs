using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Core
{
    public interface ICommand
    {
        public string Name { get; }
        public string Description { get; }
        public Action CommandFunktion { get; }
        public string[] InputProperties { get; set; }

        public void Execute(string[] properties)
        {
            InputProperties = properties;
            CommandFunktion();
        }
    }
}
