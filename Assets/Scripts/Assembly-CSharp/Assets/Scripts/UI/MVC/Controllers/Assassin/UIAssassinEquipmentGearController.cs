using System;
using AcierProtocol;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinEquipmentGearController : UIControllerBase
	{
		[SerializeField]
		private EInventorySlot m_equipmentSlot;

		[SerializeField]
		private UIRewardItemController m_rewardItem;

		public bool IsHighlighted
		{
			get
			{
				return m_rewardItem.IsHighlighted;
			}
			set
			{
				m_rewardItem.IsHighlighted = value;
			}
		}

		public Action<UIRewardItemController> OnClicked
		{
			get
			{
				return m_rewardItem.OnClicked;
			}
			set
			{
				m_rewardItem.OnClicked = value;
			}
		}

		public void Initialize(AcierProtocol.Assassin _assassin, bool _compareItemToCurrentAssassin)
		{
			m_rewardItem.MoveInfoPopupToTarget = false;
			InventoryElement elementForSlot = _assassin.GetElementForSlot(m_equipmentSlot);
			if (elementForSlot == null)
			{
				m_rewardItem.Initialize(GetItemSlotIcon());
			}
			else
			{
				m_rewardItem.Initialize(elementForSlot, true, _compareItemToCurrentAssassin);
			}
		}

		private string GetItemSlotIcon()
		{
			if (m_equipmentSlot != EInventorySlot.SlotModule)
			{
				ItemCategoryDescription value = null;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions.TryGetValue((int)GetItemCategoryByItemSlot(), out value))
				{
					return value.IconName;
				}
			}
			return "aUI_icon_boost";
		}

		private Item.EItemCategory GetItemCategoryByItemSlot()
		{
			switch (m_equipmentSlot)
			{
			case EInventorySlot.SlotWeapon:
				return Item.EItemCategory.Sword;
			case EInventorySlot.SlotBoots:
				return Item.EItemCategory.Boots;
			case EInventorySlot.SlotLegs:
				return Item.EItemCategory.Legs;
			case EInventorySlot.SlotChest:
				return Item.EItemCategory.Chest;
			case EInventorySlot.SlotArms:
				return Item.EItemCategory.Arms;
			case EInventorySlot.SlotShoulder:
				return Item.EItemCategory.Shoulder;
			case EInventorySlot.SlotInsignia:
				return Item.EItemCategory.Insignia;
			case EInventorySlot.SlotOutfit:
				return Item.EItemCategory.Outfit;
			default:
				return Item.EItemCategory.Sword;
			}
		}
	}
}
