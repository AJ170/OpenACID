using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class MultipleTargetsSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload>, IMultipleTargetsCarrier, IEffect, IMultipleTargetsEffect where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly ITargetEffect m_Effect;

		public BBList<EntityId> TargetEntityIds { get; private set; }

		public MultipleTargetsSkillEffect(ETargetSource _targetSource, ITargetEffect _effect)
			: base(EEffectType.MutipleTargetsWrapper, _effect.Owner, _targetSource)
		{
			m_Effect = _effect;
			TargetEntityIds = new BBList<EntityId>();
		}

		public override void Reset()
		{
			base.Reset();
			m_Effect.Reset();
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			WrapperEffectInstance wrapperEffectInstance = new WrapperEffectInstance(this);
			for (int i = 0; i < TargetEntityIds.Count; i++)
			{
				m_Effect.Reset();
				ISkillEffect skillEffect = m_Effect as ISkillEffect;
				if (skillEffect != null)
				{
					skillEffect.Payload = new EntityIdSkillPayload(TargetEntityIds[i]);
				}
				m_Effect.Prepare(_entity);
				if (m_Effect.Predicate == null || m_Effect.Predicate(m_Effect, _entity))
				{
					IEffectInstance effectInstance = m_Effect.Apply(_entity, _stats);
					if (effectInstance != null)
					{
						wrapperEffectInstance.WrappedEffectInstances.Add(effectInstance);
					}
				}
			}
			return wrapperEffectInstance;
		}
	}
}
