using System;
using System.Collections;
using System.Collections.Generic;
using SharedBaseLib.Core;

namespace UbiCore.Common.Tree
{
	public sealed class DepthFirstEnumerator<T> : IEnumerator<Node<T>>, IEnumerator, IDisposable where T : class
	{
		private Node<T> mRoot;

		private Node<T> mCurrent;

		private Stack<Node<T>> mStack = new Stack<Node<T>>(32);

		public Node<T> Current
		{
			get
			{
				return mCurrent;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public DepthFirstEnumerator(Node<T> _root)
		{
			mRoot = _root;
			Reset();
		}

		public void Reset()
		{
			mCurrent = mRoot;
			mStack.Push(mCurrent);
		}

		public bool MoveNext()
		{
			if (mCurrent == null || mRoot == null)
			{
				return false;
			}
			if (mStack.Count == 0)
			{
				return false;
			}
			mCurrent = mStack.Pop();
			Push(mCurrent.Children);
			return true;
		}

		void IDisposable.Dispose()
		{
		}

		private void Push(BBList<Node<T>> _list)
		{
			for (int num = _list.Count - 1; num >= 0; num--)
			{
				mStack.Push(_list[num]);
			}
		}
	}
}
