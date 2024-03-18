using IziHardGames.VFX.ForUnity.ForEcs;
using Unity.Entities;
using UnityEngine;


namespace IziHardGames.VFX.ForEcs.Examples.BlinkOnDamage
{
    public class ExampleBlinkOnDamageCubeBootstrap : MonoBehaviour
    {
        private void Update()
        {
            if (World.DefaultGameObjectInjectionWorld.IsCreated)
            {
                var world = World.DefaultGameObjectInjectionWorld;
                var groupe = world.GetOrCreateSystemManaged<Unity.Entities.SimulationSystemGroup>();

                var handle = world.CreateSystem<EcsSystemBlinkOnDamageRotate>();
                groupe.AddSystemToUpdateList(handle);

                var handle0 = world.CreateSystem<EcsSystemBlinkOnDamageSimmulateHit>();
                groupe.AddSystemToUpdateList(handle0);

                var handle1 = world.CreateSystem<EcsSystemBlinkByDamageRunVFX>();
                groupe.AddSystemToUpdateList(handle1);

                enabled = false;
            }
        }
    }
}