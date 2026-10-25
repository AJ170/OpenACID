using AcierProtocol;
using Assets.Scripts.UI.InfoPopup.Misc;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Content
{
	public class ItemInfoPopupContent : InfoPopUpContentView<ItemInfoPopupContent.ItemInfoPopUpData>
	{
		public class ItemInfoPopUpData : InfoPopUpContentData
		{
			public bool IsOutFit;

			public bool IsMysteryBox;

			public bool ShowItemQuality = true;

			public Resource Resource;

			public Item.EItemCategory Category = AcierProtocol.Item.EItemCategory.None;

			public Assassin Assassin;

			public InventoryElement Element;

			public InventoryElement CompareToElement;

			public int ItemPowerOverride;

			public string ItemColorOverride = string.Empty;

			public ItemQualityDescription ItemQualityDescription;

			public override UIInfoPopup.EContentType Type
			{
				get
				{
					if (IsOutFit)
					{
						return UIInfoPopup.EContentType.Outfits;
					}
					if (IsMysteryBox)
					{
						return UIInfoPopup.EContentType.MysteryBox;
					}
					return UIInfoPopup.EContentType.InventoryItem;
				}
			}

			public ItemData Item
			{
				get
				{
					return (Element == null || !Element.IsItem()) ? null : Element.itemData;
				}
			}

			public ItemData CompareToItem
			{
				get
				{
					return (CompareToElement == null || !CompareToElement.IsItem()) ? null : CompareToElement.itemData;
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
		private UIExtendedLabel m_valueLabel;

		[SerializeField]
		private UIExtendedLabel m_unitLabel;

		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		private UIScrollView m_scrollview;

		[SerializeField]
		private UIPortraitTexture m_imageTexture;

		[SerializeField]
		private UIAlignmentBox m_powerStatAlignmentBox;

		[SerializeField]
		private ItemStatsListHandler m_itemStatsListHandler;

		public override UIPortraitTexture GetPortraitTexture()
		{
			return m_imageTexture;
		}

		private void Awake()
		{
		}

		protected override void Render(ItemInfoPopUpData _data)
		{
			bool flag = _data.Resource != null || _data.Item != null || _data.Element.mysteryBoxData != null;
			m_mainDataContainer.SetActive(flag);
			m_helpTextLabel.gameObject.SetActive(!flag);
			if (flag)
			{
				m_qualityLabel.gameObject.SetActive(_data.ShowItemQuality);
				if (_data.ShowItemQuality)
				{
					m_qualityLabel.text = LocalizationManager.Instance.Get(_data.ItemQualityDescription.NameId);
				}
				if (m_powerStatAlignmentBox != null)
				{
					m_powerStatAlignmentBox.gameObject.SetActive(true);
				}
				if (_data.Item != null)
				{
					int num = _data.ItemPowerOverride;
					if (num <= 0)
					{
						num = _data.Item.CalculateItemPower();
					}
					m_unitLabel.text = LocalizationManager.Instance.Get(6565);
					m_valueLabel.text = LocalizationManager.Instance.GetInteger(num);
					m_itemStatsListHandler.Init(_data.Item, _data.CompareToItem, _data.Assassin);
					UIItemUtil.UpdateItem(_data.Element, true, m_imageTexture, _data.IsOutFit, _data.ItemColorOverride, 0f);
				}
				else if (_data.Resource != null)
				{
					if (m_powerStatAlignmentBox != null)
					{
						m_powerStatAlignmentBox.gameObject.SetActive(false);
					}
					ResourceDescription value = null;
					UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)_data.Resource.type, out value);
					m_valueLabel.text = LocalizationManager.Instance.GetInteger(_data.Resource.amount);
					m_itemStatsListHandler.Init(_data.Resource.type, value);
					if (_data.ShowItemQuality)
					{
						Color itemQualityColor = UIItemUtil.GetItemQualityColor(_data.ItemQualityDescription.Quality);
						string itemColorOverride = _data.ItemColorOverride;
						UIItemUtil.UpdateResourceItem(_data.Resource.type, itemQualityColor, true, m_imageTexture, false, UIItemUtil.ItemUVCoordType.ENone, itemColorOverride, 0f);
					}
					else
					{
						string itemColorOverride = _data.ItemColorOverride;
						UIItemUtil.UpdateResourceItem(_data.Resource.type, true, m_imageTexture, false, UIItemUtil.ItemUVCoordType.ENone, itemColorOverride, 0f);
					}
				}
				else if (_data.Element.mysteryBoxData != null)
				{
					EMysteryBoxType mysteryBoxType = _data.Element.mysteryBoxData.mysteryBoxType;
					StringMapper _mapper = new StringMapper();
					mysteryBoxType.FillMysteryBoxStringMapper(_data.Element.quality, ref _mapper);
					int num2 = 0;
					if (mysteryBoxType == EMysteryBoxType.MysteryBoxItemType_Tutorial)
					{
						num2 = 6730;
					}
					else if (_data.Element.descriptionlangKey != null)
					{
						num2 = _data.Element.descriptionlangKey.neutral;
					}
					else
					{
						switch (mysteryBoxType)
						{
						case EMysteryBoxType.MysteryBoxItemType_Tutorial:
							num2 = 6730;
							break;
						case EMysteryBoxType.MysteryBoxMiscType:
							num2 = 6732;
							break;
						case EMysteryBoxType.MysteryBoxRessourceType:
							num2 = 6731;
							break;
						}
					}
					m_descriptionLabel.text = LocalizationManager.Instance.Get(num2, _mapper);
					UIItemUtil.UpdateMysteryBoxElement(_data.Element, true, m_imageTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty);
				}
				if (m_powerStatAlignmentBox != null && m_powerStatAlignmentBox.gameObject.activeSelf)
				{
					m_powerStatAlignmentBox.RepositionNow = true;
				}
			}
			else
			{
				m_helpTextLabel.text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemCategoryDescriptions[(int)_data.Category].HelpTextId);
			}
			m_scrollview.ResetPosition();
		}
	}
}
