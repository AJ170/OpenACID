using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IPredicatePayload : IDelayPayload, IPayload
	{
		Func<bool> Predicate { get; set; }
	}
}
