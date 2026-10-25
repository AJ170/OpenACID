using System;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers
{
	public class GameObjectCache
	{
		public class GameObjectCacheEntry
		{
			public GameObjectCache Queue { get; private set; }

			public GameObject Item { get; private set; }

			public GameObjectCacheEntry(GameObjectCache _queue, GameObject _item)
			{
				Queue = _queue;
				Item = _item;
			}
		}

		private readonly GameObject m_template;

		private readonly Transform m_parent;

		private readonly BBQueue<GameObject> m_cache = new BBQueue<GameObject>();

		private readonly BBList<GameObject> m_items = new BBList<GameObject>();

		public SimpleDelegate OnNewItemCreated;

		public GameObjectCache(GameObject _template, Transform _parent = null)
		{
			m_template = _template;
			m_parent = _parent ?? _template.transform.parent;
			OnNewItemCreated = new SimpleDelegate(m_parent, new Type[1] { typeof(GameObject) });
		}

		public GameObjectCacheEntry Dequeue()
		{
			if (m_cache.Count == 0)
			{
				CreateNewEntry();
			}
			GameObject gameObject = m_cache.Dequeue();
			gameObject.SetActive(true);
			return new GameObjectCacheEntry(this, gameObject);
		}

		public void Release(GameObject _entry)
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i] == _entry)
				{
					_entry.SetActive(false);
					m_cache.Enqueue(_entry);
					break;
				}
			}
		}

		public void Release(GameObjectCacheEntry _entry)
		{
			_entry.Item.SetActive(false);
			m_cache.Enqueue(_entry.Item);
		}

		public void ReleaseAll(Action<GameObject> _callback = null)
		{
			m_cache.Clear();
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].gameObject != null)
				{
					if (_callback != null)
					{
						_callback(m_items[i].gameObject);
					}
					m_items[i].gameObject.SetActive(false);
				}
				m_cache.Enqueue(m_items[i]);
			}
		}

		public void Reserve(int _size)
		{
			for (int i = m_items.Count; i < _size; i++)
			{
				CreateNewEntry();
			}
		}

		public void Destroy()
		{
			for (int i = 0; i < m_items.Count; i++)
			{
				GameObject gameObject = m_items[i];
				if (gameObject != null)
				{
					if (gameObject.transform != null)
					{
						gameObject.transform.parent = null;
					}
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			m_cache.Clear();
			m_items.Clear();
		}

		private void CreateNewEntry()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(m_template);
			gameObject.SetActive(false);
			gameObject.transform.parent = m_parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localRotation = Quaternion.identity;
			m_cache.Enqueue(gameObject);
			m_items.Add(gameObject);
			if (OnNewItemCreated != null)
			{
				OnNewItemCreated.Trigger(gameObject);
			}
		}
	}
}
