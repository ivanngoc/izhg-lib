using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// <see cref="ComplexOperation"/>
	/// </summary>
	public class SubProcess : MonoBehaviour, IziHardGames.Core.IInitializable
	{
		[SerializeField] private ComplexOperation complexOperation;

		[SerializeField] public bool isComplete;
		[SerializeField] public bool isTrigerred;
		[SerializeField] public bool isInvokeRequired;
		[SerializeField] int counter;
		[Space]
		[SerializeField] UnityEvent unityEvent;

		public Action setComplete;

		private Task task;


		public void Initilize_De()
		{
			isComplete = default;
			isTrigerred = default;
			setComplete = default;
			isInvokeRequired = false;

			setComplete = default;
			task = default;
		}
		public void Initilize()
		{
			setComplete = SetComplete;

			isInvokeRequired = false;
		}

		public void Initiate()
		{
			complexOperation.ComplexStart(); //async

			TriggerExecution();
		}
		public void TriggerExecution()
		{
			isTrigerred = true;
			// do own action
			Execute();
		}

		public void Execute()
		{
			unityEvent.Invoke();
		}

		public void CancelOperation()
		{
			if (task != null)
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

				var token = cancellationTokenSource.Token;

				throw new System.NotImplementedException();
			}
		}
		public void SetComplete()
		{
			isComplete = true;

			isInvokeRequired = false;

			counter++;
		}

		public void Reset()
		{
			isComplete = false;
			isTrigerred = false;
		}

	}
}