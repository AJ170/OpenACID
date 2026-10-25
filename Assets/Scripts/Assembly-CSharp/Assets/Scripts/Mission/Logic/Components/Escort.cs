using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Debug;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Escort : TurnUpdateableComponent, IVictim
	{
		private const float TimeLeftInvalid = -1f;

		private readonly SimpleGameTimer m_threateningCountdown = new SimpleGameTimer();

		private readonly SimpleGameTimer m_resumePatrolTimer = new SimpleGameTimer();

		private readonly Threatening m_Threatening;

		private bool m_active;

		public EVictimType VictimType
		{
			get
			{
				return EVictimType.Escortee;
			}
		}

		public override int Priority
		{
			get
			{
				return 30;
			}
		}

		public override bool IsActive
		{
			get
			{
				return m_active;
			}
			set
			{
				m_active = value;
				if (!m_active)
				{
					Reset();
				}
			}
		}

		public float MaxSurviveDuration { get; private set; }

		public float ResumePatrolDelay { get; private set; }

		public bool AreEnemiesThreatening
		{
			get
			{
				return m_Threatening.AreEnemiesThreatening;
			}
		}

		public bool IsVictimAttacked
		{
			get
			{
				return m_Threatening.IsVictimAttacked;
			}
		}

		public bool IsVictimHit
		{
			get
			{
				return m_Threatening.IsVictimHit;
			}
		}

		public bool ResumeRunning
		{
			get
			{
				return m_resumePatrolTimer.IsRunning;
			}
		}

		public bool ThreateningCountdownRunning
		{
			get
			{
				return m_threateningCountdown.IsRunning;
			}
		}

		public bool ThreateningOrKillRunning
		{
			get
			{
				return m_Threatening.IsVictimAttacked || m_threateningCountdown.IsStarted;
			}
		}

		public bool ThreateningAndKillFinished
		{
			get
			{
				return m_Threatening.HasVictimKillAnimEnded && ThreateningFinished;
			}
		}

		public bool ThreateningFinished
		{
			get
			{
				return m_threateningCountdown.IsFinished;
			}
		}

		public float ThreatenedTimeLeft
		{
			get
			{
				return m_Threatening.IsVictimAttacked ? 0f : ((!m_threateningCountdown.IsStarted) ? (-1f) : m_threateningCountdown.TimeToFinish);
			}
		}

		public Escort(IEntity _entity)
			: base(_entity)
		{
			m_Threatening = new Threatening((Assets.Scripts.Mission.Logic.Entities.Patrol)_entity);
		}

		public void Init(EntityId _escortee, float _collectionRadius, float _maxSurviveDuration, float _resumePatrolDelay, int _enemyCountCap)
		{
			m_Threatening.Init(_escortee, _collectionRadius, _enemyCountCap, true);
			MaxSurviveDuration = _maxSurviveDuration;
			ResumePatrolDelay = _resumePatrolDelay;
			m_threateningCountdown.Reset();
			m_resumePatrolTimer.Reset();
			IsActive = true;
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			if (m_threateningCountdown.IsFinished && m_resumePatrolTimer.IsFinished)
			{
				return;
			}
			Profiling.Start("Escort:DoTurnUpdate:ThreatRunning", float.MaxValue);
			m_Threatening.LetEnemiesThreatVictim();
			int count = m_Threatening.EnemiesArrived.Count;
			int count2 = m_Threatening.EnemiesThreatening.Count;
			if (!m_threateningCountdown.IsStarted && count > 0)
			{
				m_threateningCountdown.Start(MaxSurviveDuration);
				m_resumePatrolTimer.Reset();
			}
			else if (m_threateningCountdown.IsRunning)
			{
				if (count == 0)
				{
					m_threateningCountdown.Reset();
					m_resumePatrolTimer.Start(ResumePatrolDelay);
				}
			}
			else if (m_resumePatrolTimer.IsFinished)
			{
				Reset();
			}
			else if (count2 != 0)
			{
				m_resumePatrolTimer.Start(ResumePatrolDelay);
			}
			else if ((m_resumePatrolTimer.IsRunning || m_Threatening.EnemiesFightingCount > 0) && (CombatController.IsActive || LogicMain.PlayerEntity.GetIsPerformingAssassination()))
			{
				m_resumePatrolTimer.Start(ResumePatrolDelay);
			}
			Profiling.Stop();
		}

		public bool RegisterArrivedEnemy(EntityId _enemyId)
		{
			return m_Threatening.RegisterArrivedEnemy(_enemyId);
		}

		public void DeregisterArrivedEnemy(EntityId _enemyId)
		{
			m_Threatening.DeregisterArrivedEnemy(_enemyId);
		}

		public bool HasArrived(EntityId _enemyId)
		{
			return m_Threatening.EnemiesArrived.Contains(_enemyId);
		}

		public EnemyCharacter GetClosestEnemyInRange()
		{
			return m_Threatening.GetClosestEnemyInRange();
		}

		public void StartKillVictim(NpcCharacter _source)
		{
			m_Threatening.StartKillVictim(_source);
		}

		public void EndKillVictim()
		{
			m_Threatening.EndKillVictim();
		}

		private void Reset()
		{
			m_Threatening.Reset();
			m_threateningCountdown.Reset();
			m_resumePatrolTimer.Reset();
		}
	}
}
