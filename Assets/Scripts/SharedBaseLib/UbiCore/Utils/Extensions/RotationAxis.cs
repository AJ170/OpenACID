using System;

namespace UbiCore.Utils.Extensions
{
	[Flags]
	public enum RotationAxis
	{
		None = 0,
		XAxis = 2,
		YAxis = 4,
		ZAxis = 8,
		All = 0xE
	}
}
