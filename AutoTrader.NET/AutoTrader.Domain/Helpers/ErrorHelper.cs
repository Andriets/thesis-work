namespace AutoTrader.Domain.Helpers
{
    public class ErrorHelper
    {
        public static string ExceptionWasThrown = "An exception was thrown.";

        public static string EntityNotFound(string entityName) => $"{entityName.ToLower()} not found.";

        public static string InvalidProperty(string propertyName) => $"Property {propertyName.ToLower()} is not valid";
    }
}
