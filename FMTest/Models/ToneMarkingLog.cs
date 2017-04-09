using FMTest.Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Models
{
    public class ToneMarkingLog
    {
        [JsonProperty("id")]
        public virtual int Id { set; get; }


        [JsonProperty("input")]
        [RdbmsName("Input")]
        public virtual string Input { set; get; }
        
        [JsonProperty("output")]
        [RdbmsName("Output")]
        public virtual string Output { set; get; }


        //[JsonProperty("clientIp")]
        [JsonIgnore()]
        [RdbmsName("ClientIp")]
        public string ClientIp { get; set; }


        [RdbmsName("LastModifiedDate")]
        [JsonIgnore()]
        public DateTime LastModifiedDate { get; set; }
    }
}