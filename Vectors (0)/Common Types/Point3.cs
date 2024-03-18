using IziHardGames.Libs.NonEngine.Math.Float;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	/// <summary>
	/// Point at 3d Plane.
	/// Custom Vector3
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct Point3 : IEquatable<Point3>
	{
		public static readonly Point3 NaN;
		public static readonly Point3 zero;
		public static readonly Point3 up;
		public static readonly Point3 down;
		public static readonly Point3 left;
		public static readonly Point3 right;
		public static readonly Point3 forward;
		public static readonly Point3 back;

		[FieldOffset(0)] public float x;
		[FieldOffset(4)] public float y;
		[FieldOffset(8)] public float z;
		[FieldOffset(12)] public float magnitudeCachedByDemand;
		public float X { get => x; set => SetX(value); }
		public float Y { get => y; set => SetY(value); }
		public float Z { get => z; set => SetZ(value); }
		public float Magnitude { get => CalcMagnitude(ref this); }

		public Point3 normalized
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Normalize(this);
			}
		}
		static Point3()
		{
			NaN = new Point3(float.NaN, float.NaN, float.NaN);
			zero = new Point3(default, default, default);
			up = new Point3(0, 1, 0);
			down = new Point3(0, -1, 0);
			right = new Point3(1, 0, 0);
			left = new Point3(-1, 0, 0);
			forward = new Point3(0, 0, 1);
			back = new Point3(0, 0, -1);
		}
		public Point3(float x, float y) : this()
		{
			this.x = x;
			this.y = y;
		}
		public Point3(float x, float y, float z) : this()
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public void SetX(float x)
		{
			this.x = x;
			magnitudeCachedByDemand = CalcMagnitude(ref this);
		}
		public void SetY(float y)
		{
			this.y = y;
			magnitudeCachedByDemand = CalcMagnitude(ref this);
		}
		public void SetZ(float z)
		{
			this.z = z;
			magnitudeCachedByDemand = CalcMagnitude(ref this);
		}

		public void Multiply(float value)
		{
			x *= value;
			y *= value;
			z *= value;
		}

		public override string ToString()
		{
			return $"({x.ToString()},{y.ToString()},{z.ToString()})";
		}
		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}
		public bool Equals(Point3 other)
		{
			return this == other;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(x, y, z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point3 Normalize(Point3 value)
		{
			float num = CalcMagnitude(ref value);
			if (num > 1E-05f)
			{
				return value / num;
			}
			return zero;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcMagnitude(ref Point3 vector)
		{
			vector.magnitudeCachedByDemand = MathF.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
			return vector.magnitudeCachedByDemand;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CalcMagnitude(float x, float y, float z)
		{
			return MathF.Sqrt(x * x + y * y + z * z);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point3 Lerp(Point3 start, Point3 end, float t)
		{
			t = MathForFloat.Clamp01(t);
			return new Point3(start.x + (end.x - start.x) * t, start.y + (end.y - start.y) * t, start.z + (end.z - start.z) * t);
		}

		public static Point3 Lerp(ref Point3 start, ref Point3 end, float t)
		{
			t = MathForFloat.Clamp01(t);
			return new Point3(start.x + (end.x - start.x) * t, start.y + (end.y - start.y) * t, start.z + (end.z - start.z) * t);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Point3 lhs, Point3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point3 Project(ref Point3 vector, ref Point3 onNormal)
		{
			float num = Dot(onNormal, onNormal);

			if (num < MathForFloat.Epsilon)
			{
				return zero;
			}
			float num2 = Dot(vector, onNormal);
			return new Point3(onNormal.x * num2 / num, onNormal.y * num2 / num, onNormal.z * num2 / num);
		}

		public static bool operator !=(Point3 a, Point3 b)
		{
			if (a.x != b.x) return true;
			if (a.y != b.y) return true;
			if (a.z != b.z) return true;
			return false;
		}
		public static bool operator ==(Point3 a, Point3 b)
		{
			if (a.x != b.x) return false;
			if (a.y != b.y) return false;
			if (a.z != b.z) return false;
			return true;
		}
		public static Point3 operator -(Point3 a, Point3 b)
		{
			return new Point3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Point3 operator +(Point3 a, Point3 b)
		{
			return new Point3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static Point3 operator *(Point3 vector, float value)
		{
			return new Point3(vector.x * value, vector.y * value, vector.z);
		}
		public static Point3 operator *(float value, Point3 vector)
		{
			return new Point3(vector.x * value, vector.y * value, vector.z);
		}
		public static Point3 operator /(Point3 a, float d)
		{
			return new Point3(a.x / d, a.y / d, a.z / d);
		}

		public static implicit operator Point2(Point3 point) => new Point2(point.x, point.y);
		public static explicit operator Point3(Point2 point) => new Point3(point.x, point.y);
	}
}
