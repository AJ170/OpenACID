using AcierProtocol;
using Assets.Scripts.UI.State;
using Database;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class LootFilterExtensions
	{
		public static GameID GetItemDefPreview(this LootFilter _lootFilter, Quality.EQuality _quality = Quality.EQuality.Mundane)
		{
			GameID result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownItem;
			if (_lootFilter.IsUniqeWeaponType())
			{
				switch (_lootFilter.lootTypes.itemCategories[0])
				{
				case Item.EItemCategory.Sword:
				case Item.EItemCategory.Blunt:
				case Item.EItemCategory.Dagger:
				case Item.EItemCategory.Polearm:
				case Item.EItemCategory.TwoHanded:
				case Item.EItemCategory.DualWield:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownWeapon;
					break;
				case Item.EItemCategory.Boots:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownBoot;
					break;
				case Item.EItemCategory.Legs:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownLegs;
					break;
				case Item.EItemCategory.Chest:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownChest;
					break;
				case Item.EItemCategory.Arms:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownHiddenblade;
					break;
				case Item.EItemCategory.Shoulder:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownShoulder;
					break;
				case Item.EItemCategory.Insignia:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownInsignia;
					break;
				case Item.EItemCategory.Outfit:
					result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnknownOutfit;
					break;
				}
			}
			else if (_lootFilter.lootTypes.mysteryBoxTypes.Length > 0)
			{
				switch (_lootFilter.lootTypes.mysteryBoxTypes[0])
				{
				case EMysteryBoxType.MysteryBoxItemType:
					switch (_quality)
					{
					case Quality.EQuality.Mundane:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxItemsMundane;
						break;
					case Quality.EQuality.Common:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxItemsCommon;
						break;
					case Quality.EQuality.Uncommon:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxItemsUncommon;
						break;
					case Quality.EQuality.Rare:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxItemsRare;
						break;
					case Quality.EQuality.Perfect:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxItemsPerfect;
						break;
					}
					break;
				case EMysteryBoxType.MysteryBoxRessourceType:
					switch (_quality)
					{
					case Quality.EQuality.Mundane:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxCurrencyMundane;
						break;
					case Quality.EQuality.Common:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxCurrencyCommon;
						break;
					case Quality.EQuality.Uncommon:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxCurrencyUncommon;
						break;
					case Quality.EQuality.Rare:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxCurrencyRare;
						break;
					case Quality.EQuality.Perfect:
						result = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.MysterBoxCurrencyPerfect;
						break;
					}
					break;
				}
			}
			return result;
		}

		public static bool IsUniqeResourceType(this LootFilter _lootFilter)
		{
			LootTypes lootTypes = _lootFilter.lootTypes;
			return lootTypes.itemCategories.Length == 0 && lootTypes.blueprintTypes.Length == 0 && lootTypes.moduleTypes.Length == 0 && lootTypes.mysteryBoxTypes.Length == 0 && lootTypes.resourceTypes.Length == 1;
		}

		public static bool IsUniqeWeaponType(this LootFilter _lootFilter)
		{
			LootTypes lootTypes = _lootFilter.lootTypes;
			return lootTypes.itemCategories.Length == 1 && lootTypes.blueprintTypes.Length == 0 && lootTypes.moduleTypes.Length == 0 && lootTypes.mysteryBoxTypes.Length == 0 && lootTypes.resourceTypes.Length == 0;
		}

		public static Quality.EQuality GetMinQualityForHeroicRank(this TemplateManager _templateManager, int _rankIdx, bool _isElite)
		{
			Quality.EQuality eQuality = Quality.EQuality.Perfect;
			for (int i = 0; i < _templateManager.MissionLootScalingMapping.entries.Length; i++)
			{
				MissionLootScalingMapping.MissionFlavourEntry missionFlavourEntry = _templateManager.MissionLootScalingMapping.entries[i];
				MissionLootScalingMapping.MissionFlavourEntry.RarityWeighting rarityWeighting = ((!_isElite) ? missionFlavourEntry.rarityWeighting : missionFlavourEntry.rarityWeightingElite);
				if (missionFlavourEntry == null)
				{
					continue;
				}
				for (int j = 0; j < rarityWeighting.entries.Length; j++)
				{
					MissionLootScalingMapping.MissionFlavourEntry.RarityWeighting.Entry entry = rarityWeighting.entries[j];
					if (entry != null && !(entry.weight <= 0f) && entry.rankIdx == _rankIdx && entry.quality < eQuality)
					{
						eQuality = entry.quality;
					}
				}
			}
			return eQuality;
		}

		public static Item.EItemCategory GetItemCategory(this LootFilter _lootFilter)
		{
			Item.EItemCategory result = Item.EItemCategory.None;
			if (_lootFilter.lootTypes != null && _lootFilter.IsUniqeWeaponType() && _lootFilter.lootTypes.itemCategories.Length > 0)
			{
				result = _lootFilter.lootTypes.itemCategories[0];
			}
			return result;
		}
	}
}
