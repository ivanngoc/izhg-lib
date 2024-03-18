using IziHardGames.Libs.NonEngine.Math.Float;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathf = IziHardGames.Libs.NonEngine.Math.Float.MathForFloat;
using Vector2 = IziHardGames.Libs.NonEngine.Vectors.Point2;

namespace IziHardGames.Libs.NonEngine.Vectors
{
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 8)]
	public struct Point2 : IEquatable<Point2>
	{
		[FieldOffset(0)] public float x;
		[FieldOffset(4)] public float y;

		public const float kEpsilon = 1E-05F;
		public const float kEpsilonNormalSqrt = 1E-15F;

		public readonly static Point2 NaN;
		public readonly static Point2 zero;
		public readonly static Point2 one;
		public readonly static Point2 left;
		public readonly static Point2 right;
		public readonly static Point2 down;
		public readonly static Point2 up;

		public float Magnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (float)System.Math.Sqrt(x * x + y * y);
			}
		}
		public float sqrMagnitude
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return x * x + y * y;
			}
		}
		public Point2 normalized
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				Point2 result = new Point2(x, y);
				result.Normalize();
				return result;
			}
		}

		static Point2()
		{
			NaN = new Point2(float.NaN, float.NaN);
			zero = new Point2();
			one = new Point2(1, 1);
			left = new Point2(-1, 0);
			right = new Point2(1, 0);
			up = new Point2(0, 1);
			down = new Point2(0, -1);
		}
		public Point2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			throw new NotSupportedException();
		}
		public bool Equals(Point2 other)
		{
			return this == other;
		}
		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Normalize()
		{
			float num = Magnitude;
			if (num > 1E-05f)
			{
				this /= num;
			}
			else
			{
				this = zero;
			}
		}
		public static Point2 Project(Point2 vector, Point2 onNormal)
		{
			float num = Dot(onNormal, onNormal);
			if (num < MathForFloat.Epsilon)
			{
				return zero;
			}
			float num2 = Dot(vector, onNormal);
			return new Point2(onNormal.x * num2 / num, onNormal.y * num2 / num);
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(Point2 lhs, Point2 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(Vector2 from, Vector2 to)
		{
			float num = (float)System.Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
			if (num < 1E-15f)
			{
				return 0f;
			}

			float num2 = Mathf.Clamp(Dot(from, to) / num, -1f, 1f);
			return (float)System.Math.Acos(num2) * 57.29578f;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SignedAngle(Vector2 from, Vector2 to)
		{
			float num = Angle(from, to);
			float num2 = Mathf.Sign(from.x * to.y - from.y * to.x);
			return num * num2;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(Vector2 a, Vector2 b)
		{
			float num = a.x - b.x;
			float num2 = a.y - b.y;
			return (float)System.Math.Sqrt(num * num + num2 * num2);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator +(Point2 a, Point2 b)
		{
			return new Point2(a.x + b.x, a.y + b.y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator -(Point2 a, Point2 b)
		{
			return new Point2(a.x - b.x, a.y - b.y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator *(Point2 a, Point2 b)
		{
			return new Point2(a.x * b.x, a.y * b.y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator /(Point2 a, Point2 b)
		{
			return new Point2(a.x / b.x, a.y / b.y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator -(Point2 a)
		{
			return new Point2(0f - a.x, 0f - a.y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator *(Point2 a, float d)
		{
			return new Point2(a.x * d, a.y * d);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator *(float d, Point2 a)
		{
			return new Point2(a.x * d, a.y * d);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point2 operator /(Point2 a, float d)
		{
			return new Point2(a.x / d, a.y / d);
		}
		public static bool operator ==(Point2 a, Point2 b) => a.x == b.x && a.y == b.y;
		public static bool operator !=(Point2 a, Point2 b) => a.x != b.x || a.y != b.y;

		public static implicit operator Point3(Point2 Point2) => new Point3(Point2.x, Point2.y);
	}
}
