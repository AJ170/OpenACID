using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public class TurnHandler
	{
		public const int LatestGuaranteedTurnAfterStart = 1;

		private int m_currentTurn;

		private float m_lastTurnTime;

		private readonly float m_deltaTurns;

		public int CurrentTurn
		{
			get
			{
				return m_currentTurn;
			}
		}

		public float LastTurnTime
		{
			get
			{
				return m_lastTurnTime;
			}
		}

		private float FrameTime { get; set; }

		public bool IsInUpdate { get; private set; }

		public bool IsInTurnUpdate { get; private set; }

		public TurnHandler(float _deltaTurns)
		{
			m_deltaTurns = _deltaTurns;
		}

		public void Execute()
		{
			Profiling.Start("Turnhandler::Execute", float.MaxValue);
			IsInUpdate = true;
			FrameTime = LogicMain.FrameTime;
			if (LogicMain.CurrentTime - m_lastTurnTime >= m_deltaTurns)
			{
				IsInTurnUpdate = true;
				m_lastTurnTime += m_deltaTurns;
				m_currentTurn++;
			}
			UpdateDistanceBuckets();
			LogicMain.LogicExecutor.UpdateFrame();
			Profiling.Start("Turnhandler::Updating Entities", float.MaxValue);
			if (Main.ProfilingDetailed)
			{
				UpdateEntitiesProf();
			}
			else
			{
				UpdateEntities();
			}
			Profiling.Stop();
			if (IsInTurnUpdate)
			{
				LogicMain.EntityManager.UpdateTurn();
			}
			IsInUpdate = false;
			IsInTurnUpdate = false;
			Profiling.Stop();
		}

		private void UpdateDistanceBuckets()
		{
			Profiling.Start("Update distance buckets", float.MaxValue);
			int gameEntityCount = LogicMain.EntityManager.GameEntityCount;
			Vector3 position = UnityEngine.Camera.main.transform.position;
			float num = LogicMain.GameStats.Get_float(StatTypes.EStatType.Game_MaxDetectionRange);
			for (int i = 0; i < gameEntityCount; i++)
			{
				GameEntity gameEntity = LogicMain.EntityManager.GetGameEntity(i);
				if (gameEntity.IsActive && gameEntity.Id != EntityId.InvalidId && UbiCore.Utils.Singleton<Timer>.Instance.time > gameEntity.DistanceBucketExpireTime)
				{
					float num2 = (gameEntity.Position - position).LengthXZ();
					if (num2 < 10f)
					{
						gameEntity.DistanceBucket = 0f;
						gameEntity.DistanceBucketExpireTime = UbiCore.Utils.Singleton<Timer>.Instance.time + 0.2f;
					}
					else
					{
						gameEntity.DistanceBucket = Math.Min((num2 - 10f) / num, 1f);
						gameEntity.DistanceBucketExpireTime = UbiCore.Utils.Singleton<Timer>.Instance.time + 2f * gameEntity.DistanceBucket;
					}
				}
			}
			Profiling.Stop();
		}

		private void UpdateEntitiesProf()
		{
			int updateableEntityCount = LogicMain.EntityManager.UpdateableEntityCount;
			for (int i = 0; i < updateableEntityCount; i++)
			{
				IEntity updateableEntity = LogicMain.EntityManager.GetUpdateableEntity(i);
				if (!updateableEntity.IsActive || !updateableEntity.IsReadyForUpdate || updateableEntity.Id == EntityId.InvalidId)
				{
					continue;
				}
				GameEntity gameEntity = updateableEntity as GameEntity;
				if (gameEntity != null && !gameEntity.IsAttached && gameEntity.DistanceBucket >= 0.5f && gameEntity.Definition.EntityDefinition.SkipLogicUpdateAllowed)
				{
					continue;
				}
				updateableEntity.SortComponents();
				ReadOnlyCollection<IUpdateableComponent> updateableLogicComponents = updateableEntity.UpdateableLogicComponents;
				int count = updateableLogicComponents.Count;
				for (int j = 0; j < count; j++)
				{
					IUpdateableComponent updateableComponent = updateableLogicComponents[j];
					Profiling.Start(updateableComponent.GetType().Name, float.MaxValue);
					if (updateableComponent.IsActive)
					{
						ITurnUpdateableComponent turnUpdateableComponent = updateableComponent as ITurnUpdateableComponent;
						IFrameUpdateableComponent frameUpdateableComponent = updateableComponent as IFrameUpdateableComponent;
						if (IsInTurnUpdate && updateableEntity.Id != EntityId.InvalidId && turnUpdateableComponent != null && CurrentTurn >= turnUpdateableComponent.NextTurnUpdateTurn)
						{
							turnUpdateableComponent.DoTurnUpdate(turnUpdateableComponent.TurnsSinceTurnUpdate);
						}
						if (updateableEntity.Id != EntityId.InvalidId && frameUpdateableComponent != null && UbiCore.Utils.Singleton<Timer>.Instance.time > frameUpdateableComponent.NextFrameUpdateTime)
						{
							frameUpdateableComponent.DoFrameUpdate(frameUpdateableComponent.TimeSinceFrameUpdate);
						}
					}
					Profiling.Stop();
				}
			}
		}

		private void UpdateEntities()
		{
			int updateableEntityCount = LogicMain.EntityManager.UpdateableEntityCount;
			for (int i = 0; i < updateableEntityCount; i++)
			{
				IEntity updateableEntity = LogicMain.EntityManager.GetUpdateableEntity(i);
				if (!updateableEntity.IsActive || !updateableEntity.IsReadyForUpdate || updateableEntity.Id == EntityId.InvalidId)
				{
					continue;
				}
				GameEntity gameEntity = updateableEntity as GameEntity;
				if (gameEntity != null && (!gameEntity.IsAttached || gameEntity is InteractableObject) && gameEntity.SkipLogicUpdateAllowed && gameEntity.DistanceBucket >= 0.5f && gameEntity.Definition.EntityDefinition.SkipLogicUpdateAllowed)
				{
					continue;
				}
				updateableEntity.SortComponents();
				ReadOnlyCollection<IUpdateableComponent> updateableLogicComponents = updateableEntity.UpdateableLogicComponents;
				int count = updateableLogicComponents.Count;
				for (int j = 0; j < count; j++)
				{
					IUpdateableComponent updateableComponent = updateableLogicComponents[j];
					if (updateableComponent.IsActive)
					{
						IFrameUpdateableComponent frameUpdateableComponent = updateableComponent as IFrameUpdateableComponent;
						ITurnUpdateableComponent turnUpdateableComponent = updateableComponent as ITurnUpdateableComponent;
						if (IsInTurnUpdate && updateableEntity.Id != EntityId.InvalidId && turnUpdateableComponent != null && CurrentTurn >= turnUpdateableComponent.NextTurnUpdateTurn)
						{
							turnUpdateableComponent.DoTurnUpdate(turnUpdateableComponent.TurnsSinceTurnUpdate);
						}
						if (updateableEntity.Id != EntityId.InvalidId && frameUpdateableComponent != null && UbiCore.Utils.Singleton<Timer>.Instance.time > frameUpdateableComponent.NextFrameUpdateTime)
						{
							frameUpdateableComponent.DoFrameUpdate(frameUpdateableComponent.TimeSinceFrameUpdate);
						}
					}
				}
			}
		}
	}
}
