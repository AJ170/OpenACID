using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ISkillRule : IRule
	{
		Enum<SkillTypes> SkillType { get; }

		SkillData.ISkillDefinition SkillDefinition { get; }

		ETriggerCategoryFlags SkillCategories { get; }

		ETriggerFrequency SkillFrequency { get; }

		TPayload GetSkillPayload<TPayload>(ISkillsCarrier _entity) where TPayload : class, ISkillPayload;
	}
}
