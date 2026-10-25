namespace Assets.Scripts.Mission.HUD
{
	public static class ScreenAnchorUtils
	{
		public static bool HasFlag(this ScreenAnchor _anchor, Direction _direction)
		{
			return ((uint)_anchor & (uint)_direction) != 0;
		}
	}
}
