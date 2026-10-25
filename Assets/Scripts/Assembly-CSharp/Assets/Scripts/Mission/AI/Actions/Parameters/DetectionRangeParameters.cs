using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class DetectionRangeParameters : ParameterContainer
	{
		public EDetectionRange Range { get; set; }

		public DetectionRangeParameters()
		{
			Range = EDetectionRange.FarAway;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Range") };
		}
	}
}
