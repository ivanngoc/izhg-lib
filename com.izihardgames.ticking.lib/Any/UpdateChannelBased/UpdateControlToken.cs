using System;

namespace IziHardGames.Ticking.Lib
{
	/// <summary>
	/// Объект для обратной связи и обратного контроля со стороные агента системы обнвления.
	/// С его помощью можно остановить процесс обнолвения и удалить его из системы.
	/// Ордер предъявляется системе обновления для выделения процесса обнолвения внутри этой системы.
	/// Ордер включает в себя опции по которым будетсоздан процесс.
	/// </summary>
	public class UpdateControlToken
	{
		/// <summary>
		/// 0 = not set
		/// </summary>
		public int id;
		public int idIssuedByUpdateSystem;

		/// <summary>
		/// <see cref="UpdateGroupe.idGroupe"/>
		/// </summary>
		public int idGroupe;
		/// <summary>
		/// Handler to execute in update
		/// </summary>
		public Action Action { get; set; }
		/// <summary>
		/// Exit update condition
		/// </summary>
		public Func<bool> Condition { get; set; }
		/// <summary>
		/// Custom func to decide to execute or skip itteration
		/// </summary>
		public Func<bool> Filter { get; set; }

		public UpdateOptions updateOptions;
		private UpdateJob updateJob;

		private readonly Action actionUpdateSuspend;
		private readonly Action actionUpdateResume;
		public bool isUpdating;

		public UpdateControlToken()
		{
			actionUpdateSuspend = UpdateSuspend;
			actionUpdateResume = UpdadeResume;
			id = GetHashCode();
		}

		public void Bind(UpdateJob updateJob)
		{
			this.updateJob = updateJob;
			updateJob.updateToken = this;
		}

		public void UpdadeSkip(int frame)
		{
			throw new NotImplementedException();
		}
		public void UpdadeSkip(float time)
		{
			throw new NotImplementedException();
		}
		public void UpdadeSkip()
		{
			throw new NotImplementedException();
		}
		public void UpdadeSkipOnce()
		{
			throw new NotImplementedException();
		}
		public void UpdadeResume()
		{
			isUpdating = true;
			updateJob.Resume();
		}
		public void UpdateSuspend()
		{
			isUpdating = false;
			updateJob.Suspend();
		}
	}
}