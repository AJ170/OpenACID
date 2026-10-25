using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class DoubleIdentityRule : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		private static readonly Enum<SkillTypes> SkillType = "Double_Identity_01".ToEnum<SkillTypes>();

		public DoubleIdentityRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			AddEffect(new ActionEffect<PlayerCharacter>(this, ETargetSource.RuleOwner, OnAction));
		}

		private static IEffectInstance OnAction(PlayerCharacter _player, IBuffedStatsCollectionHandle _stats, ActionEffect<PlayerCharacter> _effect, EntityId _targetId)
		{
			_player.PlayerStats.AddListener(delegate(IReadonlyStatValue _value, IStatValueWrapper _wrapper)
			{
				OnStatChanged(_value, _wrapper, _targetId);
			});
			return null;
		}

		private static void OnStatChanged(IReadonlyStatValue _statValue, IStatValueWrapper _previousValue, EntityId _entityId)
		{
			if (_statValue.StatType != StatTypes.EStatType.Character_Conspicuous_Notoriety)
			{
				return;
			}
			float @float = _previousValue.Float;
			if (@float < 1f)
			{
				return;
			}
			float num = _statValue.Value_float;
			if (num >= 1f)
			{
				return;
			}
			PlayerCharacter playerCharacter = _entityId.TryGetEntity<PlayerCharacter>();
			if (playerCharacter == null || playerCharacter.PlayerStats.Get_bool(StatTypes.EStatType.Player_DoubleIdentityActive))
			{
				return;
			}
			using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = playerCharacter.Skills.GetBuffedStatsCollectionHandle(SkillType))
			{
				float num2 = Mathf.Clamp01(1f - buffedStatsCollectionHandle.Get_float(StatTypes.EStatType.Rule_NotorietyDecrease));
				float num3 = Mathf.Clamp01(num * num2);
				if (!(num3 >= num))
				{
					playerCharacter.PlayerCharacterStateMachines.PlayerAlertLevel.ForceNotoriety(num3);
					int turnDuration = buffedStatsCollectionHandle.Get_int(StatTypes.EStatType.Rule_SkillCooldownDuration);
					playerCharacter.Buffs.AddBuff(new BooleanOrBuff(EBuffType.DoubleIdentity, EBuffDurationType.Finite, turnDuration, 0, true));
				}
			}
		}
	}
}
