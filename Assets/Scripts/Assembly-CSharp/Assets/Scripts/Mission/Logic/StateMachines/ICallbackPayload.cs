using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface ICallbackPayload : IDelayPayload, IPayload
	{
		Action<bool> Callback { get; }
	}
}
