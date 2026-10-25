using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxSelect : TutorialButtonClickBase
	{
		[SerializeField]
		private UIInventoryHandler m_inventoryHandler;

		[SerializeField]
		private UIExtendedWrapContent m_itemContainer;

		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		private UIInventoryItemView m_mysteryBoxItemView;

		public override string Name
		{
			get
			{
				return "Mystery Box Select";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25MysteryBox;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override void Activate()
		{
			m_mysteryBoxItemView = GetMysteryBoxItemView();
			if (m_mysteryBoxItemView == null)
			{
				CancelTutorial();
				return;
			}
			m_buttonWidget = m_mysteryBoxItemView.GetComponent<UIWidget>();
			m_button = m_mysteryBoxItemView.GetComponent<UIExtendedButton>();
			base.Activate();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
			m_button = null;
			m_mysteryBoxItemView = null;
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 6956;
		}

		public override int GetDescriptionText()
		{
			return 6898;
		}

		private UIInventoryItemView GetMysteryBoxItemView()
		{
			int firstAvailableTutorialMysteryBox = m_inventoryHandler.GetFirstAvailableTutorialMysteryBox();
			if (firstAvailableTutorialMysteryBox >= 0)
			{
				List<Transform> children = m_itemContainer.Children;
				for (int i = 0; i < children.Count; i++)
				{
					Transform transform = children[i];
					UIInventoryScrollItem component = transform.GetComponent<UIInventoryScrollItem>();
					if (component.Index == firstAvailableTutorialMysteryBox)
					{
						return transform.GetComponent<UIInventoryItemView>();
					}
				}
			}
			return null;
		}
	}
}
