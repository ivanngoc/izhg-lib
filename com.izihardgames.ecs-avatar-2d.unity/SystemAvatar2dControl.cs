using IziHardGames.UserControl.Abstractions.Lib.Environments;
using IziHardGames.UserControl.Lib.Contexts;
using Unity.Entities;
using Unity.Transforms;
using System;
using IziHardGames.UserControl.Lib.UserActions;
using IziHardGames.UserControl.Abstractions.Lib;
using Unity.Mathematics;
using UnityEngine;
using IziHardGames.UserControl.InputSystem.Unity;
using IziHardGames.Apps.Abstractions.Lib;
using IziHardGames.UserControl.Unity;
using IziHardGames.Apps.Lib;
using Unity.Burst;
using Unity.Collections;

namespace IziHardGames.ForEcs.Avatar2d.ForUnity
{


    [DisableAutoCreation]
    public partial class SystemAvatar2dControl : SystemBase
    {
        private ContextForUserActionsV2 actions;
        private DataInput dataInput;
        private UserEnvironment env;
        private Camera camera;

        protected override void OnCreate()
        {
            this.env = IziEnvironment.Environments.Current.As<UserEnvironment>();
            this.actions = env.userActions;
            this.dataInput = env.User.GetInputData<DataInput>() ?? throw new NullReferenceException();
            /// <see cref="UserControlMonoService"/>
            camera = IIziApp.Singleton.GetSingleton<Camera>();
            if (camera == null)
            {
                IziApp.Singleton.OnSingletonAddSync<Camera>(SetCamera);
            }         
        }

        private void SetCamera(object obj)
        {
            IziApp.Singleton.OnSingletonAddReverse<Camera>(SetCamera);
            this.camera = obj as Camera ?? throw new NullReferenceException();
        }

        protected override void OnUpdate()
        {
            float3 f = float3.zero;

            if (actions.IsFired<MarkerMoveUp>())
            {
                f += math.up();
            }
            if (actions.IsFired<MarkerMoveDown>())
            {
                f += math.down();
            }
            if (actions.IsFired<MarkerMoveLeft>())
            {
                f += math.left();
            }
            if (actions.IsFired<MarkerMoveRight>())
            {
                f += math.right();
            }
            JobAvatarMove jobAvatar = new JobAvatarMove()
            {
                pos = f * (SystemAPI.Time.DeltaTime),
            };
            //if (false) CheckedStateRef.CompleteDependency(); /// нужен чтобы предотвращать InvalidOperationException
            jobAvatar.Schedule();

            if (camera != null)
            {
                JobRotateToCursor jobAvatarRotate = new JobRotateToCursor(dataInput)
                {
                    time = SystemAPI.Time.ElapsedTime,
                };
                jobAvatarRotate.Schedule();
                //this.SystemHandle.
                this.Dependency.Complete();
            }
        }

        protected override void OnStartRunning()
        {

        }

        protected override void OnStopRunning()
        {

        }
    }

    [WithAll(typeof(ComponentAvatar2dForEcs))]
    internal partial struct JobAvatarMove : IJobEntity
    {
        public float3 pos;
        public void Execute(ref LocalTransform transform, ref ComponentAvatar2dForEcs avatar)
        {
            transform.Position += pos * avatar.speed;
        }
    }

    [BurstCompile]
    [WithAll(typeof(ComponentRotateToTargetProjected))]
    internal partial struct JobRotateToCursor : IJobEntity
    {
        internal float3 pointerDirectionNormalized;
        internal float3 cameraPos;
        internal float3 cameraForward;
        internal double time;

        public JobRotateToCursor(DataInput dataInput) : this()
        {
            var camera = dataInput.cameraCurrent;
            var dir = dataInput.rayMainCameraToPointer.direction.normalized;
            var camPos = camera.transform.position;

            cameraForward = camera.transform.forward;
            pointerDirectionNormalized = new float3(dir.x, dir.y, dir.z);
            cameraPos = new float3(camPos.x, camPos.y, camPos.z);
        }

        [BurstCompile]
        public void Execute(ref LocalTransform transform)
        {
            var toTarget = transform.Position - cameraPos;
            var lookNoralized = pointerDirectionNormalized - math.normalize(toTarget);
            // для объекта у которого forward это Y
            transform.Rotation = quaternion.LookRotation(cameraForward, lookNoralized);
        }
    }
}
