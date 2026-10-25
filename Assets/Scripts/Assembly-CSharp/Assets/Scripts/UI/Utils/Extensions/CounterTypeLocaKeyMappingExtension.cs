using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class CounterTypeLocaKeyMappingExtension
	{
		public static CounterTypeLocaKeyMapping.CounterTypeLocaKeyEntry GetCounterTypeLocaKey(this CounterTypeLocaKeyMapping _mapping, ECounterType _type)
		{
			if (_mapping == null)
			{
				return null;
			}
			CounterTypeLocaKeyMapping.CounterTypeLocaKeyEntry counterTypeLocaKeyEntry = _mapping.counterTypeLocaKeyEntries.Find((CounterTypeLocaKeyMapping.CounterTypeLocaKeyEntry _entry) => _entry != null && _entry.counter == _type);
			if (counterTypeLocaKeyEntry != null)
			{
				return counterTypeLocaKeyEntry;
			}
			return null;
		}
	}
}
