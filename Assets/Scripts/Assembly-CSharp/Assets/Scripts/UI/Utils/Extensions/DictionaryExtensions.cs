using System;
using System.Collections.Generic;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> _dictionary, TKey _key, TValue _defaultValue)
		{
			TValue value;
			return (!_dictionary.TryGetValue(_key, out value)) ? _defaultValue : value;
		}

		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> _source, Func<TSource, bool> _predicate)
		{
			BBList<TSource> bBList = new BBList<TSource>();
			IEnumerator<TSource> enumerator = _source.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (_predicate(enumerator.Current))
				{
					bBList.Add(enumerator.Current);
				}
			}
			return bBList;
		}
	}
}
