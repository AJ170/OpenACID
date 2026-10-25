namespace Assets.Scripts.Mission.Logic.Components
{
	internal static class TurnUpdateableHelper
	{
		public static int ComputeTurnsSinceTurnUpdate(this ITurnUpdateable _turnUpdateable)
		{
			return LogicMain.CurrentTurn - _turnUpdateable.LastTurnUpdateTurn;
		}

		public static float ComputeTimeSinceTurnUpdate(this ITurnUpdateable _turnUpdateable)
		{
			return (float)_turnUpdateable.TurnsSinceTurnUpdate * 0.1f;
		}

		public static int ComputeNextTurnUpdateTurn(this ITurnUpdateable _turnUpdateable)
		{
			int turnUpdateIntervalTurns = _turnUpdateable.TurnUpdateIntervalTurns;
			return (turnUpdateIntervalTurns > 0) ? (_turnUpdateable.LastTurnUpdateTurn + turnUpdateIntervalTurns - _turnUpdateable.LastTurnUpdateTurn % turnUpdateIntervalTurns + _turnUpdateable.TurnUpdateOffsetTurns % turnUpdateIntervalTurns) : 0;
		}
	}
}
