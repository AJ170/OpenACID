using System;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class GiveHpByChanceEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly Func<T, IBuffedStatsCollectionHandle, float> m_ChanceGetter;

		private readonly Func<T, IBuffedStatsCollectionHandle, float> m_HpBonusGetter;

		public GiveHpByChanceEffect(IRule _owner, ETargetSource _targetSource, Func<T, IBuffedStatsCollectionHandle, float> _chanceGetter, Func<T, IBuffedStatsCollectionHandle, float> _hpBonusGetter)
			: base(EEffectType.GiveHpByChance, _owner, _targetSource)
		{
			m_ChanceGetter = _chanceGetter;
			m_HpBonusGetter = _hpBonusGetter;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			float chance = m_ChanceGetter(_entity, _stats);
			if (CombatHelper.DiceRoll(chance))
			{
				float healedDmg = m_HpBonusGetter(_entity, _stats);
				Character entity = TargetEntityId.GetEntity<Character>();
				CombatHelper.ApplyGenericHeal(null, entity, EApplyDamageFlags.None, healedDmg);
			}
			return null;
		}
	}
}
