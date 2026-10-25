using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIEquipmentModule : UIEquipmentGear
	{
		private const string k_equipmentSwap = "EquipmentSwap";

		private static readonly StringId s_noEntitlementId = new StringId(7197);

		private bool m_isWaitingForSwapingItem;

		private InventoryElement m_element;

		private InventoryElement m_pendingElement;

		private ModuleData m_module;

		public override bool Empty
		{
			get
			{
				return m_module == null;
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
				return m_element;
			}
		}

		public bool HasModuleEntitlement
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotModuleEntitlementGUID);
			}
		}

		public event Action<InventoryElement> OnModuleExpire;

		protected override void DoInitialize(UIInventoryScreenData _data)
		{
			base.DoInitialize(_data);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdate);
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChange);
			base.ScreenData.OnEquipItem += OnItemEquipped;
			UpdateModule(false);
			base.IconItem.Clickable = !Empty;
		}

		protected override void DoClear()
		{
			base.DoClear();
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdate);
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChange);
			base.ScreenData.OnEquipItem -= OnItemEquipped;
		}

		protected override void DoEnable()
		{
			base.DoEnable();
			base.IconItem.OnSwapEquipment += SwapModule;
		}

		protected override void DoDisable()
		{
			base.DoDisable();
			base.IconItem.OnSwapEquipment -= SwapModule;
			m_isWaitingForSwapingItem = false;
		}

		protected override void DoClicked()
		{
			base.DoClicked();
			if (!HasModuleEntitlement)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(s_noEntitlementId), true);
			}
		}

		private void Update()
		{
			if (m_module == null || m_module.durationType != EDurationType.Time)
			{
				return;
			}
			if (m_module.Expired())
			{
				if (this.OnModuleExpire != null)
				{
					this.OnModuleExpire(m_element);
				}
				m_module = null;
				UpdateModule(true);
			}
			else
			{
				UpdateFactor();
			}
		}

		private void OnProfileUpdate(params object[] _params)
		{
			InventoryElement elementForSlot = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
			if (!elementForSlot.Equivalent(m_element))
			{
				UpdateModule(true);
			}
		}

		private void OnAssassinChange(params object[] _params)
		{
			UpdateModule(false);
		}

		private void OnItemEquipped(InventoryElement _newItem, InventoryElement _oldItem)
		{
			if (_newItem != null && _newItem.IsModule())
			{
				UpdateModule(true);
			}
		}

		private void UpdateModule(bool _animated)
		{
			m_pendingElement = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
			if (m_pendingElement != null && m_pendingElement.moduleData.Expired())
			{
				m_pendingElement = null;
			}
			if (m_pendingElement != null)
			{
				OnUnlockAnimationFinished();
			}
			if (!_animated)
			{
				SwapModule();
			}
			else if (!m_isWaitingForSwapingItem)
			{
				m_isWaitingForSwapingItem = true;
				base.IconItem.Animation.SampleAnimation(0f, "EquipmentSwap");
				base.IconItem.Animation.Play("EquipmentSwap");
			}
		}

		private void SwapModule()
		{
			m_isWaitingForSwapingItem = false;
			m_element = m_pendingElement;
			m_pendingElement = null;
			if (m_element != null && m_element.moduleData != null && !m_element.moduleData.Expired())
			{
				base.IconItem.UpdateView(m_element);
				m_module = m_element.moduleData;
			}
			else if (HasModuleEntitlement && UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID))
			{
				base.IconItem.EmptyModule();
				m_module = null;
			}
			else
			{
				base.IconItem.ReservedSlotIcon();
			}
			UpdateFactor();
			base.IconItem.Clickable = !Empty;
		}

		private void UpdateFactor()
		{
			base.IconItem.UpdateFactor((m_module == null) ? 0f : m_module.RemainingPercentage());
		}

		public void OnChangeBackgroundIcon()
		{
			if (HasModuleEntitlement && !UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID))
			{
				base.IconItem.SetModuleIcon();
			}
		}
	}
}
