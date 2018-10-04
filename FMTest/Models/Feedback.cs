using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace FMTest.Models
{
    public class Feedback
    {
        public string Name { set; get; }
        public string Email { set; get; }
        public string Message { set; get; }
    }
}