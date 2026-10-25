namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IScalableStat : IReadonlyStatValueBase, IStatValueBase
	{
		float LevelIncrement { get; set; }
	}
}
