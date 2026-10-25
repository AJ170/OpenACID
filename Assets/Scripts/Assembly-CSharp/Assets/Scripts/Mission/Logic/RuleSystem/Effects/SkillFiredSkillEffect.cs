using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SkillFiredSkillEffect : SkillEffectBase<AssassinCharacter, ISkillPayload>
	{
		private readonly Enum<SkillTypes> m_SkillType;

		public SkillFiredSkillEffect(IRule _owner, Enum<SkillTypes> _skillType)
			: base(EEffectType.SkillFired, _owner, ETargetSource.RuleOwner)
		{
			m_SkillType = _skillType;
		}

		protected override IEffectInstance Apply(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			_entity.Skills.SetRuleApplied(m_SkillType);
			return result;
		}
	}
}
