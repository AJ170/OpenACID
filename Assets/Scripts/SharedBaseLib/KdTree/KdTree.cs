using System;
using System.Collections;
using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;

namespace KdTree
{
	public class KdTree<TValue> : ICollection<TValue>, IEnumerable, IEnumerable<TValue>
	{
		private class ValueDistanceComparer : Comparer<TValue>
		{
			private int dimensionality;

			private Vector3 location;

			public Func<TValue, Vector3> locationGetter;

			public ValueDistanceComparer(Func<TValue, Vector3> locationGetter, int dimensionality, Vector3 location)
			{
				this.locationGetter = locationGetter;
				this.dimensionality = dimensionality;
				this.location = location;
			}

			public override int Compare(TValue x, TValue y)
			{
				return KdTree<TValue>.fieldComparer.Compare(KdTree<TValue>.Norm(locationGetter(x), location), KdTree<TValue>.Norm(locationGetter(y), location));
			}
		}

		private class ValueLocationComparer : Comparer<TValue>
		{
			private Func<TValue, Vector3> locationGetter;

			public int Dimension { get; set; }

			public ValueLocationComparer(Func<TValue, Vector3> locationGetter)
			{
				this.locationGetter = locationGetter;
			}

			public override int Compare(TValue x, TValue y)
			{
				return KdTree<TValue>.fieldComparer.Compare(locationGetter(x)[Dimension], locationGetter(y)[Dimension]);
			}
		}

		private const int Dimensionality = 3;

		private static readonly Func<TValue, Vector3> defaultLocationGetter = (TValue el) => (Vector3)(object)el;

		private static readonly IComparer<float> fieldComparer = Comparer<float>.Default;

		private static readonly IArithmetic<float> fieldArithmetic = Arithmetic<float>.Default;

		private Func<TValue, Vector3> locationGetter;

		private KdTreeNode<TValue> root;

		public KdTreeNode<TValue> Root
		{
			get
			{
				return root;
			}
		}

		public Func<TValue, Vector3> LocationGetter
		{
			get
			{
				return locationGetter;
			}
		}

		bool ICollection<TValue>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		int ICollection<TValue>.Count
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		private KdTree()
		{
			locationGetter = defaultLocationGetter;
		}

		public void Clear()
		{
			root.LeftChild = null;
			root.RightChild = null;
		}

		public bool Contains(TValue value)
		{
			return Find(value) != null;
		}

		public static KdTree<TValue> Construct(TValue[] _elements, Func<TValue, Vector3> _locationGetter = null, bool _isSorted = false)
		{
			if (_elements == null)
			{
				throw new ArgumentNullException("_elements");
			}
			KdTree<TValue> kdTree = new KdTree<TValue>();
			if (_locationGetter != null)
			{
				kdTree.locationGetter = _locationGetter;
			}
			kdTree.root = Construct(_elements, _elements.GetLowerBound(0), _elements.GetUpperBound(0), 0, new ValueLocationComparer(kdTree.locationGetter), _isSorted);
			return kdTree;
		}

		private static KdTreeNode<TValue> Construct(TValue[] _elements, int _startIndex, int _endIndex, int _depth, ValueLocationComparer _valueLocationComparer, bool _isSorted)
		{
			int num = _endIndex - _startIndex + 1;
			if (num == 0)
			{
				return null;
			}
			_valueLocationComparer.Dimension = _depth % 3;
			if (!_isSorted)
			{
				Array.Sort(_elements, _startIndex, num, _valueLocationComparer);
			}
			int num2 = _startIndex + num / 2;
			return new KdTreeNode<TValue>(_elements[num2])
			{
				LeftChild = Construct(_elements, _startIndex, num2 - 1, _depth + 1, _valueLocationComparer, _isSorted),
				RightChild = Construct(_elements, num2 + 1, _endIndex, _depth + 1, _valueLocationComparer, _isSorted)
			};
		}

		public IEnumerable<TValue> FindInRange(Vector3 location, float range)
		{
			BBList<TValue> bBList = new BBList<TValue>();
			FindInRange(location, root, range, bBList, 0);
			return bBList.AsReadOnly();
		}

		public void FindInRange(Vector3 location, float range, IList<TValue> nodesList)
		{
			FindInRange(location, root, range, nodesList, 0);
		}

		private void FindInRange(Vector3 location, KdTreeNode<TValue> node, float range, IList<TValue> valuesList, int depth)
		{
			if (node == null)
			{
				return;
			}
			int index = depth % 3;
			Vector3 nodeLocation = locationGetter(node.Value);
			float x = Norm(nodeLocation, location);
			if (fieldComparer.Compare(x, range) <= 0)
			{
				valuesList.Add(node.Value);
			}
			KdTreeNode<TValue> kdTreeNode = ((fieldComparer.Compare(location[index], nodeLocation[index]) < 0) ? node.LeftChild : node.RightChild);
			if (kdTreeNode != null)
			{
				FindInRange(location, kdTreeNode, range, valuesList, depth + 1);
			}
			if (fieldComparer.Compare(range, fieldArithmetic.Abs(fieldArithmetic.Subtract(nodeLocation[index], location[index]))) > 0)
			{
				KdTreeNode<TValue> kdTreeNode2 = ((kdTreeNode == node.LeftChild) ? node.RightChild : node.LeftChild);
				if (kdTreeNode2 != null)
				{
					FindInRange(location, kdTreeNode2, range, valuesList, depth + 1);
				}
			}
		}

		public static float Norm(Vector3 _nodeLocation, Vector3 _location)
		{
			Vector3 vector = default(Vector3);
			for (int i = 0; i < 3; i++)
			{
				vector[i] = _nodeLocation[i] - _location[i];
			}
			double num = 0.0;
			for (int j = 0; j < 3; j++)
			{
				num += Math.Pow(Math.Abs(vector[j]), 2.0);
			}
			return (float)Math.Pow(num, 0.5);
		}

		public TValue FindNearestNeighbor(Vector3 location)
		{
			return FindNearestNeighbor(location, root, root.Value, fieldArithmetic.MaxValue, 0);
		}

		private TValue FindNearestNeighbor(Vector3 location, KdTreeNode<TValue> node, TValue bestValue, float bestDistance, int depth)
		{
			if (node == null)
			{
				return bestValue;
			}
			int index = depth % 3;
			Vector3 nodeLocation = locationGetter(node.Value);
			float num = Norm(nodeLocation, location);
			if (!fieldArithmetic.AlmostEqual(num, fieldArithmetic.Zero) && fieldComparer.Compare(num, bestDistance) < 0)
			{
				bestValue = node.Value;
				bestDistance = num;
			}
			KdTreeNode<TValue> kdTreeNode = ((fieldComparer.Compare(location[index], nodeLocation[index]) < 0) ? node.LeftChild : node.RightChild);
			if (kdTreeNode != null)
			{
				TValue val = FindNearestNeighbor(location, kdTreeNode, bestValue, bestDistance, depth + 1);
				float num2 = Norm(locationGetter(val), location);
				bestValue = val;
				bestDistance = num2;
			}
			if (fieldComparer.Compare(bestDistance, fieldArithmetic.Abs(fieldArithmetic.Subtract(nodeLocation[index], location[index]))) > 0)
			{
				KdTreeNode<TValue> kdTreeNode2 = ((kdTreeNode == node.LeftChild) ? node.RightChild : node.LeftChild);
				if (kdTreeNode2 != null)
				{
					TValue val2 = FindNearestNeighbor(location, kdTreeNode2, bestValue, bestDistance, depth + 1);
					float num3 = Norm(locationGetter(val2), location);
					bestValue = val2;
					bestDistance = num3;
				}
			}
			return bestValue;
		}

		public KdTreeNode<TValue> Add(TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Add(value, root, 0);
		}

		private KdTreeNode<TValue> Add(TValue value, KdTreeNode<TValue> node, int depth)
		{
			if (node == null)
			{
				node = new KdTreeNode<TValue>(value);
			}
			else
			{
				int index = depth % 3;
				if (fieldComparer.Compare(locationGetter(value)[index], locationGetter(node.Value)[index]) <= 0)
				{
					node.LeftChild = Add(value, node.LeftChild, depth + 1);
				}
				else
				{
					node.RightChild = Add(value, node.RightChild, depth + 1);
				}
			}
			return node;
		}

		public KdTreeNode<TValue> Remove(TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Remove(value, root, 0);
		}

		private KdTreeNode<TValue> Remove(TValue value, KdTreeNode<TValue> node, int depth)
		{
			if (node == null)
			{
				return null;
			}
			int num = depth % 3;
			Vector3 vector = locationGetter(value);
			Vector3 vector2 = locationGetter(node.Value);
			int num2 = fieldComparer.Compare(vector[num], vector2[num]);
			if (num2 < 0)
			{
				node.LeftChild = Remove(value, node.LeftChild, depth + 1);
			}
			else if (num2 > 0)
			{
				node.RightChild = Remove(value, node.RightChild, depth + 1);
			}
			else if (node.RightChild != null)
			{
				node.Value = FindMinimum(node.RightChild, num, depth + 1);
				node.RightChild = Remove(node.Value, node.RightChild, depth + 1);
			}
			else if (node.LeftChild != null)
			{
				node.Value = FindMinimum(node.LeftChild, num, depth + 1);
				node.RightChild = Remove(node.Value, node.LeftChild, depth + 1);
				node.LeftChild = null;
			}
			else
			{
				node = null;
			}
			return node;
		}

		public KdTreeNode<TValue> Find(TValue value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return Find(value, root);
		}

		private KdTreeNode<TValue> Find(TValue value, KdTreeNode<TValue> node)
		{
			if (node == null)
			{
				return null;
			}
			if (node.Value.Equals(value))
			{
				return node;
			}
			return Find(value, node.LeftChild) ?? Find(value, node.RightChild);
		}

		private TValue FindMinimum(KdTreeNode<TValue> node, int splittingDimension, int depth)
		{
			if (node == null)
			{
				return default(TValue);
			}
			if (depth % 3 == splittingDimension)
			{
				if (node.LeftChild == null)
				{
					return node.Value;
				}
				return FindMinimum(node.LeftChild, splittingDimension, depth + 1);
			}
			Vector3 vector = locationGetter(node.Value);
			TValue val = FindMinimum(node.LeftChild, splittingDimension, depth + 1);
			TValue val2 = FindMinimum(node.RightChild, splittingDimension, depth + 1);
			bool flag = val != null && fieldComparer.Compare(locationGetter(val)[splittingDimension], vector[splittingDimension]) < 0;
			bool flag2 = val2 != null && fieldComparer.Compare(locationGetter(val2)[splittingDimension], vector[splittingDimension]) < 0;
			if (flag && !flag2)
			{
				return val;
			}
			if (flag2)
			{
				return val2;
			}
			return node.Value;
		}

		void ICollection<TValue>.Add(TValue item)
		{
			Add(item);
		}

		bool ICollection<TValue>.Remove(TValue item)
		{
			return Remove(item) != null;
		}

		public void CopyTo(TValue[] array, int arrayIndex)
		{
			IEnumerator<TValue> enumerator = GetEnumerator();
			int num = arrayIndex;
			while (enumerator.MoveNext())
			{
				array[num++] = enumerator.Current;
			}
		}

		public IEnumerator<TValue> GetEnumerator()
		{
			Stack<KdTreeNode<TValue>> visitedNodes = new Stack<KdTreeNode<TValue>>();
			visitedNodes.Push(root);
			while (visitedNodes.Count > 0)
			{
				KdTreeNode<TValue> node = visitedNodes.Pop();
				yield return node.Value;
				if (node.LeftChild != null)
				{
					visitedNodes.Push(node.LeftChild);
				}
				if (node.RightChild != null)
				{
					visitedNodes.Push(node.RightChild);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TValue>)this).GetEnumerator();
		}
	}
}
