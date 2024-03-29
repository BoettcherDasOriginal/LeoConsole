﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Core
{
    public interface ICommand
    {
        /// <summary>
        /// The name of your command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The description of your command
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The main function of your command
        /// </summary>
        public Action CommandFunction { get; }

        /// <summary>
        /// The Help Information
        /// </summary>
        public Action HelpFunction { get; }

        /// <summary>
        /// The given parameter from the user
        /// </summary>
        public string[] Arguments { get; set; }

        public void Execute(string[] properties)
        {
            Arguments = properties;
            CommandFunction();
        }
    }
}
