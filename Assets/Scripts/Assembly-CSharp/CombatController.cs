using System;
using Assets.Scripts.Animation;
using Assets.Scripts.Audio;
using Assets.Scripts.Crowd;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class CombatController : MonoBehaviour, IEntityRemovedFromListHandler
{
	public float KillTokenDecayTime = 5f;

	public bool EnableAutoEnterCombat;

	public bool EnableAutoExitCombat = true;

	public float AutoEnterCombatDistance = 10f;

	public float AutoExitCombatDistance = 10f;

	public float PlayerMaxAttackDistance = 3.5f;

	public int MaxParticipantCount = 8;

	public float ExitCombatHoldIdle = 2f;

	public float EnemyPositioningSensitivity = 3f;

	public float EnemyMinAttackDistance = 2f;

	public float EnemyMaxAttackDistance = 3f;

	public float EnemyBestAttackDistance = 2.2f;

	public float EnemyVariationAttackDistance = 0.5f;

	public float EnemyCancelAttackThreshold = 1f;

	public float EnemySolutionDistance = 1.5f;

	public float EnemyAvoidanceRadius = 2f;

	public float EnemyAvoidanceDamping = 0.2f;

	public float EnemySurroundMaxRadius = 6f;

	public float EnemySurroundIgnoreAngle = 10f;

	public float EnemySurroundCoverAngle = 120f;

	public float EnemySurroundDamping = 0.2f;

	public float EnemyFinalDamping = 0.2f;

	public float EnemyFallbackDamping = 0.1f;

	public float EnemyFallbackFactorBase = -0.75f;

	public float EnemyFallbackDistance = 2.5f;

	public float EnemyStopParryDistance = 4.5f;

	public float MaxCouldNotReachTargetTime = 1f;

	public float MaxFullUpdateInterval = 0.49f;

	public float MaxTargetDistance = 20f;

	public float MaxTargetAngle = 60f;

	public bool EnableComboChangeTarget;

	public bool EnableKillTokenKillPreset = true;

	public bool EnableKillTokenKillRuntime = true;

	public float KillTokenDecayDurationCombat = 5f;

	public float KillTokenDecayDurationNormal = 3f;

	public float KillTokenModeDecayDurationCombat = 5f;

	public float KillTokenModeDecayDurationNormal = 3f;

	public bool EnableEnemyAttack = true;

	public float EnemyBehaviourAllyFlagMaxAge = 5f;

	public float ParryStanceDuration = 0.3f;

	public bool ForceAttackAnimationId;

	public int AttackAnimationId = 1;

	public bool ForceComboAnimationId;

	public int ComboAnimationId = 1;

	public bool ForceRandomAnimationParam;

	public float RandomAnimationParam = -1f;

	public float SchedulerGlobalCooldown = 4f;

	public float AgentParrySpeed = 1f;

	public float AgentStrafeSpeed = 2f;

	public float AgentMoveSpeed = 5.12f;

	public float AgentAcceleration = 25f;

	public float AgentAngularSpeed = 1440f;

	public bool EnableFollowAgent = true;

	public float FollowAgentDamping = 0.2f;

	public bool EnableSpeedBoost = true;

	public float SpeedBoostFactor = 0.3f;

	public float SpeedReduceFactor = 0.2f;

	public float SpeedBoostThresholdMax = 7f;

	public float SpeedBoostThresholdMin = 3.5f;

	public float StayOnNavMeshDamping = 0.1f;

	public float StrafeDistanceStart = 0.3f;

	public float StrafeDistanceStop = 0.15f;

	public float HitDistance = 1.5f;

	public float HitTimeOffset;

	public float HitCorrectionDamping = 0.2f;

	public SlowMotionEventData SlowMotionParryKillMove;

	public BBDictionary<int, float[]> HitDistanceOverride = new BBDictionary<int, float[]>
	{
		{
			1101,
			new float[2] { 0.6f, 1.5f }
		},
		{
			1102,
			new float[2] { 0.6f, 0.8f }
		},
		{
			1103,
			new float[2] { 0.6f, 1.5f }
		},
		{
			1104,
			new float[2] { 0.5f, 1.2f }
		},
		{
			1106,
			new float[2] { 0.6f, 1.5f }
		}
	};

	public CombatUiData CombatUiData = new CombatUiData();

	public bool DebugPlayerImmortal;

	public bool DebugEnemiesImmortal;

	public bool DebugDisableCriticalDamage;

	public bool DebugShowInfo;

	private int m_LastParticipantJoinedTurn;

	private IParticipantCarrier m_LastJoinedParticipant;

	private readonly BBList<CombatParticipant> m_participantCache = new BBList<CombatParticipant>();

	private readonly BBList<CombatParticipant> m_participantRemove = new BBList<CombatParticipant>();

	private readonly BBList<CombatParticipant> m_participants = new BBList<CombatParticipant>();

	private bool m_finalKillCameraStarted;

	private EntityId m_finalKillTarget = EntityId.InvalidId;

	private int m_isFleeingAllowed;

	private bool m_isTurnUpdate;

	private int m_updateTurn = -1;

	private Highlight m_activeHighlight;

	private NavMeshHit m_dummyHit;

	// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
	private NavMeshPath m_pathPart1__lazy;

	private NavMeshPath m_pathPart1
	{
		get
		{
			return m_pathPart1__lazy ?? (m_pathPart1__lazy = new NavMeshPath());
		}
	}

	// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
	private NavMeshPath m_pathPart2__lazy;

	private NavMeshPath m_pathPart2
	{
		get
		{
			return m_pathPart2__lazy ?? (m_pathPart2__lazy = new NavMeshPath());
		}
	}

	private float m_schedulerGlobalCooldownMultiplier = 1f;

	private float m_schedulerLastCooldownTime;

	private float m_minSeenGlobalFleeChanceMultiplier = 1f;

	private static GameObject m_GameObject;

	private readonly BBList<CombatParticipant> m_localCooldownCompleted = new BBList<CombatParticipant>();

	public bool EnableKillTokenKill
	{
		get
		{
			return EnableKillTokenKillPreset && EnableKillTokenKillRuntime;
		}
	}

	public int LastParticipantJoinedTurn
	{
		get
		{
			return m_LastParticipantJoinedTurn;
		}
	}

	public IParticipantCarrier LastJoinedParticipant
	{
		get
		{
			return m_LastJoinedParticipant;
		}
	}

	public CombatParticipant PlayerParticipant { get; private set; }

	public static bool IsActive { get; private set; }

	public EnterCombatMode PlayerEnterCombatMode { get; set; }

	public ExitCombatMode PlayerExitCombatMode { get; set; }

	public float GlobalFleeChanceMultiplier { get; private set; }

	public float GlobalAllyGotComboKilledTime { get; set; }

	public float GlobalAllyGotAttackedTime { get; set; }

	public EntityId FinalKillTarget
	{
		get
		{
			return m_finalKillTarget;
		}
		private set
		{
			m_finalKillTarget = value;
			m_finalKillCameraStarted = false;
		}
	}

	public bool IsFleeingAllowed
	{
		get
		{
			return m_isFleeingAllowed <= 0;
		}
		set
		{
			m_isFleeingAllowed += ((!value) ? 1 : (-1));
		}
	}

	public int KillTokenCountRequired { get; set; }

	public AudioCombatController AudioController { get; private set; }

	public Highlight ActiveHighlight
	{
		get
		{
			return m_activeHighlight;
		}
		set
		{
			if (value != m_activeHighlight)
			{
				if (m_activeHighlight != null)
				{
					m_activeHighlight.RemoveMode(Highlight.EHighlightLayer.Combat);
				}
				m_activeHighlight = value;
				if (m_activeHighlight != null)
				{
					m_activeHighlight.SetMode(Highlight.EHighlightLayer.Combat, Highlight.ESelectionMode.On);
				}
			}
		}
	}

	public bool PlayerIsFleeing
	{
		get
		{
			return PlayerParticipant != null && PlayerParticipant.WantsToFlee;
		}
	}

	public static bool IsPlayerEscapeAllowed
	{
		get
		{
			return IsActive && Instance.CheckPlayerEscapeAllowed() && LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerMain.ScheduledStateType == EStateType.Invalid;
		}
	}

	public static CombatController Instance { get; private set; }

	static CombatController()
	{
		IsActive = false;
	}

	public CombatParticipant GetParticipant(int _i)
	{
		return m_participants[_i];
	}

	public int ParticipantCount()
	{
		return m_participants.Count;
	}

	public int AvailableParticipantSlots()
	{
		return Mathf.Max(MaxParticipantCount - ParticipantCount(), 0);
	}

	public static void Initialize()
	{
		m_GameObject = Assets.Scripts.Mission.Helpers.Instantiate("Combat");
		Instance = m_GameObject.GetComponent<CombatController>();
	}

	public void Release()
	{
		UnityEngine.Object.Destroy(m_GameObject);
		Instance = null;
	}

	public void LoadMission()
	{
		Instance = this;
		IsActive = false;
		PlayerParticipant = null;
		KillTokenCountRequired = LogicMain.PlayerEntity.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenRequiredForKill);
		for (int i = 0; i < MaxParticipantCount; i++)
		{
			m_participantCache.Add(new CombatParticipant(this));
		}
		LogicMain.EntityManager.RegisterHandler(this);
	}

	public void UnloadMission()
	{
		KillTokenCountRequired = 0;
		m_participants.ClearIfNotEmpty();
		IsActive = false;
		ActiveHighlight = null;
	}

	private void Awake()
	{
		Instance = this;
		AudioController = new AudioCombatController();
	}

	private void OnDestroy()
	{
		Instance = null;
		AudioController.Release();
	}

	private CombatParticipant AttachParticipant(IParticipantCarrier _carrier)
	{
		Profiling.Start("CombatController::AttachParticipant()", float.MaxValue);
		CombatParticipant combatParticipant = null;
		if (m_participantCache.Count == 0)
		{
			combatParticipant = new CombatParticipant(this);
		}
		else
		{
			combatParticipant = m_participantCache[0];
			m_participantCache.RemoveAt(0);
		}
		combatParticipant.Proxy = _carrier.CombatParticipantProxy;
		combatParticipant.OnExitCombat += OnCharacterExit;
		m_participants.Add(combatParticipant);
		Profiling.Start("AudioController.OnParticipantEnter", float.MaxValue);
		AudioController.OnParticipantEnter(combatParticipant);
		Profiling.Stop();
		Profiling.Stop();
		return combatParticipant;
	}

	private void DetachParticipant(CombatParticipant _participant)
	{
		m_participantRemove.Add(_participant);
		m_participants.Remove(_participant);
		AudioController.OnParticipantExit(_participant);
		_participant.OnExitCombat -= OnCharacterExit;
		if (ActiveHighlight == _participant.Highlight)
		{
			ActiveHighlight = null;
		}
	}

	public void EnterCombat()
	{
		ActiveHighlight = null;
		IsActive = true;
		PlayerExitCombatMode = ExitCombatMode.Invalid;
		UbiCore.Utils.Singleton<SelectionManager>.Instance.UpdateSelection();
		CombatParticipant combatParticipant = AttachParticipant(LogicMain.PlayerEntity);
		combatParticipant.EnterCombat(PlayerEnterCombatMode);
		PlayerParticipant = combatParticipant;
		GlobalFleeChanceMultiplier = 1f;
		GlobalAllyGotComboKilledTime = float.MinValue;
		GlobalAllyGotAttackedTime = float.MinValue;
	}

	public bool AddParticipant(IParticipantCarrier _participant, EnterCombatMode _mode)
	{
		if (_participant.CombatParticipantProxy.IsInCombat)
		{
			_participant.CombatParticipantProxy.CombatParticipant.KnowinglyInCombat = true;
			return true;
		}
		if (!CanParticipantJoin((Character)_participant))
		{
			return false;
		}
		CombatParticipant combatParticipant = AttachParticipant(_participant);
		combatParticipant.EnterCombat(_mode);
		m_LastParticipantJoinedTurn = LogicMain.CurrentTurn;
		m_LastJoinedParticipant = _participant;
		return true;
	}

	public void UpdateGlobalMultipliers()
	{
		m_schedulerGlobalCooldownMultiplier = 1f;
		m_minSeenGlobalFleeChanceMultiplier = 1f;
		for (int i = 0; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			m_schedulerGlobalCooldownMultiplier = Mathf.Min(combatParticipant.GlobalSchedulerCooldownMultiplier, m_schedulerGlobalCooldownMultiplier);
			m_minSeenGlobalFleeChanceMultiplier = Mathf.Min(combatParticipant.GlobalFleeChanceMultiplier, m_minSeenGlobalFleeChanceMultiplier);
		}
		GlobalFleeChanceMultiplier = m_minSeenGlobalFleeChanceMultiplier;
	}

	public void FrameUpdate()
	{
		int currentTurn = LogicMain.CurrentTurn;
		if (m_updateTurn != currentTurn)
		{
			m_isTurnUpdate = true;
			m_updateTurn = currentTurn;
		}
		if (IsActive)
		{
			Profiling.Start("Controller::Update()", float.MaxValue);
			if (PlayerParticipant != null && PlayerParticipant.NextTarget != null)
			{
				ActiveHighlight = PlayerParticipant.NextTarget.Highlight;
			}
			if (PlayerParticipant == null || PlayerParticipant.ActiveBehaviour == null || (PlayerParticipant.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Idle && IsTooFarFromEnemies()))
			{
				ExitCombat(ExitCombatMode.EnemiesTooFar);
			}
			else if (PlayerParticipant.WantsToFlee && PlayerParticipant.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Idle && !EnemyAttackInProgress())
			{
				ExitCombat(ExitCombatMode.PlayerFlee);
			}
			else
			{
				UpdateGlobalMultipliers();
				ScheduleFullUpdates();
				if (EnableAutoEnterCombat && m_isTurnUpdate && m_updateTurn % 3 == 0)
				{
					CloseEnemiesEnterCombat();
				}
				int num = 0;
				for (num = 0; num < m_participants.Count; num++)
				{
					m_participants[num].BeginUpdate();
				}
				UpdateDesiredPositions();
				if (EnableEnemyAttack)
				{
					ScheduleAttacks();
				}
				for (num = 0; num < m_participants.Count; num = Math.Min(num + 1, m_participants.Count))
				{
					m_participants[num].Update();
				}
				for (num = 0; num < m_participants.Count; num++)
				{
					m_participants[num].EndUpdate();
				}
			}
			Profiling.Stop();
		}
		for (int i = 0; i < m_participantRemove.Count; i++)
		{
			CombatParticipant combatParticipant = m_participantRemove[i];
			m_participantCache.Add(combatParticipant);
			combatParticipant.Proxy = null;
		}
		m_participantRemove.Clear();
		m_isTurnUpdate = false;
	}

	public float GetTargetHitTime(CombatBehaviour _behaviour, AttackMeleeType _attackMeleeType)
	{
		AnimatorStateInfo nextAnimatorStateInfo = _behaviour.Animator.GetNextAnimatorStateInfo(0);
		float[] value;
		if (HitDistanceOverride.TryGetValue((int)_attackMeleeType, out value))
		{
			float num = value[0];
			if (num >= 0f)
			{
				return LogicMain.CurrentTime + num - nextAnimatorStateInfo.normalizedTime * nextAnimatorStateInfo.length;
			}
		}
		AnimatorClipInfo[] nextAnimatorClipInfo = _behaviour.Animator.GetNextAnimatorClipInfo(0);
		for (int i = 0; i < nextAnimatorClipInfo.Length; i++)
		{
			AnimationClip clip = nextAnimatorClipInfo[i].clip;
			AnimatorData.Event eventByMotion = _behaviour.AnimatorDataController.GetEventByMotion(clip, AnimatorEventType.Hit);
			if (eventByMotion != null)
			{
				return LogicMain.CurrentTime + eventByMotion.m_time * nextAnimatorStateInfo.length - nextAnimatorStateInfo.normalizedTime * nextAnimatorStateInfo.length;
			}
		}
		return LogicMain.CurrentTime + 0.1f;
	}

	private void ScheduleFullUpdates()
	{
		float frameTime = LogicMain.FrameTime;
		for (int i = 0; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			if (combatParticipant.DoFullUpdate)
			{
				combatParticipant.LastFullUpdate = 0f;
				combatParticipant.DoFullUpdate = false;
			}
			combatParticipant.LastFullUpdate += frameTime;
		}
		float num = 0f;
		for (int j = 0; j < m_participants.Count; j++)
		{
			CombatParticipant combatParticipant2 = null;
			for (int k = 0; k < m_participants.Count; k++)
			{
				CombatParticipant combatParticipant3 = m_participants[k];
				float lastFullUpdate = combatParticipant3.LastFullUpdate;
				if (!(lastFullUpdate <= num) && combatParticipant3.KnowinglyInCombat)
				{
					num = lastFullUpdate;
					combatParticipant2 = combatParticipant3;
				}
			}
			if (combatParticipant2 == null)
			{
				break;
			}
			combatParticipant2.DoFullUpdate = true;
			if (num < MaxFullUpdateInterval)
			{
				break;
			}
			num = MaxFullUpdateInterval;
		}
	}

	private void ScheduleAttacks()
	{
		if (m_participants.Count <= 1 || LogicMain.CurrentTime - m_schedulerLastCooldownTime < SchedulerGlobalCooldown * m_schedulerGlobalCooldownMultiplier)
		{
			return;
		}
		m_localCooldownCompleted.ClearIfNotEmpty();
		for (int i = 1; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			if (!combatParticipant.SchedulerCanAttack)
			{
				EnemyCharacter entity = combatParticipant.Character as EnemyCharacter;
				EntityStats entityStats = combatParticipant.Entity.EntityStats;
				float num = LogicMain.CurrentTime - combatParticipant.SchedulerCooldownStartTime;
				float attackDelayFactor = entity.GetAttackDelayFactor();
				float num2 = entityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseSchedulerCooldown);
				if (num > num2 * attackDelayFactor * combatParticipant.SchedulerCooldownMultiplier && combatParticipant.CanAttack())
				{
					m_localCooldownCompleted.Add(combatParticipant);
				}
			}
		}
		if (m_localCooldownCompleted.Count == 0)
		{
			return;
		}
		float num3 = 0f;
		for (int j = 0; j < m_localCooldownCompleted.Count; j++)
		{
			num3 += m_localCooldownCompleted[j].SchedulerPriority;
		}
		float num4 = LogicMain.Random.RandF() * num3;
		float num5 = 0f;
		CombatParticipant combatParticipant2 = null;
		for (int k = 0; k < m_localCooldownCompleted.Count; k++)
		{
			combatParticipant2 = m_localCooldownCompleted[k];
			num5 += combatParticipant2.SchedulerPriority;
			if (num4 <= num5)
			{
				break;
			}
		}
		combatParticipant2.SchedulerCanAttack = true;
		m_schedulerLastCooldownTime = LogicMain.CurrentTime;
		for (int l = 0; l < m_localCooldownCompleted.Count; l++)
		{
			CombatParticipant combatParticipant3 = m_localCooldownCompleted[l];
			if (combatParticipant3 != combatParticipant2)
			{
				combatParticipant3.SchedulerPriority *= combatParticipant3.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseSchedulerPriorityMultiplier);
			}
		}
	}

	private bool EnemyAttackInProgress()
	{
		bool flag = false;
		for (int i = 1; i < m_participants.Count; i++)
		{
			if (flag)
			{
				break;
			}
			switch (m_participants[i].ActiveBehaviour.BehaviourType)
			{
			case ECombatBehaviourType.Attack:
			case ECombatBehaviourType.SkillAct:
			case ECombatBehaviourType.Taunt:
				flag |= !m_participants[i].AttackHasHit;
				break;
			}
		}
		return flag;
	}

	private void CloseEnemiesEnterCombat()
	{
		if (PlayerExitCombatMode != 0)
		{
			return;
		}
		Vector3 position = PlayerParticipant.Position;
		float num = AutoEnterCombatDistance * AutoEnterCombatDistance;
		EntityManager entityManager = LogicMain.EntityManager;
		int enemyCharacterCount = entityManager.EnemyCharacterCount;
		for (int i = 0; i < enemyCharacterCount; i++)
		{
			EnemyCharacter enemyCharacter = entityManager.GetEnemyCharacter(i);
			if (enemyCharacter.IsAttached && !enemyCharacter.CombatParticipantProxy.IsInCombat && !enemyCharacter.GetDeadOrAboutToDie())
			{
				Vector3 position2 = enemyCharacter.Position;
				if (!(position2.DistanceXZSquared(position) > num) && enemyCharacter.EnemyCharacterStateMachines.CharacterCondition.IsActionAllowed(CharacterCondition.EActionAllowed.AutoEnterCombat) && !enemyCharacter.GetIsInParcour() && !enemyCharacter.GetIsRangedUnsheathedOrInAnim(EWeaponType.Crossbow) && CombatHelper.IsCombatReachable(position2, position))
				{
					AddParticipant(enemyCharacter, EnterCombatMode.Auto);
				}
			}
		}
	}

	public void OnEntityRemovedFromList(IEntity _entity, int _idx)
	{
		if (!IsActive)
		{
			return;
		}
		int count = m_participants.Count;
		for (int i = 0; i < count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			if (combatParticipant.Entity.Id == _entity.Id)
			{
				combatParticipant.ExitCombat();
				break;
			}
		}
	}

	private void UpdateDesiredPositions()
	{
		int walkableLayerMaskPreset = NavMeshLayers.WalkableLayerMaskPreset;
		for (int i = 0; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			if (NavigationHelper.CascadingSamplePosition(combatParticipant.Position, out m_dummyHit, walkableLayerMaskPreset))
			{
				combatParticipant.Position = m_dummyHit.position;
			}
		}
		for (int j = 0; j < m_participants.Count; j++)
		{
			if (m_participants[j].DoFullUpdate)
			{
				UpdateUndisturbedPosition(m_participants[j]);
			}
		}
		for (int k = 0; k < m_participants.Count; k++)
		{
			if (m_participants[k].DoFullUpdate)
			{
				UpdateSolvedPosition(m_participants[k]);
			}
		}
	}

	private void UpdateUndisturbedPosition(CombatParticipant _participant)
	{
		int walkableLayerMaskPreset = NavMeshLayers.WalkableLayerMaskPreset;
		CombatParticipant nextTarget = _participant.NextTarget;
		if (_participant.IsDead || nextTarget == null)
		{
			return;
		}
		Vector3 position = _participant.Position;
		Vector3 position2 = nextTarget.Position;
		if (NavMesh.CalculatePath(position, position2, walkableLayerMaskPreset, m_pathPart1) && m_pathPart1.status == NavMeshPathStatus.PathComplete)
		{
			_participant.TargetPathDistance = m_pathPart1.CalculateLength();
			_participant.CouldNotReachTarget = 0f;
		}
		else
		{
			_participant.TargetPathDistance = float.MaxValue;
			_participant.CouldNotReachTarget += Mathf.Max(_participant.LastFullUpdate, MaxFullUpdateInterval);
		}
		float num = 0f;
		float num2 = 0f;
		bool flag = false;
		if (NavMesh.CalculatePath(position, _participant.DesiredPosition, walkableLayerMaskPreset, m_pathPart1) && m_pathPart1.status == NavMeshPathStatus.PathComplete)
		{
			num = m_pathPart1.CalculateLength();
		}
		if (NavMesh.CalculatePath(_participant.DesiredPosition, position2, walkableLayerMaskPreset, m_pathPart2) && m_pathPart2.status == NavMeshPathStatus.PathComplete)
		{
			float num3 = 0f;
			for (int num4 = m_pathPart2.corners.Length - 1; num4 >= 1; num4--)
			{
				float num5 = Vector3.Distance(m_pathPart2.corners[num4], m_pathPart2.corners[num4 - 1]);
				if (!flag && num3 + num5 >= _participant.DesiredTargetDistance)
				{
					float t = (_participant.DesiredTargetDistance - num3) / num5;
					_participant.DesiredPositionUndisturbed = Vector3.Lerp(m_pathPart2.corners[num4], m_pathPart2.corners[num4 - 1], t);
					flag = true;
					if (_participant.IsPlayer)
					{
					}
				}
				num3 += num5;
			}
			num2 = num3;
		}
		if (flag)
		{
			if (num + num2 - _participant.DesiredTargetDistance > EnemySolutionDistance)
			{
				NavMeshPath navMeshPath = ((!(num > EnemySolutionDistance)) ? m_pathPart2 : m_pathPart1);
				float num6 = ((!(num > EnemySolutionDistance)) ? num : 0f);
				for (int i = 1; i < navMeshPath.corners.Length; i++)
				{
					float num7 = Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i - 1]);
					if (num6 + num7 >= EnemySolutionDistance)
					{
						float t2 = (EnemySolutionDistance - num6) / num7;
						_participant.DesiredPositionUndisturbed = Vector3.Lerp(navMeshPath.corners[i - 1], navMeshPath.corners[i], t2);
						break;
					}
					num6 += num7;
				}
			}
			if (_participant.IsPlayer)
			{
			}
		}
		else
		{
			Vector3 vector = position2 - position;
			float magnitude = vector.magnitude;
			vector = ((!(magnitude > 0.01f)) ? _participant.Character.UnityComponentCache.Transform.forward : (vector / magnitude));
			magnitude = Mathf.Clamp(_participant.DesiredTargetDistance, magnitude - EnemySolutionDistance, magnitude + EnemySolutionDistance);
			_participant.DesiredPositionUndisturbed = position2 - vector * magnitude;
			if (NavigationHelper.CascadingSamplePosition(_participant.DesiredPositionUndisturbed, out m_dummyHit, walkableLayerMaskPreset))
			{
				_participant.DesiredPositionUndisturbed = m_dummyHit.position;
			}
			if (_participant.IsPlayer)
			{
			}
		}
	}

	private void UpdateSolvedPosition(CombatParticipant _participant)
	{
		float num = _participant.LastFullUpdate * EnemyPositioningSensitivity;
		num /= num + 0.1f;
		int walkableLayerMaskPreset = NavMeshLayers.WalkableLayerMaskPreset;
		CombatParticipant nextTarget = _participant.NextTarget;
		if (_participant.IsDead || nextTarget == null)
		{
			return;
		}
		Vector3 position = _participant.Position;
		Vector3 position2 = nextTarget.Position;
		Vector3 desiredPositionUndisturbed = _participant.DesiredPositionUndisturbed;
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			if (combatParticipant == _participant || combatParticipant == nextTarget)
			{
				continue;
			}
			Vector3 vector = desiredPositionUndisturbed - combatParticipant.DesiredPositionUndisturbed;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float num2 = AvoidDistance(combatParticipant, _participant);
			if (!(magnitude >= num2))
			{
				if (magnitude < 0.001f)
				{
					vector = _participant.Character.UnityComponentCache.Transform.forward;
					vector.y = 0f;
					vector.Normalize();
				}
				else
				{
					vector /= magnitude;
				}
				if (Vector3.Dot(vector, position - position2) > 0f)
				{
					float num3 = magnitude / num2;
					zero += vector * (1f - num3 * num3 * num3 * num3);
				}
			}
		}
		desiredPositionUndisturbed = NavMeshRaycastLerp(desiredPositionUndisturbed, desiredPositionUndisturbed + zero, Mathf.Clamp01(num * EnemyAvoidanceDamping), walkableLayerMaskPreset);
		if (_participant.TargetPathDistance < EnemySurroundMaxRadius && !_participant.IsPlayer)
		{
			Vector3 vector2 = position2 - desiredPositionUndisturbed;
			vector2.y = 0f;
			float magnitude2 = (position2 - position).magnitude;
			Vector3 rhs = new Vector3(vector2.z, 0f, 0f - vector2.x);
			float num4 = EnemySurroundCoverAngle;
			float num5 = 0f - EnemySurroundCoverAngle;
			float num6 = EnemyFallbackFactorBase;
			for (int j = 0; j < m_participants.Count; j++)
			{
				CombatParticipant combatParticipant2 = m_participants[j];
				if (combatParticipant2 == _participant || combatParticipant2 == nextTarget)
				{
					continue;
				}
				Vector3 vector3 = position2 - combatParticipant2.DesiredPositionUndisturbed;
				vector3.y = 0f;
				float magnitude3 = (position2 - combatParticipant2.Position).magnitude;
				float num7 = AvoidDistance(combatParticipant2, _participant);
				if (!(Mathf.Abs(magnitude2 - magnitude3) > num7))
				{
					float num8 = Vector3.Angle(vector3, vector2);
					if (magnitude3 > magnitude2)
					{
						num8 *= 1f + 3f * ((magnitude3 - magnitude2) / num7);
					}
					if (Vector3.Dot(vector3, rhs) > 0f)
					{
						num4 = Mathf.Min(num4, num8);
					}
					else
					{
						num5 = Mathf.Max(num5, 0f - num8);
					}
					if (magnitude2 > magnitude3)
					{
						float num9 = (combatParticipant2.Position - position).magnitude / EnemyFallbackDistance;
						num6 += Mathf.Max(1f - num9 * num9, 0f);
					}
				}
			}
			num6 = Mathf.Pow(2f * num6, 5f);
			num6 = Mathf.Clamp(num6, -1f, 1f);
			_participant.DesiredTargetDistanceInternal += num6 * num * EnemyFallbackDamping;
			_participant.DesiredTargetDistanceInternal = Mathf.Clamp(_participant.DesiredTargetDistanceInternal, EnemyBestAttackDistance, EnemySurroundMaxRadius);
			if (_participant.IsPlayer)
			{
				_participant.DesiredTargetDistance = _participant.DesiredTargetDistanceInternal;
			}
			else
			{
				Vector3 velocity = PlayerParticipant.Agent.velocity;
				Vector3 rhs2 = _participant.Position - PlayerParticipant.Position;
				float magnitude4 = velocity.magnitude;
				float magnitude5 = rhs2.magnitude;
				float strafingFactor = AcierInputHandler.Instance.CombatInterpreter.StrafingFactor;
				if (magnitude4 > 0.1f)
				{
					velocity *= 0.1f / magnitude4;
				}
				if (magnitude5 > 0.1f)
				{
					rhs2 *= 0.1f / magnitude5;
				}
				strafingFactor *= Vector3.Dot(velocity, rhs2) / 0.010000001f;
				_participant.DesiredTargetDistance = _participant.DesiredTargetDistanceInternal + strafingFactor * EnemyVariationAttackDistance;
			}
			float num10 = (num4 + num5) * 0.5f;
			if (Mathf.Abs(num10) > EnemySurroundIgnoreAngle)
			{
				float value = Mathf.InverseLerp(EnemySurroundMaxRadius, EnemyBestAttackDistance, _participant.TargetPathDistance);
				value = Mathf.Clamp01(value);
				Vector3 desired = position2 - Quaternion.Euler(0f, num10, 0f) * vector2;
				desiredPositionUndisturbed = NavMeshRaycastLerp(desiredPositionUndisturbed, desired, Mathf.Clamp01(num * EnemySurroundDamping * value), walkableLayerMaskPreset);
			}
		}
		desiredPositionUndisturbed = Vector3.Lerp(_participant.DesiredPosition, desiredPositionUndisturbed, Mathf.Clamp01(num * EnemyFinalDamping));
		if (NavigationHelper.SafeRaycast(_participant.DesiredPositionUndisturbed, desiredPositionUndisturbed, out m_dummyHit, walkableLayerMaskPreset))
		{
			_participant.DesiredPosition = m_dummyHit.position;
		}
		else
		{
			_participant.DesiredPosition = desiredPositionUndisturbed;
		}
		if (!_participant.IsPlayer)
		{
		}
	}

	private float AvoidDistance(CombatParticipant _participant1, CombatParticipant _participant2)
	{
		return Mathf.Max(_participant1.Agent.radius + _participant2.Agent.radius + StrafeDistanceStop, EnemyAvoidanceRadius);
	}

	private static Vector3 NavMeshRaycastLerp(Vector3 _current, Vector3 _desired, float _factor, int _walkableMask)
	{
		_desired = Vector3.Lerp(_current, _desired, _factor);
		NavMeshHit _hit;
		if (NavigationHelper.SafeRaycast(_current, _desired, out _hit, _walkableMask))
		{
			return _hit.position;
		}
		return _desired;
	}

	private bool IsTooFarFromEnemies()
	{
		if (ParticipantCount() == 1)
		{
			return true;
		}
		if (!EnableAutoExitCombat)
		{
			return false;
		}
		bool result = true;
		float num = AutoExitCombatDistance * AutoExitCombatDistance;
		int count = m_participants.Count;
		for (int i = 0; i < count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			float num2 = ((!combatParticipant.IsPlayer) ? (combatParticipant.Position - PlayerParticipant.Position).sqrMagnitude : float.MaxValue);
			ECombatBehaviourType behaviourType = combatParticipant.ActiveBehaviour.BehaviourType;
			if (num2 < num || behaviourType == ECombatBehaviourType.InitAttack || behaviourType == ECombatBehaviourType.InitWithSkill)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	private void ExitCombat(ExitCombatMode _mode)
	{
		PlayerExitCombatMode = _mode;
		IsActive = false;
		m_LastJoinedParticipant = null;
		Vector3 zero = Vector3.zero;
		int num = 0;
		while (m_participants.Count > 0)
		{
			CombatParticipant combatParticipant = m_participants[0];
			if (combatParticipant.IsInCombat)
			{
				zero += combatParticipant.Position;
				num++;
				combatParticipant.ExitCombat();
			}
		}
		if (num > 0)
		{
			zero /= (float)num;
			Crowd.Instance.AddHotspot(new CrowdHotspotData
			{
				DynamicWeight = 0f,
				EndTime = LogicMain.CurrentTime + 20f,
				Radius = Crowd.FleeCombatRadius,
				Position = zero,
				Flee = true
			});
		}
		if (PlayerExitCombatMode == ExitCombatMode.PlayerFlee)
		{
			LogicMain.MissionResult.IncreaseFledCounter();
			PlayerHelper.PlayerMain.ScheduleStateChange(EStateType.PlayerMain_BeingChased);
		}
		else if (PlayerExitCombatMode == ExitCombatMode.Disguise)
		{
			PlayerHelper.PlayerMain.ChangeStateForced(EStateType.CombatCharacterMain_Roaming);
		}
		else if (PlayerHelper.PlayerVitality.CurrentStateType == EStateType.Character_Alive)
		{
			PlayerHelper.PlayerMain.ScheduleStateChange(EStateType.CombatCharacterMain_Roaming);
		}
		if (PlayerExitCombatMode == ExitCombatMode.EnemiesTooFar)
		{
			HandleCamouflage();
		}
		ActiveHighlight = null;
		if (_mode == ExitCombatMode.PlayerFlee)
		{
			PlayerHelper.Entity.Highlight.SetMode(Highlight.EHighlightLayer.Fleeing, Highlight.ESelectionMode.Hiding);
		}
	}

	private static void HandleCamouflage()
	{
		Enum<SkillTypes> type = "Camouflage_01".ToEnum<SkillTypes>();
		PlayerCharacter playerEntity = LogicMain.PlayerEntity;
		if (playerEntity.Skills.Has(type) && playerEntity.PlayerProfileCarrier.PlayerProfile.CurrentStateType != EStateType.PlayerProfile_Chase)
		{
			float num = playerEntity.EntityStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
			float a = num - playerEntity.Skills.GetBuffedStat_float(type, StatTypes.EStatType.Rule_NotorietyDecrease);
			a = Mathf.Max(a, 0f);
			if (a < num)
			{
				playerEntity.PlayerCharacterStateMachines.PlayerAlertLevel.ForceNotoriety(a);
			}
		}
	}

	private void OnCharacterExit(CombatParticipant _participant)
	{
		if (_participant.IsPlayer)
		{
			PlayerParticipant = null;
		}
		if (!_participant.IsPlayer && IsActive)
		{
			for (int i = 1; i < m_participants.Count; i++)
			{
				CombatParticipant combatParticipant = m_participants[i];
				if (combatParticipant != _participant)
				{
					if (_participant.IsDead)
					{
						combatParticipant.AllyDiedFlag = Mathf.Min(combatParticipant.AllyDiedFlag + 1, 5);
					}
					else if (_participant.WantsToFlee)
					{
						combatParticipant.AllyFledFlag = Mathf.Min(combatParticipant.AllyFledFlag + 1, 5);
					}
				}
			}
		}
		LogicMain.LogicInterface.Combat_Exit(_participant.Proxy.Character);
		DetachParticipant(_participant);
	}

	public void PlayerWantsToFlee()
	{
		if (IsActive)
		{
			if (PlayerExitCombatMode == ExitCombatMode.Invalid)
			{
				PlayerExitCombatMode = ExitCombatMode.EndOfCombatImminent;
			}
			PlayerParticipant.WantsToFlee = true;
		}
	}

	public void EndLikeNothingHappened(ExitCombatMode _exitMode)
	{
		if (IsActive)
		{
			ExitCombat(_exitMode);
		}
	}

	public void UpdateFinalKillTarget(CombatParticipant _participant)
	{
		if (_participant.AttackWasKillMove && ParticipantCount() <= 2)
		{
			FinalKillTarget = _participant.CurrentTarget.Character.Id;
		}
		else
		{
			FinalKillTarget = EntityId.InvalidId;
		}
	}

	public ECombatCameraMode RecommendedCameraMode(bool _inCameraEvent)
	{
		IParticipantCarrier participantCarrier = FinalKillTarget.TryGetEntity<IParticipantCarrier>();
		if (_inCameraEvent)
		{
			m_finalKillCameraStarted = true;
			if (participantCarrier != null)
			{
				return ECombatCameraMode.Kill;
			}
		}
		if (!m_finalKillCameraStarted)
		{
			return ECombatCameraMode.Combat;
		}
		if (!IsActive)
		{
			return ECombatCameraMode.None;
		}
		int num = 1;
		if (participantCarrier != null && participantCarrier.CombatParticipantProxy.IsInCombat)
		{
			num++;
		}
		return (ParticipantCount() > num) ? ECombatCameraMode.Combat : ECombatCameraMode.None;
	}

	public bool FindTargetAtScreenPosition(CombatParticipant _source, Vector2 _screenPosition)
	{
		if (!_source.IsPlayer)
		{
			_source.NextTarget = (PlayerParticipant.IsDead ? null : PlayerParticipant);
			return true;
		}
		CombatParticipant combatParticipant = null;
		float num = float.PositiveInfinity;
		Ray ray = Camera.main.ScreenPointToRay(_screenPosition);
		int count = m_participants.Count;
		for (int i = 0; i < count; i++)
		{
			CombatParticipant combatParticipant2 = m_participants[i];
			if (combatParticipant2.IsDead || combatParticipant2 == _source || combatParticipant2.GameObject == null)
			{
				continue;
			}
			Collider componentInChildren = combatParticipant2.GameObject.GetComponentInChildren<Collider>();
			RaycastHit hitInfo;
			if (componentInChildren != null && componentInChildren.Raycast(ray, out hitInfo, float.PositiveInfinity))
			{
				Vector3 vector = Camera.main.WorldToScreenPoint(componentInChildren.bounds.center);
				float sqrMagnitude = new Vector2(vector.x - _screenPosition.x, vector.y - _screenPosition.y).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					combatParticipant = combatParticipant2;
				}
			}
		}
		if (combatParticipant != null)
		{
			if (_source.NextTarget != combatParticipant)
			{
				LogicMain.MissionResult.IncreaseSwitchedTarget();
			}
			_source.NextTarget = combatParticipant;
			return true;
		}
		return false;
	}

	public bool FindTargetInDirection(CombatParticipant _source, Vector3 _aim)
	{
		if (!_source.IsPlayer)
		{
			_source.NextTarget = (PlayerParticipant.IsDead ? null : PlayerParticipant);
			return true;
		}
		Vector3 position = _source.Position;
		float num = Mathf.Cos(MaxTargetAngle * ((float)Math.PI / 180f));
		float maxTargetDistance = MaxTargetDistance;
		CombatParticipant combatParticipant = null;
		int count = m_participants.Count;
		for (int i = 0; i < count; i++)
		{
			CombatParticipant combatParticipant2 = m_participants[i];
			if (!combatParticipant2.IsDead && combatParticipant2 != _source)
			{
				Vector3 position2 = combatParticipant2.Position;
				Vector3 vector = position2 - position;
				Vector3 normalized = vector.normalized;
				float magnitude = vector.magnitude;
				float num2 = Vector3.Dot(normalized, _aim);
				if (num2 > num && magnitude < maxTargetDistance)
				{
					num = num2;
					combatParticipant = combatParticipant2;
				}
			}
		}
		if (combatParticipant != null)
		{
			if (_source.NextTarget != combatParticipant)
			{
				LogicMain.MissionResult.IncreaseSwitchedTarget();
			}
			_source.NextTarget = combatParticipant;
			return true;
		}
		return false;
	}

	public bool FindClosestTarget(CombatParticipant _source)
	{
		if (!_source.IsPlayer)
		{
			_source.NextTarget = ((PlayerParticipant == null || PlayerParticipant.IsDead) ? null : PlayerParticipant);
			return true;
		}
		Vector3 position = _source.Position;
		float num = MaxTargetDistance;
		CombatParticipant combatParticipant = null;
		int count = m_participants.Count;
		for (int i = 0; i < count; i++)
		{
			CombatParticipant combatParticipant2 = m_participants[i];
			if (!combatParticipant2.IsDead && combatParticipant2 != _source)
			{
				Vector3 position2 = combatParticipant2.Position;
				float magnitude = (position2 - position).magnitude;
				if (magnitude < num)
				{
					num = magnitude;
					combatParticipant = combatParticipant2;
				}
			}
		}
		_source.NextTarget = combatParticipant;
		return combatParticipant != null;
	}

	private bool CheckPlayerEscapeAllowed()
	{
		return true;
	}

	public void TriggerFleeRoll(CombatParticipant _victim, float _fearFactor)
	{
		for (int i = 1; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant = m_participants[i];
			if (CombatHelper.DecideToFlee(PlayerParticipant, combatParticipant, _victim, _fearFactor))
			{
				combatParticipant.WantsToFlee = true;
			}
		}
	}

	public bool CanParticipantJoin(Character _newCandidate)
	{
		if (ParticipantCount() < MaxParticipantCount)
		{
			return true;
		}
		float num = AutoEnterCombatDistance * AutoEnterCombatDistance * 0.25f;
		Vector3 position = PlayerParticipant.Position;
		float num2 = Vector3.Distance(_newCandidate.Position, position) + 1.5f;
		num2 *= num2;
		CombatParticipant combatParticipant = null;
		for (int i = 1; i < m_participants.Count; i++)
		{
			CombatParticipant combatParticipant2 = m_participants[i];
			CharacterCondition characterCondition = combatParticipant2.Character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			if (characterCondition.CurrentStateType == EStateType.CharacterCondition_Normal && (combatParticipant2.ActiveBehaviour == null || combatParticipant2.ActiveBehaviour.BehaviourType == ECombatBehaviourType.Idle))
			{
				float num3 = combatParticipant2.Position.DistanceSquared(position);
				if (num3 > num2)
				{
					num2 = num3;
					combatParticipant = combatParticipant2;
				}
			}
		}
		Character character = null;
		if (combatParticipant != null)
		{
			character = combatParticipant.Character;
		}
		else if (num2 < num)
		{
			character = _newCandidate;
		}
		if (character != null)
		{
			CombatHelper.StartFleeing(character, FleeingConditionState.Reason.WalkFromCombat);
		}
		if (combatParticipant != null)
		{
			combatParticipant.ExitCombat();
			return true;
		}
		return false;
	}
}
