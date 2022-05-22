using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Core
{
    public class Config
    {
        /// <summary>
        /// Returns array of lines between #category_name: and #end in the raw config text
        /// </summary>
        /// <param name="configData">Raw Text from your config file</param>
        /// <param name="CategoryName"></param>
        /// <returns></returns>
        public static string[] GetCategory(string configData, string CategoryName)
        {
            string data = configData.ReplaceLineEndings("\n");

            string startTag = $"#{CategoryName}:\n";
            string endTag = "\n#end";
            return Utils.GetTextBetweenTags(data, startTag, endTag).Split('\n');
        }
    }
}
