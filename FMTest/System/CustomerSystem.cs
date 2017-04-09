using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class CustomerSystem
    {
        private DataSystem _dataSystem = new DataSystem();

        public List<Customer> GetAllCustomers()
        {
            var customers = _dataSystem.GetAll<Customer>("Customers").ToList();

            foreach (var customer in customers)
            {
                var result = _dataSystem.GetBy<Account>("Accounts", new Dictionary<string, object> { { "CustomerId", customer.Id } });
                Account theAccount = result.Count == 0 ? null : result[0];

                if (theAccount != null)
                    customer.Accounts.Add(theAccount);
            }

            return customers;
        }
    }
}