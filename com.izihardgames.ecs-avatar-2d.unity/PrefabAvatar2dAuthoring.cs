using Unity.Entities;
using UnityEngine;

namespace IziHardGames.ForEcs.Avatar2d.ForUnity
{
    public class PrefabAvatar2dAuthoring : MonoBehaviour
    {
        public GameObject prefab;
    }

    public class PrefabAvatar2dBaker : Baker<PrefabAvatar2dAuthoring>
    {
        public override void Bake(PrefabAvatar2dAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ComponentPrefabAvatar2d()
            {
                value = entityPrefab,
            });
        }
    }

    public struct ComponentPrefabAvatar2d : IComponentData
    {
        public Entity value;
    }
}
