using IziHardGames.Libs.NonEngine.Threading.Tasks;
using System;

namespace IziHardGames.Libs.Engine.PerFrameDistribution
{
	/// <summary>
	/// Операция которую нужно выполняить в каждом кадре
	/// </summary>
	/// <see cref="MonoAction"/>
	/// <see cref="TaskChainUnit"/>
	/// <see cref="UnityEngine.AsyncOperation"/>
	public class DistributedOperation : IDisposable
	{
		public readonly Action action;
		private Action handler;
		private Action callback;

		public DistributedOperation()
		{
			action = Execute;
		}

		public void Execute()
		{
			handler();
		}

		public void Initilize(Action handler)
		{
			this.handler = handler;
		}

		public void ContinueWith(Action callback)
		{
			this.callback = callback;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}