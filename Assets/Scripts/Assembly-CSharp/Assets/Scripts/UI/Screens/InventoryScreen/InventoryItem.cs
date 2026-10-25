using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class InventoryItem : InventoryItemBase
	{
		private static readonly StringId s_noAddToForge = new StringId(7258);

		private readonly InventoryElement m_item;

		private readonly UIInfoPopup m_popup;

		public override bool Selectable
		{
			get
			{
				return true;
			}
		}

		public override bool Clickable
		{
			get
			{
				return true;
			}
		}

		public bool IsInForge
		{
			get
			{
				return base.ScreenData.IsInForge(m_item);
			}
		}

		public override InventoryElement Element
		{
			get
			{
				return m_item;
			}
		}

		public override int Order
		{
			get
			{
				return 0;
			}
		}

		public InventoryItem(InventoryElement _item, UIInfoPopup _popup, UIInventoryScreenData _data)
			: base(_data)
		{
			m_item = _item;
			m_popup = _popup;
		}

		public override void Render(UIInventoryItemView _view)
		{
			_view.UpdateView(m_item);
			_view.ShowNewsIcon(!UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemIsSeen(m_item.id));
			_view.SetBlocked(base.ScreenData.IsInForge(m_item), true);
			if (m_item.IsItem() && base.ScreenData.State == UIInventoryScreenData.EScreenState.Inventory)
			{
				bool flag = m_item.IsItemEquippable(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin) && m_item.EntitledToEquip();
				_view.ShowNonEquipableIcon(!flag, true);
			}
			else
			{
				_view.ShowNonEquipableIcon(false, true);
			}
			_view.ShowNewsIcon(!UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemIsSeen(m_item.id));
			_view.Clickable = Clickable;
		}

		public override void Clicked(Transform _transform)
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (m_item.IsItem())
			{
				ItemInfoPopupHelper.EMode eMode = ItemInfoPopupHelper.EMode.Equip;
				if (base.ScreenData.State == UIInventoryScreenData.EScreenState.Forge)
				{
					base.ScreenData.ShowForgeGridPreview(m_item);
					eMode = ItemInfoPopupHelper.EMode.AddToForge;
					if (base.ScreenData.IsInForge(m_item) || base.ScreenData.IsForgeFull)
					{
						eMode |= ItemInfoPopupHelper.EMode.Disabled;
					}
				}
				else if (m_item.CanSell())
				{
					eMode |= ItemInfoPopupHelper.EMode.Sell;
				}
				InventoryElement equipedEquivalent = currentAssassin.GetEquipedEquivalent(m_item);
				ItemInfoPopupHelper.ShowInfoPopup(m_popup, currentAssassin, eMode, m_item, equipedEquivalent, _transform, null, true, 0, string.Empty);
				m_popup.MovePopupToTarget(_transform.gameObject);
			}
			else if (m_item.IsBlueprint())
			{
				Button[] array = new Button[1]
				{
					new Button
					{
						Active = true,
						Mode = Button.ButtonMode.StartForge,
						OnClick = null
					}
				};
				if (base.ScreenData.State == UIInventoryScreenData.EScreenState.Forge)
				{
					base.ScreenData.HideForgeGridPreview();
					array[0].Mode = Button.ButtonMode.AddToForge;
					array[0].Active = base.ScreenData.IsForgeValid && !base.ScreenData.IsInForge(m_item) && (!base.ScreenData.IsForgeFull || base.ScreenData.ForgeIndexBluePrint() != -1);
					array[0].OnClick = delegate(bool _active)
					{
						if (!_active && !base.ScreenData.IsForgeValid)
						{
							UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(s_noAddToForge));
						}
					};
				}
				else
				{
					array[0].Mode = Button.ButtonMode.StartForge;
					array[0].Active = UIForgeHandler.CanForge();
					array[0].OnClick = delegate(bool _active)
					{
						if (_active)
						{
							m_popup.Close();
							base.ScreenData.State = UIInventoryScreenData.EScreenState.Forge;
						}
					};
				}
				BlueprintInfoPopupHelper.ShowBlueprintInfoPopup(m_popup, m_item, array, true, _transform, string.Empty);
				m_popup.MovePopupToTarget(_transform.gameObject);
			}
			else if (m_item.IsModule())
			{
				ModuleInfoPopupHelper.ShowInfoPopup(m_popup, ModuleInfoPopupHelper.EMode.Equip, m_item, _transform, true, string.Empty);
				m_popup.MovePopupToTarget(_transform.gameObject);
			}
			else if (m_item.IsMysteryBox())
			{
				ItemInfoPopupHelper.ShowInfoPopup(m_popup, currentAssassin, ItemInfoPopupHelper.EMode.OpenMysteryBox, m_item, null, _transform, null, true, 0, string.Empty);
				m_popup.MovePopupToTarget(_transform.gameObject);
			}
			else if (m_item.IsCustomLoot())
			{
				CustomLootInfoPopupHelper.ShowInfoPopup(m_popup, m_item, true, _transform, string.Empty);
				m_popup.MovePopupToTarget(_transform.gameObject);
			}
		}
	}
}
