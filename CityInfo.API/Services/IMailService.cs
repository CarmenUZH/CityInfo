namespace CityInfo.API.Services
{
    public interface IMailService //Allow different implemenationss
    {
        void Send(string subject, string message);
    }
}