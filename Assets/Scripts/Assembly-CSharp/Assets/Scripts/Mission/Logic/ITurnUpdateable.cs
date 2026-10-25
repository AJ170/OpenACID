namespace Assets.Scripts.Mission.Logic
{
	public interface ITurnUpdateable : IUpdateable
	{
		int BucketTurnUpdateIntervalTurns { get; }

		int TurnUpdateIntervalTurns { get; }

		int TurnUpdateOffsetTurns { get; }

		int NextTurnUpdateTurn { get; }

		int LastTurnUpdateTurn { get; }

		int TurnsSinceTurnUpdate { get; }

		float TimeSinceTurnUpdate { get; }

		void ScheduleTurnUpdate(bool _resetLastTurnUpdateTurn);

		void DoTurnUpdate(int _deltaTurns);
	}
}
