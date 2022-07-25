using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ILeoConsole.Core;

namespace LeoConsole
{
    public static class SaveLoad
    {
        public static void saveUsers(List<User> users, string savePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(Path.Combine(savePath, "var", "LeoConsole", "Users.lcs"), FileMode.Create);

            formatter.Serialize(stream, users);
            stream.Close();
        }

        public static List<User> LoadUsers(string savePath)
        {
            if (!Directory.Exists(Path.Combine(savePath, "var", "LeoConsole")))
            {
                Directory.CreateDirectory(Path.Combine(savePath, "var", "LeoConsole"));
            }

            if (File.Exists(Path.Combine(savePath, "var", "LeoConsole", "Users.lcs")))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(Path.Combine(savePath, "var", "LeoConsole", "Users.lcs"), FileMode.Open);

                List<User> users = formatter.Deserialize(stream) as List<User>;
                stream.Close();

                return users;
            }
            else
            {
                return null;
            }
        }
    }
}
