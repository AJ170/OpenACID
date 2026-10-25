using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class FleeReasonParameters : ParameterContainer
	{
		public enum FleeReason
		{
			Default = 0,
			StrikingAssassination = 1,
			FleeFromCombat = 2,
			FleeFromCapture = 3,
			BombExplosion = 4,
			WalkFromCombat = 5
		}

		public FleeReason Reason { get; set; }

		public FleeReasonParameters()
		{
			Reason = FleeReason.Default;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Reason") };
		}
	}
}
