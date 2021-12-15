using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.LCConsole
{
    public static class LCConsole
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
                            case '0':
                                Console.ForegroundColor = ConsoleColor.Black;
                                i++;
                                break;

                            case '1':
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                i++;
                                break;

                            case '2':
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                i++;
                                break;

                            case '3':
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                i++;
                                break;

                            case '4':
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                i++;
                                break;

                            case '5':
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                i++;
                                break;

                            case '6':
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                i++;
                                break;

                            case '7':
                                Console.ForegroundColor = ConsoleColor.Gray;
                                i++;
                                break;

                            case '8':
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                i++;
                                break;

                            case '9':
                                Console.ForegroundColor = ConsoleColor.Blue;
                                i++;
                                break;

                            case 'a':
                                Console.ForegroundColor = ConsoleColor.Green;
                                i++;
                                break;

                            case 'b':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                i++;
                                break;

                            case 'c':
                                Console.ForegroundColor = ConsoleColor.Red;
                                i++;
                                break;

                            case 'd':
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                i++;
                                break;

                            case 'e':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                i++;
                                break;

                            case 'f':
                                Console.ForegroundColor = ConsoleColor.White;
                                i++;
                                break;

                            case 'r':
                                Console.ResetColor();
                                i++;
                                break;

                            default:
                                break;
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
                            case '0':
                                Console.ForegroundColor = ConsoleColor.Black;
                                i++;
                                break;

                            case '1':
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                i++;
                                break;

                            case '2':
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                i++;
                                break;

                            case '3':
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                i++;
                                break;

                            case '4':
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                i++;
                                break;

                            case '5':
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                i++;
                                break;

                            case '6':
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                i++;
                                break;

                            case '7':
                                Console.ForegroundColor = ConsoleColor.Gray;
                                i++;
                                break;

                            case '8':
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                i++;
                                break;

                            case '9':
                                Console.ForegroundColor = ConsoleColor.Blue;
                                i++;
                                break;

                            case 'a':
                                Console.ForegroundColor = ConsoleColor.Green;
                                i++;
                                break;

                            case 'b':
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                i++;
                                break;

                            case 'c':
                                Console.ForegroundColor = ConsoleColor.Red;
                                i++;
                                break;

                            case 'd':
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                i++;
                                break;

                            case 'e':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                i++;
                                break;

                            case 'f':
                                Console.ForegroundColor = ConsoleColor.White;
                                i++;
                                break;

                            case 'r':
                                Console.ResetColor();
                                i++;
                                break;

                            default:
                                break;
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
    }
}
