using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter.BusinessLogic
{
    /// <summary>
    /// Class responisble for reading the configuration. Currently hardcoded but depending on solution might 
    /// implement different ways of fethcing configuration values
    /// </summary>
    public static class ConfigurationManager
    {
        public static List<char> GetWordSeparatorsCharacters()
        {
            return new List<char>() { ' ', '\t', '\n', '\r' };
        }

        public static List<char> GetWordTrimChars()
        {
            return new List<char> { ',', '.', '-', ':', ';', '\\', '/', '"', '|', '?', '!', '*', '(', ')', '\r' };
        }
    }
}
