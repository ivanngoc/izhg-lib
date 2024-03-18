using IziHardGames.Libs.NonEngine.StateMachines.Datas;

namespace IziHardGames.Libs.NonEngine.StateMachines
{
	public interface IStateMachine
	{

	}

	/// <summary>
	/// Define how to change <see cref="IDataOfState"/>.
	/// Contain methods to manipulate <see cref="IDataOfState"/>.
	/// »Û
	/// </summary>
	public interface IBehaviourOfState
	{

	}

	/// <summary>
	/// Contain collection of behaviours. 
	/// In Update Loop of StateMachine it itterate through that list. 
	/// Container also define how that itteration will be proceeded: whether it will continiously itterate over whole list or break at certain behaviour
	/// </summary>
	public interface IContainerForBehaviours
	{

	}
}