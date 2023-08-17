/*using Application.Abstractions.Email;
using Application.DTOs;
using Application.Interface.Email;
using Model.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Email
{
    public class MailSender : IMailSender
    {
       
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _hostingEnvironment;
        *//*  IOptions<SenderGridMailSetting> options,*/
        /* private readonly SenderGridMailSetting _sendGrid;*//*
        public MailSender(IEmailService emailService, IHostingEnvironment hostingEnvironment)
        {
            _emailService = emailService;
            _hostingEnvironment = hostingEnvironment;
           *//* _sendGrid = options.Value;*//*
        }
        
        public async Task SendChangePasswordMail(User user, string userPassword)
        {
            string msg = createEmailBody(user, "Change Password", $"Username : {user.LastName} {user.FirstName} User Password : {userPassword}");
            await _emailService.SendAsync(msg, "Change Password", new List<MailRecipient>() { new MailRecipient { Address = user.Email,
                Name = $"{user.LastName} {user.FirstName}" } });
        }

        public async Task SendForgotPasswordMail(User user, string passwordResetLink)
        {
            string msg = createEmailBody(user, "Forgot Password EMail", $"Username : {user.LastName} {user.FirstName} Password ResetLink : {passwordResetLink}");
            await _emailService.SendAsync(msg, "Forgot Password EMail", new List<MailRecipient>() { new MailRecipient { Address = user.Email,
                Name = $"{user.LastName} {user.FirstName}" } });
        }

        public async Task SendResetPasswordMail(User user, string userPassword)
        {
            
            string msg = createEmailBody(user, "ResetPasswordEMail", $"Username : {user.Email}  Password : {userPassword}");
            await _emailService.SendAsync(msg, "ResetPasswordEMail", new List<MailRecipient>() { new MailRecipient { Address = user.Email,
                Name = $"{user.LastName} {user.FirstName}" } });
        }

        public async Task SendVerifyMail(User user)
        {
            string msg = createEmailBody(user, "VerifyMail", $"Username : {user.Email}  Password : {user.LastName}");
            await _emailService.SendAsync(msg, "VerifyMail", new List<MailRecipient>() { new MailRecipient { Address = user.Email,
                Name = $"{user.LastName} {user.FirstName}" } });
        }

        public async Task SendWelcomeMail(User user, string userPassword)
        {
            string msg = createEmailBody(user, "WelcomeEmail", $"Username : {user.Email}  Password : {userPassword}");
            await _emailService.SendAsync(msg, "WelcomeEmail", new List<MailRecipient>() { new MailRecipient { Address = user.Email,
                Name = $"{user.LastName} {user.FirstName}" } });
        }

        private string createEmailBody(User user, string subject, string message)
        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   
            string basePath = _hostingEnvironment.WebRootPath;

            using (StreamReader reader = new StreamReader(basePath + $"\\feedbackEmail.html"))
            {

                body = reader.ReadToEnd();
            }

            body = body.Replace("{OtherName}", user.LastName); //replacing the required things  

            body = body.Replace("{Subject}", subject);

            body = body.Replace("{Message}", message);

            return body;

        }
       
    }
}
*/