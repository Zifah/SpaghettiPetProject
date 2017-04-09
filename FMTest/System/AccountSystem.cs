using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class AccountSystem
    {
        private DataSystem _dataSystem = new DataSystem();

        public Account GetAccount(string number)
        {
            var accounts = _dataSystem.GetBy<Account>("Accounts", new Dictionary<string, object> { 
                { "Number", number }
            });

            var theAccount = accounts.Count > 0 ? accounts[0] : null;

            if (theAccount != null)
            {
                var portfolio = _dataSystem.GetPortfolio(number);
                theAccount.Stocks = portfolio;
            }

            return theAccount;
        }
    }
}