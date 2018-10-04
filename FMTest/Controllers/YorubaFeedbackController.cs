using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FMTest.Controllers
{
    public class YorubaFeedbackController : ApiController
    {
        private YorubaSystem _yorubaSystem = new YorubaSystem();

        public HttpResponseMessage Post(Feedback feedback)
        {
            YorubaFeedbackService.SubmitFeedback(feedback);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
