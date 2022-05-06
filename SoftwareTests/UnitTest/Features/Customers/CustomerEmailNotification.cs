using MediatR;

namespace Features.Customers
{
    public class CustomerEmailNotification : INotification
    {
        public string Origin { get; private set; }
        public string Destination { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }

        public CustomerEmailNotification(string origin, string destination, string subject, string message)
        {
            Origin = origin;
            Destination = destination;
            Subject = subject;
            Message = message;
        }
    }
}