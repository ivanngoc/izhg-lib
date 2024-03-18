using IziHardGames.View;
using UnityEngine;


namespace IziHardGames.Libs.Engine.View
{
	/// <summary>
	/// Update action with storableData
	/// </summary>
	public class UpdaterMaterialOfRenderer : IViewUpdaterMaterialOfRenderer
	{
		public Material materialToSet;
		public Renderer renderer;
	}
}