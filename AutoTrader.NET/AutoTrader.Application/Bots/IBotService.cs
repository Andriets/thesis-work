namespace AutoTrader.Application.Bots
{
    public interface IBotService
    {
        Task StartBot(Guid userId, Guid botId);

        Task StopBot(Guid id);

        void ContinueBot(Guid id);

        Task DeleteBot(Guid id);
    }
}
