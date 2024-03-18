using System;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	[Serializable]
	public struct Point3Int : IEquatable<Point3Int>, IComparable<Point3Int>, IVector3<int>
	{
		public readonly static Point3Int zero;
		public readonly static Point3Int one;
		public static ref readonly Point3Int ByRefZero { get => ref zero; }
		public static ref readonly Point3Int ByRefOne { get => ref zero; }

		public int x;
		public int y;
		public int z;

		public int X { get => x; set => x = value; }
		public int Y { get => y; set => y = value; }
		public int Z { get => z; set => z = value; }
		static Point3Int()
		{
			zero = new Point3Int(0, 0, 0);
			one = new Point3Int(1, 1, 1);
		}
		public Point3Int(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public int ToCellIndexOrigin(Point2Int size, Point2Int origin)
		{
			return (y - origin.y) * size.x + (x - origin.x);
		}
		public int ToCellIndex(Point3Int size, Point3Int origin)
		{
			return (y - origin.y) * size.x + (x - origin.x);
		}

		public static Point3Int operator +(Point3Int a, Point3Int b) => new Point3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		public static Point3Int operator -(Point3Int a, Point3Int b) => new Point3Int(a.x - b.x, a.y - b.y, a.z - b.z);

		public static bool operator ==(Point3Int a, Point3Int b)
		{
			if (a.x != b.x) return false;
			if (a.y != b.y) return false;
			if (a.z != b.z) return false;
			return true;
		}
		public static bool operator !=(Point3Int a, Point3Int b) => a.x != b.x || a.y != b.y || a.z != b.z;

		public override string ToString()
		{
			return $"({x.ToString()},{y.ToString()},{z.ToString()})";
		}
		public bool Equals(Point3Int other)
		{
			return this == other;
		}
		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}
		public override int GetHashCode()
		{
			int hashCode = 373119288;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			hashCode = hashCode * -1521134295 + z.GetHashCode();
			return hashCode;
		}
		public int CompareTo(Point3Int other)
		{
			throw new NotImplementedException();
		}
	}
}