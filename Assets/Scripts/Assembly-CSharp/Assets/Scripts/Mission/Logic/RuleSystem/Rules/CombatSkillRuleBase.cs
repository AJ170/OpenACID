using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class CombatSkillRuleBase : SkillRuleBase<PlayerCharacter>, ICombatAnimationControllingRule
	{
		public abstract SkillActBehaviour.IParams SkillParams { get; }

		protected CombatSkillRuleBase(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
		}
	}
}
