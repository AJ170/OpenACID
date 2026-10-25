using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class InteractionReasonParameters : ParameterContainer
	{
		public EInteractionReason InteractionReason { get; set; }

		public InteractionReasonParameters()
		{
			InteractionReason = EInteractionReason.AssassinationLeap;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("InteractionReason") };
		}
	}
}
