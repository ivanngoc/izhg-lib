using IziHardGames.Core;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.ScenarioWorkflow.Actions
{
	/// <summary>
	/// Execute update loop for <see cref="MonoActionsControl"/>
	/// </summary>
	public class MonoActionsProcessor : MonoBehaviour, IUpdatableLate
	{
		public int Priority { get; }
		public List<MonoActionsControl> monoActionsControls;

		public void ExecuteUpdateLate()
		{
			throw new System.NotImplementedException();
		}
	}
}