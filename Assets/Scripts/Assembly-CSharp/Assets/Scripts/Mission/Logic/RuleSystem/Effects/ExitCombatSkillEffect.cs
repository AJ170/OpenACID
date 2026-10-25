using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ExitCombatSkillEffect<T> : SkillEffectBase<T, EntityIdSkillPayload> where T : AssassinCharacter
	{
		private readonly ExitCombatMode m_exitCombatMode;

		public ExitCombatSkillEffect(IRule _owner, ETargetSource _targetSource, ExitCombatMode _exitCombatMode)
			: base(EEffectType.ExitCombat, _owner, _targetSource)
		{
			base.Predicate = CombatActivePredicate;
			m_exitCombatMode = _exitCombatMode;
		}

		private static bool CombatActivePredicate(IEffect _effect, IRulesCarrier _rules)
		{
			return CombatController.IsActive;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			Animator animator = _entity.UnityComponentCache.Animator;
			animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
			CombatController.Instance.EndLikeNothingHappened(m_exitCombatMode);
			return result;
		}
	}
}
