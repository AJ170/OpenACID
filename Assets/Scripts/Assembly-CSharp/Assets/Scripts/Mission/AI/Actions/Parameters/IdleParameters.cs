using System.Reflection;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class IdleParameters : ParameterContainer
	{
		public IdleType IdleType { get; set; }

		public bool CancelCurrentTask { get; set; }

		public IdleParameters()
		{
			IdleType = IdleType.Neutral;
			base.ParameterDefinitions = new PropertyInfo[2]
			{
				GetType().GetProperty("IdleType"),
				GetType().GetProperty("CancelCurrentTask")
			};
		}
	}
}
