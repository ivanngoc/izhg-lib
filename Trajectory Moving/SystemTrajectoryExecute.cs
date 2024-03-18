using System.Collections.Generic;
using UnityEngine;


namespace IziHardGames.Libs.Engine.TrajectoryMoving
{
	/// <summary>
	/// For transforms
	/// </summary>
	public class SystemTrajectoryExecute : MonoBehaviour
	{
		[SerializeField] List<ComponentTrajectory> componentTrajectories = new List<ComponentTrajectory>(100);

		#region Unity Message

		#endregion

		public void Execute()
		{
			foreach (var componentTrajectory in componentTrajectories)
			{
				componentTrajectory.MoveForward(Time.deltaTime);
			}
		}
	}
}