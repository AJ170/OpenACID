using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Helpers;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Misc
{
	public abstract class InfoStatsListHandler : MonoBehaviour
	{
		[SerializeField]
		protected UITable m_scrollTable;

		[SerializeField]
		protected GameObject m_statItemPrefab;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		protected BoxCollider m_boxCollider;

		protected GameObjectCache m_gameObjectCache;

		protected readonly List<GameObjectCache.GameObjectCacheEntry> m_entries = new List<GameObjectCache.GameObjectCacheEntry>();

		public bool IsEnable
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		protected virtual void Awake()
		{
			m_scrollTable.sorting = UITable.Sorting.Alphabetic;
		}

		protected T GetCachedGameObject<T>(int _sortIndex) where T : MonoBehaviour
		{
			GameObjectCache.GameObjectCacheEntry gameObjectCacheEntry = m_gameObjectCache.Dequeue();
			m_entries.Add(gameObjectCacheEntry);
			GameObject item = gameObjectCacheEntry.Item;
			item.name = _sortIndex.ToString("D3");
			UIWidget component = item.GetComponent<UIWidget>();
			component.leftAnchor.Set(base.transform, 0f, 0f);
			component.rightAnchor.Set(base.transform, 1f, 0f);
			component.ForceUpdate(true);
			return item.gameObject.GetComponent<T>();
		}

		protected virtual void OnEnabled()
		{
		}

		private void OnEnable()
		{
			UITable scrollTable = m_scrollTable;
			scrollTable.onReposition = (UITable.OnReposition)Delegate.Combine(scrollTable.onReposition, new UITable.OnReposition(OnTableRepositionCallback));
			OnEnabled();
		}

		private void OnDisable()
		{
			ReleaseEntries();
			UITable scrollTable = m_scrollTable;
			scrollTable.onReposition = (UITable.OnReposition)Delegate.Remove(scrollTable.onReposition, new UITable.OnReposition(OnTableRepositionCallback));
		}

		protected void ReleaseEntries()
		{
			for (int i = 0; i < m_entries.Count; i++)
			{
				m_gameObjectCache.Release(m_entries[i]);
			}
			m_entries.Clear();
		}

		private void OnTableRepositionCallback()
		{
			m_scrollView.ResetPosition();
			m_boxCollider.enabled = true;
			StartCoroutine(FixGridLayout());
		}

		private IEnumerator FixGridLayout()
		{
			m_scrollView.panel.Invalidate(true);
			yield return new WaitForEndOfFrame();
			m_scrollView.panel.Invalidate(true);
		}
	}
}
