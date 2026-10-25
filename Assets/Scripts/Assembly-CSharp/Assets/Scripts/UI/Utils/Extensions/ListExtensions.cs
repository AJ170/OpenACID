using System.Collections.Generic;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class ListExtensions
	{
		public static T Pop<T>(this List<T> _list)
		{
			if (_list.Count == 0)
			{
				return default(T);
			}
			T result = _list[0];
			_list.RemoveAt(0);
			return result;
		}

		public static bool TryGet<T>(this List<T> _list, int _index, out T _out)
		{
			_out = default(T);
			if (_index < 0 || _index >= _list.Count)
			{
				return false;
			}
			_out = _list[_index];
			return true;
		}
	}
}
