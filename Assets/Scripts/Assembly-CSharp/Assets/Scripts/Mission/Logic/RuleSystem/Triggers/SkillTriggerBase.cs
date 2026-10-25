using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public abstract class SkillTriggerBase<T, TPayload> : TargetTriggerBase<T>, ISkillPayloadCarrier, ITargetCarrier, ITrigger, IActivityConditionTrigger, ITargetTrigger, ISkillTrigger where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public ISkillPayload Payload { get; set; }

		public bool DelayedCheck { get; set; }

		public bool ActivityCondition { get; set; }

		public override ETriggerCategoryFlags Categories
		{
			get
			{
				return m_Categories;
			}
			protected set
			{
				m_Categories = value.Include(ETriggerCategoryFlags.NonInteractive | ETriggerCategoryFlags.NoSkillFired);
			}
		}

		public TPayload TypedPayload
		{
			get
			{
				return (TPayload)Payload;
			}
		}

		protected SkillTriggerBase(ETriggerType _type, IRule _owner, ETargetSource _targetSource)
			: base(_type, _owner, _targetSource)
		{
			DelayedCheck = false;
			ActivityCondition = false;
		}

		public override void Reset()
		{
			base.Reset();
			Payload = null;
		}

		protected override bool ComputeTargetEntityId(T _entity)
		{
			if (!base.ComputeTargetEntityId(_entity))
			{
				return false;
			}
			if (Payload != null && !(Payload is TPayload))
			{
				return false;
			}
			if (m_targetSource == ETargetSource.Payload)
			{
				if (Payload == null)
				{
					return false;
				}
				TargetEntityId = Payload.Cast<IEntityIdSkillPayload>().Id;
			}
			return true;
		}
	}
}
