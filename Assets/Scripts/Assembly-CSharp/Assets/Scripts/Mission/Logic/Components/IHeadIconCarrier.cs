namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IHeadIconCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		HeadIcon HeadIcon { get; }
	}
}
