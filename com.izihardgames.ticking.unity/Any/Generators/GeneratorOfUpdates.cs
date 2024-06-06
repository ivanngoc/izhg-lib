using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.Ticking.Abstractions.Lib;
using UnityEngine;

namespace IziHardGames.Ticking.ForUnity
{
	/// <summary>
	/// У UnityEngine.InputSystem.PlayerInput pyfxtybt = -100
	/// </summary>
	[DefaultExecutionOrder(-100)]
	public abstract class GeneratorOfUpdates : MonoBehaviour, IGeneratorOfUpdates, ITickProvider, IIziService, ITickService
	{
		public virtual void Reset()
		{
			Disable();
		}

		public virtual void Enable()
		{
			enabled = true;
		}

		public virtual void Disable()
		{
			enabled = false;
		}

		public void Start()
		{
			enabled = true;
		}

		public void Stop()
		{
			enabled = false;
		}
	}
}