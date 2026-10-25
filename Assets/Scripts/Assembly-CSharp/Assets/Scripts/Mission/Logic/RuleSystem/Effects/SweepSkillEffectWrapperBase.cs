using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class SweepSkillEffectWrapperBase<T, TPayload, TTargetType> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload where TTargetType : class, IEntity
	{
		protected abstract class SweepEffectInstanceBase : EffectInstanceBase
		{
			protected readonly SkillEffectsWrapper m_wrapper;

			protected readonly TTargetType m_entity;

			protected float m_progress;

			protected readonly BBList<EntityId> m_targetList = new BBList<EntityId>();

			private readonly BBList<EntityId> m_excludeList = new BBList<EntityId>();

			private static readonly BBList<IEffectInstance> s_effectInstances = new BBList<IEffectInstance>();

			protected SweepEffectInstanceBase(IEffect _effect, TTargetType _entity, SkillEffectsWrapper _wrapper)
				: base(_effect)
			{
				m_entity = _entity;
				m_wrapper = _wrapper;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (m_progress >= 1f)
				{
					return false;
				}
				SweepSkillEffectWrapperBase<T, TPayload, TTargetType> sweepSkillEffectWrapperBase = Effect.Cast<SweepSkillEffectWrapperBase<T, TPayload, TTargetType>>();
				m_progress += UbiCore.Utils.Singleton<Timer>.Instance.deltaTime / sweepSkillEffectWrapperBase.m_duration;
				m_progress = Mathf.Min(m_progress, 1f);
				m_targetList.ClearIfNotEmpty();
				OnSweepUpdate();
				for (int i = 0; i < m_targetList.Count; i++)
				{
					if (!m_excludeList.Contains(m_targetList[i]))
					{
						m_excludeList.Add(m_targetList[i]);
						m_wrapper.Payload = new EntityIdSkillPayload(m_targetList[i]);
						s_effectInstances.ClearIfNotEmpty();
						m_wrapper.ApplyEffects(s_effectInstances);
						OnSweepHitPostEffect(m_targetList[i]);
					}
				}
				return true;
			}

			protected abstract void OnSweepUpdate();

			protected abstract void OnSweepHitPostEffect(EntityId _targetId);

			public override void Abort(IRulesCarrier _entity)
			{
			}
		}

		public delegate void SweepCallback(TTargetType _entity, float _progress, BBList<EntityId> _targetIds);

		private readonly SkillEffectsWrapper m_wrapperPrototype;

		private readonly float m_duration;

		protected SweepSkillEffectWrapperBase(IRule _owner, ETargetSource _targetSource, BBList<IEffect> _effects, float _duration)
			: base(EEffectType.SweepWrapper, _owner, _targetSource)
		{
			m_wrapperPrototype = new SkillEffectsWrapper(_effects, RulesHelper.CategoryMatchingEffectPredicate);
			m_duration = _duration;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			SkillEffectsWrapper skillEffectsWrapper = m_wrapperPrototype.Clone().Cast<SkillEffectsWrapper>();
			skillEffectsWrapper.Entity = _entity;
			skillEffectsWrapper.StatValueCollection = _stats.StatValueCollection;
			return CreateEffectInstance(TargetEntityId.GetEntity<TTargetType>(), skillEffectsWrapper);
		}

		protected abstract IEffectInstance CreateEffectInstance(TTargetType _entity, SkillEffectsWrapper _wrapper);
	}
}
