using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.MissionResult
{
	public class UIMissionRewardItemBehaviour : MonoBehaviour
	{
		[SerializeField]
		private UIWidget m_container;

		[SerializeField]
		private UIExtendedLabel m_amount;

		[SerializeField]
		private UIWidget m_itemAmountContainer;

		[SerializeField]
		private UIPortraitTexture m_itemTexture;

		[SerializeField]
		private GameObject m_selectionHighlight;

		[SerializeField]
		private UnityEngine.Animation m_rewardAnimation;

		[SerializeField]
		private CompareItemHandler m_compareItemHandler;

		[SerializeField]
		private UISprite m_backgroundIconSprite;

		[SerializeField]
		private UISprite m_newIconSprite;

		public LootTemplate LootTemplate { get; private set; }

		public InventoryElement InventoryElement { get; private set; }

		public Resource Resource { get; private set; }

		public ItemDef ItemDef { get; private set; }

		public ShopBundle ShopBundle { get; private set; }

		public Quality.EQuality ItemQuality { get; private set; }

		public bool UseItemQuality { get; private set; }

		public Item.EItemCategory ItemCategory { get; private set; }

		public LootFilter LootFilter { get; private set; }

		public bool New
		{
			set
			{
				UpdateNewIcon(value);
			}
		}

		public UnityEngine.Animation RewardAnimation
		{
			get
			{
				return m_rewardAnimation;
			}
		}

		public bool IsSelected
		{
			get
			{
				return m_selectionHighlight.activeSelf;
			}
			set
			{
				m_selectionHighlight.SetActive(value);
			}
		}

		public event Action<UIMissionRewardItemBehaviour> OnClickCallback;

		private void Awake()
		{
			UpdateNewIcon(false);
			UpdateItemBackgroundIcon(Item.EItemCategory.None);
		}

		public void UpdateRewardByItem(InventoryElement _element)
		{
			ResetValues();
			InventoryElement = _element;
			if (_element != null)
			{
				UIItemUtil.UpdateItem(_element, false, m_itemTexture, false, string.Empty, 0f);
			}
			CompareItemToEquipedItem();
			int num = ((_element != null) ? _element.stackSize : 0);
			m_amount.text = LocalizationManager.Instance.GetInteger(num);
			m_itemAmountContainer.gameObject.SetActive(num > 1);
			SetVisibility(_element != null);
		}

		public void UpdateRewardByShopBundle(ShopBundle _shopBundle)
		{
			ResetValues();
			ShopBundle = _shopBundle;
			GameID previewColorGameID = _shopBundle.GetPreviewColorGameID();
			Color32 _qualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
			Color32 _textQualityColor = UIDataMapper.UITextColors.ShopMundaneOverideColor;
			UIUtil.UpdateShopItemQuality(_shopBundle, ref _qualityColor, ref _textQualityColor);
			if (_shopBundle.IsBluePrint())
			{
				_qualityColor = UIDataMapper.UITextColors.BluePrintQualityColor;
			}
			else if (_shopBundle.IsCustomLootItemType(ECustomLootTemplateType.LockPick))
			{
				_qualityColor = UIDataMapper.UITextColors.LockPickQualityColor;
			}
			UIItemUtil.UpdateShopItem(_shopBundle.GetPreviewImageGameID(), previewColorGameID, false, m_itemTexture, _qualityColor, UIItemUtil.ItemUVCoordType.EShopItem, 0f);
			m_itemAmountContainer.gameObject.SetActive(false);
			SetVisibility(true);
		}

		public void UpdateRewardByResource(EResourceType _resourceType, long _amount, Quality.EQuality _quality = Quality.EQuality.Mundane, bool _useItemQuality = false)
		{
			ResetValues();
			Resource = new Resource
			{
				amount = _amount,
				type = _resourceType
			};
			ItemQuality = _quality;
			UseItemQuality = _useItemQuality;
			Color quality = UIItemUtil.s_defaultShopItemColor;
			if (_useItemQuality)
			{
				quality = UIItemUtil.GetItemQualityColor(_quality);
			}
			UIItemUtil.UpdateResourceItem(_resourceType, quality, false, m_itemTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
			m_amount.text = LocalizationManager.Instance.GetInteger(_amount);
			m_itemAmountContainer.gameObject.SetActive(_amount > 1);
			SetVisibility(true);
		}

		public void UpdateRewardByResource(Resource _resource, Quality.EQuality _quality = Quality.EQuality.Mundane, bool _useItemQuality = false)
		{
			ResetValues();
			Resource = _resource;
			ItemQuality = _quality;
			UseItemQuality = _useItemQuality;
			Color quality = UIItemUtil.s_defaultShopItemColor;
			if (_useItemQuality)
			{
				quality = UIItemUtil.GetItemQualityColor(_quality);
			}
			UIItemUtil.UpdateResourceItem(_resource.type, quality, false, m_itemTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
			m_amount.text = LocalizationManager.Instance.GetInteger(_resource.amount);
			m_itemAmountContainer.gameObject.SetActive(_resource.amount > 1);
			SetVisibility(true);
		}

		public void UpdateRewardByTemplate(LootTemplate _template)
		{
			ResetValues();
			if (_template.resourceTemplate != null)
			{
				UpdateRewardByResource(_template.resourceTemplate.maxResource);
			}
			else if (_template.entitlementTemplate == null)
			{
				UpdateRewardByItem(_template.GetItemFromLootTemplate());
			}
			LootTemplate = _template;
		}

		public void UpdateRewardByGameID(GameID _gameId, Quality.EQuality _quality, LootFilter _lootFilter)
		{
			ResetValues();
			LootFilter = _lootFilter;
			ItemCategory = LootFilter.GetItemCategory();
			ItemDef = Main.Instance.GameDB.GetItem<ItemDef>(_gameId);
			ItemQuality = _quality;
			ColorDef colorDefForQuality = UIItemUtil.GetColorDefForQuality(_quality);
			bool isUnknownItem = ItemDef.ItemType != ItemDef.EItemType.Consumables;
			UIItemUtil.UpdateItemByColorDef(_gameId, colorDefForQuality.Color0, false, null, m_itemTexture, false, UIItemUtil.ItemUVCoordType.ENone, isUnknownItem, 0f);
			m_itemAmountContainer.gameObject.SetActive(false);
			SetVisibility(true);
		}

		public void ShowEmptyItemContainer(Item.EItemCategory _category = Item.EItemCategory.None)
		{
			PrepareEmptyIcon();
			UpdateItemBackgroundIcon(_category);
		}

		public void ShowEmptyModulesContainer()
		{
			PrepareEmptyIcon();
			UpdateEmptyModulesIcon();
		}

		public void SetVisibility(bool _visible)
		{
			base.gameObject.SetActive(_visible);
		}

		private void OnClick()
		{
			if (this.OnClickCallback != null)
			{
				this.OnClickCallback(this);
			}
		}

		private void PrepareEmptyIcon()
		{
			ResetValues();
			m_container.gameObject.SetActive(false);
			SetVisibility(true);
		}

		private void UpdateEmptyModulesIcon()
		{
			UIItemUtil.UpdateModuleImage(UbiCore.Utils.Singleton<UIDataMapper>.Instance.EmptyModuleGUID, false, m_itemTexture);
		}

		private void UpdateItemBackgroundIcon(Item.EItemCategory _category)
		{
			ItemCategoryDescription value;
			if (_category == Item.EItemCategory.None)
			{
				m_backgroundIconSprite.gameObject.SetActive(false);
			}
			else if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions.TryGetValue((int)_category, out value))
			{
				m_backgroundIconSprite.atlas = Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Animus);
				m_backgroundIconSprite.spriteName = value.IconName;
			}
		}

		private void UpdateNewIcon(bool _new)
		{
			m_newIconSprite.gameObject.SetActive(_new);
		}

		private void ResetValues()
		{
			InventoryElement = null;
			Resource = null;
			LootTemplate = null;
			ItemQuality = Quality.EQuality.Mundane;
			UseItemQuality = true;
			m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
		}

		private void CompareItemToEquipedItem()
		{
			if (InventoryElement.IsItem())
			{
				Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				InventoryElement elementForCategory = currentAssassin.GetElementForCategory(InventoryElement.itemData.category);
				if (elementForCategory == null)
				{
					m_compareItemHandler.State = CompareItemHandler.EState.Better;
					return;
				}
				int num = InventoryElement.CompareTo(elementForCategory);
				if (InventoryElement.itemData.category != elementForCategory.itemData.category)
				{
					m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
				}
				else if (num == 0)
				{
					m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
				}
				else
				{
					m_compareItemHandler.State = ((num > 0) ? CompareItemHandler.EState.Better : CompareItemHandler.EState.Worse);
				}
			}
			else
			{
				m_compareItemHandler.State = CompareItemHandler.EState.Neutral;
			}
		}
	}
}
