using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class SkillEffectBase<T, TPayload> : TargetEffectBase<T>, ISkillPayloadCarrier, ITargetCarrier, IEffect, ITargetEffect, ISkillEffect where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public ISkillPayload Payload { get; set; }

		public TPayload TypedPayload
		{
			get
			{
				return (TPayload)Payload;
			}
		}

		protected SkillEffectBase(EEffectType _type, IRule _owner, ETargetSource _targetSource)
			: base(_type, _owner, _targetSource)
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

		public override void ComputeTargetEntityId(IRulesCarrier _rulesCarrier)
		{
			base.ComputeTargetEntityId(_rulesCarrier);
			ETargetSource targetSource = m_targetSource;
			if (targetSource == ETargetSource.Payload)
			{
				TargetEntityId = Payload.Cast<IEntityIdSkillPayload>().Id;
			}
		}
	}
}
