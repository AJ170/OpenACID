using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.UI.LogicInterface
{
	public class SkillTypes : IEnumDefinition
	{
		public const string BaseSkillSuffix = "_01";

		public const string ParameterSeparator = "::";

		public const string Eagle_Vision = "Eagle_Vision";

		public const string Call_Hireling = "Call_Hireling";

		public const string Flee_From_Combat = "Flee_From_Combat";

		public const string Heartseeker = "Heartseeker_01";

		public const string Blinding_Metal = "Blinding_Metal_01";

		public const string Intimidation = "Intimidation_01";

		public const string Fast_Fingers = "Fast_Fingers_01";

		public const string Piercing_Strike = "Piercing_Strike_01";

		public const string Smoke_Bomb = "Smoke_Bomb_01";

		public const string Splinter_Bomb = "Splinter_Bomb_01";

		public const string Distraction = "Distraction_01";

		public const string Sweet_Music = "Sweet_Music_01";

		public const string Disguise = "Disguise_01";

		public const string Sticky_Bomb = "Sticky_Bomb_01";

		public const string Jack_in_the_Box = "Jack_in_the_Box_01";

		public const string Camouflage = "Camouflage_01";

		public const string Carnival = "Carnival_01";

		public const string Stunning_Strike = "Stunning_Strike_01";

		public const string Sunder = "Sunder_01";

		public const string Silent_Dagger = "Silent_Dagger_01";

		public const string Surge_of_Fury = "Surge_of_Fury_01";

		public const string Fan_of_Knives = "Fan_of_Knives_01";

		public const string Pickpocketing = "Pickpocketing_01";

		public const string Throw_Sand = "Throw_Sand_01";

		public const string Tripwire_Bomb = "Tripwire_Bomb_01";

		public const string Lossy_Dreams_Approaching = "Lossy_Dreams_Approaching";

		public const string Greed = "Greed_01";

		public const string Double_Identity = "Double_Identity_01";

		public static Enum<SkillTypes> Invalid
		{
			get
			{
				return Enum<SkillTypes>.Invalid;
			}
		}

		public bool GetLookupName(string _name, int _try, ref string _result)
		{
			int num = _name.IndexOf("::");
			if (num >= 0)
			{
				_name = _name.Substring(0, num);
			}
			switch (_try)
			{
			case 1:
				_result = _name;
				return true;
			case 2:
				_result = SkillData.FormatSkillName(_name, 0);
				return true;
			default:
				return false;
			}
		}
	}
}
