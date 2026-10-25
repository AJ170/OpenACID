namespace Assets.Scripts.Mission.Utilities
{
	public interface IEnumDefinition
	{
		bool GetLookupName(string _name, int _try, ref string _result);
	}
}
