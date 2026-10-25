namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ICharacterStatsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		CharacterStats CharacterStats { get; }
	}
}
