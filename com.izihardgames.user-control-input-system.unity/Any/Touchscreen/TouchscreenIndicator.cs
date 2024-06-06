using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
// using UnityEngine.UI;

namespace IziHardGames.UserControl.InputSystem.ForUnity.ForTouchscreen
{
	public class TouchscreenIndicator : MonoBehaviour
	{
// 		[SerializeField] Transform[] indicatorsPertouch;
// 		[SerializeField] new Camera camera;
// 		[SerializeField] Text info;

// 		#region Unity Message

// 		private void Reset()
// 		{
// 			camera = FindObjectOfType<Camera>();
// 		}
// #pragma warning disable HAA0201, HAA0601
// 		public void LateUpdate()
// 		{
// 			if (Touchscreen.current != null)
// 			{
// 				Touchscreen touchscreen = Touchscreen.current;
// 				info.text = $"TCount: {touchscreen.touches.Count}" +
// 					$"	T0: {touchscreen.touches[0].ReadValue().phase}" +
// 					$"	T1: {touchscreen.touches[1].ReadValue().phase}" +
// 					$"	T2: {touchscreen.touches[2].ReadValue().phase}" +
// 					$"	T3: {touchscreen.touches[3].ReadValue().phase}" +
// 					$"	T4: {touchscreen.touches[4].ReadValue().phase}" +
// 					$"	T5: {touchscreen.touches[5].ReadValue().phase}" +
// 					$"	T6: {touchscreen.touches[6].ReadValue().phase}" +
// 					$"	T7: {touchscreen.touches[7].ReadValue().phase}" +
// 					$"	T8: {touchscreen.touches[8].ReadValue().phase}" +
// 					$"	T9: {touchscreen.touches[9].ReadValue().phase}" +
// 					$"";
// 				Execute();
// 			}
// 		}
// 		#endregion

// 		public void Execute()
// 		{
// 			Touchscreen touchscreen = Touchscreen.current;

// 			for (int i = 0; i < touchscreen.touches.Count; i++)
// 			{
// 				TouchState touchState = touchscreen.touches[i].ReadValue();

// 				if (touchState.phase == UnityEngine.InputSystem.TouchPhase.Canceled || touchState.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
// 				{
// 					throw new ArgumentOutOfRangeException($"{touchState.phase}");
// 				}
// 				if (touchState.phase != UnityEngine.InputSystem.TouchPhase.None)
// 				{
// 					if (touchState.phase == UnityEngine.InputSystem.TouchPhase.Ended)
// 					{
// 						indicatorsPertouch[i].gameObject.SetActive(false);
// 					}
// 					else
// 					{
// 						if (touchState.phase == UnityEngine.InputSystem.TouchPhase.Moved || touchState.phase == UnityEngine.InputSystem.TouchPhase.Began)
// 						{
// 							if (!indicatorsPertouch[i].gameObject.activeSelf)
// 							{
// 								indicatorsPertouch[i].gameObject.SetActive(true);
// 							}
// 							///<see cref="UnityEngine.InputSystem.TouchPhase.Began"/> проскальзывает между кадрами. То есть может следующая фаза наступить без индикации бегина. Ненадежный параметр
// 							indicatorsPertouch[i].transform.position = touchState.position;
// 						}
// 					}
// 				}
// 			}
// 		}
// 		public void ExecuteV2()
// 		{
// 			Touchscreen touchscreen = Touchscreen.current;

// 			for (int i = 0; i < touchscreen.touches.Count; i++)
// 			{
// 				TouchState touchState = touchscreen.touches[i].ReadValue();

// 				if (touchState.phase == UnityEngine.InputSystem.TouchPhase.Ended)
// 				{
// 					indicatorsPertouch[i].gameObject.SetActive(false);
// 				}
// 				else
// 				{
// 					if (touchState.phase == UnityEngine.InputSystem.TouchPhase.Began)
// 					{
// 						indicatorsPertouch[i].gameObject.SetActive(true);
// 						indicatorsPertouch[i].transform.position = touchState.position;
// 					}
// 					else
// 					{
// 						if (touchState.phase == UnityEngine.InputSystem.TouchPhase.Moved)
// 						{

// 						}
// 					}
// 				}
// 			}
// 		}
	}
}