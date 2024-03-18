using IziHardGames.Apps.NetStd21;
using IziHardGames.Apps.ForUnity;
using IziHardGames.UserControl.Abstractions.NetStd21;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

namespace IziHardGames.UserControl.InputSystem.ForUnity
{
    public class DataInput : IInputDataSet
    {
#if UNITY_EDITOR || DEBUG
        public static DataInput? forEditor;
        public static int instances;
#endif
        public Vector3 worldPosMainCamera;

        // when devices like mouse or touchscreen perform press
        public Vector3 pointerPosAtScreenPressed;
        public Vector3 pointerPosAtScreenPressedPrevious;
        public Vector3 pointerPosAtScreenPressedDelta;

        public Vector3 pointerPosAtScreen3d;
        public Vector3 pointerPosAtScreen3dPrevious;
        public Vector3 pointerPosAtScreen3dDelta;
        public Vector3 pointerPosAtScreen3dDeltaNormalized;

        public Vector2 pointerPosAtScreen2dNew;
        public Vector2 pointerPosAtScreen2dPrevious;

        public Vector3 pointerPosAtWorld;
        public Vector3 pointerPosAtWorldPrevious;
        /// <summary>
        /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.Mouse.html#UnityEngine_InputSystem_Mouse_scroll
        /// </summary>
        public Vector2 scroll;

        #region TouchScreen
        /// <summary>
        /// Вектор между двумя первыми точками тачскрина
        /// </summary>
        public Vector2 twoTouchDelta;
        public Vector2 twoTouchDeltaPrevious;
        public Vector2 twoTochDeltaDiff;

        public Vector2 twoTouchDeltaCenter;
        public Vector2 twoTouchDeltaCenterPrevious;
        public Vector2 twoTouchDeltaCenterDiff;

        public float twoTouchLength;
        /// <summary>
        /// Delta of <see cref="twoTouchLength"/> between current and previous frames
        /// </summary>
        public float twoTouchLengthDelta;
        public float twoTouchLengthPrevious;
        #endregion

        public Vector3 screenPosV3ClickNew;
        public Vector2 screenPosV2ClickNew;

        public Ray rayMainCameraToPointer;

        /// <summary>
        /// Поинтер был перемещен в этом кадре
        /// </summary>
        public bool isPointerMoved;
        public bool isLeftClickHold;
        public bool isLeftClickPressedThisUpdate;
        public bool isLeftClicPressedLastUpdate;

        public float mouseLeftButton;
        public float mouseRightButton;
        public float mouseMiddleButton;

        public Camera? cameraCurrent;

        public DataInput()
        {
            cameraCurrent = IziApp.Singleton?.GetSingleton<Camera>();
            AppMonoGlobalEvents.OnMainCameraChange += SetCamera;
            AppMonoGlobalEvents.OnNoCamera += SetCameraReverse;
#if UNITY_EDITOR || DEBUG
            Interlocked.Increment(ref instances);
            forEditor = this;
#endif
        }

        public void SetCamera(Camera camera)
        {
            cameraCurrent = camera;
        }
        public void SetCameraReverse()
        {
            cameraCurrent = default;
        }
        /// <summary>
        /// Расчитать производные данные об указателе
        /// </summary>
        public void CalculatePointerGenerics(Pointer pointer)
        {
            pointerPosAtScreen3dPrevious = pointerPosAtScreen3d;
            pointerPosAtScreen2dPrevious = pointerPosAtScreen3dPrevious;

            pointerPosAtScreen2dNew = pointer.position.ReadValue();
            pointerPosAtScreen3d = pointerPosAtScreen2dNew;

            isPointerMoved = (pointerPosAtScreen3dPrevious - pointerPosAtScreen3d).sqrMagnitude > 0;

            pointerPosAtScreen3dDelta = pointerPosAtScreen3d - pointerPosAtScreen3dPrevious;
            pointerPosAtScreen3dDeltaNormalized = pointerPosAtScreen3dDelta.normalized;

            pointerPosAtWorldPrevious = pointerPosAtWorld;

            if (cameraCurrent != null)
            {
                pointerPosAtScreen3d.z = cameraCurrent.nearClipPlane;
                rayMainCameraToPointer = cameraCurrent!.ScreenPointToRay(pointerPosAtScreen3d);
                if (cameraCurrent.orthographic)
                {
                    pointerPosAtWorld = cameraCurrent.ScreenToWorldPoint(pointerPosAtScreen3d);
                }
                else
                {
                    pointerPosAtWorld = (rayMainCameraToPointer.direction.normalized * cameraCurrent.nearClipPlane) + cameraCurrent.transform.position;
#if UNITY_EDITOR
                    //Debug.DrawRay(rayMainCameraToPointer.origin, rayMainCameraToPointer.direction, Color.cyan);
                    //Debug.DrawLine(cameraCurrent.transform.position, pointerPosAtWorld, Color.magenta);
#endif
                }
            }
        }

        /// <summary>
        /// Расчет точки процецирования от камеры через позицию указателя на расстояние Z между камерой и объектом.
        /// </summary>
        /// <param name="targetPosition">позиция целового объекта для выравнивания глубины Z</param>
        /// <param name="cameraPosition"></param>
        /// <param name="pointerDirection">напрвления указателя для перспективной камеры</param>
        /// <returns></returns>
        public static Vector3 ProjectCursorToTargetPlanePerspective(Vector3 targetPosition, Vector3 cameraPosition, Vector3 pointerDirection)
        {
            var toTarget = targetPosition - cameraPosition;
            // перпендикуляр плоскости для обнаружения глубины (уровня) относительно Camera.transofrm.Forward. По сути перпендикуляр к видимой плоскости от камеры
            var porjToTargetPlane = Vector3.Project(toTarget, Vector3.forward);
            var angleDegree = Vector3.Angle(porjToTargetPlane, pointerDirection);
            var angleRad = angleDegree * Mathf.Deg2Rad;
            var cos = Mathf.Cos(angleRad);
            var dest = (toTarget.magnitude / cos) * pointerDirection.normalized + cameraPosition;
#if DEBUG
            //Debug.Log($"angleDegree:{angleDegree}; angleRad:{angleRad}; tan:{cos};");
            //Debug.DrawLine(dest, cameraPosition, Color.cyan);
            //Debug.DrawLine(porjToTargetPlane + cameraPosition, cameraPosition, Color.magenta);
#endif
            return dest;
        }
    }
}