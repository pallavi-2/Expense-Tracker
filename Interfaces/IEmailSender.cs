namespace ExpenseTracker.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(string toEmail, string subject);
    }
}
