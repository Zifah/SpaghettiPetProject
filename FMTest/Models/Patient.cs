using FMTest.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Models
{
    public class Patient
    {
        public Patient()
        {
        }

        [RdbmsName("Name")]
        public virtual string Name { set; get; }

        [RdbmsName("Hospital")]
        public virtual string Hospital { set; get; }

        [RdbmsName("Age")]
        public virtual int Age { set; get; }


        [RdbmsName("Address")]
        public virtual string Address { set; get; }


        [RdbmsName("CreationTime")]
        public virtual DateTime CreationTime
        {
            set; get;
        }


        public virtual string Created
        {
            get
            {
                return CreationTime.ToString("dd MMM yyyy");
            }
        }
    }
}