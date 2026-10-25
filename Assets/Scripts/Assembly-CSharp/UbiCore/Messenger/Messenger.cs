using System;
using System.Collections.Generic;
using UbiCore.Common.Tree;
using UnityEngine;

namespace UbiCore.Messenger
{
	public sealed class Messenger
	{
		public delegate void EventCallback<T>(T _message) where T : IMessage;

		private delegate void EventCallback(IMessage _message);

		private static Messenger sInstance;

		private Dictionary<Type, EventCallback> mEventTable = new Dictionary<Type, EventCallback>(1024);

		private Dictionary<Delegate, EventCallback> mEventLookup = new Dictionary<Delegate, EventCallback>(1024);

		public static LogLevel LogLevel { get; set; }

		public static Messenger Instance
		{
			get
			{
				if (sInstance == null)
				{
					sInstance = new Messenger();
				}
				return sInstance;
			}
		}

		private Messenger()
		{
			if (Application.isPlaying && !MessengerManager.IsDestroyed)
			{
				new GameObject("MessengerManager").AddComponent<MessengerManager>();
			}
			LogLevel = LogLevel.Errors;
		}

		public void Clean()
		{
			mEventTable.Clear();
			mEventLookup.Clear();
		}

		public Node<MessageInformation> GetTree()
		{
			return GetSubTree(typeof(Message));
		}

		public void AddListener<T>(EventCallback<T> _handler) where T : IMessage
		{
			if (!mEventLookup.ContainsKey(_handler))
			{
				EventCallback eventCallback = delegate(IMessage _event)
				{
					_handler((T)_event);
				};
				mEventLookup[_handler] = eventCallback;
				Type typeFromHandle = typeof(T);
				EventCallback value;
				if (mEventTable.TryGetValue(typeFromHandle, out value))
				{
					value = (mEventTable[typeFromHandle] = (EventCallback)Delegate.Combine(value, eventCallback));
				}
				else
				{
					mEventTable[typeFromHandle] = eventCallback;
				}
			}
		}

		public void RemoveListener<T>(EventCallback<T> _handler) where T : IMessage
		{
			Type typeFromHandle = typeof(T);
			EventCallback value = null;
			if (!mEventLookup.TryGetValue(_handler, out value))
			{
				return;
			}
			EventCallback value2;
			if (mEventTable.TryGetValue(typeFromHandle, out value2))
			{
				value2 = (EventCallback)Delegate.Remove(value2, value);
				if (value2 == null)
				{
					mEventTable.Remove(typeFromHandle);
				}
				else
				{
					mEventTable[typeFromHandle] = value2;
				}
			}
			mEventLookup.Remove(_handler);
		}

		public void Broadcast(IMessage _message, SendMode _mode = SendMode.NoListenerRequired)
		{
			EventCallback value;
			if (mEventTable.TryGetValue(_message.GetType(), out value))
			{
				value(_message);
			}
			else if (_mode != 0)
			{
			}
		}

		private void Log(string _msg)
		{
			if (LogLevel >= LogLevel.All)
			{
			}
		}

		public int GetNumberOfListeners(Type _type)
		{
			EventCallback value;
			if (mEventTable.TryGetValue(_type, out value))
			{
				Delegate @delegate = value;
				if ((object)@delegate != null)
				{
					return @delegate.GetInvocationList().Length;
				}
			}
			return 0;
		}

		private Node<MessageInformation> GetSubTree(Type _root)
		{
			List<Node<MessageInformation>> list = new List<Node<MessageInformation>>(32);
			Type[] types = _root.Assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsSubclassOf(_root))
				{
					list.Add(GetSubTree(type));
				}
			}
			Node<MessageInformation> node = new Node<MessageInformation>(new MessageInformation(_root, GetNumberOfListeners(_root)));
			node.AddChildren(list);
			node.EnumerationOrder = eEumerationOrder.eDepthFirst;
			return node;
		}
	}
}
