using System;
using UnityEngine;

namespace IziHardGames.Libs.Engine.SpaceMap.QuadMap
{
	/// <summary>
	/// Itterator for getting each position at distance == <see cref="radius"/> from <see cref="center"/>.
	/// Moving starts at left bot corner.
	/// Order by sides: left, top, right, bot.
	/// </summary>
	public struct ItteratorForQuadMapCircled
	{
		public int countOfElementsInCircle;
		public int radius;
		public int i;
		public Vector3Int center;
		public Vector3Int Current { get; set; }
		public Vector3Int[] offsets;

		public ItteratorForQuadMapCircled(Vector3Int[] offsets, Vector3Int center, int radius) : this()
		{
			this.center = center;
			this.radius = radius;
			this.offsets = offsets;
		}

		public bool MoveNext()
		{
			throw new NotImplementedException();
		}
	}
}