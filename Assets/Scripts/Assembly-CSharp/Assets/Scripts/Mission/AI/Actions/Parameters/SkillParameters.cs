using System.Reflection;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.AI.Actions.Parameters
{
	public class SkillParameters : ParameterContainer
	{
		public Enum<SkillTypes> Skill { get; set; }

		public SkillParameters()
		{
			Skill = "Sweet_Music_01".ToEnum<SkillTypes>();
			base.ParameterDefinitions = new PropertyInfo[1] { GetType().GetProperty("Skill") };
		}
	}
}
