using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class FearsomeSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public FearsomeSkillEffect(IRule _owner, ETargetSource _targetSource)
			: base(EEffectType.Fearsome, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			CombatParticipant combatParticipant = null;
			if (m_targetSource == ETargetSource.Payload)
			{
				IParticipantCarrier participantCarrier = TargetEntityId.TryGetEntity<IParticipantCarrier>();
				if (participantCarrier == null)
				{
					return result;
				}
				combatParticipant = participantCarrier.CombatParticipantProxy.CombatParticipant;
				if (combatParticipant == null)
				{
					return result;
				}
			}
			float num = _stats.Get_float(StatTypes.EStatType.Rule_FearsomeFactor);
			if (num <= 0f)
			{
				return result;
			}
			CombatController instance = CombatController.Instance;
			if (instance != null && CombatController.IsActive)
			{
				instance.TriggerFleeRoll(combatParticipant, num);
			}
			return result;
		}
	}
}
