using AcierProtocol;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class ObjectInteraction : StateMachineBase<InteractableObject, ObjectInteraction>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.ObjectInteraction;
			}
		}

		public bool RequiresLockPick { get; set; }

		public override EStateType InitialState
		{
			get
			{
				EObjectCategory category = Entity.InteractableObjectDefinition.Definition.Category;
				if (category == EObjectCategory.Mission_EObjectCategory)
				{
					return EStateType.Object_Disabled;
				}
				return EStateType.Object_Enabled;
			}
		}

		public ObjectInteraction(InteractableObject _entity)
			: base(_entity)
		{
		}

		public static void InitObjectInteraction()
		{
			StateMachineBase<InteractableObject, ObjectInteraction>.InitStateMachineBase();
			StateMachineBase<InteractableObject, ObjectInteraction>.AddState<State<InteractableObject>>(EStateType.Object_Enabled, null, null, OnUpdateEnabled);
			StateMachineBase<InteractableObject, ObjectInteraction>.AddState<State<InteractableObject>>(EStateType.Object_Disabled, OnEnterDisabled, null, OnUpdateDisabled);
			StateMachineBase<InteractableObject, ObjectInteraction>.AddTransition(EStateType.Object_Enabled, EStateType.Object_Disabled);
			StateMachineBase<InteractableObject, ObjectInteraction>.AddTransition(EStateType.Object_Disabled, EStateType.Object_Enabled);
			StateMachineBase<InteractableObject, ObjectInteraction>.AddControlling(EStateType.Object_Disabled, EStateMachineType.PlayerMain, EStateType.PlayerMain_ObjectInteraction);
		}

		protected override void InitState(EStateType _initStateType)
		{
			base.InitState(_initStateType);
			Entity.HeadIcon.IsVisible = _initStateType == EStateType.Object_Enabled;
		}

		private static void OnEnterDisabled(IStateMachine<InteractableObject> stateMachine, IPayload payload, IState<InteractableObject> prevState, IState<InteractableObject> currentState)
		{
			IHeadIconCarrier entity = stateMachine.Entity;
			if (entity != null)
			{
				entity.HeadIcon.IsVisible = false;
			}
			stateMachine.Entity.InteractableObjectStats.Set_bool(StatTypes.EStatType.Interaction_Triggered, true);
			stateMachine.Entity.InteractableObjectStats.Set_int(StatTypes.EStatType.Entity_MapIconType, 0);
		}

		private static void OnUpdateDisabled(IStateMachine<InteractableObject> _stateMachine, IPayload _payload, IState<InteractableObject> _currentState)
		{
			IHeadIconCarrier entity = _stateMachine.Entity;
			if (entity != null)
			{
				entity.HeadIcon.IsVisible = PlayerHelper.AssassinInteraction.CanInteractWithInteractableObject(_stateMachine.Entity.Id);
			}
		}

		private static void OnUpdateEnabled(IStateMachine<InteractableObject> stateMachine, IPayload payload, IState<InteractableObject> currentState)
		{
			IHeadIconCarrier entity = stateMachine.Entity;
			if (entity != null)
			{
				entity.HeadIcon.IsVisible = PlayerHelper.AssassinInteraction.CanInteractWithInteractableObject(stateMachine.Entity.Id);
			}
		}
	}
}
