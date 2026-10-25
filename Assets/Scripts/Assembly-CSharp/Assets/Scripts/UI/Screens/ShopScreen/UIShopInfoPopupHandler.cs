using System;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopInfoPopupHandler : MonoBehaviour
	{
		[SerializeField]
		private UIInfoPopup m_infoPopup;

		[SerializeField]
		private UIShopScrollers m_scroller;

		private UIShopItemDetails m_currentSelectedItem;

		private UIScrollView m_scrollview;

		private string m_currentSelectedItemId = string.Empty;

		private UIInfoPopup.EPlacementSide m_infoPopupPlacement = UIInfoPopup.EPlacementSide.Auto;

		private void Awake()
		{
		}

		private void Start()
		{
			UIShopScrollers scroller = m_scroller;
			scroller.OnItemSelectedCallback = (Action<UIShopBaseItem, UIShopBaseItem>)Delegate.Combine(scroller.OnItemSelectedCallback, new Action<UIShopBaseItem, UIShopBaseItem>(OnItemSelected));
			m_infoPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
			m_scrollview = m_scroller.ItemScroller.ItemWrapContainer.WrapScrollView;
			UIScrollView scrollview = m_scrollview;
			scrollview.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Combine(scrollview.onDragStarted, new UIScrollView.OnDragNotification(OnScrollViewDragStarted));
			UIScrollView scrollview2 = m_scrollview;
			scrollview2.onScrolling = (UIScrollView.OnDragNotification)Delegate.Combine(scrollview2.onScrolling, new UIScrollView.OnDragNotification(OnScrollViewDragStarted));
			m_infoPopup.OnPopupClosedCallback += OnPopupClosed;
		}

		private void OnDisable()
		{
			if (m_infoPopup != null)
			{
				m_infoPopup.Close();
			}
		}

		private void OnDestroy()
		{
			if (m_scroller != null)
			{
				UIShopScrollers scroller = m_scroller;
				scroller.OnItemSelectedCallback = (Action<UIShopBaseItem, UIShopBaseItem>)Delegate.Remove(scroller.OnItemSelectedCallback, new Action<UIShopBaseItem, UIShopBaseItem>(OnItemSelected));
			}
			if (m_scrollview != null)
			{
				UIScrollView scrollview = m_scrollview;
				scrollview.onDragStarted = (UIScrollView.OnDragNotification)Delegate.Remove(scrollview.onDragStarted, new UIScrollView.OnDragNotification(OnScrollViewDragStarted));
				UIScrollView scrollview2 = m_scrollview;
				scrollview2.onScrolling = (UIScrollView.OnDragNotification)Delegate.Remove(scrollview2.onScrolling, new UIScrollView.OnDragNotification(OnScrollViewDragStarted));
			}
			m_scrollview = null;
			m_infoPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
			m_infoPopup.OnPopupClosedCallback -= OnPopupClosed;
			m_currentSelectedItem = null;
		}

		private void OnBeforePopupCloseCallback()
		{
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.OnDisplayChanged -= new SimpleDelegate.CallbackFunction(OnCurrentItemDisplayChanged);
				m_currentSelectedItem.ResetItemContainer();
			}
		}

		private void OnPopupClosed()
		{
			m_currentSelectedItem = null;
		}

		private bool ShowShopInfoPopup(UIShopItemDetails _detail)
		{
			if (_detail == null || _detail.NextShopItemViewType != UIShopBaseItem.EShopItemViewType.ItemInfo || _detail.ShopBundle.shopPreview == null || !_detail.ShopBundle.isShopPreviewEnabled || _detail.ShopBundle.bundleValue > 0 || _detail.ShopBundle.payout.lootTemplates.Length == 0)
			{
				return false;
			}
			return true;
		}

		private void OnItemSelected(UIShopBaseItem _new, UIShopBaseItem _old)
		{
			UIShopItemDetails uIShopItemDetails = ((!(_new != null) || _new.ShopDataNodeType != UIShopBaseItem.EShopNodeType.Item) ? null : ((UIShopItemDetails)_new));
			if (uIShopItemDetails == null || !ShowShopInfoPopup(uIShopItemDetails))
			{
				m_infoPopup.Close();
				return;
			}
			m_infoPopup.UpdateByShopBundle(uIShopItemDetails.ShopBundle, uIShopItemDetails.InfoPopupArrowTarget.transform);
			if (m_currentSelectedItem != null)
			{
				m_currentSelectedItem.OnDisplayChanged -= new SimpleDelegate.CallbackFunction(OnCurrentItemDisplayChanged);
			}
			m_currentSelectedItem = uIShopItemDetails;
			m_currentSelectedItemId = uIShopItemDetails.ShopItemId;
			m_currentSelectedItem.OnDisplayChanged += new SimpleDelegate.CallbackFunction(OnCurrentItemDisplayChanged);
			m_infoPopupPlacement = m_infoPopup.MovePopupToTarget(m_currentSelectedItem.InfoPopupArrowTarget);
		}

		private void OnScrollViewDragStarted()
		{
			if (m_currentSelectedItem != null && !m_infoPopup.IsClosing)
			{
				m_infoPopup.Close();
			}
		}

		private void OnCurrentItemDisplayChanged(params object[] _params)
		{
			if ((int)_params[0] == 0)
			{
				m_infoPopup.Close();
			}
		}

		private void Update()
		{
			if (!(m_currentSelectedItem == null) && !(m_currentSelectedItem.ShopItemId != m_currentSelectedItemId))
			{
				m_infoPopup.MovePopupToTarget(m_currentSelectedItem.InfoPopupArrowTarget, m_infoPopupPlacement);
			}
		}
	}
}
