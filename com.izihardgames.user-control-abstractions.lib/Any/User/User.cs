namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    public abstract class User
    {
        public readonly int id;
        public abstract T GetInputData<T>() where T : class, IInputDataSet;
        protected User(int id)
        {
            this.id = id;
        }
    }
}
