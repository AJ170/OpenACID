using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AllReadyForExitCombatTrigger<T> : SkillTriggerBase<T, ISkillPayload> where T : ISkillsCarrier
	{
		public AllReadyForExitCombatTrigger(IRule _owner)
			: base(ETriggerType.AllReadyForExitCombat, _owner, ETargetSource.Custom)
		{
			base.Frequency = ETriggerFrequency.Framewise;
			Categories = ETriggerCategoryFlags.InCombat;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			CombatController instance = CombatController.Instance;
			int num = instance.ParticipantCount();
			for (int i = 0; i < num; i++)
			{
				if (!instance.GetParticipant(i).ReadyForExitCombat())
				{
					return false;
				}
			}
			return num > 0;
		}
	}
}
