using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class AlertLevelParameters : ParameterContainer
	{
		public EStateType AlertLevel { get; set; }

		public AlertLevelParameters()
		{
			AlertLevel = EStateType.CharacterAlertLevel_Anonymous;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("AlertLevel") };
		}
	}
}
