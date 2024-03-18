using IziHardGames.Apps.ForEcs.ForUnity;
using Unity.Entities;
using UnityEngine;

namespace IziHardGames.ForEcs.Avatar2d.ForUnity
{
    public class EcseAvatar2dAuthoring : MonoBehaviour
    {
        [SerializeField, Range(1f, 10f)] private float speedMove = 1f;
        [SerializeField] private float speedAttack = 1f;
        [SerializeField] private float speedProjectile = 5f;
        [SerializeField] public GameObject projectilePrefab;
        public float Speed => speedMove;
        public float SpeedAttack => speedAttack;
        public float SpeedProjectile => speedProjectile;

        public class EcseAvatar2dBaker : Baker<EcseAvatar2dAuthoring>
        {
            public override void Bake(EcseAvatar2dAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ComponentAvatar2dForEcs() { speed = authoring.Speed });
                AddComponent(entity, new ComponentRotateToTargetProjected() { });
                AddComponent(entity, new ComponentAttackSpeed() { atacksPerSecond = authoring.SpeedAttack });

                var projectile = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic);

                AddComponent(entity, new ComponentAttackWithProjectile() { projectile = projectile, projectileSpeed = authoring.SpeedProjectile });
            }
        }
    }
}
