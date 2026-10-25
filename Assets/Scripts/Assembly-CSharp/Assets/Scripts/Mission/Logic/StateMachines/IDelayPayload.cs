using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IDelayPayload : IPayload
	{
		EStateType NextState { get; set; }

		EStateType CancelationState { get; set; }

		bool ImmediateStateChange { get; set; }

		Action FinishedAction { get; set; }

		TriggerAction TriggerAction { get; set; }

		bool IsActive { get; }

		bool HasBeenCanceled { get; }

		void ResetDelayState();

		void Cancel();
	}
}
