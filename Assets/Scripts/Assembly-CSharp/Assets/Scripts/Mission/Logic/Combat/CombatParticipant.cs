using System;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public class CombatParticipant : TreeRuntimeData, IAnimatorEventListener, IPositionProvider
	{
		public enum EActionQueuedType
		{
			Attack = 0,
			Taunt = 1
		}

		public delegate void ChangeHandler(CombatParticipant _participant);

		public delegate void OnCombatBehaviourTypeChangedDelegate(ECombatBehaviourType _type, params object[] _params);

		public delegate void ChangedDelegate<in T>(T _old, T _new);

		private CombatParticipantProxy m_Proxy;

		public NavMeshAgent Agent;

		private Animator m_animator;

		private AnimatorDataController m_animatorDataController;

		private AnimationController m_animationController;

		private readonly CombatController m_combatController;

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private NavMeshPath m_path__lazy;

		private NavMeshPath m_path
		{
			get
			{
				return m_path__lazy ?? (m_path__lazy = new NavMeshPath());
			}
		}

		private bool m_dead;

		private CombatParticipant m_currentTarget;

		private Vector3 m_previousPosition;

		private float m_combatStance;

		private bool m_knowinglyInCombat;

		private int m_actionsQueued;

		private bool m_attackWasKillMove;

		private readonly AudioCombatParticipantListener m_audioCombatParticipantListener;

		private readonly CombatBehaviour[] m_behaviours;

		private Vector3 m_cachedPosition;

		private Vector3 m_cachedPositionEyes;

		private Quaternion m_cachedRotation;

		private bool m_isInUpdate;

		public CombatParticipant NextTarget
		{
			get
			{
				return Proxy.NextTarget;
			}
			set
			{
				if (Proxy.NextTarget != value)
				{
					if (Proxy.NextTarget != null)
					{
						CombatParticipant nextTarget = Proxy.NextTarget;
						nextTarget.OnDie = (ChangeHandler)Delegate.Remove(nextTarget.OnDie, new ChangeHandler(OnNextTargetVanished));
						CombatParticipant nextTarget2 = Proxy.NextTarget;
						nextTarget2.OnExitCombat = (ChangeHandler)Delegate.Remove(nextTarget2.OnExitCombat, new ChangeHandler(OnNextTargetVanished));
					}
					Proxy.NextTarget = value;
					if (Proxy.NextTarget != null)
					{
						DesiredPosition = Proxy.NextTarget.Position;
						float desiredTargetDistance = (DesiredTargetDistanceInternal = CombatController.Instance.EnemyBestAttackDistance);
						DesiredTargetDistance = desiredTargetDistance;
						TargetPathDistance = float.MaxValue;
						CombatParticipant nextTarget3 = Proxy.NextTarget;
						nextTarget3.OnDie = (ChangeHandler)Delegate.Combine(nextTarget3.OnDie, new ChangeHandler(OnNextTargetVanished));
						CombatParticipant nextTarget4 = Proxy.NextTarget;
						nextTarget4.OnExitCombat = (ChangeHandler)Delegate.Combine(nextTarget4.OnExitCombat, new ChangeHandler(OnNextTargetVanished));
					}
				}
			}
		}

		public bool IsPlayer
		{
			get
			{
				return Proxy.IsPlayer;
			}
		}

		public bool IsInCombat
		{
			get
			{
				return Proxy.IsInCombat;
			}
			private set
			{
				Proxy.IsInCombat = value;
			}
		}

		public IEntity Entity
		{
			get
			{
				return Proxy.Entity;
			}
		}

		public GameObject GameObject
		{
			get
			{
				return Proxy.GameObject;
			}
		}

		public IGameEntity GameEntity
		{
			get
			{
				return Proxy.GameEntity;
			}
		}

		public Character Character
		{
			get
			{
				return Proxy.GameEntity.Cast<Character>();
			}
		}

		public CombatController CombatController
		{
			get
			{
				return m_combatController;
			}
		}

		public CombatParticipantProxy Proxy
		{
			get
			{
				return m_Proxy;
			}
			set
			{
				if (m_Proxy != value)
				{
					if (m_Proxy != null)
					{
						m_animatorDataController.UnregisterEventListener(this);
						m_audioCombatParticipantListener.Detach();
						m_animatorDataController = null;
						m_animationController.TaskManager.AnimationTaskExited -= OnAnimationTaskExited;
						m_animationController = null;
						Agent = null;
						m_animator = null;
						Highlight = null;
						CurrentTarget = null;
						NextTarget = null;
					}
					m_Proxy = value;
					if (m_Proxy != null)
					{
						m_animatorDataController = m_Proxy.GameObject.GetComponent<AnimatorDataController>();
						m_animator = m_Proxy.GameEntity.UnityComponentCache.Animator;
						m_animationController = m_Proxy.GameEntity.UnityComponentCache.AnimationController;
						m_animationController.TaskManager.AnimationTaskExited += OnAnimationTaskExited;
						Agent = m_Proxy.GameEntity.UnityComponentCache.NavMeshAgent;
						m_animatorDataController.RegisterEventListener(this);
						m_audioCombatParticipantListener.Attach(m_animatorDataController, m_Proxy.GameObject.GetComponent<WeaponHandler>(), m_Proxy.GameObject.GetComponent<AudioSource>());
						Highlight = m_Proxy.GameEntity.Highlight;
						IsDead = Character.GetIsDead();
						m_previousPosition = m_Proxy.GameObject.transform.position;
					}
				}
			}
		}

		public IBehaviorTreeHolder BehaviorTreeHolder { get; private set; }

		public bool IsDead
		{
			get
			{
				return m_dead;
			}
			set
			{
				if (m_dead != value)
				{
					m_dead = value;
					if (m_dead && this.OnDie != null)
					{
						this.OnDie(this);
					}
				}
			}
		}

		public CombatParticipant CurrentTarget
		{
			get
			{
				return m_currentTarget;
			}
			set
			{
				if (m_currentTarget != value)
				{
					if (m_currentTarget != null)
					{
						CombatParticipant currentTarget = m_currentTarget;
						currentTarget.OnDie = (ChangeHandler)Delegate.Remove(currentTarget.OnDie, new ChangeHandler(OnCurrentTargetVanished));
						CombatParticipant currentTarget2 = m_currentTarget;
						currentTarget2.OnExitCombat = (ChangeHandler)Delegate.Remove(currentTarget2.OnExitCombat, new ChangeHandler(OnCurrentTargetVanished));
					}
					if (this.CurrentTargetChanged != null)
					{
						this.CurrentTargetChanged(m_currentTarget, value);
					}
					m_currentTarget = value;
					if (m_currentTarget != null)
					{
						CombatParticipant currentTarget3 = m_currentTarget;
						currentTarget3.OnDie = (ChangeHandler)Delegate.Combine(currentTarget3.OnDie, new ChangeHandler(OnCurrentTargetVanished));
						CombatParticipant currentTarget4 = m_currentTarget;
						currentTarget4.OnExitCombat = (ChangeHandler)Delegate.Combine(currentTarget4.OnExitCombat, new ChangeHandler(OnCurrentTargetVanished));
					}
				}
			}
		}

		public bool KnowinglyInCombat
		{
			get
			{
				return m_knowinglyInCombat;
			}
			set
			{
				if (!m_knowinglyInCombat && value)
				{
					m_animationController.TaskManager.CreateAnimationTaskCombat(Entity.Cast<IParticipantCarrier>());
					if (IsPlayer)
					{
						EStateType currentStateType = Character.Cast<AssassinCharacter>().PlayerProfileCarrier.PlayerProfile.CurrentStateType;
						IdleType type = ((currentStateType == EStateType.PlayerProfile_Low) ? IdleType.LowProfile : IdleType.HighProfile);
						Animator.SetCurrentIdleType(type);
					}
					else
					{
						Animator.SetCurrentIdleType(IdleType.Neutral);
						if (!EnemyHelper.EnterCombat(GameEntity.Cast<EnemyCharacter>()))
						{
							value = false;
						}
					}
					Character.Cast<IParticipantCarrier>().RemoveBuffsOnEnterCombat();
				}
				m_knowinglyInCombat = value;
			}
		}

		public bool AttackWasKillMove
		{
			get
			{
				return m_attackWasKillMove;
			}
			set
			{
				if (m_attackWasKillMove != value)
				{
					if (this.AttackWasKillMoveChanged != null)
					{
						this.AttackWasKillMoveChanged(m_attackWasKillMove, value);
					}
					m_attackWasKillMove = value;
				}
			}
		}

		public CombatBehaviour ActiveBehaviour { get; private set; }

		public Highlight Highlight { get; private set; }

		public bool AttackWasParried { get; set; }

		public bool AttackWasCombo { get; set; }

		public bool AttackHasHit { get; set; }

		public bool AttackWasDodged { get; set; }

		public bool KillMoveWasParried { get; set; }

		public bool ActionQueued
		{
			get
			{
				return m_actionsQueued > 0;
			}
		}

		public EActionQueuedType ActionQueuedType { get; private set; }

		public float AttackTime { get; set; }

		public bool WantsToFlee { get; set; }

		public bool SkillQueued
		{
			get
			{
				return IsPlayer && PlayerHelper.SelectedSkill.IsValid;
			}
		}

		public bool ActionOrSkillQueued
		{
			get
			{
				return ActionQueued || SkillQueued;
			}
		}

		public bool EnableAttack { get; set; }

		public bool EnableParry { get; set; }

		public bool EnableDodge { get; set; }

		public bool EnableParryKillMove { get; set; }

		public float SchedulerPriority { get; set; }

		public float SchedulerCooldownMultiplier { get; set; }

		public float SchedulerCooldownStartTime { get; set; }

		public bool SchedulerCanAttack { get; set; }

		public Vector3 DesiredPositionUndisturbed { get; set; }

		public Vector3 DesiredPosition { get; set; }

		public float CouldNotReachTarget { get; set; }

		public float TargetPathDistance { get; set; }

		public float DesiredTargetDistance { get; set; }

		public float DesiredTargetDistanceInternal { get; set; }

		public int AllyDiedFlag { get; set; }

		public int AllyFledFlag { get; set; }

		public bool IsAttackedFlag { get; set; }

		public int AttackComboCount { get; set; }

		public CombatParticipant LastAttacker { get; set; }

		public float LastFullUpdate { get; set; }

		public bool DoFullUpdate { get; set; }

		public float WarningEndTime { get; set; }

		public float GlobalSchedulerCooldownMultiplier { get; set; }

		public float GlobalFleeChanceMultiplier { get; set; }

		public Vector3 Position
		{
			get
			{
				return (!m_isInUpdate) ? GameEntity.Position : m_cachedPosition;
			}
			set
			{
				if (m_isInUpdate)
				{
					m_cachedPosition = value;
				}
				else
				{
					GameEntity.Position = value;
				}
			}
		}

		public Vector3 PositionEyes
		{
			get
			{
				return (!m_isInUpdate) ? Animator.GetBoneTransform(HumanBodyBones.Head).position : m_cachedPositionEyes;
			}
		}

		public Quaternion Rotation
		{
			get
			{
				return (!m_isInUpdate) ? GameEntity.Rotation : m_cachedRotation;
			}
			set
			{
				if (m_isInUpdate)
				{
					m_cachedRotation = value;
				}
				else
				{
					GameEntity.Rotation = value;
				}
			}
		}

		public Vector3 Scale
		{
			get
			{
				return GameEntity.Scale;
			}
			set
			{
				GameEntity.Scale = value;
			}
		}

		public Animator Animator
		{
			get
			{
				return m_animator;
			}
		}

		public event ChangeHandler OnExitCombat;

		public event ChangeHandler OnDie;

		public event OnCombatBehaviourTypeChangedDelegate OnCombatBehaviourTypeChanged;

		public event ChangedDelegate<CombatParticipant> CurrentTargetChanged;

		public event ChangedDelegate<bool> AttackWasKillMoveChanged;

		public CombatParticipant(CombatController _owner)
		{
			m_combatController = _owner;
			m_behaviours = new CombatBehaviour[13]
			{
				null,
				new InitAttackBehaviour(this),
				new InitUnawareBehaviour(this),
				new InitWithSkillBehaviour(this),
				new IdleBehaviour(this),
				new AttackBehaviour(this),
				new ParryBehaviour(this),
				new GetHitBehaviour(this),
				new DieBehaviour(this),
				new SkillActBehaviour(this),
				new SkillReactBehaviour(this),
				new ExternalBehaviour(this),
				new TauntBehaviour(this)
			};
			m_audioCombatParticipantListener = new AudioCombatParticipantListener(this);
			ActionQueuedType = EActionQueuedType.Attack;
		}

		public void Update()
		{
			if (!GameEntity.IsAttached)
			{
				ExitCombat();
				return;
			}
			if (Character.GetIsDead() && ActiveBehaviour.BehaviourType != ECombatBehaviourType.Die)
			{
				IsDead = true;
				ExitCombat();
				return;
			}
			if (!IsPlayer && CouldNotReachTarget > m_combatController.MaxCouldNotReachTargetTime)
			{
				ExitCombat();
				return;
			}
			if (DoFullUpdate)
			{
				Behave();
			}
			CheckForCancelAction();
			if (SkillQueued)
			{
				CancelAction();
			}
			if (ActionOrSkillQueued && ActiveBehaviour.Act())
			{
				ConsumeAction();
			}
			ParryStance();
			Animator.speed = 1f;
			ActiveBehaviour.Update();
		}

		public bool ReadyForExitCombat()
		{
			if (ActiveBehaviour == null)
			{
				return false;
			}
			if (!KnowinglyInCombat)
			{
				return true;
			}
			switch (ActiveBehaviour.BehaviourType)
			{
			case ECombatBehaviourType.InitAttack:
			case ECombatBehaviourType.InitUnaware:
			case ECombatBehaviourType.Idle:
			case ECombatBehaviourType.Die:
				return true;
			case ECombatBehaviourType.External:
			{
				CharacterCondition characterCondition = Character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				return characterCondition.CurrentStateType != EStateType.CharacterCondition_Normal;
			}
			default:
				return false;
			}
		}

		public void EnterCombat(EnterCombatMode _mode)
		{
			Init();
			IsInCombat = true;
			m_isInUpdate = false;
			m_knowinglyInCombat = false;
			IsDead = false;
			CancelAction();
			AttackWasParried = false;
			KillMoveWasParried = false;
			AttackWasKillMove = false;
			AttackHasHit = false;
			AttackWasDodged = false;
			EnableParry = false;
			EnableDodge = false;
			EnableParryKillMove = false;
			EnableAttack = true;
			WantsToFlee = false;
			CurrentTarget = null;
			SchedulerPriority = 0f;
			TargetPathDistance = float.MaxValue;
			CouldNotReachTarget = 0f;
			DoFullUpdate = false;
			LastFullUpdate = 0f;
			WarningEndTime = -1f;
			GlobalSchedulerCooldownMultiplier = 1f;
			GlobalFleeChanceMultiplier = 1f;
			BehaviorTreeHolder = Proxy.RequestBehaviorTreeHolder();
			CombatHelper.ResetKillTokenDecayClock(Character);
			CharacterCondition characterCondition = Character.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
			if (characterCondition.CurrentStateType != EStateType.CharacterCondition_Normal && (_mode == EnterCombatMode.Attack || _mode == EnterCombatMode.Normal || _mode == EnterCombatMode.Skill))
			{
				Character.Cast<IParticipantCarrier>().RemoveBuffsOnEnterCombat();
				characterCondition.ExternalChange();
			}
			if (_mode != EnterCombatMode.External && characterCondition.CurrentStateType == EStateType.CharacterCondition_Normal)
			{
				if (!m_animationController.TaskManager.CreateAnimationTaskCombat(Entity.Cast<IParticipantCarrier>()))
				{
					_mode = EnterCombatMode.External;
				}
			}
			else
			{
				_mode = EnterCombatMode.External;
			}
			EnemyCharacter enemyCharacter = Character as EnemyCharacter;
			if (enemyCharacter != null && enemyCharacter.GetIsInDisguise())
			{
				enemyCharacter.SetDisguised(false);
			}
			if (IsPlayer && _mode == EnterCombatMode.Attack && Character.Cast<PlayerCharacter>().GetIsAnonymous())
			{
				Character.Buffs.AddBuff(new BooleanOrBuff(EBuffType.InitAnonymousBonus, true));
			}
			if (!IsPlayer)
			{
				Character.CanBePooled = false;
			}
			KnowinglyInCombat = _mode != EnterCombatMode.External;
			LogicMain.LogicInterface.Combat_Enter(Character);
			m_animationController.StateMachine.TransitionStarted += OnAnimatorTransitionStarted;
			m_animationController.StateMachine.StateEntered += OnAnimatorStateEntered;
			m_animationController.StateMachine.StateMachineEntered += OnAnimatorStateMachineEntered;
			Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			ECombatBehaviourType type;
			switch (_mode)
			{
			case EnterCombatMode.Attack:
				type = ECombatBehaviourType.InitAttack;
				break;
			case EnterCombatMode.Unaware:
				type = ECombatBehaviourType.InitUnaware;
				break;
			case EnterCombatMode.Skill:
				type = ECombatBehaviourType.InitWithSkill;
				break;
			case EnterCombatMode.External:
				type = ECombatBehaviourType.External;
				break;
			default:
				type = ECombatBehaviourType.Idle;
				break;
			}
			SwitchBehaviour(type);
		}

		public void ExitCombat()
		{
			Dispose();
			IsInCombat = false;
			bool flag = IsDead || Character.GetDeadOrAboutToDie();
			if (!flag)
			{
				Agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
			}
			AnimationTaskBase currentTask = m_animationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
			bool flag2 = !flag && currentTask != null && currentTask.TaskType == AnimationTaskType.Combat;
			if (flag2)
			{
				Agent.ResetPath();
				Animator.SetCurrentGeneralState(GeneralState.Default);
				Animator.SetCurrentReactionType(ReactionType.Invalid);
				Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
				Animator.SetCurrentMovementState(MovementState.Idle);
				Animator.SetCurrentComboAttack(Flag.False);
				Animator.SetCurrentStopAction(true);
				Animator.speed = 1f;
			}
			if (!IsPlayer)
			{
				EnemyHelper.ExitCombat(GameEntity.Cast<EnemyCharacter>());
				if (m_combatController.FinalKillTarget != Character.Id)
				{
					Character.CanBePooled = true;
				}
			}
			Character.ExitCombat(flag);
			m_animationController.StateMachine.TransitionStarted -= OnAnimatorTransitionStarted;
			m_animationController.StateMachine.StateEntered -= OnAnimatorStateEntered;
			m_animationController.StateMachine.StateMachineEntered -= OnAnimatorStateMachineEntered;
			LookAtDisable();
			SwitchBehaviour(ECombatBehaviourType.Invalid);
			if (this.OnExitCombat != null)
			{
				this.OnExitCombat(this);
			}
			bool flag3 = false;
			if (!flag && IsPlayer)
			{
				ExitCombatMode playerExitCombatMode = CombatController.PlayerExitCombatMode;
				flag3 = playerExitCombatMode == ExitCombatMode.Invalid || playerExitCombatMode == ExitCombatMode.EnemiesTooFar;
				int num = CombatController.ParticipantCount();
				for (int i = 0; i < num; i++)
				{
					flag3 &= CombatController.GetParticipant(i).Character.GetDeadOrAboutToDie();
				}
				if (flag3)
				{
					m_animationController.TaskManager.CreateAnimationTaskHoldCombatIdle();
				}
			}
			if (!flag3 && !flag2 && !flag && Animator.GetCurrentGeneralState() == GeneralState.Combat)
			{
				Animator.SetCurrentGeneralState(GeneralState.Default);
			}
		}

		public void SwitchBehaviour(ECombatBehaviourType _type, params object[] _params)
		{
			if (Assets.Scripts.Offline.PortDebug.Enabled)
			{
				Assets.Scripts.Offline.PortDebug.Log(string.Format("combat {0}: {1} -> {2}", (Agent != null) ? Agent.name : "?", (ActiveBehaviour != null) ? ActiveBehaviour.BehaviourType.ToString() : "none", _type));
			}
			if (ActiveBehaviour != null)
			{
				ActiveBehaviour.End();
			}
			ActiveBehaviour = null;
			if (_type != 0)
			{
				ActiveBehaviour = m_behaviours[(int)_type];
				ActiveBehaviour.Start(_params);
				if (this.OnCombatBehaviourTypeChanged != null)
				{
					this.OnCombatBehaviourTypeChanged(_type, _params);
				}
			}
		}

		internal CombatBehaviour GetBehaviour(ECombatBehaviourType _type)
		{
			return m_behaviours[(int)_type];
		}

		private void OnCurrentTargetVanished(CombatParticipant _participant)
		{
			CurrentTarget = null;
			if (NextTarget == _participant)
			{
				NextTarget = null;
				CancelAction();
			}
		}

		private void OnNextTargetVanished(CombatParticipant _participant)
		{
			NextTarget = null;
			CancelAction();
		}

		private void OnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
		{
			if (_eventArgs.AnimationTask.TaskType == AnimationTaskType.Combat && IsInCombat)
			{
				if (ActiveBehaviour != null && ActiveBehaviour.BehaviourType != ECombatBehaviourType.Die)
				{
					SwitchBehaviour(ECombatBehaviourType.External);
				}
				CancelAction();
			}
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			if (ActiveBehaviour != null)
			{
				switch (_event.Type)
				{
				case AnimatorEventType.Dodge:
					ActiveBehaviour.OnDodgeEvent(_event);
					break;
				case AnimatorEventType.Hit:
					ActiveBehaviour.OnHitEvent(_event);
					break;
				case AnimatorEventType.Parry:
					ActiveBehaviour.OnParryEvent(_event);
					break;
				case AnimatorEventType.Combo:
					ActiveBehaviour.OnComboEvent(_event);
					break;
				case AnimatorEventType.ReactionStart:
					ActiveBehaviour.OnReactionStartEvent(_event);
					break;
				case AnimatorEventType.AllowReact:
					ActiveBehaviour.OnAllowReactEvent(_event);
					break;
				}
			}
			return false;
		}

		protected virtual void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			ActiveBehaviour.OnAnimatorStateEntered(_sender, _state);
		}

		protected virtual void OnAnimatorTransitionStarted(object _sender, AnimatorStateMachine.TransitionEventArgs _transition)
		{
			ActiveBehaviour.OnAnimatorTransitionStarted(_sender, _transition);
		}

		protected virtual void OnAnimatorStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _machine)
		{
			ActiveBehaviour.OnAnimatorStateMachineEntered(_sender, _machine);
		}

		public void CancelAction()
		{
			m_actionsQueued = 0;
		}

		public void ConsumeAction()
		{
			if (m_actionsQueued > 0)
			{
				m_actionsQueued--;
			}
		}

		public bool QueueAction(EActionQueuedType _type, int _ammount = 1)
		{
			if (m_actionsQueued > 0)
			{
				return false;
			}
			CombatHelper.ResetKillTokenDecayClock(Character);
			m_actionsQueued = _ammount;
			ActionQueuedType = _type;
			return true;
		}

		private void CheckForCancelAction()
		{
			if (ShouldCancelAction())
			{
				ConsumeAction();
				SchedulerCanAttack = false;
			}
		}

		private bool ShouldCancelAction()
		{
			if (IsPlayer || !ActionQueued)
			{
				return false;
			}
			if (ActiveBehaviour == null || (ActiveBehaviour.BehaviourType != ECombatBehaviourType.Idle && ActiveBehaviour.BehaviourType != ECombatBehaviourType.Parry))
			{
				return true;
			}
			CombatController combatController = m_combatController;
			return TargetPathDistance <= combatController.EnemyMinAttackDistance - combatController.EnemyCancelAttackThreshold || TargetPathDistance >= combatController.EnemyMaxAttackDistance + combatController.EnemyCancelAttackThreshold;
		}

		public void Behave()
		{
			GlobalSchedulerCooldownMultiplier = 1f;
			GlobalFleeChanceMultiplier = 1f;
			if (BehaviorTreeHolder != null)
			{
				BehaviorTreeHolder.UpdateTurn(this);
			}
			if (EnableParry && Character.Buffs.HasBuffs(EBuffType.Dazed))
			{
				EnableParry = false;
			}
			IsAttackedFlag = false;
		}

		public void FollowAgent()
		{
			if (m_combatController.EnableFollowAgent && LogicMain.FrameTime > 0f)
			{
				Vector3 b = m_previousPosition + Agent.velocity * LogicMain.FrameTime;
				Position = Vector3.Lerp(Position, b, LogicMain.FrameTime / (LogicMain.FrameTime + m_combatController.FollowAgentDamping));
			}
		}

		public void MoveToPosition(Vector3 _targetPosition, bool _forceMaxSpeed = false)
		{
			if (IsDead)
			{
				return;
			}
			if ((_targetPosition - Position).magnitude < m_combatController.StrafeDistanceStart)
			{
				Agent.ResetPath();
			}
			else if (NavMesh.CalculatePath(Position, _targetPosition, NavMeshLayers.WalkableLayerMaskPreset, m_path))
			{
				if (!Agent.SetPath(m_path))
				{
					Agent.Warp(Position);
					Agent.SetPath(m_path);
				}
				Agent.Resume();
				Agent.updatePosition = false;
				Agent.updateRotation = false;
				Agent.acceleration = m_combatController.AgentAcceleration;
				Agent.angularSpeed = m_combatController.AgentAngularSpeed;
				Agent.stoppingDistance = m_combatController.StrafeDistanceStop;
				Agent.autoBraking = false;
				if (_forceMaxSpeed)
				{
					Agent.speed = m_combatController.AgentMoveSpeed;
				}
				else
				{
					Agent.speed = Mathf.Lerp(m_combatController.AgentStrafeSpeed, m_combatController.AgentParrySpeed, m_combatStance);
				}
			}
			else
			{
				Agent.ResetPath();
			}
		}

		private void ParryStance()
		{
			float num = LogicMain.FrameTime / m_combatController.ParryStanceDuration;
			m_combatStance = Mathf.Clamp01(m_combatStance + ((!EnableParry) ? (0f - num) : num));
			Animator.SetCurrentCombatStance(Mathf.SmoothStep(0f, 1f, m_combatStance));
		}

		public void LookAtPosition(Vector3 _lookAtPosition)
		{
			m_animationController.SetLookAtWeight(1f, 0.3f, 0.9f, 1f, 1f);
			m_animationController.SetLookAtPosition(_lookAtPosition);
		}

		public void LookAtDisable()
		{
			m_animationController.SetLookAtWeight(0f, 0.3f, 0.9f, 1f, 1f);
		}

		public override string ToString()
		{
			if (Proxy == null || Proxy.GameObject == null)
			{
				return "Not attached";
			}
			return Proxy.GameObject.name;
		}

		public bool CanAttack()
		{
			if (!EnableAttack)
			{
				return false;
			}
			if (ActiveBehaviour.BehaviourType == ECombatBehaviourType.External)
			{
				return false;
			}
			return true;
		}

		public bool CanParry()
		{
			if (!EnableParry && m_combatStance < 0.5f)
			{
				return false;
			}
			return CanParryInternal();
		}

		public bool CanParryKillMove()
		{
			if (!EnableParryKillMove)
			{
				return false;
			}
			return CanParryInternal();
		}

		private bool CanParryInternal()
		{
			if (ActiveBehaviour.BehaviourType == ECombatBehaviourType.Attack && !AttackHasHit)
			{
				return false;
			}
			EnemyCharacter enemyCharacter = Character as EnemyCharacter;
			if (enemyCharacter != null && enemyCharacter.EnemyCharacterStateMachines.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Normal)
			{
				return false;
			}
			return true;
		}

		public bool CanDodge()
		{
			if (!EnableDodge)
			{
				return false;
			}
			EnemyCharacter enemyCharacter = Character as EnemyCharacter;
			if (enemyCharacter != null && enemyCharacter.EnemyCharacterStateMachines.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Normal)
			{
				return false;
			}
			return true;
		}

		public bool CanTaunt()
		{
			if (ActiveBehaviour.BehaviourType != ECombatBehaviourType.Idle)
			{
				return false;
			}
			EnemyCharacter enemyCharacter = Character as EnemyCharacter;
			if (enemyCharacter != null && enemyCharacter.EnemyCharacterStateMachines.CharacterCondition.CurrentStateType != EStateType.CharacterCondition_Normal)
			{
				return false;
			}
			return true;
		}

		public bool InAttackDistanceMax()
		{
			if (NextTarget == null)
			{
				return false;
			}
			if (IsPlayer)
			{
				return TargetPathDistance <= m_combatController.PlayerMaxAttackDistance;
			}
			return TargetPathDistance <= m_combatController.EnemyMaxAttackDistance;
		}

		public bool InAttackDistanceMin()
		{
			if (NextTarget == null)
			{
				return false;
			}
			return IsPlayer || TargetPathDistance >= m_combatController.EnemyMinAttackDistance;
		}

		public void BeginUpdate()
		{
			m_isInUpdate = true;
			m_cachedPosition = GameEntity.Position;
			m_cachedRotation = GameEntity.Rotation;
			Transform boneTransform = Animator.GetBoneTransform(HumanBodyBones.Head);
			m_cachedPositionEyes = boneTransform.position;
		}

		public void EndUpdate()
		{
			m_isInUpdate = false;
			if ((GameEntity.Position - m_cachedPosition).sqrMagnitude > 0.0001f * LogicMain.FrameTime * LogicMain.FrameTime)
			{
				GameEntity.Position = m_cachedPosition;
			}
			if (Quaternion.Angle(GameEntity.Rotation, m_cachedRotation) > 1f * LogicMain.FrameTime)
			{
				GameEntity.Rotation = m_cachedRotation;
			}
			m_previousPosition = m_cachedPosition;
		}

		public void TryUnsheathMelee()
		{
			Character character = Character;
			CharacterDrawWeaponBase characterDrawMeleeWeapon = character.StateMachines.Cast<ICharacterDrawMeleeWeaponCarrier>().CharacterDrawMeleeWeapon;
			if (characterDrawMeleeWeapon.ChosenWeaponState != EWeaponState.Unsheathed)
			{
				if (character.GetIsRangedUnsheathedOrInAnim(EWeaponType.Crossbow))
				{
					character.ChooseWeaponState(EWeaponCategory.Ranged, EWeaponState.Sheathed);
				}
				else
				{
					character.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
				}
			}
		}

		public bool IsWeaponUnsheathed()
		{
			CharacterDrawWeaponBase characterDrawMeleeWeapon = Character.StateMachines.Cast<ICharacterDrawMeleeWeaponCarrier>().CharacterDrawMeleeWeapon;
			return characterDrawMeleeWeapon.CurrentStateType == EStateType.CharacterDrawWeapon_Unsheathed && characterDrawMeleeWeapon.ScheduledStateType == EStateType.Invalid;
		}
	}
}
