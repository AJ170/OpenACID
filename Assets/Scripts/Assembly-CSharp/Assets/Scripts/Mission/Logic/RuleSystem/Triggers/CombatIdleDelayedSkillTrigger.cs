using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class CombatIdleDelayedSkillTrigger<T, TPayload> : SkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly bool m_weaponRequired;

		public CombatIdleDelayedSkillTrigger(IRule _owner, ETargetSource _targetSource, bool _weaponRequired = false)
			: base(ETriggerType.CombatIdle, _owner, _targetSource)
		{
			base.DelayedCheck = true;
			base.Frequency = ETriggerFrequency.Framewise;
			Categories = ETriggerCategoryFlags.InCombat;
			m_weaponRequired = _weaponRequired;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			CombatParticipant combatParticipant = TargetEntityId.GetEntity<IParticipantCarrier>().CombatParticipantProxy.CombatParticipant;
			return combatParticipant != null && (!m_weaponRequired || combatParticipant.IsWeaponUnsheathed()) && combatParticipant.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Idle;
		}
	}
}
