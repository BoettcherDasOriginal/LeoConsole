using System.Diagnostics;

namespace ILeoConsole.Core
{
    public class Processes
    {
        public static int Run(string name, string args, string pwd)
        {
            try
            {
                Process p = new Process();

                p.StartInfo.FileName = name;
                p.StartInfo.Arguments = args;
                p.StartInfo.WorkingDirectory = pwd;

                p.Start();
                p.WaitForExit();

                return p.ExitCode;
            }
            catch
            {
                return -1;
            }
            return -1;
        }

        public static string CheckOutput(string name, string args, string pwd)
        {
            try
            {
                Process p = new Process();

                p.StartInfo.FileName = name;
                p.StartInfo.Arguments = args;
                p.StartInfo.WorkingDirectory = pwd;
                p.StartInfo.RedirectStandardOutput = true;

                p.Start();
                string data = p.StandardOutput.ReadToEnd();
                p.WaitForExit();

                return data;
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}
