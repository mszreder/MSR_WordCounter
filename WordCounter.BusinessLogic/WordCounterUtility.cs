using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter.BusinessLogic
{
    public class WordCounterUtility : IWordCounter
    {
        private IDictionary<string, int> wordCountDictionary;

        private List<char> wordCharsTemp;

        private HashSet<char> wordSeparatorChars;

        private char[] charactersToExcludeFromWordEndingAndBeginnig;

        private void SafeAddToDictionary(string word, IDictionary<string, int> counterDictionary)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }

            if (counterDictionary.ContainsKey(word))
            {
                counterDictionary[word]++;
            }
            else
            {
                counterDictionary[word] = 1;
            }
        }

        public WordCounterUtility(List<char> wordSeparatorChars, List<char> charsToTrim, IDictionary<string,int> counterDictionary)
        {
            this.wordSeparatorChars = new HashSet<char>(wordSeparatorChars);
            this.charactersToExcludeFromWordEndingAndBeginnig = charsToTrim.ToArray();
            this.wordCountDictionary = counterDictionary;
            this.wordCharsTemp = new List<char>();
        }
              
        /// <summary>
        /// Includes currentChar into calculation. If this is last char (driven by parameter) then finalize buuilding word.
        /// </summary>
        /// <param name="currentChar">current read char</param>
        /// <param name="isLastChar">indicates if current char is last char in sequence</param>
        public void CalculateChar(char currentChar, bool isLastChar)
        {
            char currentToLowerChar = Char.ToLowerInvariant(currentChar);

            if (wordSeparatorChars.Contains(currentToLowerChar)) //separator found
            {
                // If any wordChars are present - and separator found - we can build the word
                if (wordCharsTemp.Any())
                {
                    string word = new string(wordCharsTemp.ToArray()); //build word
                    word = word.Trim(charactersToExcludeFromWordEndingAndBeginnig); //remove chars not building the word
                    SafeAddToDictionary(word, wordCountDictionary);
                }
                
                //Reset wordCharsTemp 
                wordCharsTemp = new List<char>();
            }
            else
            {
                //this is char which can build the word (non word separator one)
                wordCharsTemp.Add(currentToLowerChar);

                if (isLastChar) 
                {
                    //If this is last character and there are any items in word char list then add it to the counter dictionary
                    if (wordCharsTemp.Any())
                    {
                        string word = new string(wordCharsTemp.ToArray()); //buld word
                        word = word.Trim(charactersToExcludeFromWordEndingAndBeginnig);

                        SafeAddToDictionary(word, wordCountDictionary);

                        wordCharsTemp = new List<char>();
                    }
                }

            }
        }

        /// <summary>
        /// Calculates number of word occurences in given string.
        /// </summary>
        /// <param name="sequence">sentence</param>
        /// <returns></returns>
        public IDictionary<string,int> CountWordsInStringSequence(string sequence)
        {
            this.Reset();
            if (string.IsNullOrWhiteSpace(sequence))
            {
                return wordCountDictionary;
            }
            
            bool isLastChar = sequence.Count() == 0;
            for (int i=0; i<sequence.Count(); i++)
            {
                char currentChar = sequence[i]; 
                isLastChar = i == (sequence.Count() - 1);
                CalculateChar(currentChar, isLastChar );
            }

            return wordCountDictionary;
        }

        /// <summary>
        /// Resetes counter
        /// </summary>
        public void Reset()
        {
            this.wordCharsTemp = new List<char>();
            this.wordCountDictionary.Clear();
        }

        public IDictionary<string,int> GetCurrentFinishedWordCounter()
        {
            return this.wordCountDictionary;
        }
    }
}
