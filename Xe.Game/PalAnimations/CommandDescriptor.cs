using System;
using System.Collections.Generic;
using Xe.Graphics;

namespace Xe.Game.PalAnimations
{
	public class CommandDescriptor
    {
		public static IDictionary<CommandType, CommandDescriptor> Commands { get; set; }

		static CommandDescriptor()
		{
			Commands = new Dictionary<CommandType, CommandDescriptor>
			{
				[CommandType.None] = new CommandDescriptor(
					"A command that does nothing :)",
					null),

				[CommandType.CopyColor] = new CommandDescriptor(
					"Copy a color from a location to another",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Source index",
							Minimum = 0,
							Maximum = 255
						},
					}),

				[CommandType.SwapColor] = new CommandDescriptor(
					"Swap two colors",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Source index",
							Minimum = 0,
							Maximum = 255
						},
					}),

				[CommandType.SetColor] = new CommandDescriptor(
					"Set a specific color",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Red channel",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Blue channel",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Green channel",
							Minimum = 0,
							Maximum = 255,
						},
					}),

				[CommandType.AddColor] = new CommandDescriptor(
					"Add color",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Red channel",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Blue channel",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Green channel",
							Minimum = 0,
							Maximum = 255,
						},
					}),

				[CommandType.MultiplyColor] = new CommandDescriptor(
					"Multiply a specific color",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Red channel",
							Minimum = 0,
							Maximum = 256.0f,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Blue channel",
							Minimum = 0,
							Maximum = 256.0f,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Green channel",
							Minimum = 0,
							Maximum = 256.0f,
						},
					}),

				[CommandType.SetHue] = new CommandDescriptor(
					"Set a specific Hue",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Hue degree",
							Minimum = -720.0f,
							Maximum = +720.0f,
						},
					}),

				[CommandType.AddHue] = new CommandDescriptor(
					"Add some Hue",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Hue degree",
							Minimum = -720.0f,
							Maximum = +720.0f,
						},
					}),

				[CommandType.SetBrightness] = new CommandDescriptor(
					"Set the brightness",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Brightness",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.AddBrightness] = new CommandDescriptor(
					"Add the brightness",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Brightness",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.MultiplyBrightness] = new CommandDescriptor(
					"Multiply the brightness",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Brightness",
							Minimum = 0.0f,
							Maximum = 256.0f,
						},
					}),

				[CommandType.SetSaturation] = new CommandDescriptor(
					"Set the saturation",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Saturation",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.AddSaturation] = new CommandDescriptor(
					"Add the saturation",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Saturation",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.MultiplySaturation] = new CommandDescriptor(
					"Multiply the saturation",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Saturation",
							Minimum = 0.0f,
							Maximum = 256.0f,
						},
					}),

				[CommandType.SetAlpha] = new CommandDescriptor(
					"Set the opacity",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Alpha",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.AddAlpha] = new CommandDescriptor(
					"Add the opacity",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Alpha",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.MultiplyAlpha] = new CommandDescriptor(
					"Multiply the opacity",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Alpha",
							Minimum = 0.0f,
							Maximum = 256.0f,
						},
					}),

				[CommandType.Invert] = new CommandDescriptor(
					"Invert the color",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Inversion heaviness",
							Minimum = 0.0f,
							Maximum = 1.0f,
						},
					}),

				[CommandType.RotateHue] = new CommandDescriptor(
					"Rotate the hue of the color",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Destination index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(float),
							Name = "Hue movement per second",
							Minimum = -720.0f,
							Maximum = +720.0f,
						},
					}),

				[CommandType.RotateRight] = new CommandDescriptor(
					"Rotate a group of colors to the right",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Start index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Number of colors to rotate",
							Minimum = 2,
							Maximum = 64,
						},
					}),

				[CommandType.RotateLeft] = new CommandDescriptor(
					"Rotate a group of colors to the left",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Start index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Number of colors to rotate",
							Minimum = 2,
							Maximum = 64,
						},
					}),

				[CommandType.RotateLeft] = new CommandDescriptor(
					"Rotate a group of colors to the left",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Start index",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Number of colors to rotate",
							Minimum = 2,
							Maximum = 64,
						},
					}),

				[CommandType.LoadClut] = new CommandDescriptor(
					"Replace the current Clut with a new one",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte[]),
							Name = "Data",
						},
					}),

				[CommandType.ApplyClut] = new CommandDescriptor(
					"Apply the Clut filtering by a key color",
					new CommandParameter[]
					{
						new CommandParameter()
						{
							Type = typeof(byte[]),
							Name = "Data",
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Red channel of key color",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Blue channel of key color",
							Minimum = 0,
							Maximum = 255,
						},
						new CommandParameter()
						{
							Type = typeof(byte),
							Name = "Green channel of key color",
							Minimum = 0,
							Maximum = 255,
						},
					}),
			};
		}

		public CommandDescriptor(string description, IEnumerable<CommandParameter> parameters)
		{
			Parameters = parameters;
			Description = description;
		}

		public IEnumerable<CommandParameter> Parameters { get; }

		public string Description { get; set; }
	}
}

