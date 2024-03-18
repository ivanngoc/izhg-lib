using UnityEngine;


namespace IziHardGames.Libs.Engine.ResourcesManagment
{
	[CreateAssetMenu(menuName = "IziHardGames/Scriptable/ResourcesManagment/Sprites Groupe", fileName = "SpritesGroupe")]
	public class ScriptableSpritesGroupe : ScriptableObject
	{
		[SerializeField] public int idGroupe;
		[SerializeField] public Sprite[] sprites;

		#region Unity Message


		#endregion
	}
}