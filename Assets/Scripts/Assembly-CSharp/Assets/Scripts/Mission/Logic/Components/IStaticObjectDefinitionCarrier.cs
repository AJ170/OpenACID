namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IStaticObjectDefinitionCarrier : IDefinitionCarrier
	{
		StaticObjectDefinition StaticObjectDefinition { get; }
	}
}
