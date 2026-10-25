using System;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Animations.ValueObservers;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIEquipmentHandler : UIScreenChild, IInitialized<UIInventoryScreenData>
	{
		[SerializeField]
		private UIInfoPopup m_detailsPopup;

		[SerializeField]
		private UIExtendedButton m_detailsButton;

		[SerializeField]
		private UIExtendedButton m_forgeButton;

		[SerializeField]
		private UIAssassinSelectionHelper m_assassinSelection;

		[SerializeField]
		private UIDurationObserver m_assassinFade;

		private UIInventoryScreenData m_data;

		private UIEquipmentGear[] m_equipmentSlots = new UIEquipmentGear[0];

		public event Action<bool> OnInfoPopUpChange;

		protected override void OnScreenInit()
		{
			m_equipmentSlots = GetComponentsInChildren<UIEquipmentGear>(true);
			UIEquipmentModule uIEquipmentModule = m_equipmentSlots.Find((UIEquipmentGear _x) => _x is UIEquipmentModule) as UIEquipmentModule;
			if (uIEquipmentModule != null)
			{
				uIEquipmentModule.OnModuleExpire += OnModuleExpire;
			}
		}

		protected override void OnScreenDestroy()
		{
			if (m_equipmentSlots != null)
			{
				UIEquipmentModule uIEquipmentModule = m_equipmentSlots.Find((UIEquipmentGear _x) => _x is UIEquipmentModule) as UIEquipmentModule;
				if (uIEquipmentModule != null)
				{
					uIEquipmentModule.OnModuleExpire -= OnModuleExpire;
				}
				m_equipmentSlots = null;
			}
		}

		public void Initialize(UIInventoryScreenData _data)
		{
			m_data = _data;
			for (int i = 0; i < m_equipmentSlots.Length; i++)
			{
				m_equipmentSlots[i].Initialize(_data, m_assassinSelection);
				m_equipmentSlots[i].OnClickCallback += OnEquipmentClicked;
			}
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChange);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdate);
			m_data.OnStateChanged += OnStateChanged;
			OnProfileUpdate(null);
			RemoveSelection();
		}

		public void Clear()
		{
			m_data.OnStateChanged -= OnStateChanged;
			m_data = null;
			for (int i = 0; i < m_equipmentSlots.Length; i++)
			{
				m_equipmentSlots[i].Clear();
				m_equipmentSlots[i].OnClickCallback -= OnEquipmentClicked;
			}
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChange);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdate);
			m_assassinSelection.FadeAvatar(true, 0.1f);
		}

		public void RemoveSelection()
		{
			m_detailsPopup.Close();
		}

		public UIEquipmentGear FindSelection()
		{
			return m_equipmentSlots.Find((UIEquipmentGear _e) => _e.Selected);
		}

		private void OnModuleExpire(InventoryElement _inventoryElement)
		{
			m_detailsPopup.Close();
		}

		private void OnEnable()
		{
			ResetSelection();
			m_detailsPopup.OnBeforePopupCloseCallback += OnPopupClosedCallback;
			m_detailsPopup.OnPopupOpenedCallback += OnPopupOpenedCallback;
			EventDelegate.Add(m_forgeButton.onClick, OnForgeClicked);
			EventDelegate.Add(m_detailsButton.onClick, OnDetailsClicked);
			UIDurationObserver assassinFade = m_assassinFade;
			assassinFade.OnObserverEnable = (Action<float>)Delegate.Combine(assassinFade.OnObserverEnable, new Action<float>(AssassinFadeTrigger));
		}

		private void OnDisable()
		{
			m_detailsPopup.OnBeforePopupCloseCallback -= OnPopupClosedCallback;
			m_detailsPopup.OnPopupOpenedCallback -= OnPopupOpenedCallback;
			EventDelegate.Remove(m_forgeButton.onClick, OnForgeClicked);
			EventDelegate.Remove(m_detailsButton.onClick, OnDetailsClicked);
			UIDurationObserver assassinFade = m_assassinFade;
			assassinFade.OnObserverEnable = (Action<float>)Delegate.Remove(assassinFade.OnObserverEnable, new Action<float>(AssassinFadeTrigger));
		}

		private void OnProfileUpdate(params object[] _params)
		{
			m_forgeButton.isEnabled = UIForgeHandler.IsForgeUnlocked();
		}

		private void OnStateChanged(UIInventoryScreenData.StateChangeData _stateChangeData)
		{
			RemoveSelection();
		}

		private void OnDetailsClicked()
		{
			m_data.State = UIInventoryScreenData.EScreenState.Details;
		}

		private void OnForgeClicked()
		{
			if (m_forgeButton.isEnabled)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_Large);
				m_data.State = UIInventoryScreenData.EScreenState.Forge;
			}
			else
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(7346));
			}
		}

		private void OnPopupOpenedCallback()
		{
			if (this.OnInfoPopUpChange != null)
			{
				this.OnInfoPopUpChange(true);
			}
		}

		private void OnPopupClosedCallback()
		{
			if (this.OnInfoPopUpChange != null)
			{
				this.OnInfoPopUpChange(false);
			}
			ResetSelection();
		}

		private void OnAssassinChange(params object[] _params)
		{
			m_detailsPopup.Close();
			ResetSelection();
		}

		private void OnEquipmentClicked(UIEquipmentGear _gear)
		{
			if (_gear.Empty)
			{
				m_detailsPopup.Close();
			}
			bool selected = _gear.Selected;
			ResetSelection();
			UpdateSelection(_gear, !selected);
		}

		public void OnScreenOpenFinsihed()
		{
			if (!IsSlotUnlocked(NotifySlotUnlockText))
			{
				TriggerUnlockAnimation();
			}
		}

		private void NotifySlotUnlockText(UIEquipmentGear slotUnlocked)
		{
			int num = 2235;
			if (slotUnlocked is UIEquipmentModule)
			{
				num = 7345;
			}
			else if (slotUnlocked is UIEquipmentItem)
			{
				UIEquipmentItem uIEquipmentItem = slotUnlocked as UIEquipmentItem;
				if (uIEquipmentItem.Category.EntitledToEquip())
				{
					switch (uIEquipmentItem.Category)
					{
					case Item.EItemCategory.Insignia:
						num = 7344;
						break;
					case Item.EItemCategory.Outfit:
						num = 7343;
						break;
					}
				}
			}
			UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(num), true, TriggerUnlockAnimation);
		}

		private bool IsSlotUnlocked(Action<UIEquipmentGear> _callback)
		{
			UIEquipmentGear uIEquipmentGear = null;
			for (int i = 0; i < m_equipmentSlots.Length; i++)
			{
				if (m_equipmentSlots[i] is UIEquipmentModule)
				{
					uIEquipmentGear = ((!Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotModuleEntitlementGUID) || UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID)) ? null : m_equipmentSlots[i]);
				}
				else if (m_equipmentSlots[i] is UIEquipmentItem)
				{
					UIEquipmentItem uIEquipmentItem = m_equipmentSlots[i] as UIEquipmentItem;
					if (!uIEquipmentItem.Category.EntitledToEquip())
					{
						continue;
					}
					switch (uIEquipmentItem.Category)
					{
					case Item.EItemCategory.Insignia:
						uIEquipmentGear = ((!UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID)) ? m_equipmentSlots[i] : null);
						break;
					case Item.EItemCategory.Outfit:
						uIEquipmentGear = ((!UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID)) ? m_equipmentSlots[i] : null);
						break;
					}
				}
				if (uIEquipmentGear != null && _callback != null)
				{
					_callback(uIEquipmentGear);
					return true;
				}
			}
			return false;
		}

		private void TriggerUnlockAnimation()
		{
			for (int i = 0; i < m_equipmentSlots.Length; i++)
			{
				bool playUnlockAnim = false;
				if (m_equipmentSlots[i] is UIEquipmentModule)
				{
					playUnlockAnim = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotModuleEntitlementGUID) && !UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotModuleEntitlementGUID);
				}
				else if (m_equipmentSlots[i] is UIEquipmentItem)
				{
					UIEquipmentItem uIEquipmentItem = m_equipmentSlots[i] as UIEquipmentItem;
					if (!uIEquipmentItem.Category.EntitledToEquip())
					{
						continue;
					}
					switch (uIEquipmentItem.Category)
					{
					case Item.EItemCategory.Insignia:
						playUnlockAnim = !UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID);
						break;
					case Item.EItemCategory.Outfit:
						playUnlockAnim = !UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID);
						break;
					}
				}
				m_equipmentSlots[i].PlaySlotUnlockAnim(playUnlockAnim);
			}
		}

		private void AssassinFadeTrigger(float _duration)
		{
			if (m_data != null)
			{
				m_assassinSelection.FadeAvatar(m_data.State == UIInventoryScreenData.EScreenState.Inventory, _duration);
			}
		}

		private void ResetSelection()
		{
			for (int i = 0; i < m_equipmentSlots.Length; i++)
			{
				m_equipmentSlots[i].Selected = false;
			}
		}

		private void UpdateSelection(UIEquipmentGear _gear, bool _selected)
		{
			_gear.Selected = _selected;
			InventoryElement inventoryElement = _gear.InventoryElement;
			if (inventoryElement != null && _selected)
			{
				if (inventoryElement.IsItem())
				{
					Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
					Transform indicatorTarget = _gear.transform;
					ItemInfoPopupHelper.ShowInfoPopup(m_detailsPopup, currentAssassin, ItemInfoPopupHelper.EMode.Unequip, inventoryElement, null, indicatorTarget, null, true, 0, string.Empty);
				}
				else if (inventoryElement.IsModule())
				{
					ModuleInfoPopupHelper.ShowInfoPopup(m_detailsPopup, ModuleInfoPopupHelper.EMode.None, inventoryElement, _gear.transform, true, string.Empty);
				}
			}
			else
			{
				m_detailsPopup.Close();
			}
		}
	}
}
