using System;
using System.Collections.Generic;
using System.Text;

namespace Xe
{
    public static partial class Math
	{
		public static sbyte Min(sbyte x, sbyte y) => System.Math.Min(x, y);
		public static byte Min(byte x, byte y) => System.Math.Min(x, y);
		public static short Min(short x, short y) => System.Math.Min(x, y);
		public static ushort Min(ushort x, ushort y) => System.Math.Min(x, y);
		public static int Min(int x, int y) => System.Math.Min(x, y);
		public static uint Min(uint x, uint y) => System.Math.Min(x, y);
		public static long Min(long x, long y) => System.Math.Min(x, y);
		public static ulong Min(ulong x, ulong y) => System.Math.Min(x, y);
		public static float Min(float x, float y) => System.Math.Min(x, y);
		public static double Min(double x, double y) => System.Math.Min(x, y);
		public static decimal Min(decimal x, decimal y) => System.Math.Min(x, y);

		public static sbyte Max(sbyte x, sbyte y) => System.Math.Max(x, y);
		public static byte Max(byte x, byte y) => System.Math.Max(x, y);
		public static short Max(short x, short y) => System.Math.Max(x, y);
		public static ushort Max(ushort x, ushort y) => System.Math.Max(x, y);
		public static int Max(int x, int y) => System.Math.Max(x, y);
		public static uint Max(uint x, uint y) => System.Math.Max(x, y);
		public static long Max(long x, long y) => System.Math.Max(x, y);
		public static ulong Max(ulong x, ulong y) => System.Math.Max(x, y);
		public static float Max(float x, float y) => System.Math.Max(x, y);
		public static double Max(double x, double y) => System.Math.Max(x, y);
		public static decimal Max(decimal x, decimal y) => System.Math.Max(x, y);

		public static sbyte Abs(sbyte x, sbyte y) => System.Math.Abs(x);
		public static short Abs(short x, short y) => System.Math.Abs(x);
		public static int Abs(int x, int y) => System.Math.Abs(x);
		public static long Abs(long x, long y) => System.Math.Abs(x);
		public static float Abs(float x, float y) => System.Math.Abs(x);
		public static double Abs(double x, double y) => System.Math.Abs(x);
		public static decimal Abs(decimal x, decimal y) => System.Math.Abs(x);

		public static sbyte Range(sbyte x, sbyte min, sbyte max) => x > min ? x < max ? x : max : min;
		public static byte Range(byte x, byte min, byte max) => x > min ? x < max ? x : max : min;
		public static short Range(short x, short min, short max) => x > min ? x < max ? x : max : min;
		public static ushort Range(ushort x, ushort min, ushort max) => x > min ? x < max ? x : max : min;
		public static int Range(int x, int min, int max) => x > min ? x < max ? x : max : min;
		public static uint Range(uint x, uint min, uint max) => x > min ? x < max ? x : max : min;
		public static long Range(long x, long min, long max) => x > min ? x < max ? x : max : min;
		public static ulong Range(ulong x, ulong min, ulong max) => x > min ? x < max ? x : max : min;
		public static float Range(float x, float min, float max) => x > min ? x < max ? x : max : min;
		public static double Range(double x, double min, double max) => x > min ? x < max ? x : max : min;
		public static decimal Range(decimal x, decimal min, decimal max) => x > min ? x < max ? x : max : min;

        public static int Round(double x)
        {
            return (int)System.Math.Round(x);
        }
	}
}
