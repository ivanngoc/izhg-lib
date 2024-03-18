using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IziHardGames;
using IziHardGames.View;
using IziHardGames.Core;
using System.Linq;

namespace IziHardGames.Libs.Engine.Animations
{
	/// <summary>
	/// 
	/// </summary>
	/// <tips>
	/// https://answers.unity.com/questions/741357/play-two-animation-simultaneously-using-animator.html
	/// </tips>
	[RequireComponent(typeof(Animator))]
	public class ControllerForAnimations : MonoBehaviour, IUnique, IVisionControllable, IInitializable, IDeinitializable, IExcluded
	{
		public int Id { get; set; }
		public ControlOfVisibility ControlView { get => controlOfVisibility; set => controlOfVisibility = value; }


		[SerializeField] public Animator animator;
		[SerializeField] public ControlOfVisibility controlOfVisibility;
		//[SerializeField] public string[] stringStates;


		private void Reset()
		{
			animator = GetComponent<Animator>();
			controlOfVisibility = GetComponent<ControlOfVisibility>();
		}
		[ContextMenu("Form Presets")]
		private void FormPresets()
		{
			//stringStates = animator.runtimeAnimatorController.animationClips.Select(x => x.name).ToArray();
			//stringStates = animator.GetCurrentAnimatorStateInfo(0);
		}
		public void Initilize()
		{
		}
		public void InitilizeReverse()
		{
		}

		public void Play(string state, int layer, float fixedTime)
		{
			//animator.Play(state, layer, progress01);
			animator.PlayInFixedTime(state, layer, fixedTime);
		}
	}
}