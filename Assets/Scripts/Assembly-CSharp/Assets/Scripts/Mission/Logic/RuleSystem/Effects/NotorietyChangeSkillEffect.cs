using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class NotorietyChangeSkillEffect<T> : SkillEffectBase<T, EntityIdSkillPayload> where T : ISkillsCarrier
	{
		private readonly float m_NotorietyMax;

		public NotorietyChangeSkillEffect(IRule _owner, float _notorietyMax, ETargetSource _targetSource)
			: base(EEffectType.NotorietyChange, _owner, _targetSource)
		{
			m_NotorietyMax = _notorietyMax;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			PlayerCharacter entity = TargetEntityId.GetEntity<PlayerCharacter>();
			float num = entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
			if (num > m_NotorietyMax)
			{
				entity.PlayerCharacterStateMachines.PlayerAlertLevel.ForceNotoriety(m_NotorietyMax);
			}
			return result;
		}
	}
}
