using System;
using System.Collections;
using System.Collections.Generic;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace UbiCore.Common.Tree
{
	public class Node<T> : IDisposable, IEnumerable<Node<T>>, IEnumerable where T : class
	{
		private BBList<Node<T>> mChildren = new BBList<Node<T>>(32);

		private Node<T> mParent;

		private T mValue;

		public bool Leaf
		{
			get
			{
				return mChildren.Count == 0;
			}
		}

		public eEumerationOrder EnumerationOrder { get; set; }

		public Node<T> Parent
		{
			get
			{
				return mParent;
			}
			set
			{
				mParent = value;
			}
		}

		public BBList<Node<T>> Children
		{
			get
			{
				return mChildren;
			}
		}

		public T Value
		{
			get
			{
				return mValue;
			}
			set
			{
				mValue = value;
			}
		}

		public int Depth
		{
			get
			{
				Node<T> parent = mParent;
				int num = 0;
				while (parent != null)
				{
					num++;
					parent = parent.Parent;
				}
				return num;
			}
		}

		public Node(T _value, Node<T> _parent = null)
		{
			Value = _value;
			Parent = _parent;
			EnumerationOrder = eEumerationOrder.eBreadthFirst;
		}

		public void AddChild(Node<T> _child)
		{
			_child.Parent = this;
			mChildren.Add(_child);
		}

		public void AddChildren(IEnumerable<Node<T>> _children)
		{
			foreach (Node<T> _child in _children)
			{
				AddChild(_child);
			}
		}

		public Node<T> Find(Func<T, bool> _predicate)
		{
			if (_predicate(Value))
			{
				return this;
			}
			return Children.Select((Node<T> _child) => _child.Find(_predicate)).FirstOrDefault((Node<T> _child) => _child != null);
		}

		public Node<T> Find(T _value)
		{
			return Find((T _v) => _v == _value);
		}

		void IDisposable.Dispose()
		{
		}

		public IEnumerator<Node<T>> GetEnumerator()
		{
			if (EnumerationOrder != 0)
			{
				int num = 1;
				return new BreadthFirstEnumerator<T>(this);
			}
			return new DepthFirstEnumerator<T>(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
