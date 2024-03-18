using Unity.Entities;
using UnityEngine;

namespace IziHardGames.Apps.ForEcs.Abstractions.ForUnity
{
    public class UnityEcsBoostrapMono : MonoBehaviour
    {
        [SerializeField] private bool isInitilized;
        private void Awake()
        {
            isInitilized = false;
        }
        private void Update()
        {
            if (World.DefaultGameObjectInjectionWorld.IsCreated)
            {
                enabled = false;
                IziUnityEcs.NotifyDefaultWorldCreated(World.DefaultGameObjectInjectionWorld);
                isInitilized = true;
            }
        }

        private void OnValidate()
        {
            isInitilized = false;
        }
    }
}
