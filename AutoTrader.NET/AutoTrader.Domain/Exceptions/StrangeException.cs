namespace AutoTrader.Domain.Exceptions
{
    public class StrangeException : Exception
    {
        public StrangeException()
        {
        }

        public StrangeException(string message)
            : base(message)
        {
        }
    }
}
