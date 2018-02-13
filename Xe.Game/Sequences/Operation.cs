using System;
using System.Collections.Generic;
using System.Text;

namespace Xe.Game.Sequences
{
	internal static class Config
	{
		public const int BgmLayersCount = 2;
	}

	public enum Operation
	{
		None = 0,

		#region Commands
		
		[Description("Abort script.")]
		Abort = 0x30,

		[Description("Pause the sequence for a specified lapse.")]
		[Parameter(1.0f, 0.0f, 100.0f, "Timer of sleep in seconds")]
		Sleep = 0x31,

		[Description("Modify the speed of the sequence.")]
		[Parameter(1.0f, 0.0f, 10.0f, "Speed multiplier")]
		SetTimeMuliplier = 0x32,

		[Description("Remove all asynchronous operations.")]
		RemoveAsynchronousOperations = 0x33,

		[Description("Interrupt the current sequence and play a new one.")]
		[Parameter((string)null, "Sequence name")]
		PlaySequence = 0x38,

		[Description("Change map. Does remove any asynchronous operations too.")]
		[Parameter(0, 0, 255, "Zone")]
		[Parameter(0, 0, 255, "Map")]
		[Parameter(0, 0, 255, "Door")]
		ChangeMap = 0x39,

		[Description("Activate gamepad rumbling.")]
		[Parameter(0.0f, 0.0f, 1.0f, "Low-frequency rumble strength")]
		[Parameter(0.0f, 0.0f, 1.0f, "High-frequency rumble strength")]
		GamepadRumble = 0x3E,

		// TODO
		[Description("Change gamepad color.")]
		GamepadColor = 0x3F,

		[Description("Return from a black screen.")]
		[Parameter(1.0f, 0.0f, 10.0f, "Timer of fade in seconds")]
		FadeInBlack = 0x40,

		[Description("Timer of fade in seconds.")]
		[Parameter(1.0f, 0.0f, 10.0f, "Timer of fade in seconds")]
		FadeInWhite = 0x41,

		[Description("Timer of fade in seconds.")]
		[Parameter(1.0f, 0.0f, 10.0f, "Timer of fade in seconds")]
		FadeOutBlack = 0x42,

		[Description("Timer of fade in seconds.")]
		[Parameter(1.0f, 0.0f, 10.0f, "Timer of fade in seconds")]
		FadeOutWhite = 0x43,
		
		[Description("Decide to lock or not the camera on the current position.")]
		[Parameter(true, "Lock camera")]
		CameraLock = 0x44,

		[Description("Shake the camera.")]
		[Parameter(2.0, -16.0, +16.0, "Horizontal shake strength")]
		[Parameter(2.0, -16.0, +16.0, "Vertical shake strength")]
		[Parameter(1.0f, 0.0f, 100.0f, "Timer")]
		CameraShake = 0x45,

		[Description("Set the camera to the specified position.")]
		[Parameter(0, -32768, +32767, "Destination X")]
		[Parameter(0, -32768, +32767, "Destination Y")]
		CameraSet = 0x46,

		[Description("Move the camera to a specified position.")]
		[Parameter(0, -32768, +32767, "Destination X")]
		[Parameter(0, -32768, +32767, "Destination Y")]
		[Parameter(0.0f, 0.0f, +256.0f, "Movement speed")]
		CameraMove = 0x47,

		[Description("Play a BGM")]
		[Parameter(0, 0, Config.BgmLayersCount, "Layer index")]
		[Parameter(1.0f, 0.0f, 10.0f, "Fade in")]
		[Parameter((string)null, "Name of BGM file")]
		PlayBgm = 0x48,

		[Description("Stop the current BGM.")]
		[Parameter(0, 0, Config.BgmLayersCount, "Layer index")]
		[Parameter(1.0f, 0.0f, 10.0f, "Fade out")]
		StopBgm = 0x49,

		[Description("Pause the current BGM and maintain the position.")]
		[Parameter(0, 0, Config.BgmLayersCount, "Layer index")]
		[Parameter(1.0f, 0.0f, 10.0f, "Fade out")]
		PauseBgm = 0x4A,

		[Description("Change the volume of the specified BGM layer.")]
		[Parameter(0, 0, Config.BgmLayersCount, "Layer index")]
		[Parameter(1.0f, 0.0f, 1.0f, "Volume")]
		SetBgmVolume = 0x4B,

		[Description("Play a sound effect.")]
		[Parameter(1.0f, 0.0f, 1.0f, "Volume left")]
		[Parameter(1.0f, 0.0f, 1.0f, "Volume right")]
		PlaySe = 0x4C,

		[Description("Stop all the executing sound effects.")]
		StopAllSe = 0x4D,

		[Description("Decide to lock or not the current player input.")]
		[Parameter(true, "Lock input")]
		PlayerInputLock = 0x50,

		[Description("Flag battle")]
		[Parameter(false, "Is in battle")]
		BattleFlag = 0x51,

		// TODO
		[Description("Add the specified item into the inventory.")]
		AddItem = 0x58,

		// TODO
		[Description("Add the specified equipment into the inventory.")]
		AddEquip = 0x59,

		// TODO
		[Description("Add the specified skill into the skills list.")]
		AddSkill = 0x5A,

		// TODO
		[Description("Add the specified ability into the abilities list.")]
		AddAbility = 0x5B,

		#endregion

		#region Actions

		// TODO
		[Description("Follow the specified entity.")]
		CameraFollow = 0xA0,

		// TODO
		[Description("Change the position of the specified entity.")]
		EntityPosition = 0xA1,
		
		[Description("Move the specified entity.")]
		[Parameter((string)null, "Name of the entity (optional)")]
		[Parameter((string)null, "Group's name of the entity (optional)")]
		[Parameter(0, -32768, +32767, "Destination X")]
		[Parameter(0, -32768, +32767, "Destination Y")]
		[Parameter(0.0f, 0.0f, +256.0f, "Movement speed")]
		EntityMove = 0xA2,

		// TODO
		[Description("Set an animation for the specified entry.")]
		EntityAnimation = 0xA3,

		// TODO
		[Description("Set the direction for the specified entry.")]
		Entityirection,

		#endregion

		#region Dialog management

		// TODO
		[Description("Set a faceset for the dialog.")]
		DialogFaceset,

		// TODO
		[Description("Change the color of the next text.")]
		TextColor,

		#endregion
	}
}
