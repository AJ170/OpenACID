using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class SkillFiredTrigger<T> : CategorySkillTriggerBase<T, ISkillPayload> where T : IRulesCarrier, ISkillsCarrier
	{
		private readonly Enum<SkillTypes> m_SkillType;

		public Enum<SkillTypes> SkillType
		{
			get
			{
				return m_SkillType;
			}
		}

		public SkillFiredTrigger(IRule _owner, Enum<SkillTypes> _skillType)
			: base(ETriggerType.SkillFired, _owner, UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(_skillType).Frequency, UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(_skillType).Categories, ETargetSource.Custom)
		{
			m_SkillType = _skillType;
			base.DelayedCheck = true;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && _entity.Cast<ISkillsCarrier>().Skills.GetState(m_SkillType) == ESkillState.Triggered;
		}

		public override bool GetIsCategoryMatching(IRulesCarrier _entity)
		{
			return base.GetIsCategoryMatching(_entity) && Categories.Include(ETriggerCategoryFlags.NoSkillFired).Contains(_entity.Rules.CurrentTriggerCategoryFlags);
		}
	}
}
