using System;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Types;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class SkillDataExtensions
	{
		public static UIModifierId GenerateModifierKey(string _name, int _rank)
		{
			return new UIModifierId(string.Format("{0}::{1}", _name, _rank));
		}

		public static float CompareMagnitude(this Assets.Scripts.Mission.Definitions.SkillData.SkillPreview _first, Assets.Scripts.Mission.Definitions.SkillData.SkillPreview _second)
		{
			if (_second == null)
			{
				return _first.Magnitude;
			}
			return (float)(Math.Round(_first.Magnitude) - Math.Round(_second.Magnitude));
		}
	}
}
