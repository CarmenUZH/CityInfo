namespace CityInfo.API.Services
{
    public class LocalMailService
    {
        private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "worker@mycompany.com";

        public void Send(string subject, string message)
        {
            //pretending to send a mail, he's not setting up a mail service, sadge
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $"sent with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
