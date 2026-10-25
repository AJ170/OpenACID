using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class EffectsWrapper : ICloneable
	{
		protected readonly Predicate<IEffect, IRulesCarrier> m_EffectsPredicate;

		protected readonly BBList<IEffect> m_Effects;

		public IRulesCarrier Entity { get; set; }

		public BuffedReadonlyStatValueCollection StatValueCollection { get; set; }

		public EffectsWrapper(BBList<IEffect> _effects)
			: this(_effects, RulesHelper.CategoryMatchingEffectPredicate)
		{
		}

		public EffectsWrapper(BBList<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _predicate)
			: this(_effects, _predicate, null, null)
		{
		}

		protected EffectsWrapper(BBList<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _predicate, IRulesCarrier _entity, BuffedReadonlyStatValueCollection _statValueCollection)
		{
			Entity = _entity;
			StatValueCollection = _statValueCollection;
			m_Effects = _effects;
			m_EffectsPredicate = _predicate;
		}

		public void ApplyEffects(BBList<IEffectInstance> _effectInstances = null)
		{
			using (IBuffedStatsCollectionHandle stats = StatValueCollection.Apply(Entity as ISuperBuffsStatsCarrier))
			{
				IEffectInstance effectInstance = null;
				for (int i = 0; i < m_Effects.Count; i++)
				{
					m_Effects[i].Reset();
					PrepareEffect(m_Effects[i]);
					if ((m_EffectsPredicate == null || m_EffectsPredicate(m_Effects[i], Entity)) && (m_Effects[i].Predicate == null || m_Effects[i].Predicate(m_Effects[i], Entity)))
					{
						effectInstance = m_Effects[i].Apply(Entity, stats);
						if (_effectInstances != null && effectInstance != null)
						{
							_effectInstances.Add(effectInstance);
						}
					}
				}
			}
		}

		protected virtual void PrepareEffect(IEffect _effect)
		{
			_effect.Prepare(Entity);
		}

		public virtual object Clone()
		{
			return new EffectsWrapper(m_Effects, m_EffectsPredicate, Entity, StatValueCollection);
		}
	}
}
