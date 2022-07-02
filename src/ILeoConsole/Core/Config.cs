using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ILeoConsole.Core
{
    public class Config
    {
        #region CreateConfig

        public static string GetConfigPath(string SavePath, string PluginName)
        {
            string path = Path.Combine(SavePath, "var", PluginName);
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            return path;
        }

        public static void WriteConfig(string configPath, string fileName, string configContent)
        {
            string path = Path.Combine(configPath, fileName);

            File.WriteAllText(path, configContent);
        }

        public static string ReadConfig(string configPath, string fileName)
        {
            string path = Path.Combine(configPath, fileName);
            return File.ReadAllText(path);
        }

        #endregion

        #region ConfigReadFunctions

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

        #endregion
    }
}
