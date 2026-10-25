using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class AssassinationLeapRule : AssassinationNoParkourRuleBase
	{
		public AssassinationLeapRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters, EStateType.PlayerMain_AssassinationLeap)
		{
			AddEffect(new AddCrowdHotspotSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, 0f, 20f, 15f, true, 0f, 0f, ReactionType.Invalid));
		}
	}
}
