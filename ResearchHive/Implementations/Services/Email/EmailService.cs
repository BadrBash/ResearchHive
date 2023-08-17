/*using Application.DTOs;
using Microsoft.Extensions.Options;
*//*using SendGrid.Helpers.Mail;
using SendGrid;*//*

namespace Application.Services.Email
{
    public class EmailService //: //IEmailService
    {

        *//*private readonly SenderGridMailSetting _senderGridMailSetting;
        IOptions<SenderGridMailSetting> senderGridMailSettingOption,*//*
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
*//*            _senderGridMailSetting = senderGridMailSettingOption.Value;*//*
            _logger = logger;
        }
        public async Task SendAsync(string msg, string title, IEnumerable<MailRecipient> recipients)
        {
            var message = new SendGridMessage();

            var toEmailAddresses = recipients.Select(t => new EmailAddress(t.Address, t.Name)).ToList();
            // Cc
            message.AddCc("ismailb@seersitconsulting.com", "Bello Ismail");
            message.SetSubject(title);
            message.AddContent(MimeType.Text, msg);

            var sendGridClient = new SendGridClient(_senderGridMailSetting.ApiKey);
            var fromEmailAddress = new EmailAddress(_senderGridMailSetting.Email, _senderGridMailSetting.Sender);
            var emailMessage = MailHelper.CreateSingleEmailToMultipleRecipients(fromEmailAddress, toEmailAddresses, title, msg, msg);

            var response = await sendGridClient.SendEmailAsync(emailMessage);

            if (response.IsSuccessStatusCode)
            {

                _logger.LogInformation("Email sent successfully");
            }
            else
            {
                _logger.LogError("Email sending failed");
            }
        }
        public async Task SendEmailWithFileAsync(string msg, string title, IEnumerable<MailRecipient> recipients, string path)
        {


            var sendGridClient = new SendGridClient(_senderGridMailSetting.ApiKey);
            var fromEmailAddress = new EmailAddress(_senderGridMailSetting.Email, _senderGridMailSetting.Sender);
            var toEmailAddresses = recipients.Select(t => new EmailAddress(t.Address, t.Name)).ToList();
            var emailMessage = MailHelper.CreateSingleEmailToMultipleRecipients(fromEmailAddress, toEmailAddresses, title, msg, msg);

            var filename = $"uploadReport_{DateTime.Now.ToString("yyyy-MM-dd")}.csv";

            // Incoming File Stream
            //var fileStream = File.Open(path, FileMode.Open);
            //var memoryStream = new MemoryStream();
            //fileStream.CopyTo(memoryStream); ;
            //IDictionary<string, Stream> attachments = null;

            //attachments.Add(filename, memoryStream);

            //if (attachments != null)
            //{
            //    foreach (var attachment in attachments)
            //    {
            //        await emailMessage.AddAttachmentAsync(attachment.Key, attachment.Value);
            //    }
            //}

            var attachmentContent = Convert.ToBase64String(File.ReadAllBytes(path));
            var attachment = new Attachment
            {
                Content = attachmentContent,
                Filename = filename,
                Type = "text/csv",
                Disposition = "attachment"
            };

            emailMessage.AddAttachment(attachment);

            var response = await sendGridClient.SendEmailAsync(emailMessage);

            if (response.IsSuccessStatusCode)
            {

                _logger.LogInformation("Email sent successfully");
            }
            else
            {
                _logger.LogError("Email sending failed");
            }


            //var email = new SendGridMessage();

            //var toEmailAddresses = recipients.Select(t => new EmailAddress(t.Address, t.Name)).ToList();

            //email.AddTo(new EmailAddress(_senderGridMailSetting.Email, _senderGridMailSetting.Sender));

            //email.AddCc("ismailb@seersitconsulting.com", "Ismail Bello");

            //email.SetSubject(title);
            //email.AddContent(MimeType.Text, msg);

            //// Incoming File Stream
            //var fileStream = File.Open(path, FileMode.Open);

            //// Create a new instance of memorystream
            //var memoryStream = new MemoryStream();

            //// Use the .CopyTo() method and write current filestream to memory stream
            //fileStream.CopyTo(memoryStream);

            //// Convert Stream To Array
            //byte[] byteArray = memoryStream.ToArray();

            //var filename = $"uploadReport_{DateTime.Now.ToString("yyyy-MM-dd")}.csv";

            //var attachmentContent = Convert.ToBase64String(File.ReadAllBytes($"{path}"));
            //var attachment = new Attachment
            //{
            //    Content = attachmentContent,
            //    Filename = filename,
            //    Type = "text/csv",
            //    Disposition = "attachment"
            //};

            //email.AddAttachment(attachment);


            //var client = new SendGridClient(_senderGridMailSetting.ApiKey);
            //var response = await client.SendEmailAsync(email);

            //if (response.IsSuccessStatusCode)
            //{

            //    _logger.LogInformation("Email sent successfully");
            //}
            //else
            //{
            //    _logger.LogError("Email sending failed");
            //}

        }

    }
}
*/