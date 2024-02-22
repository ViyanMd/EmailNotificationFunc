using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace EmailNotificationFunc
{
    [StorageAccount("AzureWebJobsStorage")]
    public class EmailNotification
    {
        private readonly ILogger<EmailNotification> _logger;
        public EmailNotification(ILogger<EmailNotification> logger)
        {
            _logger = logger;
        }

        [FunctionName(nameof(EmailNotification))]
        [return: SendGrid(ApiKey = "SendGridApiKey")]
        public SendGridMessage Run([BlobTrigger("files/{name}")] string name, Uri uri, IDictionary<string, string> metaData)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("viyanomd@gmail.com", "Sender"),
                Subject = "File download URL",
                HtmlContent = $"This link for <a href='{metaData["SASURL"]}'>{metaData["FileName"]}</a> is valid for 1 hour.<br/><i>For security purposes the name of your file has been changed</i>."
            };

            msg.AddTo(new EmailAddress($"{metaData["Email"]}", "Recepient"));

            return msg;
        }
    }
}
