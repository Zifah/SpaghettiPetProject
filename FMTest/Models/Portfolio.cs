using FMTest.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Models
{
    public class Portfolio
    {
        [RdbmsName("AccountId")]
        public virtual int AccountId { set; get; }
        [RdbmsName("CompanyId")]
        public virtual int CompanyId { set; get; }
        [RdbmsName("PurchaseDate")]
        public virtual DateTime PurchaseDate { set; get; }
        [RdbmsName("PurchasePrice")]
        public virtual Decimal PurchasePrice { set; get; }
        [RdbmsName("CurrentPrice")]
        public virtual Decimal CurrentPrice { set; get; }
        [RdbmsName("CompanyName")]
        public virtual string CompanyName { set; get; }
        [RdbmsName("CompanyIndustry")]
        public virtual string CompanyIndustry { set; get; }

        [RdbmsName("AccountNumber")]
        public virtual string AccountNumber { set; get; }

        public virtual string PurchaseDateDisplay
        {
            get
            {
                return PurchaseDate.ToString("dd MMM yyyy");
            }
        }

        public virtual Decimal Growth
        {
            get
            {
                return CurrentPrice - PurchasePrice;
            }
        }
        public virtual string GrowthDisplay
        {
            get
            {
                bool isNegative = Growth < 0;
                return string.Format("{0}{1}{2}", 
                    isNegative ? "(" : string.Empty,
                    Growth,
                    isNegative ? ")" : string.Empty);
            }
        }
    }
}