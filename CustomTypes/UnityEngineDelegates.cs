using UnityEngine;

namespace IziHardGames.Libs.Engine.CustomTypes
{
	public class UnityEngineDelegates : MonoBehaviour
	{
		#region Unity Message


		#endregion
	}
}

namespace UnityEngine
{

	public delegate void ActionVector2Ref(ref Vector2 vector2);
	public delegate void ActionVector3Ref(ref Vector3 vector3);


}
