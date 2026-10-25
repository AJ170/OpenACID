using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class RemoveSkillSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly Enum<SkillTypes> m_SkillType;

		public RemoveSkillSkillEffect(IRule _owner, ETargetSource _targetSource, Enum<SkillTypes> _skillType)
			: base(EEffectType.RemoveSkill, _owner, _targetSource)
		{
			m_SkillType = _skillType;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			ISkillsCarrier entity = TargetEntityId.GetEntity<ISkillsCarrier>();
			entity.Skills.Remove(m_SkillType);
			return null;
		}
	}
}
