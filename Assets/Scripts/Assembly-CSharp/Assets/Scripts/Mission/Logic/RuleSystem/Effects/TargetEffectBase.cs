using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class TargetEffectBase<T> : EffectBase<T>, ITargetCarrier, IEffect, ITargetEffect where T : IRulesCarrier
	{
		protected readonly ETargetSource m_targetSource;

		public EntityId TargetEntityId { get; set; }

		public ETargetSource TargetSource
		{
			get
			{
				return m_targetSource;
			}
		}

		protected TargetEffectBase(EEffectType _type, IRule _owner, ETargetSource _targetSource)
			: base(_type, _owner)
		{
			m_targetSource = _targetSource;
		}

		public virtual void ComputeTargetEntityId(IRulesCarrier _rulesCarrier)
		{
			switch (m_targetSource)
			{
			case ETargetSource.RuleOwner:
				TargetEntityId = _rulesCarrier.Id;
				break;
			case ETargetSource.Player:
				TargetEntityId = LogicMain.PlayerEntity.Id;
				break;
			case ETargetSource.Payload:
				break;
			}
		}

		public override void Prepare(IRulesCarrier _entity)
		{
			base.Prepare(_entity);
			ComputeTargetEntityId(_entity);
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return null;
		}
	}
}
