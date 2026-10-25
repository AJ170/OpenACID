using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class IntimidationRule : SkillRuleBase<PlayerCharacter>
	{
		public ISkillEffect StunEffect { get; private set; }

		public IntimidationRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			StunEffect = AddEffect(new BuffSkillEffect<PlayerCharacter, IEntityIdSkillPayload>(this, ETargetSource.Custom, BuffHelpers.MakeStunningBuffGenerator(StatValueCollection)));
		}
	}
}
