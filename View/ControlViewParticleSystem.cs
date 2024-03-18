using UnityEngine;

namespace IziHardGames.View
{
	[RequireComponent(typeof(ParticleSystem))]
	public class ControlViewParticleSystem : ControlOfVisibility
	{
		[Header("ControlViewParticleSystem")]
		[SerializeField] public new ParticleSystem particleSystem;
		[SerializeField] public new Renderer renderer;

#if UNITY_EDITOR
		public override void Reset()
		{
			base.Reset();
			particleSystem = GetComponent<ParticleSystem>();
			//particleSystem.main.playOnAwake = false;
			renderer = GetComponent<Renderer>();
		}
#endif


		public override void Show()
		{
			base.Show();
			renderer.enabled = true;
		}

		public override void Hide()
		{
			base.Hide();
			renderer.enabled = false;
		}
	}
}