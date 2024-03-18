using System.Collections.Generic;
using IziHardGames.UserControl.Abstractions.NetStd21;

namespace IziHardGames.UserControl.Lib.UserActions
{
    public class TriggerFromInput : TriggerSource
    {
        private InputCollectTask inputCollectTask;
        private readonly List<Device> devices = new List<Device>();

        public override void Collect()
        {
            throw new System.NotImplementedException();
        }

		public override void Calculate()
		{
			throw new System.NotImplementedException();
		}
	}
}