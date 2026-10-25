using System;
using UnityEngine;

namespace UbiCore.Utils
{
	[Serializable]
	public class Range<T> where T : IComparable
	{
		[SerializeField]
		private T _min;

		[SerializeField]
		private T _max;

		public T Min
		{
			get
			{
				return _min;
			}
			set
			{
				if (value.CompareTo(Max) <= 0)
				{
					_min = value;
				}
			}
		}

		public T Max
		{
			get
			{
				return _max;
			}
			set
			{
				if (value.CompareTo(Min) >= 0)
				{
					_max = value;
				}
			}
		}

		public override string ToString()
		{
			return string.Format("Min={0}, Max={1}", Min, Max);
		}

		public Range(T _min, T _max)
		{
			this._min = _min;
			this._max = _max;
			if (this._min.CompareTo(this._max) > 0)
			{
				this._min = this._max;
			}
		}
	}
}
