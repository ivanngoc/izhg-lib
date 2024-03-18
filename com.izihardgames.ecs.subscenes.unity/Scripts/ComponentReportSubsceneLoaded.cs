using IziHardGames.Apps.ForEcs.Abstractions.ForUnity;
using IziHardGames.Apps.Scenes.Unity;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace IziHardGames.Ecs.Subscenes.ForUnity
{
    [RequireComponent(typeof(SubScene))]
    public class ComponentReportSubsceneLoaded : MonoBehaviour, IAwaitableForSceneReport
    {
        public bool IsFinished { get => isFinished; }
        [SerializeField] private SubScene subScene;
        [SerializeField] private bool isFinished;
        private void Reset()
        {
            subScene = GetComponent<SubScene>();
            enabled = true;
        }
        private void Update()
        {
            //if (World.DefaultGameObjectInjectionWorld.IsCreated)
            //{
            //var world = World.DefaultGameObjectInjectionWorld;
            if (IziUnityEcs.TryGetDefaultWorld(out var world))
            {
                var entity = SceneSystem.GetSceneEntity(world.Unmanaged, subScene.SceneGUID);
                if (SceneSystem.IsSceneLoaded(world.Unmanaged, entity))
                {
                    enabled = false;
                    isFinished = true;
                }
            }
            //}
        }
    }
}
