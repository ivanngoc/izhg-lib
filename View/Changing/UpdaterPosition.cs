using IziHardGames.View;
using UnityEngine;


namespace IziHardGames.Libs.Engine.View
{
	/// <summary>
	/// Update action with storableData
	/// </summary>
	public class UpdaterPosition : IViewUpdaterPosition
	{
		public Transform transform;
		public Vector3 positionToSet;
	}
}