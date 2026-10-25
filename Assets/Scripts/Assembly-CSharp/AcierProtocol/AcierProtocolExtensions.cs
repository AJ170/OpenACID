using System;
using System.Collections.Generic;
using SharedBaseLib.System.Extensions;

namespace AcierProtocol
{
	public static class AcierProtocolExtensions
	{
		public static int Count<T>(this T[] _array, Predicate<T> _predicate)
		{
			int num = 0;
			for (int i = 0; i < _array.Length; i++)
			{
				if (_predicate(_array[i]))
				{
					num++;
				}
			}
			return num;
		}

		public static T Find<T>(this T[] _array, Predicate<T> _predicate)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (_predicate(_array[i]))
				{
					return _array[i];
				}
			}
			return default(T);
		}

		public static T[] Where<T>(this T[] _array, Predicate<T> _predicate)
		{
			int num = 0;
			for (int i = 0; i < _array.Length; i++)
			{
				if (_predicate(_array[i]))
				{
					num++;
				}
			}
			T[] array = new T[num];
			int num2 = 0;
			for (int j = 0; j < _array.Length; j++)
			{
				if (_predicate(_array[j]))
				{
					array[num2++] = _array[j];
				}
			}
			return array;
		}

		public static T[] RemoveAt<T>(this T[] _source, int _index)
		{
			T[] array = new T[_source.Length - 1];
			if (_index >= 0)
			{
				Array.Copy(_source, 0, array, 0, _index);
			}
			if (_index < _source.Length - 1)
			{
				Array.Copy(_source, _index + 1, array, _index, _source.Length - _index - 1);
			}
			return array;
		}

		public static T[] Remove<T>(this T[] _source, T _obj) where T : class
		{
			if (_obj != null)
			{
				for (int i = 0; i < _source.Length; i++)
				{
					if (object.Equals(_obj, _source[i]))
					{
						_source = _source.RemoveAt(i);
						break;
					}
				}
			}
			return _source;
		}

		public static T First<T>(this T[] _array, Predicate<T> _predicate)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (_predicate(_array[i]))
				{
					return _array[i];
				}
			}
			throw new Exception("Not found");
		}

		public static int FindIndex<T>(this T[] _array, Predicate<T> _predicate)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (_predicate(_array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static bool Contains<T>(this T[] _array, T _item)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (object.Equals(_item, _array[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool Exists<T>(this T[] _array, Predicate<T> _predicate)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (_predicate(_array[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static int IndexOf<T>(this T[] _array, T _item)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (object.Equals(_item, _array[i]))
				{
					return i;
				}
			}
			return -1;
		}

		public static T[] AddRange<T>(this T[] _array, IEnumerable<T> _items)
		{
			int num = _array.Length;
			T[] array = _items.ToArray();
			Array.Resize(ref _array, num + array.Length);
			Array.Copy(array.ToArray(), 0, _array, num, array.Length);
			return _array;
		}

		public static T[] AddRange<T>(this T[] _array, IList<T> _items)
		{
			int num = _array.Length;
			Array.Resize(ref _array, num + _items.Count);
			Array.Copy(_items.ToArray(), 0, _array, num, _items.Count);
			return _array;
		}

		public static T[] AddRange<T>(this T[] _array, T[] _items)
		{
			int num = _array.Length;
			Array.Resize(ref _array, num + _items.Length);
			Array.Copy(_items.ToArray(), 0, _array, num, _items.Length);
			return _array;
		}

		public static T[] Add<T>(this T[] _array, T _item)
		{
			int num = _array.Length;
			Array.Resize(ref _array, num + 1);
			_array[_array.Length - 1] = _item;
			return _array;
		}

		public static bool ArrayEqual<T>(this T[] _lhs, T[] _rhs)
		{
			return _lhs.ArrayEqual(_rhs, (T _x, T _y) => _x.Equals(_y));
		}

		public static bool ArrayEqual<T>(this T[] _lhs, T[] _rhs, Func<T, T, bool> _compare)
		{
			if (_lhs == null && _rhs == null)
			{
				return true;
			}
			if (_lhs == null || _rhs == null)
			{
				return false;
			}
			if (_lhs.Length != _rhs.Length)
			{
				return false;
			}
			for (int i = 0; i < _lhs.Length; i++)
			{
				if (!_compare(_lhs[i], _rhs[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
