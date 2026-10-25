using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.ShopScreen;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.ShopBases
{
	public abstract class TutorialShopSelectItemBase : TutorialFlowBase
	{
		[SerializeField]
		private FullscreenBlocker m_blocker;

		[SerializeField]
		private FullscreenBlocker m_itemsBlocker;

		[SerializeField]
		private UISlantedPopup m_textPopup;

		[SerializeField]
		private UIShopDetails m_shopDetails;

		private UIShopBaseItem m_shopBaseItem;

		private UIDragScrollView m_shopItemDrag;

		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		protected void Awake()
		{
			m_shopDetails.ItemScroller.OnItemWrapped += OnShopItemWrapped;
		}

		public override void Activate()
		{
			base.Activate();
			ActivateMenuBarBlockers(true);
			UnblockOptionsButton(m_blocker);
			m_textPopup.SetHeaderTextLocalized(GetHeaderText());
			m_textPopup.SetDescriptionTextLocalized(GetDescriptionText());
			m_textPopup.MoveIn();
			m_itemsBlocker.gameObject.SetActive(true);
			m_blocker.gameObject.SetActive(true);
			m_blocker.IncrementDepth();
			UIShopDetails shopDetails = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails;
			ShopCategoryNode categoryRootNode = shopDetails.CategoryRootNode;
			ShopItemNode shopItemById = categoryRootNode.GetShopItemById(GetItemToSelect());
			shopDetails.OpenAndSetFocusOnItem(shopItemById);
			ActivateBackButton(false);
		}

		protected abstract string GetItemToSelect();

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				ActivateMenuBarBlockers(false);
				m_shopDetails.ItemScroller.OnItemWrapped -= OnShopItemWrapped;
				EventDelegate.Remove(m_shopBaseItem.OnSelected, OnItemSelected);
				m_textPopup.MoveOutInstant();
				m_shopItemDrag.enabled = true;
				m_itemsBlocker.DecrementDepth();
				m_itemsBlocker.gameObject.SetActive(false);
				m_blocker.DecrementDepth();
				m_blocker.gameObject.SetActive(false);
				ActivateBackButton(true);
			}
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.ShopScreen;
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_shopItemDrag = null;
			if (m_shopDetails != null && m_shopDetails.ItemScroller != null)
			{
				m_shopDetails.ItemScroller.OnItemWrapped -= OnShopItemWrapped;
			}
			if (m_shopBaseItem != null)
			{
				EventDelegate.Remove(m_shopBaseItem.OnSelected, OnItemSelected);
				m_shopBaseItem = null;
			}
		}

		private void OnShopItemWrapped(UIShopBaseItem _item)
		{
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsShopTutorialBeyondGotoShop(Category, GetGotoShopStep()))
			{
				return;
			}
			ShopItemNode shopItemNode = _item.CurrentShopBaseNode as ShopItemNode;
			if (shopItemNode != null && string.Compare(shopItemNode.ShopBundle.id, GetItemToSelect(), true) == 0)
			{
				if ((bool)m_shopBaseItem)
				{
					EventDelegate.Remove(m_shopBaseItem.OnSelected, OnItemSelected);
					m_itemsBlocker.DecrementDepth();
				}
				if ((bool)m_shopItemDrag)
				{
					m_shopItemDrag.enabled = true;
				}
				ClearEffects();
				m_shopBaseItem = _item;
				EventDelegate.Add(m_shopBaseItem.OnSelected, OnItemSelected);
				UIWidget component = m_shopBaseItem.gameObject.GetComponent<UIWidget>();
				m_shopItemDrag = component.GetComponent<UIDragScrollView>();
				m_shopItemDrag.enabled = false;
				UIWidget[] components = component.GetComponents<UIWidget>();
				m_itemsBlocker.SetNonBlocked(components);
				m_itemsBlocker.IncrementDepth();
				CreateButtonHighlight(component.gameObject, Vector3.zero);
			}
		}

		private void OnItemSelected()
		{
			EventDelegate.Remove(m_shopBaseItem.OnSelected, OnItemSelected);
			m_shopItemDrag.enabled = false;
			m_itemsBlocker.DecrementDepth();
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public abstract EFlowId GetGotoShopStep();

		public abstract int GetHeaderText();

		public abstract int GetDescriptionText();
	}
}
