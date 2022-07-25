using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        #region JConfig
        public static Dictionary<string,string> JConfigGetAll(string pluginName, string savePath)
        {
            string configFile = Path.Combine(GetConfigPath(savePath, pluginName), "config.json");
            if (!File.Exists(configFile))
            {
                return new Dictionary<string,string>();
            }

            try
            {
                string content = File.ReadAllText(configFile);
                return JsonSerializer.Deserialize<Dictionary<string,string>>(content);
            }
            catch
            {
                return new Dictionary<string,string>();
            }
        }

        public static string JConfigGet(string pluginName, string savePath, string key)
        {
            JConfigGetAll(pluginName, savePath).TryGetValue(key, out string val);
            return val;
        }

        public static void JConfigSet(string pluginName, string savePath, string key, string val)
        {
            Dictionary<string,string> oldConfig = JConfigGetAll(pluginName, savePath);
            oldConfig[key] = val;

            string configFile = Path.Combine(GetConfigPath(savePath, pluginName), "config.json");
            string configString = JsonSerializer.Serialize(oldConfig);
            File.WriteAllText(configFile, configString);
        }

        #endregion
    }
}
