using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class DistanceParameters : ParameterContainer
	{
		private const float DefaultDistance = 2f;

		public float Distance { get; set; }

		public DistanceParameters()
		{
			Distance = 2f;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Distance") };
		}
	}
}
