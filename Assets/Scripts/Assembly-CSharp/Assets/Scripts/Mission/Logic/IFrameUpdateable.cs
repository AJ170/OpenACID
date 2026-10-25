namespace Assets.Scripts.Mission.Logic
{
	public interface IFrameUpdateable : IUpdateable
	{
		float FrameUpdateIntervalSeconds { get; }

		float FrameUpdateOffsetPercent { get; }

		float NextFrameUpdateTime { get; }

		float LastFrameUpdateTime { get; }

		int TurnsSinceFrameUpdate { get; }

		float TimeSinceFrameUpdate { get; }

		void ScheduleFrameUpdate(bool _resetLastFrameUpdateTime);

		void DoFrameUpdate(float _deltaTime);
	}
}
