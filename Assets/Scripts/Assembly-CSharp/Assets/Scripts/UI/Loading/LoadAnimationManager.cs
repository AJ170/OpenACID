using System.Collections;
using System.Collections.Generic;
using Database;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Loading
{
	public class LoadAnimationManager : MonoBehaviour
	{
		public struct SLoadObjectInfo
		{
			private readonly UIWidget m_loadObject;

			public UIWidget LoadObject
			{
				get
				{
					return m_loadObject;
				}
			}

			public float Size
			{
				get
				{
					return Mathf.Min(LoadObject.width, LoadObject.height);
				}
			}

			public SLoadObjectInfo(UIWidget _object)
			{
				m_loadObject = _object;
			}

			public bool Equals(SLoadObjectInfo _other)
			{
				return LoadObject == _other.LoadObject;
			}

			public override bool Equals(object _obj)
			{
				if (object.ReferenceEquals(null, _obj))
				{
					return false;
				}
				return _obj is SLoadObjectInfo && Equals((SLoadObjectInfo)_obj);
			}

			public override int GetHashCode()
			{
				return LoadObject.GetHashCode();
			}

			public static bool operator ==(SLoadObjectInfo _lhs, SLoadObjectInfo _rhs)
			{
				return _lhs.LoadObject == _rhs.LoadObject;
			}

			public static bool operator !=(SLoadObjectInfo _lhs, SLoadObjectInfo _rhs)
			{
				return !(_lhs == _rhs);
			}
		}

		private class PoolObject
		{
			public LoadAnimationDefinition Def { get; private set; }

			public bool IsPooled { get; set; }

			public PoolObject(LoadAnimationDefinition _def)
			{
				Def = _def;
			}
		}

		private class LoadAnimWrapper
		{
			private float m_showTimeDelay;

			private bool m_shownWithTimeDelay;

			public SLoadObjectInfo Info { get; private set; }

			public LoadAnimationDefinition LoadAnimDefinition { get; private set; }

			public float Size
			{
				get
				{
					return Info.Size;
				}
			}

			public LoadAnimWrapper(SLoadObjectInfo _info)
			{
				Info = _info;
			}

			public void Set(LoadAnimationDefinition _def)
			{
				LoadAnimDefinition = _def;
				LoadAnimDefinition.LoadAnimPrefab.transform.parent = Info.LoadObject.transform;
				LoadAnimDefinition.LoadAnimPrefab.transform.localPosition = Vector3.zero;
				LoadAnimDefinition.LoadAnimPrefab.transform.localRotation = Quaternion.identity;
				LoadAnimDefinition.LoadAnimPrefab.transform.localScale = Vector3.one;
				float num = Info.LoadObject.pivotOffset.x - 0.5f;
				float num2 = Info.LoadObject.pivotOffset.y - 0.5f;
				LoadAnimDefinition.LoadAnimPrefab.transform.localPosition = new Vector3((float)Info.LoadObject.width * (0f - num), (float)Info.LoadObject.height * (0f - num2), 0f);
				LoadAnimDefinition.LoadAnimPrefab.depth = Info.LoadObject.depth + 10;
			}

			public void Show(int _delay)
			{
				m_showTimeDelay = _delay;
				m_shownWithTimeDelay = true;
			}

			public void Update(float _delta)
			{
				m_showTimeDelay -= _delta;
				if (m_showTimeDelay <= 0f && !m_shownWithTimeDelay)
				{
					m_shownWithTimeDelay = false;
					LoadAnimDefinition.Show();
				}
			}

			public void Hide()
			{
				m_shownWithTimeDelay = false;
				LoadAnimDefinition.Hide();
			}
		}

		private class DescendingFloatComparer : IComparer<float>
		{
			public int Compare(float _a, float _b)
			{
				return (_a < _b) ? 1 : ((_a > _b) ? (-1) : 0);
			}
		}

		private const string LoadAnimationManagerPrefab = "UI/Animus/Widgets/LoadAnimationManager";

		[SerializeField]
		private List<LoadAnimationDefinition> m_prefabDefs = new List<LoadAnimationDefinition>();

		[SerializeField]
		[HideInInspector]
		private bool m_isOverridden;

		private static SortedDictionary<float, List<PoolObject>> m_pools;

		private static BBList<LoadAnimationManager> m_managers = new BBList<LoadAnimationManager>();

		private readonly List<LoadAnimWrapper> m_loadAnims = new List<LoadAnimWrapper>();

		private SortedDictionary<float, List<PoolObject>> m_overriddenPools;

		private SortedDictionary<float, List<PoolObject>> Pools
		{
			get
			{
				if (m_isOverridden)
				{
					return m_overriddenPools = m_overriddenPools ?? new SortedDictionary<float, List<PoolObject>>(new DescendingFloatComparer());
				}
				SortedDictionary<float, List<PoolObject>> obj = m_pools ?? new SortedDictionary<float, List<PoolObject>>(new DescendingFloatComparer());
				m_pools = obj;
				return obj;
			}
		}

		public bool IsLoading
		{
			get
			{
				return m_loadAnims.Count > 0;
			}
		}

		public static LoadAnimationManager CreateNewInstance(Transform _parent)
		{
			LoadAnimationManager loadAnimationManager = Object.Instantiate(Resources.Load("UI/Animus/Widgets/LoadAnimationManager", typeof(LoadAnimationManager))) as LoadAnimationManager;
			if (loadAnimationManager != null && _parent != null)
			{
				loadAnimationManager.transform.parent = _parent;
			}
			return loadAnimationManager;
		}

		public bool AddLoadAnim(SLoadObjectInfo _info, int _delay = 1)
		{
			if (_info.LoadObject != null && Find(_info) == null)
			{
				LoadAnimWrapper loadAnimWrapper = new LoadAnimWrapper(_info);
				LoadAnimationDefinition def = Unpool(loadAnimWrapper.Size);
				loadAnimWrapper.Set(def);
				loadAnimWrapper.Show(_delay);
				m_loadAnims.Add(loadAnimWrapper);
				return true;
			}
			return false;
		}

		public void RemoveLoadAnim(SLoadObjectInfo _info)
		{
			LoadAnimWrapper loadAnimWrapper = Find(_info);
			if (loadAnimWrapper != null)
			{
				Pool(loadAnimWrapper);
				loadAnimWrapper.Hide();
				m_loadAnims.Remove(loadAnimWrapper);
			}
		}

		private void Awake()
		{
			m_managers.Add(this);
			m_prefabDefs.Sort(new LoadAnimationDefinition.DescendingOrder());
			for (int i = 0; i < m_prefabDefs.Count; i++)
			{
				LoadAnimationDefinition loadAnimationDefinition = m_prefabDefs[i];
				if (!Pools.ContainsKey(loadAnimationDefinition.ObjectMaxSize))
				{
					Pools.Add(loadAnimationDefinition.ObjectMaxSize, new List<PoolObject>());
				}
			}
		}

		private void OnDestroy()
		{
			m_managers.Remove(this);
			for (int num = m_loadAnims.Count - 1; num >= 0; num--)
			{
				RemoveLoadAnim(m_loadAnims[num].Info);
			}
			m_loadAnims.Clear();
			if (m_pools != null && m_managers.Count <= 0)
			{
				m_pools.Clear();
			}
		}

		private void Update()
		{
			float deltaTime = RealTime.deltaTime;
			for (int i = 0; i < m_loadAnims.Count; i++)
			{
				m_loadAnims[i].Update(deltaTime);
			}
		}

		private LoadAnimWrapper Find(SLoadObjectInfo _info)
		{
			for (int i = 0; i < m_loadAnims.Count; i++)
			{
				LoadAnimWrapper loadAnimWrapper = m_loadAnims[i];
				if (loadAnimWrapper.Info == _info)
				{
					return loadAnimWrapper;
				}
			}
			return null;
		}

		private void Pool(LoadAnimWrapper _wrapper)
		{
			List<PoolObject> poolFromSize = GetPoolFromSize(_wrapper.Size);
			for (int i = 0; i < poolFromSize.Count; i++)
			{
				PoolObject poolObject = poolFromSize[i];
				if (!poolObject.IsPooled && poolObject.Def == _wrapper.LoadAnimDefinition)
				{
					poolObject.IsPooled = true;
					break;
				}
			}
		}

		private LoadAnimationDefinition Unpool(float _size)
		{
			List<PoolObject> poolFromSize = GetPoolFromSize(_size);
			if (poolFromSize != null)
			{
				for (int i = 0; i < poolFromSize.Count; i++)
				{
					PoolObject poolObject = poolFromSize[i];
					if (poolObject.IsPooled)
					{
						poolObject.IsPooled = false;
						return poolObject.Def;
					}
				}
				return CreatePoolObj(_size);
			}
			return null;
		}

		private List<PoolObject> GetPoolFromSize(float _size)
		{
			foreach (KeyValuePair<float, List<PoolObject>> pool in Pools)
			{
				if (pool.Key < _size)
				{
					return pool.Value;
				}
			}
			return null;
		}

		private LoadAnimationDefinition CreatePoolObj(float _size)
		{
			for (int i = 0; i < m_prefabDefs.Count; i++)
			{
				LoadAnimationDefinition loadAnimationDefinition = m_prefabDefs[i];
				if (loadAnimationDefinition.ObjectMaxSize < _size)
				{
					UIWidget @object = Object.Instantiate(loadAnimationDefinition.LoadAnimPrefab);
					LoadAnimationDefinition loadAnimationDefinition2 = new LoadAnimationDefinition(@object, loadAnimationDefinition.ObjectMaxSize);
					if (!Pools.ContainsKey(loadAnimationDefinition2.ObjectMaxSize))
					{
						Pools.Add(loadAnimationDefinition2.ObjectMaxSize, new List<PoolObject>());
					}
					Pools[loadAnimationDefinition2.ObjectMaxSize].Add(new PoolObject(loadAnimationDefinition2));
					return loadAnimationDefinition2;
				}
			}
			return null;
		}

		public IEnumerator BeginLoadUnityResource(UnityResource _resource, SLoadObjectInfo _info)
		{
			AddLoadAnim(_info);
			yield return Main.Instance.StartCoroutine(_resource.PrepareAsync());
			RemoveLoadAnim(_info);
		}
	}
}
