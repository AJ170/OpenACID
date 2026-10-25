using System.Reflection;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class MeleeParameters : ParameterContainer
	{
		public AttackMeleeType AttackMeleeType { get; set; }

		public MeleeParameters()
		{
			AttackMeleeType = AttackMeleeType.Invalid;
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("AttackMeleeType") };
		}
	}
}
