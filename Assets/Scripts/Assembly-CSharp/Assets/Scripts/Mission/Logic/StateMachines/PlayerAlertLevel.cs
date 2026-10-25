using AcierProtocol;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using UbiCore;
using UbiCore.Debug;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerAlertLevel : StateMachineBase<PlayerCharacter, PlayerAlertLevel>
	{
		private static class NotorietyHelper
		{
			public static void AddNotorietyContinuous(GameEntity _target, ENotorietyGameContinuous _notorietyGameContinuous)
			{
				float deltaValue = LogicMain.GameDefinition.NotoritiesContinuous[(int)_notorietyGameContinuous] * 0.1f;
				AddNotoriety(_target, deltaValue);
			}

			public static void DecreaseNotorietyBelow(GameEntity _target, float _maxValue)
			{
				IStatValue statValue = _target.EntityStats.GetStatValue(StatTypes.EStatType.Character_Conspicuous_Notoriety);
				statValue.Value_float = Mathf.Min(_maxValue, statValue.Value_float);
			}

			public static void IncreaseNotorietyAbove(GameEntity _target, float _minValue)
			{
				IStatValue statValue = _target.EntityStats.GetStatValue(StatTypes.EStatType.Character_Conspicuous_Notoriety);
				statValue.Value_float = Mathf.Max(_minValue, statValue.Value_float);
			}

			public static void AddNotoriety(GameEntity _target, float _deltaValue)
			{
				_target.EntityStats.AddFloat(StatTypes.EStatType.Character_Conspicuous_Notoriety, _deltaValue);
			}
		}

		private class EntityStatusCache
		{
			public bool m_isStalking;

			public bool m_isHiding;

			public bool m_isInCombat;

			public bool m_isRunning;

			public EStateType m_mainState;

			public EntityId m_assasinationTarget;

			private readonly AssassinCharacter m_assassinCharacter;

			public EntityStatusCache(AssassinCharacter _assassinCharacter)
			{
				m_assassinCharacter = _assassinCharacter;
			}

			public bool Refresh()
			{
				m_isStalking = m_assassinCharacter.GetIsStalking();
				m_isHiding = m_assassinCharacter.GetIsHiding();
				m_isInCombat = m_assassinCharacter.GetIsInCombat();
				m_isRunning = m_assassinCharacter.GetIsRunning();
				PlayerMain playerMain = m_assassinCharacter.PlayerMainCarrier.PlayerMain;
				m_mainState = playerMain.CurrentStateType;
				m_assasinationTarget = EntityId.InvalidId;
				if (m_assassinCharacter.GetIsPerformingAssassination())
				{
					PlayerMain.TargetState.Payload payload = playerMain.CurrentStatePayload as PlayerMain.TargetState.Payload;
					if (payload != null)
					{
						m_assasinationTarget = payload.Target.Id;
					}
				}
				else
				{
					KillFromHideout activeKillFromHideout = m_assassinCharacter.GetActiveKillFromHideout();
					m_assasinationTarget = ((activeKillFromHideout == null) ? EntityId.InvalidId : activeKillFromHideout.KillTarget);
				}
				return true;
			}
		}

		private const float MaxInteractionDuration = 1f;

		private const float PlayerMainStateChangeMinDecreaseDelay = 0.5f;

		private const float NotorietyForceAlarmedThreshold = 0.9f;

		public static bool s_cheatNoNotoriety;

		public static bool s_cheatNoDecreaseNotorietyIfUnseen;

		private readonly SimpleGameTimer m_conspiciousIntervalTimer = new SimpleGameTimer();

		private readonly SimpleGameTimer m_decreaseIntervalTimer = new SimpleGameTimer();

		private readonly SimpleGameTimer m_interactionResetTimer = new SimpleGameTimer();

		private readonly SimpleGameTimer m_decreaseDelay = new SimpleGameTimer();

		private EStateType m_lastDecreaseState;

		private float m_increaseToNotoriety;

		private float m_decreaseToNotoriety = float.MaxValue;

		private readonly EntityStatusCache m_entityStatusCache;

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerAlertLevel;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CharacterAlertLevel_Anonymous;
			}
		}

		public EInteractionReason RegisteredInteraction { get; private set; }

		public bool ExposingInteractionRegistered { get; private set; }

		public bool ConspiciousCooldownRunning
		{
			get
			{
				return m_conspiciousIntervalTimer.IsRunning;
			}
		}

		public PlayerAlertLevel(PlayerCharacter _entity)
			: base(_entity)
		{
			m_entityStatusCache = new EntityStatusCache(Entity);
		}

		public static void InitPlayerAlertLevel()
		{
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.InitStateMachineBase();
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddState<State<PlayerCharacter>>(EStateType.CharacterAlertLevel_Anonymous, OnEnterAnonymous, null, OnUpdateGeneric);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddState<State<PlayerCharacter>>(EStateType.CharacterAlertLevel_Alerted, OnEnterAlerted, null, OnUpdateGeneric);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddState<State<PlayerCharacter>>(EStateType.CharacterAlertLevel_Exposed, OnEnterExposed, OnExitExposed, OnUpdateGeneric);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddTransition(EStateType.CharacterAlertLevel_Anonymous, EStateType.CharacterAlertLevel_Alerted);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddTransition(EStateType.CharacterAlertLevel_Anonymous, EStateType.CharacterAlertLevel_Exposed);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddTransition(EStateType.CharacterAlertLevel_Alerted, EStateType.CharacterAlertLevel_Anonymous);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddTransition(EStateType.CharacterAlertLevel_Alerted, EStateType.CharacterAlertLevel_Exposed);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddTransition(EStateType.CharacterAlertLevel_Exposed, EStateType.CharacterAlertLevel_Anonymous);
			StateMachineBase<PlayerCharacter, PlayerAlertLevel>.AddTransition(EStateType.CharacterAlertLevel_Exposed, EStateType.CharacterAlertLevel_Alerted);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			RegisteredInteraction = EInteractionReason.Invalid;
			ExposingInteractionRegistered = false;
			Entity.PlayerMainCarrier.PlayerMain.StateChanged += OnMainStateChanged;
			Messenger.Instance.AddListener<StatValueChangedEvent>(OnStatValueChangedEvent);
			Reset();
		}

		public override void OnUnregistered()
		{
			base.OnUnregistered();
			Entity.PlayerMainCarrier.PlayerMain.StateChanged -= OnMainStateChanged;
			Messenger.Instance.RemoveListener<StatValueChangedEvent>(OnStatValueChangedEvent);
		}

		private void OnMainStateChanged(IStateMachine _statemachine, EStateType _from, EStateType _to)
		{
			PlayerMain.TargetState.Payload payload = _statemachine.TryGetPayload<PlayerMain.TargetState.Payload>(_from);
			if (payload != null && payload.InteractionReason != EInteractionReason.Invalid)
			{
				RegisterInteraction(payload.InteractionReason);
			}
		}

		private static void OnUpdateGeneric(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _currentstate)
		{
			Profiling.Start("PlayerAlertLevel.Update", float.MaxValue);
			PlayerAlertLevel playerAlertLevel = (PlayerAlertLevel)_statemachine;
			EntityStatusCache entityStatusCache = playerAlertLevel.m_entityStatusCache;
			entityStatusCache.Refresh();
			playerAlertLevel.CheckInteractionTimer();
			playerAlertLevel.CheckForIncreaseAndDecreaseToNotoriety();
			playerAlertLevel.CheckForSuspiciousCharacters(entityStatusCache);
			playerAlertLevel.DecreaseNotorietyIfNotConspicious(entityStatusCache);
			ChangeStateByNotoriety(_statemachine);
			Profiling.Stop();
		}

		private static void OnEnterAnonymous(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _prevstate, IState<PlayerCharacter> _currentstate)
		{
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.AnonymousLevel);
		}

		private static void OnEnterAlerted(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _prevstate, IState<PlayerCharacter> _currentstate)
		{
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.AlertedLevel);
		}

		private static void OnEnterExposed(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _prevstate, IState<PlayerCharacter> _currentstate)
		{
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.ExposedLevel);
		}

		private static void OnExitExposed(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _currentstate, IState<PlayerCharacter> _nextstate)
		{
		}

		public void RegisterInteraction(EInteractionReason _interactionReason)
		{
			if (_interactionReason != EInteractionReason.Invalid)
			{
				RegisteredInteraction = _interactionReason;
				RegisterInteractionInternal();
			}
		}

		public void RegisterExposingInteraction()
		{
			ExposingInteractionRegistered = true;
			RegisterInteractionInternal();
		}

		public void RegisterIncreaseToNotoriety(float _minNotoriety)
		{
			m_increaseToNotoriety = Mathf.Max(m_increaseToNotoriety, _minNotoriety);
			m_lastDecreaseState = EStateType.Invalid;
		}

		public void RegisterDecreaseToNotoriety(float _maxNotoriety)
		{
			m_decreaseToNotoriety = Mathf.Min(m_decreaseToNotoriety, _maxNotoriety);
		}

		public void ForceNotoriety(float _notoriety)
		{
			Reset();
			float num = Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
			if (num < _notoriety)
			{
				RegisterIncreaseToNotoriety(_notoriety);
			}
			else
			{
				RegisterDecreaseToNotoriety(_notoriety);
			}
			CheckForIncreaseAndDecreaseToNotoriety();
			ChangeStateByNotoriety(this, true);
		}

		private void CheckForIncreaseAndDecreaseToNotoriety()
		{
			NotorietyHelper.IncreaseNotorietyAbove(Entity, m_increaseToNotoriety);
			m_increaseToNotoriety = 0f;
			NotorietyHelper.DecreaseNotorietyBelow(Entity, m_decreaseToNotoriety);
			m_decreaseToNotoriety = float.MaxValue;
		}

		private void Reset()
		{
			m_decreaseToNotoriety = float.MaxValue;
			m_increaseToNotoriety = 0f;
			m_conspiciousIntervalTimer.Reset();
			m_decreaseDelay.Reset();
			m_interactionResetTimer.Reset();
			m_lastDecreaseState = EStateType.Invalid;
		}

		private void CheckForSuspiciousCharacters(EntityStatusCache _entityStatusCache)
		{
			float num = -1f;
			if (m_conspiciousIntervalTimer.IsRunning)
			{
				return;
			}
			float radius = LogicMain.GameStats.Get_float(StatTypes.EStatType.Game_MaxDetectionRange);
			foreach (NpcCharacter item in LogicMain.EntityManager.CollectEntitiesInRadius<NpcCharacter>(Entity.Position, radius))
			{
				if (item is AwareInteractionCharacter || item is EnemyCharacter)
				{
					num = CheckIfSuspicious(item, num, _entityStatusCache);
				}
			}
			if (num >= MathHelper.Epsilon)
			{
				NotorietyHelper.AddNotoriety(Entity, num);
				m_conspiciousIntervalTimer.Start(Entity.PlayerStats.Get_float(StatTypes.EStatType.Player_NotorietyInterval));
			}
		}

		private float CheckIfSuspicious(NpcCharacter _npcCharacter, float _notorietyDelta, EntityStatusCache _entityStatusCache)
		{
			ICharacterAwarenessCarrier characterAwarenessCarrier = _npcCharacter.StateMachines as ICharacterAwarenessCarrier;
			if (!_npcCharacter.GetDeadOrAboutToDie() && _entityStatusCache.m_assasinationTarget != _npcCharacter.Id && !IsSilenced(_npcCharacter))
			{
				EDetectionRange eDetectionRange = (EDetectionRange)_npcCharacter.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range);
				if (eDetectionRange != EDetectionRange.FarAway)
				{
					float _notorietyDelta2 = -1f;
					if (!_npcCharacter.GetIsUnconscious())
					{
						if (_entityStatusCache.m_isHiding && !_entityStatusCache.m_isStalking)
						{
							CheckForSuspiciousInteraction(_npcCharacter, ref _notorietyDelta2);
						}
						else if (_npcCharacter.GetIsInCombat())
						{
							CheckForSuspiciousInteraction(_npcCharacter, ref _notorietyDelta2);
							CheckForSuspiciousActions(_npcCharacter, EStateType.CharacterAwareness_Undefined, _entityStatusCache, ref _notorietyDelta2);
						}
						else
						{
							EStateType currentStateType = characterAwarenessCarrier.CharacterAwareness.CurrentStateType;
							switch (currentStateType)
							{
							case EStateType.CharacterAwareness_Low:
							case EStateType.CharacterAwareness_High:
							case EStateType.CharacterAwareness_Alarmed:
								CheckForSuspiciousInteraction(_npcCharacter, ref _notorietyDelta2);
								CheckForSuspiciousActions(_npcCharacter, currentStateType, _entityStatusCache, ref _notorietyDelta2);
								break;
							default:
								if (!(_npcCharacter is ICharacterConditionCarrierEntity) || _npcCharacter.GetIsConfused())
								{
									CheckForSuspiciousInteraction(_npcCharacter, ref _notorietyDelta2);
								}
								break;
							}
						}
					}
					if (_notorietyDelta2 > 0.9f)
					{
						characterAwarenessCarrier.CharacterAwareness.ScheduleStateChangeForced(EStateType.CharacterAwareness_Alarmed);
					}
					_notorietyDelta = Mathf.Max(_notorietyDelta, _notorietyDelta2);
				}
			}
			return _notorietyDelta;
		}

		private static bool IsSilenced(NpcCharacter _npcCharacter)
		{
			EnemyCharacter enemyCharacter = _npcCharacter as EnemyCharacter;
			return enemyCharacter != null && enemyCharacter.GetIsSilenced();
		}

		private void CheckInteractionTimer()
		{
			if (m_interactionResetTimer.IsFinished || m_conspiciousIntervalTimer.IsFinished)
			{
				m_interactionResetTimer.Reset();
				RegisteredInteraction = EInteractionReason.Invalid;
				ExposingInteractionRegistered = false;
			}
		}

		private void DecreaseNotorietyIfNotConspicious(EntityStatusCache _entityStatusCache)
		{
			if (m_conspiciousIntervalTimer.IsRunning || m_decreaseDelay.IsRunning || m_decreaseIntervalTimer.IsRunning)
			{
				return;
			}
			m_decreaseIntervalTimer.Start(Entity.PlayerStats.Get_float(StatTypes.EStatType.Player_NotorietyDecreaseInterval));
			_entityStatusCache.Refresh();
			EStateType eStateType = ((!_entityStatusCache.m_isStalking) ? _entityStatusCache.m_mainState : EStateType.PlayerStalkingZone_Stalking);
			if (eStateType == m_lastDecreaseState)
			{
				switch (eStateType)
				{
				case EStateType.CombatCharacterMain_Roaming:
				case EStateType.CombatCharacterMain_Parcour:
				case EStateType.PlayerMain_NpcInteraction:
				case EStateType.PlayerMain_ObjectInteraction:
				case EStateType.PlayerMain_BeingChased:
					if (!Entity.CheckBeingChased())
					{
						NotorietyHelper.AddNotorietyContinuous(Entity, ENotorietyGameContinuous.Unseen);
					}
					break;
				case EStateType.PlayerMain_HidingInObject:
					if (IdlingInHideout())
					{
						NotorietyHelper.AddNotorietyContinuous(Entity, ENotorietyGameContinuous.HidingInObject);
					}
					break;
				case EStateType.PlayerMain_HidingInPatrol:
					NotorietyHelper.AddNotorietyContinuous(Entity, ENotorietyGameContinuous.HidingInPatrol);
					break;
				case EStateType.PlayerStalkingZone_Stalking:
					NotorietyHelper.AddNotorietyContinuous(Entity, ENotorietyGameContinuous.HidingInStalkingZone);
					break;
				}
			}
			else
			{
				m_lastDecreaseState = eStateType;
				if (m_decreaseDelay.TimeToFinish < 0.5f)
				{
					m_decreaseDelay.Start(0.5f);
				}
			}
		}

		private void CheckForSuspiciousActions(NpcCharacter _npcCharacter, EStateType _awarenessStateType, EntityStatusCache _entityStatusCache, ref float _notorietyDelta)
		{
			if (!(_notorietyDelta < MathHelper.Epsilon))
			{
				return;
			}
			if (_entityStatusCache.m_isInCombat)
			{
				_notorietyDelta = GetNotorietyContinuous(_npcCharacter, ENotorietyNpcContinuous.Combat);
				return;
			}
			if (Entity.EntityStats.Get_bool(StatTypes.EStatType.Player_IsInRestrictedArea))
			{
				_notorietyDelta = GetNotorietyContinuous(_npcCharacter, ENotorietyNpcContinuous.RestrictedArea);
			}
			if (_notorietyDelta < MathHelper.Epsilon && _entityStatusCache.m_isRunning)
			{
				_notorietyDelta = ((_awarenessStateType <= EStateType.CharacterAwareness_Low) ? GetNotorietyContinuous(_npcCharacter, ENotorietyNpcContinuous.RunInAwarenessLow) : GetNotorietyContinuous(_npcCharacter, ENotorietyNpcContinuous.RunInAwarenessHigh));
			}
			if (_notorietyDelta < MathHelper.Epsilon)
			{
				_notorietyDelta = ((_awarenessStateType <= EStateType.CharacterAwareness_Low) ? GetNotorietyContinuous(_npcCharacter, ENotorietyNpcContinuous.WalkInAwarenessLow) : GetNotorietyContinuous(_npcCharacter, ENotorietyNpcContinuous.WalkInAwarenessHigh));
			}
		}

		private void CheckForSuspiciousInteraction(NpcCharacter _npc, ref float _notorietyDelta)
		{
			if (ExposingInteractionRegistered)
			{
				_notorietyDelta = 1f;
			}
			else if (RegisteredInteraction != EInteractionReason.Invalid)
			{
				_notorietyDelta = GetNotorietyInteractions(_npc, RegisteredInteraction);
			}
		}

		private float GetNotorietyContinuous(NpcCharacter _npc, ENotorietyNpcContinuous _notorietyType)
		{
			return _npc.NpcDefinition.Definition.NotoritiesContinuous[(int)_notorietyType];
		}

		private float GetNotorietyInteractions(NpcCharacter _npc, EInteractionReason _interactionReason)
		{
			return _npc.NpcDefinition.Definition.NotoritiesInteractions[(int)_interactionReason];
		}

		private static bool ChangeStateByNotoriety(IStateMachine<PlayerCharacter> _statemachine, bool _immediateForce = false)
		{
			EStateType eStateType = EStateType.CharacterAlertLevel_Anonymous;
			float num = _statemachine.Entity.EntityStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
			if (num > 0.99f)
			{
				eStateType = EStateType.CharacterAlertLevel_Exposed;
			}
			else if (num >= 0.5f)
			{
				eStateType = EStateType.CharacterAlertLevel_Alerted;
			}
			if (eStateType == _statemachine.CurrentStateType)
			{
				return false;
			}
			if (_immediateForce)
			{
				_statemachine.ChangeStateForced(eStateType);
			}
			else
			{
				_statemachine.ScheduleStateChange(eStateType);
			}
			return true;
		}

		private bool IdlingInHideout()
		{
			PlayerMain.HidingInObjectState.HidingInObjectPayload activeHidingInObjectPayload = Entity.GetActiveHidingInObjectPayload();
			KillFromHideout activeKillFromHideout = Entity.GetActiveKillFromHideout();
			return (activeKillFromHideout == null || activeKillFromHideout.IsIdling) && activeHidingInObjectPayload != null && !activeHidingInObjectPayload.ShouldExit;
		}

		private void OnStatValueChangedEvent(StatValueChangedEvent _message)
		{
			if (_message.Owner == Entity && _message.StatValue.StatType == StatTypes.EStatType.Character_Conspicuous_Notoriety && _message.StatValue.Value_float > _message.PreviousValueWrapper.Float)
			{
				float duration = Entity.PlayerStats.Get_float(StatTypes.EStatType.Player_NotorietyDecreaseDelay);
				m_decreaseDelay.Start(duration);
			}
		}

		private void RegisterInteractionInternal()
		{
			m_interactionResetTimer.Start(1f);
			m_conspiciousIntervalTimer.Reset();
			m_lastDecreaseState = EStateType.Invalid;
		}
	}
}
