namespace IziHardGames.Apps
{
    /// <summary>
    /// Common app
    /// </summary>
    public interface IIziApp
    {
        public IziHardGames.DependencyInjection.Contracts.IServiceProvider GetServiceProvider();
    }
}
