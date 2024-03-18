namespace IziHardGames.UserControl.Abstractions.NetStd21
{
    /// <summary>
    /// Part of ECS. Data container
    /// </summary>
	public interface IUserActionData
    {

    }

    public interface IUserActionDataForHistory
    {

    }

    /// <summary>
    /// Part of ECS. Job. Modifying <see cref="IUserActionData"/>
    /// </summary>
    public interface IUserActionJob
    {

    }
    /// <summary>
    /// Job + Data 
    /// </summary>
    public interface IUserAction : IUserActionJob, IUserActionData
    {

    }

    public interface IUserActionRevertable : IUserAction
    {

    }

    public interface IUserActionBuilder
    {

    }

    public interface IUserActionRecordable<T> where T : unmanaged
    {
        T GetRecord();
    }
}
