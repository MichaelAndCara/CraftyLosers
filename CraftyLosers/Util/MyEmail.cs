using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace CraftyLosers.Util
{
    public class MyEmail : MailMessage
    {
        private MySmtpClient _mySmtpClient;
        private MySmtpClient MySmtpClient { get { return _mySmtpClient ?? (_mySmtpClient = new MySmtpClient()); } }

        public MyEmail(string toAddress) : base("craftyloser@gmail.com", toAddress)
        {
            base.Bcc.Add("craftyloser@gmail.com");
        }
    }
}