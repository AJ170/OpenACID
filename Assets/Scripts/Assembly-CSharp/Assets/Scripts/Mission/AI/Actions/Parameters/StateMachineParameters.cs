using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class StateMachineParameters : ParameterContainer
	{
		public EStateType State { get; set; }

		public StateMachineParameters()
		{
			State = EStateType.Invalid;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("State") };
		}
	}
}
