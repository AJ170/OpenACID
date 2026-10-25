using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class InteractionCharacterMain : StateMachineBase<InteractionCharacter, InteractionCharacterMain>
	{
		public class FleeState : State<InteractionCharacter>
		{
			public class FleeTargetPayload : IPayload
			{
				public Vector3 FleeTarget { get; set; }

				public virtual void Reset()
				{
				}
			}

			public FleeState(EStateType _stateType)
				: this(_stateType, null, null, null)
			{
			}

			public FleeState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
				: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
			{
			}

			public override IPayload CreatePayload(IStateMachine<InteractionCharacter> _stateMachine)
			{
				return new FleeTargetPayload();
			}
		}

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.InteractionCharacterMain;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.CharacterInteractionMain_Idle;
			}
		}

		public float CurrentNotoriety
		{
			get
			{
				return Entity.EntityStats.Get_float(StatTypes.EStatType.MissionCharacter_CurrentNotoriety);
			}
			private set
			{
				Entity.EntityStats.Set_float(StatTypes.EStatType.MissionCharacter_CurrentNotoriety, value);
			}
		}

		private bool FleeOnTransgression
		{
			get
			{
				return Entity.EntityStats.Get_bool(StatTypes.EStatType.MissionCharacter_FleeOnTransgression);
			}
		}

		private Vector3 FleePosition
		{
			get
			{
				return Entity.EntityStats.Get_Vector3(StatTypes.EStatType.MissionCharacter_FleePosition);
			}
		}

		private float MaxNotoriety
		{
			get
			{
				return Entity.EntityStats.Get_float(StatTypes.EStatType.MissionCharacter_MaxNotoriety);
			}
		}

		public InteractionCharacterMain(InteractionCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitInteractionCharacterMain()
		{
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.InitStateMachineBase();
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteractionMain_Idle, null, null, OnUpdateIdle);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddState<State<InteractionCharacter>>(EStateType.CharacterInteractionMain_Busy, OnEnterBusy, OnExitBusy, OnUpdateBusy);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddState<FleeState>(EStateType.CharacterInteractionMain_MissionAlerted, null, null, OnUpdateMissionAlerted);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddTransition(EStateType.CharacterInteractionMain_Idle, EStateType.CharacterInteractionMain_MissionAlerted);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddTransition(EStateType.CharacterInteractionMain_MissionAlerted, EStateType.CharacterInteractionMain_Idle);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddTransition(EStateType.CharacterInteractionMain_Idle, EStateType.CharacterInteractionMain_Busy);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddTransition(EStateType.CharacterInteractionMain_Busy, EStateType.CharacterInteractionMain_Idle);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddControlling(EStateType.CharacterInteractionMain_Idle, EStateMachineType.CharacterInteraction, EStateType.Generic_WaitingForAnim);
			StateMachineBase<InteractionCharacter, InteractionCharacterMain>.AddControlling(EStateType.CharacterInteractionMain_Busy, EStateMachineType.CharacterInteraction, EStateType.CharacterInteraction_OfferOpen);
		}

		private static void OnUpdateIdle(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> currentState)
		{
			if (stateMachine.Entity.NpcDefinition.Definition.InteractionType != 0)
			{
				stateMachine.Entity.HeadIcon.IsVisible = PlayerHelper.AssassinInteraction.CanInteractWithNpc(stateMachine.Entity.Id);
			}
			stateMachine.Cast<InteractionCharacterMain>().HandleNotoriety();
		}

		private static void OnEnterBusy(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> prevState, IState<InteractionCharacter> currentState)
		{
			InteractionCharacter entity = stateMachine.Entity;
			entity.HeadIcon.IsVisible = false;
			entity.InteractionCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Immortal);
			entity.CharacterAi.IsActive = false;
		}

		private static void OnUpdateBusy(IStateMachine<InteractionCharacter> _stateMachine, IPayload _payload, IState<InteractionCharacter> _currentState)
		{
			_stateMachine.Cast<InteractionCharacterMain>().HandleNotoriety();
		}

		private static void OnExitBusy(IStateMachine<InteractionCharacter> stateMachine, IPayload payload, IState<InteractionCharacter> currentState, IState<InteractionCharacter> nextState)
		{
			stateMachine.Entity.HeadIcon.IsVisible = true;
			stateMachine.Entity.InteractionCharacterStateMachines.CharacterVitality.ScheduleStateChange(EStateType.Character_Alive);
			stateMachine.Entity.HeadIcon.IconAttachment.SetIcon(IconMaterial.Invalid, 8);
			stateMachine.Entity.CharacterAi.IsActive = true;
		}

		private static void OnUpdateMissionAlerted(IStateMachine<InteractionCharacter> _stateMachine, IPayload _payload, IState<InteractionCharacter> _currentState)
		{
			_stateMachine.Cast<InteractionCharacterMain>().HandleNotoriety();
		}

		private void HandleNotoriety()
		{
			AwareInteractionCharacter awareInteractionCharacter = Entity as AwareInteractionCharacter;
			if (awareInteractionCharacter == null)
			{
				return;
			}
			float num = LogicMain.PlayerEntity.PlayerStats.Get_float(StatTypes.EStatType.Character_Conspicuous_Notoriety);
			if (Mathf.Abs(num - CurrentNotoriety) >= 0.001f)
			{
				CurrentNotoriety = num;
			}
			if (Entity.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
			{
				return;
			}
			if (Mathf.Max(CurrentNotoriety - 0.001f, 0f) > MaxNotoriety)
			{
				CharacterAwareness characterAwareness = awareInteractionCharacter.AwareInteractionCharacterStateMachines.CharacterAwareness;
				if (((IStateMachine)this).CurrentStateType == EStateType.CharacterInteractionMain_Idle)
				{
					if (FleeOnTransgression)
					{
						if (characterAwareness.CurrentStateType == EStateType.CharacterAwareness_Alarmed)
						{
							LogicMain.LogicInterface.OnSendInfo(EInfoEventType.CharacterAlarmed);
							((IStateMachine)this).ScheduleStateChange(EStateType.CharacterInteractionMain_MissionAlerted);
							FleeState.FleeTargetPayload payload = ((IStateMachine)this).GetPayload<FleeState.FleeTargetPayload>(EStateType.CharacterInteractionMain_MissionAlerted);
							payload.FleeTarget = FleePosition;
						}
					}
					else if (((IStateMachine)this).CurrentStateType == EStateType.CharacterInteractionMain_Idle)
					{
						LogicMain.LogicInterface.OnSendInfo(EInfoEventType.CharacterRejects);
						((IStateMachine)this).ScheduleStateChange(EStateType.CharacterInteractionMain_Busy);
					}
				}
				else if (((IStateMachine)this).CurrentStateType == EStateType.CharacterInteractionMain_Busy)
				{
					Entity.HeadIcon.IsVisible = true;
					Entity.HeadIcon.IconAttachment.SetIcon(IconMaterial.InteractUnavailableExposed, 8);
				}
			}
			else if (!FleeOnTransgression && ((IStateMachine)this).CurrentStateType == EStateType.CharacterInteractionMain_Busy)
			{
				((IStateMachine)this).ScheduleStateChange(EStateType.CharacterInteractionMain_Idle);
			}
		}
	}
}
