using System;
using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Threatening : IAnimatorEventListener
	{
		public readonly Assets.Scripts.Mission.Logic.Entities.Patrol Patrol;

		public readonly BBList<EnemyCharacter> EnemiesThreatening = new BBList<EnemyCharacter>(10);

		public readonly BBList<EnemyCharacter> EnemiesInRange = new BBList<EnemyCharacter>(10);

		public readonly BBHashSet<EntityId> EnemiesArrived = new BBHashSet<EntityId>();

		public EntityId Victim { get; private set; }

		public float CollectionRadius { get; private set; }

		public int EnemyCountCap { get; private set; }

		public bool AddOtherPatrolMembers { get; private set; }

		public EntityId Killer { get; private set; }

		public int EnemiesFightingCount { get; private set; }

		public bool IsVictimAttacked { get; private set; }

		public bool IsVictimHit { get; private set; }

		public bool HasVictimKillAnimEnded { get; private set; }

		public bool AreEnemiesThreatening
		{
			get
			{
				return EnemiesThreatening.Count > 0 && !HasVictimKillAnimEnded;
			}
		}

		public Threatening(Assets.Scripts.Mission.Logic.Entities.Patrol _patrol)
		{
			Patrol = _patrol;
			Victim = EntityId.InvalidId;
			Killer = EntityId.InvalidId;
		}

		public void Init(EntityId _escortee, float _collectionRadius, int _enemyCountCap, bool _addOtherPatrolMembers)
		{
			Victim = _escortee;
			CollectionRadius = _collectionRadius;
			EnemyCountCap = _enemyCountCap;
			AddOtherPatrolMembers = _addOtherPatrolMembers;
		}

		public void Reset()
		{
			int count = EnemiesThreatening.Count;
			for (int i = 0; i < count; i++)
			{
				EnemyCharacter enemyCharacter = EnemiesThreatening[i];
				RemoveBuffFromEnemy(enemyCharacter.Id);
			}
			Killer = EntityId.InvalidId;
			EnemiesArrived.Clear();
			EnemiesInRange.Clear();
			EnemiesThreatening.Clear();
			EnemiesFightingCount = 0;
			IsVictimAttacked = false;
			IsVictimHit = false;
			HasVictimKillAnimEnded = false;
		}

		public EnemyCharacter GetClosestEnemyInRange()
		{
			EnemyCharacter result = null;
			float num = float.MaxValue;
			int count = EnemiesThreatening.Count;
			for (int i = 0; i < count; i++)
			{
				EnemyCharacter enemyCharacter = EnemiesThreatening[i];
				float num2 = enemyCharacter.Position.DistanceXZSquared(Patrol.Position);
				if (num2 < num)
				{
					num = num2;
					result = enemyCharacter;
				}
			}
			return result;
		}

		public bool RegisterArrivedEnemy(EntityId _enemyId)
		{
			if (IsVictimHit)
			{
				return false;
			}
			if (EnemiesThreatening.Contains(_enemyId.Entity as EnemyCharacter))
			{
				EnemiesArrived.Add(_enemyId);
				return true;
			}
			RemoveBuffFromEnemy(_enemyId);
			return false;
		}

		public void DeregisterArrivedEnemy(EntityId _enemyId)
		{
			EnemiesThreatening.Remove(_enemyId.GetEntity<EnemyCharacter>());
			EnemiesArrived.Remove(_enemyId);
			RemoveBuffFromEnemy(_enemyId);
		}

		public void StartKillVictim(NpcCharacter _source)
		{
			IsVictimAttacked = true;
			Killer = _source.Id;
			if (_source.IsAttached)
			{
				_source.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
			}
			else
			{
				HitVictim();
			}
		}

		public void EndKillVictim()
		{
			HitVictim();
			HasVictimKillAnimEnded = true;
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			if (_event.Type == AnimatorEventType.Hit)
			{
				HitVictim();
				return true;
			}
			return false;
		}

		public void LetEnemiesThreatVictim()
		{
			EnemiesInRange.Clear();
			EnemiesThreatening.Clear();
			EnemiesFightingCount = 0;
			EnemyHelper.FindEnemiesInRange(Patrol.Position, CollectionRadius, ReadyToThreat, EnemiesInRange);
			int num = Math.Min(EnemiesInRange.Count, EnemyCountCap);
			for (int i = 0; i < num; i++)
			{
				EnemyCharacter enemyCharacter = EnemiesInRange[i];
				if (!AddEnemyInRange(enemyCharacter) || !AddOtherPatrolMembers)
				{
					continue;
				}
				BBList<MemberInfo> entities = enemyCharacter.Patrol.PatrolComponent.Formation.Entities;
				int count = entities.Count;
				for (int j = 0; j < count; j++)
				{
					EntityId id = entities[j].m_id;
					if (id != enemyCharacter.Id)
					{
						EnemyCharacter enemyCharacter2 = id.TryGetEntity<EnemyCharacter>();
						if (enemyCharacter2 != null)
						{
							AddEnemyInRange(enemyCharacter2);
						}
					}
				}
			}
		}

		private bool ReadyToThreat(EnemyCharacter _enemyCharacter)
		{
			if (_enemyCharacter.GetDeadOrAboutToDie() || _enemyCharacter.GetIsImmortal() || !_enemyCharacter.GetIsThreateningOrNormal())
			{
				return false;
			}
			NpcCharacter npcCharacter = Victim.Entity as NpcCharacter;
			Vector3 direction = _enemyCharacter.Position - npcCharacter.Position;
			Vector3 headPosition = npcCharacter.GetHeadPosition();
			bool flag = Physics.Raycast(headPosition, direction, direction.magnitude, Layers.StaticCollidersLayersMaskPreset);
			return !flag;
		}

		private bool AddEnemyInRange(EnemyCharacter _enemyCharacter)
		{
			bool flag = false;
			if (_enemyCharacter.GetIsInCombat())
			{
				EnemiesFightingCount++;
			}
			else
			{
				flag = StartThreatening(_enemyCharacter);
				if (flag && !EnemiesThreatening.Contains(_enemyCharacter))
				{
					EnemiesThreatening.Add(_enemyCharacter);
				}
			}
			return flag;
		}

		private bool StartThreatening(EnemyCharacter _enemyCharacter)
		{
			return _enemyCharacter.StartThreatening(Victim);
		}

		private void HitVictim()
		{
			IsVictimHit = true;
			if (Killer.GameEntity != null && Killer.GameEntity.IsAttached)
			{
				Killer.GameEntity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
			}
		}

		private static void RemoveBuffFromEnemy(EntityId _enemyId)
		{
			EnemyCharacter enemyCharacter = _enemyId.Entity as EnemyCharacter;
			enemyCharacter.Buffs.RemoveBuffs(EBuffType.ThreatVictim, 1);
		}
	}
}
