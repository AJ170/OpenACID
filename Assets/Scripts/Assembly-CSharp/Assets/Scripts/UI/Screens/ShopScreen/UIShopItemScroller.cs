using System.Collections.Generic;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.TweenHandling;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopItemScroller : MonoBehaviour
	{
		public delegate void UIShopBaseItemCallback(UIShopBaseItem _item);

		[SerializeField]
		private UIExtendedWrapContent m_itemWrapContainer;

		[SerializeField]
		private UIShopScrollers m_parent;

		[SerializeField]
		private UIRect m_scrollArea;

		[SerializeField]
		private GameObject m_overlayContainer;

		[SerializeField]
		private GameObject m_overlayItem1X2;

		[SerializeField]
		private GameObject m_overlayAdPrefab;

		[SerializeField]
		private UIHeroicRankUnlock m_heroicRankUnlockOverlay;

		private GameObjectCache m_overlayItems1X2Cache;

		private GameObjectCache m_overlayBannerItems1X2Cache;

		private ShopCategoryNode m_currentDataNode;

		public UIExtendedWrapContent ItemWrapContainer
		{
			get
			{
				return m_itemWrapContainer;
			}
		}

		public UIHeroicRankUnlock HeroicRankUnlockOverlay
		{
			get
			{
				return m_heroicRankUnlockOverlay;
			}
		}

		public event UIShopBaseItemCallback OnItemWrapped;

		public void Init()
		{
			m_itemWrapContainer.OnUpdateItem += OnUpdateItemWrap;
			m_itemWrapContainer.OnItemCreated += OnItemWrapCreated;
			m_itemWrapContainer.Init();
			m_overlayItems1X2Cache = new GameObjectCache(m_overlayItem1X2, m_overlayContainer.transform);
			m_overlayItems1X2Cache.OnNewItemCreated += new SimpleDelegate.CallbackFunction(OnOverlayCreated);
			m_overlayItems1X2Cache.Reserve(2);
			m_overlayBannerItems1X2Cache = new GameObjectCache(m_overlayAdPrefab, m_overlayContainer.transform);
			m_overlayBannerItems1X2Cache.Reserve(1);
		}

		public void OnForceUpdateLayout(object[] _params = null)
		{
			for (int i = 0; i < m_itemWrapContainer.Children.Count; i++)
			{
				UIShopItemViewController component = m_itemWrapContainer.Children[i].GetComponent<UIShopItemViewController>();
				component.UpdateData();
			}
		}

		public void UpdateData(ShopCategoryNode _currentCategoryContainer)
		{
			m_currentDataNode = _currentCategoryContainer;
			base.gameObject.SetActive(true);
			m_itemWrapContainer.ItemCount = _currentCategoryContainer.ItemCount;
			m_itemWrapContainer.WrapScrollView.ResetPosition();
		}

		public UIShopBaseItem GetShopBaseItemById(string _id)
		{
			UIShopBaseItem result = null;
			List<Transform> sortedChildrenByPositionList = m_itemWrapContainer.SortedChildrenByPositionList;
			for (int i = 0; i < sortedChildrenByPositionList.Count; i++)
			{
				UIShopItemViewController component = sortedChildrenByPositionList[i].GetComponent<UIShopItemViewController>();
				UIShopBaseItem currentActiveView = component.CurrentActiveView;
				ShopItemNode shopItemNode = currentActiveView.CurrentShopBaseNode as ShopItemNode;
				if (shopItemNode != null && string.Compare(shopItemNode.ShopBundle.id, _id, true) == 0)
				{
					result = currentActiveView;
					break;
				}
			}
			return result;
		}

		public UIShopBaseItem GetShopBaseItemByHustonId(string _houstonId)
		{
			UIShopBaseItem result = null;
			List<Transform> sortedChildrenByPositionList = m_itemWrapContainer.SortedChildrenByPositionList;
			for (int i = 0; i < sortedChildrenByPositionList.Count; i++)
			{
				UIShopItemViewController component = sortedChildrenByPositionList[i].GetComponent<UIShopItemViewController>();
				UIShopBaseItem currentActiveView = component.CurrentActiveView;
				ShopItemNode shopItemNode = currentActiveView.CurrentShopBaseNode as ShopItemNode;
				if (shopItemNode != null && string.Compare(shopItemNode.ShopBundle.houstonId, _houstonId, true) == 0)
				{
					result = currentActiveView;
					break;
				}
			}
			return result;
		}

		public void AddChildrenToTweener(TweenHandler _handler)
		{
			List<Transform> sortedChildrenByPositionList = m_itemWrapContainer.SortedChildrenByPositionList;
			for (int i = 0; i < sortedChildrenByPositionList.Count; i++)
			{
				UIShopItemViewController component = sortedChildrenByPositionList[i].GetComponent<UIShopItemViewController>();
				if (component.IsValid)
				{
					BBTargetedTween[] enterScreenTweens = component.CurrentActiveView.EnterScreenTweens;
					for (int j = 0; j < enterScreenTweens.Length; j++)
					{
						enterScreenTweens[j].StartOrder = i + 1;
					}
					_handler.AddTweens(enterScreenTweens, 1, false);
				}
			}
		}

		public void SetFocusOnChildByIndex(int _index)
		{
			m_itemWrapContainer.SetFocusOnChildByIndex(_index);
		}

		public void ResetScroller()
		{
			m_itemWrapContainer.WrapScrollView.ResetPosition();
		}

		private void OnDestroy()
		{
			m_currentDataNode = null;
			DestroyWraperContainer(m_itemWrapContainer);
			if (m_overlayItems1X2Cache != null)
			{
				m_overlayItems1X2Cache.OnNewItemCreated -= new SimpleDelegate.CallbackFunction(OnOverlayCreated);
				m_overlayItems1X2Cache.ReleaseAll(delegate(GameObject _obj)
				{
					if (_obj != null && m_parent != null)
					{
						UIShopBaseItem component = _obj.GetComponent<UIShopBaseItem>();
						if (component != null)
						{
							EventDelegate.Remove(component.OnSelected, m_parent.OnItemSelected);
						}
					}
				});
				m_overlayItems1X2Cache.Destroy();
			}
			if (m_overlayBannerItems1X2Cache != null)
			{
				m_overlayBannerItems1X2Cache.ReleaseAll();
				m_overlayBannerItems1X2Cache.Destroy();
			}
		}

		private void DestroyWraperContainer(UIExtendedWrapContent _container)
		{
			if (!(_container != null))
			{
				return;
			}
			_container.OnUpdateItem -= OnUpdateItemWrap;
			_container.OnItemCreated -= OnItemWrapCreated;
			if (!(m_parent != null) || _container.Children == null)
			{
				return;
			}
			for (int i = 0; i < _container.Children.Count; i++)
			{
				Transform transform = _container.Children[i];
				if (transform != null)
				{
					UIShopBaseItem component = transform.GetComponent<UIShopBaseItem>();
					if (component != null)
					{
						EventDelegate.Remove(component.OnSelected, m_parent.OnItemSelected);
					}
				}
			}
		}

		private GameObjectCache GetGameObjectCacheByShopBaseNode(ShopBaseNode _baseNode)
		{
			GameObjectCache result = m_overlayItems1X2Cache;
			if (_baseNode != null)
			{
				EShopDataType dataType = _baseNode.DataType;
				if (dataType == EShopDataType.HappyHourBanner)
				{
					result = m_overlayBannerItems1X2Cache;
				}
			}
			return result;
		}

		private void OnUpdateItemWrap(Transform _transform, int _index)
		{
			if (m_currentDataNode != null)
			{
				ShopBaseNode itemByIndex = m_currentDataNode.GetItemByIndex(_index);
				UIShopItemViewController component = _transform.GetComponent<UIShopItemViewController>();
				component.UpdateData(itemByIndex, m_scrollArea, GetGameObjectCacheByShopBaseNode(itemByIndex));
				if (this.OnItemWrapped != null)
				{
					this.OnItemWrapped(component.CurrentActiveView);
				}
			}
		}

		private void OnOverlayCreated(object[] _params)
		{
			GameObject gameObject = (GameObject)_params[0];
			OnItemWrapCreated(gameObject.transform);
		}

		private void OnItemWrapCreated(Transform _transform)
		{
			UIShopBaseItem component = _transform.GetComponent<UIShopBaseItem>();
			if (component != null)
			{
				EventDelegate.Add(component.OnSelected, m_parent.OnItemSelected);
				UIShopItemDetails component2 = _transform.GetComponent<UIShopItemDetails>();
				if (component2 != null)
				{
					component2.ShopItemsScrollParent = this;
				}
			}
		}
	}
}
