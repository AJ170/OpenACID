using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.Logic.Combat.AI.Parameters
{
	internal class ActionQueuedTypeParameters : ParameterContainer
	{
		public const CombatParticipant.EActionQueuedType DefaultBehaviorType = CombatParticipant.EActionQueuedType.Attack;

		public CombatParticipant.EActionQueuedType Value { get; set; }

		public ActionQueuedTypeParameters()
		{
			Value = CombatParticipant.EActionQueuedType.Attack;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Value") };
		}
	}
}
