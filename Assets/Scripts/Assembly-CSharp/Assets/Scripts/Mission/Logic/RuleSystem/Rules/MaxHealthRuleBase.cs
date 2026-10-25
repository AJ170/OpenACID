using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class MaxHealthRuleBase : PassiveStatModifierRuleBase<PlayerCharacter>
	{
		private class EffectInstance : EffectInstanceBase
		{
			private PlayerCharacter m_Entity;

			public EffectInstance(IEffect _effect, PlayerCharacter _entity)
				: base(_effect)
			{
				m_Entity = _entity;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				return true;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_Entity.EntityStats.UpdateComputedStatsExplicit();
				m_Entity.EntityStats.Copy(StatTypes.EStatType.Character_Conflict_Health, StatTypes.EStatType.Character_Conflict_Health);
			}
		}

		protected readonly CustomBuffGenerator<float> m_Generator;

		protected MaxHealthRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			m_Generator = AddMaxHealthEffect();
			AddEffect(new ActionEffect<PlayerCharacter>(this, ETargetSource.RuleOwner, Action));
		}

		protected abstract CustomBuffGenerator<float> AddMaxHealthEffect();

		private IEffectInstance Action(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats, ActionEffect<PlayerCharacter> _effect, EntityId _targetId)
		{
			_entity.EntityStats.UpdateComputedStatsExplicit();
			_entity.EntityStats.Copy(StatTypes.EStatType.Character_Conflict_ActualMaxHealth, StatTypes.EStatType.Character_Conflict_Health);
			return new EffectInstance(_effect, _entity);
		}
	}
}
