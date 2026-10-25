using System;

namespace Assets.Scripts.Mission.HUD
{
	[Flags]
	public enum Direction
	{
		Top = 1,
		Bottom = 2,
		Left = 4,
		Right = 8,
		Middle = 0x10,
		Center = 0x20
	}
}
