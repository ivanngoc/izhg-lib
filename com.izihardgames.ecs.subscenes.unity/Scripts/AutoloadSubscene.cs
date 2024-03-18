using IziHardGames.Apps.ForEcs.Abstractions.ForUnity;
using Unity.Scenes;
using UnityEngine;

namespace IziHardGames.Ecs.Subscenes.ForUnity
{
    [RequireComponent(typeof(SubScene))]
    public class AutoloadSubscene : MonoBehaviour
    {
        [SerializeField] private SubScene subScene;

        private void Reset()
        {
            subScene.AutoLoadScene = true;
            subScene = GetComponent<SubScene>();
        }
        public void OnEnable()
        {
            if (IziUnityEcs.TryGetDefaultWorld(out var world))
            {
                //Unity.Scenes.SceneSystem.LoadSceneAsync(world);

            }
        }
    }
}
