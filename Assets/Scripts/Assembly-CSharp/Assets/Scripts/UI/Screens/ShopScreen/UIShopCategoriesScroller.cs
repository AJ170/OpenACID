using System;
using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.TweenHandling;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopCategoriesScroller : MonoBehaviour
	{
		private const int c_preCreateCategoryCount = 5;

		[SerializeField]
		private GameObject m_categoryItemTemplet;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private UIExtendedTable m_itemsTable;

		[SerializeField]
		private UITable m_mainTable;

		[SerializeField]
		private UIShopScrollers m_parent;

		[SerializeField]
		private TweenHandler m_tweenHandler;

		[SerializeField]
		private UIFocusChildInScrollView m_focusChildInScrollView;

		[SerializeField]
		private UIShopAdvertisements m_advertisements;

		private int m_waitingForLayoutCounter;

		private int m_itemIndexToFocusAfterLayoutUpdate = -1;

		private ShopCategoryNode m_currentDataNode;

		private readonly List<UIShopBaseItem> m_itemCreated = new List<UIShopBaseItem>();

		private readonly List<UIShopBaseItem> m_fakeItems = new List<UIShopBaseItem>();

		private FakeCategoryNode m_adColonyNode;

		public List<UIShopBaseItem> Items
		{
			get
			{
				return m_itemCreated;
			}
		}

		private bool IsWaitingForLayout
		{
			get
			{
				return m_waitingForLayoutCounter > 0;
			}
		}

		private void Awake()
		{
			m_itemsTable.OnSortTable += OnSortTable;
		}

		public void Init()
		{
			CreateShopCategories(5);
			m_advertisements.Init();
			m_adColonyNode = new FakeCategoryNode();
			UIPanel component = m_scrollView.GetComponent<UIPanel>();
			if (component != null)
			{
				component.onGeometryUpdated = (UIPanel.OnGeometryUpdated)Delegate.Combine(component.onGeometryUpdated, new UIPanel.OnGeometryUpdated(OnGeometryUpdated));
			}
		}

		public void UpdateShopCategories(ShopCategoryNode _currentCategoryContainer)
		{
			m_advertisements.UpdateAdvertisments();
			base.gameObject.SetActive(true);
			m_currentDataNode = _currentCategoryContainer;
			if (m_currentDataNode.IsOnlyContainingItems || m_currentDataNode.ItemCount == 0)
			{
				return;
			}
			CreateShopCategories(m_currentDataNode.ItemCount - m_itemCreated.Count);
			int num = 0;
			for (int i = 0; i < m_itemCreated.Count; i++)
			{
				bool flag = num < m_currentDataNode.ItemCount;
				UIShopBaseItem uIShopBaseItem = m_itemCreated[i];
				if (flag)
				{
					ShopBaseNode itemByIndex = m_currentDataNode.GetItemByIndex(num);
					UpdateShopItem(uIShopBaseItem, itemByIndex);
					num++;
				}
				uIShopBaseItem.gameObject.SetActive(flag);
			}
			if (AdColonyManager.IsAvailable)
			{
				CreateShopCategories(1 - m_fakeItems.Count, true);
				for (int j = 0; j < m_fakeItems.Count; j++)
				{
					UpdateFakeItem(m_fakeItems[j], m_adColonyNode);
				}
			}
			m_waitingForLayoutCounter++;
			m_advertisements.UpdateLayout(OnRepositionItemTable);
			m_waitingForLayoutCounter++;
			m_itemsTable.Children.Clear();
			UIExtendedTable itemsTable = m_itemsTable;
			itemsTable.onReposition = (UITable.OnReposition)Delegate.Combine(itemsTable.onReposition, new UITable.OnReposition(OnRepositionItemTable));
			m_itemsTable.repositionNow = true;
		}

		public void AddEnterScreenTweens(TweenHandler _handler)
		{
			m_tweenHandler.ResetToBeginning();
			m_tweenHandler.RemoveAllTweendReferences();
			int num = m_advertisements.AddEnterScreenTweensForLargeAdvertisements(_handler);
			for (int i = 0; i < m_itemsTable.Children.Count; i++)
			{
				UIShopBaseItem component = m_itemsTable.Children[i].GetComponent<UIShopBaseItem>();
				BBTargetedTween[] enterScreenTweens = component.EnterScreenTweens;
				for (int j = 0; j < enterScreenTweens.Length; j++)
				{
					enterScreenTweens[j].StartOrder = num + i + 1;
				}
				_handler.AddTweens(enterScreenTweens, 1, false);
			}
		}

		public void StartTweenForNewCategorySelected(UIShopBaseItem _selectedCategoryDetails)
		{
			m_tweenHandler.RemoveAllTweendReferences();
			m_advertisements.AddNewCategorySelectedForLargeAdvertisements(_selectedCategoryDetails, m_tweenHandler);
			for (int i = 0; i < m_itemsTable.Children.Count; i++)
			{
				UIShopBaseItem component = m_itemsTable.Children[i].GetComponent<UIShopBaseItem>();
				BBTargetedTween[] array = null;
				if (component.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Category || component.ShopDataNodeType == UIShopBaseItem.EShopNodeType.AdColony)
				{
					UIShopCategoryDetails uIShopCategoryDetails = component as UIShopCategoryDetails;
					array = ((!(_selectedCategoryDetails == uIShopCategoryDetails)) ? uIShopCategoryDetails.FadeOutItemTweens : uIShopCategoryDetails.SelectItemTweens);
				}
				else
				{
					if (component.ShopDataNodeType != UIShopBaseItem.EShopNodeType.Advertisement)
					{
						continue;
					}
					UIShopAdvertisementDetails uIShopAdvertisementDetails = component as UIShopAdvertisementDetails;
					array = ((!(_selectedCategoryDetails == uIShopAdvertisementDetails)) ? uIShopAdvertisementDetails.FadeOutTweens : uIShopAdvertisementDetails.SelectTweens);
				}
				m_tweenHandler.AddTweens(array, 0, false);
				component.ActivateColliders(false);
			}
			EventDelegate.Add(m_tweenHandler.OnTweeningFinished, OnChangeCategoryTweensFinished, true);
			m_tweenHandler.StartTimeDelay = 0.1f;
			m_tweenHandler.SortListByGroupId();
			m_tweenHandler.StartTweeningForward();
		}

		public void ClearCategoryChangeCallback()
		{
			if (m_tweenHandler != null)
			{
				EventDelegate.Remove(m_tweenHandler.OnTweeningFinished, OnChangeCategoryTweensFinished);
			}
		}

		public void SetFocusOnChildByIndex(int _index)
		{
			if (IsWaitingForLayout)
			{
				m_itemIndexToFocusAfterLayoutUpdate = _index;
				return;
			}
			m_itemIndexToFocusAfterLayoutUpdate = -1;
			int activatedItemsCount = m_advertisements.ActivatedItemsCount;
			if (activatedItemsCount > 0 && _index == 0)
			{
				m_focusChildInScrollView.FocusedObject = m_advertisements.GetFirstAd().transform;
				return;
			}
			_index -= activatedItemsCount;
			if (_index >= 0 && _index < m_itemCreated.Count)
			{
				m_focusChildInScrollView.FocusedObject = m_itemCreated[_index].transform;
			}
		}

		public void ResetScroller()
		{
			m_scrollView.ResetPosition();
		}

		public void OnScreenDestroy()
		{
			if (m_scrollView != null && m_scrollView.panel != null)
			{
				UIPanel panel = m_scrollView.panel;
				panel.onGeometryUpdated = (UIPanel.OnGeometryUpdated)Delegate.Remove(panel.onGeometryUpdated, new UIPanel.OnGeometryUpdated(OnGeometryUpdated));
			}
		}

		private void OnGeometryUpdated()
		{
			m_scrollView.deactivateDrag = !m_scrollView.shouldMoveHorizontally;
		}

		private void OnDisable()
		{
			ClearCategoryChangeCallback();
		}

		private void OnDestroy()
		{
			m_itemsTable.OnSortTable -= OnSortTable;
			if (m_tweenHandler != null)
			{
				m_tweenHandler.RemoveAllTweendReferences();
			}
			m_currentDataNode = null;
			for (int i = 0; i < m_itemCreated.Count; i++)
			{
				UIShopBaseItem uIShopBaseItem = m_itemCreated[i];
				if (uIShopBaseItem != null && m_parent != null)
				{
					EventDelegate.Remove(uIShopBaseItem.OnSelected, m_parent.OnItemSelected);
				}
				NGUITools.Destroy(uIShopBaseItem);
			}
			for (int j = 0; j < m_fakeItems.Count; j++)
			{
				UIShopBaseItem uIShopBaseItem2 = m_fakeItems[j];
				if (uIShopBaseItem2 != null && m_parent != null)
				{
					EventDelegate.Remove(uIShopBaseItem2.OnSelected, m_parent.OnItemSelected);
				}
				NGUITools.Destroy(uIShopBaseItem2);
			}
			m_itemCreated.Clear();
			m_fakeItems.Clear();
		}

		private static int SortCategories(Transform _a, Transform _b)
		{
			UIShopBaseItem component = _a.gameObject.GetComponent<UIShopBaseItem>();
			UIShopBaseItem component2 = _b.gameObject.GetComponent<UIShopBaseItem>();
			if (component.ShopDataNodeType != component2.ShopDataNodeType && component.ShopDataNodeType != UIShopBaseItem.EShopNodeType.AdColony && component2.ShopDataNodeType != UIShopBaseItem.EShopNodeType.AdColony)
			{
				return ((int)component2.ShopDataNodeType).CompareTo((int)component.ShopDataNodeType);
			}
			if (component.CurrentShopBaseNode != null && component2.CurrentShopBaseNode != null)
			{
				return component.CurrentShopBaseNode.SortID.CompareTo(component2.CurrentShopBaseNode.SortID);
			}
			return 0;
		}

		private void OnSortTable(List<Transform> _list)
		{
			_list.Sort(SortCategories);
		}

		private void OnChangeCategoryTweensFinished()
		{
			m_parent.SwitchToNextCategory();
			base.gameObject.SetActive(false);
		}

		private void OnRepositionItemTable()
		{
			m_waitingForLayoutCounter--;
			if (!IsWaitingForLayout)
			{
				m_waitingForLayoutCounter = 1;
				UIExtendedTable itemsTable = m_itemsTable;
				itemsTable.onReposition = (UITable.OnReposition)Delegate.Remove(itemsTable.onReposition, new UITable.OnReposition(OnRepositionItemTable));
				UITable mainTable = m_mainTable;
				mainTable.onReposition = (UITable.OnReposition)Delegate.Combine(mainTable.onReposition, new UITable.OnReposition(OnRepositionmMainTable));
				m_mainTable.repositionNow = true;
			}
		}

		private void OnRepositionmMainTable()
		{
			UITable mainTable = m_mainTable;
			mainTable.onReposition = (UITable.OnReposition)Delegate.Remove(mainTable.onReposition, new UITable.OnReposition(OnRepositionmMainTable));
			ResetScroller();
			m_waitingForLayoutCounter = 0;
			m_scrollView.deactivateDrag = !m_scrollView.shouldMoveHorizontally;
			if (m_itemIndexToFocusAfterLayoutUpdate >= 0)
			{
				SetFocusOnChildByIndex(m_itemIndexToFocusAfterLayoutUpdate);
			}
		}

		private void CreateShopCategories(int _newItemCount, bool _fakeItem = false)
		{
			if (!(m_categoryItemTemplet != null) || _newItemCount <= 0)
			{
				return;
			}
			for (int i = 0; i < _newItemCount; i++)
			{
				GameObject gameObject = NGUITools.AddChild(m_itemsTable.gameObject, m_categoryItemTemplet);
				UIShopBaseItem component = gameObject.GetComponent<UIShopBaseItem>();
				EventDelegate.Add(component.OnSelected, m_parent.OnItemSelected);
				if (_fakeItem)
				{
					m_fakeItems.Add(component);
				}
				else
				{
					m_itemCreated.Add(component);
				}
			}
		}

		private void UpdateShopItem(UIShopBaseItem _shopItem, ShopBaseNode _data)
		{
			_shopItem.ResetScale();
			_shopItem.UpdateData(_data);
		}

		private void UpdateFakeItem(UIShopBaseItem _shopItem, FakeCategoryNode _data)
		{
			_shopItem.ResetScale();
			_shopItem.UpdateData(_data);
		}
	}
}
