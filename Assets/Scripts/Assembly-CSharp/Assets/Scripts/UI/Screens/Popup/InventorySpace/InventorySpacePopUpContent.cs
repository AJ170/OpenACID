using System.Collections.Generic;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.InventorySpace
{
	public class InventorySpacePopUpContent : PopUpContentView<InventorySpacePopUpContent.InventoryPopUpData>
	{
		public class InventoryPopUpData : PopUpContentData
		{
			public enum EDisplayType
			{
				Invalid = 0,
				Inventory = 1,
				AssassinSlot = 2,
				Energy = 3
			}

			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public string Description;

			public int Increase;

			public EDisplayType DisplayType;

			public bool ResourceSymbolAdditive;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.InventorySpace;
				}
			}

			public override bool HasResourceContentDisplay
			{
				get
				{
					return true;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_spaceLabel;

		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		private UIAlignmentBox m_vBox;

		[SerializeField]
		private UIWidget m_inventoryWidget;

		[SerializeField]
		private UIWidget m_assassinSlotWidget;

		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		private void Awake()
		{
		}

		protected override void Render(InventoryPopUpData _data)
		{
			m_inventoryWidget.gameObject.SetActive(false);
			m_assassinSlotWidget.gameObject.SetActive(false);
			switch (_data.DisplayType)
			{
			case InventoryPopUpData.EDisplayType.Inventory:
				m_inventoryWidget.gameObject.SetActive(true);
				m_spaceLabel.text = LocalizationManager.Instance.Get(4661, new StringMapper { { "inv_value", _data.Increase } });
				break;
			case InventoryPopUpData.EDisplayType.AssassinSlot:
				m_assassinSlotWidget.gameObject.SetActive(true);
				break;
			}
			m_descriptionLabel.text = _data.Description;
			m_uiPopupResources.ConfigurateItemsCost(_data.ResourcesCost, _data.ResourceSymbolAdditive);
		}

		public override void UpdateLayout()
		{
			EventDelegate.Add(m_vBox.OnReposition, delegate
			{
				EventDelegate.Execute(onLayoutFinished);
			}, true);
			m_vBox.RepositionNow = true;
		}
	}
}
