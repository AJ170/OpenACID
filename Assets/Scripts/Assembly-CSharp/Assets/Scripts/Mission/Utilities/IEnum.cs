namespace Assets.Scripts.Mission.Utilities
{
	public interface IEnum
	{
		IEnumInfos Type { get; }

		IEnum Make(int _index);

		IEnum Make(string _name);
	}
}
