using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using RentAll.DTO.Account;

namespace RentAll.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(EmailSendDto emailsend)
        {
            //initialize mailjet client with aoi and secret key
            MailjetClient client = new MailjetClient(_config["MailJet:ApiKey"], _config["MailJet:SecretKey"]);

            //We initialize email builder with details such as where email is comming from, subject, body and recipient email.
            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_config["Email:From"], _config["Email:Applicationname"]))
                .WithSubject(emailsend.Subject)
                .WithHtmlPart(emailsend.Body)
                .WithTo(new SendContact(emailsend.To)).Build();

            //sends the email
            var response = await client.SendTransactionalEmailAsync(email);

            //if send was successful to status success return true.
            if (response.Messages != null)
            {
                if (response.Messages[0].Status == "success")
                {
                    return true;
                }
            }
            return false;


        }
    }
}
