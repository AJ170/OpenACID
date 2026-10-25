using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class TargetCollectorSkillEffectsWrapper<T, TPayload> : TargetCollectorSkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : IEntityIdSkillPayload
	{
		private readonly MultipleTargetsSkillEffectsWrapper m_Wrapper;

		private static readonly BBList<IEffectInstance> s_WrappedEffectInstances = new BBList<IEffectInstance>();

		public TargetCollectorSkillEffectsWrapper(IRule _owner, ETargetSource _targetSource, TargetCollector _targetCollector, IEnumerable<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _effectPredicate)
			: base(EEffectType.WrappedTargetCollecting, _owner, _targetSource, _targetCollector)
		{
			m_Wrapper = new MultipleTargetsSkillEffectsWrapper(new BBList<IEffect>(_effects), _effectPredicate);
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			WrapperEffectInstance wrapperEffectInstance = new WrapperEffectInstance(this);
			s_WrappedEffectInstances.ClearIfNotEmpty();
			m_Wrapper.TargetEntitiyIds.ClearIfNotEmpty();
			m_Wrapper.TargetEntitiyIds.AddRange(TargetEntityIds);
			m_Wrapper.Entity = _entity;
			m_Wrapper.Payload = Payload;
			m_Wrapper.StatValueCollection = _stats.StatValueCollection;
			m_Wrapper.ApplyEffects(s_WrappedEffectInstances);
			wrapperEffectInstance.WrappedEffectInstances.AddRange(s_WrappedEffectInstances);
			return wrapperEffectInstance;
		}
	}
}
