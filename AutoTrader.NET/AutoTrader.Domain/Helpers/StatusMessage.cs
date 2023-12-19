namespace AutoTrader.Domain.Helpers
{
    public static class StatusMessage
    {
        public static string Created = "Bot is created";

        public static string InProgress = "Bot is in progress";

        public static string PausedByUser = "Bot paused by user";

        public static string PausedByError(string errorMessage) => $"Bot paused by an error: {errorMessage}";
    }
}
