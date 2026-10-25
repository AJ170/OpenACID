using System.Collections.Generic;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.InventoryScreenBases
{
	public abstract class TutorialInventorySelectBase : TutorialButtonClickBase
	{
		[SerializeField]
		private UIInventoryHandler m_inventoryHandler;

		[SerializeField]
		private UIExtendedWrapContent m_itemContainer;

		private UIInventoryItemView m_equippableItem;

		protected override void OnSetupInternal()
		{
			m_equippableItem = GetEquippableItemView();
			if (m_equippableItem == null)
			{
				CancelTutorial();
				return;
			}
			m_buttonWidget = m_equippableItem.GetComponent<UIWidget>();
			m_button = m_equippableItem.GetComponent<UIExtendedButton>();
			base.OnSetupInternal();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_equippableItem = null;
			m_buttonWidget = null;
			m_button = null;
		}

		protected UIInventoryItemView GetEquippableItemView()
		{
			int itemIndexByTechnicalName = m_inventoryHandler.GetItemIndexByTechnicalName(GetItemTechnicalName());
			List<Transform> children = m_itemContainer.Children;
			return TutorialHelper.GetInventoryItemView(children, itemIndexByTechnicalName);
		}

		protected abstract string GetItemTechnicalName();

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}
	}
}
