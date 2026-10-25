using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class ShopCategoryNode : ShopBaseNode
	{
		public StringId NameKey;

		public string ItemIconName;

		public EShopCategory Category;

		private BBList<int> m_itemEntriesIndex = new BBList<int>();

		private readonly BBList<ShopBaseNode> m_itemEntries = new BBList<ShopBaseNode>();

		public override EShopDataType DataType
		{
			get
			{
				return EShopDataType.Category;
			}
		}

		public int ItemCount
		{
			get
			{
				return ItemEntriesIndex.Count;
			}
		}

		public bool IsUnlocked
		{
			get
			{
				return true;
			}
		}

		public BBList<ShopBaseNode> RawItemEntriesList
		{
			get
			{
				return m_itemEntries;
			}
		}

		public bool IsOnlyContainingItems
		{
			get
			{
				if (m_itemEntries.Count > 0)
				{
					for (int i = 0; i < m_itemEntries.Count; i++)
					{
						if (m_itemEntries[i] != null && m_itemEntries[i].DataType == EShopDataType.Category)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		private BBList<int> ItemEntriesIndex
		{
			get
			{
				if (m_itemEntriesIndex.Count == 0)
				{
					for (int i = 0; i < m_itemEntries.Count; i++)
					{
						ShopBaseNode shopBaseNode = m_itemEntries[i];
						if (shopBaseNode == null || shopBaseNode.DataType != EShopDataType.DirectPurchasItem)
						{
							m_itemEntriesIndex.Add(i);
						}
					}
				}
				return m_itemEntriesIndex;
			}
			set
			{
				m_itemEntriesIndex = value;
			}
		}

		public void ForceUpdateIndexList()
		{
			m_itemEntriesIndex.Clear();
		}

		public bool HasDirectPurchasesItem()
		{
			for (int i = 0; i < m_itemEntries.Count; i++)
			{
				ShopItemNode shopItemNode = m_itemEntries[i] as ShopItemNode;
				if (shopItemNode != null && shopItemNode.ShopBundle != null && shopItemNode.ShopBundle.IsDirectPurchasesBundle())
				{
					return true;
				}
			}
			return false;
		}

		public bool HasOnlyDirectPurchasesItem()
		{
			for (int i = 0; i < m_itemEntries.Count; i++)
			{
				ShopItemNode shopItemNode = m_itemEntries[i] as ShopItemNode;
				if (shopItemNode == null || (shopItemNode.ShopBundle != null && !shopItemNode.ShopBundle.IsDirectPurchasesBundle()))
				{
					return false;
				}
			}
			return true;
		}

		public bool HasDiscount()
		{
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			EventCategoryPercentageManager eventCategoryPercentageManager = (EventCategoryPercentageManager)eventManager.GetEventManger(EEventType.Event_Shop_Category_Percentage);
			BBList<EventCategoryPercentage> categoryPercentageEventsByCategory = eventCategoryPercentageManager.GetCategoryPercentageEventsByCategory(Category);
			for (int i = 0; i < categoryPercentageEventsByCategory.Count; i++)
			{
				EventCategoryPercentage eventCategoryPercentage = categoryPercentageEventsByCategory[i];
				if (eventCategoryPercentage != null && eventCategoryPercentage.LastValidState)
				{
					return true;
				}
			}
			EventShopBundlePercentageManager eventShopBundlePercentageManager = (EventShopBundlePercentageManager)eventManager.GetEventManger(EEventType.Event_Shop_Item_Percentage);
			for (int j = 0; j < m_itemEntries.Count; j++)
			{
				ShopItemNode shopItemNode = m_itemEntries[j] as ShopItemNode;
				if (shopItemNode == null || shopItemNode.ShopBundle == null)
				{
					continue;
				}
				BBList<EventShopBundlePercentage> shopBundlePercentageEvents = eventShopBundlePercentageManager.GetShopBundlePercentageEvents(shopItemNode.ShopBundle.id);
				for (int k = 0; k < shopBundlePercentageEvents.Count; k++)
				{
					EventShopBundlePercentage eventShopBundlePercentage = shopBundlePercentageEvents[k];
					if (eventShopBundlePercentage != null && eventShopBundlePercentage.LastValidState)
					{
						return true;
					}
				}
			}
			return false;
		}

		public int GetChildIndexByNode(ShopBaseNode _node)
		{
			for (int i = 0; i < m_itemEntries.Count; i++)
			{
				if (m_itemEntries[i] != _node)
				{
					continue;
				}
				for (int j = 0; j < ItemEntriesIndex.Count; j++)
				{
					if (ItemEntriesIndex[j] == i)
					{
						return j;
					}
				}
				return 0;
			}
			return 0;
		}

		public int GetItemEntrieIndex(int _itemIndex)
		{
			if (ItemEntriesIndex.Count > _itemIndex)
			{
				return ItemEntriesIndex[_itemIndex];
			}
			return -1;
		}

		public ShopBaseNode GetItemByIndex(int _itemIndex)
		{
			if (ItemEntriesIndex.Count > _itemIndex)
			{
				int index = ItemEntriesIndex[_itemIndex];
				return m_itemEntries[index];
			}
			return null;
		}

		public ShopItemNode GetShopItemById(string _bundleId)
		{
			for (int i = 0; i < m_itemEntries.Count; i++)
			{
				ShopBaseNode shopBaseNode = m_itemEntries[i];
				if (shopBaseNode == null)
				{
					continue;
				}
				if (shopBaseNode.DataType == EShopDataType.Category)
				{
					ShopCategoryNode shopCategoryNode = shopBaseNode as ShopCategoryNode;
					if (shopCategoryNode != null)
					{
						ShopItemNode shopItemById = shopCategoryNode.GetShopItemById(_bundleId);
						if (shopItemById != null)
						{
							return shopItemById;
						}
					}
				}
				else if (shopBaseNode.DataType != EShopDataType.HappyHourBanner)
				{
					ShopItemNode shopItemNode = shopBaseNode as ShopItemNode;
					if (shopItemNode != null && string.Equals(shopItemNode.ShopBundle.id, _bundleId, StringComparison.InvariantCultureIgnoreCase))
					{
						return shopItemNode;
					}
				}
			}
			return null;
		}

		public ShopCategoryNode GetShopCategoryByType(EShopCategory _category)
		{
			for (int i = 0; i < m_itemEntries.Count; i++)
			{
				ShopBaseNode shopBaseNode = m_itemEntries[i];
				if (shopBaseNode != null && shopBaseNode.DataType == EShopDataType.Category)
				{
					ShopCategoryNode shopCategoryNode = shopBaseNode as ShopCategoryNode;
					if (shopCategoryNode == null || shopCategoryNode.Category == _category)
					{
						return shopCategoryNode;
					}
					ShopCategoryNode shopCategoryByType = shopCategoryNode.GetShopCategoryByType(_category);
					if (shopCategoryByType != null)
					{
						return shopCategoryByType;
					}
				}
			}
			return null;
		}

		public void Insert(int _index, ShopBaseNode _newNode, bool _updateIndexList = true)
		{
			m_itemEntries.Insert(_index, _newNode);
			if (_updateIndexList)
			{
				m_itemEntriesIndex.Clear();
			}
		}

		public void InsertNode(ShopBaseNode _newNode)
		{
			if (_newNode.DataType == EShopDataType.HappyHourBanner)
			{
				m_itemEntries.Insert(0, _newNode);
				m_itemEntries.Insert(1, null);
			}
			else
			{
				int index = m_itemEntries.Count;
				for (int i = 0; i < m_itemEntries.Count; i++)
				{
					ShopBaseNode shopBaseNode = m_itemEntries[i];
					if (shopBaseNode != null && (_newNode.SortID < shopBaseNode.SortID || (_newNode.DataType != shopBaseNode.DataType && _newNode.DataType == EShopDataType.Category) || (_newNode.DataType != shopBaseNode.DataType && _newNode.DataType == EShopDataType.HappyHourBanner)))
					{
						index = i;
						break;
					}
				}
				m_itemEntries.Insert(index, _newNode);
			}
			m_itemEntriesIndex.Clear();
		}

		public void UpdateAvailableItemsByProductPricings(ProductPricings _productPricings)
		{
			m_itemEntriesIndex.Clear();
			for (int i = 0; i < m_itemEntries.Count; i++)
			{
				ShopDirectPurchasItemNode shopDirectPurchasItemNode = null;
				ShopBaseNode shopBaseNode = m_itemEntries[i];
				if (shopBaseNode == null)
				{
					m_itemEntriesIndex.Add(i);
					continue;
				}
				switch (shopBaseNode.DataType)
				{
				case EShopDataType.Category:
				{
					ShopCategoryNode shopCategoryNode = shopBaseNode as ShopCategoryNode;
					shopCategoryNode.UpdateAvailableItemsByProductPricings(_productPricings);
					m_itemEntriesIndex.Add(i);
					break;
				}
				case EShopDataType.DirectPurchasItem:
					shopDirectPurchasItemNode = shopBaseNode as ShopDirectPurchasItemNode;
					if (shopDirectPurchasItemNode != null && _productPricings != null)
					{
						ProductPricing productPricing = _productPricings.GetProductPricing(shopDirectPurchasItemNode.ShopBundle.houstonId);
						if (productPricing != null)
						{
							shopDirectPurchasItemNode.ItemCurrencyText = productPricing.productPrice;
							m_itemEntriesIndex.Add(i);
						}
					}
					break;
				default:
					m_itemEntriesIndex.Add(i);
					break;
				}
			}
		}
	}
}
