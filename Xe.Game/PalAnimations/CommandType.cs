namespace Xe.Game.PalAnimations
{
    public enum CommandType
    {
		None = 0,
		CopyColor = 1,
		SwapColor = 2,

		SetColor = 4,
		AddColor = 5,
		MultiplyColor = 6,

		SetHue = 8,
		AddHue = 9,

		SetBrightness = 12,
		AddBrightness = 13,
		MultiplyBrightness = 14,

		SetSaturation = 16,
		AddSaturation = 17,
		MultiplySaturation = 18,

		SetAlpha = 20,
		AddAlpha = 21,
		MultiplyAlpha = 22,

		Invert = 24,
		//RotateHue = 25,
		RotateRight = 26,
		RotateLeft = 27,
		//LoadClut = 28,
		//ApplyClut = 29,
	}
}
