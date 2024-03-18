using System;
using IziHardGames.UserControl.Abstractions.NetStd21;
using IziHardGames.UserControl.InputSystem.ForUnity;

namespace IziHardGames.UserControl.ForUnity
{
    public class UserMono : User
    {
        private DataInput dataInput;

        public UserMono(DataInput dataInput) : base(DateTime.Now.GetHashCode())
        {
            this.dataInput = dataInput;
        }

        public override T GetInputData<T>()
        {
            return (dataInput as T) ?? throw new System.NullReferenceException("data input is not set");
        }
    }
}