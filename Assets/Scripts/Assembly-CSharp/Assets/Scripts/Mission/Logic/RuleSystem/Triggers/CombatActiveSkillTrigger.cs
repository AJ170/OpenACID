using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class CombatActiveSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public CombatActiveSkillTrigger(IRule _owner, ETargetSource _targetSource)
			: base(ETriggerType.CombatActive, _owner, ETriggerFrequency.Framewise, ETriggerCategoryFlags.InCombat, _targetSource)
		{
		}

		public CombatActiveSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource)
			: base(ETriggerType.CombatActive, _owner, _frequency, _categories, _targetSource)
		{
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			IParticipantCarrier participantCarrier = TargetEntityId.TryGetEntity<IParticipantCarrier>();
			return participantCarrier != null && participantCarrier.CombatParticipantProxy.IsInCombat;
		}
	}
}
