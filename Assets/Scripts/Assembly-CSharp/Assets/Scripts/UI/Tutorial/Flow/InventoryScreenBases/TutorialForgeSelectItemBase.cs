using AcierProtocol;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.InventoryScreenBases
{
	public abstract class TutorialForgeSelectItemBase : TutorialButtonClickBase
	{
		[SerializeField]
		protected UIInventoryHandler m_inventoryHandler;

		[SerializeField]
		protected UIExtendedWrapContent m_itemContainer;

		private UIInventoryItemView m_itemToSelect;

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Forge;
			}
		}

		public override void Activate()
		{
			m_itemToSelect = GetItemViewToSelect();
			if (m_itemToSelect == null)
			{
				m_inventoryHandler.OnVisibleInventoryChange += OnVisibleInventoryChange;
			}
			else
			{
				OnVisibleInventoryChange();
			}
		}

		public override void Deactivate()
		{
			m_itemToSelect = null;
			m_inventoryHandler.OnVisibleInventoryChange -= OnVisibleInventoryChange;
			base.Deactivate();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
			m_itemToSelect = null;
			if (m_inventoryHandler != null)
			{
				m_inventoryHandler.OnVisibleInventoryChange -= OnVisibleInventoryChange;
			}
		}

		private void OnVisibleInventoryChange()
		{
			m_inventoryHandler.OnVisibleInventoryChange -= OnVisibleInventoryChange;
			m_itemToSelect = GetItemViewToSelect();
			if (m_itemToSelect == null)
			{
				CancelTutorial();
				return;
			}
			m_buttonWidget = m_itemToSelect.GetComponent<UIWidget>();
			m_button = m_itemToSelect.GetComponent<UIExtendedButton>();
			base.Activate();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		protected abstract UIInventoryItemView GetItemViewToSelect();

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}
	}
}
