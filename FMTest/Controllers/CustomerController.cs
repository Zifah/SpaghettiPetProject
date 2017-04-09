using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMTest.Controllers
{
    public class CustomerController : ApiController
    {
        public CustomerSystem _customerSystem = new CustomerSystem();
        // GET: api/Customer
        public IList<Customer> Get()
        {
            var result = _customerSystem.GetAllCustomers();
            return result;
        }
    }
}
