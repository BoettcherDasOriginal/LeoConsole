using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Core
{
    public class LConsole
    {

        #region Write

        /// <summary>
        /// Writes the specified string value, followed by the current line terminator, and provides simple colored output
        /// </summary>
        /// <param name="value"></param>
        public static void WriteLine(string value)
        {
            Write(value + "\n");
        }

        /// <summary>
        /// Writes the specified string value and provides simple colored output
        /// </summary>
        /// <param name="value"></param>
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

        /// <summary>
        /// Reads the next line of characters and adds default UNIX console features (for example autocompletion and arrow-up history) 
        /// </summary>
        /// <param name="commands"></param>
        /// <returns>The next line of characters</returns>
        public static string ReadLine(List<ICommand> commands)
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            int rows = 0;

            var input = string.Empty;
            ConsoleKey key = ConsoleKey.NoName;
            ConsoleKey lastKey;
            do
            {
                int maxCursorX = cursorX + input.Length;
                int rowDif = Console.CursorTop - cursorY;
                if (rowDif > rows) { rows = rowDif; }

                var keyInfo = Console.ReadKey(intercept: true);
                lastKey = key;
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
                    if(Console.CursorTop == cursorY)
                    {
                        if (Console.CursorLeft > 0 && Console.CursorLeft > cursorX)
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                    }
                    else if(Console.CursorTop > cursorY)
                    {
                        if (Console.CursorLeft > 0)
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                        else
                        {
                            Console.SetCursorPosition(Console.BufferWidth - 1, Console.CursorTop - 1);
                        }
                    }
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if (Console.CursorTop == cursorY)
                    {
                        if (Console.CursorLeft < Console.BufferWidth - 1 && Console.CursorLeft < maxCursorX)
                        {
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        }
                        else if(Console.CursorTop < Console.CursorTop + rows)
                        {
                            Console.SetCursorPosition(1, Console.CursorTop + 1);
                        }
                    }
                    else if (Console.CursorTop > cursorY)
                    {
                        int newMaxCursorX = (rowDif * (Console.BufferWidth - 1) - (input.Length + cursorX - 1)) * -1;

                        if (Console.CursorLeft < Console.BufferWidth - 1 && Console.CursorLeft < newMaxCursorX)
                        {
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                        }
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if(CommandHistory.Count > 0 && CommandHistory.Count > historyPosition + 1 && historyPosition + 1 < CommandBuffer)
                    {
                        historyPosition++;
                        if(historyPosition < 0) { historyPosition = 0; }
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

            if(lastKey == ConsoleKey.UpArrow) { historyPosition += 2; }
            else if(lastKey == ConsoleKey.DownArrow) { historyPosition -= 2; }

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

        /// <summary>
        /// Prints a yes/no dialog with the given message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="acceptEnter"></param>
        /// <returns>Returns TRUE if the user answers yes</returns>
        public static bool YesNoDialog(string message, bool acceptEnter = true)
        {
            Console.WriteLine(message + " Y/N");
            Console.Write(">");
            string anser = Console.ReadLine();

            if (acceptEnter)
            {
                switch (anser.ToLower())
                {
                    default:
                        Console.WriteLine("'" + anser + "' is not a valid answer!");
                        return false;
                        break;

                    case "n":
                        return false;
                        break;

                    case "":
                    case "y":
                        return true;
                        break;
                }
            }
            else
            {
                switch (anser.ToLower())
                {
                    default:
                        Console.WriteLine("'" + anser + "' is not a valid answer!");
                        return false;
                        break;

                    case "n":
                        return false;
                        break;

                    case "y":
                        return true;
                        break;
                }
            }
        }
    }
}
