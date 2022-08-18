namespace Bank.Core.Interfaces
{
    public interface ISmsSender
    {
        string SendMessage(string messageBody, string sendNumber);
    }
}
