using IziHardGames.Attributes;
using IziHardGames.UserControl.Abstractions.Lib;
using Unity.Entities;

namespace IziHardGames.UserControl.InputSystem.ForEcs
{
    [InputCollector]
    [DisableAutoCreation]
    public partial class SystemCollectInput : SystemBase
    {
        public void Initilize()
        {

        }

        protected override void OnUpdate()
        {
            //IziInputSystem.
            throw new System.NotImplementedException();
        }
    }
}
