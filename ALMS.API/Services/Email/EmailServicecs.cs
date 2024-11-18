using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("AML Secure Libary System", "arianhit89@gmail.com"));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { TextBody = body };
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465); // Replace with your SMTP server
            client.Authenticate("arianhit89@gmail.com", "sqjn skuu xicg xogx"); // SMTP credentials
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
