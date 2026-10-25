using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.Utils;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Content
{
	internal class UnknownItemPopupContent : InfoPopUpContentView<UnknownItemPopupContent.UnknownItemInfoPopUpData>
	{
		public class UnknownItemInfoPopUpData : InfoPopUpContentData
		{
			public LootFilter LootFilter;

			public Item.EItemCategory ItemCategory = Item.EItemCategory.None;

			public ItemDef ItemDefinition;

			public bool ShowItemQuality = true;

			public ItemQualityDescription ItemQualityDescription;

			public override UIInfoPopup.EContentType Type
			{
				get
				{
					return UIInfoPopup.EContentType.UnknownItem;
				}
			}
		}

		[SerializeField]
		private GameObject m_mainDataContainer;

		[SerializeField]
		private UIExtendedLabel m_qualityLabel;

		[SerializeField]
		private UIExtendedLabel m_helpTextLabel;

		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		private UIPortraitTexture m_imageTexture;

		public override UIPortraitTexture GetPortraitTexture()
		{
			return m_imageTexture;
		}

		protected override void Render(UnknownItemInfoPopUpData _data)
		{
			bool flag = _data.ItemDefinition != null;
			m_mainDataContainer.SetActive(flag);
			m_helpTextLabel.gameObject.SetActive(!flag);
			if (flag)
			{
				m_qualityLabel.gameObject.SetActive(_data.ShowItemQuality);
				if (_data.ShowItemQuality)
				{
					m_qualityLabel.text = LocalizationManager.Instance.Get(_data.ItemQualityDescription.NameId);
				}
				Quality.EQuality quality = _data.ItemQualityDescription.Quality;
				Color itemQualityColor = UIItemUtil.GetItemQualityColor(quality);
				bool isUnknownItem = _data.ItemDefinition.ItemType != ItemDef.EItemType.Consumables;
				if (_data.ItemDefinition != null)
				{
					UIItemUtil.UpdateItemByColorDef(_data.ItemDefinition.GameID, itemQualityColor, true, null, m_imageTexture, false, UIItemUtil.ItemUVCoordType.ENone, isUnknownItem, 0f);
				}
				m_descriptionLabel.text = GetDescriptionString(_data.LootFilter, quality, _data.ItemCategory);
			}
		}

		private string GetDescriptionString(LootFilter _lootFilter, Quality.EQuality _quality, Item.EItemCategory _category)
		{
			string empty = string.Empty;
			if (_lootFilter.lootTypes.mysteryBoxTypes.Length == 0)
			{
				StringMapper stringMapper = new StringMapper();
				stringMapper.Add("quality", LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_quality].NameId));
				if (_category == Item.EItemCategory.None)
				{
					stringMapper.Add("type", LocalizationManager.Instance.Get(7718));
				}
				else
				{
					stringMapper.Add("type", LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions[(int)_category].CategoryNameId));
				}
				return LocalizationManager.Instance.Get(7708, stringMapper);
			}
			EMysteryBoxType eMysteryBoxType = _lootFilter.lootTypes.mysteryBoxTypes[0];
			StringMapper _mapper = new StringMapper();
			eMysteryBoxType.FillMysteryBoxStringMapper(_quality, ref _mapper);
			StringId key = null;
			MysteryBoxDescription value = null;
			if (eMysteryBoxType == EMysteryBoxType.MysteryBoxItemType)
			{
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemMysteryBoxDescription.TryGetValue((int)_quality, out value);
			}
			else
			{
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceMysteryBoxDescription.TryGetValue((int)_quality, out value);
			}
			if (value != null)
			{
				key = value.descriptionId;
			}
			return LocalizationManager.Instance.Get(key, _mapper);
		}
	}
}
