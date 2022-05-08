using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILeoConsole.Localization
{
    public interface ILocalization
    {
        /// <summary>
        /// The language for the specified translations (for example "en" for english)
        /// </summary>
        string Language { get; }

        /// <summary>
        /// Contains your keys and there given translation
        /// </summary>
        string[,] Dictionary { get; set; }

        /// <summary>
        /// DictionaryInit() is called when LeoConsole loads this localization 
        /// </summary>
        void DictionaryInit();
    }
}
