using IziHardGames.Core;

#if UNITY_EDITOR

namespace IziHardGames.NamespaceWrappers
{
	public class InterfacesEngineCore
	{
		#region Unity Message
		#endregion
	}
}
#endif

namespace IziHardGames.Libs.Engine.Core
{
	/// <summary>
	/// ƒл¤ объектов одного типа которые имеют каждый свой уникальный id но созаютс¤ в количестве больше 1 (пол¤ id не константные и не статические)
	/// </summary>
	public interface IUniqueRef : IUnique
	{
		IUniqueComponent IUniqueComponent { get; set; }
	}
}