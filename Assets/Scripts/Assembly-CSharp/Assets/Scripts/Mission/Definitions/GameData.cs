using System;
using AssetBundleData;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class GameData : UbiCore.Utils.Singleton<GameData>
	{
		public interface IGameDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IReadonlyStatValueCollection
		{
			float[] NotoritiesContinuous { get; }

			float MaxWaitAfterStareDuration { get; }

			float MaxPushDelay { get; }

			float MinNotorietyInterval { get; }

			float MaxNotorietyInterval { get; }

			float SlopeNotorietyInterval { get; }

			float MinNotorietyDecreaseInterval { get; }

			float MaxNotorietyDecreaseInterval { get; }

			float SlopeNotorietyDecreaseInterval { get; }

			float NotorietyDecreaseDelay { get; }

			float CrowdFleeDistance { get; }

			float CrowdFleeDefaultExtensionTime { get; }

			float CrowdFleeAfterStrikingAssasinationExtensionTime { get; }

			float CrowdFleeFromBombExplosionMinTime { get; }

			float EnemyFleeFromCombatDistance { get; }

			float EnemyFleeFromCombatDuration { get; }

			float EnemyFleeFromCombatViralDistance { get; }

			float EnemyFleeFromCombatViralDuration { get; }

			float EnemyWalkFromCombatDuration { get; }

			float BowArrowSpeed { get; }

			float DefaultDaggerSpeed { get; }

			float NpcFleeFromCaptureMinTime { get; }

			float EnemyChaseSpeedFactor { get; }

			float EnemySearchSweepAngle { get; }

			float KillMoveDeflectDamageMultiplier { get; }
		}

		[Database("GameDefinition")]
		public class GameDefinition : EntityData.EntityDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IGameDefinition, IReadonlyStatValueCollection
		{
			public override Type InstanceType
			{
				get
				{
					return typeof(GameEntity);
				}
			}

			public override EntityTypes.EType EntityType
			{
				get
				{
					return EntityTypes.EType.GameLogic;
				}
			}

			public float[] NotoritiesContinuous { get; private set; }

			public float MaxWaitAfterStareDuration { get; private set; }

			public float MaxPushDelay { get; private set; }

			public float MinNotorietyInterval { get; private set; }

			public float MaxNotorietyInterval { get; private set; }

			public float SlopeNotorietyInterval { get; private set; }

			public float MinNotorietyDecreaseInterval { get; private set; }

			public float MaxNotorietyDecreaseInterval { get; private set; }

			public float SlopeNotorietyDecreaseInterval { get; private set; }

			public float NotorietyDecreaseDelay { get; private set; }

			public float CrowdFleeDistance { get; private set; }

			public float CrowdFleeDefaultExtensionTime { get; private set; }

			public float CrowdFleeAfterStrikingAssasinationExtensionTime { get; private set; }

			public float CrowdFleeFromBombExplosionMinTime { get; private set; }

			public float EnemyFleeFromCombatDistance { get; set; }

			public float EnemyFleeFromCombatDuration { get; private set; }

			public float EnemyFleeFromCombatViralDistance { get; private set; }

			public float EnemyFleeFromCombatViralDuration { get; private set; }

			public float EnemyWalkFromCombatDuration { get; private set; }

			public float CharacterDetectionSearchingAngleOverride { get; private set; }

			public float BowArrowSpeed { get; private set; }

			public float DefaultDaggerSpeed { get; private set; }

			public float NpcFleeFromCaptureMinTime { get; private set; }

			public float EnemyChaseSpeedFactor { get; private set; }

			public float EnemySearchSweepAngle { get; private set; }

			public float KillMoveDeflectDamageMultiplier { get; private set; }

			public GameDefinition(AssetBundleInfo _assetBundleInfo)
				: base(GameStats.StatsHelpers, _assetBundleInfo)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				if (_s.Mode == EMode.EMode_Read)
				{
					NotoritiesContinuous = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.Game_NotoritiesContinuous).GetValuesCopy_float();
					MaxWaitAfterStareDuration = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_MaxWaitAfterStareDuration);
					MaxPushDelay = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_MaxPushDelay);
					MinNotorietyInterval = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_MinNotorietyInterval);
					MaxNotorietyInterval = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_MaxNotorietyInterval);
					SlopeNotorietyInterval = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_SlopeNotorietyInterval);
					MinNotorietyDecreaseInterval = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_MinNotorietyDecreaseInterval);
					MaxNotorietyDecreaseInterval = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_MaxNotorietyDecreaseInterval);
					SlopeNotorietyDecreaseInterval = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_SlopeNotorietyDecreaseInterval);
					NotorietyDecreaseDelay = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_NotorietyDecreaseDelay);
					CrowdFleeDistance = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_CrowdFleeDistance);
					CrowdFleeDefaultExtensionTime = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_CrowdFleeDefaultExtensionTime);
					CrowdFleeAfterStrikingAssasinationExtensionTime = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_CrowdFleeAfterStrikingAssasinationExtensionTime);
					CrowdFleeFromBombExplosionMinTime = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_CrowdFleeFromBombExplosionMinTime);
					EnemyFleeFromCombatDistance = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemyFleeFromCombatDistance);
					EnemyFleeFromCombatDuration = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemyFleeFromCombatDuration);
					EnemyFleeFromCombatViralDistance = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemyFleeFromCombatViralDistance);
					EnemyFleeFromCombatViralDuration = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemyFleeFromCombatViralDuration);
					EnemyWalkFromCombatDuration = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemyWalkFromCombatDuration);
					BowArrowSpeed = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_BowArrowSpeed);
					DefaultDaggerSpeed = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_DefaultDaggerSpeed);
					NpcFleeFromCaptureMinTime = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_NpcFleeFromCaptureMinTime);
					EnemyChaseSpeedFactor = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemyChaseSpeedFactor);
					EnemySearchSweepAngle = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_EnemySearchSweepAngle);
					KillMoveDeflectDamageMultiplier = m_StatValueCollection.Get_float(StatTypes.EStatType.Game_KillMoveDeflectDamageMultiplier);
					if (UbiCore.Utils.Singleton<GameData>.IsAvailable)
					{
						UbiCore.Utils.Singleton<GameData>.Instance.SetGameDefinition(this);
					}
				}
			}
		}

		private GameDefinition m_GameDefinition;

		public IGameDefinition Definition
		{
			get
			{
				return m_GameDefinition;
			}
		}

		private void SetGameDefinition(GameDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_GameDefinition = _definition;
			}
		}
	}
}
