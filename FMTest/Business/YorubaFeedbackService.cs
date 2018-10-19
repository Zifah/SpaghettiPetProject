using FMTest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FMTest.Business
{
    public class YorubaFeedbackService
    {
        private const string FEEDBACK_SENDER_EMAIL = "yorubatonemarker@hafiz.com.ng";
        public static void SubmitFeedback(Feedback feedback)
        {
            string body = $"<b>Message</b>: {feedback.Message}<br>" +
                $"<b>Sender Name</b>: {feedback.Name}<br>" +
                $"<b>Sender Email</b>: {feedback.Email}";
            EmailUtility.SendEmail(feedback.Name, FEEDBACK_SENDER_EMAIL, ConfigurationManager.AppSettings["Feedback-Email-Receiver"],
                "Yoruba Tone Marker Feedback", body);
        }
    }
}