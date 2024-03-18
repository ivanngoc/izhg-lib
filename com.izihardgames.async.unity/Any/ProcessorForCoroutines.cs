using System.Collections;
using UnityEngine;


namespace IziHardGames.Libs.Engine.Async.Coroutines
{
	public class ProcessorForCoroutines : MonoBehaviour
	{
		public static ProcessorForCoroutines singleton;
		public static void Run(IEnumerator enumerator)
		{
			singleton.StartCoroutine(enumerator);
		}
		public void Initilize()
		{
			singleton = this;
		}
	}
}