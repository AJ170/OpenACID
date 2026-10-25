using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class HasKillTokensSkillTrigger<TEntity, TPayload> : CategorySkillTriggerBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly int m_killTokensRequired;

		public HasKillTokensSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, int _killTokensRequired)
			: base(ETriggerType.KillTokens, _owner, _frequency, _categoryFlags, _targetSource)
		{
			m_killTokensRequired = _killTokensRequired;
		}

		protected override bool IsActive(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			PlayerCharacter playerCharacter = TargetEntityId.TryGetEntity<PlayerCharacter>();
			return playerCharacter != null && playerCharacter.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount) >= m_killTokensRequired;
		}
	}
}
