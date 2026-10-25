using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class InventoryExtensions
	{
		private static bool FindInventoryResourcePredicate(ResourcePropertiesMapping.ResourcePropertiesEntry x)
		{
			return x.resourceType == EResourceType.InventorySlot;
		}

		public static InventoryElement FindItemWithId(string _id)
		{
			InventoryElement inventoryElement = Array.Find(Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.elements, (InventoryElement element) => _id == element.id);
			if (inventoryElement != null)
			{
				return inventoryElement;
			}
			return null;
		}

		public static InventoryElement FindItemWithTechnicalName(string _technicalName)
		{
			InventoryElement inventoryElement = Array.Find(Singleton<StateManager>.Instance.ProfileManager.Profile.Inventory.elements, (InventoryElement element) => _technicalName == element.technicalName);
			if (inventoryElement != null)
			{
				return inventoryElement;
			}
			return null;
		}

		public static int GetItemAmountWithId(string _id)
		{
			InventoryElement inventoryElement = FindItemWithId(_id);
			if (inventoryElement != null)
			{
				return inventoryElement.stackSize;
			}
			return 0;
		}

		public static List<InventoryElement> GetAllMysteryBoxesOfTypeAndQuality(this Inventory _inventory, EMysteryBoxType _type, Quality.EQuality _quality)
		{
			List<InventoryElement> list = new List<InventoryElement>();
			for (int i = 0; i < _inventory.elements.Length; i++)
			{
				if (_inventory.elements[i].mysteryBoxData != null && _inventory.elements[i].mysteryBoxData.mysteryBoxType == _type && _inventory.elements[i].quality == _quality)
				{
					list.Add(_inventory.elements[i]);
				}
			}
			return list;
		}

		public static string GetBoxIdOfBiggestStack(this Inventory _inventory, EMysteryBoxType _type, Quality.EQuality _quality)
		{
			List<InventoryElement> allMysteryBoxesOfTypeAndQuality = _inventory.GetAllMysteryBoxesOfTypeAndQuality(_type, _quality);
			if (allMysteryBoxesOfTypeAndQuality.Count > 1)
			{
				allMysteryBoxesOfTypeAndQuality.Sort((InventoryElement x, InventoryElement y) => x.stackSize.CompareTo(y.stackSize));
			}
			return (allMysteryBoxesOfTypeAndQuality.Count <= 0) ? string.Empty : allMysteryBoxesOfTypeAndQuality[0].id;
		}

		public static int GetAmountOfBoxesByTypeAndQuality(this Inventory _inventory, EMysteryBoxType _type, Quality.EQuality _quality)
		{
			int num = 0;
			List<InventoryElement> allMysteryBoxesOfTypeAndQuality = _inventory.GetAllMysteryBoxesOfTypeAndQuality(_type, _quality);
			for (int i = 0; i < allMysteryBoxesOfTypeAndQuality.Count; i++)
			{
				num += allMysteryBoxesOfTypeAndQuality[i].stackSize;
			}
			return num;
		}

		public static bool IsItem(this InventoryElement _element)
		{
			return _element.category == EInventoryElementCategory.InventoryItem;
		}

		public static bool IsModule(this InventoryElement _element)
		{
			return _element.category == EInventoryElementCategory.InventoryModule;
		}

		public static bool IsMysteryBox(this InventoryElement _element)
		{
			return _element.category == EInventoryElementCategory.InventoryMysteryBox;
		}

		public static bool IsCustomLoot(this InventoryElement _element)
		{
			return _element.category == EInventoryElementCategory.InventoryCustom;
		}

		public static bool IsBlueprint(this InventoryElement _element)
		{
			return _element.category == EInventoryElementCategory.InventoryBlueprint;
		}

		public static bool HasInventorySpaceForItem(this InventoryElement _element)
		{
			InventoryElement[] elements = new InventoryElement[1] { _element };
			return elements.HasInventorySpaceForItem();
		}

		public static bool HasInventorySpaceForItem(this InventoryElement[] _elements)
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			UserProfile profile = profileManager.Profile;
			int num = profileManager.InventoryFreeSpace;
			if (num < _elements.Length)
			{
				foreach (InventoryElement inventoryElement in _elements)
				{
					switch (inventoryElement.category)
					{
					case EInventoryElementCategory.InventoryItem:
						num--;
						break;
					case EInventoryElementCategory.InventoryModule:
						if (!profile.HasInventoryStackSpaceForModule(inventoryElement.moduleData.moduleType, inventoryElement.quality))
						{
							num--;
						}
						break;
					case EInventoryElementCategory.InventoryBlueprint:
						if (!profile.HasInventoryStackSpaceForBlueprint(inventoryElement.blueprintData.type))
						{
							num--;
						}
						break;
					case EInventoryElementCategory.InventoryMysteryBox:
						if (!profile.HasInventoryStackSpaceForMysteryBox(inventoryElement.mysteryBoxData.mysteryBoxType, inventoryElement.quality))
						{
							num--;
						}
						break;
					case EInventoryElementCategory.InventoryCustom:
						if (!profile.HasInventoryStackSpaceForCustomLoot(inventoryElement.customLootData.customLootType, inventoryElement.quality))
						{
							num--;
						}
						break;
					default:
						continue;
					}
					if (num < 0)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
