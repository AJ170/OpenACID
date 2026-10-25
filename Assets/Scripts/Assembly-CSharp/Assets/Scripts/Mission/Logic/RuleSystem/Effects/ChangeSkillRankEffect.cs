using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ChangeSkillRankEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private class EffectInstance : EffectInstanceBase
		{
			private readonly ISkillsCarrier m_Entity;

			private readonly Enum<SkillTypes> m_SkillType;

			private readonly int m_Value;

			private readonly int m_EndTurn;

			public EffectInstance(IEffect _effect, ISkillsCarrier _entity, Enum<SkillTypes> _skillType, int _value, int _endTurn)
				: base(_effect)
			{
				m_Entity = _entity;
				m_SkillType = _skillType;
				m_Value = _value;
				m_EndTurn = _endTurn;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				bool flag = m_EndTurn >= 0 && LogicMain.CurrentTurn >= m_EndTurn;
				if (flag)
				{
					Abort(_entity);
				}
				return flag;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_Entity.Skills.RemoveRankBuff(m_SkillType, m_Value);
			}
		}

		private readonly Enum<SkillTypes> m_SkillType;

		private readonly int m_Value;

		private readonly int m_TurnDuration;

		public ChangeSkillRankEffect(IRule _owner, ETargetSource _targetSource, Enum<SkillTypes> _skillType, int _value, int _turnDuration)
			: base(EEffectType.ChangeSkillRank, _owner, _targetSource)
		{
			m_SkillType = _skillType;
			m_Value = _value;
			m_TurnDuration = _turnDuration;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			ISkillsCarrier entity = TargetEntityId.GetEntity<ISkillsCarrier>();
			entity.Skills.AddRankBuff(m_SkillType, m_Value);
			return new EffectInstance(this, entity, m_SkillType, m_Value, LogicMain.CurrentTurn + m_TurnDuration);
		}
	}
}
