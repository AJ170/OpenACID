using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Misc
{
	public class ItemStatsListHandler : InfoStatsListHandler
	{
		[SerializeField]
		private UIExtendedLabel m_specialOutfitDescription;

		private UIWidget m_gridSpacer;

		protected override void Awake()
		{
			base.Awake();
			GameObject gameObject = NGUITools.AddChild(m_scrollTable.gameObject);
			m_gridSpacer = gameObject.AddComponent<UIWidget>();
			m_gridSpacer.SetDimensions(10, 10);
			m_gridSpacer.leftAnchor.Set(base.transform, 0f, 0f);
			m_gridSpacer.rightAnchor.Set(base.transform, 1f, 0f);
		}

		public void Init(ItemData _item, ItemData _other = null, Assassin _assassin = null)
		{
			if (m_gameObjectCache == null)
			{
				m_gameObjectCache = new GameObjectCache(m_statItemPrefab, m_scrollTable.transform);
			}
			m_boxCollider.enabled = false;
			ReleaseEntries();
			int _baseValue = _item.baseValue;
			int _baseValue2 = ((_other == null) ? (-1) : _other.baseValue);
			List<Modification> itemMods = _item.UniqueModifiers(ref _baseValue);
			List<Modification> otherItemMods = ((_other != null) ? _other.UniqueModifiers(ref _baseValue2) : null);
			List<ItemExtensions.ModificationComparison> list = ItemExtensions.CompareWith(itemMods, otherItemMods);
			m_specialOutfitDescription.gameObject.SetActive(false);
			ModifierValueHandler modifierValueHandler = null;
			int num = 0;
			int equippableItemLevel = _item.GetEquippableItemLevel();
			int currentLevel = ((_assassin == null) ? equippableItemLevel : _assassin.level);
			modifierValueHandler = GetCachedGameObject<ModifierValueHandler>(num++);
			modifierValueHandler.InitLevel(_item.level, LocalizationManager.Instance.Get(5264), equippableItemLevel, currentLevel);
			if (equippableItemLevel != _item.level)
			{
				modifierValueHandler = GetCachedGameObject<ModifierValueHandler>(num++);
				modifierValueHandler.InitLevel(equippableItemLevel, LocalizationManager.Instance.Get(7719), equippableItemLevel, currentLevel);
			}
			m_gridSpacer.name = num.ToString("D3");
			m_gridSpacer.ForceUpdate(true);
			num++;
			if (_item.category.IsWeaponCategory() || _item.category.IsArmorCategory())
			{
				modifierValueHandler = GetCachedGameObject<ModifierValueHandler>(num++);
				modifierValueHandler.InitBaseValue(_baseValue, LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions[(int)_item.category].ValueUnitIdNormalCase), _baseValue2);
			}
			for (int i = 0; i < list.Count; i++)
			{
				ItemExtensions.ModificationComparison modificationComparison = list[i];
				modifierValueHandler = GetCachedGameObject<ModifierValueHandler>(num++);
				modifierValueHandler.Init(modificationComparison.NameId, modificationComparison.Value, modificationComparison.Diff, modificationComparison.StatType, modificationComparison.StatUnit, modificationComparison.ModuleModifierType);
			}
			m_scrollTable.repositionNow = true;
		}

		public void Init(EResourceType _resType, ResourceDescription _resDesc)
		{
			m_boxCollider.enabled = false;
			ReleaseEntries();
			m_specialOutfitDescription.text = LocalizationManager.Instance.Get(_resDesc.ItemDescriptionId);
			m_specialOutfitDescription.gameObject.SetActive(true);
			m_scrollTable.Reposition();
		}
	}
}
