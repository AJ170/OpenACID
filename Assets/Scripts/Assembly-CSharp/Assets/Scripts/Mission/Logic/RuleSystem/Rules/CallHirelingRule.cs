using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class CallHirelingRule : SkillRuleBase<PlayerCharacter>
	{
		public CallHirelingRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddTrigger(new EnemySelectedTrigger(this, SkillFrequency, SkillCategories));
			AddEffect(new AbortSkillEffect(this, "Disguise_01".ToEnum<SkillTypes>()));
			AddEffect(new SpawnHirelingSkillEffect(this));
			AddEffect(new RemoveSkillSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, "Call_Hireling".ToEnum<SkillTypes>()));
			AddEffect(new RemoveSkillsSkillEffect<PlayerCharacter, ISkillPayload>(this, ETargetSource.RuleOwner, SkillsGetter));
		}

		private void SkillsGetter(ISkillsCarrier _skillsCarrier, BBSimpleList<Enum<SkillTypes>> _skillTypes)
		{
			_skillTypes.Clear();
			AvatarData.IAvatarDefinition playerDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetPlayerDefinition();
			IReadonlyStatCollectionValue readonlyStatCollectionValue = playerDefinition.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_Skills);
			for (int i = 0; i < readonlyStatCollectionValue.Count; i++)
			{
				string text = readonlyStatCollectionValue.Get_string(i);
				if (!string.IsNullOrEmpty(text))
				{
					SkillData.ISkillDefinition definition = text.ToEnum<SkillTypes>().GetDefinition();
					if (definition.ActivationType == ESkillActivationType.PassiveHireling)
					{
						_skillTypes.Add(definition.Type);
					}
				}
			}
		}
	}
}
