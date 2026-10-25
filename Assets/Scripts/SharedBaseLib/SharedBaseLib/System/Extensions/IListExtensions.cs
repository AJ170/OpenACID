using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SharedBaseLib.Core;

namespace SharedBaseLib.System.Extensions
{
	public static class IListExtensions
	{
		private static readonly StringBuilder s_HelperBuilder = new StringBuilder(1024);

		public static void Swap<T>(this IList<T> list, int firstIndex, int secondIndex)
		{
			if (firstIndex != secondIndex)
			{
				T value = list[firstIndex];
				list[firstIndex] = list[secondIndex];
				list[secondIndex] = value;
			}
		}

		public static string ToArrayString<T>(this List<T> _list, Func<T, string> _converter)
		{
			lock (s_HelperBuilder)
			{
				s_HelperBuilder.Length = 0;
				for (int i = 0; i < _list.Count; i++)
				{
					s_HelperBuilder.Append(_converter(_list[i]));
				}
				return s_HelperBuilder.ToString();
			}
		}

		public static void Shuffle<T>(this IList<T> _list, Random32 _random)
		{
			for (int num = _list.Count - 1; num >= 1; num--)
			{
				int index = _random.Rand(0, num);
				T value = _list[num];
				_list[num] = _list[index];
				_list[index] = value;
			}
		}

		public static void Sort<T>(this BBSimpleList<T> _list, Func<T, T, int> _compareFunc)
		{
			for (int i = 0; i < _list.Count; i++)
			{
				for (int j = i + 1; j < _list.Count; j++)
				{
					if (_compareFunc(_list[i], _list[j]) > 0)
					{
						T value = _list[i];
						_list[i] = _list[j];
						_list[j] = value;
					}
				}
			}
		}

		public static bool Any<T>(this IEnumerable<T> _enumerable)
		{
			return _enumerable.GetEnumerator().MoveNext();
		}

		public static T First<T>(this IEnumerable<T> _enumerable)
		{
			IEnumerator<T> enumerator = _enumerable.GetEnumerator();
			enumerator.MoveNext();
			return enumerator.Current;
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> _enumerable)
		{
			IEnumerator<T> enumerator = _enumerable.GetEnumerator();
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
			return default(T);
		}

		public static int Count<T>(this IEnumerable<T> _enumerable)
		{
			int num = 0;
			foreach (T item in _enumerable)
			{
				T val = item;
				num++;
			}
			return num;
		}

		public static T[] ToArray<T>(this IEnumerable<T> _enumerable)
		{
			int num = 0;
			T[] array = new T[_enumerable.Count()];
			foreach (T item in _enumerable)
			{
				array[num] = item;
				num++;
			}
			return array;
		}

		public static T[] ToArray<T>(this HashSet<T> _set)
		{
			int num = 0;
			T[] array = new T[_set.Count];
			foreach (T item in _set)
			{
				array[num] = item;
				num++;
			}
			return array;
		}

		public static T[] ToArray<T>(this BBSimpleList<T> _list)
		{
			T[] array = new T[_list.Count];
			for (int i = 0; i < _list.Count; i++)
			{
				array[i] = _list[i];
			}
			return array;
		}

		public static List<T> ToList<T>(this IEnumerable<T> _enumerable)
		{
			BBList<T> bBList = new BBList<T>(_enumerable.Count());
			foreach (T item in _enumerable)
			{
				bBList.Add(item);
			}
			return bBList;
		}

		public static List<T> ToList<T>(this HashSet<T> _set)
		{
			BBList<T> bBList = new BBList<T>(_set.Count);
			foreach (T item in _set)
			{
				bBList.Add(item);
			}
			return bBList;
		}

		public static BBList<T> ToList<T>(this BBHashSet<T> _set)
		{
			BBList<T> bBList = new BBList<T>(_set.Count);
			foreach (T item in _set)
			{
				bBList.Add(item);
			}
			return bBList;
		}

		public static bool Contains<T>(this List<T> _enumerable, T _value)
		{
			for (int i = 0; i < _enumerable.Count; i++)
			{
				if (_enumerable[i].Equals(_value))
				{
					return true;
				}
			}
			return false;
		}

		public static List<KeyValuePair<TKey, TValue>> ToList<TKey, TValue>(this Dictionary<TKey, TValue> _dict)
		{
			List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>(_dict.Count);
			foreach (KeyValuePair<TKey, TValue> item in _dict)
			{
				list.Add(item);
			}
			return list;
		}

		public static List<T> Except<T>(this List<T> _list, List<T> _other)
		{
			List<T> list = new List<T>(Math.Max(_list.Count - _other.Count, 0));
			for (int i = 0; i < _list.Count; i++)
			{
				T item = _list[i];
				bool flag = false;
				for (int j = 0; j < _other.Count; j++)
				{
					if (item.Equals(_other[j]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<TSource> OrderBy<TSource, TKey>(this List<TSource> _list, Func<TSource, TKey> _keyFunc, Func<TKey, TKey, int> _compareFunc)
		{
			return _list.AsReadOnly().OrderBy(_keyFunc, _compareFunc);
		}

		public static List<TSource> OrderBy<TSource, TKey>(this ReadOnlyCollection<TSource> _list, Func<TSource, TKey> _keyFunc, Func<TKey, TKey, int> _compareFunc)
		{
			int count = _list.Count;
			List<TSource> list = new List<TSource>(count);
			list.AddRange(_list);
			for (int i = 0; i < count; i++)
			{
				TSource val = list[i];
				for (int j = i + 1; j < count; j++)
				{
					TSource val2 = list[j];
					if (_compareFunc(_keyFunc(val), _keyFunc(val2)) > 0)
					{
						list[i] = val2;
						list[j] = val;
						val = val2;
					}
				}
			}
			return list;
		}

		public static List<T> Where<T>(this List<T> _list, Func<T, bool> _predicate)
		{
			BBList<T> bBList = new BBList<T>();
			int count = _list.Count;
			for (int i = 0; i < count; i++)
			{
				T val = _list[i];
				if (_predicate(val))
				{
					bBList.Add(val);
				}
			}
			return bBList;
		}

		public static T First<T>(this List<T> _list)
		{
			if (_list.Count != 0)
			{
				return _list[0];
			}
			return default(T);
		}

		public static T First<T>(this List<T> _list, Func<T, bool> _predicate)
		{
			int count = _list.Count;
			for (int i = 0; i < count; i++)
			{
				T val = _list[i];
				if (_predicate(val))
				{
					return val;
				}
			}
			throw new Exception("No Matching item was found!");
		}

		public static T Last<T>(this List<T> _source)
		{
			if (_source.Count > 1)
			{
				return _source[_source.Count - 1];
			}
			if (_source.Count == 1)
			{
				return _source[0];
			}
			return default(T);
		}

		public static List<TSource> Concat<TSource>(this List<TSource> _this, IEnumerable<TSource> _second)
		{
			foreach (TSource item in _second)
			{
				_this.Add(item);
			}
			return _this;
		}

		public static TSource FirstOrDefault<TSource>(this IList<TSource> _list, Func<TSource, bool> _predicate)
		{
			for (int i = 0; i < _list.Count; i++)
			{
				TSource val = _list[i];
				if (_predicate(val))
				{
					return val;
				}
			}
			return default(TSource);
		}

		public static bool All<T>(this List<T> _list, Func<T, bool> _predicate)
		{
			for (int i = 0; i < _list.Count; i++)
			{
				T arg = _list[i];
				if (!_predicate(arg))
				{
					return false;
				}
			}
			return true;
		}

		public static bool All<T>(this T[] _array, Func<T, bool> _predicate)
		{
			foreach (T arg in _array)
			{
				if (!_predicate(arg))
				{
					return false;
				}
			}
			return true;
		}

		public static int Count<T>(this List<T> _list, Func<T, bool> _predicate)
		{
			return _list.AsReadOnly().Count(_predicate);
		}

		public static int Count<T>(this ReadOnlyCollection<T> _list, Func<T, bool> _predicate)
		{
			int num = 0;
			int count = _list.Count;
			for (int i = 0; i < count; i++)
			{
				T arg = _list[i];
				if (_predicate(arg))
				{
					num++;
				}
			}
			return num;
		}

		public static List<TKey> Select<T, TKey>(this IEnumerable<T> _enumerable, Func<T, TKey> _keyFunc)
		{
			BBList<TKey> bBList = new BBList<TKey>();
			foreach (T item in _enumerable)
			{
				bBList.Add(_keyFunc(item));
			}
			return bBList;
		}

		public static List<TKey> Select<T, TKey>(this List<T> _list, Func<T, TKey> _keyFunc)
		{
			int count = _list.Count;
			BBList<TKey> bBList = new BBList<TKey>(count);
			for (int i = 0; i < count; i++)
			{
				bBList.Add(_keyFunc(_list[i]));
			}
			return bBList;
		}

		public static BBSimpleList<TKey> Select<T, TKey>(this BBSimpleList<T> _list, Func<T, TKey> _keyFunc)
		{
			int count = _list.Count;
			BBSimpleList<TKey> bBSimpleList = new BBSimpleList<TKey>(count);
			for (int i = 0; i < count; i++)
			{
				bBSimpleList.Add(_keyFunc(_list[i]));
			}
			return bBSimpleList;
		}

		public static bool SequenceEqual<T>(this List<T> _list, List<T> _other)
		{
			int count = _list.Count;
			if (count != _other.Count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				if (!_list[i].Equals(_other[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool SequenceEqual<T>(this BBSimpleList<T> _list, BBSimpleList<T> _other)
		{
			int count = _list.Count;
			if (count != _other.Count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				if (!_list[i].Equals(_other[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static bool Any<TSource>(this IEnumerable<TSource> _source, Func<TSource, bool> _predicate)
		{
			foreach (TSource item in _source)
			{
				if (_predicate(item))
				{
					return true;
				}
			}
			return false;
		}

		public static bool Any<TSource>(this BBSimpleList<TSource> _source, Func<TSource, bool> _predicate)
		{
			for (int i = 0; i < _source.Count; i++)
			{
				TSource arg = _source[i];
				if (_predicate(arg))
				{
					return true;
				}
			}
			return false;
		}

		public static int FindIndex<TSource>(this BBSimpleList<TSource> _source, Func<TSource, bool> _predicate)
		{
			for (int i = 0; i < _source.Count; i++)
			{
				TSource arg = _source[i];
				if (_predicate(arg))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
