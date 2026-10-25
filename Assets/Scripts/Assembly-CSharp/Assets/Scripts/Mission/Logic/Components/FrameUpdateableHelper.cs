using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	internal static class FrameUpdateableHelper
	{
		public static int ComputeTurnsSinceFrameUpdate(this IFrameUpdateable _frameUpdateable)
		{
			return (int)(_frameUpdateable.TimeSinceFrameUpdate / 0.1f);
		}

		public static float ComputeTimeSinceFrameUpdate(this IFrameUpdateable _frameUpdateable)
		{
			return LogicMain.CurrentTime - _frameUpdateable.LastFrameUpdateTime;
		}

		public static float ComputeNextFrameUpdateTime(this IFrameUpdateable _frameUpdateable)
		{
			float frameUpdateIntervalSeconds = _frameUpdateable.FrameUpdateIntervalSeconds;
			float frameUpdateOffsetPercent = _frameUpdateable.FrameUpdateOffsetPercent;
			return (!(frameUpdateIntervalSeconds > 0f)) ? (-1f) : ((Mathf.Floor(_frameUpdateable.LastFrameUpdateTime / frameUpdateIntervalSeconds) + 1f + frameUpdateOffsetPercent) * frameUpdateIntervalSeconds);
		}
	}
}
