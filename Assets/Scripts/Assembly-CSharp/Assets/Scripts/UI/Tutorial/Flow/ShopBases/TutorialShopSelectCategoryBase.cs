using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.ShopScreen;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.ShopBases
{
	public abstract class TutorialShopSelectCategoryBase : TutorialFlowBase
	{
		[SerializeField]
		private FullscreenBlocker m_blocker;

		[SerializeField]
		private FullscreenBlocker m_categoryBlocker;

		[SerializeField]
		private UISlantedPopup m_textPopup;

		[SerializeField]
		private UIShopDetails m_shopDetails;

		private UIDragScrollView m_drag;

		private UIShopBaseItem m_shopCategoryItem;

		protected void Awake()
		{
		}

		public override void Activate()
		{
			base.Activate();
			ActivateMenuBarBlockers(true);
			m_textPopup.gameObject.SetActive(true);
			m_textPopup.SetHeaderTextLocalized(GetHeaderText());
			m_textPopup.SetDescriptionTextLocalized(GetDescriptionText());
			m_textPopup.MoveIn();
			m_categoryBlocker.gameObject.SetActive(true);
			m_blocker.gameObject.SetActive(true);
			EventDelegate.Add(m_shopDetails.OnShopCategoryChanged, OnShopCategoryChanged);
			UIWidget uIWidget = null;
			List<UIShopBaseItem> categoryItems = m_shopDetails.CategoryItems;
			for (int i = 0; i < categoryItems.Count; i++)
			{
				ShopCategoryNode shopCategoryNode = categoryItems[i].CurrentShopBaseNode as ShopCategoryNode;
				if (shopCategoryNode != null && shopCategoryNode.Category == GetShopCategory())
				{
					m_shopDetails.CategoryScroller.SetFocusOnChildByIndex(i);
					m_shopCategoryItem = categoryItems[i];
					uIWidget = categoryItems[i].gameObject.GetComponent<UIWidget>();
					break;
				}
			}
			m_drag = uIWidget.GetComponent<UIDragScrollView>();
			m_drag.enabled = false;
			UIWidget[] componentsInChildren = uIWidget.GetComponentsInChildren<UIWidget>(true);
			m_categoryBlocker.AddNonBlocked(componentsInChildren);
			UnblockOptionsButton(m_blocker);
			UnblockOptionsButton(m_categoryBlocker);
			m_categoryBlocker.IncrementDepth();
			m_blocker.IncrementDepth();
			CreateButtonHighlight(uIWidget.gameObject, Vector3.zero);
			ActivateBackButton(false);
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				ActivateMenuBarBlockers(false);
				EventDelegate.Remove(m_shopDetails.OnShopCategoryChanged, OnShopCategoryChanged);
				m_categoryBlocker.gameObject.SetActive(false);
				m_categoryBlocker.DecrementDepth();
				m_blocker.gameObject.SetActive(false);
				m_blocker.DecrementDepth();
				ActivateBackButton(true);
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (m_shopCategoryItem != null)
			{
				EventDelegate.Remove(m_shopDetails.OnShopCategoryChanged, OnShopCategoryChanged);
			}
			m_shopCategoryItem = null;
			m_drag = null;
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.ShopScreen;
		}

		private void OnShopCategoryChanged()
		{
			m_drag.enabled = true;
			EventDelegate.Remove(m_shopCategoryItem.OnSelected, OnShopCategoryChanged);
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		protected abstract EShopCategory GetShopCategory();

		public abstract int GetHeaderText();

		public abstract int GetDescriptionText();
	}
}
