using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCounter.BusinessLogic
{
    public interface IWordCounter
    {
        void CalculateChar(char currentChar, bool isLastChar);

        IDictionary<string, int> CountWordsInStringSequence(string sequence);

        void Reset();

        IDictionary<string, int> GetCurrentFinishedWordCounter();
              
    }
}
