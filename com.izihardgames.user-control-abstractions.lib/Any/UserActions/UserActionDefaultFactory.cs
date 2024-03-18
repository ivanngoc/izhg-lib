namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public static class UserActionDefaultFactory
    {
        public static T Create<T>() where T : UserAction, new()
        {
            throw new System.NotImplementedException();
        }
    }
}
