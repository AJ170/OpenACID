using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	internal class FleeEffect : SkillEffectBase<PlayerCharacter, ISkillPayload>
	{
		private class FleeEffectInstance : EffectInstanceBase
		{
			private const float EffectMinTime = 0.5f;

			private readonly SimpleGameTimer m_minTimer = new SimpleGameTimer();

			public FleeEffectInstance(IEffect _effect)
				: base(_effect)
			{
				m_minTimer.Start(0.5f);
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (((AssassinCharacter)_entity).GetIsHiding() || (m_minTimer.IsFinished && !((ISkillsCarrier)_entity).Skills.GetIsActive("Flee_From_Combat".ToEnum<SkillTypes>())))
				{
					Abort(_entity);
					return false;
				}
				return true;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				PlayerHelper.Entity.Highlight.RemoveMode(Highlight.EHighlightLayer.Fleeing);
			}
		}

		public FleeEffect(IRule _owner, ETargetSource _targetSource)
			: base(EEffectType.Action, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			CombatController.Instance.PlayerWantsToFlee();
			return new FleeEffectInstance(this);
		}
	}
}
