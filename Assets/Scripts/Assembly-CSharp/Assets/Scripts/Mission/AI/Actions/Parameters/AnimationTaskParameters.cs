using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	internal class AnimationTaskParameters : ParameterContainer
	{
		public AnimationTaskManager.ELayer Layer { get; set; }

		public AnimationTaskType AnimationTaskType { get; set; }

		public AnimationTaskParameters()
		{
			Layer = AnimationTaskManager.ELayer.Base;
			AnimationTaskType = AnimationTaskType.Idle;
			base.ParameterDefinitions = new PropertyInfo[2]
			{
				GetType().GetProperty("Layer"),
				GetType().GetProperty("AnimationTaskType")
			};
		}
	}
}
