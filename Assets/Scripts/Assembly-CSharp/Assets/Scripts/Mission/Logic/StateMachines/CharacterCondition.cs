using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.UI.LogicInterface;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CharacterCondition : StateMachineUnityComponentListeningBase<Character, CharacterCondition>, IAnimatorEventListener
	{
		public class Condition
		{
			public EStateType State { get; private set; }

			public StatTypes.EStatType CharacterStat { get; private set; }

			public IconMaterial Icon { get; private set; }

			public AnimationTaskType AnimTaskType { get; private set; }

			public EAnimAllowed AnimationAllowed { get; private set; }

			public EActionAllowed ActionAllowed { get; private set; }

			public bool IsStateChangeAllowed { get; private set; }

			public bool IsOnlyExplicit { get; private set; }

			public bool IsNoReaction { get; private set; }

			public Condition(EStateType _state, StatTypes.EStatType _characterStat, IconMaterial _icon, AnimationTaskType _animTaskType, EAnimAllowed _animAllowed, EActionAllowed _action, bool _isStateChangeAllowed = true, bool _isNoReaction = false, bool _isOnlyExplicit = false)
			{
				State = _state;
				CharacterStat = _characterStat;
				Icon = _icon;
				AnimTaskType = _animTaskType;
				AnimationAllowed = _animAllowed;
				ActionAllowed = _action;
				IsStateChangeAllowed = _isStateChangeAllowed;
				IsNoReaction = _isNoReaction;
				IsOnlyExplicit = _isOnlyExplicit;
			}
		}

		[Flags]
		public enum EAnimAllowed
		{
			None = 0,
			GetHit = 1,
			Dodge = 2,
			StunReaction = 4
		}

		[Flags]
		public enum EActionAllowed
		{
			None = 0,
			StunningStrike = 1,
			AutoEnterCombat = 2
		}

		private const EStateType StateStart = EStateType.CharacterCondition_Normal;

		private const EStateType StateEnd = EStateType.CharacterCondition_Captured;

		private static readonly Condition[] ConditionPriorities = new Condition[17]
		{
			new Condition(EStateType.CharacterCondition_Knockdowned, StatTypes.EStatType.Character_Condition_IsKnockdowned, IconMaterial.Invalid, AnimationTaskType.Knockdowned, EAnimAllowed.GetHit, EActionAllowed.AutoEnterCombat),
			new Condition(EStateType.CharacterCondition_Blackjacked, StatTypes.EStatType.Character_Condition_IsStunned, IconMaterial.ConditionStunned, AnimationTaskType.BlackjackTarget, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.AutoEnterCombat, true, false, true),
			new Condition(EStateType.CharacterCondition_StunningStrike, StatTypes.EStatType.Character_Condition_IsStunned, IconMaterial.ConditionStunned, AnimationTaskType.StunningStrikeReaction, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.AutoEnterCombat, true, false, true),
			new Condition(EStateType.CharacterCondition_Stunned, StatTypes.EStatType.Character_Condition_IsStunned, IconMaterial.ConditionStunned, AnimationTaskType.Stunned, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.AutoEnterCombat),
			new Condition(EStateType.CharacterCondition_StickyBombPanic, StatTypes.EStatType.Character_Condition_IsStickyBombPaniced, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_Sanded, StatTypes.EStatType.Character_Condition_IsSanded, IconMaterial.ConditionStunned, AnimationTaskType.ThrowSandReaction, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.AutoEnterCombat, true, false, true),
			new Condition(EStateType.CharacterCondition_SmokeBombed, StatTypes.EStatType.Character_Condition_IsSmokeBombed, IconMaterial.ConditionStunned, AnimationTaskType.SmokeBombReaction, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat),
			new Condition(EStateType.CharacterCondition_Fleeing, StatTypes.EStatType.Character_Condition_IsFleeing, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.None, true, true),
			new Condition(EStateType.CharacterCondition_Confused, StatTypes.EStatType.Character_Condition_IsConfused, IconMaterial.ConditionSidetracked, AnimationTaskType.Unknown, EAnimAllowed.GetHit | EAnimAllowed.StunReaction, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_Seduced, StatTypes.EStatType.Character_Condition_IsSeduced, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.GetHit, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_Greedy, StatTypes.EStatType.Character_Condition_IsGreedy, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.GetHit, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_JackBoxed, StatTypes.EStatType.Character_Condition_IsJackBoxed, IconMaterial.ConditionSidetracked, AnimationTaskType.Unknown, EAnimAllowed.GetHit, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_LuredBySweetMusic, StatTypes.EStatType.Character_Condition_IsLuredBySweetMusic, IconMaterial.ConditionSidetracked, AnimationTaskType.Unknown, EAnimAllowed.GetHit, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_ThreateningVictim, StatTypes.EStatType.Character_Condition_IsThreateningVictim, IconMaterial.CombatEnter, AnimationTaskType.Unknown, EAnimAllowed.GetHit, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true),
			new Condition(EStateType.CharacterCondition_OnBench, StatTypes.EStatType.Invalid, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.None, EActionAllowed.None, false, true),
			new Condition(EStateType.CharacterCondition_Captured, StatTypes.EStatType.Invalid, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.None, EActionAllowed.None, true, true),
			new Condition(EStateType.CharacterCondition_Normal, StatTypes.EStatType.Invalid, IconMaterial.Invalid, AnimationTaskType.Unknown, EAnimAllowed.GetHit | EAnimAllowed.Dodge | EAnimAllowed.StunReaction, EActionAllowed.StunningStrike | EActionAllowed.AutoEnterCombat, true, true)
		};

		public static readonly int[] AlarmedStates = new int[6] { 17, 18, 19, 16, 21, 20 };

		private Condition m_currentCondition;

		private Condition m_previousCondition;

		private bool m_firstUpdateAfterAttach;

		private bool m_wasKilledFromHideout;

		private AnimationTaskParameters m_externalParameters;

		private bool m_externalDontStartTask;

		private readonly int[] m_activeBuffs = new int[41];

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.CharacterCondition;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return BucketTurnUpdateIntervalTurns;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CharacterCondition_Normal;
			}
		}

		public bool IsStateChangeAllowed
		{
			get
			{
				return m_currentCondition.IsStateChangeAllowed;
			}
		}

		private static Condition NormalCondition
		{
			get
			{
				return ConditionPriorities[ConditionPriorities.Length - 1];
			}
		}

		private static Condition BenchCondition
		{
			get
			{
				return ConditionPriorities[ConditionPriorities.Length - 3];
			}
		}

		private static Condition NpcCapturedCondition
		{
			get
			{
				return ConditionPriorities[ConditionPriorities.Length - 2];
			}
		}

		public CharacterCondition(Character _entity)
			: base(_entity)
		{
			m_currentCondition = (m_previousCondition = NormalCondition);
			_entity.Buffs.OnBuffEvent += OnBuffEvent;
		}

		public static void InitCharacterCondition()
		{
			StateMachineUnityComponentListeningBase<Character, CharacterCondition>.InitStateMachineUnityComponentListeningBase();
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Normal, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_StunningStrike, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Stunned, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Knockdowned, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Blackjacked, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<LastKnownAreaState>(EStateType.CharacterCondition_SmokeBombed, OnEnterSheathInstant, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Sanded, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<StickyBombPanicConditionState>(EStateType.CharacterCondition_StickyBombPanic, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<FleeingConditionState>(EStateType.CharacterCondition_Fleeing, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<GreedyConditionState>(EStateType.CharacterCondition_Greedy, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<TargetConditionState>(EStateType.CharacterCondition_Seduced, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<JackBoxConditionState>(EStateType.CharacterCondition_JackBoxed, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<TargetConditionState>(EStateType.CharacterCondition_ThreateningVictim, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Confused, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_LuredBySweetMusic, OnEnterSweetMusic, OnExitSweetMusic, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_Captured, OnEnter, OnExit, OnUpdate);
			for (EStateType eStateType = EStateType.CharacterCondition_Normal; eStateType <= EStateType.CharacterCondition_Captured; eStateType++)
			{
				for (EStateType eStateType2 = EStateType.CharacterCondition_Normal; eStateType2 <= EStateType.CharacterCondition_Captured; eStateType2++)
				{
					if (eStateType != eStateType2)
					{
						StateMachineBase<Character, CharacterCondition>.AddTransition(eStateType, eStateType2);
					}
				}
			}
			StateMachineBase<Character, CharacterCondition>.AddState<State<Character>>(EStateType.CharacterCondition_OnBench, OnEnter, OnExit, OnUpdate);
			StateMachineBase<Character, CharacterCondition>.AddTransition(EStateType.CharacterCondition_Normal, EStateType.CharacterCondition_OnBench);
			StateMachineBase<Character, CharacterCondition>.AddControlling(EStateType.CharacterCondition_Seduced, EStateMachineType.PatrolSeduce, EStateType.PatrolSeduce_Dancing);
			StateMachineBase<Character, CharacterCondition>.AddControlling(EStateType.CharacterCondition_Normal, EStateMachineType.PatrolSeduce, EStateType.PatrolSeduce_Dancing);
			StateMachineBase<Character, CharacterCondition>.AddControlling(EStateType.CharacterCondition_Knockdowned, EStateMachineType.ProjectileFlight, EStateType.Projectile_Arrived);
			StateMachineBase<Character, CharacterCondition>.AddControlling(EStateType.CharacterCondition_Knockdowned, EStateMachineType.EnemyVitality, EStateType.Character_Alive);
			StateMachineBase<Character, CharacterCondition>.AddControlling(EStateType.CharacterCondition_Confused, EStateMachineType.ProjectileFlight, EStateType.Projectile_Arrived);
			StateMachineBase<Character, CharacterCondition>.AddControlling(EStateType.CharacterCondition_Confused, EStateMachineType.CharacterAwareness, EStateType.CharacterAwareness_Alarmed);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			m_currentCondition = GetCondition(EStateType.Invalid, true);
			if (m_currentCondition.State != CurrentStateType)
			{
				ChangeState(m_currentCondition.State);
			}
			if (Entity.IsAttached)
			{
				m_firstUpdateAfterAttach = true;
			}
		}

		public override void Attach()
		{
			base.Attach();
			for (int i = 0; i < m_activeBuffs.Length; i++)
			{
				if (m_activeBuffs[i] > 0)
				{
					BuffData.IBuffDefinition definition = UbiCore.Utils.Singleton<BuffData>.Instance.GetDefinition((EBuffType)i);
					for (int j = 0; j < m_activeBuffs[i]; j++)
					{
						UpdateBuffEffect(definition, false);
					}
				}
			}
		}

		public override void OnAttachedAndInitialized()
		{
			base.OnAttachedAndInitialized();
			Entity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited += OnAnimationTaskExited;
			Entity.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
			m_firstUpdateAfterAttach = true;
			OnUpdateInternal(false, EStartIdleAnimAfterTaskPolicy.DontCare);
		}

		public override void Detach()
		{
			Entity.UnityComponentCache.AnimationController.TaskManager.AnimationTaskExited -= OnAnimationTaskExited;
			Entity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
			for (int i = 0; i < m_activeBuffs.Length; i++)
			{
				if (m_activeBuffs[i] > 0)
				{
					BuffData.IBuffDefinition definition = UbiCore.Utils.Singleton<BuffData>.Instance.GetDefinition((EBuffType)i);
					for (int j = 0; j < m_activeBuffs[i]; j++)
					{
						UpdateBuffEffect(definition, true);
					}
				}
			}
			base.Detach();
		}

		public EntityId GetTargetFromCondition(EStateType _conditionType)
		{
			TargetConditionState.Payload payload = GetPayload<TargetConditionState.Payload>(_conditionType);
			return (!payload.Target.IsValid) ? EntityId.InvalidId : payload.Target;
		}

		public void ExternalChange(EStateType _externalRequest = EStateType.Invalid, AnimationTaskParameters _externalParameters = null)
		{
			m_externalParameters = null;
			m_externalDontStartTask = false;
			if (_externalRequest != CurrentStateType)
			{
				m_externalParameters = _externalParameters;
				OnUpdateInternal(false, EStartIdleAnimAfterTaskPolicy.DontCare, _externalRequest);
			}
		}

		public void ExternalChangeNoTask()
		{
			m_externalParameters = null;
			m_externalDontStartTask = true;
			OnUpdateInternal(false, EStartIdleAnimAfterTaskPolicy.DontCare);
		}

		public bool IsAnimationAllowed(EAnimAllowed _animAllowed)
		{
			return (m_currentCondition.AnimationAllowed & _animAllowed) == _animAllowed;
		}

		public bool IsActionAllowed(EActionAllowed _action)
		{
			return (m_currentCondition.ActionAllowed & _action) == _action;
		}

		private static void OnEnter(IStateMachine<Character> _stateMachine, IPayload _payload, IState<Character> _prevState, IState<Character> _currentState)
		{
			_stateMachine.Cast<CharacterCondition>().ChooseAndStartAnimationTask(EStartIdleAnimAfterTaskPolicy.DontCare);
		}

		private static void OnExit(IStateMachine<Character> _stateMachine, IPayload _payload, IState<Character> _currentState, IState<Character> _nextState)
		{
			if (!(_stateMachine.Entity is EnemyCharacter))
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < AlarmedStates.Length; i++)
			{
				flag |= AlarmedStates[i] == (int)_currentState.StateType;
				flag2 |= AlarmedStates[i] == (int)_nextState.StateType;
			}
			if (flag && !flag2)
			{
				Vector3 sourcePos = _stateMachine.Entity.Position;
				LastKnownAreaState.Payload payload = _payload as LastKnownAreaState.Payload;
				if (payload != null)
				{
					float y = LogicMain.Random.Rand(0f, 360f);
					float num = LogicMain.Random.Rand(0f, payload.Radius);
					sourcePos = payload.Position + Quaternion.Euler(0f, y, 0f) * Vector3.forward * num;
				}
				EffectsHelper.CharacterAlarmed(sourcePos, _stateMachine.Entity);
			}
		}

		private static void OnEnterSheathInstant(IStateMachine<Character> _stateMachine, IPayload _payload, IState<Character> _prevState, IState<Character> _currentState)
		{
			OnEnter(_stateMachine, _payload, _prevState, _currentState);
			ICharacterDrawMeleeWeaponCarrier characterDrawMeleeWeaponCarrier = _stateMachine.Entity.CharacterStateMachines as ICharacterDrawMeleeWeaponCarrier;
			if (characterDrawMeleeWeaponCarrier != null)
			{
				characterDrawMeleeWeaponCarrier.CharacterDrawMeleeWeapon.ForceInstantSheathedWeapon();
			}
		}

		private static void OnEnterSweetMusic(IStateMachine<Character> _statemachine, IPayload _payload, IState<Character> _prevstate, IState<Character> _currentstate)
		{
			OnEnter(_statemachine, _payload, _prevstate, _currentstate);
			_statemachine.Entity.CanBePooled = false;
		}

		private static void OnExitSweetMusic(IStateMachine<Character> _statemachine, IPayload _payload, IState<Character> _currentstate, IState<Character> _nextstate)
		{
			_statemachine.Entity.CanBePooled = true;
			OnExit(_statemachine, _payload, _currentstate, _nextstate);
		}

		private static void OnUpdate(IStateMachine<Character> _stateMachine, IPayload _payload, IState<Character> _currentState)
		{
			if ((_stateMachine.CurrentStateType != EStateType.CharacterCondition_Normal || _stateMachine.Entity.GetDeadOrAboutToDie()) && MayChangeCondition(_stateMachine))
			{
				_stateMachine.Cast<CharacterCondition>().OnUpdateInternal(false, EStartIdleAnimAfterTaskPolicy.DontCare);
			}
		}

		private void OnAnimationTaskExited(object _sender, AnimationTaskManager.AnimationTaskEventArgs _eventArgs)
		{
			if (_eventArgs.Canceled || _eventArgs.AnimationTask.Layer != 0)
			{
				return;
			}
			AnimationTaskManager taskManager = Entity.UnityComponentCache.AnimationController.TaskManager;
			ReadOnlyCollection<AnimationTaskBase> currentTasks = taskManager.GetCurrentTasks(AnimationTaskManager.ELayer.Base);
			if (currentTasks.Count > 0)
			{
				return;
			}
			switch (_eventArgs.AnimationTask.TaskType)
			{
			case AnimationTaskType.BenchExit:
			{
				AnimationTaskBenchExit animationTaskBenchExit = (AnimationTaskBenchExit)_eventArgs.AnimationTask;
				if (!animationTaskBenchExit.StartRunning || !Entity.Cast<AssassinCharacter>().GetIsNavigating(true))
				{
					OnUpdateInternal(true, EStartIdleAnimAfterTaskPolicy.DontCare);
				}
				break;
			}
			case AnimationTaskType.InteractableObject:
				break;
			case AnimationTaskType.BenchEnter:
				taskManager.CreateAnimationTaskBenchIdle();
				break;
			case AnimationTaskType.HayStackEnter:
			case AnimationTaskType.HayStackIdle:
			case AnimationTaskType.HayStackKillInstigator:
			case AnimationTaskType.LeapOfFaith:
				Debug.Log("CreateAnimationTaskHayStackIdle");
				taskManager.CreateAnimationTaskHayStackIdle();
				break;
			case AnimationTaskType.HayStackKillTarget:
			case AnimationTaskType.RooftopGardenKillTarget:
				m_wasKilledFromHideout = true;
				OnUpdateInternal(true, EStartIdleAnimAfterTaskPolicy.DontCare);
				break;
			case AnimationTaskType.RooftopGardenEnter:
			case AnimationTaskType.RooftopGardenKillInstigator:
				taskManager.CreateAnimationTaskRooftopGardenIdle();
				break;
			default:
				OnUpdateInternal(true, _eventArgs.AnimationTask.StartIdleWhenFinishedPolicy);
				break;
			}
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			if (_event.Type == AnimatorEventType.RestartCondition)
			{
				OnUpdateInternal(true, EStartIdleAnimAfterTaskPolicy.No);
			}
			return false;
		}

		private void OnUpdateInternal(bool _restartTask, EStartIdleAnimAfterTaskPolicy _startIdle, EStateType _externalRequest = EStateType.Invalid)
		{
			Entity.EntityStats.UpdateComputedStatsExplicit();
			Condition condition = GetCondition(_externalRequest, _restartTask || m_firstUpdateAfterAttach);
			if (m_currentCondition.IsStateChangeAllowed && condition.State != CurrentStateType)
			{
				GetCondition(_externalRequest, _restartTask || m_firstUpdateAfterAttach);
				m_previousCondition = m_currentCondition;
				m_currentCondition = condition;
				ChangeState(condition.State);
			}
			else if (_restartTask || m_firstUpdateAfterAttach)
			{
				m_externalParameters = null;
				ChooseAndStartAnimationTask(_startIdle);
			}
			m_firstUpdateAfterAttach = false;
			m_externalParameters = null;
			m_externalDontStartTask = false;
		}

		private void ChooseAndStartAnimationTask(EStartIdleAnimAfterTaskPolicy _startIdle)
		{
			IconMaterial icon = m_currentCondition.Icon;
			if (m_currentCondition.State == EStateType.CharacterCondition_ThreateningVictim && ((NpcCharacter)GetTargetFromCondition(m_currentCondition.State).GameEntity).Patrol.VictimComponent is Captive)
			{
				icon = IconMaterial.Invalid;
			}
			Entity.HeadIcon.IconAttachment.SetIcon(icon, 2);
			if (Entity.HeadIcon.IconAttachment.VisibilityPredicate == null)
			{
				Entity.HeadIcon.IconAttachment.VisibilityPredicate = IconVisibilityPredicate;
			}
			if (!Entity.IsAttached || m_externalDontStartTask)
			{
				return;
			}
			AnimationController animationController = Entity.UnityComponentCache.AnimationController;
			AnimationTaskManager taskManager = animationController.TaskManager;
			animationController.Animator.SetCurrentNoConditionActive(m_currentCondition.IsNoReaction);
			switch (CurrentStateType)
			{
			case EStateType.CharacterCondition_Knockdowned:
				if (m_externalParameters is AnimationTaskKnockdowned.Parameters)
				{
					AnimationTaskKnockdowned.Parameters parameters = m_externalParameters.Cast<AnimationTaskKnockdowned.Parameters>();
					taskManager.CreateAnimationTaskKnockdowned(parameters.Source, parameters.Target, parameters.ForceToGround);
				}
				else
				{
					int currentState = animationController.StateMachine.GetCurrentState();
					bool forceToGround = !AnimationStateHashes.IsStateContainedInArray(currentState, AnimationStateHashes.States_PushBack);
					taskManager.CreateAnimationTaskKnockdowned(null, Entity, forceToGround);
				}
				break;
			case EStateType.CharacterCondition_StunningStrike:
				taskManager.CreateAnimationTaskStunningStrikeReaction(Entity);
				break;
			case EStateType.CharacterCondition_Stunned:
			{
				int currentStateMachineHash = animationController.StateMachine.GetCurrentStateMachineHash();
				bool fromGround = currentStateMachineHash == AnimationStateMachineHashes.StateMachine_PushBack.NameHash;
				taskManager.CreateAnimationTaskStunned(Entity, fromGround);
				break;
			}
			case EStateType.CharacterCondition_Blackjacked:
				break;
			case EStateType.CharacterCondition_Sanded:
				taskManager.CreateAnimationTaskThrowSandReaction();
				break;
			case EStateType.CharacterCondition_SmokeBombed:
				taskManager.CreateAnimationTaskSmokeBombReaction();
				break;
			case EStateType.CharacterCondition_StickyBombPanic:
			case EStateType.CharacterCondition_Fleeing:
			case EStateType.CharacterCondition_Greedy:
			case EStateType.CharacterCondition_Seduced:
			case EStateType.CharacterCondition_JackBoxed:
			case EStateType.CharacterCondition_ThreateningVictim:
			case EStateType.CharacterCondition_Confused:
			case EStateType.CharacterCondition_LuredBySweetMusic:
			{
				AnimationTaskType activeTask = taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
				Condition condition = Array.Find(ConditionPriorities, (Condition _c) => _c.AnimTaskType == activeTask);
				if (condition != null)
				{
					goto case EStateType.CharacterCondition_Normal;
				}
				break;
			}
			case EStateType.CharacterCondition_Normal:
			{
				AnimationTaskType currentTaskType = taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
				if (Entity.GetDeadOrAboutToDie())
				{
					if (currentTaskType != AnimationTaskType.Dead && currentTaskType != AnimationTaskType.HayStackKillTarget && currentTaskType != AnimationTaskType.RooftopGardenKillTarget && !m_wasKilledFromHideout)
					{
						taskManager.CreateAnimationTaskDead();
					}
					else if (m_wasKilledFromHideout && Entity.GetIsDead() && Entity.StateMachines is ICharacterDrawMeleeWeaponCarrier)
					{
						CharacterDrawWeaponMelee characterDrawWeaponMelee = (CharacterDrawWeaponMelee)Entity.StateMachines.GetStateMachine(EStateMachineType.CharacterDrawWeaponMelee);
						characterDrawWeaponMelee.ChosenWeaponState = EWeaponState.Detached;
						WeaponHandler weaponHandler = characterDrawWeaponMelee.GetWeaponHandler();
						if (weaponHandler != null)
						{
							weaponHandler.ShowWeapon(false);
						}
					}
				}
				else
				{
					if (currentTaskType != AnimationTaskType.Unknown && currentTaskType != m_previousCondition.AnimTaskType && m_previousCondition.AnimTaskType != AnimationTaskType.Unknown)
					{
						break;
					}
					IParticipantCarrier participantCarrier = Entity as IParticipantCarrier;
					if (participantCarrier != null && participantCarrier.CombatParticipantProxy.IsInCombat)
					{
						taskManager.CreateAnimationTaskCombat(participantCarrier);
					}
					else if (_startIdle != EStartIdleAnimAfterTaskPolicy.No)
					{
						AssassinCharacter assassinCharacter = Entity as AssassinCharacter;
						if (_startIdle == EStartIdleAnimAfterTaskPolicy.Yes || assassinCharacter == null || !assassinCharacter.GetIsNavigating(true))
						{
							taskManager.CreateAnimationTaskIdle();
						}
					}
				}
				break;
			}
			case EStateType.CharacterCondition_OnBench:
				taskManager.CreateAnimationTaskNpcBenchIdle(UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.GetBenchSeatByEntityId(Entity.Id));
				break;
			case EStateType.CharacterCondition_Captured:
				break;
			}
		}

		private bool IconVisibilityPredicate()
		{
			Vector3 position = LogicMain.PlayerEntity.Position;
			Vector3 position2 = Entity.Position;
			return (position - position2).LengthXZ() <= 20f;
		}

		private static void SetEffectEnabled(IEffectsCarrier _entity, string _effect, bool _enabled)
		{
			if (_enabled)
			{
				_entity.Effects.OnEffectOn(_effect);
			}
			else
			{
				_entity.Effects.OnEffectOff(_effect);
			}
		}

		private static bool MayChangeCondition(IStateMachine<Character> _stateMachine)
		{
			if (!_stateMachine.Entity.IsAttached)
			{
				return true;
			}
			if (_stateMachine.Entity is IParticipantCarrier)
			{
				CombatParticipantProxy combatParticipantProxy = _stateMachine.Entity.Cast<IParticipantCarrier>().CombatParticipantProxy;
				if (combatParticipantProxy.IsInCombat)
				{
					ECombatBehaviourType behaviourType = combatParticipantProxy.CombatParticipant.ActiveBehaviour.BehaviourType;
					if (behaviourType == ECombatBehaviourType.GetHit || behaviourType == ECombatBehaviourType.Die)
					{
						return false;
					}
				}
			}
			AnimationTaskManager taskManager = _stateMachine.Entity.UnityComponentCache.AnimationController.TaskManager;
			AnimationTaskType currentTaskType = taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			AnimationTaskType animationTaskType = currentTaskType;
			if (animationTaskType == AnimationTaskType.GetHit)
			{
				return false;
			}
			return true;
		}

		public Condition GetCondition(EStateType _externalRequest, bool _restartTask, bool _ignoreDeath = false)
		{
			if (_externalRequest == EStateType.Invalid && Entity is IParticipantCarrier && !_ignoreDeath)
			{
				if (Entity.GetHasZeroHealth())
				{
					return NormalCondition;
				}
				if (Entity.CharacterStats.Get_bool(StatTypes.EStatType.Character_Conflict_AboutToDie))
				{
					return (!_restartTask) ? m_currentCondition : NormalCondition;
				}
			}
			if (Entity.GetIsCaptured())
			{
				return NpcCapturedCondition;
			}
			if (UbiCore.Utils.Singleton<HotspotService>.Instance != null)
			{
				Bench.Seat benchSeatByEntityId = UbiCore.Utils.Singleton<HotspotService>.Instance.BenchManager.GetBenchSeatByEntityId(Entity.Id);
				if (benchSeatByEntityId != null)
				{
					return BenchCondition;
				}
			}
			CharacterStats characterStats = Entity.CharacterStats;
			for (int i = 0; i < ConditionPriorities.Length - 1; i++)
			{
				Condition condition = ConditionPriorities[i];
				if (condition.State == _externalRequest)
				{
					return condition;
				}
				if ((!condition.IsOnlyExplicit || (condition == m_currentCondition && !_restartTask)) && condition.CharacterStat != StatTypes.EStatType.Invalid && characterStats.Get_int(condition.CharacterStat) > 0)
				{
					return condition;
				}
			}
			return NormalCondition;
		}

		private void OnBuffEvent(BuffEvent _event)
		{
			BuffData.IBuffDefinition definition = _event.Buff.Definition;
			if (definition != null)
			{
				int num = m_activeBuffs[(int)_event.Buff.Type] + ((!_event.Removed) ? 1 : (-1));
				m_activeBuffs[(int)_event.Buff.Type] = num;
				UpdateBuffEffect(definition, _event.Removed);
			}
		}

		private void UpdateBuffEffect(BuffData.IBuffDefinition _buffDef, bool _removed)
		{
			for (int i = 0; i < _buffDef.StatusEffectNames.Length; i++)
			{
				SetEffectEnabled(Entity, _buffDef.StatusEffectNames[i], !_removed);
			}
			if (_removed)
			{
				for (int j = 0; j < _buffDef.EndEffectNames.Length; j++)
				{
					Entity.Effects.OnEffectTrigger(_buffDef.EndEffectNames[j]);
				}
			}
			if (!_removed)
			{
				for (int k = 0; k < _buffDef.StartEffectNames.Length; k++)
				{
					Entity.Effects.OnEffectTrigger(_buffDef.StartEffectNames[k]);
				}
			}
		}
	}
}
