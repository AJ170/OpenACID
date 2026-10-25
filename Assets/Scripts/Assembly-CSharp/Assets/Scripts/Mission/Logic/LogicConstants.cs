using System;
using AcierProtocol;
using SharedBaseLib.System;

namespace Assets.Scripts.Mission.Logic
{
	public static class LogicConstants
	{
		public enum ComponentPriorities
		{
			Definition = 0,
			UnityComponentCache = 1,
			EntityTracking = 2,
			Effect = 3,
			GameObjectProxy = 4,
			WeaponHandlerCache = 5,
			NavAgentSurrogate = 6,
			StateMachines = 7,
			Patrol = 8,
			CharacterAi = 9,
			SpawnPoint = 10,
			Participant = 11,
			HeadIcon = 12,
			SelectionIcon = 13,
			RooftopgardenClothes = 14,
			InteractableLocators = 15,
			CodexEntry = 16,
			Vision = 17,
			Highlight = 18,
			Projectile = 19,
			PlayerEventHandler = 20,
			AssassinInteraction = 21,
			KillFromHaystack = 22,
			KillFromBench = 23,
			KillFromRooftopGarden = 24,
			CharacterAppearance = 25,
			PigeonryComponent = 26,
			StalkingZoneComponent = 27,
			MissionResult = 28,
			SelectionCollider = 29,
			Escort = 30,
			Captive = 31,
			Rules = 32,
			Skills = 33,
			Buffs = 34,
			EntityStats = 35,
			LogicInterface = 36
		}

		public enum BuffPriorities
		{
			Overwriting = 0,
			NumberMaximum = 1,
			NumberMinimum = 2,
			NumberAdditive = 3,
			NumberMultiplicative = 4,
			BooleanAnd = 5,
			BooleanOr = 6
		}

		public const float PigeonryInteractionDurationSec = 2f;

		public const float AssassinationStealthPatrolSidetrackedDurationSec = 2f;

		public const float HidingInPatrolMinDuration = 1.5f;

		public const float HidingInPatrolRejoinDelay = 1f;

		public const float SelectionRange = 20f;

		public const float EnterCombatPointOfNoReturnDistance = 5f;

		public const float EnterCombatUnsheathWeaponHighProfileDistance = 10f;

		public const float EnterCombatUnsheathWeaponLowProfileDistance = 7f;

		public const float StartCombatDistance = 4f;

		public const float StartCombatDistanceVariation = 3f;

		public const float StartCombatReferenceSpeed = 6.28f;

		public const float ChasingSuccessDistance = 1f;

		public const float ChasingSuccessDistanceSqr = 1f;

		public const float ChasingSuccessDistanceParcour = 4f;

		public const float ChasingSuccessDistanceParcourSqr = 16f;

		public const float InspectionPokingSuccessDistance = 0.7f;

		public const float InspectionPokingSuccessDistanceSqr = 0.48999998f;

		public const float InspectionSuccessDistance = 4f;

		public const float InspectionSuccessDistanceSqr = 16f;

		public const float PushDistance = 0.7f;

		public const float PushDistanceSqr = 0.48999998f;

		public const float PushAbortDistance = 2f;

		public const float PushAbortDistanceSqr = 4f;

		public const float StartTalkDistance = 2f;

		public const float StartDeliveryDistance = 1.5f;

		public const float StartFreeCapturedNpcsDistance = 1f;

		public const float StartHidingDistance = 3f;

		public const float StartOffMeshLinkDistance = 1f;

		public const float ClickDistanceLeaveHotspotAndMoveTo = 2f;

		public const float HideoutSearchRange = 4f;

		public const float MinLeapOfFaithHeight = 2f;

		public const float LeapOfFaithConeStartHeight = 5f;

		public const float LeapOfFaithConeEndHeight = 50f;

		public const float LeapOfFaithConeStartDistance = 10f;

		public const float LeapOfFaithConeEndDistance = 20f;

		public const int MaxSkillRank = 5;

		public const int DeathTurnDuration = 70;

		public const float BenchInactiveAfterAssassinationDurationSec = 10f;

		public const int RecentlyEnemyKilledAfterCombatTurns = 50;

		public const float MaxEnemyAwareDistance = 25f;

		public const int MaxClimbingHitCount = 1;

		public const int TurnUpdateableBucketScaling = 20;

		public const float NearBucketMaxRadius = 10f;

		public const float SkipEntityRatio = 0.5f;

		public const float MinHealthThreshold = 1f;

		public const int TurnUpdateableRegisterOffset = 7;

		public const float SeduceApproachedDistancePatrol = 4f;

		public const float SeduceApproachedDistancePatrolCheck = 1f;

		public const float SeduceApproachedDistanceEnemy = 3f;

		public const float SeduceApproachedDistanceEnemySqr = 9f;

		public const float SeduceRange = 7.9f;

		public const float MaxSeduceDancingDuration = 10f;

		private const float GreedyApproachedDistance = 0.75f;

		public const float GreedyApproachedDistanceSqr = 0.5625f;

		private const float StickyBombApproachedDistance = 0.75f;

		public const float StickyBombApproachedDistanceSqr = 0.5625f;

		public const float StickyBombPanicRunDistance = 2.5f;

		public const float AlertedNotorietyMin = 0.5f;

		public const float ExposedNotorietyThreshold = 0.99f;

		public const float NotorityEqualityThreshold = 0.001f;

		public const float NotorietyMax = 1f;

		private const float ThreatEscortDistanceEnemy = 3.5f;

		public const float ThreatEscortDistanceEnemySqr = 12.25f;

		private const float ThreatCaptiveDistanceEnemy = 2f;

		public const float ThreatCaptiveDistanceEnemySqr = 4f;

		public const int KillTokenCooldownBleeding = 9;

		public static readonly int AutoPassableNavMeshLayerMask;

		public static readonly int[] DetectionRangeValues;

		private static float[] DefaultSpeeds;

		public static bool AreDefaultSpeedsInitialized
		{
			get
			{
				return DefaultSpeeds != null;
			}
		}

		static LogicConstants()
		{
			AutoPassableNavMeshLayerMask = NavMeshLayers.WalkableLayerMaskPreset | NavMeshLayers.ClimbLayerMask | NavMeshLayers.JumpDownLayerMask | NavMeshLayers.BalanceLayerMask | NavMeshLayers.JumpHorizontalLayerMask | NavMeshLayers.RunPassOverLayerMask | NavMeshLayers.RunPassOverDownLayerMask | NavMeshLayers.ParkourSequenceLayerMask;
			DetectionRangeValues = (int[])Enum.GetValues(typeof(EDetectionRange));
		}

		public static float GetDefaultSpeedValue(this EMovementSpeed _speed)
		{
			return DefaultSpeeds[(int)_speed];
		}

		public static void InitializeDefaultSpeeds(AnimationController _animationController)
		{
			if (DefaultSpeeds == null)
			{
				DefaultSpeeds = new float[Enum.GetValues(typeof(EMovementSpeed)).Length];
				SetDefaultSpeed(_animationController, EMovementSpeed.WalkSlow);
				SetDefaultSpeed(_animationController, EMovementSpeed.Walk);
				SetDefaultSpeed(_animationController, EMovementSpeed.WalkFast);
				SetDefaultSpeed(_animationController, EMovementSpeed.Run);
				SetDefaultSpeed(_animationController, EMovementSpeed.Sprint);
			}
		}

		private static void SetDefaultSpeed(AnimationController _animationController, EMovementSpeed _movementSpeed)
		{
			DefaultSpeeds[(int)_movementSpeed] = _animationController.CalculateSpeed(_movementSpeed);
		}
	}
}
