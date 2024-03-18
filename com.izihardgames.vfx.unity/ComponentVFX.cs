using IziHardGames.Core;
using IziHardGames.View;
using System;
using UnityEngine;

namespace IziHardGames.Libs.Enginge.VFX
{
	[RequireComponent(typeof(ControlViewParticleSystem))]
	public class ComponentVFX : MonoBehaviour, IUnique, IVisionControllable, IInitializable, IDeinitializable
	{
		public int Id { get => id; set => id = value; }
		public ControlOfVisibility ControlView { get => controlViewParticleSystem; set => controlViewParticleSystem = (ControlViewParticleSystem)value; }

		[SerializeField] public int id;
		[SerializeField] public ControlViewParticleSystem controlViewParticleSystem;
		[SerializeField] public new ParticleSystem particleSystem;

		private bool isRunOnce;

		public Func<bool> pollIsPlaying;
		public Func<bool> pollIsComplete;
		public Action dispose;
		public Action<ComponentVFX> utilize;

		public bool isStarted;
		public bool isPLaying;
		public bool isComplete;
		public bool isStopped;

		#region Unity Message
		private void Reset()
		{
			controlViewParticleSystem = GetComponent<ControlViewParticleSystem>();
			particleSystem = GetComponent<ParticleSystem>();
		}
		#endregion

		public void Initilize()
		{
			if (!isRunOnce)
			{
				isRunOnce = true;
				pollIsPlaying = IsPlaying;
				dispose = ConsumeCompletion;
				pollIsComplete = IsComplete;
			}
		}

		public void InitilizeReverse()
		{

		}

		public bool IsComplete()
		{
			IsPlaying();
			return isComplete;
		}
		public bool IsPlaying()
		{
			var isPlayingNew = particleSystem.isPlaying;

			if (!isPlayingNew && isPLaying)
			{
				isComplete = true;
			}
			isPLaying = isPlayingNew;
			return isPLaying;
		}
		public void Play()
		{
			//controlViewParticleSystem.Show();
			particleSystem.Play();
			isStarted = true;
		}
		public void Stop()
		{
			particleSystem.Stop();
			//controlViewParticleSystem.Hide();
			isStopped = true;
		}

		public void ConsumeCompletion()
		{
			Stop();
			isComplete = default;
			isPLaying = default;
			isStarted = default;
			isStopped = default;
			utilize(this);
		}
	}
}