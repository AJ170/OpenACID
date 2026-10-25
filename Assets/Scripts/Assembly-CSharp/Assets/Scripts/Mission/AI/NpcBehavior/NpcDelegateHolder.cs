using System;
using Assets.Scripts.Mission.AI.Actions;
using Assets.Scripts.Mission.AI.Actions.Parameters;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals;
using Assets.Scripts.Mission.AI.Knowledge;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.AI.NpcBehavior
{
	public class NpcDelegateHolder : IDelegateHolder, INpcDelegateHolder
	{
		private static class ParameterHelper
		{
			public static readonly string[] ParameterTypeNames;

			public static readonly BBDictionary<int, string[]> DelegateMap;

			public static readonly BBDictionary<int, string[]> ParameterMap;

			private static readonly string[] m_actionParameters;

			private static readonly string[] m_conditionalParameters;

			static ParameterHelper()
			{
				m_actionParameters = new string[Enum.GetValues(typeof(EActionParameterizedType)).Length];
				m_actionParameters[0] = EParametersType.FleeReasonParameters.ToString();
				m_actionParameters[1] = EParametersType.FleeReasonParameters.ToString();
				m_actionParameters[2] = EParametersType.StateMachineParameters.ToString();
				m_actionParameters[3] = EParametersType.FloatParameters.ToString();
				m_actionParameters[4] = EParametersType.BoolParameters.ToString();
				m_actionParameters[5] = EParametersType.IdleParameters.ToString();
				m_actionParameters[6] = EParametersType.MeleeParameters.ToString();
				m_actionParameters[7] = EParametersType.TargetParameters.ToString();
				m_conditionalParameters = new string[Enum.GetValues(typeof(EConditionalParameterizedType)).Length];
				m_conditionalParameters[0] = EParametersType.SkillParameters.ToString();
				m_conditionalParameters[1] = EParametersType.StateMachineParameters.ToString();
				m_conditionalParameters[2] = EParametersType.VictimParameters.ToString();
				m_conditionalParameters[3] = EParametersType.InteractionReasonParameters.ToString();
				m_conditionalParameters[4] = EParametersType.AnimationTaskParameters.ToString();
				m_conditionalParameters[5] = EParametersType.AngleParameters.ToString();
				m_conditionalParameters[6] = EParametersType.DetectionRangeParameters.ToString();
				m_conditionalParameters[7] = EParametersType.DistanceParameters.ToString();
				m_conditionalParameters[8] = EParametersType.StateMachineParameters.ToString();
				ParameterTypeNames = Enum.GetNames(typeof(EParametersType));
				ParameterMap = new BBDictionary<int, string[]>
				{
					{ 2, m_actionParameters },
					{ 9, m_conditionalParameters }
				};
				DelegateMap = new BBDictionary<int, string[]>
				{
					{
						1,
						Enum.GetNames(typeof(EActionType))
					},
					{
						2,
						Enum.GetNames(typeof(EActionParameterizedType))
					},
					{
						8,
						Enum.GetNames(typeof(EConditionalType))
					},
					{
						9,
						Enum.GetNames(typeof(EConditionalParameterizedType))
					}
				};
			}
		}

		private BehaviorAction.DelegateType[] m_actionDelegates;

		private BehaviorAction.CancelDelegateType[] m_actionCancelDelegates;

		private BehaviorAction.AttachDelegateType[] m_actionAttachDelegates;

		private BehaviorActionParameterized.DelegateType[] m_actionParameterizedDelegates;

		private BehaviorActionParameterized.CancelDelegateType[] m_actionParameterizedCancelDelegates;

		private BehaviorActionParameterized.AttachDelegateType[] m_actionParameterizedAttachDelegates;

		private Conditional.DelegateType[] m_conditionalDelegates;

		private ConditionalParameterized.DelegateType[] m_conditionalParameterizedDelegates;

		protected ActionsContainer m_actionsConditions;

		protected CharacterKnowledgeContainer m_knowledge = new CharacterKnowledgeContainer();

		public ActionsContainer ActionsConditions
		{
			get
			{
				return m_actionsConditions;
			}
		}

		public CharacterKnowledgeContainer Knowledge
		{
			get
			{
				return m_knowledge;
			}
		}

		public NpcDelegateHolder(NpcTreeHolder _npcTreeHolder)
		{
			m_knowledge.CreateTargetKnowledge(_npcTreeHolder);
			m_knowledge.CreateCombatKnowledge(_npcTreeHolder);
			m_knowledge.CreateCommonKnowledge(_npcTreeHolder);
			m_actionsConditions = new ActionsContainer(_npcTreeHolder);
			CreateActionDelegates();
			CreateActionParameterizedDelegates();
			CreateConditionalDelegates();
			CreateConditionalParameterizedDelegates();
		}

		public BehaviorAction.DelegateType GetActionDelegate(string _name)
		{
			return m_actionDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorAction.CancelDelegateType GetActionCancelDelegate(string _name)
		{
			return m_actionCancelDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorAction.AttachDelegateType GetActionAttachDelegate(string _name)
		{
			return m_actionAttachDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorActionParameterized.DelegateType GetActionParameterizedDelegate(string _name)
		{
			return m_actionParameterizedDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public BehaviorActionParameterized.CancelDelegateType GetActionParameterizedCancelDelegate(string _name)
		{
			return m_actionParameterizedCancelDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public BehaviorActionParameterized.AttachDelegateType GetActionParameterizedAttachDelegate(string _name)
		{
			return m_actionParameterizedAttachDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public Conditional.DelegateType GetConditionalDelegate(string _name)
		{
			return m_conditionalDelegates[ConvertConditionalStringToIndex(_name)];
		}

		public ConditionalParameterized.DelegateType GetConditionalParameterizedDelegate(string _name)
		{
			return m_conditionalParameterizedDelegates[ConvertConditionalParameterizedStringToIndex(_name)];
		}

		public IParameterContainer CreateActionParameterContainer(int _index)
		{
			string[] array = ParameterHelper.ParameterMap[2];
			return CreateParameterContainer(array[_index]);
		}

		public IParameterContainer CreateConditionalParameterContainer(int _index)
		{
			string[] array = ParameterHelper.ParameterMap[9];
			return CreateParameterContainer(array[_index]);
		}

		public IParameterContainer CreateParameterContainer(string _parameterName)
		{
			switch ((EParametersType)(int)Enum.Parse(typeof(EParametersType), _parameterName))
			{
			case EParametersType.AlertLevelParameters:
				return new AlertLevelParameters();
			case EParametersType.DetectionRangeParameters:
				return new DetectionRangeParameters();
			case EParametersType.DistanceParameters:
				return new DistanceParameters();
			case EParametersType.SkillParameters:
				return new SkillParameters();
			case EParametersType.StateMachineParameters:
				return new StateMachineParameters();
			case EParametersType.FleeReasonParameters:
				return new FleeReasonParameters();
			case EParametersType.FloatParameters:
				return new FloatParameters();
			case EParametersType.BoolParameters:
				return new BoolParameters();
			case EParametersType.IdleParameters:
				return new IdleParameters();
			case EParametersType.MeleeParameters:
				return new MeleeParameters();
			case EParametersType.TargetParameters:
				return new TargetParameters();
			case EParametersType.AnimationTaskParameters:
				return new Assets.Scripts.Mission.AI.Actions.Parameters.AnimationTaskParameters();
			case EParametersType.AngleParameters:
				return new AngleParameters();
			case EParametersType.VictimParameters:
				return new VictimParameters();
			case EParametersType.InteractionReasonParameters:
				return new InteractionReasonParameters();
			default:
				return null;
			}
		}

		public string ConvertActionIndexToString(int _index)
		{
			return ((EActionType)_index).ToString();
		}

		public string ConvertActionParameterizedIndexToString(int _index)
		{
			return ((EActionParameterizedType)_index).ToString();
		}

		public string ConvertConditionalIndexToString(int _index)
		{
			return ((EConditionalType)_index).ToString();
		}

		public string ConvertConditionalParameterizedIndexToString(int _index)
		{
			return ((EConditionalParameterizedType)_index).ToString();
		}

		public int ConvertActionStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EActionType), _name);
		}

		public int ConvertActionParameterizedStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EActionParameterizedType), _name);
		}

		public int ConvertConditionalStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EConditionalType), _name);
		}

		public int ConvertConditionalParameterizedStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EConditionalParameterizedType), _name);
		}

		public string[] GetParameterTypeNames()
		{
			return ParameterHelper.ParameterTypeNames;
		}

		public string[] GetDelegateNames(EBehaviorType _behaviorType)
		{
			if (ParameterHelper.DelegateMap.ContainsKey((int)_behaviorType))
			{
				return ParameterHelper.DelegateMap[(int)_behaviorType];
			}
			return null;
		}

		public string[] GetParameterNames(EBehaviorType _behaviorType)
		{
			if (ParameterHelper.ParameterMap.ContainsKey((int)_behaviorType))
			{
				return ParameterHelper.ParameterMap[(int)_behaviorType];
			}
			return null;
		}

		public BehaviorAction.AttachDelegateType GetDefaultAttachDelegate()
		{
			return m_actionsConditions.PatrolActions.AttachDefault;
		}

		private void CreateActionDelegates()
		{
			m_actionDelegates = new BehaviorAction.DelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionCancelDelegates = new BehaviorAction.CancelDelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionAttachDelegates = new BehaviorAction.AttachDelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionDelegates[1] = m_actionsConditions.PatrolActions.ActionPatrol;
			m_actionCancelDelegates[1] = m_actionsConditions.PatrolActions.CancelPatrol;
			m_actionAttachDelegates[1] = m_actionsConditions.PatrolActions.AttachPatrol;
			m_actionDelegates[0] = m_actionsConditions.PatrolActions.ActionReturnToPatrol;
			m_actionAttachDelegates[0] = m_actionsConditions.PatrolActions.AttachReturnToPatrol;
			m_actionDelegates[2] = m_actionsConditions.ChaseActions.ActionApproachTarget;
			m_actionAttachDelegates[2] = m_actionsConditions.ChaseActions.AttachApproachTarget;
			m_actionCancelDelegates[2] = m_actionsConditions.ChaseActions.CancelApproachTarget;
			m_actionDelegates[3] = m_actionsConditions.ChaseActions.ActionWait;
			m_actionDelegates[4] = m_actionsConditions.ChaseActions.ActionTurnInLastKnownRotationY;
			m_actionDelegates[5] = m_actionsConditions.ChaseActions.ActionLookLeftOfLastKnownRotationY;
			m_actionAttachDelegates[5] = m_actionsConditions.ChaseActions.AttachLookAroundLastKnownRotationY;
			m_actionCancelDelegates[5] = m_actionsConditions.ChaseActions.CancelLookAroundLastKnownRotationY;
			m_actionDelegates[6] = m_actionsConditions.ChaseActions.ActionLookRightOfLastKnownRotationY;
			m_actionAttachDelegates[6] = m_actionsConditions.ChaseActions.AttachLookAroundLastKnownRotationY;
			m_actionCancelDelegates[6] = m_actionsConditions.ChaseActions.CancelLookAroundLastKnownRotationY;
			m_actionDelegates[7] = m_actionsConditions.ChaseActions.ActionCombat;
			m_actionDelegates[8] = m_actionsConditions.ChaseActions.ActionUnsheathMelee;
			m_actionDelegates[9] = m_actionsConditions.ChaseActions.ActionSheathMelee;
			m_actionDelegates[10] = m_actionsConditions.ChaseActions.ActionClearHidingSpots;
			m_actionDelegates[11] = m_actionsConditions.ChaseActions.ActionCollectHidingSpots;
			m_actionDelegates[12] = m_actionsConditions.ChaseActions.ActionChooseHidingSpot;
			m_actionDelegates[13] = m_actionsConditions.ChaseActions.ActionMoveToHidingSpot;
			m_actionDelegates[14] = m_actionsConditions.ChaseActions.ActionInspectHidingSpot;
			m_actionCancelDelegates[14] = m_actionsConditions.ChaseActions.CancelInspectHidingSpot;
			m_actionDelegates[15] = m_actionsConditions.RangedCombatActions.ActionUnsheathRanged;
			m_actionCancelDelegates[15] = m_actionsConditions.RangedCombatActions.CancelUnsheathRanged;
			m_actionDelegates[16] = m_actionsConditions.RangedCombatActions.ActionAimWithRanged;
			m_actionAttachDelegates[16] = m_actionsConditions.RangedCombatActions.AttachAimWithRanged;
			m_actionCancelDelegates[16] = m_actionsConditions.RangedCombatActions.CancelAimWithRanged;
			m_actionDelegates[17] = m_actionsConditions.RangedCombatActions.ActionShootWithRanged;
			m_actionDelegates[18] = m_actionsConditions.RangedCombatActions.ActionThrowDagger;
			m_actionDelegates[19] = m_actionsConditions.RangedCombatActions.ActionStrafe;
			m_actionDelegates[20] = m_actionsConditions.ReactOnTargetActions.ActionUpdatePushDelay;
			m_actionDelegates[21] = m_actionsConditions.ReactOnTargetActions.ActionStartApproachTimer;
			m_actionDelegates[22] = m_actionsConditions.ReactOnTargetActions.ActionApproachTargetForPush;
			m_actionDelegates[23] = m_actionsConditions.ReactOnTargetActions.ActionPush;
			m_actionDelegates[24] = m_actionsConditions.ReactOnTargetActions.ActionDismiss;
			m_actionDelegates[25] = m_actionsConditions.ReactOnTargetActions.ActionStareAtTarget;
			m_actionDelegates[26] = m_actionsConditions.ReactOnTargetActions.ActionRestartWaitAfterStare;
			m_actionDelegates[27] = m_actionsConditions.SidetrackedActions.ActionSweetMusicLureReaction;
			m_actionDelegates[28] = m_actionsConditions.SidetrackedActions.ActionApproachTargetSweetMusic;
			m_actionDelegates[29] = m_actionsConditions.SidetrackedActions.ActionJackInTheBoxReaction;
			m_actionDelegates[30] = m_actionsConditions.SidetrackedActions.ActionApproachTargetJackInTheBox;
			m_actionAttachDelegates[30] = m_actionsConditions.SidetrackedActions.AttachApproachTargetJackInTheBox;
			m_actionDelegates[31] = m_actionsConditions.SidetrackedActions.ActionApproachTargetSeduced;
			m_actionAttachDelegates[31] = m_actionsConditions.SidetrackedActions.AttachApproachTargetSeduced;
			m_actionDelegates[32] = m_actionsConditions.SidetrackedActions.ActionIdleAtTargetSeduced;
			m_actionCancelDelegates[32] = m_actionsConditions.SidetrackedActions.CancelIdleAtTargetSeduced;
			m_actionDelegates[33] = m_actionsConditions.SidetrackedActions.ActionApproachTargetGreedy;
			m_actionAttachDelegates[33] = m_actionsConditions.SidetrackedActions.AttachApproachTargetGreedy;
			m_actionDelegates[34] = m_actionsConditions.SidetrackedActions.ActionIdleAtTargetGreedy;
			m_actionDelegates[35] = m_actionsConditions.SidetrackedActions.ActionApproachTargetStickyBombPanic;
			m_actionAttachDelegates[35] = m_actionsConditions.SidetrackedActions.AttachApproachTargetStickyBombPanic;
			m_actionDelegates[36] = m_actionsConditions.SidetrackedActions.ActionIdleAtTargetStickyBombPanic;
			m_actionDelegates[37] = m_actionsConditions.SidetrackedActions.ActionShoutAtEscortee;
			m_actionDelegates[38] = m_actionsConditions.SidetrackedActions.ActionApproachTargetThreatVictim;
			m_actionAttachDelegates[38] = m_actionsConditions.SidetrackedActions.AttachApproachTargetThreatVictim;
			m_actionCancelDelegates[38] = m_actionsConditions.SidetrackedActions.CancelApproachTargetThreatVictim;
			m_actionAttachDelegates[39] = m_actionsConditions.SidetrackedActions.AttachIdleAtTargetThreatVictim;
			m_actionDelegates[39] = m_actionsConditions.SidetrackedActions.ActionIdleAtTargetThreatVictim;
			m_actionCancelDelegates[39] = m_actionsConditions.SidetrackedActions.CancelIdleAtTargetThreatVictim;
			m_actionDelegates[40] = m_actionsConditions.SidetrackedActions.ActionKillEscortee;
			m_actionDelegates[41] = m_actionsConditions.SidetrackedActions.ActionKillCaptive;
			m_actionDelegates[42] = m_actionsConditions.SidetrackedActions.ActionRegisterVictimDead;
			m_actionDelegates[43] = m_actionsConditions.SidetrackedActions.ActionEndThreatening;
			m_actionDelegates[44] = m_actionsConditions.MissionActions.ActionApproachMissionAlertedTarget;
			m_actionAttachDelegates[44] = m_actionsConditions.MissionActions.AttachApproachMissionAlertedTarget;
			m_actionDelegates[45] = m_actionsConditions.FleeActions.ActionCrowdFlee;
			m_actionAttachDelegates[45] = m_actionsConditions.FleeActions.AttachCrowdFlee;
			m_actionCancelDelegates[45] = m_actionsConditions.FleeActions.CancelCrowdFlee;
			m_actionDelegates[46] = m_actionsConditions.FleeActions.ActionEnemyFleeFromCombat;
			m_actionDelegates[47] = m_actionsConditions.FleeActions.ActionEnemyWait;
			m_actionDelegates[48] = m_actionsConditions.FleeActions.ActionAlarmNearGuards;
			m_actionDelegates[49] = m_actionsConditions.CourtesanActions.SeduceWalk;
			m_actionCancelDelegates[49] = m_actionsConditions.CourtesanActions.CancelSeduceWalk;
			m_actionDelegates[50] = m_actionsConditions.CourtesanActions.SeduceGuards;
			m_actionCancelDelegates[50] = m_actionsConditions.CourtesanActions.CancelSeduceGuards;
			m_actionDelegates[52] = m_actionsConditions.MiscActions.ActionRestartAlarmedActivity;
			m_actionDelegates[51] = m_actionsConditions.MiscActions.ActionUpdateLastKnownPosition;
			m_actionDelegates[53] = m_actionsConditions.MiscActions.ActionPlayerShouldExitHidingSpot;
			m_actionDelegates[54] = m_actionsConditions.MiscActions.ActionGetExecutedOnGround;
		}

		private void CreateActionParameterizedDelegates()
		{
			m_actionParameterizedDelegates = new BehaviorActionParameterized.DelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedCancelDelegates = new BehaviorActionParameterized.CancelDelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedAttachDelegates = new BehaviorActionParameterized.AttachDelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedDelegates[0] = m_actionsConditions.FleeActions.ActionUpdateFleeSpeed;
			m_actionParameterizedDelegates[1] = m_actionsConditions.FleeActions.ActionUpdateFleeTimer;
			m_actionParameterizedDelegates[2] = m_actionsConditions.MiscActions.ActionSetCharacterAwareness;
			m_actionParameterizedDelegates[3] = m_actionsConditions.MiscActions.ActionIncreaseToNotoriety;
			m_actionParameterizedDelegates[4] = m_actionsConditions.MiscActions.ActionSetDisguised;
			m_actionParameterizedAttachDelegates[5] = m_actionsConditions.MiscActions.AttachIdle;
			m_actionParameterizedDelegates[5] = m_actionsConditions.MiscActions.ActionIdle;
			m_actionParameterizedDelegates[6] = m_actionsConditions.MiscActions.ActionGetKilled;
			m_actionParameterizedDelegates[7] = m_actionsConditions.MiscActions.ActionTurn;
		}

		private void CreateConditionalDelegates()
		{
			m_conditionalDelegates = new Conditional.DelegateType[Enum.GetValues(typeof(EConditionalType)).Length];
			if (m_knowledge.CommonKnowledge != null)
			{
				m_conditionalDelegates[0] = m_knowledge.CommonKnowledge.IsAttached;
				m_conditionalDelegates[1] = m_knowledge.CommonKnowledge.CanActCommon;
				m_conditionalDelegates[2] = m_knowledge.CommonKnowledge.IsDisguised;
				m_conditionalDelegates[3] = m_knowledge.CommonKnowledge.IsEnemy;
				m_conditionalDelegates[4] = m_knowledge.CommonKnowledge.IsDead;
				m_conditionalDelegates[5] = m_knowledge.CommonKnowledge.IsUnconscious;
				m_conditionalDelegates[6] = m_knowledge.CommonKnowledge.IsInNormalCondition;
				m_conditionalDelegates[7] = m_knowledge.CommonKnowledge.IsJackBoxed;
				m_conditionalDelegates[8] = m_knowledge.CommonKnowledge.IsSeducing;
				m_conditionalDelegates[9] = m_knowledge.CommonKnowledge.IsSeduced;
				m_conditionalDelegates[10] = m_knowledge.CommonKnowledge.IsGreedy;
				m_conditionalDelegates[31] = m_knowledge.CommonKnowledge.IsConfused;
				m_conditionalDelegates[32] = m_knowledge.CommonKnowledge.IsLuredBySweetMusic;
				m_conditionalDelegates[11] = m_knowledge.CommonKnowledge.IsStickyBombPaniced;
				m_conditionalDelegates[12] = m_knowledge.CommonKnowledge.IsThreateningVictim;
				m_conditionalDelegates[13] = m_knowledge.CommonKnowledge.IsVictimThreatened;
				m_conditionalDelegates[14] = m_knowledge.CommonKnowledge.IsFleeingFromCombat;
				m_conditionalDelegates[15] = m_knowledge.CommonKnowledge.IsFleeingFromCombatViral;
				m_conditionalDelegates[16] = m_knowledge.CommonKnowledge.IsWalkingFromCombat;
				m_conditionalDelegates[21] = m_knowledge.CommonKnowledge.IsCaptured;
				m_conditionalDelegates[22] = m_knowledge.CommonKnowledge.IsFleeingFromCapture;
				m_conditionalDelegates[23] = m_knowledge.CommonKnowledge.IsSilenced;
				m_conditionalDelegates[24] = m_knowledge.CommonKnowledge.IsApproachedToBeAssasinated;
				m_conditionalDelegates[17] = m_knowledge.CommonKnowledge.AwarenessLow;
				m_conditionalDelegates[18] = m_knowledge.CommonKnowledge.AwarenessHigh;
				m_conditionalDelegates[19] = m_knowledge.CommonKnowledge.Alarmed;
				m_conditionalDelegates[20] = m_knowledge.CommonKnowledge.TargetExposedInClosedRange;
				m_conditionalDelegates[25] = m_knowledge.CommonKnowledge.IsWithinExtendedRangeOfPatrolPosition;
				m_conditionalDelegates[26] = m_knowledge.CommonKnowledge.IsInCameraLOS;
				m_conditionalDelegates[28] = m_knowledge.CommonKnowledge.IsVictimAttacked;
				m_conditionalDelegates[27] = m_knowledge.CommonKnowledge.IsThreateningFinalized;
				m_conditionalDelegates[29] = m_knowledge.CommonKnowledge.IsVictimHit;
				m_conditionalDelegates[30] = m_knowledge.CommonKnowledge.HasArrivedAtVictim;
			}
			if (m_knowledge.CombatKnowledge != null)
			{
				m_conditionalDelegates[33] = m_knowledge.CombatKnowledge.CombatActive;
				m_conditionalDelegates[34] = m_knowledge.CombatKnowledge.InCombat;
				m_conditionalDelegates[35] = m_knowledge.CombatKnowledge.HasRangedWeapon;
				m_conditionalDelegates[36] = m_knowledge.CombatKnowledge.HasBow;
				m_conditionalDelegates[37] = m_knowledge.CombatKnowledge.HasCrossbow;
				m_conditionalDelegates[38] = m_knowledge.CombatKnowledge.IsRangedSheathed;
				m_conditionalDelegates[39] = m_knowledge.CombatKnowledge.MeleeWeaponUnsheathedChoosen;
				m_conditionalDelegates[40] = m_knowledge.CombatKnowledge.DidSeeBombExplosion;
				m_conditionalDelegates[41] = m_knowledge.CombatKnowledge.ConflictCountBelowCap;
			}
			if (m_knowledge.TargetKnowledge != null)
			{
				m_conditionalDelegates[42] = m_knowledge.TargetKnowledge.TargetInAlertedOrExposed;
				m_conditionalDelegates[43] = m_knowledge.TargetKnowledge.TargetInAlerted;
				m_conditionalDelegates[44] = m_knowledge.TargetKnowledge.TargetChasedByEnemies;
				m_conditionalDelegates[45] = m_knowledge.TargetKnowledge.TargetHidingInObject;
				m_conditionalDelegates[46] = m_knowledge.TargetKnowledge.TargetHiding;
				m_conditionalDelegates[47] = m_knowledge.TargetKnowledge.TargetRunning;
				m_conditionalDelegates[48] = m_knowledge.TargetKnowledge.TargetInDisguise;
				m_conditionalDelegates[49] = m_knowledge.TargetKnowledge.TargetInRestrictedArea;
				m_conditionalDelegates[50] = m_knowledge.TargetKnowledge.TargetDead;
				m_conditionalDelegates[51] = m_knowledge.TargetKnowledge.TargetAttackable;
				m_conditionalDelegates[52] = m_knowledge.TargetKnowledge.TargetIsPerformingStrikingAssasination;
				m_conditionalDelegates[54] = m_knowledge.TargetKnowledge.TargetEagleVisionIsActive;
				m_conditionalDelegates[55] = m_knowledge.TargetKnowledge.TargetInRangeNotHiding;
				m_conditionalDelegates[53] = m_knowledge.TargetKnowledge.IsTargetInCombatDistance;
			}
			m_conditionalDelegates[56] = m_actionsConditions.ReactOnTargetActions.IsPushOrTurnPlaying;
			m_conditionalDelegates[57] = m_actionsConditions.ReactOnTargetActions.IsDismissOrTurnPlaying;
			m_conditionalDelegates[58] = m_actionsConditions.ReactOnTargetActions.IsPushDelayFinished;
			m_conditionalDelegates[59] = m_actionsConditions.ReactOnTargetActions.IsPushApproachFinished;
			m_conditionalDelegates[60] = m_actionsConditions.ReactOnTargetActions.IsDismissCooldownRunning;
			m_conditionalDelegates[61] = m_actionsConditions.ReactOnTargetActions.IsWaitAfterStareRunning;
			m_conditionalDelegates[62] = m_actionsConditions.FleeActions.IsFleeTimerRunning;
			m_conditionalDelegates[63] = m_actionsConditions.RangedCombatActions.ConditionKeepChargingIsRunning;
			m_conditionalDelegates[64] = m_actionsConditions.RangedCombatActions.ConditionChargingIsFinished;
			m_conditionalDelegates[65] = m_actionsConditions.RangedCombatActions.ConditionChargingIsRunning;
			m_conditionalDelegates[66] = m_actionsConditions.ChaseActions.ConditionHasHidingSpotsCollected;
			m_conditionalDelegates[67] = m_actionsConditions.ChaseActions.ConditionInspectionAtRouseTime;
			m_conditionalDelegates[68] = m_actionsConditions.ChaseActions.ConditionInspectionRouseForced;
		}

		private void CreateConditionalParameterizedDelegates()
		{
			m_conditionalParameterizedDelegates = new ConditionalParameterized.DelegateType[Enum.GetValues(typeof(EConditionalParameterizedType)).Length];
			if (m_knowledge.CommonKnowledge != null)
			{
				m_conditionalParameterizedDelegates[1] = m_knowledge.CommonKnowledge.IsInInteractionCharacterMainState;
				m_conditionalParameterizedDelegates[2] = m_knowledge.CommonKnowledge.IsVictimType;
				m_conditionalParameterizedDelegates[3] = m_knowledge.CommonKnowledge.IsApproachedByAssassin;
			}
			if (m_knowledge.TargetKnowledge != null)
			{
				m_conditionalParameterizedDelegates[4] = m_knowledge.TargetKnowledge.TargetPlayingTask;
				m_conditionalParameterizedDelegates[5] = m_knowledge.TargetKnowledge.TargetMaxAngleVertical;
				m_conditionalParameterizedDelegates[6] = m_knowledge.TargetKnowledge.IsTargetInParameterRange;
				m_conditionalParameterizedDelegates[7] = m_knowledge.TargetKnowledge.IsTargetInParameterDistance;
				m_conditionalParameterizedDelegates[8] = m_knowledge.TargetKnowledge.IsTargetInParameterMainState;
			}
		}
	}
}
