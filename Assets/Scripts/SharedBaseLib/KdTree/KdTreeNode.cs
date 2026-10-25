namespace KdTree
{
	public class KdTreeNode<TValue>
	{
		private TValue value;

		private KdTreeNode<TValue> leftChild;

		private KdTreeNode<TValue> rightChild;

		public TValue Value
		{
			get
			{
				return value;
			}
			internal set
			{
				this.value = value;
			}
		}

		public KdTreeNode<TValue> LeftChild
		{
			get
			{
				return leftChild;
			}
			internal set
			{
				leftChild = value;
			}
		}

		public KdTreeNode<TValue> RightChild
		{
			get
			{
				return rightChild;
			}
			internal set
			{
				rightChild = value;
			}
		}

		public KdTreeNode(TValue value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			return string.Format("{0} -> {1} ; {2}", value, (leftChild == null) ? "-" : leftChild.value.ToString(), (rightChild == null) ? "-" : rightChild.value.ToString());
		}
	}
}
