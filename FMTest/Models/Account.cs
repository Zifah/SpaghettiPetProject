using FMTest.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Models
{
    public class Account
    {
        public Account()
        {
            Stocks = new List<Portfolio>();
        }

        [RdbmsName("Number")]
        public virtual string Number { set; get; }

        [RdbmsName("CreationTime")]
        public virtual DateTime CreationTime { set; get; }

        [RdbmsName("CustomerId")]
        public virtual int CustomerId { set; get; }
        public virtual IList<Portfolio> Stocks { set; get; }
    }
}