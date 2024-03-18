using System;

namespace IziHardGames.UserControl.Lib.Contexts
{
	public interface IUserContextProvider
	{


	}

	/// <summary>
	/// Хранилище заготовленных функций-выборок по состоянию пользовательской среды. Результат выполнения функции также хранится здесь.
	/// Необходим для централизованного хранения и доступа к состоянию среды.
	/// </summary>
	public class UserContextStates
	{
		private Func<bool>[] checkers;
		private bool[] checkResult;
		public int currentState;

		/// <summary>
		/// Max 32 checkers. 
		/// </summary>
		/// <param name="checkers"></param>
		public UserContextStates(Func<bool>[] checkers)
		{
			if (checkers.Length > 32) throw new ArgumentException($"there can be only 32 checkers (int bits count, int as flags)");

			this.checkers = checkers;
			checkResult = new bool[checkers.Length];
		}

		/// <summary>
		/// Collect data of Environment
		/// </summary>
		public virtual void Grab()
		{
			for (int i = 0; i < checkResult.Length; i++)
			{
				checkResult[i] = checkers[i].Invoke();
			}
		}
		/// <summary>
		/// Check if grabbed context is matched
		/// </summary>
		protected virtual void Check()
		{
			CheckersToIndex();
		}
		private void CheckersToIndex()
		{
			for (int i = 0; i < checkResult.Length; i++)
			{
				if (checkResult[i])
				{
					currentState = currentState & (1 << i);
				}
				else
				{
					currentState = currentState & (~(1 << i));
				}
			}
		}

		public bool GetState(int index)
		{
			return checkResult[index];
		}

		public void AddState(int index, Func<bool> stateSelector)
		{
			checkers[index] = stateSelector;
		}
	}
}