using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LeoConsole
{
    public static class SaveLoad
    {
        public static void savePath(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Data data = new Data();
            
            if(!Directory.Exists(data.DefaultSavePath))
            {
                Directory.CreateDirectory(data.DefaultSavePath);
            }

            FileStream stream = new FileStream(data.DefaultSavePath + "SavePath.lcs", FileMode.Create);

            formatter.Serialize(stream, path);
            stream.Close();
        }

        public static string LoadPath()
        {
            Data data = new Data();
            string newPath;

            if (!Directory.Exists(data.DefaultSavePath))
            {
                Directory.CreateDirectory(data.DefaultSavePath);
            }

            if (File.Exists(data.DefaultSavePath + "SavePath.lcs"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(data.DefaultSavePath + "SavePath.lcs", FileMode.Open);

                newPath = formatter.Deserialize(stream) as string;
                stream.Close();

                return newPath;
            }
            else
            {
                return data.DefaultSavePath;
            }
        }

        public static void saveUser(User user, string savePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Data data = new Data();

            FileStream stream = new FileStream(savePath + "user/" + "User.lcs", FileMode.Create);

            formatter.Serialize(stream, user);
            stream.Close();
        }

        public static User LoadUser(string savePath)
        {
            Data data = new Data();

            if(!Directory.Exists(savePath + "user/"))
            {
                Directory.CreateDirectory(savePath + "user/");
            }

            if(File.Exists(savePath + "user/" + "User.lcs"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(savePath + "user/" + "User.lcs", FileMode.Open);

                User user = formatter.Deserialize(stream) as User;
                stream.Close();

                return user;
            }
            else
            {
                return null;
            }
        }
    }
}
