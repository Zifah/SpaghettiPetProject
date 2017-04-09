using Newtonsoft.Json;
using System.Collections.Generic;

namespace FMTest.Models
{
    public class ParagraphOutput
    {

        [JsonProperty("id")]
        public int Id { set; get; }

        [JsonProperty("words")]
        public string[] Words { set; get; }

        [JsonProperty("wordSyllables")]
        public Dictionary<string, string[]> WordSyllables { set; get; }

        [JsonProperty("syllableForms")]
        public Dictionary<string, string[]> SyllableForms { set; get; }
    }
}