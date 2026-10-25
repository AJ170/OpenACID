using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class ParticipantCarrierSkillEffectBase<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public EntityId SourceEntityId { get; set; }

		public IParticipantCarrier Source
		{
			get
			{
				return SourceEntityId.TryGetEntity<IParticipantCarrier>();
			}
		}

		public IParticipantCarrier Target
		{
			get
			{
				return TargetEntityId.GetEntity<IParticipantCarrier>();
			}
		}

		public CombatParticipant SourceParticipant
		{
			get
			{
				return (Source == null) ? null : Source.CombatParticipantProxy.CombatParticipant;
			}
		}

		public CombatParticipant TargetParticipant
		{
			get
			{
				return Target.CombatParticipantProxy.CombatParticipant;
			}
		}

		protected ParticipantCarrierSkillEffectBase(EEffectType _effectType, IRule _owner, ETargetSource _targetSource)
			: base(_effectType, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			switch (m_targetSource)
			{
			case ETargetSource.Payload:
				SourceEntityId = _entity.Id;
				break;
			case ETargetSource.RuleOwner:
				SourceEntityId = Payload.Cast<IEntityIdSkillPayload>().Id;
				break;
			}
			return result;
		}
	}
}
