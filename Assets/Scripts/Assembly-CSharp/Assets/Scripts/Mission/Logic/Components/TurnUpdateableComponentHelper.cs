namespace Assets.Scripts.Mission.Logic.Components
{
	internal static class TurnUpdateableComponentHelper
	{
		public static int ComputeBucketTurnUpdateIntervalTurns(this ITurnUpdateableComponent _turnUpdateable)
		{
			return (int)((IGameEntity)_turnUpdateable.Entity).DistanceBucket * 20;
		}
	}
}
