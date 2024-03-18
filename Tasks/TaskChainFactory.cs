using IziHardGames.Libs.Engine.Memory;
using IziHardGames.Ticking.Lib;
using System;

namespace IziHardGames.Libs.NonEngine.Threading.Tasks
{
	public class TaskChainFactory
	{
		private readonly IObjectPool<TaskChainUnit> poolobject;
		private readonly IUpdateService updateService;

		public TaskChainFactory(IObjectPool<TaskChainUnit> poolobject, IUpdateService updateService)
		{
			this.poolobject = poolobject;
			this.updateService = updateService;
		}

		public TaskChainUnit CreateChain(Action handler)
		{
			TaskChainUnit taskChainUnit = poolobject.Rent();
			//UpdateProcess updateProcess = updateService.Insert();
			throw new NotImplementedException();
		}
	}
}