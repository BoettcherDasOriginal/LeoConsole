using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using ILeoConsole.Core;
using ILeoConsole.Localization;
using ILeoConsole.Plugin;
using System.Diagnostics;

namespace LeoConsole
{
    public static class Reboot
    {
        public static void StartReboot(string savepath)
        {
            LConsole.MessageSuc0(LocalisationManager.GetLocalizationFromKey("lc_rebootStart"));
            
            foreach(IPlugin plugin in PluginLoader.Plugins)
            {
                plugin.PluginShutdown();
            }

            //Win
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string rebootFilePath = Path.Combine(savepath, "..", "reboot.bat");
                File.WriteAllText(rebootFilePath, ReadResource("win_reboot.bat"));

                string workingDirectory = Path.Combine(savepath, "..");

                Process currentProcesse = Process.GetCurrentProcess();
                string currentPid = currentProcesse.Id.ToString();

                string args = "\"" + workingDirectory + "\"" + " " + currentPid;
                Processes.Run(rebootFilePath, args, "");
            }
            else
            {
                LConsole.MessageErr0("currently not supported on this os!");
            }
        }

        static string ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!name.StartsWith(nameof(LeoConsole)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
