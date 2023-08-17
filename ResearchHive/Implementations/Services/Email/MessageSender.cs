/*using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit;
using TheLogoPhilia.Interfaces;
using TheLogoPhilia.Interfaces.IRepositories;
using TheLogoPhilia.Models;

namespace TheLogoPhilia.Implementations
{
    public class MessageSender : IMessageSender
    {
        public string SendMailToMultipleUser(string Subject, List<string> userEmails, string message)
        {
             MimeMessage  MimeMessage = new MimeMessage();
             
            MimeMessage.From.Add(new MailboxAddress("The LogoPhilia Administrator", "thelogophilia@gmail.com"));
            foreach (var emailAddress in userEmails)
            {
                MimeMessage.To.Add(MailboxAddress.Parse(emailAddress));
            }
             
             MimeMessage.Subject = Subject;
             MimeMessage.Body = new TextPart("plain")
             {
                   Text = @$"Dear Esteemed User, It's {DateTime.UtcNow} How are you doing?,
                   {message}",
             };
            string EmailAddress = "thelogophilia@gmail.com";
            string Password = "LogoPhiliaApp";
             SmtpClient client  = new SmtpClient();
             try
             {
                  client.Connect("smtp.gmail.com", 465, true);
                  client.Authenticate(EmailAddress,Password);
                  client.Send(MimeMessage);
                  return "Successfully Sent";
             }
             catch (System.Exception ex)
             {
                  throw new Exception(ex.Message);
             }
             finally
             {
                 client.Disconnect(true);
                 client.Dispose();
             }
        }

        public async Task<string> SendMailToMultipleUserAboutDiscussion(string Subject, List<string> userEmails, string message, List<string> PostUrls)
        {
          MimeMessage  MimeMessage = new MimeMessage();
             
            MimeMessage.From.Add(new MailboxAddress("The LogoPhilia Administrator", "thelogophilia@gmail.com"));
            foreach (var emailAddress in userEmails)
            {
                MimeMessage.To.Add(MailboxAddress.Parse(emailAddress));
            }
             
             MimeMessage.Subject = Subject;
           
             int linkNumber= 1;
             foreach (var item in PostUrls)
             {
                 MimeMessage.Body = new TextPart("html")
                 {
                     Text = @$"
                     Dear Esteemed User, It's {DateTime.UtcNow} How are you doing?,
                       {message}
                      Link {linkNumber++} "
                      + $"<a href={item}/> Click To View Post </a>",
                 };
             }
            string EmailAddress = "thelogophilia@gmail.com";
            string Password = "LogoPhiliaApp";
             SmtpClient client  = new SmtpClient();
             try
             {
                  client.Connect("smtp.gmail.com", 465, true);
                  client.Authenticate(EmailAddress,Password);
                  client.Send(MimeMessage);
                  return "Successfully Sent";
             }
             catch (System.Exception ex)
             {
                  throw new Exception(ex.Message);
             }
             finally
             {
                await client.DisconnectAsync(true);
                 client.Dispose();
             }
        }

        public string SendMailToSingleUser(string Subject, string userEmail, string message)
        {
           MimeMessage  MimeMessage = new MimeMessage();
             
            MimeMessage.From.Add(new MailboxAddress("The LogoPhilia Administrator", "thelogophilia@gmail.com"));
             MimeMessage.To.Add(MailboxAddress.Parse(userEmail));
             
             MimeMessage.Subject = Subject;
             MimeMessage.Body = new TextPart("plain")
             {
                   Text = @$"Dear Esteemed User, It's {DateTime.UtcNow} How are you doing?,
                   {message}",
             };
            string EmailAddress = "thelogophilia@gmail.com";
            string Password = "LogoPhiliaApp";
             SmtpClient client  = new SmtpClient();
             try
             {
                  client.Connect("smtp.gmail.com", 465, true);
                  client.Authenticate(EmailAddress,Password);
                  client.Send(MimeMessage);
                  return "Successfully Sent";
             }
             catch (System.Exception ex)
             {
                  throw new Exception(ex.Message);
             }
             finally
             {
                 client.Disconnect(true);
                 client.Dispose();
             }
        }
    }
}*/