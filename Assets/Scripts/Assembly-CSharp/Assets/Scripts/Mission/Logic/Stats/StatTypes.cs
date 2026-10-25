using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.Formations;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.StateMachines;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public static class StatTypes
	{
		public enum EStatType
		{
			Invalid = -1,
			__Start_Immutable_Rule = 0,
			Rule_SkillDuration = 1,
			Rule_SkillCooldownDuration = 2,
			Rule_SkillCooldownRelativeDurationReduction = 3,
			Rule_AttractionRange = 4,
			Rule_Damage = 5,
			Rule_SkillDamageFactor = 6,
			Rule_AnonymousDamageMultiplier = 7,
			Rule_ThrowRange = 8,
			Rule_ThrowAngle = 9,
			Rule_ThrowTargets = 10,
			Rule_SilencedDuration = 11,
			Rule_StunDuration = 12,
			Rule_ShreddedDuration = 13,
			Rule_ShreddedFactor = 14,
			Rule_KnockdownDuration = 15,
			Rule_KnockdownChance = 16,
			Rule_ApproachMaxDistance = 17,
			Rule_ApproachMinDistance = 18,
			Rule_ApproachStartDistance = 19,
			Rule_ApproachMinHeight = 20,
			Rule_ApproachStartHeight = 21,
			Rule_EffectRadius = 22,
			Rule_BleedDuration = 23,
			Rule_BleedInterval = 24,
			Rule_BleedAmount = 25,
			Rule_BleedAmountRelative = 26,
			Rule_DurationFactor = 27,
			Rule_GreedyDuration = 28,
			Rule_GreedyAttractionRadius = 29,
			Rule_JackBoxStartDelay = 30,
			Rule_SweetMusicDuration = 31,
			Rule_SweetMusicHidingSpots = 32,
			Rule_GlitchHidingSpots = 33,
			Rule_ProjectileSpeed = 34,
			Rule_ProjectileDelay = 35,
			Rule_ProjectileTriggerRadius = 36,
			Rule_ProjectileTriggerDelay = 37,
			Rule_FastFingersStrikeCount = 38,
			Rule_PiercingStrikeCount = 39,
			Rule_PiercingStrikeDamage = 40,
			Rule_PiercingSurpriseArmourFactor = 41,
			Rule_IncognitoBonus = 42,
			Rule_IncognitoBonusDuration = 43,
			Rule_IncognitoFactor = 44,
			Rule_PrecisionBonus = 45,
			Rule_PrecisionFactor = 46,
			Rule_PrecisionFactorBonus = 47,
			Rule_EvasionBonus = 48,
			Rule_EvasionFactor = 49,
			Rule_StrengthBonus = 50,
			Rule_StrengthFactor = 51,
			Rule_CriticalEyeDuration = 52,
			Rule_CriticalChanceBonus = 53,
			Rule_DodgeChanceBonus = 54,
			Rule_StealthModifierBonus = 55,
			Rule_ChanceFactor = 56,
			Rule_CriticalMultiplierBonus = 57,
			Rule_CriticalMultiplierFactor = 58,
			Rule_CriticalDamageBonus = 59,
			Rule_ArmourRatingBonus = 60,
			Rule_ArmourRatingBonusDuration = 61,
			Rule_ArmourRatingFactor = 62,
			Rule_KillTokenChance = 63,
			Rule_FrightenedDuration = 64,
			Rule_FrightenedEnemyCount = 65,
			Rule_FrightenedAttackDelayFactor = 66,
			Rule_FrightenedFearsomeFactor = 67,
			Rule_RetrieveHealthChance = 68,
			Rule_RetrieveHealthAmount = 69,
			Rule_RetrieveHealthRelative = 70,
			Rule_MaxHealthBonus = 71,
			Rule_MaxHealthFactor = 72,
			Rule_BlindedDuration = 73,
			Rule_NotorietyIncreaseDelay = 74,
			Rule_NotorietyDecrease = 75,
			Rule_FxGuid = 76,
			Rule_HirelingLevelFactor_1 = 77,
			Rule_HirelingLevelFactor_2 = 78,
			Rule_HirelingLevelFactor_3 = 79,
			Rule_HirelingLevelFactor_4 = 80,
			Rule_HirelingLevelFactor_1_Links = 81,
			Rule_HirelingLevelFactor_2_Links = 82,
			Rule_HirelingLevelFactor_3_Links = 83,
			Rule_HirelingLevelFactor_4_Links = 84,
			Rule_ParameterFactor = 85,
			Rule_FearsomeFactor = 86,
			Rule_FearsomeFactorBonus = 87,
			Rule_FearsomeFactorFactor = 88,
			Rule_WeaponDamageBonus = 89,
			Rule_WeaponDamageDeflectBonus = 90,
			Rule_WeaponDamageFactor = 91,
			Rule_DamageFactor = 92,
			Rule_ArmorPiercingFlatBonus = 93,
			Rule_ArmourDamageReductionBonus = 94,
			Rule_ConfusedDuration = 95,
			Rule_FeintBonus = 96,
			Rule_PickpocketingChance = 97,
			Rule_PickpocketingFactor = 98,
			Rule_FallingDamageReductionFactor = 99,
			Rule_AssassinClassType = 100,
			Rule_InitAnonymousDamageMultiplierBonus = 101,
			Rule_ItemModWeight = 102,
			Rule_GunpowderDamageBonusFactor = 103,
			__End_Immutable_Rule = 104,
			__Start_Immutable_Game = 105,
			Game_NotoritiesContinuous = 106,
			Game_MaxWaitAfterStareDuration = 107,
			Game_MaxPushDelay = 108,
			Game_NotorietyDecreaseDelay = 109,
			Game_MinNotorietyInterval = 110,
			Game_MaxNotorietyInterval = 111,
			Game_SlopeNotorietyInterval = 112,
			Game_MinNotorietyDecreaseInterval = 113,
			Game_MaxNotorietyDecreaseInterval = 114,
			Game_SlopeNotorietyDecreaseInterval = 115,
			Game_CrowdFleeDistance = 116,
			Game_CrowdFleeDefaultExtensionTime = 117,
			Game_CrowdFleeAfterStrikingAssasinationExtensionTime = 118,
			Game_CrowdFleeFromBombExplosionMinTime = 119,
			Game_EnemyFleeFromCombatDistance = 120,
			Game_EnemyFleeFromCombatDuration = 121,
			Game_EnemyFleeFromCombatViralDistance = 122,
			Game_EnemyFleeFromCombatViralDuration = 123,
			Game_EnemyWalkFromCombatDuration = 124,
			Game_BowArrowSpeed = 125,
			Game_DefaultDaggerSpeed = 126,
			Game_NpcFleeFromCaptureMinTime = 127,
			Game_EnemyChaseSpeedFactor = 128,
			Game_EnemySearchSweepAngle = 129,
			Game_KillMoveDeflectDamageMultiplier = 130,
			__End_Immutable_Game = 131,
			__Start_Immutable_Patrol = 132,
			Patrol_FormationType = 133,
			Patrol_MaxCount = 134,
			Patrol_MaxExtend = 135,
			Patrol_MaxRandomDeviation = 136,
			Patrol_InteractionReason = 137,
			Patrol_FixedLeaderSlot = 138,
			__End_Immutable_Patrol = 139,
			__Start_Immutable_MapGenerator = 140,
			MapGenerator_Age = 141,
			MapGenerator_MapWeight = 142,
			MapGenerator_UnlockLevel = 143,
			__End_Immutable_MapGenerator = 144,
			__Start_Immutable_Character = 145,
			Character_Rules = 146,
			Character_Skills = 147,
			Character_DetectionCastOffset = 148,
			Character_DefaultProjectileSpawnOffset = 149,
			Character_DisplayNameLocaKeys = 150,
			Character_PortraitDisplayNameLocaKeys = 151,
			__End_Immutable_Character = 152,
			__Start_Immutable_CharacterConflict = 153,
			Character_Conflict_BaseHealth = 154,
			Character_Conflict_BaseStrength = 155,
			Character_Conflict_BasePrecision = 156,
			Character_Conflict_BaseEvasion = 157,
			Character_Conflict_BaseWeaponDamage = 158,
			Character_Conflict_BaseWeaponDeflect = 159,
			Character_Conflict_BaseArmourRating = 160,
			Character_Conflict_BaseCriticalMultiplier = 161,
			Character_Conflict_BaseCourage = 162,
			Character_Conflict_BaseSchedulerPriorityInitial = 163,
			Character_Conflict_BaseSchedulerPriorityMultiplier = 164,
			Character_Conflict_BaseSchedulerCooldown = 165,
			Character_Conflict_BaseAttackWarningDuration = 166,
			Character_Conflict_BaseRangedWeaponDamage = 167,
			Character_Conflict_BaseRangedChargingDuration = 168,
			Character_Conflict_BaseRangedChargingResetDuration = 169,
			Character_Conflict_BaseRangedFirstShotDuration = 170,
			Character_Conflict_MinStrikeDamage = 171,
			Character_Conflict_StrengthDamageOverride = 172,
			Character_Conflict_StrengthDamageReductionOverride = 173,
			Character_Conflict_ArmourDamageReductionOverride = 174,
			Character_Conflict_DodgeChanceOverride = 175,
			Character_Conflict_CriticalChanceOverride = 176,
			Character_Conflict_ArmourPiercingFlatOverride = 177,
			Character_Conflict_FeintOverride = 178,
			__End_Immutable_CharacterConflict = 179,
			__Start_Immutable_Npc = 180,
			Npc_BehaviorTreePath = 181,
			Npc_CombatBehaviorTreePath = 182,
			Npc_InteractionType = 183,
			Npc_InteractionIcon = 184,
			Npc_IgnoreCoins = 185,
			Npc_CanDoParcour = 186,
			Npc_RunSpeedOverwrite = 187,
			__End_Immutable_Npc = 188,
			__Start_Immutable_Enemy = 189,
			Enemy_ThreatLevel = 190,
			Enemy_HidingSpotInspectionDistance = 191,
			Enemy_HidingSpotInspectionMask = 192,
			Enemy_PickpocketingCoins = 193,
			__End_Immutable_Enemy = 194,
			__Start_Immutable_CharacterDetection = 195,
			CharacterDetection_DetectionDistances = 196,
			CharacterDetection_DetectionAngles = 197,
			CharacterDetection_DetectionAnglesVertical = 198,
			CharacterDetection_CloseRangeAudioDistance = 199,
			CharacterDetection_CloseRangeFactorStrikingAssassination = 200,
			CharacterDetection_CloseRangeFactorStealthAssassination = 201,
			CharacterDetection_CloseRangeFactorCombat = 202,
			CharacterDetection_CloseRangeFactorStalking = 203,
			CharacterDetection_CloseRangeDistanceStalking = 204,
			CharacterDetection_NotoritiesContinuous = 205,
			CharacterDetection_NotoritiesInteractions = 206,
			CharacterDetection_StayAlarmedMinTime = 207,
			CharacterDetection_MomentOfChaosTime = 208,
			__End_Immutable_CharacterDetection = 209,
			__Start_Immutable_Player = 210,
			Player_BaseIncognito = 211,
			Player_Level = 212,
			Player_ItemPower = 213,
			Player_StalkingZoneEnterDurationSecBase = 214,
			Player_StalkingZoneEnterDurationSecOffset = 215,
			Player_KillTokenRequiredForKill = 216,
			Player_BaseFearsomeFactor = 217,
			Player_Invincible = 218,
			__End_Immutable_Player = 219,
			__Start_Mutable_Game = 220,
			Game_RecentlyKilledEnemyTurn = 221,
			Game_RecentlyKilledEnemyCount = 222,
			Game_MaxDetectionRange = 223,
			__End_Mutable_Game = 224,
			__Start_Mutable_Entity = 225,
			Entity_MapIconType = 226,
			Entity_DisplayNameFallback = 227,
			Entity_DisplayNameLocaKey = 228,
			Entity_FadeMode = 229,
			Entity_FadeDuration = 230,
			__End_Mutable_Entity = 231,
			__Start_Mutable_Patrol = 232,
			Patrol_Enabled = 233,
			__End_Mutable_Patrol = 234,
			__Start_Mutable_Character = 235,
			Character_BirthTurn = 236,
			Character_DeathTurn = 237,
			Character_PortraitDisplayNameLocaKey = 238,
			__End_Mutable_Character = 239,
			__Start_Mutable_Npc = 240,
			Npc_PatrolId = 241,
			Npc_AnimatorNpcId = 242,
			Npc_DidSeeBombExplosion = 243,
			Npc_FleeSpeed = 244,
			Npc_FleeSourcePosition = 245,
			__End_Mutable_Npc = 246,
			__Start_Mutable_MissionCharacter = 247,
			MissionCharacter_FleeOnTransgression = 248,
			MissionCharacter_FleePosition = 249,
			MissionCharacter_MaxNotoriety = 250,
			MissionCharacter_CurrentNotoriety = 251,
			__End_Mutable_MissionCharacter = 252,
			__Start_Mutable_Interaction = 253,
			Interaction_Triggered = 254,
			Interaction_IsRelevant = 255,
			__End_Mutable_Interaction = 256,
			__Start_Mutable_CharacterConspicuous = 257,
			Character_Conspicuous_Notoriety = 258,
			__End_Mutable_CharacterConspicuous = 259,
			__Start_Mutable_Player = 260,
			Player_KillTokenModeActive = 261,
			Player_KillTokenCount = 262,
			Player_KillTokenDecayClock = 263,
			Player_ProtectedFromDeath = 264,
			Player_ClimbingHitCount = 265,
			Player_LastFallingHeight = 266,
			Player_LastTurnKillTokenFromBleeding = 267,
			Player_SprintInHighProfileOverride = 268,
			Player_MovementPrevented = 269,
			Player_MovementPreventedException = 270,
			Player_TotalDamage = 271,
			__End_Mutable_Player = 272,
			__Start_Mutable_CharacterConflict = 273,
			Character_Conflict_Health = 274,
			Character_Conflict_AboutToDie = 275,
			Character_Conflict_CurrentSourceCharacter = 276,
			Character_Conflict_CurrentTargetCharacter = 277,
			Character_Conflict_CurrentDamageFlags = 278,
			Character_Conflict_KillTypeFlags = 279,
			__End_Mutable_CharacterConflict = 280,
			__Start_Mutable_Enemy = 281,
			Enemy_IsDisguised = 282,
			__End_Mutable_Enemy = 283,
			__Start_Mutable_Projectile = 284,
			Projectile_FlightType = 285,
			Projectile_SourcePosition = 286,
			Projectile_TargetId = 287,
			Projectile_TargetDodges = 288,
			Projectile_Speed = 289,
			Projectile_Delay = 290,
			Projectile_TriggerRadius = 291,
			Projectile_TriggerDelay = 292,
			__End_Mutable_Projectile = 293,
			__Start_Mutable_StaticObject = 294,
			__End_Mutable_StaticObject = 295,
			__Start_Computed_Character = 296,
			__End_Computed_Character = 297,
			__Start_Computed_CharacterConflict = 298,
			Character_Conflict_ActualMaxHealth = 299,
			Character_Conflict_ActualStrength = 300,
			Character_Conflict_ActualPrecision = 301,
			Character_Conflict_ActualEvasion = 302,
			Character_Conflict_ActualArmourRating = 303,
			Character_Conflict_ActualWeaponDamage = 304,
			Character_Conflict_ActualWeaponDeflect = 305,
			Character_Conflict_ActualCriticalMultiplier = 306,
			Character_Conflict_ActualDamageMultiplier = 307,
			Character_Conflict_ActualCourage = 308,
			Character_Conflict_BleedingDamage = 309,
			Character_Conflict_BleedingDamageRelative = 310,
			Character_Conflict_FastFingersStrikeCount = 311,
			Character_Conflict_PiercingStrikeCount = 312,
			Character_Conflict_PiercingSurpriseArmourFactor = 313,
			Character_Conflict_PrecisionFactorShredded = 314,
			Character_Conflict_ArmourDamageReduction = 315,
			Character_Conflict_ArmourPiercingFlat = 316,
			Character_Conflict_ArmourPiercingFactor = 317,
			Character_Conflict_Feint = 318,
			Character_Conflict_AttackChance = 319,
			Character_Conflict_DodgeChance = 320,
			Character_Conflict_CriticalChance = 321,
			Character_Conflict_CriticalDamageBonus = 322,
			Character_Conflict_GunpowderDamageBonusFactor = 323,
			Character_Conflict_ActualRangedlWeaponDamage = 324,
			Character_Conflict_WasPickpocketed = 325,
			Character_Conflict_InitAnonymousBonus = 326,
			Character_Conflict_InitAnonymousDamageMultiplier = 327,
			Character_Conflict_SkillDamageBonusFactor = 328,
			__End_Computed_CharacterConflict = 329,
			__Start_Computed_CharacterCondition = 330,
			Character_Condition_IsStunned = 331,
			Character_Condition_IsSmokeBombed = 332,
			Character_Condition_IsSanded = 333,
			Character_Condition_IsKnockdowned = 334,
			Character_Condition_IsSeduced = 335,
			Character_Condition_IsGreedy = 336,
			Character_Condition_IsJackBoxed = 337,
			Character_Condition_IsStickyBombPaniced = 338,
			Character_Condition_IsThreateningVictim = 339,
			Character_Condition_IsConfused = 340,
			Character_Condition_IsLuredBySweetMusic = 341,
			Character_Condition_IsFleeing = 342,
			__End_Computed_CharacterCondition = 343,
			__Start_Computed_CharacterDetection = 344,
			Character_Detection_Range = 345,
			Character_Detection_Distance = 346,
			Character_Detection_DeltaAngle = 347,
			Character_Detection_DeltaAngleVertical = 348,
			Character_Detection_NormalRangeEnterTurn = 349,
			Character_Detection_CloseRangeEnterTurn = 350,
			__End_Computed_CharacterDetection = 351,
			__Start_Computed_CharacterConspicuous = 352,
			Character_Conspicuous_NotorietyIncreaseDelay = 353,
			__End_Computed_CharacterConspicuous = 354,
			__Start_Computed_Enemy = 355,
			Enemy_Silenced = 356,
			Enemy_AttackDelayFactor = 357,
			Enemy_FearsomeFactor = 358,
			__End_Computed_Enemy = 359,
			__Start_Computed_Player = 360,
			Player_AwareEnemies = 361,
			Player_ChasingEnemies = 362,
			Player_IsInRestrictedArea = 363,
			Player_ClosestEnemyThreateningOrNormal = 364,
			Player_ClosestEnemyDistance = 365,
			Player_ClosestEnemyDistanceSqr = 366,
			Player_EagleVision = 367,
			Player_Disguise = 368,
			Player_StealthModifier = 369,
			Player_EnemyWaitAfterStareDuration = 370,
			Player_EnemyPushDelay = 371,
			Player_NotorietyInterval = 372,
			Player_NotorietyDecreaseInterval = 373,
			Player_NotorietyDecreaseDelay = 374,
			Player_ActualIncognito = 375,
			Player_EnemyEscapeKillTokenChance = 376,
			Player_GlitchHidingSpots = 377,
			Player_IsHiding = 378,
			Player_ActualFearsomeFactor = 379,
			Player_DoubleIdentityActive = 380,
			Player_FallingDamageReductionFactor = 381,
			Player_WasJustRevived = 382,
			Player_Invulnerable = 383,
			__End_Computed_Player = 384,
			__Start_Computed_Game = 385,
			__End_Computed_Game = 386,
			Count = 387
		}

		public class StatsRange
		{
			public readonly EStatType Start = EStatType.Invalid;

			public readonly EStatType End = EStatType.Invalid;

			public bool IsEmpty
			{
				get
				{
					return Start == End;
				}
			}

			public StatsRange()
			{
			}

			public StatsRange(EStatType _start, EStatType _end)
			{
				Start = _start;
				End = _end;
			}

			public bool Contains(EStatType _type)
			{
				return _type > Start && _type < End;
			}
		}

		public enum EStatCategory
		{
			Invalid = 0,
			Immutable = 1,
			Mutable = 2,
			Computed = 3
		}

		public enum EStatUpdateType
		{
			Invalid = 0,
			Turnwise = 1,
			Framewise = 2,
			Explicit = 3
		}

		public enum EStatUnit
		{
			Invalid = 0,
			Meters = 1,
			Seconds = 2,
			Degree = 3,
			Health = 4,
			Turns = 5,
			Chance = 6,
			Factor = 7,
			FactorBonus = 8,
			MetersPerSecond = 9
		}

		private struct StaticStatProperties
		{
			public EStatCategory Category;

			public EStatUpdateType UpdateType;

			public bool FireEventOnChange;

			public bool ForcedExport;

			public bool Scaled;

			public EStatUnit Unit;
		}

		private static readonly BBList<int> s_statTypeValues;

		private static readonly StaticStatProperties[] s_statTypeProperties;

		private static readonly IStatValueBase[] s_statValueTemplates;

		public static bool IsInitialized { get; private set; }

		public static ReadOnlyCollection<int> StatTypeValues { get; private set; }

		public static ReadOnlyCollection<IStatValueBase> StatValueTemplates { get; private set; }

		static StatTypes()
		{
			s_statTypeValues = new BBList<int>();
			s_statTypeProperties = new StaticStatProperties[387];
			s_statValueTemplates = new IStatValueBase[387];
			Initialize();
		}

		public static EStatUnit GetStatUnit(EStatType _type)
		{
			return s_statTypeProperties[(int)_type].Unit;
		}

		public static EStatCategory GetStatCategory(EStatType _type)
		{
			return s_statTypeProperties[(int)_type].Category;
		}

		public static EStatUpdateType GetStatUpdateType(EStatType _type)
		{
			return s_statTypeProperties[(int)_type].UpdateType;
		}

		public static bool GetFireEventOnChange(EStatType _type)
		{
			return s_statTypeProperties[(int)_type].FireEventOnChange;
		}

		public static bool GetIsScaled(EStatType _type)
		{
			return s_statTypeProperties[(int)_type].Scaled;
		}

		public static bool GetForcedExport(EStatType _type)
		{
			return s_statTypeProperties[(int)_type].ForcedExport;
		}

		public static bool IsImmutable(EStatType _type)
		{
			return GetStatCategory(_type) == EStatCategory.Immutable;
		}

		public static bool IsMutable(EStatType _type)
		{
			return GetStatCategory(_type) == EStatCategory.Mutable;
		}

		public static bool IsComputed(EStatType _type)
		{
			return GetStatCategory(_type) == EStatCategory.Computed;
		}

		public static bool IsStatCollectionValue(EStatType _type)
		{
			return StatValueTemplates[(int)_type] is IStatCollectionValue;
		}

		private static StaticStatProperties GetStatProperties(EStatType _statType)
		{
			return s_statTypeProperties[(int)_statType];
		}

		private static void InitStatValueTemplate(IStatValueBase _value, EStatUnit _unit, EStatCategory _category, EStatUpdateType _updateType)
		{
			InitStatValueTemplate(_value, _unit, _category, _updateType, false, false);
		}

		private static void InitStatValueTemplate(IStatValueBase _value, EStatUnit _unit, EStatCategory _category, EStatUpdateType _updateType, bool _forcedExport)
		{
			InitStatValueTemplate(_value, _unit, _category, _updateType, _forcedExport, false);
		}

		private static void InitStatValueTemplate(IStatValueBase _value, EStatUnit _unit, EStatCategory _category, EStatUpdateType _updateType, bool _forcedExport, bool _fireEventOnChange)
		{
			InitStatValueTemplate(_value, _unit, _category, _updateType, _forcedExport, _fireEventOnChange, false);
		}

		private static void InitStatValueTemplate(IStatValueBase _value, EStatUnit _unit, EStatCategory _category, EStatUpdateType _updateType, bool _forcedExport, bool _fireEventOnChange, bool _scaled)
		{
			Assets.Scripts.Engine.Helpers.Verify(s_statValueTemplates[(int)_value.StatType] == null, "Stat value template {0} is initialized more than once!", _value.StatType);
			s_statValueTemplates[(int)_value.StatType] = _value;
			s_statTypeProperties[(int)_value.StatType] = new StaticStatProperties
			{
				Category = _category,
				UpdateType = _updateType,
				FireEventOnChange = _fireEventOnChange,
				ForcedExport = _forcedExport,
				Scaled = _scaled,
				Unit = _unit
			};
		}

		public static void Initialize()
		{
			if (IsInitialized)
			{
				return;
			}
			StatTypeValues = new ReadOnlyCollection<int>(s_statTypeValues);
			Array values = Enum.GetValues(typeof(EStatType));
			for (int i = 0; i < values.Length - 1; i++)
			{
				object value = values.GetValue(i);
				if (!value.ToString().StartsWith("__Start") && !value.ToString().StartsWith("__End"))
				{
					s_statTypeValues.Add((int)value);
				}
			}
			StatValueTemplates = new ReadOnlyCollection<IStatValueBase>(s_statValueTemplates);
			EnumStatValueWrapper<EFormationType>.Initialize(typeof(EFormationType));
			new EnumStatValueWrapper<EFormationType>();
			EnumStatValueWrapper<EInteractionReason>.Initialize(typeof(EInteractionReason));
			new EnumStatValueWrapper<EInteractionReason>();
			EnumStatValueWrapper<NpcData.NpcDefinition.ENpcInteractionType>.Initialize(typeof(NpcData.NpcDefinition.ENpcInteractionType));
			new EnumStatValueWrapper<NpcData.NpcDefinition.ENpcInteractionType>();
			EnumStatValueWrapper<IconMaterial>.Initialize(typeof(IconMaterial));
			new EnumStatValueWrapper<IconMaterial>();
			EnumStatValueWrapper<EHidingSpotFlags>.Initialize(typeof(EHidingSpotFlags));
			new EnumStatValueWrapper<EHidingSpotFlags>();
			EnumStatValueWrapper<EMapIconType>.Initialize(typeof(EMapIconType));
			new EnumStatValueWrapper<EMapIconType>();
			EnumStatValueWrapper<EDetectionRange>.Initialize(typeof(EDetectionRange));
			new EnumStatValueWrapper<EDetectionRange>();
			EnumStatValueWrapper<EApplyDamageFlags>.Initialize(typeof(EApplyDamageFlags));
			new EnumStatValueWrapper<EApplyDamageFlags>();
			EnumStatValueWrapper<EKillType>.Initialize(typeof(EKillType));
			new EnumStatValueWrapper<EKillType>();
			EnumStatValueWrapper<EMovementSpeed>.Initialize(typeof(EMovementSpeed));
			new EnumStatValueWrapper<EMovementSpeed>();
			EnumStatValueWrapper<EProjectileFlightType>.Initialize(typeof(EProjectileFlightType));
			new EnumStatValueWrapper<EProjectileFlightType>();
			EnumStatValueWrapper<EStatType>.Initialize(typeof(EStatType));
			new EnumStatValueWrapper<EStatType>();
			EnumStatValueWrapper<EEra>.Initialize(typeof(EEra));
			new EnumStatValueWrapper<EEra>();
			EnumStatValueWrapper<Assassin.EClassTypeId>.Initialize(typeof(Assassin.EClassTypeId));
			new EnumStatValueWrapper<Assassin.EClassTypeId>();
			InitStatValueTemplate(new StatEnumMapValue_Float<ENotorietyGameContinuous>(EStatType.Game_NotoritiesContinuous, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MaxWaitAfterStareDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MaxPushDelay, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_NotorietyDecreaseDelay, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MinNotorietyInterval, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MaxNotorietyInterval, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_SlopeNotorietyInterval, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MinNotorietyDecreaseInterval, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MaxNotorietyDecreaseInterval, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_SlopeNotorietyDecreaseInterval, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_CrowdFleeDistance, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_CrowdFleeDefaultExtensionTime, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_CrowdFleeAfterStrikingAssasinationExtensionTime, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_CrowdFleeFromBombExplosionMinTime, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemyFleeFromCombatDistance, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemyFleeFromCombatDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemyFleeFromCombatViralDistance, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemyFleeFromCombatViralDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemyWalkFromCombatDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_BowArrowSpeed, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_DefaultDaggerSpeed, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_NpcFleeFromCaptureMinTime, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemyChaseSpeedFactor, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_EnemySearchSweepAngle, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_KillMoveDeflectDamageMultiplier, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EFormationType>>(EStatType.Patrol_FormationType, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Patrol_MaxCount, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Patrol_MaxExtend, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Patrol_MaxRandomDeviation, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EInteractionReason>>(EStatType.Patrol_InteractionReason, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Patrol_FixedLeaderSlot, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_String>(EStatType.Npc_BehaviorTreePath, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_String>(EStatType.Npc_CombatBehaviorTreePath, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<NpcData.NpcDefinition.ENpcInteractionType>>(EStatType.Npc_InteractionType, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<IconMaterial>>(EStatType.Npc_InteractionIcon, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Npc_IgnoreCoins, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Npc_CanDoParcour, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Npc_RunSpeedOverwrite, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Enemy_ThreatLevel, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Enemy_HidingSpotInspectionDistance, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EHidingSpotFlags>>(EStatType.Enemy_HidingSpotInspectionMask, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Enemy_PickpocketingCoins, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatEnumMapValueScalable_Float<EDetectionRange>(EStatType.CharacterDetection_DetectionDistances, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatEnumMapValueScalable_Float<EDetectionRange>(EStatType.CharacterDetection_DetectionAngles, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatEnumMapValueScalable_Float<EDetectionRange>(EStatType.CharacterDetection_DetectionAnglesVertical, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.CharacterDetection_CloseRangeAudioDistance, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.CharacterDetection_CloseRangeFactorStrikingAssassination, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.CharacterDetection_CloseRangeFactorStealthAssassination, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.CharacterDetection_CloseRangeFactorCombat, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.CharacterDetection_CloseRangeFactorStalking, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.CharacterDetection_CloseRangeDistanceStalking, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatEnumMapValueScalable_Float<ENotorietyNpcContinuous>(EStatType.CharacterDetection_NotoritiesContinuous, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatEnumMapValueScalable_Float<EInteractionReason>(EStatType.CharacterDetection_NotoritiesInteractions, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.CharacterDetection_StayAlarmedMinTime, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.CharacterDetection_MomentOfChaosTime, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<EnumStatValueWrapper<EMapIconType>>(EStatType.Entity_MapIconType, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, true);
			InitStatValueTemplate(new StatValue<StatValueWrapper_String>(EStatType.Entity_DisplayNameFallback, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Entity_DisplayNameLocaKey, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Entity_FadeMode, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Entity_FadeDuration, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Patrol_Enabled, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_BirthTurn, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_DeathTurn, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_PortraitDisplayNameLocaKey, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Interaction_Triggered, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Interaction_IsRelevant, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_AwareEnemies, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_ChasingEnemies, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_IsInRestrictedArea, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_EntityId>(EStatType.Player_ClosestEnemyThreateningOrNormal, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_ClosestEnemyDistance, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_ClosestEnemyDistanceSqr, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_EagleVision, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_Disguise, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_StealthModifier, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_EnemyWaitAfterStareDuration, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_EnemyPushDelay, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_NotorietyInterval, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_NotorietyDecreaseInterval, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_NotorietyDecreaseDelay, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_ActualIncognito, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_EnemyEscapeKillTokenChance, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EHidingSpotFlags>>(EStatType.Player_GlitchHidingSpots, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_IsHiding, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_ActualFearsomeFactor, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_DoubleIdentityActive, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_FallingDamageReductionFactor, null), EStatUnit.Factor, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_WasJustRevived, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_Invulnerable, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseHealth, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseStrength, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BasePrecision, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseEvasion, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseWeaponDamage, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseWeaponDeflect, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseArmourRating, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseCriticalMultiplier, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseCourage, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseSchedulerPriorityInitial, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseSchedulerPriorityMultiplier, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseSchedulerCooldown, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseAttackWarningDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_BaseRangedWeaponDamage, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, false, false, true);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseRangedChargingDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseRangedChargingResetDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BaseRangedFirstShotDuration, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_MinStrikeDamage, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_StrengthDamageOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_StrengthDamageReductionOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_ArmourDamageReductionOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_DodgeChanceOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_CriticalChanceOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_ArmourPiercingFlatOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapperScalable_Float>(EStatType.Character_Conflict_FeintOverride, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.MissionCharacter_FleeOnTransgression, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Vector3>(EStatType.MissionCharacter_FleePosition, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.MissionCharacter_MaxNotoriety, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.MissionCharacter_CurrentNotoriety, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<StatValueWrapper_String>(EStatType.Character_Rules, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<StatValueWrapper_String>(EStatType.Character_Skills, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Vector3>(EStatType.Character_DetectionCastOffset, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<StatValueWrapper_Int>(EStatType.Character_DisplayNameLocaKeys, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<StatValueWrapper_Int>(EStatType.Character_PortraitDisplayNameLocaKeys, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Vector3>(EStatType.Character_DefaultProjectileSpawnOffset, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_BaseIncognito, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_StalkingZoneEnterDurationSecBase, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_StalkingZoneEnterDurationSecOffset, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_KillTokenRequiredForKill, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_Level, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_ItemPower, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_BaseFearsomeFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_Invincible, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Game_RecentlyKilledEnemyTurn, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Game_RecentlyKilledEnemyCount, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Game_MaxDetectionRange, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueClamped<StatValueWrapper_Float>(EStatType.Character_Conflict_Health, null, new StatValueConstantThresholdProvider<StatValueWrapper_Float>(new StatValueWrapper_Float
			{
				Float = 0f
			}), new StatValueStatThresholdProvider<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualMaxHealth)), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, false);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Character_Conflict_AboutToDie, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_EntityId>(EStatType.Character_Conflict_CurrentSourceCharacter, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_EntityId>(EStatType.Character_Conflict_CurrentTargetCharacter, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EApplyDamageFlags>>(EStatType.Character_Conflict_CurrentDamageFlags, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EKillType>>(EStatType.Character_Conflict_KillTypeFlags, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Enemy_IsDisguised, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualMaxHealth, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit, false, true);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualStrength, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualPrecision, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualEvasion, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualWeaponDamage, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualWeaponDeflect, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualArmourRating, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualCriticalMultiplier, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualDamageMultiplier, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualCourage, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ActualRangedlWeaponDamage, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_AttackChance, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_DodgeChance, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_CriticalChance, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_CriticalDamageBonus, null), EStatUnit.Health, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_GunpowderDamageBonusFactor, null), EStatUnit.Factor, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Conflict_FastFingersStrikeCount, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Conflict_PiercingStrikeCount, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_PiercingSurpriseArmourFactor, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_PrecisionFactorShredded, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BleedingDamage, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_BleedingDamageRelative, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Character_Conflict_WasPickpocketed, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Character_Conflict_InitAnonymousBonus, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_InitAnonymousDamageMultiplier, null), EStatUnit.Factor, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_SkillDamageBonusFactor, null), EStatUnit.Factor, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ArmourDamageReduction, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ArmourPiercingFlat, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_ArmourPiercingFactor, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conflict_Feint, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsStunned, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsSmokeBombed, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsSanded, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsKnockdowned, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsSeduced, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsGreedy, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsJackBoxed, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsStickyBombPaniced, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsThreateningVictim, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsConfused, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsLuredBySweetMusic, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Condition_IsFleeing, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Explicit);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EDetectionRange>>(EStatType.Character_Detection_Range, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Detection_Distance, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Detection_DeltaAngle, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Detection_DeltaAngleVertical, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Detection_NormalRangeEnterTurn, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Character_Detection_CloseRangeEnterTurn, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValueClampedByConstants<StatValueWrapper_Float>(EStatType.Character_Conspicuous_Notoriety, null, new StatValueWrapper_Float
			{
				Float = 0f
			}, new StatValueWrapper_Float
			{
				Float = 1f
			}), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, true);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Character_Conspicuous_NotorietyIncreaseDelay, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapper_EntityId>(EStatType.Npc_PatrolId, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, true);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Npc_AnimatorNpcId, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, false);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Npc_DidSeeBombExplosion, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, false);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EMovementSpeed>>(EStatType.Npc_FleeSpeed, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, false);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Vector3>(EStatType.Npc_FleeSourcePosition, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, false);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Enemy_Silenced, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Enemy_AttackDelayFactor, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Enemy_FearsomeFactor, null), EStatUnit.Invalid, EStatCategory.Computed, EStatUpdateType.Turnwise);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_KillTokenModeActive, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_KillTokenCount, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_KillTokenDecayClock, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_ProtectedFromDeath, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_ClimbingHitCount, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_LastFallingHeight, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_LastTurnKillTokenFromBleeding, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Player_SprintInHighProfileOverride, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapper_Bool>(EStatType.Player_MovementPrevented, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid, false, true);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Player_MovementPreventedException, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Player_TotalDamage, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EProjectileFlightType>>(EStatType.Projectile_FlightType, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Vector3>(EStatType.Projectile_SourcePosition, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_EntityId>(EStatType.Projectile_TargetId, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Bool>(EStatType.Projectile_TargetDodges, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Projectile_Speed, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Projectile_Delay, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Projectile_TriggerRadius, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Projectile_TriggerDelay, null), EStatUnit.Invalid, EStatCategory.Mutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_SkillDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_SkillCooldownDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_SkillCooldownRelativeDurationReduction, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_AttractionRange, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_Damage, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_SkillDamageFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_StunDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_ShreddedDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ShreddedFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_KnockdownDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_KnockdownChance, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_AnonymousDamageMultiplier, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ThrowRange, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ThrowAngle, null), EStatUnit.Degree, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_ThrowTargets, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_SilencedDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ApproachMaxDistance, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ApproachMinDistance, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ApproachStartDistance, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ApproachMinHeight, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ApproachStartHeight, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_EffectRadius, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_BleedDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_BleedInterval, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_BleedAmount, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_BleedAmountRelative, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_DurationFactor, null), EStatUnit.FactorBonus, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_GreedyDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_GreedyAttractionRadius, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_JackBoxStartDelay, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_SweetMusicDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EHidingSpotFlags>>(EStatType.Rule_SweetMusicHidingSpots, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<EHidingSpotFlags>>(EStatType.Rule_GlitchHidingSpots, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ProjectileSpeed, null), EStatUnit.MetersPerSecond, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_ProjectileDelay, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ProjectileTriggerRadius, null), EStatUnit.Meters, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_ProjectileTriggerDelay, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_FastFingersStrikeCount, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_PiercingStrikeCount, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PiercingStrikeDamage, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PiercingSurpriseArmourFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_IncognitoBonus, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_IncognitoBonusDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_IncognitoFactor, null), EStatUnit.FactorBonus, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PrecisionBonus, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PrecisionFactor, null), EStatUnit.FactorBonus, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PrecisionFactorBonus, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_EvasionBonus, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_EvasionFactor, null), EStatUnit.FactorBonus, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_StrengthBonus, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_StrengthFactor, null), EStatUnit.FactorBonus, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_CriticalEyeDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_CriticalChanceBonus, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_DodgeChanceBonus, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_StealthModifierBonus, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ChanceFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_CriticalMultiplierBonus, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_CriticalMultiplierFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_CriticalDamageBonus, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ArmourRatingBonus, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_ArmourRatingBonusDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ArmourRatingFactor, null), EStatUnit.FactorBonus, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_KillTokenChance, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_FrightenedDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_FrightenedEnemyCount, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FrightenedAttackDelayFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FrightenedFearsomeFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_RetrieveHealthChance, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_RetrieveHealthAmount, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_RetrieveHealthRelative, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_MaxHealthBonus, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_MaxHealthFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_BlindedDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_NotorietyIncreaseDelay, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_NotorietyDecrease, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_String>(EStatType.Rule_FxGuid, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_HirelingLevelFactor_1, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_HirelingLevelFactor_2, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_HirelingLevelFactor_3, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_HirelingLevelFactor_4, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<EnumStatValueWrapper<EStatType>>(EStatType.Rule_HirelingLevelFactor_1_Links, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<EnumStatValueWrapper<EStatType>>(EStatType.Rule_HirelingLevelFactor_2_Links, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<EnumStatValueWrapper<EStatType>>(EStatType.Rule_HirelingLevelFactor_3_Links, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatCollectionValue<EnumStatValueWrapper<EStatType>>(EStatType.Rule_HirelingLevelFactor_4_Links, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ParameterFactor, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FearsomeFactor, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FearsomeFactorBonus, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FearsomeFactorFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_WeaponDamageBonus, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_WeaponDamageDeflectBonus, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_WeaponDamageFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_DamageFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ArmorPiercingFlatBonus, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ArmourDamageReductionBonus, null), EStatUnit.Health, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Int>(EStatType.Rule_ConfusedDuration, null), EStatUnit.Turns, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FeintBonus, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PickpocketingChance, null), EStatUnit.Chance, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_PickpocketingFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_FallingDamageReductionFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<EnumStatValueWrapper<Assassin.EClassTypeId>>(EStatType.Rule_AssassinClassType, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_InitAnonymousDamageMultiplierBonus, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_ItemModWeight, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValue<StatValueWrapper_Float>(EStatType.Rule_GunpowderDamageBonusFactor, null), EStatUnit.Factor, EStatCategory.Immutable, EStatUpdateType.Invalid);
			InitStatValueTemplate(new StatValueWithOwner<EnumStatValueWrapper<EEra>>(EStatType.MapGenerator_Age, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapper_Int>(EStatType.MapGenerator_MapWeight, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, true);
			InitStatValueTemplate(new StatValueWithOwner<StatValueWrapper_Int>(EStatType.MapGenerator_UnlockLevel, null), EStatUnit.Invalid, EStatCategory.Immutable, EStatUpdateType.Invalid, true);
			IsInitialized = true;
		}
	}
}
