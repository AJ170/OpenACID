using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.Logic.Combat.AI.Parameters
{
	public class ActiveCombatBehaviorParameters : ParameterContainer
	{
		public const ECombatBehaviourType DefaultBehaviorType = ECombatBehaviourType.Invalid;

		public ECombatBehaviourType Value { get; set; }

		public ActiveCombatBehaviorParameters()
		{
			Value = ECombatBehaviourType.Invalid;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Value") };
		}
	}
}
