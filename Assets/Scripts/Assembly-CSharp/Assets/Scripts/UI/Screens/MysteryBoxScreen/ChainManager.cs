using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MysteryBoxScreen
{
	public class ChainManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_itemPrefab;

		[SerializeField]
		private UIAlignmentBox m_alignmentBox;

		[SerializeField]
		private UIInfoPopup m_infoPopup;

		[SerializeField]
		private GameObject m_startTarget;

		public SimpleDelegate OnAllAnimationsDone;

		private List<GameObjectCache.GameObjectCacheEntry> m_containerObjects = new List<GameObjectCache.GameObjectCacheEntry>();

		private List<MysteryBoxItemContainer> m_itemContainers = new List<MysteryBoxItemContainer>();

		private MysteryBoxItemContainer m_lastSelected;

		private int m_chainLenght;

		private Assets.Scripts.UI.Screens.MysteryBox.MysteryBoxData m_data;

		private GameObjectCache m_gameObjectCache;

		public SimpleDelegate ItemContainersAdded;

		private ChainManager()
		{
			ItemContainersAdded = new SimpleDelegate(this, new Type[1] { typeof(List<MysteryBoxItemContainer>) });
			OnAllAnimationsDone = new SimpleDelegate(this, new Type[0]);
		}

		public SimpleDelegate GetChainDataClaimedEvent()
		{
			if (m_data != null)
			{
				return m_data.OnClaimedChain;
			}
			return null;
		}

		private void Awake()
		{
			m_infoPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
		}

		public void Setup(Assets.Scripts.UI.Screens.MysteryBox.MysteryBoxData _data, UIMysteryBoxBehaviour _behaviour)
		{
			m_data = _data;
			m_data.OnClaimedChain += new SimpleDelegate.CallbackFunction(ClosePopup);
			m_data.OnLootedAgain += new SimpleDelegate.CallbackFunction(ClosePopup);
			m_chainLenght = _data.OverallChainLenght;
			if (m_gameObjectCache == null)
			{
				m_gameObjectCache = new GameObjectCache(m_itemPrefab, base.transform);
			}
			m_gameObjectCache.Reserve(m_chainLenght);
			ReleaseItems();
			m_itemContainers.Clear();
			for (int i = 0; i < m_chainLenght; i++)
			{
				GameObjectCache.GameObjectCacheEntry gameObjectCacheEntry = m_gameObjectCache.Dequeue();
				gameObjectCacheEntry.Item.name = i.ToString("D3");
				m_containerObjects.Add(gameObjectCacheEntry);
				m_itemContainers.Add(gameObjectCacheEntry.Item.GetComponent<MysteryBoxItemContainer>());
			}
			ItemContainersAdded.Trigger(m_itemContainers);
			for (int j = 0; j < m_itemContainers.Count; j++)
			{
				MysteryBoxItemContainer next = null;
				if (j < m_itemContainers.Count - 1)
				{
					next = m_itemContainers[j + 1];
				}
				m_itemContainers[j].Setup(_data, j, j == m_chainLenght - 1, m_startTarget, _behaviour, next);
				m_itemContainers[j].ClickDelegate += new SimpleDelegate.CallbackFunction(OnItemClick);
			}
			m_alignmentBox.RepositionNow = true;
		}

		private void ReleaseItems()
		{
			for (int i = 0; i < m_containerObjects.Count; i++)
			{
				m_containerObjects[i].Item.GetComponent<MysteryBoxItemContainer>().ClickDelegate -= new SimpleDelegate.CallbackFunction(OnItemClick);
				m_gameObjectCache.Release(m_containerObjects[i]);
			}
			m_containerObjects.Clear();
		}

		private void ClearScreen()
		{
			ReleaseItems();
			m_data.OnClaimedChain -= new SimpleDelegate.CallbackFunction(ClosePopup);
			m_data.OnLootedAgain -= new SimpleDelegate.CallbackFunction(ClosePopup);
		}

		public bool AnimationsActive()
		{
			for (int i = 0; i < m_itemContainers.Count; i++)
			{
				if (m_itemContainers[i].AnimationRunning)
				{
					return true;
				}
			}
			return false;
		}

		public void CheckForRunningAnimations()
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(CheckForRunningAnimationsCoroutine());
			}
		}

		private IEnumerator CheckForRunningAnimationsCoroutine()
		{
			yield return new WaitForEndOfFrame();
			bool canBreak = false;
			while (!canBreak)
			{
				for (int i = 0; i < m_itemContainers.Count; i++)
				{
					if (m_itemContainers[i].AnimationRunning)
					{
						i = 0;
					}
					yield return new WaitForEndOfFrame();
				}
				canBreak = true;
			}
			OnAllAnimationsDone.Trigger();
		}

		private void OnItemClick(params object[] _objects)
		{
			MysteryBoxItemContainer mysteryBoxItemContainer = (MysteryBoxItemContainer)_objects[0];
			if (m_lastSelected != null && m_lastSelected != mysteryBoxItemContainer)
			{
				m_lastSelected.Selected = false;
			}
			mysteryBoxItemContainer.Selected = !mysteryBoxItemContainer.Selected;
			if (m_lastSelected == mysteryBoxItemContainer)
			{
				m_infoPopup.Close();
				m_lastSelected = null;
				return;
			}
			m_lastSelected = mysteryBoxItemContainer;
			if (mysteryBoxItemContainer.CarriedElement == null)
			{
				return;
			}
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (!mysteryBoxItemContainer.IsRewardItem)
			{
				ItemInfoPopupHelper.ShowInfoPopup(m_infoPopup, currentAssassin, ItemInfoPopupHelper.EMode.None, mysteryBoxItemContainer.CarriedElement, null, mysteryBoxItemContainer.transform, null, true, 0, string.Empty);
				m_infoPopup.MovePopupToTarget(mysteryBoxItemContainer.gameObject);
				return;
			}
			MysteryBoxItemContainer mysteryBoxItemContainer2 = m_itemContainers[m_itemContainers.Count - 2];
			if (mysteryBoxItemContainer2.CarriedResource != null || mysteryBoxItemContainer2.CarriedElement != null)
			{
				ItemInfoPopupHelper.ShowInfoPopup(m_infoPopup, currentAssassin, ItemInfoPopupHelper.EMode.None, mysteryBoxItemContainer.CarriedElement, null, mysteryBoxItemContainer.transform, null, true, 0, string.Empty);
				m_infoPopup.MovePopupToTarget(mysteryBoxItemContainer.gameObject);
			}
		}

		private void OnBeforePopupCloseCallback()
		{
			if (m_lastSelected != null)
			{
				m_lastSelected.Selected = false;
				m_lastSelected = null;
			}
		}

		private void ClosePopup(params object[] _objects)
		{
			m_infoPopup.Close();
		}

		private void OnDestroy()
		{
			m_infoPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
			ClearScreen();
		}
	}
}
