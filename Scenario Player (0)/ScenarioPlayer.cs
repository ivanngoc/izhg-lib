using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IziHardGames.Libs.NonEngine.ScenarioPlaying
{
	public class ScenarioManager
	{
		public static void Import(List<DataForScenario> dataForScenarios, List<DataForScenarioStep> dataForScenarioSteps)
		{

		}
	}

	public class ScenarioPlayer
	{
		public void Enter(ScenarioStep step)
		{
			throw new System.NotImplementedException();
		}
		public void Exit(ScenarioStep step)
		{
			throw new System.NotImplementedException();
		}


		public void Complete(ScenarioStep step)
		{
			throw new System.NotImplementedException();
		}
		public void Skip(ScenarioStep step)
		{
			throw new System.NotImplementedException();
		}
	}
	/// <summary>
	/// Сценарий игры. Совокупность последовательных контрльных точек. Также может содержать ветвление. По сути это граф из контрольных точек
	/// </summary>
	public class Scenario
	{


	}
	/// <summary>
	/// Контролная точка в сценарии игры.
	/// </summary>
	public class ScenarioStep
	{
		public event Action OnEnterEvent;
		public event Action OnExitEvent;
		public event Action OnCompleteEvent;
	}


	[Serializable]
	public class DataForScenario
	{
		public int id;
	}
	[Serializable]
	public class DataForScenarioStep
	{
		public int id;
	}

	/// <summary>
	/// Predefined Scenario Action
	/// </summary>
	public enum EScenarioAction
	{
		None,
		ShowMsg,
		ShowDialog,
	}
}