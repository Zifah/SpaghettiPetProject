using FMTest.Models;
using FMTest.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class YorubaSystem
    {
        public ParagraphOutput ProcessParagraph(string paragraph)
        {
            paragraph = paragraph.Trim();
            var words = paragraph.Split(' ');

            Dictionary<string, string[]> wordSyllables = new Dictionary<string, string[]>();
            Dictionary<string, string[]> syllableForms = new Dictionary<string, string[]>();
            List<string> allSyllables = new List<string>();

            foreach (var word in words.Distinct())
            {
                var syllables = YorubaHelper.SyllabicateWord(word);
                wordSyllables.Add(word, syllables);
                allSyllables.AddRange(syllables);
            }

            foreach (var syllable in allSyllables.Distinct())
            {
                syllableForms.Add(syllable, YorubaHelper.ConjugateSyllable(syllable));
            }

            ;

            var result = new ParagraphOutput
            {
                Id = SaveToneMarkingLog(paragraph),
                Words = words,
                SyllableForms = syllableForms,
                WordSyllables = wordSyllables
            };


            return result;
        }

        internal List<ToneMarkingLog> GetRecentlyProcessed()
        {
            return new DataSystem().GetProcessedYorubaWords();
        }

        public int SaveToneMarkingLog(string words)
        {
            try
            {
                return new DataSystem().SaveToneMarkingLog(words);
            }

            catch
            {
                return 0;
            }
        }

        public int UpdateToneMarkingLogs(ToneMarkingLog log)
        {

            try
            {
                log.ClientIp = HttpContext.Current.Request.Headers["X-Forwarded-For"] ?? HttpContext.Current.Request.UserHostAddress;
                return new DataSystem().UpdateToneMarkingLog(log);
            }

            catch
            {
                return 0;
            }
        }
    }
}