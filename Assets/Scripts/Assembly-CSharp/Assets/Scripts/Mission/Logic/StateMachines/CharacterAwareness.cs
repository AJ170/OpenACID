using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class CharacterAwareness : StateMachineBase<NpcCharacter, CharacterAwareness>
	{
		public class TimedAwarenessState : State<NpcCharacter>
		{
			public class TimedPayload : IPayload
			{
				public SimpleGameTimer ActiveTimer { get; set; }

				public TimedPayload()
				{
					ActiveTimer = new SimpleGameTimer();
				}

				public void Reset()
				{
					ActiveTimer.Reset();
				}

				public void RestartActiveTimer(IStateMachine<NpcCharacter> _stateMachine)
				{
					ActiveTimer.Start(GetStat_float(_stateMachine, StatTypes.EStatType.CharacterDetection_StayAlarmedMinTime));
				}
			}

			public TimedAwarenessState(EStateType _stateType)
				: this(_stateType, null, null, null)
			{
			}

			public TimedAwarenessState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<NpcCharacter> _stateMachine)
			{
				return new TimedPayload();
			}
		}

		public class AlarmedState : TimedAwarenessState
		{
			public class AlarmedPayload : TimedPayload
			{
				private const float SteeringDirectionMagnitudeThreshold = 0.2f;

				private const float SteeringDirectionMagnitudeThresholdSqr = 0.040000003f;

				public Vector3 LastKnownPosition { get; private set; }

				public float LastKnownRotation { get; private set; }

				public bool LastKnownIsSet { get; private set; }

				public AlarmedPayload()
				{
					LastKnownPosition = new Vector3(float.MinValue, float.MinValue, float.MinValue);
					LastKnownRotation = float.MinValue;
					LastKnownIsSet = false;
				}

				public void UpdateLastKnownPosition(NpcCharacter _entity, IPositionProvider _target)
				{
					IPositionProvider positionProvider = _target ?? _entity.GetTargetEntity();
					Vector3 vector = positionProvider.Position;
					PlayerCharacter playerCharacter = positionProvider as PlayerCharacter;
					if (playerCharacter != null && CharacterHelper.GetIsInParcour(playerCharacter))
					{
						vector = playerCharacter.GetMainStateMachine().GetPayload<PlayerMain.ParcourState.ParcourPayload>(EStateType.CombatCharacterMain_Parcour).ParcourDestination;
					}
					UpdateLastKnownPosition(_entity, vector);
					if (playerCharacter != null)
					{
						Vector3 steeringTarget = playerCharacter.GetSteeringTarget();
						Vector3 direction = steeringTarget - vector;
						if (direction.sqrMagnitude > 0.040000003f)
						{
							LastKnownRotation = direction.AngleZX();
						}
					}
					LastKnownIsSet = true;
				}

				public void UpdateLastKnownPosition(NpcCharacter _entity, Vector3 _targetPos)
				{
					LastKnownPosition = _targetPos;
					Vector3 position = _entity.Position;
					Vector3 direction = _targetPos - position;
					LastKnownRotation = direction.AngleZX();
					LastKnownIsSet = true;
				}

				public void CopyLastKnownValues(AlarmedPayload _otherPayload)
				{
					LastKnownPosition = _otherPayload.LastKnownPosition;
					LastKnownRotation = _otherPayload.LastKnownRotation;
					LastKnownIsSet = true;
				}
			}

			public AlarmedState(EStateType _stateType)
				: base(_stateType)
			{
			}

			public AlarmedState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<NpcCharacter> _stateMachine)
			{
				return new AlarmedPayload();
			}
		}

		private const float AwareIconExtensionTime = 0.5f;

		private readonly SimpleGameTimer m_awareInconExtensionTimer = new SimpleGameTimer();

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.CharacterAwareness;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CharacterAwareness_Unaware;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return BucketTurnUpdateIntervalTurns;
			}
		}

		public CharacterAwareness(NpcCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitCharacterAwareness()
		{
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddState<State<NpcCharacter>>(EStateType.CharacterAwareness_Undefined, OnEnterUndefined, OnExitUndefined, OnUpdateUndefined);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddState<State<NpcCharacter>>(EStateType.CharacterAwareness_Unaware, OnEnterUnaware, null, OnUpdateUnaware);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddState<State<NpcCharacter>>(EStateType.CharacterAwareness_Low, null, OnExitUpdateLastKnownPosition, OnUpdateLow);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddState<State<NpcCharacter>>(EStateType.CharacterAwareness_High, null, OnExitUpdateLastKnownPosition, OnUpdateHigh);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddState<AlarmedState>(EStateType.CharacterAwareness_Alarmed, OnEnterAlarmed, null, OnUpdateAlarmed);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Undefined, EStateType.CharacterAwareness_Unaware);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Undefined, EStateType.CharacterAwareness_Low);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Undefined, EStateType.CharacterAwareness_High);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Undefined, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Unaware, EStateType.CharacterAwareness_Low);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Unaware, EStateType.CharacterAwareness_High);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Unaware, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Unaware, EStateType.CharacterAwareness_Undefined);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Low, EStateType.CharacterAwareness_Unaware);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Low, EStateType.CharacterAwareness_High);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Low, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Low, EStateType.CharacterAwareness_Undefined);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_High, EStateType.CharacterAwareness_Unaware);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_High, EStateType.CharacterAwareness_Low);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_High, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_High, EStateType.CharacterAwareness_Undefined);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Alarmed, EStateType.CharacterAwareness_Unaware);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Alarmed, EStateType.CharacterAwareness_Low);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Alarmed, EStateType.CharacterAwareness_High);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Alarmed, EStateType.CharacterAwareness_Undefined);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddTransition(EStateType.CharacterAwareness_Alarmed, EStateType.CharacterAwareness_Alarmed);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddControlling(EStateType.CharacterAwareness_Alarmed, EStateMachineType.PlayerMain, EStateType.PlayerMain_AssassinationLeap);
			StateMachineBase<NpcCharacter, CharacterAwareness>.AddControlling(EStateType.CharacterAwareness_Alarmed, EStateMachineType.ProjectileFlight, EStateType.Projectile_Arrived);
			int[] alarmedStates = CharacterCondition.AlarmedStates;
			for (int i = 0; i < alarmedStates.Length; i++)
			{
				StateMachineBase<NpcCharacter, CharacterAwareness>.AddControlling(EStateType.CharacterAwareness_Alarmed, EStateMachineType.CharacterCondition, (EStateType)alarmedStates[i]);
			}
		}

		private static void OnEnterUndefined(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _prevstate, IState<NpcCharacter> _currentstate)
		{
			SetIcon(_stateMachine.Entity, IconMaterial.Invalid);
		}

		private static void OnUpdateUndefined(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _state)
		{
			ChangeStateByDetectionRange(_stateMachine);
			EnemyCharacter enemyCharacter = _stateMachine.Entity as EnemyCharacter;
			if (enemyCharacter != null)
			{
				SetIcon(enemyCharacter, IconMaterial.Invalid);
			}
		}

		private static void OnExitUndefined(IStateMachine<NpcCharacter> __statemachine, IPayload __payload, IState<NpcCharacter> __currentstate, IState<NpcCharacter> __nextstate)
		{
			SetIcon(__statemachine.Entity, IconMaterial.Invalid);
		}

		private static void OnEnterUnaware(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _previousState, IState<NpcCharacter> _currentState)
		{
			SetIcon(_stateMachine.Entity, IconMaterial.Invalid);
		}

		private static void OnUpdateUnaware(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _state)
		{
			ChangeStateByDetectionRange(_stateMachine);
		}

		private static void OnUpdateLow(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _state)
		{
			CharacterAwareness characterAwareness = (CharacterAwareness)_stateMachine;
			characterAwareness.SetIconByConspicuousCooldown();
			ChangeStateByDetectionRange(_stateMachine);
		}

		private static void OnUpdateHigh(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _state)
		{
			CharacterAwareness characterAwareness = (CharacterAwareness)_stateMachine;
			characterAwareness.SetIconByConspicuousCooldown();
			ChangeStateByDetectionRange(_stateMachine);
		}

		private static void OnEnterAlarmed(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _prevState, IState<NpcCharacter> _currentState)
		{
			SetIcon(_stateMachine.Entity, IconMaterial.AwarenessAlarmed);
			AlarmedState.AlarmedPayload alarmedPayload = (AlarmedState.AlarmedPayload)_payload;
			alarmedPayload.RestartActiveTimer(_stateMachine);
			PlayerCharacter targetEntity = _stateMachine.Entity.GetTargetEntity() as PlayerCharacter;
			UpdateLastKnownPositionToPlayer(_stateMachine, alarmedPayload, targetEntity);
			SetConfusedByPlayerFleeing(_stateMachine);
		}

		private static void OnUpdateAlarmed(IStateMachine<NpcCharacter> _stateMachine, IPayload _payload, IState<NpcCharacter> _state)
		{
			if (ShouldChangeToUndefinedState(_stateMachine, true))
			{
				ChangeStateByDetectionRange(_stateMachine);
				return;
			}
			CharacterAwareness characterAwareness = (CharacterAwareness)_stateMachine;
			AlarmedState.AlarmedPayload alarmedPayload = (AlarmedState.AlarmedPayload)_payload;
			PlayerCharacter playerCharacter = _stateMachine.Entity.GetTargetEntity() as PlayerCharacter;
			if (UpdateLastKnownPositionToPlayer(_stateMachine, alarmedPayload, playerCharacter))
			{
				SetConfusedByPlayerFleeing(_stateMachine);
			}
			else if (CharacterHelper.GetIsInParcour(playerCharacter) || characterAwareness.Entity.GetIsInParcour())
			{
				alarmedPayload.RestartActiveTimer(_stateMachine);
			}
			else if (alarmedPayload.ActiveTimer.IsFinished)
			{
				ChangeStateByDetectionRange(_stateMachine);
			}
		}

		private static bool UpdateLastKnownPositionToPlayer(IStateMachine<NpcCharacter> _stateMachine, AlarmedState.AlarmedPayload _awarenessPayload, PlayerCharacter _targetEntity)
		{
			EDetectionRange eDetectionRange = (EDetectionRange)_stateMachine.Entity.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range);
			if (eDetectionRange > EDetectionRange.Extended || (_targetEntity.GetIsHiding() && !_targetEntity.GetIsPerformingAssassination()))
			{
				return false;
			}
			_awarenessPayload.RestartActiveTimer(_stateMachine);
			_awarenessPayload.UpdateLastKnownPosition(_stateMachine.Entity, LogicMain.PlayerEntity);
			return true;
		}

		private static void OnExitUpdateLastKnownPosition(IStateMachine<NpcCharacter> _statemachine, IPayload _payload, IState<NpcCharacter> _currentstate, IState<NpcCharacter> _nextstate)
		{
			UpdateLastKnownPosition(_statemachine);
		}

		private static void UpdateLastKnownPosition(IStateMachine<NpcCharacter> _statemachine)
		{
			AlarmedState.AlarmedPayload payload = _statemachine.GetPayload<AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
			payload.UpdateLastKnownPosition(_statemachine.Entity, LogicMain.PlayerEntity);
		}

		private static bool ChangeStateByDetectionRange(IStateMachine<NpcCharacter> _statemachine)
		{
			EStateType eStateType = EStateType.CharacterAwareness_Unaware;
			GameEntity targetEntity = _statemachine.Entity.GetTargetEntity();
			if (ShouldChangeToUndefinedState(_statemachine))
			{
				eStateType = EStateType.CharacterAwareness_Undefined;
			}
			else
			{
				PlayerCharacter playerCharacter = targetEntity as PlayerCharacter;
				bool flag = playerCharacter != null && playerCharacter.PlayerCharacterStateMachines.PlayerAlertLevel.CurrentStateType == EStateType.CharacterAlertLevel_Exposed;
				switch ((EDetectionRange)_statemachine.Entity.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range))
				{
				case EDetectionRange.Close:
					eStateType = ((!_statemachine.Entity.GetIsApproachedToBeAssasinated()) ? ((!flag) ? EStateType.CharacterAwareness_High : EStateType.CharacterAwareness_Alarmed) : EStateType.CharacterAwareness_Undefined);
					break;
				case EDetectionRange.Normal:
					eStateType = ((!flag) ? ChooseLowOrUnaware(_statemachine) : EStateType.CharacterAwareness_Alarmed);
					break;
				default:
					eStateType = EStateType.CharacterAwareness_Unaware;
					break;
				}
			}
			if (eStateType != _statemachine.CurrentStateType && _statemachine.ScheduledStateType == EStateType.Invalid)
			{
				_statemachine.ScheduleStateChange(eStateType);
				return true;
			}
			return false;
		}

		private static EStateType ChooseLowOrUnaware(IStateMachine<NpcCharacter> _statemachine)
		{
			ICharacterConditionCarrierEntity characterConditionCarrierEntity = _statemachine.Entity as ICharacterConditionCarrierEntity;
			if (characterConditionCarrierEntity == null)
			{
				return EStateType.CharacterAwareness_Low;
			}
			switch (characterConditionCarrierEntity.CharacterConditionCarrier.CharacterCondition.CurrentStateType)
			{
			case EStateType.CharacterCondition_ThreateningVictim:
			{
				EntityId targetFromCondition = characterConditionCarrierEntity.CharacterConditionCarrier.CharacterCondition.GetTargetFromCondition(characterConditionCarrierEntity.CharacterConditionCarrier.CharacterCondition.CurrentStateType);
				if (((NpcCharacter)targetFromCondition.GameEntity).Patrol.VictimComponent is Captive)
				{
					return EStateType.CharacterAwareness_Low;
				}
				return EStateType.CharacterAwareness_Unaware;
			}
			case EStateType.CharacterCondition_Normal:
				return EStateType.CharacterAwareness_Low;
			default:
				return EStateType.CharacterAwareness_Unaware;
			}
		}

		private static bool ShouldChangeToUndefinedState(IStateMachine<NpcCharacter> _statemachine, bool _ignoreHiding = false)
		{
			NpcCharacter entity = _statemachine.Entity;
			PlayerCharacter playerCharacter = _statemachine.Entity.GetTargetEntity() as PlayerCharacter;
			return !IsAlive(entity) || playerCharacter == null || (!_ignoreHiding && DoesHidingDemandUndefinedState(_statemachine)) || entity.GetIsInCombat() || entity.GetIsUnconscious() || playerCharacter.CharacterStats.Get_bool(StatTypes.EStatType.Player_Disguise) || FleeingFromCombat(entity) || entity.GetIsBeingAssassinated() || playerCharacter.GetIsPerformingAssassination();
		}

		private static bool DoesHidingDemandUndefinedState(IStateMachine<NpcCharacter> _statemachine)
		{
			PlayerCharacter entity = _statemachine.Entity.GetTargetEntity() as PlayerCharacter;
			if (entity.GetIsExposed())
			{
				return false;
			}
			if (entity.GetIsStalking() && _statemachine.Entity.EntityStats.Get_int(StatTypes.EStatType.Character_Detection_Range) == 0)
			{
				return false;
			}
			return entity.GetIsHiding() || entity.GetIsMainStateDelayed(EStateType.PlayerMain_HidingInObject);
		}

		private static bool FleeingFromCombat(NpcCharacter _npcCharacter)
		{
			ICharacterConditionCarrier characterConditionCarrier = _npcCharacter.CharacterStateMachines as ICharacterConditionCarrier;
			return characterConditionCarrier != null && characterConditionCarrier.CharacterCondition.CurrentStateType == EStateType.CharacterCondition_Fleeing;
		}

		private static void SetConfusedByPlayerFleeing(IStateMachine<NpcCharacter> _stateMachine)
		{
			EnemyCharacter enemyCharacter = _stateMachine.Entity as EnemyCharacter;
			if (enemyCharacter != null)
			{
				enemyCharacter.SetConfusedByPlayerFleeing();
			}
		}

		private static bool IsAlive(NpcCharacter _npcCharacter)
		{
			return !_npcCharacter.GetIsDead();
		}

		private static float GetStat_float(IStateMachine<NpcCharacter> _stateMachine, StatTypes.EStatType _statType)
		{
			return _stateMachine.Entity.EntityStats.Get_float(_statType);
		}

		private static void SetIcon(Character _entity, IconMaterial _material)
		{
			EntityIcon iconAttachment = _entity.HeadIcon.IconAttachment;
			iconAttachment.SetIcon(_material, 1);
			iconAttachment.IsHiddenOnBorders = false;
		}

		private void SetIconByConspicuousCooldown()
		{
			PlayerCharacter targetAsPlayer = Entity.GetTargetAsPlayer();
			if (targetAsPlayer != null)
			{
				if (targetAsPlayer.PlayerCharacterStateMachines.PlayerAlertLevel.ConspiciousCooldownRunning)
				{
					m_awareInconExtensionTimer.Start(0.5f);
				}
				if (m_awareInconExtensionTimer.IsRunning)
				{
					SetIcon(Entity, IconMaterial.AwarenessHigh);
				}
				else
				{
					SetIcon(Entity, IconMaterial.AwarenessLow);
				}
			}
		}
	}
}
