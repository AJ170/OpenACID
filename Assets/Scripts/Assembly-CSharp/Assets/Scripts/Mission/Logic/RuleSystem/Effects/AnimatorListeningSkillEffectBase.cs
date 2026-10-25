using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class AnimatorListeningSkillEffectBase<T, TPayload> : AnimatorListeningEffectBase<T>, ISkillPayloadCarrier, ITargetCarrier, IEffect, ITargetEffect, ISkillEffect where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public ISkillPayload Payload { get; set; }

		public TPayload TypedPayload
		{
			get
			{
				return (TPayload)Payload;
			}
		}

		protected AnimatorListeningSkillEffectBase(EEffectType _type, IRule _owner, ETargetSource _targetSource, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash)
			: base(_type, _owner, _targetSource, _eventType, _layer, _animHash)
		{
		}

		public override void Reset()
		{
			base.Reset();
			Payload = null;
		}

		public IEffectInstance Apply(ISkillsCarrier _entity)
		{
			using (IBuffedStatsCollectionHandle stats = Owner.StatValueCollection.Apply(_entity))
			{
				return Apply(_entity, stats);
			}
		}
	}
}
