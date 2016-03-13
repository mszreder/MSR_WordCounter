using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WordCounter.BusinessLogic;
using System.IO;
using System.Diagnostics;
using System;

namespace WordCounter.Test.BusinessLogicTests
{
    [TestClass]
    public class WordCounterUtilityTests
    {
        private WordCounterUtility CreateInstance()
        {
            return new WordCounterUtility(ConfigurationManager.GetWordSeparatorsCharacters(), ConfigurationManager.GetWordTrimChars(), new Dictionary<string, int>()); ;
        }

        private void AssertCorrectness(IDictionary<string,int> calculated, IDictionary<string,int> expected)
        {
            Assert.AreEqual(expected.Count, calculated.Count, "Different number of words calculated");

            foreach (var correctAnswerElement in expected)
            {
                Assert.IsTrue(calculated.ContainsKey(correctAnswerElement.Key), string.Format("{0} element not found in calculatedAnswer",correctAnswerElement.Key));

                Assert.AreEqual(correctAnswerElement.Value, calculated[correctAnswerElement.Key],
                    string.Format("{0} element value is {1} but is should be {2}", correctAnswerElement.Key, calculated[correctAnswerElement.Key], correctAnswerElement.Value));
            }
        }

        [TestMethod]
        public void ExcerciseExampleTest()
        {
            string sentence = "This is a statement, and so is this.";

            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["this"] = 2;
            correctAnswer["is"] = 2;
            correctAnswer["a"] = 1;
            correctAnswer["statement"] = 1;
            correctAnswer["and"] = 1;
            correctAnswer["so"] = 1;

            AssertCorrectness(calculatedAnswer, calculatedAnswer);
               
        }

        [TestMethod]
        public void WordWithDashTest()
        {
            string sentence = "This is cross-section of the public.";

            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["this"] = 1;
            correctAnswer["is"] = 1;
            correctAnswer["cross-section"] = 1;
            correctAnswer["of"] = 1;
            correctAnswer["the"] = 1;
            correctAnswer["public"] = 1;

            AssertCorrectness(calculatedAnswer, calculatedAnswer);
        }

        [TestMethod]
        public void EmailAddressTest()
        {
            string sentence = "Further information or details can be sent to some@email.com, someOtherEmail@domain.com or  some@email.com";

            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["further"] = 1;
            correctAnswer["information"] = 1;
            correctAnswer["or"] = 2;
            correctAnswer["details"] = 1;
            correctAnswer["can"] = 1;
            correctAnswer["be"] = 1;
            correctAnswer["sent"] = 1;
            correctAnswer["to"] = 1;
            correctAnswer["some@email.com"] = 2;
            correctAnswer["someOtherEmail@domain.com"] = 1;


            AssertCorrectness(calculatedAnswer, calculatedAnswer);
        }

        [TestMethod]
        public void EmptyStringTest()
        {
            string sequence = string.Empty;
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            Assert.AreEqual(0, calculatedAnswer.Count, "Some word found - expected empty");
        }

        [TestMethod]
        public void OnlyDashesTest()
        {
            string sequence = "--------";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            Assert.AreEqual(0, calculatedAnswer.Count, "Some word found - expected empty");
        }

        [TestMethod]
        public void MultiLineHandlingTest()
        {
            string sequence = @"Some Multiline
sequence in
test.";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["some"] = 1;
            correctAnswer["multiline"] = 1;
            correctAnswer["sequence"] = 1;
            correctAnswer["in"] = 1;
            correctAnswer["test"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void MultiLineDialogTest()
        {
            string sequence = @"Dialog (me and you):
- hi,
-Hi you too!
-bye";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["dialog"] = 1;
            correctAnswer["me"] = 1;
            correctAnswer["and"] = 1;
            correctAnswer["you"] = 2;
            correctAnswer["hi"] = 2;
            correctAnswer["too"] = 1;
            correctAnswer["bye"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void OneWordSequenceTest()
        {
            string sequence = "Die";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["die"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void OneWordWithAdditionalCharAtTheEnd()
        {
            string sequence = "Die!";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["die"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void OneWordWithSpecialCharAtTheEndWithSpaceBetween()
        {
            string sequence = "Die !";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["die"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void NullStringTest()
        {
            string sequence = null;
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sequence);

            Assert.IsTrue(calculatedAnswer.Count == 0, "Expected empty");
        }

        [TestMethod]
        public void MultipleWordBreakersWitBeginWithSpaceTests()
        {
            string sentence = "abc   aftertriplespace        afterdoubletab.   aftertriplespace and     afterdoubletab";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);
         
            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["abc"] = 1;
            correctAnswer["aftertriplespace"] = 2;
            correctAnswer["afterdoubletab"] = 2;
            correctAnswer["and"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void StrangeFrechWordTest()
        {
            string sentence = "Qu'est-ce que ?";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["qu'est-ce"] = 1;
            correctAnswer["que"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void FrenchInterpunctionTest()
        {
            string sentence = "	Les enfants  les règles d'orthographe à l'école primaire. Qu'est-ce que ?";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["les"] = 2;
            correctAnswer["enfants"] = 1;
            correctAnswer["règles"] = 1;
            correctAnswer["d'orthographe"] = 1;
            correctAnswer["à"] = 1;
            correctAnswer["l'école"] = 1;
            correctAnswer["primaire"] = 1;
            correctAnswer["qu'est-ce"] = 1;
            correctAnswer["que"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }

        [TestMethod]
        public void MultipleSentencesTest()
        {
            string sentence = "abc, abc - dbe. Dbe and ref. Abc.";
            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();

            IWordCounter wc = CreateInstance();
            calculatedAnswer = wc.CountWordsInStringSequence(sentence);

            IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["abc"] = 3;
            correctAnswer["dbe"] = 2;
            correctAnswer["and"] = 1;
            correctAnswer["ref"] = 1;
            AssertCorrectness(calculatedAnswer, correctAnswer);
        }
        
        [TestMethod]
        public void ExecutionTimeTest()
        {
            string fileName = "BigFileTest.txt";

            try
            {
                File.Delete(fileName);
            }
            catch
            { };
            
            int iterations = 1000000;
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                for(int i=0; i< iterations; i++)
                {
                    sw.WriteLine("abc, def - ghi. Abc. ");
                }
                sw.Close();
            }

            IDictionary<string, int> calculatedAnswer = new Dictionary<string, int>();
            IWordCounter wc = CreateInstance();
            Stopwatch stopwatch = new Stopwatch();
            //0 current
            //1 - next
            using (StreamReader sr = new StreamReader(fileName))
            {
               
                stopwatch.Start();

                bool isLastChar = false;
                int[] readValues = new int[2] { -1, -1 };

                readValues[0] = sr.Read();
                readValues[1] = sr.Read();

                if (readValues[0]==-1)
                {
                    return;
                }

                if (readValues[0] != -1 && readValues[1] == -1)
                {
                    isLastChar = true;
                }

                wc.CalculateChar((char)readValues[0], isLastChar);

                while(isLastChar == false)
                {
                    readValues[0] = readValues[1];
                    readValues[1] = sr.Read();
                    if (readValues[0] != -1 && readValues[1] == -1)
                    {
                        isLastChar = true;
                    }
                    wc.CalculateChar((char)readValues[0], isLastChar);

                }

                calculatedAnswer = wc.GetCurrentFinishedWordCounter();
                stopwatch.Stop();
                sr.Close();
            }
            
                IDictionary<string, int> correctAnswer = new Dictionary<string, int>();
            correctAnswer["abc"] = 2*iterations;
            correctAnswer["def"] = 1 * iterations;
            correctAnswer["ghi"] = 1 * iterations;
            AssertCorrectness(calculatedAnswer, correctAnswer);

            long elapsedTimeinMs = stopwatch.ElapsedMilliseconds;
            long expectedElapsedTime = (long)(0.006 * iterations);
            Assert.IsFalse(elapsedTimeinMs > expectedElapsedTime, string.Format("Processing time too long. Actual: {0}, Expected {1}",elapsedTimeinMs,expectedElapsedTime));
        }
    }
}
