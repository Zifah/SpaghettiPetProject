using FMTest.Business;
using System;
using System.Collections.Generic;

namespace FMTest.Models
{
    public class Customer
    {
        public Customer()
        {
            Accounts = new List<Account>();
        }

        public virtual int Id { set; get; }
        [RdbmsName("Name")]
        public virtual string Name { set; get; }
        [RdbmsName("CreationTime")]
        public virtual DateTime CreationTime { set; get; }
        [RdbmsName("DateOfBirth")]
        public virtual DateTime DateOfBirth { set; get; }
        [RdbmsName("Gender")]
        public virtual string Gender { set; get; }
        public virtual IList<Account> Accounts { set; get; }
        public virtual string Created
        {
            get
            {
                return CreationTime.ToString("dd MMM yyyy");
            }
        }
        public virtual int Age
        {
            get
            {
                var result = (DateTime.Today - DateOfBirth).Days / 365;
                return result;
            }
        }
    }
}