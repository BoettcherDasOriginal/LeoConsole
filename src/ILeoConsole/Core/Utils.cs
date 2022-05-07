using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Core
{
    public class Utils
    {
        #region STRING FUNCTIONS

        public static string GetTextBetweenTags(string text, string start_tag, string end_tag)
        {
            return text.Split(start_tag)[1].Split(end_tag)[0];
        }

        public static string ReplaceTextBetweenTags(string text, string replacement, string start_tag, string end_tag)
        {
            int start_index = text.IndexOf(start_tag);
            int end_index = start_index + text.Substring(start_index + 1).IndexOf(end_tag);
            return text.Substring(0, start_index) + replacement + text.Substring(end_index + 2);
        }

        #endregion

        #region INPUT HANDLER TOOLS

        public static string[] HandelApostropheInput(string input)
        {
            input = HandelSpace(input);
            string[] output = input.Split(' ');

            for (int i = 0; i < output.Length; i++)
            {
                string propertie = string.Empty;

                if (output[i].Contains('@'))
                {
                    propertie = Utils.GetTextBetweenTags(output[i], "@", "@");
                    string replacement = propertie;
                    switch (propertie)
                    {
                        case "space":
                            replacement = " ";
                            break;
                    }

                    output[i] = Utils.ReplaceTextBetweenTags(output[i], replacement, "@", "@");
                }
            }

            return output;
        }

        private static string HandelSpace(string input)
        {
            if (input.Contains('\''))
            {
                string propertie = Utils.GetTextBetweenTags(input, "\'", "\'");
                propertie = propertie.Replace(" ", "@space@");
                return HandelSpace(Utils.ReplaceTextBetweenTags(input, propertie, "\'", "\'"));
            }
            else
            {
                return input;
            }
        }

        #endregion
    }
}
