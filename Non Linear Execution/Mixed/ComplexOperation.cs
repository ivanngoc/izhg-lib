using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// Complex task with any initiator
	/// </summary>
	public class ComplexOperation : MonoBehaviour, IziHardGames.Core.IInitializable
	{
		/// ConstantsCore.GROUPE_CORE
		public int Priority { get => orderNumber; }
		public int orderNumber;

		[SerializeField] private int id;
		[SerializeField] private int counter;
		[SerializeField] private bool isTriggered;
		[Space]
		[Space]
		[SerializeField] public SubProcess[] subProcesses;
		[SerializeField] public UnityEvent finishAction;
		[SerializeField] public UnityEvent OnOperationStart;
		[SerializeField] public UnityEvent OnOperationEnd;

		public Action cancelation;

		#region Unity Message
		private void OnEnable()
		{

		}
		#endregion
		public void Initilize_De()
		{
			cancelation = default;

			counter = default;

			isTriggered = default;

			for (int i = 0; i < subProcesses.Length; i++)
			{
				subProcesses[i].Initilize_De();
			}
		}
		public void Initilize()
		{
			cancelation = CancelComplexOperation;

			for (int i = 0; i < subProcesses.Length; i++)
			{
				subProcesses[i].Initilize();
			}
		}

		public void ExecuteUpdate()
		{
			Check();
		}

		public void ComplexStart()
		{
			if (!isTriggered)
			{
				isTriggered = true;

				OnOperationStart?.Invoke();
			}
			enabled = true;

			foreach (var item in subProcesses)
			{
				if (!item.isTrigerred)
				{
					item.TriggerExecution();
				}
			}
		}

		public void CancelComplexOperation()
		{
			for (int i = 0; i < subProcesses.Length; i++)
			{
				subProcesses[i].CancelOperation();
			}
		}

		public void Check()
		{
			foreach (var item in subProcesses)
			{
				if (item.isInvokeRequired)
				{
					item.Execute();

					continue;
				}
			}

			if (subProcesses.Any(x => !x.isComplete))
				return;

			Finish();
		}
		private void Finish()
		{
			ComplexReset();

			counter++;

			enabled = false;

			isTriggered = default;

			finishAction?.Invoke();

			OnOperationEnd?.Invoke();
		}
		public void ComplexReset()
		{
			foreach (var item in subProcesses)
			{
				item.Reset();
			}
		}

	}
}