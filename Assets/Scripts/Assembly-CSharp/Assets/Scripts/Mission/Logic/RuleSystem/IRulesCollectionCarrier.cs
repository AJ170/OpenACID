using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IRulesCollectionCarrier : Data.IDefinition
	{
		RulesCollection RulesCollection { get; }
	}
}
