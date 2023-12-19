namespace AutoTrader.Domain.Exceptions
{
    public class AutoTraderException : Exception
    {
        public AutoTraderException()
        {
        }

        public AutoTraderException(string message)
            : base(message)
        {
        }

        public AutoTraderException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
