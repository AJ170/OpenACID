namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatCollectionValue : IReadonlyStatCollectionValue, IReadonlyStatValueBase, IStatValueBase
	{
		void Clear();

		IStatCollectionValue Add_int(int _value);

		IStatCollectionValue Add_float(float _value);

		IStatCollectionValue Add_bool(bool _value);

		IStatCollectionValue Add_string(string _value);

		IStatCollectionValue Remove_int(int _values);

		IStatCollectionValue Remove_float(float _value);

		IStatCollectionValue Remove_bool(bool _value);

		IStatCollectionValue Remove_string(string _value);

		IStatCollectionValue Set_int(int _values, int _idx);

		IStatCollectionValue Set_float(float _value, int _idx);

		IStatCollectionValue Set_bool(bool _value, int _idx);

		IStatCollectionValue Set_string(string _value, int _idx);
	}
}
