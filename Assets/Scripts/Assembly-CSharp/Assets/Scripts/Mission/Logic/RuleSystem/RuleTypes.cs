using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class RuleTypes : IEnumDefinition
	{
		public static Enum<RuleTypes> Invalid
		{
			get
			{
				return Enum<RuleTypes>.Invalid;
			}
		}

		public bool GetLookupName(string _name, int _try, ref string _result)
		{
			if (_try == 1)
			{
				_result = _name;
				return true;
			}
			return false;
		}
	}
}
