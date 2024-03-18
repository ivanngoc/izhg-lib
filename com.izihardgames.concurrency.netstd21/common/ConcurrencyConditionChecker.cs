using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Concurrency
{
	/// <summary>
	/// Check conditions for synchronal activation/deactivation
	/// </summary>
	internal class ConcurrencyConditionChecker
	{
		internal bool isPassedChecks;
		private List<Func<bool>> checkers = new List<Func<bool>>(2);

		internal void AddCheck(Func<bool> checker)
		{
			checkers.Add(checker);
		}

		internal void Execute()
		{
			foreach (var item in checkers)
			{
				if (!item.Invoke())
				{
					isPassedChecks = false;
					return;
				}
			}
			isPassedChecks = true;
		}
	}
}