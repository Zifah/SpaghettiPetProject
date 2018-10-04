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
        public static void SubmitFeedback(Feedback feedback)
        {
            EmailUtility.SendEmail(feedback.Name, feedback.Email, ConfigurationManager.AppSettings["Feedback-Email-Receiver"],
                "Yoruba Tone Marker Feedback", feedback.Message);
        }
    }
}