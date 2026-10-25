using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.ShopScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIInventoryHandler : UIScreenChild, IInitialized<UIInventoryScreenData>
	{
		private const string k_updateInventoryTrack = "UpdateInventory";

		private const string k_updateItemTrack = "ItemSwap";

		private static readonly StringId s_inventorySpaceId = new StringId(6540);

		private static readonly StringMapper s_sizeMapper = new StringMapper
		{
			{ "used_space", 0 },
			{ "free_space", 0 },
			{ "total_space", 0 }
		};

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_sizeLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedWrapContent m_wrapper;

		[CheckAssigned]
		[SerializeField]
		private UIInventoryFilter m_filter;

		[SerializeField]
		[CheckAssigned]
		private UIInfoPopup m_detailsPopup;

		[CheckAssigned]
		[SerializeField]
		private UIInventoryBlocker m_blocker;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAnimation m_updateTween;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		[CheckAssigned]
		private UIPanel m_scrollViewPanel;

		private bool m_isAnimatingWrapper;

		private bool m_isIgnoreWrapperUpdate;

		private UIInventoryScreenData m_data;

		private readonly List<InventoryItemBase> m_items = new List<InventoryItemBase>();

		private List<InventoryItemBase> m_pendingItems = new List<InventoryItemBase>();

		private UIInventoryItemView m_currentItemView;

		private int m_currentIndex = -1;

		private UIInventoryBlocker.BlockerHandle m_fullBlocker;

		private CompareItemGroups m_sortItemGroups = new CompareItemGroups();

		private ItemCompareBase m_tmpSortingComparer;

		private InventoryItem m_tutorialItem;

		public UIInventoryItemView CurrentItemView
		{
			get
			{
				return m_currentItemView;
			}
		}

		public UIInventoryScreenData ScreenData
		{
			get
			{
				return m_data;
			}
		}

		private bool ShowArrows
		{
			get
			{
				return m_currentItemView != null && m_detailsPopup.IndicatorTarget != null && m_detailsPopup.IndicatorTarget == m_currentItemView.transform && m_currentItemView.Selected;
			}
		}

		private InventoryItem TutorialItem
		{
			get
			{
				return (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25Forge)) ? null : m_tutorialItem;
			}
			set
			{
				m_tutorialItem = value;
				if (value != null)
				{
					m_pendingItems = GetActiveItems();
					UpdateItems(GetActiveItems());
				}
			}
		}

		public event Action<UIInventoryItemView> OnSelectedItemsChange;

		public event Action OnVisibleInventoryChange;

		public void SetTutorialItem(InventoryElement _inventoryelement)
		{
			TutorialItem = new InventoryItem(_inventoryelement, m_detailsPopup, m_data);
		}

		public void ResetTutorialItem()
		{
			TutorialItem = null;
		}

		public void PreInitSortingComparer(ItemCompareBase _sortingComparer)
		{
			m_tmpSortingComparer = _sortingComparer;
		}

		public void Initialize(UIInventoryScreenData _data)
		{
			m_data = _data;
			m_data.CurrentSorting = m_tmpSortingComparer;
			m_tmpSortingComparer = null;
			m_filter.Initialize(_data);
			m_data.OnFilterChange += OnFilterChange;
			m_data.OnStateChanged += OnStateChanged;
			m_data.OnAddedToForge += OnAddedToForge;
			m_data.OnRemovedFromForge += OnRemovedFromForge;
			m_data.OnForgeCleared += OnForgeCleared;
			Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper.OnPurchasedEvent += new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
			ItemInfoPopupHelper.OnEquipedItemCallback.Add(OnEquipItem);
			RemoveSelection();
			UpdateInventory();
			m_scrollViewPanel.leftAnchor.absolute -= 50;
			m_scrollViewPanel.rightAnchor.absolute += 50;
		}

		public void Clear()
		{
			m_data.OnFilterChange -= OnFilterChange;
			m_data.OnStateChanged -= OnStateChanged;
			m_data.OnAddedToForge -= OnAddedToForge;
			m_data.OnRemovedFromForge -= OnRemovedFromForge;
			m_data.OnForgeCleared -= OnForgeCleared;
			ItemInfoPopupHelper.OnEquipedItemCallback.Remove(OnEquipItem);
			m_data.ResetSorting();
			m_data = null;
			m_filter.Clear();
			m_items.Clear();
			m_updateTween.SetAnimationList(new UnityEngine.Animation[0], string.Empty);
			m_updateTween.enabled = false;
			m_currentIndex = -1;
			m_isAnimatingWrapper = false;
			m_isIgnoreWrapperUpdate = false;
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails != null && Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.BuyShopItemHelper.OnPurchasedEvent -= new SimpleDelegate.CallbackFunction(OnShopItemPurchased);
			}
		}

		public void RemoveSelection(bool _closePopup = true)
		{
			if (_closePopup)
			{
				m_detailsPopup.Close();
			}
			else
			{
				OnBeforePopupCloseCallback();
			}
		}

		protected override void OnScreenInit()
		{
			m_fullBlocker = m_blocker.GetBlocker(UIInventoryBlocker.EBlocker.Full);
			m_updateTween.IgnoreParent = true;
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			m_sizeLabel.text = string.Empty;
			m_wrapper.OnItemCreated += OnItemCreated;
			m_wrapper.OnUpdateItem += OnUpdateItem;
			m_wrapper.Init();
		}

		private void OnItemCreated(Transform _transform)
		{
			_transform.gameObject.AddMissingComponent<UIInventoryScrollItem>();
			UIInventoryItemView component = _transform.GetComponent<UIInventoryItemView>();
			component.Init();
			component.OnViewClicked += OnItemClicked;
		}

		private void OnUpdateItem(Transform _transform, int _index)
		{
			if (!m_isIgnoreWrapperUpdate)
			{
				if (m_isAnimatingWrapper)
				{
					UpdateAnimatedItemAtIndex(_transform, _index);
				}
				else
				{
					UpdateItemAtIndex(_transform, _index);
				}
			}
		}

		protected override void OnScreenDestroy()
		{
			EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			m_wrapper.OnItemCreated -= OnItemCreated;
			m_wrapper.OnUpdateItem -= OnUpdateItem;
			if (m_wrapper.Children == null)
			{
				return;
			}
			for (int i = 0; i < m_wrapper.Children.Count; i++)
			{
				Transform transform = m_wrapper.Children[i];
				if (transform != null)
				{
					UIInventoryItemView component = transform.GetComponent<UIInventoryItemView>();
					if (component != null)
					{
						component.OnViewClicked -= OnItemClicked;
					}
				}
			}
		}

		private void OnScreenActivate()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnInventoryUpdate += OnInventoryUpdate;
			m_detailsPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
			m_detailsPopup.OnPopupClosedCallback += OnPopupClosedCallback;
			m_detailsPopup.OnPopupOpenedCallback += OnPopUpOpenedCallback;
			m_filter.OnFilterOpen += OnFilterOpen;
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChange);
		}

		private void OnScreenDeActivate()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnInventoryUpdate -= OnInventoryUpdate;
			m_detailsPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
			m_detailsPopup.OnPopupClosedCallback -= OnPopupClosedCallback;
			m_detailsPopup.OnPopupOpenedCallback -= OnPopUpOpenedCallback;
			m_filter.OnFilterOpen -= OnFilterOpen;
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChange);
			m_isAnimatingWrapper = false;
			m_isIgnoreWrapperUpdate = false;
			m_items.Clear();
			m_pendingItems = null;
		}

		private void OnForgeCleared()
		{
			UpdateInventory();
		}

		private void OnRemovedFromForge(InventoryElement _inventoryElement, int _index)
		{
			UpdateList();
		}

		private void OnAddedToForge(InventoryElement _inventoryElement, int _index)
		{
			UpdateList();
			ClearSelection();
		}

		private void OnAssassinChange(params object[] _params)
		{
			m_detailsPopup.Close();
			UpdateList();
		}

		private void UpdateList()
		{
			foreach (Transform child in m_wrapper.Children)
			{
				UIInventoryItemView component = child.GetComponent<UIInventoryItemView>();
				component.UpdateView();
			}
		}

		private void OnStateChanged(UIInventoryScreenData.StateChangeData _stateChangeData)
		{
			RemoveSelection();
			ClearSelection();
		}

		private void OnFilterOpen()
		{
			m_detailsPopup.Close();
		}

		private void OnShopItemPurchased(object[] _params)
		{
			string bundleId = (string)_params[0];
			if (m_data.State != UIInventoryScreenData.EScreenState.Forge)
			{
				return;
			}
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			ShopCategoryNode categoryRootNode = shopDetails.CategoryRootNode;
			ShopItemNode shopItemById = categoryRootNode.GetShopItemById(bundleId);
			if (shopItemById.ShopBundle.payout == null || shopItemById.ShopBundle.payout.lootTemplates.Length <= 0)
			{
				return;
			}
			LootTemplate.BlueprintTemplate blueprintTemplate = shopItemById.ShopBundle.payout.lootTemplates[0].blueprintTemplate;
			if (blueprintTemplate == null)
			{
				return;
			}
			EBlueprintType type = blueprintTemplate.type;
			InventoryElement inventoryElement = null;
			for (int i = 0; i < m_items.Count; i++)
			{
				if (m_items[i].Element.IsBlueprint() && m_items[i].Element.blueprintData.type == type && !m_data.IsForgeEmpty && (inventoryElement == null || inventoryElement.stackSize > m_items[i].Element.stackSize))
				{
					inventoryElement = m_items[i].Element;
				}
			}
			ItemInfoPopupHelper.OnAddToForgeCallback.Trigger(null, inventoryElement);
		}

		private void OnInventoryUpdate(InventoryElement[] _new, InventoryElement[] _old, int _newSize, int _oldSize)
		{
			if (_newSize != _oldSize || !_old.ArrayEqual(_new, CompareItems))
			{
				UpdateInventory();
			}
		}

		private void OnItemClicked(UIInventoryItemView _view)
		{
			int currentIndex = m_currentIndex;
			int index = _view.GetComponent<UIInventoryScrollItem>().Index;
			InventoryItemBase _out;
			m_data.VisibleInventory.TryGet(currentIndex, out _out);
			InventoryItemBase _out2;
			m_data.VisibleInventory.TryGet(index, out _out2);
			if (UpdateSelection(_view, m_currentItemView, _out2, _out))
			{
				m_currentIndex = index;
				m_currentItemView = _view;
				m_currentItemView.UpdateView();
			}
			else
			{
				m_currentIndex = -1;
				m_detailsPopup.Close();
			}
			Action<UIInventoryItemView> onSelectedItemsChange = this.OnSelectedItemsChange;
			if (onSelectedItemsChange != null)
			{
				onSelectedItemsChange(m_currentItemView);
			}
		}

		private bool UpdateSelection(UIInventoryItemView _newItemView, UIInventoryItemView _oldItemView, InventoryItemBase _newItemBase, InventoryItemBase _oldItemBase)
		{
			if (_newItemView == _oldItemView && _newItemBase.Selectable)
			{
				_oldItemView.Selected = false;
				m_detailsPopup.Close();
				return false;
			}
			if (_newItemBase.Clickable)
			{
				_newItemBase.Clicked(_newItemView.transform);
				_newItemView.UpdateView();
			}
			if (_newItemBase.Selectable)
			{
				if (_oldItemView != null && _oldItemBase != null && _oldItemView.Index == m_currentIndex)
				{
					_oldItemView.Selected = false;
				}
				_newItemView.Selected = true;
				if (_newItemBase.Element != null)
				{
					UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemSetSeen(_newItemBase.Element.id);
				}
				else
				{
					m_detailsPopup.Close();
				}
				return _newItemBase.Selectable;
			}
			if (_oldItemView != null && _oldItemBase != null)
			{
				m_detailsPopup.Close();
			}
			m_filter.Close();
			return false;
		}

		private void OnPopUpOpenedCallback()
		{
			m_filter.Close();
		}

		private void OnPopupClosedCallback()
		{
			ClearSelection();
			m_currentIndex = -1;
		}

		private void OnBeforePopupCloseCallback()
		{
			ClearSelection();
			m_currentIndex = -1;
			m_data.HideForgeGridPreview();
		}

		private void OnFilterChange()
		{
			ClearSelection();
			m_currentIndex = -1;
			StartCoroutine(co_UpdateInventory());
		}

		private void OnEquipItem(params object[] _params)
		{
			InventoryElement newItem = _params[1] as InventoryElement;
			InventoryElement oldItem = _params[2] as InventoryElement;
			bool equipped = (bool)_params[3];
			m_data.ItemEquipped(newItem, oldItem, equipped);
		}

		private void UpdateItems(List<InventoryItemBase> _elements)
		{
			m_data.VisibleInventory = _elements;
			m_wrapper.ItemCount = _elements.Count;
			m_wrapper.WrapScrollView.ResetPosition();
			if (this.OnVisibleInventoryChange != null)
			{
				this.OnVisibleInventoryChange();
			}
		}

		private void ClearSelection()
		{
			if (m_currentItemView != null)
			{
				if (m_currentIndex == m_currentItemView.Index)
				{
					m_currentItemView.Selected = false;
				}
				m_currentItemView = null;
			}
		}

		private List<InventoryItemBase> GetActiveItems()
		{
			List<InventoryItemBase> list = m_items.FindAll(m_data.CurrentFilter);
			if (m_data.State == UIInventoryScreenData.EScreenState.Inventory)
			{
				ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
				if (m_data.CurrentFilter == new Predicate<InventoryItemBase>(ItemFilters.FilterAll))
				{
					for (int i = 0; i < profileManager.InventoryFreeSpace; i++)
					{
						list.Add(new EmptyInventoryItem(m_data));
					}
					if (profileManager.CanExpandInventory)
					{
						list.Add(new BuyInventorySlot(m_data));
					}
				}
			}
			else if (m_data.State == UIInventoryScreenData.EScreenState.Forge)
			{
				list.Add(new BuyBluePrintSlot(m_data, OnPopUpOpenedCallback));
			}
			InventoryItem tutorialItem = TutorialItem;
			if (tutorialItem != null)
			{
				list.Add(tutorialItem);
			}
			m_sortItemGroups.ItemCompare = m_data.CurrentSorting;
			list.Sort(m_sortItemGroups);
			return list;
		}

		private void LoadInventory()
		{
			m_items.Clear();
			InventoryElement[] elements = Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.elements;
			for (int i = 0; i < elements.Length; i++)
			{
				m_items.Add(new InventoryItem(elements[i], m_detailsPopup, m_data));
			}
			UpdateItems(GetActiveItems());
			m_wrapper.WrapScrollView.ResetPosition();
		}

		private void UpdateInventory()
		{
			if (m_data.CanUpdateInventory)
			{
				ClearSelection();
				m_currentIndex = -1;
				LoadInventory();
				UpdateInventorySpace();
			}
		}

		private void UpdateInventorySpace()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			s_sizeMapper["free_space"] = LocalizationManager.Instance.GetInteger(profileManager.InventoryFreeSpace);
			s_sizeMapper["used_space"] = LocalizationManager.Instance.GetInteger(profileManager.InventoryUsage);
			s_sizeMapper["total_space"] = LocalizationManager.Instance.GetInteger(profileManager.InventorySize);
			m_sizeLabel.color = ((profileManager.InventoryFreeSpace <= 0) ? ((Color)UIDataMapper.UITextColors.NegativeColor) : Color.white);
			m_sizeLabel.text = LocalizationManager.Instance.Get(s_inventorySpaceId, s_sizeMapper);
		}

		public int GetItemIndexByTechnicalName(string _technicalName)
		{
			if (m_data.VisibleInventory == null)
			{
				return -1;
			}
			for (int i = 0; i < m_data.VisibleInventory.Count; i++)
			{
				InventoryItemBase inventoryItemBase = m_data.VisibleInventory[i];
				if (inventoryItemBase.Element != null && inventoryItemBase.Element.technicalName.Equals(_technicalName))
				{
					return i;
				}
			}
			return -1;
		}

		public int GetItemIndexOfBuyBluePrintSlot()
		{
			if (m_data.VisibleInventory == null)
			{
				return -1;
			}
			for (int i = 0; i < m_data.VisibleInventory.Count; i++)
			{
				InventoryItemBase inventoryItemBase = m_data.VisibleInventory[i];
				if (inventoryItemBase is BuyBluePrintSlot)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetItemIndexThatCanBeAddedToForge()
		{
			if (m_data.VisibleInventory == null)
			{
				return 0;
			}
			for (int i = 0; i < m_data.VisibleInventory.Count; i++)
			{
				InventoryItemBase inventoryItemBase = m_data.VisibleInventory[i];
				InventoryItem inventoryItem = inventoryItemBase as InventoryItem;
				if (inventoryItem != null && inventoryItem.Clickable && inventoryItemBase.Element.CanBeForged() && !inventoryItem.IsInForge)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetFirstAvailableTutorialMysteryBox()
		{
			for (int i = 0; i < m_data.VisibleInventory.Count; i++)
			{
				InventoryItemBase inventoryItemBase = m_data.VisibleInventory[i];
				if (inventoryItemBase.Element != null && inventoryItemBase.Element.IsMysteryBox() && inventoryItemBase.Element.mysteryBoxData != null && inventoryItemBase.Element.mysteryBoxData.mysteryBoxType == EMysteryBoxType.MysteryBoxItemType_Tutorial)
				{
					return i;
				}
			}
			return -1;
		}

		public IEnumerator co_UpdateInventory()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			m_pendingItems = GetActiveItems();
			if (m_pendingItems.Count == 0 && m_data.VisibleInventory.Count == 0)
			{
				yield break;
			}
			m_isIgnoreWrapperUpdate = true;
			m_wrapper.WrapScrollView.ResetPosition();
			m_isIgnoreWrapperUpdate = false;
			m_isAnimatingWrapper = true;
			m_fullBlocker.On(false, false);
			m_wrapper.ItemCount = Mathf.Max(m_pendingItems.Count, m_data.VisibleInventory.Count);
			m_animation.SampleAnimation(0f, string.Empty);
			yield return wait;
			List<Transform> children = m_wrapper.Children;
			int animatedCount = Mathf.Min(m_wrapper.ItemCount, children.Count);
			List<UnityEngine.Animation> anims = new List<UnityEngine.Animation>(animatedCount);
			for (int j = 0; j < animatedCount; j++)
			{
				UnityEngine.Animation anim = children[j].GetComponent<UnityEngine.Animation>();
				anim.SampleAnimation(0f, "ItemSwap");
				anims.Add(anim);
			}
			m_updateTween.SetAnimationList(anims.ToArray(), "ItemSwap");
			m_animation.Play("UpdateInventory");
			do
			{
				yield return wait;
			}
			while (m_animation.IsPlaying("UpdateInventory") || m_updateTween.IsAnimating);
			m_updateTween.enabled = false;
			m_fullBlocker.Off(false);
			m_data.VisibleInventory = m_pendingItems;
			m_isIgnoreWrapperUpdate = true;
			m_wrapper.ItemCount = m_pendingItems.Count;
			m_isIgnoreWrapperUpdate = false;
			for (int i = 0; i < m_wrapper.Children.Count; i++)
			{
				UIInventoryItemView item = m_wrapper.Children[i].GetComponent<UIInventoryItemView>();
				if (!item.gameObject.activeSelf)
				{
					item.EmptyItem(Item.EItemCategory.None);
				}
			}
			m_isAnimatingWrapper = false;
			if (this.OnVisibleInventoryChange != null)
			{
				this.OnVisibleInventoryChange();
			}
		}

		private void UpdateItemAtIndex(Transform _transform, int _index)
		{
			if (_index >= 0 && _index < m_data.VisibleInventory.Count)
			{
				UIInventoryScrollItem component = _transform.GetComponent<UIInventoryScrollItem>();
				UIInventoryItemView component2 = _transform.GetComponent<UIInventoryItemView>();
				InventoryItemBase item = m_data.VisibleInventory[_index];
				component2.UpdateView(item, _index);
				component2.SetSelected(m_currentIndex == _index, true);
				m_detailsPopup.ShowArrows(ShowArrows);
				component.Index = _index;
			}
		}

		private void UpdateAnimatedItemAtIndex(Transform _transform, int _index)
		{
			if (_index >= 0)
			{
				UIInventoryScrollItem component = _transform.GetComponent<UIInventoryScrollItem>();
				component.Index = _index;
				UIInventoryItemView component2 = _transform.GetComponent<UIInventoryItemView>();
				InventoryItemBase item = null;
				if (_index < m_pendingItems.Count)
				{
					item = m_pendingItems[_index];
				}
				component2.Prepare(item, _index);
			}
		}

		private static bool CompareItems(InventoryElement _lhs, InventoryElement _rhs)
		{
			return _lhs.id.Equals(_rhs.id) && _lhs.stackSize == _rhs.stackSize;
		}
	}
}
