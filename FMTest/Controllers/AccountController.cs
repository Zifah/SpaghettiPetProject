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
    public class AccountController : ApiController
    {
        public AccountSystem _accountSystem = new AccountSystem();
        // GET: api/Customer
        public Account Get(string number)
        {
            Account result = _accountSystem.GetAccount(number);
            return result;
        }
    }
}
