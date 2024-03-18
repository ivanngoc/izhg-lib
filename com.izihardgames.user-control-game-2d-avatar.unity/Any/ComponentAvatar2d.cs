using System;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;
using IziHardGames.Apps.ForUnity;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.Abstractions.NetStd21.Environments;
using UnityEngine;

namespace IziHardGames.UserControl.Game2d.Avatar.Unity.Moving
{
    public class ComponentAvatar2d : MonoBehaviour, IScriptableId
    {
        public static ComponentAvatar2d Current;
        private static Vector3 leftUp = new Vector3(-1, 1, 0);
        private static Vector3 rightUp = new Vector3(1, 1, 0);
        private static Vector3 leftDown = new Vector3(-1, -1, 0);
        private static Vector3 rightDown = new Vector3(1, -1, 0);

        [SerializeField] public float speed = 1;
        [SerializeField] private bool isInitilized;
        [SerializeField] ScriptableId id;
        private User user;
        public ScriptableId Id { get => id; set => id = value; }

        public void Initilize(User user)
        {
            if (isInitilized) throw new System.InvalidOperationException("Object is already initilized");
            isInitilized = true;
            this.user = user;
        }
        private void OnEnable()
        {
            if (isInitilized != true) throw new InvalidOperationException("Object must be initilized");
            IziEnvironment.Environments[user.id].Modes[typeof(UModeAvatar2d)].Enable();
        }
#if UNITY_EDITOR
        private void Reset()
        {
            isInitilized = false;
            enabled = false;
        }
#endif

        private void OnDisable()
        {
            IziEnvironment.Environments[user.id].Modes[typeof(UModeAvatar2d)].Disable();
        }

        internal void MoveRight()
        {
            transform.position += Vector3.right * (Time.deltaTime * speed);
        }
        internal void MoveLeft()
        {
            transform.position += Vector3.left * (Time.deltaTime * speed);
        }

        internal void MoveUp()
        {
            transform.position += Vector3.up * (Time.deltaTime * speed);
        }
        internal void MoveDown()
        {
            transform.position += Vector3.down * (Time.deltaTime * speed);
        }

        internal void MoveLeftUp()
        {
            transform.position += leftUp * (Time.deltaTime * speed);
        }

        internal void MoveLeftDown()
        {
            transform.position += leftDown * (Time.deltaTime * speed);
        }

        internal void MoveRightUp()
        {
            transform.position += rightUp * (Time.deltaTime * speed);
        }
        internal void MoveRightDown()
        {
            transform.position += rightDown * (Time.deltaTime * speed);
        }

        private void Awake()
        {
            Current = this;
        }

        internal Vector3 GetPosition()
        {
            return transform.position;
        }

        internal void SetRotation(Quaternion quaternion)
        {
            transform.rotation = quaternion;
        }

    }
}
