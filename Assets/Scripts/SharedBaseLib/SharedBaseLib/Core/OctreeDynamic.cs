using UbiCore;
using UbiCore.Debug;
using UnityEngine;

namespace SharedBaseLib.Core
{
	public class OctreeDynamic : Octree<IOctreeDynamicEntity>
	{
		public OctreeDynamic(float _worldSize, float _looseness, int _maxDepth, Vector3 _center)
			: this(_worldSize, _looseness, _maxDepth, 0, _center, null)
		{
		}

		protected OctreeDynamic(float _worldSize, float _looseness, int _maxDepth, int _depth, Vector3 _center, Octree<IOctreeDynamicEntity> _parent)
			: base(_worldSize, _looseness, _maxDepth, _depth, _center, _parent)
		{
		}

		protected override Octree<IOctreeDynamicEntity> ConstructChild(float _worldSize, float _looseness, int _maxDepth, int _depth, Vector3 _center, Octree<IOctreeDynamicEntity> _parent)
		{
			return new OctreeDynamic(_worldSize, _looseness, _maxDepth, _depth, _center, _parent);
		}

		public void Add(IOctreeDynamicEntity _entity)
		{
			_entity.CurrentCell = (OctreeDynamic)Add(_entity, new Bounds(_entity.Center, new Vector3(_entity.Radius, _entity.Radius, _entity.Radius)));
		}

		public void Update(IOctreeDynamicEntity _entity)
		{
			Profiling.Start("OctreeDynamic::Update", float.MaxValue);
			if (_entity.CurrentCell != null)
			{
				if (!_entity.CurrentCell.Contains(_entity))
				{
					_entity.CurrentCell.Remove(_entity);
					Profiling.Start("2nd find", float.MaxValue);
					_entity.CurrentCell = (OctreeDynamic)Add(_entity, _entity.Center, _entity.Radius);
					Profiling.Stop();
				}
			}
			else
			{
				Add(_entity);
			}
			Profiling.Stop();
		}

		private bool Contains(IOctreeDynamicEntity _boundingBox)
		{
			return mBounds.Intersects(_boundingBox.Center, _boundingBox.Radius);
		}

		private bool Contains(Vector3 _center, float _radius)
		{
			return mBounds.Intersects(_center, _radius);
		}

		public OctreeDynamic Find(Vector3 _center, float _radius)
		{
			if (mChildren != null)
			{
				for (int i = 0; i < mChildren.Length; i++)
				{
					OctreeDynamic octreeDynamic = (OctreeDynamic)mChildren[i];
					if (octreeDynamic != null)
					{
						OctreeDynamic octreeDynamic2 = octreeDynamic.Find(_center, _radius);
						if (octreeDynamic2 != null)
						{
							return octreeDynamic2;
						}
					}
				}
			}
			if (Contains(_center, _radius))
			{
				return this;
			}
			return null;
		}

		public override void GetObjects(BBList<IOctreeDynamicEntity> _objects, ref Bounds _bounds)
		{
			if (!Intersects(ref _bounds))
			{
				return;
			}
			for (int i = 0; i < mObjects.Count; i++)
			{
				IOctreeDynamicEntity octreeDynamicEntity = mObjects[i];
				if (_bounds.Intersects(octreeDynamicEntity.Center, octreeDynamicEntity.Radius))
				{
					_objects.Add(octreeDynamicEntity);
				}
			}
			if (mChildren == null)
			{
				return;
			}
			for (int j = 0; j < mChildren.Length; j++)
			{
				OctreeDynamic octreeDynamic = (OctreeDynamic)mChildren[j];
				if (octreeDynamic != null)
				{
					octreeDynamic.GetObjects(_objects, ref _bounds);
				}
			}
		}

		public override void GetObjects(BBList<IOctreeDynamicEntity> _objects, Vector3 _center, float _radius)
		{
			if (!mBounds.Intersects(_center, _radius))
			{
				return;
			}
			for (int i = 0; i < mObjects.Count; i++)
			{
				IOctreeDynamicEntity octreeDynamicEntity = mObjects[i];
				if ((_center - octreeDynamicEntity.Center).magnitude <= _radius + octreeDynamicEntity.Radius)
				{
					_objects.Add(octreeDynamicEntity);
				}
			}
			if (mChildren == null)
			{
				return;
			}
			for (int j = 0; j < mChildren.Length; j++)
			{
				OctreeDynamic octreeDynamic = (OctreeDynamic)mChildren[j];
				if (octreeDynamic != null)
				{
					octreeDynamic.GetObjects(_objects, _center, _radius);
				}
			}
		}

		public override int DrawDebug(ref Bounds _bounds)
		{
			if (!Intersects(ref _bounds))
			{
				return 0;
			}
			int num = 0;
			Gizmos.DrawWireCube(mBounds.center, mBounds.size);
			if (mObjects.Count > 0)
			{
				foreach (IOctreeDynamicEntity mObject in mObjects)
				{
					Gizmos.DrawWireSphere(mObject.Center, mObject.Radius);
					num++;
				}
			}
			if (mChildren != null)
			{
				for (int i = 0; i < mChildren.Length; i++)
				{
					Octree<IOctreeDynamicEntity> octree = mChildren[i];
					if (octree != null)
					{
						num += octree.DrawDebug(ref _bounds);
					}
				}
			}
			return num;
		}
	}
}
