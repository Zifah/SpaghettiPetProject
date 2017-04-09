using FMTest.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Models
{
    public class Company
    {
        [RdbmsName("Name")]
        public virtual string Name { set; get; }
        [RdbmsName("Industry")]
        public virtual string Industry { set; get; }

        [RdbmsName("CurrentPrice")]
        public int CurrentPrice { get; set; }
    }
}