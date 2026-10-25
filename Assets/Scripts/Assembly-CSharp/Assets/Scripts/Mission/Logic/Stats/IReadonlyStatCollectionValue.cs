namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IReadonlyStatCollectionValue : IReadonlyStatValueBase
	{
		int Count { get; }

		bool Contains_int(int _value);

		bool Contains_float(float _value);

		bool Contains_bool(bool _value);

		bool Contains_string(string _value);

		int Get_int(int _idx);

		float Get_float(int _idx);

		bool Get_bool(int _idx);

		string Get_string(int _idx);

		int[] GetValuesCopy_int();

		float[] GetValuesCopy_float();

		bool[] GetValuesCopy_bool();

		string[] GetValuesCopy_string();

		void CopyTo(IStatValueWrapper _wrapper, int _idx);
	}
}
