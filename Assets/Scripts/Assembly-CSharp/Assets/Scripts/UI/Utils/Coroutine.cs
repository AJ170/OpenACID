using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class Coroutine<T>
	{
		private T m_result;

		private Exception m_exception;

		public Coroutine coroutine { get; set; }

		public T value
		{
			get
			{
				if (m_exception != null)
				{
					throw m_exception;
				}
				return m_result;
			}
		}

		public IEnumerator WrappedEnumerator(IEnumerator _wrapped)
		{
			object yielded;
			while (true)
			{
				try
				{
					if (!_wrapped.MoveNext())
					{
						yield break;
					}
				}
				catch (Exception ex)
				{
					Exception e = ex;
					m_exception = e;
					yield break;
				}
				yielded = _wrapped.Current;
				if (yielded != null && yielded.GetType() == typeof(T))
				{
					break;
				}
				yield return yielded;
			}
			m_result = (T)yielded;
		}
	}
}
