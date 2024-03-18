
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.InputSystem.ForUnity;

// using IziHardGames.UserControl.InputSystem.Unity;
using UnityEngine;

namespace IziHardGames.UserControl.Game2d.Avatar.Unity.Moving
{
	public class RotateToCursor : UserActionRotation
    {
        public override void Execute()
        {
            var dataInput = Users.Curernt.GetInputData<DataInput>();
            var cam = dataInput.cameraCurrent;
            if (cam.orthographic)
            {
                var posTo = dataInput.pointerPosAtWorld;
                posTo.z = 0;
                var posFrom = ComponentAvatar2d.Current.GetPosition();
                posFrom.z = 0;
                var dir = posTo - posFrom;
                ComponentAvatar2d.Current.SetRotation(Quaternion.LookRotation(Vector3.forward, dir));
            }
            else
            {
                var posFrom = ComponentAvatar2d.Current.GetPosition();
                var posTo = DataInput.ProjectCursorToTargetPlanePerspective(posFrom, cam.transform.position, dataInput.rayMainCameraToPointer.direction);
                var dir = posTo - posFrom;
                ComponentAvatar2d.Current.SetRotation(Quaternion.LookRotation(Vector3.forward, dir));
            }
        }
    }
}
