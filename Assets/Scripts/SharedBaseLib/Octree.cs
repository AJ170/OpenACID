using System;
using SharedBaseLib.Core;
using UnityEngine;

public class Octree<T>
{
	private readonly float mLooseness;

	protected internal readonly int mDepth;

	private readonly int mMaxDepth;

	protected internal readonly Vector3 mCenter;

	protected internal readonly float mLength;

	protected internal readonly Bounds mBounds;

	protected internal BBList<T> mObjects = new BBList<T>();

	protected internal Octree<T>[] mChildren;

	protected internal readonly float mWorldSize;

	private bool mFixed;

	protected internal readonly Octree<T> mParent;

	public Octree(float _worldSize, float _looseness, int _depth)
		: this(_worldSize, _looseness, _depth, 0, Vector3.zero, (Octree<T>)null)
	{
	}

	public Octree(float _worldSize, float _looseness, int _depth, Vector3 _center)
		: this(_worldSize, _looseness, _depth, 0, _center, (Octree<T>)null)
	{
	}

	protected Octree(float _worldSize, float _looseness, int _maxDepth, int _depth, Vector3 _center, Octree<T> _parent)
	{
		mParent = _parent;
		mWorldSize = _worldSize;
		mLooseness = _looseness;
		mDepth = _depth;
		mCenter = _center;
		mMaxDepth = _maxDepth;
		mLength = mLooseness * mWorldSize / (float)Math.Pow(2.0, mDepth);
		float num = mLength / 2f;
		mBounds = default(Bounds);
		mBounds.min = mCenter - new Vector3(num, num, num);
		mBounds.max = mCenter + new Vector3(num, num, num);
	}

	public Octree<T> Find(T _obj)
	{
		if (mObjects.Contains(_obj))
		{
			return this;
		}
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				Octree<T> octree = mChildren[i];
				if (octree != null)
				{
					Octree<T> octree2 = octree.Find(_obj);
					if (octree2 != null)
					{
						return octree2;
					}
				}
			}
		}
		return null;
	}

	public Octree<T> Find(Bounds bounds)
	{
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				Octree<T> octree = mChildren[i];
				if (octree != null)
				{
					Octree<T> octree2 = octree.Find(bounds);
					if (octree2 != null)
					{
						return octree2;
					}
				}
			}
		}
		if (Contains(ref bounds))
		{
			return this;
		}
		return null;
	}

	public bool Remove(T _obj)
	{
		return mObjects.Remove(_obj);
	}

	protected internal bool Contains(ref Bounds _boundingBox)
	{
		if (mBounds.Contains(_boundingBox.min))
		{
			return mBounds.Contains(_boundingBox.max);
		}
		return false;
	}

	protected internal bool Contains(Bounds _boundingBox)
	{
		if (mBounds.Contains(_boundingBox.min))
		{
			return mBounds.Contains(_boundingBox.max);
		}
		return false;
	}

	protected internal bool Intersects(ref Bounds _boundingBox)
	{
		return mBounds.Intersects(_boundingBox);
	}

	public int Fixate()
	{
		mFixed = true;
		return DeleteEmptyLeafs();
	}

	private bool IsEmptyLeaf()
	{
		bool flag = true;
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				Octree<T> octree = mChildren[i];
				if (octree != null)
				{
					flag &= octree.IsEmptyLeaf();
				}
			}
		}
		if (flag)
		{
			return mObjects.Count == 0;
		}
		return false;
	}

	private int DeleteEmptyLeafs()
	{
		int num = 0;
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				if (mChildren[i] != null)
				{
					mChildren[i].DeleteEmptyLeafs();
					if (mChildren[i].IsEmptyLeaf())
					{
						mChildren[i] = null;
						num++;
					}
				}
			}
		}
		mObjects.TrimExcess();
		return num;
	}

	public Octree<T> Add(T _object, Vector3 _center, float _radius)
	{
		Bounds _boundingBox = default(Bounds);
		_boundingBox.min = _center - new Vector3(_radius, _radius, _radius);
		_boundingBox.max = _center + new Vector3(_radius, _radius, _radius);
		if (Contains(ref _boundingBox))
		{
			return Add(_object, _boundingBox, _center);
		}
		return null;
	}

	public Octree<T> Add(T _object, Bounds _boundingBox)
	{
		if (Contains(ref _boundingBox))
		{
			return Add(_object, _boundingBox, _boundingBox.center);
		}
		return null;
	}

	protected virtual Octree<T> ConstructChild(float _worldSize, float _looseness, int _maxDepth, int _depth, Vector3 _center, Octree<T> _parent)
	{
		return new Octree<T>(_worldSize, _looseness, _maxDepth, _depth, _center, _parent);
	}

	protected internal Octree<T> Add(T _object, Bounds _boundingBox, Vector3 _center)
	{
		if (mDepth < mMaxDepth)
		{
			if (mChildren == null)
			{
				mChildren = new Octree<T>[8];
			}
			int sectorIdx = GetSectorIdx(_center);
			if (mChildren[sectorIdx] == null)
			{
				mChildren[sectorIdx] = ConstructChild(mWorldSize, mLooseness, mMaxDepth, mDepth + 1, mCenter + GetSectorCenter(sectorIdx), this);
			}
			if (mChildren[sectorIdx].Contains(ref _boundingBox))
			{
				return mChildren[sectorIdx].Add(_object, _boundingBox, _center);
			}
		}
		mObjects.Add(_object);
		return this;
	}

	private Vector3 GetSectorCenter(int _idx)
	{
		float num = mLength / mLooseness / 4f;
		switch (_idx)
		{
		case 0:
			return new Vector3(0f - num, num, 0f - num);
		case 1:
			return new Vector3(num, num, 0f - num);
		case 2:
			return new Vector3(0f - num, num, num);
		case 3:
			return new Vector3(num, num, num);
		case 4:
			return new Vector3(0f - num, 0f - num, 0f - num);
		case 5:
			return new Vector3(num, 0f - num, 0f - num);
		case 6:
			return new Vector3(0f - num, 0f - num, num);
		default:
			return new Vector3(num, 0f - num, num);
		}
	}

	private int GetSectorIdx(Vector3 _position)
	{
		return ((!(_position.x <= mCenter.x)) ? 1 : 0) + ((!(_position.y >= mCenter.y)) ? 4 : 0) + ((!(_position.z <= mCenter.z)) ? 2 : 0);
	}

	public int GetNumberOfNodes()
	{
		int num = 1;
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				Octree<T> octree = mChildren[i];
				if (octree != null)
				{
					num += octree.GetNumberOfNodes();
				}
			}
		}
		return num;
	}

	public virtual void GetObjects(BBList<T> _objects, Vector3 _pos, float _radius)
	{
		Bounds _bounds = default(Bounds);
		_bounds.min = _pos - new Vector3(_radius, _radius, _radius);
		_bounds.max = _pos + new Vector3(_radius, _radius, _radius);
		GetObjects(_objects, ref _bounds);
	}

	public virtual void GetObjects(BBList<T> _objects, ref Bounds _bounds)
	{
		if (!Intersects(ref _bounds))
		{
			return;
		}
		if (mObjects.Count > 0)
		{
			for (int i = 0; i < mObjects.Count; i++)
			{
				_objects.Add(mObjects[i]);
			}
		}
		if (mChildren == null)
		{
			return;
		}
		for (int j = 0; j < mChildren.Length; j++)
		{
			Octree<T> octree = mChildren[j];
			if (octree != null)
			{
				octree.GetObjects(_objects, ref _bounds);
			}
		}
	}

	public void GetObjects(BBList<T> _objects)
	{
		if (mObjects.Count > 0)
		{
			for (int i = 0; i < mObjects.Count; i++)
			{
				_objects.Add(mObjects[i]);
			}
		}
		if (mChildren == null)
		{
			return;
		}
		for (int j = 0; j < mChildren.Length; j++)
		{
			Octree<T> octree = mChildren[j];
			if (octree != null)
			{
				octree.GetObjects(_objects);
			}
		}
	}

	public int DrawDebug()
	{
		int num = 0;
		if (mObjects.Count > 0)
		{
			Gizmos.DrawWireCube(mBounds.center, mBounds.size);
			num++;
		}
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				Octree<T> octree = mChildren[i];
				if (octree != null)
				{
					num += octree.DrawDebug();
				}
			}
		}
		return num;
	}

	public int DrawDebug(Vector3 _pos, float _radius)
	{
		Bounds _bounds = default(Bounds);
		_bounds.min = _pos - new Vector3(_radius, _radius, _radius);
		_bounds.max = _pos + new Vector3(_radius, _radius, _radius);
		return DrawDebug(ref _bounds);
	}

	public virtual int DrawDebug(ref Bounds _bounds)
	{
		if (!Intersects(ref _bounds))
		{
			return 0;
		}
		int num = 0;
		Gizmos.DrawWireCube(mBounds.center, mBounds.size);
		if (mObjects.Count > 0)
		{
			Gizmos.DrawWireCube(mBounds.center, mBounds.size);
			num += mObjects.Count;
		}
		if (mChildren != null)
		{
			for (int i = 0; i < mChildren.Length; i++)
			{
				Octree<T> octree = mChildren[i];
				if (octree != null)
				{
					num += octree.DrawDebug(ref _bounds);
				}
			}
		}
		return num;
	}
}
