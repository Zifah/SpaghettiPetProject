using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Utilities
{
    public class YorubaHelper
    {
        static readonly char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };

        static readonly string[] wordsEndingWithN = new string[]
        { "dan", "gan", "han", "jan", "kan", "ran", "san", "tan", "yan", "din", "fin", "gin", "jin", "min", "pin", "rin", "sin",
            "yin", "fon", "pon", "won", "bun", "dun", "fun", "gun", "kun", "run", "sun", "tun", "wun", "yun", "gbon", "gbin" };

        static Dictionary<string, string[]> vowelForms = new Dictionary<string, string[]>()
        {
            { "a", new string[] { "à", "a", "á" } },
            { "e", new string[] { "è", "ẹ̀", "e", "ẹ", "é", "ẹ́" } },
            { "i", new string[] { "ì", "i", "í" } },
            { "o", new string[] { "ò", "ọ̀", "o", "ọ", "ó", "ọ́" } },
            { "u", new string[] { "ù", "u", "ú" } },
            { "A", new string[] { "À", "A", "Á" } },
            { "E", new string[] { "È", "È̩", "E", "Ẹ", "É", "Ẹ́" } },
            { "I", new string[] { "Ì", "I", "Í" } },
            { "O", new string[] { "Ò", "Ọ̀", "O", "Ọ", "Ó", "Ọ́" } },
            { "U", new string[] { "Ù", "U", "Ú" } },
            { "s", new string[] { "s", "ṣ" } },
            { "S", new string[] { "S", "Ṣ" } }
        };

        static Dictionary<string, string[]> nForms = new Dictionary<string, string[]>()
        {
            { "n", new string[] { "n", "ń" } },
            { "N", new string[] { "N", "Ń" } }
        };

        public static string[] SyllabicateWord(string word)
        {
            // if word is one character, return word
            if (string.IsNullOrWhiteSpace(word))
            {
                return new string[] { };
            }

            var wordLength = word.Length;

            // if word is two characters, 
            // both vowels: split
            // else return whole word
            if (wordLength == 2)
            {
                if (IsVowel(word[0]) && IsVowel(word[1]))
                {
                    return new string[] { word[0].ToString(), word[1].ToString() };
                }

                return new string[] { word };
            }

            var tempHolder = "";
            var syllables = new List<string>();
            // three words or more
            // stop when
            // current character is vowel and next character is not n and previous character is not consonant
            // 
            // current character is n and preceding word is a vowel and next word is none or
            // current character is n and the two preceding words with n form a valid three letter word and next character is a consonant
            for (int i = 0; i < wordLength; i++)
            {
                var currentChar = word[i];
                tempHolder = $"{tempHolder}{currentChar}";

                if (i == wordLength - 1)
                {
                    syllables.Add(tempHolder);
                    tempHolder = "";
                    continue;
                }

                var nextChar = word[i + 1];

                if (IsVowel(currentChar) &&
                    (i == 0
                    || (nextChar != 'n' && nextChar != 'N')
                    || (!wordsEndingWithN.Contains($"{tempHolder}n".ToLower()) && i + 2 != wordLength)))
                {
                    syllables.Add(tempHolder);
                    tempHolder = "";
                    continue;
                }

                if ((currentChar == 'n' || currentChar == 'N') && (!IsVowel(nextChar) || (wordsEndingWithN.Contains(tempHolder))))
                {
                    syllables.Add(tempHolder);
                    tempHolder = "";
                    continue;
                }

                // CONSIDERATION FOR ADDING THIS IS ONGOING
                //if (currentChar == 'm' && (!IsVowel(nextChar)))
                //{
                //    syllables.Add(tempHolder);
                //    tempHolder = "";
                //    continue;
                //}
            }

            return syllables.ToArray();
        }

        public static string[] ConjugateSyllable(string word)
        {
            if (word.ToLower() == "n")
            {
                return nForms[word];
            }

            var allForms = new List<string> { { word } };

            for (int i = 0; i < word.Length; i++)
            {
                var currentLetter = word[i];

                if (vowelForms.Keys.ToList().Contains(Convert.ToString(currentLetter)))
                {
                    var theArray = vowelForms[Convert.ToString(currentLetter)];
                    var newMembers = new List<string>();

                    for (int j = 0; j < allForms.Count; j++)
                    {
                        var currentForm = allForms[j].ToList();
                        int lastCharLength = 1;

                        for (int k = 0; k < theArray.Length; k++)
                        {
                            for (int m = 0; m < lastCharLength; m++)
                            {
                                currentForm.RemoveAt(i);
                            }

                            for (int l = 0; l < theArray[k].Length; l++)
                            {
                                currentForm.Insert(l + i, theArray[k][l]);
                            }

                            lastCharLength = theArray[k].Length;
                            newMembers.Add(String.Join("", currentForm));
                        }
                    }
                    allForms.AddRange(newMembers);
                }
            }

            var result = allForms.Distinct().ToList();

            #region This is to swap the positions of the first neutral conjugate with the first do conjugate
            if (result.Count > 2 && result.Count % 3 == 0)
            {
                string temp = allForms[0];
                result.RemoveAt(0);
                result.Insert((result.Count + 1) / 3, temp);
            } 
            #endregion

            return result.ToArray();
        }

        static bool IsVowel(char subject)
        {
            return vowels.Contains(subject);
        }
    }
}