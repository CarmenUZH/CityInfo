namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = "cloudadmin@mycompany.com";
        private string _mailFrom = "cloudworker@mycompany.com";

        public void Send(string subject, string message)
        {
            //pretending to send a mail, he's not setting up a mail service, sadge
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $"sent with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
