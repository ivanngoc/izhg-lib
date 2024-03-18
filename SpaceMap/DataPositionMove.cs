using IziHardGames.Libs.NonEngine.Vectors;
using System;

namespace IziHardGames.Libs.Engine.Components.NonMono
{
	/// <summary>
	/// Serilizable class For "hot" saving  state. 
	/// </summary>
	[Serializable]
	public class DataPositionMove
	{		
		public Point3 start;
		public Point3 end;
		public Point3 direction;
		public Point3 direction01;

		public float distance;
		public float distanceLeft;
		public float speed;
		public bool isMoving;

		public DataPositionMove()
		{
		

		}

		private bool IsMovingReverse()
		{
			return !isMoving;
		}
		private bool IsMoving()
		{
			return isMoving;
		}
	}
}