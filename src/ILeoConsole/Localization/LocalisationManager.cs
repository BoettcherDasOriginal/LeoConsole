using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Localization
{
    public class LocalisationManager
    {
        public static string Language = "en";

        public static List<ILocalization> Localizations = new List<ILocalization>();

        public static void Init()
        {
            foreach(ILocalization l in Localizations)
            {
                l.DictionaryInit();
            }
        }

        /// <summary>
        /// Returns the translation for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>KEY if the key does not exist</returns>
        public static string GetLocalizationFromKey(string key)
        {
            foreach(ILocalization localization in Localizations)
            {
                if(localization.Language == Language)
                {
                    for(int i = 0; i < localization.Dictionary.Length / 2; i++)
                    {
                        if(localization.Dictionary[i,0] == key)
                        {
                            return localization.Dictionary[i,1];
                        }
                    }
                }
            }

            return key;
        }
    }
}
