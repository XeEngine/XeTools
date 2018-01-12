﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Tools.Modules
{
	public partial class TileCollision
	{
		public static class Effects
		{
			public static readonly Guid None = Guid.Empty;
			public static readonly Guid Solid = new Guid(0x4a986433, 0x9311, 0x41bb, 0x8b, 0x23, 0xe1, 0x42, 0xed, 0xfd, 0x90, 0xa3);
			public static readonly Guid SolidShaped = new Guid(0x82877ad2, 0xfe94, 0x4223, 0x93, 0x44, 0x8d, 0xea, 0x6d, 0x09, 0xf3, 0xfc);
			public static readonly Guid LayerChangeAbsolute = new Guid(0x3a9f7524, 0xe1d2, 0x49c2, 0x9a, 0xbf, 0xf3, 0x5b, 0x75, 0x9a, 0x3b, 0x2e);
			public static readonly Guid LayerChangeRelative = new Guid(0x423152d2, 0xcd10, 0x4e57, 0x9a, 0x5d, 0x64, 0x38, 0xcc, 0x01, 0xbf, 0x26);
			public static readonly Guid DepthChangeAbsolute = new Guid(0xc2485aa3, 0x32a9, 0x40ee, 0xa5, 0xc0, 0x43, 0x12, 0x50, 0xe9, 0xd1, 0x1a);
			public static readonly Guid DepthChangeRelative = new Guid(0x15e1086a, 0xe205, 0x4745, 0x8f, 0xb0, 0x11, 0x52, 0x56, 0xc7, 0xc2, 0x7f);
			public static readonly Guid LayerChangeAbsoluteDepthLS = new Guid(0x950d4f8, 0x2583, 0x4c33, 0x80, 0xd5, 0x2, 0xc9, 0x7d, 0xf, 0x55, 0xb1);
			public static readonly Guid LayerChangeAbsoluteDepthLE = new Guid(0x58c38572, 0xa5fb, 0x424d, 0xad, 0x38, 0xd1, 0x68, 0xfc, 0xbe, 0x9e, 0x29);
			public static readonly Guid LayerChangeAbsoluteDepthGT = new Guid(0x3d0f0590, 0x39e5, 0x4a75, 0x99, 0xa4, 0x74, 0xe9, 0xa6, 0xa4, 0x5b, 0x5e);
			public static readonly Guid LayerChangeAbsoluteDepthGE = new Guid(0x8a443148, 0xcd4b, 0x4bd8, 0x80, 0xcf, 0x41, 0x7d, 0xb5, 0xeb, 0xca, 0xd3);
			public static readonly Guid LayerChangeRelativeDepthLS = new Guid(0xc8a8b4da, 0x1901, 0x4ca5, 0x81, 0x8d, 0x28, 0x70, 0x21, 0xb1, 0xad, 0x7c);
			public static readonly Guid LayerChangeRelativeDepthLE = new Guid(0x9f1ca035, 0x943, 0x4c60, 0x88, 0x7a, 0x90, 0xca, 0x1f, 0x5e, 0xa8, 0x95);
			public static readonly Guid LayerChangeRelativeDepthGT = new Guid(0xd682bccb, 0xbf6e, 0x4d17, 0x84, 0x5, 0x28, 0xf6, 0x6, 0x88, 0x71, 0xdf);
			public static readonly Guid LayerChangeRelativeDepthGE = new Guid(0xcbef38fc, 0xc151, 0x4e11, 0xac, 0x4b, 0x6d, 0x14, 0xb, 0xc7, 0x4c, 0x7a);
			public static readonly Guid LayerChangeAbsoluteDepthReach = new Guid(0x29e609e, 0xc625, 0x4ac8, 0xb7, 0x4c, 0x3, 0xc5, 0x3d, 0xf7, 0x9d, 0x66);
			public static readonly Guid LayerChangeRelativeDepthReach = new Guid(0xa4ed3e60, 0xd6fe, 0x4c39, 0xb4, 0x27, 0xe0, 0xce, 0x43, 0x10, 0x8a, 0x35);
			public static readonly Guid Climb = new Guid(0x4c84e981, 0xcc4e, 0x41e9, 0x95, 0x5b, 0xd4, 0x34, 0xa1, 0x3d, 0xc0, 0x7d);
			public static readonly Guid WalkEffect = new Guid(0x280f67c3, 0x6446, 0x4b53, 0xa0, 0x93, 0xdc, 0xfd, 0xd2, 0xff, 0x0d, 0x77);
			public static readonly Guid Behavior = new Guid(0x74f93026, 0x025b, 0x490e, 0xa6, 0xd9, 0xb5, 0xd8, 0xcc, 0xc1, 0x5d, 0x8b);
		}

		public static class Walks
		{
			public static readonly Guid Default = Guid.Empty;
			public static readonly Guid Grass = new Guid(0x9181e583, 0x2522, 0x4d60, 0xae, 0xc9, 0xf8, 0xe4, 0xe0, 0x82, 0xab, 0x11);
			public static readonly Guid Wood = new Guid(0x867c248a, 0xb753, 0x46a7, 0x99, 0x33, 0xf7, 0x8c, 0xb1, 0x5c, 0x27, 0xbd);
			public static readonly Guid LowWater = new Guid(0x41a1afe8, 0x7d86, 0x47a9, 0x9f, 0xbe, 0x30, 0x85, 0x9d, 0xfa, 0x0a, 0x33);
			public static readonly Guid Snow = new Guid(0x121a2020, 0xe4b3, 0x44a6, 0x83, 0x7c, 0x10, 0x76, 0x85, 0x18, 0x8b, 0xc5);
			public static readonly Guid Ice = new Guid(0xa9c286f1, 0x6097, 0x4645, 0xbe, 0xc9, 0x86, 0xe7, 0x22, 0x18, 0x3f, 0x33);
		}

		public static class Behaviors
		{
			public static readonly Guid None = Guid.Empty;
			public static readonly Guid Damage005 = new Guid(0x12b45365, 0x8d60, 0x4a7e, 0xbc, 0x07, 0xe1, 0x45, 0x5c, 0xcf, 0x1f, 0x2c);
			public static readonly Guid Damage010 = new Guid(0x2742ff15, 0xba99, 0x45eb, 0xbb, 0x0b, 0xb0, 0xb2, 0x8d, 0xcd, 0xb4, 0xd3);
			public static readonly Guid HurtNoDamage = new Guid(0xa4389de9, 0xcda3, 0x497d, 0xae, 0x67, 0x7d, 0x66, 0x2e, 0x26, 0x2b, 0xaf);
			public static readonly Guid Slowdown = new Guid(0xbdee0b9e, 0x8215, 0x4333, 0x95, 0x0a, 0xfd, 0xba, 0xdf, 0x26, 0x9e, 0xb3);
			public static readonly Guid Skid = new Guid(0xfa5ad3cf, 0x4381, 0x49d7, 0x97, 0x61, 0x29, 0x1e, 0x71, 0xa3, 0x15, 0x62);
		}

		public static Dictionary<Guid, byte> EffectsDictionary = new Dictionary<Guid, byte>()
		{
			{ Effects.None, 0x00 },
			{ Effects.Solid, 0x01 },
			{ Effects.SolidShaped, 0x02 },
			{ Effects.Climb, 0x03 },
			{ Effects.WalkEffect, 0x04 },
			{ Effects.Behavior, 0x05 },
			{ Effects.LayerChangeAbsolute, 0x10 },
			{ Effects.LayerChangeRelative, 0x11 },
			{ Effects.DepthChangeAbsolute, 0x12 },
			{ Effects.DepthChangeRelative, 0x13 },
			{ Effects.LayerChangeAbsoluteDepthLS, 0x20 },
			{ Effects.LayerChangeAbsoluteDepthLE, 0x21 },
			{ Effects.LayerChangeAbsoluteDepthGT, 0x22 },
			{ Effects.LayerChangeAbsoluteDepthGE, 0x23 },
			{ Effects.LayerChangeRelativeDepthLS, 0x24 },
			{ Effects.LayerChangeRelativeDepthLE, 0x25 },
			{ Effects.LayerChangeRelativeDepthGT, 0x26 },
			{ Effects.LayerChangeRelativeDepthGE, 0x27 },
			{ Effects.LayerChangeAbsoluteDepthReach, 0x28 },
			{ Effects.LayerChangeRelativeDepthReach, 0x29 },
		};

		public static Dictionary<Guid, byte> WalksDictionary = new Dictionary<Guid, byte>()
		{
			{ Walks.Default, 0x00 },
			{ Walks.Grass, 0x01 },
			{ Walks.Wood, 0x02 },
			{ Walks.LowWater, 0x03 },
			{ Walks.Snow, 0x04 },
			{ Walks.Ice, 0x05 },
		};

		public static Dictionary<Guid, byte> BehaviorsDictionary = new Dictionary<Guid, byte>()
		{
			{ Behaviors.None, 0x00 },
			{ Behaviors.Slowdown, 0x01 },
			{ Behaviors.Skid, 0x02 },
			{ Behaviors.HurtNoDamage, 0x10 },
			{ Behaviors.Damage005, 0x11 },
			{ Behaviors.Damage010, 0x12 },
		};
	}
}
