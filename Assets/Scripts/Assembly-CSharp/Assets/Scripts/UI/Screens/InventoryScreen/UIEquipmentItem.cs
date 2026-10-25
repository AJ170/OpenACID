using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIEquipmentItem : UIEquipmentGear
	{
		private const string k_equipmentSwap = "EquipmentSwap";

		[SerializeField]
		private Item.EItemCategory m_category = Item.EItemCategory.None;

		private InventoryElement m_itemData;

		private InventoryElement m_pendingData;

		public Item.EItemCategory Category
		{
			get
			{
				return m_category;
			}
		}

		public override bool Empty
		{
			get
			{
				return m_itemData == null;
			}
		}

		public override bool Selectable
		{
			get
			{
				return true;
			}
		}

		public override InventoryElement InventoryElement
		{
			get
			{
				return m_itemData;
			}
		}

		public bool HasInsigniaEntitlement()
		{
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID);
		}

		public bool HasOutfitEntitlement()
		{
			return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID);
		}

		protected override void DoInitialize(UIInventoryScreenData _data)
		{
			base.DoInitialize(_data);
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(AssassinChangeCallback);
			base.ScreenData.OnEquipItem += OnItemEquipped;
			base.ScreenData.OnUnEquipItem += OnUnequippItem;
			UpdateIcon(false);
			base.IconItem.Clickable = !Empty;
		}

		protected override void DoClear()
		{
			base.DoClear();
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(AssassinChangeCallback);
			base.ScreenData.OnEquipItem -= OnItemEquipped;
			base.ScreenData.OnUnEquipItem -= OnUnequippItem;
		}

		protected override void DoEnable()
		{
			base.DoEnable();
			base.IconItem.OnSwapEquipment += OnItemSwap;
		}

		protected override void DoDisable()
		{
			base.DoDisable();
			base.IconItem.OnSwapEquipment -= OnItemSwap;
		}

		protected override void DoClicked()
		{
			base.DoClicked();
			if (!m_category.EntitledToEquip())
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(m_category.MissingEntitlementPopupMessage()), true);
			}
		}

		private void UpdateIcon(bool _animate)
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin != null)
			{
				m_pendingData = currentAssassin.GetElementForCategory(m_category);
				if (m_pendingData != null)
				{
					OnUnlockAnimationFinished();
				}
				if (!_animate)
				{
					OnItemSwap();
					return;
				}
				base.IconItem.Animation.SampleAnimation(0f, "EquipmentSwap");
				base.IconItem.Animation.Play("EquipmentSwap");
			}
		}

		private void OnItemSwap()
		{
			m_itemData = m_pendingData;
			m_pendingData = null;
			if (m_itemData != null)
			{
				base.IconItem.UpdateView(m_itemData);
			}
			else if (m_category.EntitledToEquip() && m_category.WasEntitledToEquipShown())
			{
				base.IconItem.EmptyItem(m_category);
			}
			else
			{
				base.IconItem.ReservedSlotIcon();
			}
			base.IconItem.Clickable = !Empty;
		}

		private void HandleChange(InventoryElement _newItem, InventoryElement _oldItem)
		{
			if (ItemExtensions.AltersVisuals(m_category, _newItem, _oldItem))
			{
				base.AssassinSelection.UpdateAssassin(true, m_category == Item.EItemCategory.Sword);
			}
		}

		private void AssassinChangeCallback(params object[] _params)
		{
			UpdateIcon(false);
		}

		private void OnUnequippItem(InventoryElement _newItem, InventoryElement _oldItem)
		{
			if (_oldItem.itemData != null && _oldItem.itemData.category == m_category)
			{
				UpdateIcon(true);
				HandleChange(_newItem, _oldItem);
			}
		}

		private void OnItemEquipped(InventoryElement _newItem, InventoryElement _oldItem)
		{
			if (_newItem.itemData != null && _newItem.itemData.category == m_category)
			{
				HandleChange(_newItem, _oldItem);
				UpdateIcon(true);
			}
		}

		public void OnChangeBackgroundIcon()
		{
			base.IconItem.SetEmptyItemIcon(m_category, 0f);
		}
	}
}
