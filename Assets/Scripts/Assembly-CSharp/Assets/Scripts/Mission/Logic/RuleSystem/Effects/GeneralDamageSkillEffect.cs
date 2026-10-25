using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class GeneralDamageSkillEffect<T, TPayload> : ParticipantCarrierSkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly EApplyDamageFlags m_flags;

		private readonly bool m_increaseKillTokens;

		public GeneralDamageSkillEffect(IRule _owner, ETargetSource _targetSource, EApplyDamageFlags _flags, bool _increaseKillTokens)
			: base(EEffectType.GeneralDamage, _owner, _targetSource)
		{
			m_flags = _flags;
			m_increaseKillTokens = _increaseKillTokens;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			Character character = (Character)base.Target;
			if (m_increaseKillTokens && character.GetDeadOrAboutToDie())
			{
				return result;
			}
			CombatHelper.ApplySkillDamage(_entity, (Character)base.Source, character, _stats, m_flags);
			if (m_increaseKillTokens)
			{
				CombatHelper.IncreaseKillTokenCount(character.GetHasZeroHealth());
			}
			return result;
		}
	}
}
