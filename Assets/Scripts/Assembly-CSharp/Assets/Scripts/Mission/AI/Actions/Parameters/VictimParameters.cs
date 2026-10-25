using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class VictimParameters : ParameterContainer
	{
		public EVictimType VictimType { get; set; }

		public VictimParameters()
		{
			VictimType = EVictimType.Invalid;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("VictimType") };
		}
	}
}
