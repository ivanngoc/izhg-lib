using Unity.Entities;

namespace IziHardGames.ForEcs.Avatar2d.ForUnity
{
	/// <summary>
	/// Поворот к курсору
	/// </summary>
	public struct ComponentRotateToTargetProjected : IComponentData
	{

	}

	public struct ComponentAvatar2dForEcs : IComponentData
	{
		public float speed;
	}
    public struct ComponentAvatarSingleton : IComponentData
    {
        public Entity avatar;
        public ComponentAvatar2dForEcs component;
	}
}
