using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole;
using ILeoConsole.Core;
using ILeoConsole.Localization;

namespace LeoConsole
{
    public class UserFunctions
    {
        Data data;

        public UserFunctions(Data data)
        {
            this.data = data;
        }

        public void newKonto(bool root, List<User> users)
        {
            User newUser = new User();

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_userNew"));

            while (true)
            {
                Console.Write(LocalisationManager.GetLocalizationFromKey("lc_username") + ": ");
                string userName = Console.ReadLine();

                bool same = false;
                foreach(User user in users)
                {
                    if (userName == user.name)
                    {
                        Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_userNameTaken"));

                        same = true;
                        break;
                    }
                }

                if (!same) { newUser.name = userName; break; }
            }

            Console.Write(LocalisationManager.GetLocalizationFromKey("lc_greeting") + ": ");
            newUser.begrüßung = Console.ReadLine();

            bool passwordCheck = false;
            do
            {
                Console.Write("Password: ");
                var pass = string.Empty;
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        Console.Write("\b \b");
                        pass = pass.Substring(0, pass.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        pass += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.WriteLine("");
                Console.Write("Confirm Password: ");
                var passCheck = string.Empty;
                ConsoleKey keyCheck;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    keyCheck = keyInfo.Key;

                    if (keyCheck == ConsoleKey.Backspace && passCheck.Length > 0)
                    {
                        Console.Write("\b \b");
                        passCheck = passCheck.Substring(0, passCheck.Length - 1);
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        passCheck += keyInfo.KeyChar;
                    }
                } while (keyCheck != ConsoleKey.Enter);

                if (pass == passCheck)
                {
                    newUser.password = pass;
                    passwordCheck = true;
                }
                else
                {
                    Console.WriteLine("Wrong Password!");
                }
            } while (!passwordCheck);

            newUser.root = root;

            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_userFinish"));
            Console.ReadKey();

            users.Add(newUser);
            SaveLoad.saveUsers(users, data.SavePath);
            Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_userReboot"));
            Console.ReadKey();

            LeoConsole leoConsole = new LeoConsole();
            leoConsole.reboot();
        }

        public User UserLogin(List<User> users)
        {
            LConsole.WriteLine("[§aLogin§r]");

            User result = null;

            Dictionary<string, string> userInfos = new Dictionary<string, string>();
            foreach (User u in users)
            {
                userInfos.Add(u.name, u.password);
            }

            do
            {
                Console.Write("Username: ");
                string username = Console.ReadLine();
                if (userInfos.ContainsKey(username))
                {
                    Console.Write("Password: ");
                    var pass = string.Empty;
                    ConsoleKey key;
                    do
                    {
                        var keyInfo = Console.ReadKey(intercept: true);
                        key = keyInfo.Key;

                        if (key == ConsoleKey.Backspace && pass.Length > 0)
                        {
                            Console.Write("\b \b");
                            pass = pass.Substring(0, pass.Length - 1);
                        }
                        else if (!char.IsControl(keyInfo.KeyChar))
                        {
                            Console.Write("*");
                            pass += keyInfo.KeyChar;
                        }
                    } while (key != ConsoleKey.Enter);

                    Console.WriteLine("");

                    if (userInfos.Contains(new KeyValuePair<string, string>(username, pass)))
                    {
                        foreach (User u in users)
                        {
                            if (u.name == username && u.password == pass) { result = u; break; }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Password!\n");
                    }
                }
                else
                {
                    Console.WriteLine(LocalisationManager.GetLocalizationFromKey("lc_userDoesNotExist"));
                }

                if (result != null)
                {
                    break;
                }

            } while (true);

            foreach(IData data in LeoConsole.datas)
            {
                data.User = result;
            }

            return result;
        }
    }
}
