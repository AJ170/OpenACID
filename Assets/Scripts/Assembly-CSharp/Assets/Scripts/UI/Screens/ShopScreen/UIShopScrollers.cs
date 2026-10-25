using System;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.TweenHandling;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopScrollers : MonoBehaviour
	{
		[SerializeField]
		private UIShopCategoriesScroller m_categoryScroller;

		[SerializeField]
		private UIShopItemScroller m_itemScroller;

		[SerializeField]
		private UIShopDetails m_shopDetails;

		[SerializeField]
		private TweenHandler m_changeLayoutTweenHandler;

		public Action<UIShopBaseItem, UIShopBaseItem> OnItemSelectedCallback;

		private float m_categoryTweenStartTimeDelay;

		private UIShopBaseItem m_lastSelectedShopItem;

		private ShopCategoryNode m_currentDataNode;

		public UIShopCategoriesScroller CategoryScroller
		{
			get
			{
				return m_categoryScroller;
			}
		}

		public UIShopItemScroller ItemScroller
		{
			get
			{
				return m_itemScroller;
			}
		}

		public void Init()
		{
			m_itemScroller.Init();
			m_categoryScroller.Init();
		}

		public void HideScrollers()
		{
			m_categoryScroller.gameObject.SetActive(false);
			m_itemScroller.gameObject.SetActive(false);
			m_currentDataNode = null;
			m_lastSelectedShopItem = null;
			SelectionChanged(null);
		}

		public void UpdateShopLayout(ShopCategoryNode _currentCategoryContainer, bool _switchToItemScrollerInstant)
		{
			m_currentDataNode = _currentCategoryContainer;
			m_lastSelectedShopItem = null;
			SelectionChanged(null);
			m_changeLayoutTweenHandler.RemoveAllTweendReferences();
			m_changeLayoutTweenHandler.StartTimeDelay = 0.05f + m_categoryTweenStartTimeDelay;
			m_categoryTweenStartTimeDelay = 0f;
			if (m_currentDataNode.IsOnlyContainingItems)
			{
				if (_switchToItemScrollerInstant)
				{
					m_categoryScroller.gameObject.SetActive(false);
				}
				m_itemScroller.UpdateData(_currentCategoryContainer);
				m_itemScroller.AddChildrenToTweener(m_changeLayoutTweenHandler);
			}
			else
			{
				m_itemScroller.gameObject.SetActive(false);
				m_categoryScroller.UpdateShopCategories(_currentCategoryContainer);
				m_categoryScroller.AddEnterScreenTweens(m_changeLayoutTweenHandler);
			}
			m_changeLayoutTweenHandler.StartTweeningForward();
		}

		public void SetFocusOnChildByIndex(int _index)
		{
			if (m_currentDataNode != null && m_currentDataNode.IsOnlyContainingItems)
			{
				if (m_changeLayoutTweenHandler.IsTweening)
				{
					m_changeLayoutTweenHandler.ResetToEnd();
				}
				m_itemScroller.ItemWrapContainer.OnFocusFinishedEvent.Clear();
				m_itemScroller.ItemWrapContainer.MoveInstant = true;
				m_itemScroller.SetFocusOnChildByIndex(_index);
				m_changeLayoutTweenHandler.RemoveAllTweendReferences();
				m_itemScroller.AddChildrenToTweener(m_changeLayoutTweenHandler);
				EventDelegate.Add(m_itemScroller.ItemWrapContainer.OnFocusFinishedEvent, delegate
				{
					m_itemScroller.ItemWrapContainer.MoveInstant = false;
					m_itemScroller.SetFocusOnChildByIndex(_index);
					m_changeLayoutTweenHandler.StartTweeningForward();
					EventDelegate.Add(m_itemScroller.ItemWrapContainer.OnFocusFinishedEvent, delegate
					{
						UIShopDetails.s_flipOnFocusId = string.Empty;
					}, true);
				}, true);
				m_itemScroller.SetFocusOnChildByIndex(0);
			}
			else
			{
				m_categoryScroller.SetFocusOnChildByIndex(_index);
			}
		}

		public void OnTapBack()
		{
			ClearCallbacks();
		}

		public void ClearCallbacks()
		{
			m_categoryScroller.ClearCategoryChangeCallback();
		}

		public void ResetScroller()
		{
			if (m_currentDataNode != null && m_currentDataNode.IsOnlyContainingItems)
			{
				m_itemScroller.ResetScroller();
			}
			else
			{
				m_categoryScroller.ResetScroller();
			}
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnClosePopup();
		}

		public void SwitchToNextCategory()
		{
			m_shopDetails.SetNextToCurrentCategory();
		}

		private void OnEnable()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewCloseCallback += OnItemBundlePreviewClose;
			}
		}

		private void OnDisable()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewCloseCallback -= OnItemBundlePreviewClose;
			}
			if (m_shopDetails != null)
			{
				EventDelegate.Remove(m_shopDetails.OnDirectPurchasesItemUpdated, OnSelectCategoryVerified);
			}
		}

		private void OnDestroy()
		{
			m_currentDataNode = null;
			m_lastSelectedShopItem = null;
		}

		private void OnItemBundlePreviewClose()
		{
			if (m_lastSelectedShopItem != null)
			{
				m_lastSelectedShopItem.ResetItemContainer();
			}
		}

		public void SelectionChanged(UIShopBaseItem _selectedItem)
		{
			Action<UIShopBaseItem, UIShopBaseItem> onItemSelectedCallback = OnItemSelectedCallback;
			if (onItemSelectedCallback != null)
			{
				onItemSelectedCallback(_selectedItem, m_lastSelectedShopItem);
			}
			if (_selectedItem != null && _selectedItem.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Item)
			{
				ShopBundle shopBundle = ((UIShopItemDetails)_selectedItem).ShopBundle;
				if (shopBundle != null && shopBundle.bundleValue != 0)
				{
					TrackingManager.OpenShopBundles("Shop", shopBundle.id);
				}
				Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OpenPreviewByShopItem(shopBundle, string.Empty, string.Empty);
			}
			else
			{
				Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnClosePopup();
			}
		}

		public void OnItemSelected()
		{
			UIShopBaseItem current = UIShopBaseItem.Current;
			if (current.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Category || current.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Advertisement)
			{
				m_lastSelectedShopItem = current;
				ShopCategoryNode _nextCategory = null;
				if (current.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Advertisement)
				{
					UIShopAdvertisementDetails uIShopAdvertisementDetails = (UIShopAdvertisementDetails)current;
					ShopBaseNode _selectItem = null;
					uIShopAdvertisementDetails.GetSelectTargets(out _nextCategory, out _selectItem);
				}
				else
				{
					_nextCategory = m_lastSelectedShopItem.CurrentShopBaseNode as ShopCategoryNode;
					if (_nextCategory != null && !_nextCategory.IsUnlocked)
					{
						UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(7019));
						return;
					}
				}
				if (_nextCategory == null)
				{
					return;
				}
				if (_nextCategory.HasDirectPurchasesItem())
				{
					EventDelegate.Add(m_shopDetails.OnDirectPurchasesItemUpdated, OnSelectCategoryVerified, true);
					m_shopDetails.UpdateDirectPurchasesShopList(_nextCategory);
				}
				else
				{
					OnSelectCategoryVerified();
				}
			}
			else if (current.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Item)
			{
				if (m_lastSelectedShopItem != null && m_lastSelectedShopItem != current)
				{
					m_lastSelectedShopItem.ResetItemContainer();
				}
				m_lastSelectedShopItem = current;
			}
			else if (current.ShopDataNodeType == UIShopBaseItem.EShopNodeType.AdColony && AdColonyManager.IsAvailable)
			{
				AdColonyManager.Instance.ShowAdColonyVideoWithCallback(AdColonyManager.ERewardType.MysteryBox, true);
			}
			SelectionChanged(current);
		}

		private void OnSelectCategoryVerified()
		{
			UIShopBaseItem lastSelectedShopItem = m_lastSelectedShopItem;
			ShopCategoryNode _nextCategory = null;
			ShopBaseNode _selectItem = null;
			if (m_lastSelectedShopItem.ShopDataNodeType == UIShopBaseItem.EShopNodeType.Advertisement)
			{
				UIShopAdvertisementDetails uIShopAdvertisementDetails = m_lastSelectedShopItem as UIShopAdvertisementDetails;
				uIShopAdvertisementDetails.GetSelectTargets(out _nextCategory, out _selectItem);
			}
			else
			{
				_nextCategory = m_lastSelectedShopItem.CurrentShopBaseNode as ShopCategoryNode;
			}
			if (_nextCategory == null)
			{
				return;
			}
			m_shopDetails.NextCategoryContainer = _nextCategory;
			m_changeLayoutTweenHandler.ResetToEnd();
			m_changeLayoutTweenHandler.RemoveAllTweendReferences();
			if (_nextCategory.IsOnlyContainingItems)
			{
				m_categoryTweenStartTimeDelay = 0.2f;
				m_shopDetails.SetCurrentCategory(_nextCategory, false);
				int focusOnChildByIndex = 0;
				if (_selectItem != null)
				{
					focusOnChildByIndex = m_shopDetails.CurrentCategoryContainer.GetChildIndexByNode(_selectItem);
				}
				SetFocusOnChildByIndex(focusOnChildByIndex);
			}
			m_categoryScroller.StartTweenForNewCategorySelected(lastSelectedShopItem);
		}
	}
}
