namespace IziHardGames.Libs.NonEngine.Input
{
	/// <summary>
	/// Набор объектов (контролов устройства ввода-вывода через которые пользователь управляет программмой) предоставляемых движком
	/// </summary>
	public class ControlSetNonEngine
	{
		public bool isActivated;

		public virtual bool IsSameSet(ControlSetNonEngine controlSet)
		{
			return this == controlSet;
		}

		public virtual void CheckCompatibleCombination(object compare)
		{

		}
	}
}