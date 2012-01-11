using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace CraftyLosers.Util
{
    public class MySmtpClient : SmtpClient
    {
        public MySmtpClient() : base("smtp.gmail.com", 587)
        {
            base.EnableSsl = true;
            base.DeliveryMethod = SmtpDeliveryMethod.Network;
            base.UseDefaultCredentials = false;
            base.Credentials = new NetworkCredential("craftyloser@gmail.com", "pr3v3n1n9");
        }  

        //var fromAddress = new MailAddress("craftyloser@gmail.com");
        //    var toAddress = new MailAddress(user.Email, user.UserName);
        //    const string fromPassword = "pr3v3n1n9";
        //    const string subject = "Welcome to Crafty Losers!";
        //    string body = "Welcome to Crafty Losers!  Good luck!";

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        //    };
        //    using (var message = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
        //    {
        //        smtp.Send(message);
        //    }
    }
}