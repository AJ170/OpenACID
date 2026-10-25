using SharedBaseLib.Core;

namespace Assets.Scripts.Crowd
{
	public static class CrowdHelper
	{
		public static void SwapRemove<T>(this BBList<T> _list, T _item)
		{
			for (int i = 0; i < _list.Count; i++)
			{
				if (object.Equals(_item, _list[i]))
				{
					_list[i] = _list[_list.Count - 1];
					_list.RemoveAt(_list.Count - 1);
					break;
				}
			}
		}
	}
}
