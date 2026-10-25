using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class SweetMusicRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		public SweetMusicRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddOverwritingSuperBuff_float(StatTypes.EStatType.Rule_AttractionRange, "Distraction_01".ToEnum<SkillTypes>());
			AddOverwritingSuperBuff_int(StatTypes.EStatType.Rule_SweetMusicDuration);
			AddOverwritingSuperBuff_int(StatTypes.EStatType.Rule_SweetMusicHidingSpots);
		}
	}
}
