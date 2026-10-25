using System;

namespace SharedBaseLib.Core
{
	public class SimpleDelegate
	{
		public delegate void CallbackFunction(params object[] _params);

		private readonly BBList<CallbackFunction> m_callbacks = new BBList<CallbackFunction>();

		private readonly BBList<CallbackFunction> m_callbackSwap = new BBList<CallbackFunction>();

		private readonly object m_owner;

		private readonly Type[] m_paramTypes;

		public SimpleDelegate(object _owner, Type[] _paramTypes)
		{
			m_owner = _owner;
			m_paramTypes = _paramTypes;
		}

		public void Trigger(params object[] _params)
		{
			if (m_callbacks.Count != 0)
			{
				for (int i = 0; i < m_callbacks.Count; i++)
				{
					m_callbackSwap.Add(m_callbacks[i]);
				}
				for (int j = 0; j < m_callbackSwap.Count; j++)
				{
					m_callbackSwap[j](_params);
				}
				m_callbackSwap.Clear();
			}
		}

		public void Add(CallbackFunction _listener)
		{
			m_callbacks.Add(_listener);
		}

		public void AddUnique(CallbackFunction _listener)
		{
			if (!m_callbacks.Contains(_listener))
			{
				m_callbacks.Add(_listener);
			}
		}

		public void Remove(CallbackFunction _listener)
		{
			m_callbacks.Remove(_listener);
		}

		public static SimpleDelegate operator +(SimpleDelegate _delegate, CallbackFunction _other)
		{
			_delegate.Add(_other);
			return _delegate;
		}

		public static SimpleDelegate operator -(SimpleDelegate _delegate, CallbackFunction _other)
		{
			_delegate.Remove(_other);
			return _delegate;
		}
	}
}
