using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class AbortSkillEffect : EffectBase<ISkillsCarrier>
	{
		public Enum<SkillTypes> SkillType { get; private set; }

		public AbortSkillEffect(IRule _owner, Enum<SkillTypes> _skillType)
			: base(EEffectType.AbortSkill, _owner)
		{
			SkillType = _skillType;
		}

		protected override IEffectInstance Apply(ISkillsCarrier _entity, IBuffedStatsCollectionHandle _stats)
		{
			_entity.Skills.Abort(SkillType);
			return null;
		}
	}
}
