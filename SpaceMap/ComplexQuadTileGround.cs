namespace IziHardGames.Libs.NonEngine.Graphs.QuadTile.Ground
{
	public class ComplexQuadTileGround
	{
		public ManagerCircuits managerCircuits;
		public DataGroundDynamic dataGroundDynamic;
		public DataGroundStatic dataGroundStatic;

		public void Bind(DataGroundDynamic dataGroundDynamic, DataGroundStatic dataGroundStatic, ManagerCircuits managerCircuits)
		{
			this.dataGroundDynamic = dataGroundDynamic;
			this.dataGroundStatic = dataGroundStatic;
			this.managerCircuits = managerCircuits;

			managerCircuits.dataGroundDynamic = dataGroundDynamic;
			managerCircuits.dataGroundStatic = dataGroundStatic;

			JobFindCircuits.dataGroundStatic = dataGroundStatic;
		}
	}
	//public readonly struct CircuiteGroupe
	//{
	//	public readonly int indexStart;
	//	public readonly int indexEnd;
	//	public readonly int idGroupe;
	//}
}//namespace
