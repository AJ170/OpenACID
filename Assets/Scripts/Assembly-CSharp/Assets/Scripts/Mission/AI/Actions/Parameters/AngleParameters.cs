using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class AngleParameters : ParameterContainer
	{
		private const float DefaultAngle = 0f;

		public float Angle { get; set; }

		public bool Absolute { get; set; }

		public AngleParameters()
		{
			Angle = 0f;
			Absolute = true;
			base.ParameterDefinitions = new PropertyInfo[2]
			{
				GetType().GetProperty("Angle"),
				GetType().GetProperty("Absolute")
			};
		}
	}
}
