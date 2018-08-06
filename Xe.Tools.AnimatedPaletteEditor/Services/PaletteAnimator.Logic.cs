using System;
using System.Collections.Generic;
using System.Linq;
using Xe.Game;
using Xe.Game.PalAnimations;
using Xe.Graphics;

namespace Xe.Tools.Components.AnimatedPaletteEditor.Services
{
    public partial class PaletteAnimator
    {
		private class CommandEntry
		{
			public Action<PaletteAnimator, PalCommand, double, object[]> Execute { get; set; }

			public Func<object[], byte[]> Burn { get; set; }
		}

		private Dictionary<CommandType, CommandEntry> entries = new Dictionary<CommandType, CommandEntry>()
		{
			[CommandType.None] = new CommandEntry()
			{
				Execute = (s, c, t, p) => { },
				Burn = p => null
			},

			[CommandType.CopyColor] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var src = Get<byte>(p[1]);
					s.palette[dst * 4 + 0] = Math.Lerp(s.backbuffer[dst * 4 + 0], s.backbuffer[src * 4 + 0], t);
					s.palette[dst * 4 + 1] = Math.Lerp(s.backbuffer[dst * 4 + 1], s.backbuffer[src * 4 + 1], t);
					s.palette[dst * 4 + 2] = Math.Lerp(s.backbuffer[dst * 4 + 2], s.backbuffer[src * 4 + 2], t);
					s.palette[dst * 4 + 3] = Math.Lerp(s.backbuffer[dst * 4 + 3], s.backbuffer[src * 4 + 3], t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]), Get<byte>(p[1]) }
			},

			[CommandType.SwapColor] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var src = Get<byte>(p[1]);
					s.palette[dst * 4 + 0] = Math.Lerp(s.backbuffer[dst * 4 + 0], s.backbuffer[src * 4 + 0], t);
					s.palette[dst * 4 + 1] = Math.Lerp(s.backbuffer[dst * 4 + 1], s.backbuffer[src * 4 + 1], t);
					s.palette[dst * 4 + 2] = Math.Lerp(s.backbuffer[dst * 4 + 2], s.backbuffer[src * 4 + 2], t);
					s.palette[dst * 4 + 3] = Math.Lerp(s.backbuffer[dst * 4 + 3], s.backbuffer[src * 4 + 3], t);
					s.palette[src * 4 + 0] = Math.Lerp(s.backbuffer[src * 4 + 0], s.backbuffer[dst * 4 + 0], t);
					s.palette[src * 4 + 1] = Math.Lerp(s.backbuffer[src * 4 + 1], s.backbuffer[dst * 4 + 1], t);
					s.palette[src * 4 + 2] = Math.Lerp(s.backbuffer[src * 4 + 2], s.backbuffer[dst * 4 + 2], t);
					s.palette[src * 4 + 3] = Math.Lerp(s.backbuffer[src * 4 + 3], s.backbuffer[dst * 4 + 3], t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]), Get<byte>(p[1]) }
			},

			[CommandType.SetColor] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var r = Get<byte>(p[1]);
					var g = Get<byte>(p[2]);
					var b = Get<byte>(p[3]);
					s.palette[dst * 4 + 0] = Math.Lerp(s.backbuffer[dst * 4 + 0], r, t);
					s.palette[dst * 4 + 1] = Math.Lerp(s.backbuffer[dst * 4 + 1], g, t);
					s.palette[dst * 4 + 2] = Math.Lerp(s.backbuffer[dst * 4 + 2], b, t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.AddColor] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var r = Get<byte>(p[1]);
					var g = Get<byte>(p[2]);
					var b = Get<byte>(p[3]);
					s.palette[dst * 4 + 0] = Math.Lerp(s.backbuffer[dst * 4 + 0], (byte)Math.Min(s.backbuffer[dst * 4 + 0] + r, byte.MaxValue), t);
					s.palette[dst * 4 + 1] = Math.Lerp(s.backbuffer[dst * 4 + 1], (byte)Math.Min(s.backbuffer[dst * 4 + 1] + g, byte.MaxValue), t);
					s.palette[dst * 4 + 2] = Math.Lerp(s.backbuffer[dst * 4 + 2], (byte)Math.Min(s.backbuffer[dst * 4 + 2] + b, byte.MaxValue), t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.MultiplyColor] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var r = Get<byte>(p[1]);
					var g = Get<byte>(p[2]);
					var b = Get<byte>(p[3]);
					s.palette[dst * 4 + 0] = Math.Lerp(s.backbuffer[dst * 4 + 0], (byte)Math.Min(s.backbuffer[dst * 4 + 0] * r, byte.MaxValue), t);
					s.palette[dst * 4 + 1] = Math.Lerp(s.backbuffer[dst * 4 + 1], (byte)Math.Min(s.backbuffer[dst * 4 + 1] * g, byte.MaxValue), t);
					s.palette[dst * 4 + 2] = Math.Lerp(s.backbuffer[dst * 4 + 2], (byte)Math.Min(s.backbuffer[dst * 4 + 2] * b, byte.MaxValue), t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.SetHue] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var hue = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.h = Math.Lerp(hsv.h, hue, t);
					if (hsv.h > 720.0f)
						hsv.v -= 360.0f;
					if (hsv.h < -720.0f)
						hsv.v += 360.0f;
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.AddHue] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var hue = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.h = Math.Lerp(hsv.h, hsv.h + hue, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.SetBrightness] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var brightness = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.v = Math.Lerp(hsv.v, brightness, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.AddBrightness] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var brightness = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.v = Math.Lerp(hsv.v, hsv.v + brightness, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.MultiplyBrightness] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var brightness = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.v = Math.Lerp(hsv.v, hsv.v * brightness, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.SetSaturation] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var saturation = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.s = Math.Lerp(hsv.s, saturation, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.AddSaturation] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var saturation = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.s = Math.Lerp(hsv.s, hsv.s + saturation, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.MultiplySaturation] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var saturation = Get<float>(p[1]);

					var src = new Color(s.backbuffer[dst * 4 + 0], s.backbuffer[dst * 4 + 1], s.backbuffer[dst * 4 + 2], s.backbuffer[dst * 4 + 3]);
					var hsv = new ColorHsv(new ColorF(src));
					hsv.s = Math.Lerp(hsv.s, hsv.s * saturation, t);
					var final = hsv.ToColorF().ToColor();

					s.palette[dst * 4 + 0] = final.r;
					s.palette[dst * 4 + 1] = final.g;
					s.palette[dst * 4 + 2] = final.b;
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.SetAlpha] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var alpha = Get<float>(p[1]);
					s.palette[dst * 4 + 3] = Math.Lerp(s.backbuffer[dst * 4 + 3], (byte)(alpha * 255.0f), t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.AddAlpha] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var alpha = Get<float>(p[1]);
					s.palette[dst * 4 + 3] = Math.Lerp(s.backbuffer[dst * 4 + 3], (byte)Math.Min(s.backbuffer[dst * 4 + 3] + (byte)(alpha * 255.0f), byte.MaxValue), t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.MultiplyAlpha] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var alpha = Get<float>(p[1]);
					s.palette[dst * 4 + 3] = Math.Lerp(s.backbuffer[dst * 4 + 3], (byte)Math.Min(s.backbuffer[dst * 4 + 3] * (byte)(alpha * 255.0f), byte.MaxValue), t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.Invert] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var dst = Get<byte>(p[0]);
					var heaviness = Get<float>(p[1]);
					s.palette[dst * 4 + 0] = Math.Lerp(s.backbuffer[dst * 4 + 0], (byte)((255 - s.backbuffer[dst * 4 + 0]) * heaviness), t);
					s.palette[dst * 4 + 1] = Math.Lerp(s.backbuffer[dst * 4 + 1], (byte)((255 - s.backbuffer[dst * 4 + 1]) * heaviness), t);
					s.palette[dst * 4 + 2] = Math.Lerp(s.backbuffer[dst * 4 + 2], (byte)((255 - s.backbuffer[dst * 4 + 2]) * heaviness), t);
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.RotateRight] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var index = Get<byte>(p[0]);
					var count = Get<byte>(p[1]);

					var length = c.End - c.Start;
					var dstIndex = Math.Floor(t * count);
					t = 1.0;
					for (int i = 0; i < count; i++)
					{
						UtilCopyColor(s.palette, index + count - i - 1, s.backbuffer, index + (dstIndex + i + 1) % count, index + (dstIndex + i) % count, t);
					}
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},

			[CommandType.RotateLeft] = new CommandEntry()
			{
				Execute = (s, c, t, p) =>
				{
					var index = Get<byte>(p[0]);
					var count = Get<byte>(p[1]);

					var length = c.End - c.Start;
					var dstIndex = Math.Floor(t * count);
					for (int i = 0; i < count; i++)
					{
						UtilCopyColor(s.palette, index + i, s.backbuffer, index + (dstIndex + i + 1) % count, index + (dstIndex + i) % count, t);
					}
				},
				Burn = p => new byte[] { Get<byte>(p[0]) }
			},
		};

		public double Timer { get; private set; }

		public PalAction Action { get; private set; }

		public void PlayAction(PalAction action)
		{
			Timer = 0.0f;
			Action = new PalAction()
			{
				Name = "dummy",
				Commands = action?.Commands
			};
		}

		public void PlayAction(IEnumerable<PalCommand> commands)
		{
			Timer = 0.0f;
			Action = new PalAction()
			{
				Name = "dummy",
				Commands = commands.ToList()
			};
		}

		public void PlayCommand(PalCommand command)
		{
			PlayAction(new List<PalCommand>()
				{
					command
				}
			);
		}

		public void Update(double deltaTime)
		{
			var oldDeltaTime = Timer;
			Timer += deltaTime;
			if (Action?.Commands == null)
				return;

			foreach (var command in Action.Commands)
			{
				Execute(command, oldDeltaTime, Timer, command.Loop);
			}
		}

		private void Execute(PalCommand command, double oldDeltaTime, double newDeltaTime, double loop)
		{
			bool doesLoop = loop < 10000.0; // arbitrary number
			if (doesLoop && newDeltaTime > command.End)
			{
				oldDeltaTime = ((oldDeltaTime - loop) % (command.End - loop)) + loop;
				newDeltaTime = ((newDeltaTime - loop) % (command.End - loop)) + loop;
			}

			if (command.Start <= newDeltaTime || (command.End < newDeltaTime && command.End >= oldDeltaTime))
			{
				double t;
				bool hasFinished = command.End < newDeltaTime;

				if (hasFinished == false)
				{
					t = (newDeltaTime - command.Start) * 1.0 / (command.End - command.Start);
					t = EaseUtility.Calculate(command.Ease, t);
				}
				else
				{
					t = 1.0;
				}

				if (command.InvertedTimer)
				{
					t = 1.0 - t;
				}

				var parameters = command.Parameters.ToArray();
				entries[command.Command].Execute(this, command, t, parameters);
				if (doesLoop == false && hasFinished == true)
				{
					var toBurn = entries[command.Command].Burn(parameters);
					if (toBurn != null)
					{
						for (int i = 0; i < toBurn.Length; i++)
						{
							backbuffer[i] = palette[i];
						}
					}
				}
			}
		}

		private static T Get<T>(object o)
		{
			var t = typeof(T);
			return (T)(o.GetType() == t ? o : Convert.ChangeType(o, t));
		}

		private static void UtilCopyColor(byte[] dst, int dstIndex, byte[] src, int aIndex, int bIndex, double t)
		{
			dst[dstIndex * 4 + 0] = Math.Lerp(src[aIndex * 4 + 0], src[bIndex * 4 + 0], t);
			dst[dstIndex * 4 + 1] = Math.Lerp(src[aIndex * 4 + 1], src[bIndex * 4 + 1], t);
			dst[dstIndex * 4 + 2] = Math.Lerp(src[aIndex * 4 + 2], src[bIndex * 4 + 2], t);
			dst[dstIndex * 4 + 3] = Math.Lerp(src[aIndex * 4 + 3], src[bIndex * 4 + 3], t);
		}
	}
}
