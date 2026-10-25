using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.ModuleScreen;
using Assets.Scripts.UI.State;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class ItemExtensions
	{
		public class ModificationComparison
		{
			public StringId NameId;

			public float Value;

			public float Diff;

			public StatTypes.EStatType StatType;

			public StatTypes.EStatUnit StatUnit;

			public EModuleModifier ModuleModifierType;
		}

		private static int[] s_visualChangingItems = new int[2] { 2, 14 };

		public static bool AltersVisuals(Item.EItemCategory _category, InventoryElement _newItem, InventoryElement _oldItem)
		{
			if (_category == Item.EItemCategory.Insignia)
			{
				if (_newItem != null && _newItem.IsHeadAccessoryItem())
				{
					return true;
				}
				if (_oldItem != null && _oldItem.IsHeadAccessoryItem())
				{
					return true;
				}
			}
			else
			{
				for (int i = 0; i < s_visualChangingItems.Length; i++)
				{
					if (s_visualChangingItems[i] == (int)_category)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static string GetTitle(this InventoryElement _element)
		{
			return LocalizationManager.Instance.Get(_element.langKey.neutral, _element.GetTitleStringMapper());
		}

		public static StringMapper GetTitleStringMapper(this InventoryElement _element)
		{
			StringMapper stringMapper = new StringMapper();
			if (_element.moduleData != null && _element.moduleData.modifications != null && _element.moduleData.modifications.Length > 0)
			{
				Modification modification = _element.moduleData.modifications[0];
				stringMapper.Add("modules_1_skill_1_factor", modification.effectValue);
			}
			return stringMapper;
		}

		public static Quality.EQuality GetItemDisplayQuality(this InventoryElement _element)
		{
			Quality.EQuality eQuality = _element.quality;
			if (_element.category == EInventoryElementCategory.InventoryModule)
			{
				ModuleQualityMapping moduleQualityMapping = Singleton<StateManager>.Instance.TemplateManager.Globals.moduleQualityMapping;
				eQuality = moduleQualityMapping.GetModuleDisplayItemQuality(eQuality);
			}
			return eQuality;
		}

		public static bool Equivalent(this InventoryElement _element, InventoryElement _other)
		{
			if (_element == null && _other == null)
			{
				return true;
			}
			if (_element == null || _other == null)
			{
				return false;
			}
			if (_element.category != _other.category)
			{
				return false;
			}
			switch (_element.category)
			{
			case EInventoryElementCategory.InventoryItem:
				return _element.itemData.category == _other.itemData.category;
			case EInventoryElementCategory.InventoryModule:
				return _element.moduleData.IsModuleEqual(_other.moduleData);
			case EInventoryElementCategory.InventoryBlueprint:
				return _element.blueprintData.type == _other.blueprintData.type;
			case EInventoryElementCategory.InventoryMysteryBox:
				return _element.mysteryBoxData.mysteryBoxType == _other.mysteryBoxData.mysteryBoxType;
			case EInventoryElementCategory.InventoryCustom:
				return _element.customLootData.customLootType == _other.customLootData.customLootType;
			default:
				return false;
			}
		}

		public static bool IsHeadAccessoryItem(this InventoryElement _element)
		{
			GameID _gameId;
			if (_element != null && Main.Instance.GameDB.TryFindGameIdFromGuid(_element.guid, out _gameId))
			{
				ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(_gameId);
				if (!string.IsNullOrEmpty(item.ItemPrefab.GetResourcePath()))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsItemEquippable(this InventoryElement _element, Assassin _assassin)
		{
			if (_element == null || _assassin == null)
			{
				return false;
			}
			if (_element.IsItem())
			{
				return _element.isEquipable && _element.IsAssassinLevelReachedForItem(_assassin);
			}
			return _element.isEquipable;
		}

		public static bool IsAssassinLevelReachedForItem(this InventoryElement _element, Assassin _assassin)
		{
			return _element.itemData.GetEquippableItemLevel() <= _assassin.level;
		}

		public static bool IsValidInUI(this Item.EItemCategory _category)
		{
			return _category > Item.EItemCategory.None;
		}

		public static bool IsValidAsFilter(this Item.EItemCategory _category)
		{
			return _category >= Item.EItemCategory.None;
		}

		public static bool CanBeForged(this InventoryElement _element)
		{
			return _element.IsItem() || _element.IsBlueprint();
		}

		public static bool CanStartForge(this InventoryElement _element)
		{
			return _element.IsItem();
		}

		public static bool VisibleForForge(this InventoryElement _element)
		{
			return _element.CanBeForged() || _element.IsBlueprint();
		}

		public static int CompareTo(this InventoryElement _item, InventoryElement _other)
		{
			if (_other == null)
			{
				return 1;
			}
			if (!_item.IsItem() || !_other.IsItem())
			{
				return 0;
			}
			return _item.itemData.CalculateItemPower().CompareTo(_other.itemData.CalculateItemPower());
		}

		public static bool CanEquipItem(this InventoryElement _element, Assassin _assassin)
		{
			if (_element.IsItemEquippable(_assassin) && _element.EntitledToEquip() && (!Singleton<StateManager>.Instance.ProfileManager.HasOverflowOfItemsInInventory || _assassin.GetEquipedEquivalent(_element) == null))
			{
				return true;
			}
			return false;
		}

		public static bool CanSell(this InventoryElement _item)
		{
			return _item.IsItem();
		}

		public static Resource SellValue(this InventoryElement _item)
		{
			if (!_item.CanSell())
			{
				return null;
			}
			ItemSellMapping itemSellMapping = Singleton<StateManager>.Instance.TemplateManager.ItemSellMapping;
			ItemSellMapping.QualityMapping qualityMapping = itemSellMapping.qualityMapping.Find((ItemSellMapping.QualityMapping _x) => _x.quality == _item.quality);
			ItemSellMapping.QualityMapping.LevelMapping levelMapping = qualityMapping.levelMappings.Find((ItemSellMapping.QualityMapping.LevelMapping _x) => _x.level == _item.itemData.level);
			Resource resource = new Resource();
			resource.amount = levelMapping.amount;
			resource.type = EResourceType.SoftCurrency;
			return resource;
		}

		public static bool EntitledToEquip(this InventoryElement _item)
		{
			if (_item.IsModule())
			{
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotModuleEntitlementGUID);
			}
			return !_item.IsItem() || _item.itemData.category.EntitledToEquip();
		}

		public static bool EntitledToEquip(this Item.EItemCategory _category)
		{
			switch (_category)
			{
			case Item.EItemCategory.Insignia:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID);
			case Item.EItemCategory.Outfit:
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID);
			default:
				return true;
			}
		}

		public static bool WasEntitledToEquipShown(this Item.EItemCategory _category)
		{
			switch (_category)
			{
			case Item.EItemCategory.Insignia:
				return UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID);
			case Item.EItemCategory.Outfit:
				return UbiCore.Utils.Singleton<LocalStorage>.Instance.WasEntitlementShown(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID);
			default:
				return true;
			}
		}

		public static StringId MissingEntitlementPopupMessage(this InventoryElement _item)
		{
			if (_item.IsModule())
			{
				return new StringId(7197);
			}
			if (_item.IsItem())
			{
				return _item.itemData.category.MissingEntitlementPopupMessage();
			}
			return StringId.Invalid;
		}

		public static StringId MissingEntitlementPopupMessage(this Item.EItemCategory _category)
		{
			ItemCategoryDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions.TryGetValue((int)_category, out value))
			{
				return value.MissingEntitlementMessage;
			}
			return StringId.Invalid;
		}

		public static List<ModificationComparison> CompareWith(List<Modification> _itemMods, List<Modification> _otherItemMods)
		{
			List<ModificationComparison> list = new List<ModificationComparison>(_itemMods.Count + ((_otherItemMods != null) ? _otherItemMods.Count : 0));
			list.AddRange(CompareList(_itemMods, _otherItemMods, list, false));
			if (_otherItemMods != null)
			{
				list.AddRange(CompareList(_otherItemMods, _itemMods, list, true));
			}
			list.Sort((ModificationComparison l, ModificationComparison r) => l.StatType.CompareTo(r.StatType));
			return list;
		}

		public static int CalculateItemPower(this ItemData _item)
		{
			int num = 0;
			float num2 = 0f;
			Globals globals = Singleton<StateManager>.Instance.TemplateManager.Globals;
			if (_item.category.IsWeaponCategory())
			{
				num2 += globals.itemModWeightWeaponDamage * (float)_item.baseValue;
			}
			else if (_item.category.IsArmorCategory())
			{
				num2 += globals.itemModWeightArmorRating * (float)_item.baseValue;
			}
			float num3 = 0f;
			for (int i = 0; i < _item.modifications.Length; i++)
			{
				string skillGuid = _item.modifications[i].skillGuid;
				if (string.IsNullOrEmpty(skillGuid) || skillGuid.StartsWith("_ColorVariation"))
				{
					continue;
				}
				GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(skillGuid);
				if (!(gameID == GameID.Invalid))
				{
					Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition item = Main.Instance.GameDB.GetItem<Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition>(gameID);
					Assets.Scripts.Mission.Definitions.SkillData.ISkillDefinition definition = item.Name.ToEnum<SkillTypes>().GetDefinition();
					float num4 = definition.RuleDefinition.Get_float(StatTypes.EStatType.Rule_ItemModWeight);
					if (_item.modifications[i].plainItemPower)
					{
						num3 += (float)_item.modifications[i].effectValue * num4;
					}
					else
					{
						num2 += (float)_item.modifications[i].effectValue * num4;
					}
				}
			}
			return Mathf.CeilToInt(Mathf.Pow(num2, globals.itemPowerExponential) * globals.itemPowerFactor + num3);
		}

		public static int GetEquippableItemLevel(this ItemData _data)
		{
			int num = _data.level;
			string skillGuid = Singleton<StateManager>.Instance.TemplateManager.Globals.itemLevelReductionSkillGuid;
			Modification modification = _data.modifications.Find((Modification x) => x.skillGuid == skillGuid);
			if (modification != null)
			{
				num -= modification.effectValue;
			}
			return num;
		}

		private static IEnumerable<ModificationComparison> CompareList(List<Modification> _lhs, List<Modification> _rhs, List<ModificationComparison> _result, bool _twisted)
		{
			List<ModificationComparison> list = new List<ModificationComparison>(_lhs.Count);
			for (int i = 0; i < _lhs.Count; i++)
			{
				Modification modification = _lhs[i];
				if (!modification.skillGuid.StartsWith("_ColorVariation") && !(modification.skillGuid == Singleton<StateManager>.Instance.TemplateManager.Globals.itemLevelReductionSkillGuid) && !(modification.skillGuid == Singleton<StateManager>.Instance.TemplateManager.Globals.itemPowerBonusSkillGuid))
				{
					ModificationComparison modificationComparison = null;
					modificationComparison = ((modification.moduleModifier != 0) ? CompareOtherModifier(modification, _rhs, _result, _twisted) : CompareClientModifier(modification, _rhs, _result, _twisted));
					if (modificationComparison != null)
					{
						list.Add(modificationComparison);
					}
				}
			}
			return list;
		}

		private static ModificationComparison CompareClientModifier(Modification _leftMod, List<Modification> _rhs, List<ModificationComparison> _result, bool _twisted)
		{
			GameDB gameDB = Main.Instance.GameDB;
			GameID gameID = gameDB.FindGameIdFromGuid(_leftMod.skillGuid);
			Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition item = gameDB.GetItem<Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition>(gameID);
			Modification modification = ((_rhs != null) ? _rhs.Find((Modification x) => x.skillGuid == _leftMod.skillGuid) : null);
			Assets.Scripts.Mission.Definitions.SkillData instance = UbiCore.Utils.Singleton<Assets.Scripts.Mission.Definitions.SkillData>.Instance;
			Assets.Scripts.Mission.Definitions.SkillData.SkillPreview skillPreview = instance.GetSkillPreview(_leftMod.skillGuid, _leftMod.effectValue, false);
			Assets.Scripts.Mission.Definitions.SkillData.SkillPreview skillPreview2 = ((modification == null) ? null : instance.GetSkillPreview(modification.skillGuid, modification.effectValue, false));
			if (_twisted && skillPreview != null && skillPreview2 != null)
			{
				return null;
			}
			StringId key = _leftMod.BuildUnitRawKey();
			if (_result.Find((ModificationComparison x) => x.NameId == key) == null)
			{
				ModificationComparison modificationComparison = new ModificationComparison();
				modificationComparison.NameId = key;
				modificationComparison.Value = ((!_twisted && skillPreview != null) ? skillPreview.Magnitude : 0f);
				modificationComparison.Diff = ((_rhs == null) ? 0f : ((!_twisted) ? skillPreview.CompareMagnitude(skillPreview2) : (0f - skillPreview.Magnitude)));
				modificationComparison.StatType = ((skillPreview != null) ? skillPreview.StatType : skillPreview2.StatType);
				modificationComparison.StatUnit = StatTypes.GetStatUnit((skillPreview != null) ? skillPreview.RuleStatType : skillPreview2.RuleStatType);
				modificationComparison.ModuleModifierType = _leftMod.moduleModifier;
				return modificationComparison;
			}
			return null;
		}

		private static ModificationComparison CompareOtherModifier(Modification _leftMod, List<Modification> _rhs, List<ModificationComparison> _result, bool _twisted)
		{
			StringId key = _leftMod.BuildUnitRawKey();
			if (_result.Find((ModificationComparison x) => x.NameId == key) == null)
			{
				Modification modification = ((_rhs != null) ? _rhs.Find((Modification _mod) => _mod.moduleModifier == _leftMod.moduleModifier) : null);
				int num = _leftMod.effectValue;
				if (modification != null)
				{
					num = _leftMod.effectValue - modification.effectValue;
				}
				ModificationComparison modificationComparison = new ModificationComparison();
				modificationComparison.NameId = key;
				modificationComparison.Value = ((!_twisted) ? _leftMod.effectValue : 0);
				modificationComparison.Diff = ((_rhs != null) ? ((!_twisted) ? num : (-_leftMod.effectValue)) : 0);
				modificationComparison.StatUnit = StatTypes.EStatUnit.Chance;
				modificationComparison.ModuleModifierType = _leftMod.moduleModifier;
				return modificationComparison;
			}
			return null;
		}

		public static Modification Clone(this LootTemplate.ItemTemplate.ModStaticTemplate _mod)
		{
			Modification modification = new Modification();
			modification.effectValue = _mod.maxValue;
			modification.skillGuid = _mod.skillGuid;
			modification.moduleModifier = _mod.moduleModifier;
			modification.plainItemPower = _mod.plainItemPower;
			return modification;
		}

		public static Modification Clone(this Modification _mod)
		{
			Modification modification = new Modification();
			modification.effectValue = _mod.effectValue;
			modification.skillGuid = _mod.skillGuid;
			modification.moduleModifier = _mod.moduleModifier;
			modification.plainItemPower = _mod.plainItemPower;
			return modification;
		}

		public static List<Modification> UniqueModifiers(this ItemData _item, ref int _baseValue)
		{
			List<Modification> list = new List<Modification>(_item.modifications.Length);
			bool flag = _item.category.IsWeaponCategory();
			bool flag2 = _item.category.IsArmorCategory();
			for (int i = 0; i < _item.modifications.Length; i++)
			{
				Modification modification = list.Find((Modification x) => x.skillGuid == _item.modifications[i].skillGuid);
				if (modification == null)
				{
					if (flag && _item.modifications[i].skillGuid == GlobalGameIDs.WeaponDamageBonusSkillGuidStr)
					{
						_baseValue += _item.modifications[i].effectValue;
					}
					else if (flag2 && _item.modifications[i].skillGuid == GlobalGameIDs.ArmorRatingBonusSkillGuidStr)
					{
						_baseValue += _item.modifications[i].effectValue;
					}
					else
					{
						list.Add(_item.modifications[i].Clone());
					}
				}
				else
				{
					modification.effectValue += _item.modifications[i].effectValue;
				}
			}
			return list;
		}

		public static int CompareItem(this InventoryElement _itemA, InventoryElement _itemB)
		{
			if (_itemB.IsItem() && _itemB.itemData.level != _itemA.itemData.level)
			{
				return _itemB.itemData.level.CompareTo(_itemA.itemData.level);
			}
			if (_itemA.category != _itemB.category)
			{
				return _itemA.category.CompareTo(_itemB.category);
			}
			if (_itemB.quality != _itemA.quality)
			{
				return _itemB.quality.CompareTo(_itemA.quality);
			}
			if (_itemB.IsItem() && _itemB.itemData.baseValue != _itemA.itemData.baseValue)
			{
				return _itemB.itemData.baseValue.CompareTo(_itemA.itemData.baseValue);
			}
			if (_itemA.langKey.neutral != _itemB.langKey.neutral)
			{
				return _itemB.langKey.neutral.CompareTo(_itemA.langKey.neutral);
			}
			return 0;
		}

		public static bool IsModuleEqual(this ModuleData _itemA, ModuleData _itemB)
		{
			return _itemA != null && _itemB != null && _itemA.moduleType == _itemB.moduleType && _itemA.duration == _itemB.duration && _itemA.durationType == _itemB.durationType && _itemA.expireTimeStamp == _itemB.expireTimeStamp;
		}

		public static LootTemplate GetFirstItemTemplateWrapper(this LootTemplate[] _lootTemplates)
		{
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].itemTemplate != null)
				{
					return _lootTemplates[i];
				}
			}
			return null;
		}

		public static LootTemplate.ItemTemplate GetFirstItemTemplate(this LootTemplate[] _lootTemplates)
		{
			LootTemplate firstItemTemplateWrapper = _lootTemplates.GetFirstItemTemplateWrapper();
			return (firstItemTemplateWrapper != null) ? firstItemTemplateWrapper.itemTemplate : null;
		}

		public static int DoForItemTemplates(this LootTemplate[] _lootTemplates, Action<LootTemplate.ItemTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].itemTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i].itemTemplate);
					}
					num++;
				}
			}
			return num;
		}

		public static int DoForMysteryBoxTemplateWrappers(this LootTemplate[] _lootTemplates, Action<LootTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].mysteryBoxTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i]);
					}
					num++;
				}
			}
			return num;
		}

		public static int DoForMysteryBoxTemplates(this LootTemplate[] _lootTemplates, Action<LootTemplate.MysteryBoxTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].mysteryBoxTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i].mysteryBoxTemplate);
					}
					num++;
				}
			}
			return num;
		}

		public static int DoForItemTemplateWrappers(this LootTemplate[] _lootTemplates, Action<LootTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].itemTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i]);
					}
					num++;
				}
			}
			return num;
		}

		public static LootTemplate GetFirstResourceTemplateWrapper(this LootTemplate[] _lootTemplates)
		{
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].resourceTemplate != null)
				{
					return _lootTemplates[i];
				}
			}
			return null;
		}

		public static LootTemplate.ResourceTemplate GetFirstResourceTemplate(this LootTemplate[] _lootTemplates)
		{
			LootTemplate firstResourceTemplateWrapper = _lootTemplates.GetFirstResourceTemplateWrapper();
			return (firstResourceTemplateWrapper != null) ? firstResourceTemplateWrapper.resourceTemplate : null;
		}

		public static int DoForResourceTemplates(this LootTemplate[] _lootTemplates, Action<LootTemplate.ResourceTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].resourceTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i].resourceTemplate);
					}
					num++;
				}
			}
			return num;
		}

		public static int DoForResourceTemplateWrappers(this LootTemplate[] _lootTemplates, Action<LootTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].resourceTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i]);
					}
					num++;
				}
			}
			return num;
		}

		public static LootTemplate GetFirstModuleTemplateWrapper(this LootTemplate[] _lootTemplates)
		{
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].moduleTemplate != null)
				{
					return _lootTemplates[i];
				}
			}
			return null;
		}

		public static LootTemplate GetFirstMysteryBoxTemplateWrapper(this LootTemplate[] _lootTemplates)
		{
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].mysteryBoxTemplate != null)
				{
					return _lootTemplates[i];
				}
			}
			return null;
		}

		public static LootTemplate.ModuleTemplate GetFirstModuleTemplate(this LootTemplate[] _lootTemplates)
		{
			LootTemplate firstModuleTemplateWrapper = _lootTemplates.GetFirstModuleTemplateWrapper();
			return (firstModuleTemplateWrapper != null) ? firstModuleTemplateWrapper.moduleTemplate : null;
		}

		public static LootTemplate GetFirstMysteryBoxTemplate(this LootTemplate[] _lootTemplates)
		{
			LootTemplate firstMysteryBoxTemplateWrapper = _lootTemplates.GetFirstMysteryBoxTemplateWrapper();
			return (firstMysteryBoxTemplateWrapper != null) ? firstMysteryBoxTemplateWrapper : null;
		}

		public static int DoForModuleTemplates(this LootTemplate[] _lootTemplates, Action<LootTemplate.ModuleTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].moduleTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i].moduleTemplate);
					}
					num++;
				}
			}
			return num;
		}

		public static int DoForModuleTemplateWrappers(this LootTemplate[] _lootTemplates, Action<LootTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].moduleTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i]);
					}
					num++;
				}
			}
			return num;
		}

		public static int DoForEntitlementTemplateWrappers(this LootTemplate[] _lootTemplates, Action<LootTemplate> _action)
		{
			int num = 0;
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].entitlementTemplate != null)
				{
					if (_action != null)
					{
						_action(_lootTemplates[i]);
					}
					num++;
				}
			}
			return num;
		}

		public static LootTemplate GetFirstBlueprintTemplateWrapper(this LootTemplate[] _lootTemplates)
		{
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				if (_lootTemplates[i].blueprintTemplate != null)
				{
					return _lootTemplates[i];
				}
			}
			return null;
		}

		public static LootTemplate GetFirstBlueprintTemplate(this LootTemplate[] _lootTemplates)
		{
			return _lootTemplates.GetFirstBlueprintTemplateWrapper();
		}

		public static void FillLootTemplateStringBuilder(this LootTemplate[] _lootTemplates, ref StringMapper _stringMapper)
		{
			for (int i = 0; i < _lootTemplates.Length; i++)
			{
				_lootTemplates[i].FillLootTemplatetringBuilder(ref _stringMapper);
			}
		}

		public static void FillLootTemplatetringBuilder(this LootTemplate _lootTemplates, ref StringMapper _stringMapper)
		{
			if (_lootTemplates.moduleTemplate != null)
			{
				LootTemplate.ModuleTemplate moduleTemplate = _lootTemplates.moduleTemplate;
				if (moduleTemplate != null && moduleTemplate.skills.Length > 0)
				{
					_stringMapper.Add("modules_1_skill_1_factor", moduleTemplate.skills[0].effectValue);
				}
			}
		}

		public static bool IsWeaponCategory(this Item.EItemCategory _itemCategory)
		{
			switch (_itemCategory)
			{
			case Item.EItemCategory.Sword:
			case Item.EItemCategory.Blunt:
			case Item.EItemCategory.Dagger:
			case Item.EItemCategory.Polearm:
			case Item.EItemCategory.TwoHanded:
			case Item.EItemCategory.DualWield:
				return true;
			default:
				return false;
			}
		}

		public static bool IsArmorCategory(this Item.EItemCategory _itemCategory)
		{
			if (_itemCategory == Item.EItemCategory.None)
			{
				return false;
			}
			return !_itemCategory.IsWeaponCategory() && _itemCategory != Item.EItemCategory.Outfit;
		}

		public static bool IsMysteryBox(this LootTemplate _template)
		{
			return _template.mysteryBoxTemplate != null;
		}

		public static bool IsMysteryBox(this LootTemplate[] _templates, bool _isSingleItem = true)
		{
			if (_isSingleItem)
			{
				return _templates.Length == 1 && _templates[0].IsMysteryBox();
			}
			for (int i = 0; i < _templates.Length; i++)
			{
				if (_templates[i].mysteryBoxTemplate != null)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsBluePrint(this LootTemplate _template)
		{
			return _template.blueprintTemplate != null;
		}

		public static bool IsBluePrint(this LootTemplate[] _templates, bool _isSingleItem = true)
		{
			if (_isSingleItem)
			{
				return _templates.Length == 1 && _templates[0].IsBluePrint();
			}
			for (int i = 0; i < _templates.Length; i++)
			{
				if (_templates[i].blueprintTemplate != null)
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsCodexEntitlement(this LootTemplate _template)
		{
			if (_template.entitlementTemplate != null)
			{
				return Singleton<StateManager>.Instance.CodexManager.GetCodexDefinitionByEntitlementGuid(_template.entitlementTemplate.gameIdGuid) != null;
			}
			return false;
		}

		public static bool IsCustomItemType(this LootTemplate _template, ECustomLootTemplateType _type)
		{
			return _template.customTemplate != null && _template.customTemplate.type == ECustomLootTemplateType.LockPick;
		}

		public static bool IsCustomItemType(this LootTemplate[] _templates, ECustomLootTemplateType _type, bool _isSingleItem = true)
		{
			if (_isSingleItem)
			{
				return _templates.Length == 1 && _templates[0].customTemplate != null && _templates[0].customTemplate.type == _type;
			}
			for (int i = 0; i < _templates.Length; i++)
			{
				if (_templates[i].customTemplate != null && _templates[i].customTemplate.type == _type)
				{
					return true;
				}
			}
			return false;
		}

		public static InventoryElement GetItemFromLootTemplate(this LootTemplate _template, int _amountMultiplier = 1)
		{
			int neutral = -1;
			if (_template.descKeys.Length > 0)
			{
				neutral = _template.descKeys[0];
			}
			int neutral2 = -1;
			if (_template.nameKeys.Length > 0)
			{
				neutral2 = _template.nameKeys[0];
			}
			InventoryElement inventoryElement = new InventoryElement();
			inventoryElement.createdAt = 0L;
			inventoryElement.debugRollCode = "FakeItem";
			inventoryElement.descriptionlangKey = new LangKey
			{
				neutral = neutral
			};
			inventoryElement.guid = _template.guid;
			inventoryElement.id = string.Empty;
			inventoryElement.langKey = new LangKey
			{
				neutral = neutral2
			};
			inventoryElement.quality = _template.quality;
			inventoryElement.stackSize = _template.GetItemStackCount() * _amountMultiplier;
			inventoryElement.templateName = string.Empty;
			inventoryElement.itemData = null;
			inventoryElement.moduleData = null;
			inventoryElement.mysteryBoxData = null;
			inventoryElement.blueprintData = null;
			InventoryElement inventoryElement2 = inventoryElement;
			inventoryElement2.CopyItemModificationsToItem(_template.itemTemplate, _template.level);
			inventoryElement2.CopyModuleSkillToModule(_template.moduleTemplate);
			inventoryElement2.CopyMysteryBox(_template.mysteryBoxTemplate);
			inventoryElement2.CopyCustomLoot(_template.customTemplate);
			inventoryElement2.CopyBlueprint(_template.blueprintTemplate);
			return inventoryElement2;
		}

		private static int GetItemStackCount(this LootTemplate _template)
		{
			int num = 1;
			if (_template.blueprintTemplate != null)
			{
				return (int)_template.blueprintTemplate.maxAmount;
			}
			if (_template.mysteryBoxTemplate != null)
			{
				return (int)_template.mysteryBoxTemplate.maxAmount;
			}
			if (_template.moduleTemplate != null)
			{
				return (int)_template.moduleTemplate.maxAmount;
			}
			if (_template.customTemplate != null)
			{
				return (int)_template.customTemplate.maxAmount;
			}
			return 1;
		}

		private static void CopyItemModificationsToItem(this InventoryElement _item, LootTemplate.ItemTemplate _itemTemplate, int _level)
		{
			if (_itemTemplate != null)
			{
				_item.category = EInventoryElementCategory.InventoryItem;
				_item.itemData = new ItemData
				{
					baseValue = _itemTemplate.maxBaseStatValue,
					level = _level,
					category = _itemTemplate.category,
					modifications = new Modification[_itemTemplate.presetMods.Length]
				};
				for (int i = 0; i < _itemTemplate.presetMods.Length; i++)
				{
					LootTemplate.ItemTemplate.ModStaticTemplate mod = _itemTemplate.presetMods[i];
					_item.itemData.modifications[i] = mod.Clone();
				}
			}
		}

		private static void CopyModuleSkillToModule(this InventoryElement _module, LootTemplate.ModuleTemplate _moduleTemplate)
		{
			if (_moduleTemplate != null)
			{
				_module.category = EInventoryElementCategory.InventoryModule;
				_module.moduleData = new ModuleData
				{
					duration = _moduleTemplate.duration,
					durationType = _moduleTemplate.durationType,
					expireTimeStamp = 0L,
					moduleType = _moduleTemplate.type,
					modifications = new Modification[_moduleTemplate.skills.Length]
				};
				for (int i = 0; i < _moduleTemplate.skills.Length; i++)
				{
					Modification mod = _moduleTemplate.skills[i];
					_module.moduleData.modifications[i] = mod.Clone();
				}
			}
		}

		private static void CopyMysteryBox(this InventoryElement _box, LootTemplate.MysteryBoxTemplate _boxTemplate)
		{
			if (_boxTemplate != null)
			{
				_box.category = EInventoryElementCategory.InventoryMysteryBox;
				_box.mysteryBoxData = new MysteryBoxData
				{
					mysteryBoxType = _boxTemplate.type
				};
			}
		}

		private static void CopyCustomLoot(this InventoryElement _box, LootTemplate.CustomTemplate _customTemplate)
		{
			if (_customTemplate != null)
			{
				_box.category = EInventoryElementCategory.InventoryCustom;
				_box.customLootData = new CustomLootData
				{
					customLootType = _customTemplate.type
				};
			}
		}

		private static void CopyBlueprint(this InventoryElement _blueprint, LootTemplate.BlueprintTemplate _blueprintTemplate)
		{
			if (_blueprintTemplate != null)
			{
				_blueprint.category = EInventoryElementCategory.InventoryBlueprint;
				_blueprint.blueprintData = new BlueprintData
				{
					type = _blueprintTemplate.type
				};
			}
		}

		public static GameID GetEquipSoundGameIDByItemType(this InventoryElement _item)
		{
			switch (_item.category)
			{
			case EInventoryElementCategory.InventoryItem:
				switch (_item.itemData.category)
				{
				case Item.EItemCategory.Sword:
				case Item.EItemCategory.Blunt:
				case Item.EItemCategory.Dagger:
				case Item.EItemCategory.Polearm:
				case Item.EItemCategory.TwoHanded:
				case Item.EItemCategory.DualWield:
					return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_weapon;
				case Item.EItemCategory.Insignia:
					if (_item.IsHeadAccessoryItem())
					{
						return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_bf_inventory_equip_piratestuff;
					}
					return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_insignia;
				case Item.EItemCategory.Chest:
				case Item.EItemCategory.Arms:
				case Item.EItemCategory.Shoulder:
					return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_armour;
				case Item.EItemCategory.Boots:
				case Item.EItemCategory.Legs:
				case Item.EItemCategory.Outfit:
					return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_cloth;
				default:
					return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_equip;
				}
			case EInventoryElementCategory.InventoryModule:
				return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_booster;
			case EInventoryElementCategory.InventoryBlueprint:
				return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_inventory_equip_blueprints;
			default:
				return UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_equip;
			}
		}

		public static LootFilter[] GetLootFiltersForContractMissionKind(this EMissionKind missionKind)
		{
			MissionTypeLootMapping.Entry[] entries = Singleton<StateManager>.Instance.TemplateManager.MissionTypeLootMapping.entries;
			foreach (MissionTypeLootMapping.Entry entry in entries)
			{
				if (entry.missionKind == missionKind)
				{
					return entry.lootFilters;
				}
			}
			return null;
		}
	}
}
