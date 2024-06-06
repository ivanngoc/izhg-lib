using UnityEngine;
using IziHardGames.UserControl.Abstractions.NetStd21;
using UnityEngine.InputSystem;
using System.Linq;

namespace IziHardGames.UserControl.InputSystem.ForUnity
{
    internal class FactoryForUnityPointerContainer: InputIContainerFactory
    {
		public override InputContainer GetNew(int id, int value, Device device)
		{
            UnityPointerContainer container = new UnityPointerContainer(value, device);
            return container;
        }
	}
    internal class FactoryForUnityKeyItem : InputIContainerFactory
    {
        public override InputContainer GetNew(int id, int keycode, Device device)
        {
            UnityInputKeyContainer unityKey = new UnityInputKeyContainer(keycode, device);
            unityKey.key = (Key)keycode;
            return unityKey;
        }
    }

    internal class UnityDeviceFinder : DeviceFinder
    {
        public override Device ByTarget(object target)
        {
            if (target is InputDevice device)
            {
                if (TryFindDeviceBySerial(device, out Device result)) return result;
                return Device.NotFound;
            }
            else
            {
                throw new System.NotImplementedException();
            }
        }

        public static bool TryFindDeviceBySerial(InputDevice device, out Device result)
        {
            string serial = device.description.serial;
            result = RegistryForDevices.devices.FirstOrDefault(x => x.serial == serial);
            return result != null;
        }
    }
}