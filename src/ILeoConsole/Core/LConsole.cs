﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Core
{
    public class LConsole
    {

        #region Write

        public static void WriteLine(string value)
        {
            Char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                switch (chars[i])
                {
                    case '§':
                        switch (chars[i + 1])
                        {
                            case '0': Console.ForegroundColor = ConsoleColor.Black; i++; break;

                            case '1': Console.ForegroundColor = ConsoleColor.DarkBlue; i++; break;

                            case '2': Console.ForegroundColor = ConsoleColor.DarkGreen; i++; break;

                            case '3': Console.ForegroundColor = ConsoleColor.DarkCyan; i++; break;

                            case '4': Console.ForegroundColor = ConsoleColor.DarkRed; i++; break;

                            case '5': Console.ForegroundColor = ConsoleColor.DarkMagenta; i++; break;

                            case '6': Console.ForegroundColor = ConsoleColor.DarkYellow; i++; break;

                            case '7': Console.ForegroundColor = ConsoleColor.Gray; i++; break;

                            case '8': Console.ForegroundColor = ConsoleColor.DarkGray; i++; break;

                            case '9': Console.ForegroundColor = ConsoleColor.Blue; i++; break;

                            case 'a': Console.ForegroundColor = ConsoleColor.Green; i++; break;

                            case 'b': Console.ForegroundColor = ConsoleColor.Cyan; i++; break;

                            case 'c': Console.ForegroundColor = ConsoleColor.Red; i++; break;

                            case 'd': Console.ForegroundColor = ConsoleColor.Magenta; i++; break;

                            case 'e': Console.ForegroundColor = ConsoleColor.Yellow; i++; break;

                            case 'f': Console.ForegroundColor = ConsoleColor.White; i++; break;

                            case 'r': Console.ResetColor(); i++; break;

                            default: break;
                        }
                        break;

                    default:
                        Console.Write(chars[i]);
                        break;
                }
            }

            Console.Write("\n");
            Console.ResetColor();
        }

        public static void Write(string value)
        {
            Char[] chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                switch (chars[i])
                {
                    case '§':
                        switch (chars[i + 1])
                        {
                            case '0': Console.ForegroundColor = ConsoleColor.Black; i++; break;

                            case '1': Console.ForegroundColor = ConsoleColor.DarkBlue; i++; break;

                            case '2': Console.ForegroundColor = ConsoleColor.DarkGreen; i++; break;

                            case '3': Console.ForegroundColor = ConsoleColor.DarkCyan; i++; break;

                            case '4': Console.ForegroundColor = ConsoleColor.DarkRed; i++; break;

                            case '5': Console.ForegroundColor = ConsoleColor.DarkMagenta; i++; break;

                            case '6': Console.ForegroundColor = ConsoleColor.DarkYellow; i++; break;

                            case '7': Console.ForegroundColor = ConsoleColor.Gray; i++; break;

                            case '8': Console.ForegroundColor = ConsoleColor.DarkGray; i++; break;

                            case '9': Console.ForegroundColor = ConsoleColor.Blue; i++; break;

                            case 'a': Console.ForegroundColor = ConsoleColor.Green; i++; break;

                            case 'b': Console.ForegroundColor = ConsoleColor.Cyan; i++; break;

                            case 'c': Console.ForegroundColor = ConsoleColor.Red; i++; break;

                            case 'd': Console.ForegroundColor = ConsoleColor.Magenta; i++; break;

                            case 'e': Console.ForegroundColor = ConsoleColor.Yellow; i++; break;

                            case 'f': Console.ForegroundColor = ConsoleColor.White; i++; break;

                            case 'r': Console.ResetColor(); i++; break;

                            default: break;
                        }
                        break;

                    default:
                        Console.Write(chars[i]);
                        break;
                }
            }

            Console.ResetColor();
        }

        #endregion

        #region Read

        public static int CommandBuffer = 100;
        static int historyPosition = 0;
        public static List<string> CommandHistory = new List<string>();

        public static string ReadLine(List<ICommand> commands)
        {
            var input = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && input.Length > 0)
                {
                    Console.Write("\b \b");
                    input = input.Substring(0, input.Length - 1);
                }
                else if(key == ConsoleKey.Tab)
                {
                    int options = 0;
                    List<string> names = new List<string>();
                    foreach(ICommand command in commands)
                    {
                        if(command.Name.StartsWith(input, StringComparison.OrdinalIgnoreCase)) { options++; names.Add(command.Name); }
                    }

                    if(options > 1)
                    {
                        Console.WriteLine();
                        for(int i = 0; i < names.Count; i++)
                        {
                            Console.Write(names.ToArray()[i] + ", ");
                        }
                        AddToCommandHistory(input);
                        input = string.Empty;
                        break;
                    }
                    else if(options > 0)
                    {
                        string command = names[0];
                        string cmd = command.Substring(input.Length, command.Length - input.Length);
                        Console.Write(cmd);
                        input += cmd;
                    }
                }
                else if(key == ConsoleKey.LeftArrow) 
                {
                    if(Console.CursorLeft > 0)
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if(Console.CursorLeft < Console.BufferWidth - 1)
                    {
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if(CommandHistory.Count > 0 && CommandHistory.Count > historyPosition + 1 && historyPosition + 1 < CommandBuffer)
                    {
                        historyPosition++;
                        for(int i = 0; i < input.Length; i++)
                        {
                            Console.Write("\b \b");
                        }
                        Console.Write(CommandHistory[historyPosition]);
                        input = CommandHistory[historyPosition];
                    }
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    if (CommandHistory.Count > 0 && CommandHistory.Count > historyPosition - 1 && historyPosition - 1 >= 0)
                    {
                        historyPosition--;
                        for (int i = 0; i < input.Length; i++)
                        {
                            Console.Write("\b \b");
                        }
                        Console.Write(CommandHistory[historyPosition]);
                        input = CommandHistory[historyPosition];
                    }
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(keyInfo.KeyChar);
                    input += keyInfo.KeyChar;
                }

            } while (key != ConsoleKey.Enter);

            AddToCommandHistory(input);

            Console.WriteLine();

            return input;
        }

        static void AddToCommandHistory(string value)
        {
            if (CommandHistory.Count < CommandBuffer)
            {
                if (!CommandHistory.Contains(value)) { CommandHistory.Add(value); historyPosition++; }
                else { historyPosition--; }
            }
            else
            {
                CommandHistory.Remove(CommandHistory[0]);
                if (!CommandHistory.Contains(value)) { CommandHistory.Add(value); historyPosition++; }
                else { historyPosition--; }
            }
        }

        #endregion

    }
}
