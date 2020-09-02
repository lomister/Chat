namespace TeamABootcampAplication.Models.Services
{
    #pragma warning disable SA1600 // Elements should be documented
    using System;
    using System.Net;
    using System.Net.Mail;

    public class EmailService : IEmailService
    {
        public EmailService()
        {
            GmailUsername = "hermeschat2020@gmail.com";
            GmailPassword = "Bootcamp2";
            GmailHost = "smtp.gmail.com";
            GmailPort = 587;
            GmailSSL = true;
        }

        public static string GmailUsername { get; set; }

        public static string GmailPassword { get; set; }

        public static string GmailHost { get; set; }

        public static int GmailPort { get; set; }

        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsHtml { get; set; }

        public bool SendEmail(string email, string newPassword)
        {
            if (ValidateSendEmail(email, newPassword))
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = GmailHost,
                    Port = GmailPort,
                    EnableSsl = GmailSSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(GmailUsername, GmailPassword),
                };

                using var message = new MailMessage(GmailUsername, email)
                {
                    Subject = "Reset Password",
                    Body = $"Welcome. You have changed your password. Your new password is: {newPassword}",
                };

                smtp.Send(message);
                smtp.Dispose();
                return true;
            }

            return false;
        }

        protected static bool ValidateSendEmail(string email, string newPassword)
        {
            if (email != null && newPassword != null)
            {
                return true;
            }

            return false;
        }
    }

    public interface IEmailService
    {
        public bool SendEmail(string email, string newPassword);
    }
    #pragma warning restore SA1600 // Elements should be documented
}
