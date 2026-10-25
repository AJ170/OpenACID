using AcierProtocol;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Captive : TurnUpdateableComponent, IVictim
	{
		private readonly Threatening m_Threatening;

		private bool m_IsActive;

		private int m_LiberationTurn;

		public bool CanLiberate
		{
			get
			{
				return !IsVictimAttacked && m_LiberationTurn == 0 && m_Threatening.Victim.IsValid;
			}
		}

		public EVictimType VictimType
		{
			get
			{
				return EVictimType.Captive;
			}
		}

		public override int Priority
		{
			get
			{
				return 31;
			}
		}

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

		public bool ThreateningOrKillRunning
		{
			get
			{
				return m_Threatening.IsVictimAttacked && m_Threatening.EnemiesThreatening.Count > 0;
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
				if (m_Threatening.EnemiesThreatening.Count == 0)
				{
					return false;
				}
				EnemyCharacter enemyCharacter = m_Threatening.EnemiesThreatening[0];
				if (enemyCharacter.GetIsDead())
				{
					return false;
				}
				EStateType currentStateType = enemyCharacter.EnemyCharacterStateMachines.CharacterAwareness.CurrentStateType;
				if (currentStateType == EStateType.CharacterAwareness_Alarmed)
				{
					return true;
				}
				if (m_Threatening.IsVictimAttacked)
				{
					return true;
				}
				return false;
			}
		}

		public override bool IsActive
		{
			get
			{
				return m_IsActive;
			}
			set
			{
				if (m_IsActive != value)
				{
					m_IsActive = value;
					if (!m_IsActive && m_Threatening.Victim.IsValid && !m_Threatening.IsVictimAttacked)
					{
						LogicMain.PlayerEntity.Vision.RemoveEntity(m_Threatening.Victim);
					}
					StopLiberation();
					m_Threatening.Reset();
					Entity.EntityStats.Set_bool(StatTypes.EStatType.Patrol_Enabled, m_IsActive);
				}
			}
		}

		public Captive(IEntity _entity)
			: base(_entity)
		{
			m_Threatening = new Threatening((Assets.Scripts.Mission.Logic.Entities.Patrol)_entity);
		}

		public void Init(EntityId _victim, float _collectionRadius, int _enemyCountCap)
		{
			m_Threatening.Init(_victim, _collectionRadius, _enemyCountCap, false);
			LogicMain.MissionInfo.CaptiveCount++;
			LogicMain.PlayerEntity.Vision.AddEntity(_victim);
			IsActive = true;
		}

		public bool Liberate()
		{
			if (!CanLiberate)
			{
				return false;
			}
			m_LiberationTurn = LogicMain.CurrentTurn;
			LogicMain.PlayerEntity.EntityStats.Set_bool(StatTypes.EStatType.Player_MovementPrevented, true);
			return true;
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
			NpcCharacter entity = m_Threatening.Victim.GetEntity<NpcCharacter>();
			entity.Patrol.PatrolComponent.MissionData.SetMapIconType(EMapIconType.Invalid_EMapIconType);
			entity.CanBePooled = false;
			LogicMain.PlayerEntity.Vision.RemoveEntity(entity.Id);
		}

		public void EndKillVictim()
		{
			m_Threatening.EndKillVictim();
			LogicMain.MissionResult.IncreaseLostCaptivesCount();
			LogicMain.MissionInfo.CaptiveCount--;
			NpcCharacter npcCharacter = m_Threatening.Victim.TryGetEntity<NpcCharacter>();
			npcCharacter.CanBePooled = true;
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			base.DoTurnUpdate(_deltaTurns);
			if (!m_Threatening.Victim.IsValid)
			{
				return;
			}
			Assets.Scripts.Mission.Logic.Entities.Patrol patrol = (Assets.Scripts.Mission.Logic.Entities.Patrol)Entity;
			if (m_LiberationTurn > 0 && LogicMain.CurrentTurn - m_LiberationTurn >= 30 && !patrol.PatrolStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
			{
				if (patrol.PatrolComponent.Formation.Entities.Count > 0)
				{
					Vector3 position = patrol.PatrolComponent.Formation.Entities[0].m_id.GameEntity.Position;
					if (PlayerHelper.Entity.Position.DistanceXZSquared(position) <= 1f)
					{
						PlayerHelper.Entity.FinalizeFreeCaptives(patrol.Id);
						IsActive = false;
						StopLiberation();
					}
					else
					{
						StopLiberation();
					}
				}
				else
				{
					StopLiberation();
				}
			}
			NpcCharacter npcCharacter = m_Threatening.Victim.TryGetEntity<NpcCharacter>();
			if (m_Threatening.IsVictimAttacked)
			{
				if (!npcCharacter.IsAttached)
				{
					LogicMain.EntityManager.UnregisterEntity(npcCharacter);
				}
			}
			else if (m_Threatening.EnemiesThreatening.Count == 0 && !m_Threatening.IsVictimHit && !PlayerHelper.IsPerformingStrikingAssassination)
			{
				m_Threatening.LetEnemiesThreatVictim();
			}
		}

		private void StopLiberation()
		{
			m_LiberationTurn = 0;
			LogicMain.PlayerEntity.EntityStats.Set_bool(StatTypes.EStatType.Player_MovementPrevented, false);
		}
	}
}
