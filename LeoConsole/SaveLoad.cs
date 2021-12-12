﻿using System;
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
        public static void saveUsers(List<User> users, string savePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Data data = new Data();

            FileStream stream = new FileStream(savePath + "user/" + "Users.lcs", FileMode.Create);

            formatter.Serialize(stream, users);
            stream.Close();
        }

        public static List<User> LoadUsers(string savePath)
        {
            Data data = new Data();

            if(!Directory.Exists(savePath + "user/"))
            {
                Directory.CreateDirectory(savePath + "user/");
            }

            if(File.Exists(savePath + "user/" + "Users.lcs"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(savePath + "user/" + "Users.lcs", FileMode.Open);

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
