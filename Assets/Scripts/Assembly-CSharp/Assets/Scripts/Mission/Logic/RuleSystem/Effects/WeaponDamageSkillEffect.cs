using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class WeaponDamageSkillEffect<T, TPayload> : ParticipantCarrierSkillEffectBase<T, TPayload> where T : ISkillsCarrier, IParticipantCarrier where TPayload : ISkillPayload
	{
		private readonly EApplyDamageFlags m_flags;

		public WeaponDamageSkillEffect(IRule _owner, ETargetSource _targetSource, EApplyDamageFlags _flags = EApplyDamageFlags.None)
			: base(EEffectType.WeaponDamage, _owner, _targetSource)
		{
			m_flags = _flags;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			CombatHelper.ApplyWeaponDamage(base.Source.CombatParticipantProxy.Character, base.Target.CombatParticipantProxy.Character, m_flags);
			return result;
		}
	}
}
