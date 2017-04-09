using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FMTest.Models
{
    public class Paragraph
    {
        [JsonProperty("content")]
        public string Content { set; get; }
    }
}