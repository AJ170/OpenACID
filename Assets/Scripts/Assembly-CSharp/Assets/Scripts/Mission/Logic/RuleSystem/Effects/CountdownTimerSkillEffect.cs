using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.Combat;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class CountdownTimerSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		protected class EffectInstance : EffectInstanceBase
		{
			private const int TurnsPerSecond = 10;

			private int m_lastTurnUpdate;

			private readonly int m_endTurn;

			private EntityId m_targetId = EntityId.InvalidId;

			private readonly BBList<ICombatAnnotation> m_activeAnnotations = new BBList<ICombatAnnotation>(2);

			public EffectInstance(IEffect _effect, EntityId _targetId, int _duration)
				: base(_effect)
			{
				m_targetId = _targetId;
				m_lastTurnUpdate = -1;
				m_endTurn = LogicMain.CurrentTurn + _duration;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				int currentTurn = LogicMain.CurrentTurn;
				if (m_lastTurnUpdate == currentTurn)
				{
					return true;
				}
				m_lastTurnUpdate = currentTurn;
				int num = m_endTurn - currentTurn;
				if (num <= 0)
				{
					return Exit();
				}
				if (num % 10 == 0)
				{
					IGameEntity gameEntity = m_targetId.TryGetEntity<IGameEntity>();
					if (gameEntity == null)
					{
						return Exit();
					}
					AddAnnotation(CombatAnnotation.GetNumber(gameEntity, num / 10, CombatAnnotation.Type.Timer));
				}
				return true;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				Exit();
			}

			private bool Exit()
			{
				while (m_activeAnnotations.Count > 0)
				{
					m_activeAnnotations[0].Release();
				}
				return false;
			}

			private void AddAnnotation(ICombatAnnotation _annotation)
			{
				_annotation.OnRelease += OnAnnotationRelease;
				m_activeAnnotations.Add(_annotation);
			}

			private void OnAnnotationRelease(ICombatAnnotation _o)
			{
				m_activeAnnotations.Remove(_o);
				_o.OnRelease -= OnAnnotationRelease;
			}
		}

		private readonly StatTypes.EStatType m_durationStat;

		public CountdownTimerSkillEffect(IRule _owner, ETargetSource _targetSource, StatTypes.EStatType _durationStat)
			: base(EEffectType.CountdownTimer, _owner, _targetSource)
		{
			m_durationStat = _durationStat;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			int duration = _stats.Get_int(m_durationStat);
			return new EffectInstance(this, TargetEntityId, duration);
		}
	}
}
