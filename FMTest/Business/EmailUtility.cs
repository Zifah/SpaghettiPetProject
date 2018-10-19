using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class EmailUtility
    {
        public static void SendEmail(string senderName, string senderEmail, string receiverEmail, string subject, string body)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["Mailgun-Root-Url"]);
            client.Authenticator = new HttpBasicAuthenticator("api", ConfigurationManager.AppSettings["Mailgun-Email-Api-Key"]);

            var request = new RestRequest("messages", Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("from", $"{senderName} <{senderEmail}>");
            request.AddParameter("to",receiverEmail);
            request.AddParameter("subject", subject);
            request.AddParameter("html", body);
            IRestResponse response = client.Execute(request);
            string responseBody = response.Content;
        }
    }
}