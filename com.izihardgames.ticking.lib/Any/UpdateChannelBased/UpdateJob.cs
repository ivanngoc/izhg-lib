using System;
using System.Threading.Tasks;

namespace IziHardGames.Ticking.Lib
{
	/// <summary>
	/// Наименьшая единица в системе обновления. 
	/// Логически - атомарная задача, вызов метода.
	/// </summary>
	public class UpdateJob
	{
		public static int idInitial;
		public int id;

		public UpdateStep updateStep;
		public UpdateControlToken updateToken;
		private Action action;
		private Task task;
		public bool isComplete;

		private Action actionTrigger;
		private Action actionCached;
		public string name;
		private readonly Action runSync;
		private readonly Action runTaskParallel;

		public readonly Action dummy;
		static UpdateJob()
		{
			idInitial = 400;
		}

		public UpdateJob()
		{
			dummy = DummyCall;
			runSync = RunSync;
			runTaskParallel = RunTaskParallel;

			id = idInitial;
			idInitial++;
		}
		public void Execute()
		{
			actionTrigger();
		}
		/// <summary>
		/// Suspend by calling empty body function
		/// </summary>
		public void Suspend()
		{
			actionCached = actionTrigger;
			actionTrigger = dummy;
		}
		public void Resume()
		{
			actionTrigger = actionCached;
		}
		private void Initilize(UpdateControlToken token)
		{
			action = token.Action;
			updateToken = token;

			token.Bind(this);

			switch (token.updateOptions.eUpdateJobType)
			{
				case EUpdateJobType.None: throw new ArgumentException();
				case EUpdateJobType.Sync: actionTrigger = runSync; break;
				case EUpdateJobType.AsyncTask: throw new NotImplementedException();
				case EUpdateJobType.AsyncEnumerator: throw new NotImplementedException();
				case EUpdateJobType.Poll: throw new NotImplementedException();
				case EUpdateJobType.TaskParallel: actionTrigger = runTaskParallel; break;
				default: break;
			}
			actionCached = actionTrigger;
		}

		private void RunSync()
		{
			action();
			isComplete = true;
		}
		private void RunTaskParallel()
		{
			task = Task.Factory.StartNew(action);
		}

		public static UpdateJob Wrap(UpdateControlToken token)
		{
			UpdateJob updateJob = new UpdateJob();
			updateJob.Initilize(token);
			return updateJob;
		}

		public void DummyCall()
		{
			isComplete = true;
		}

#if UNITY_EDITOR
		public Action DebugGetAction() => action;
#endif

	}
}