using FMTest.Business;
using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FMTest.Controllers
{
    public class YorubaController : ApiController
    {
        private YorubaSystem _yorubaSystem = new YorubaSystem();


        [Route("api/yoruba/paragraph/derivatives")]
        public ParagraphOutput Post(Paragraph paragraph)
        {
            try
            {
                var result = _yorubaSystem.ProcessParagraph(paragraph.Content);
                return result;
            }

            catch (Exception ex)
            {
                return null;
            }
        }


        [Route("api/yoruba/paragraph/derivatives")]
        public HttpResponseMessage Put(ToneMarkingLog log)
        {
            try
            {
                var result = _yorubaSystem.UpdateToneMarkingLogs(log);
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }

            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }


        [Route("api/yoruba/paragraph/derivatives")]
        public List<ToneMarkingLog> Get()
        {

            List<ToneMarkingLog> result = _yorubaSystem.GetRecentlyProcessed();
            return result;
        }
    }
}
