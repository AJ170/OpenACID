namespace Assets.Scripts.Mission.Logic.Components
{
	public interface INpcDefinitionCarrier : IDefinitionCarrier, ICharacterDefinitionCarrier
	{
		NpcDefinition NpcDefinition { get; }
	}
}
