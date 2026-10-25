using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public abstract class TargetTriggerBase<T> : TriggerBase<T>, ITargetCarrier, ITrigger, ITargetTrigger where T : IRulesCarrier
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

		protected TargetTriggerBase(ETriggerType _type, IRule _owner, ETargetSource _targetSource)
			: base(_type, _owner)
		{
			m_targetSource = _targetSource;
		}

		protected virtual bool ComputeTargetEntityId(T _entity)
		{
			switch (m_targetSource)
			{
			case ETargetSource.RuleOwner:
				TargetEntityId = _entity.Id;
				break;
			case ETargetSource.Player:
				TargetEntityId = LogicMain.PlayerEntity.Id;
				break;
			}
			return true;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			if (!ComputeTargetEntityId(_entity))
			{
				return false;
			}
			return true;
		}
	}
}
