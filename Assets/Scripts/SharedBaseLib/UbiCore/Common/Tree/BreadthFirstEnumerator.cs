using System;
using System.Collections;
using System.Collections.Generic;
using SharedBaseLib.Core;

namespace UbiCore.Common.Tree
{
	public sealed class BreadthFirstEnumerator<T> : IEnumerator<Node<T>>, IEnumerator, IDisposable where T : class
	{
		private Node<T> mRoot;

		private Node<T> mCurrent;

		private BBQueue<Node<T>> mQueue = new BBQueue<Node<T>>(32);

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

		public BreadthFirstEnumerator(Node<T> _root)
		{
			mRoot = _root;
			Reset();
		}

		public void Reset()
		{
			mCurrent = mRoot;
			mQueue.Enqueue(mCurrent);
		}

		public bool MoveNext()
		{
			if (mCurrent == null || mRoot == null)
			{
				return false;
			}
			if (mQueue.Count == 0)
			{
				return false;
			}
			mCurrent = mQueue.Dequeue();
			if (mCurrent == null)
			{
				return false;
			}
			Enqueue(mCurrent.Children);
			return true;
		}

		void IDisposable.Dispose()
		{
		}

		private void Enqueue(BBList<Node<T>> _list)
		{
			foreach (Node<T> item in _list)
			{
				mQueue.Enqueue(item);
			}
		}
	}
}
