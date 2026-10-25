using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopItemViewController : MonoBehaviour
	{
		[SerializeField]
		private UIShopItemDetails m_normalItemDetails;

		private UIFocusChildInScrollView m_focusChildInScrollView;

		private UIShopBaseItem m_overlayItemDetails;

		private GameObjectCache.GameObjectCacheEntry m_overlayItemCacheItem;

		private GameObjectCache m_previewCache;

		private ShopBaseNode m_itemNodeData;

		public UIShopBaseItem CurrentActiveView
		{
			get
			{
				return (!(m_overlayItemDetails != null)) ? m_normalItemDetails : m_overlayItemDetails;
			}
		}

		public bool IsValid
		{
			get
			{
				return m_itemNodeData != null;
			}
		}

		private UIFocusChildInScrollView FocusChildInScrollView
		{
			get
			{
				if (m_focusChildInScrollView == null)
				{
					m_focusChildInScrollView = NGUITools.FindInParents<UIFocusChildInScrollView>(base.gameObject);
				}
				return m_focusChildInScrollView;
			}
		}

		public void UpdateData()
		{
			if (m_itemNodeData != null)
			{
				if (ShowItemAsOverlay())
				{
					m_overlayItemDetails.UpdateData(m_itemNodeData);
				}
				else
				{
					m_normalItemDetails.UpdateData(m_itemNodeData);
				}
			}
		}

		public void UpdateData(ShopBaseNode _baseNode, UIRect _anchorTarget, GameObjectCache _previewCache)
		{
			ReleasePreview();
			m_previewCache = _previewCache;
			m_itemNodeData = _baseNode;
			if (m_itemNodeData == null)
			{
				m_normalItemDetails.DisableItem();
			}
			else if (ShowItemAsOverlay())
			{
				SetupPreview(_anchorTarget);
				m_overlayItemDetails.ResetScale();
				m_overlayItemDetails.UpdateData(m_itemNodeData);
				UIFocusOnClick component = m_overlayItemDetails.GetComponent<UIFocusOnClick>();
				if (component != null)
				{
					component.FocusChildInScrollView = FocusChildInScrollView;
				}
				m_normalItemDetails.DisableItem();
			}
			else
			{
				m_normalItemDetails.ResetScale();
				m_normalItemDetails.UpdateData(m_itemNodeData);
			}
		}

		private void Awake()
		{
		}

		private void OnDisable()
		{
			ReleasePreview();
			m_previewCache = null;
			m_itemNodeData = null;
			m_focusChildInScrollView = null;
		}

		private void ReleasePreview()
		{
			if (m_overlayItemCacheItem != null)
			{
				m_previewCache.Release(m_overlayItemCacheItem);
			}
			m_overlayItemDetails = null;
			m_overlayItemCacheItem = null;
		}

		private bool ShowItemAsOverlay()
		{
			if (m_itemNodeData != null)
			{
				if (m_itemNodeData.DataType == EShopDataType.HappyHourBanner)
				{
					return true;
				}
				ShopItemNode shopItemNode = (ShopItemNode)m_itemNodeData;
				return shopItemNode.ShopBundle.HasHeroicRankPayout();
			}
			return false;
		}

		private void SetupPreview(UIRect _anchorTarget)
		{
			if (m_overlayItemDetails == null)
			{
				m_overlayItemCacheItem = m_previewCache.Dequeue();
			}
			UIWidget component = m_overlayItemCacheItem.Item.GetComponent<UIWidget>();
			component.leftAnchor.target = m_normalItemDetails.transform;
			component.rightAnchor.target = m_normalItemDetails.transform;
			component.topAnchor.target = _anchorTarget.transform;
			component.bottomAnchor.target = _anchorTarget.transform;
			component.leftAnchor.relative = 0f;
			component.rightAnchor.relative = 1f;
			component.bottomAnchor.relative = 0f;
			component.topAnchor.relative = 1f;
			component.leftAnchor.absolute = 0;
			component.rightAnchor.absolute = 0;
			component.bottomAnchor.absolute = 12;
			component.topAnchor.absolute = 0;
			component.ResetAndUpdateAnchors();
			m_overlayItemDetails = m_overlayItemCacheItem.Item.GetComponent<UIShopBaseItem>();
		}
	}
}
