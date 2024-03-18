#if UNITY_EDITOR
using IziHardGames.Ticking.Lib;
using IziHardGames.Ticking.Lib.ApplicationLevel;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Develop.Core
{
	public class UpdatesDebug : MonoBehaviour
	{
		[SerializeField] List<GameObject> gameObjects = new List<GameObject>();

		private void OnEnable()
		{
			Init();
		}

		private void OnDisable()
		{
			foreach (var item in gameObjects)
			{
				Destroy(item);
			}
			gameObjects.Clear();
		}
		private void Reset()
		{
			gameObject.tag = "EditorOnly";
		}


		public void Init()
		{
			UpdateWithChannels updateService = Reg.GetSingleton<UpdateWithChannels>();
			///<see cref="UpdateChannelV1"/>			
			///<see cref="UpdateGroupe"/>			
			///<see cref="UpdateStep"/>			
			///<see cref="UpdateJob"/>			

			GameObject root = new GameObject("UpdateRoot");
			gameObjects.Add(root);

			foreach (var item in updateService.updateChannelDefault.DebugGetUpdateGroups())
			{
				var steps = item.DebugGetUpdateSteps();

				GameObject groupeGo = new GameObject($"GROUPE ID:{item.idGroupe}_ORDER:{item.order}_{((EUpdatePhase)item.idGroupe).ToString()}");
				groupeGo.transform.SetParent(root.transform);

				foreach (var step in steps)
				{
					GameObject stepGO = new GameObject($"STEP ID:{step.id}_Priority_{step.priority}");
					stepGO.transform.SetParent(groupeGo.transform);
					stepGO.transform.SetAsLastSibling();
					gameObjects.Add(stepGO);
					foreach (var job in step.updateJobs)
					{
						Action action = job.DebugGetAction();
						GameObject jobGO = new GameObject($"Job {job.name}: {action.Target.GetType().FullName}");
						jobGO.transform.SetParent(stepGO.transform);
						jobGO.transform.SetAsLastSibling(); ;
						gameObjects.Add(jobGO);
						var comp = jobGO.AddComponent<DebugUpdateJob>();
						comp.job = job;
						jobGO.SetActive(job.isComplete);
					}
				}
			}
		}
	}
}
#endif
