namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IAvatarDefinitionCarrier : IDefinitionCarrier, ICharacterDefinitionCarrier
	{
		AvatarDefinition AvatarDefinition { get; }
	}
}
