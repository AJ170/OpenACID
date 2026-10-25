using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class RemoveSkillsSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly Action<ISkillsCarrier, BBSimpleList<Enum<SkillTypes>>> m_SkillsGetter;

		private static readonly BBSimpleList<Enum<SkillTypes>> s_SkillTypeList = new BBSimpleList<Enum<SkillTypes>>();

		public RemoveSkillsSkillEffect(IRule _owner, ETargetSource _targetSource, Action<ISkillsCarrier, BBSimpleList<Enum<SkillTypes>>> _skillsGetter)
			: base(EEffectType.RemoveSkill, _owner, _targetSource)
		{
			m_SkillsGetter = _skillsGetter;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			ISkillsCarrier entity = TargetEntityId.GetEntity<ISkillsCarrier>();
			s_SkillTypeList.Clear();
			m_SkillsGetter(entity, s_SkillTypeList);
			for (int i = 0; i < s_SkillTypeList.Count; i++)
			{
				entity.Skills.Remove(s_SkillTypeList[i]);
			}
			return null;
		}
	}
}
