using System;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace IziHardGames.Ecs.Subscenes.ForUnity
{

	public class IziEcsBootstrap2 //: ICustomBootstrap
    {
        public bool Initialize(string defaultWorldName)
        {
            Debug.Log($"ICustomBootstrap2 {defaultWorldName}");
            return false;
        }
    }
    // Multiple custom ICustomBootstrap specified, ignoring IziHardGames.Ecs.Subscenes.ForUnity.IziEcsBootstrap
    public class IziEcsBootstrap : ICustomBootstrap
    {
        public bool Initialize(string defaultWorldName)
        {
            // true if the bootstrap has performed initialization, or false if default world initialization should be performed.
            Debug.Log($"ICustomBootstrap {defaultWorldName}");
            return false;
        }
    }

    internal class ManagerAuthoringBaker : Baker<ManagerAuthoring>
    {
        public float SomeValue;
        public override void Bake(ManagerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
        }
    }
    /// <summary>
    /// <see cref="Unity.Entities.Baker"/>
    /// </summary>
    internal class ManagerAuthoring : MonoBehaviour
    {

        public void Awake()
        {
            Debug.LogError("Awaked", this);
        }

        public void Test()
        {
            // Baker<ManagerAuthoring>
            // SceneSystem.LoadSceneAsync();
        }
    }
}
