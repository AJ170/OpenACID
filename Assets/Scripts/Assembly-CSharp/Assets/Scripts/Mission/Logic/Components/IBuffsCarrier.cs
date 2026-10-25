namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IBuffsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		Buffs Buffs { get; }
	}
}
