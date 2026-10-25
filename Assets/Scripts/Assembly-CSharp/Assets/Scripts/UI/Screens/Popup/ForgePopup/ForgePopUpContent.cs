using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.ForgePopup
{
	public class ForgePopUpContent : PopUpContentView<ForgePopUpContent.ForgePopUpData>
	{
		public class ForgePopUpData : PopUpContentData
		{
			public enum EMode
			{
				Invalid = -1,
				ForgeFull = 0,
				ForgeOverspend = 1,
				ReRoll = 2
			}

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.Forge;
				}
			}

			public override bool HasResourceContentDisplay
			{
				get
				{
					return true;
				}
			}

			public string Description { get; set; }

			public EMode Mode { get; set; }

			public Quality.EQuality Quality { get; set; }

			public GameID GUID { get; set; }

			public InventoryElement Element { get; set; }

			public List<UIPopupResourcesContent.ResourceConfiguration> Cost { get; set; }
		}

		public static readonly StringId ForgeDescriptionFull = new StringId(7692);

		public static readonly StringId ForgeDescriptionOverSpend = new StringId(7691);

		public static readonly StringId RerollDescription = new StringId(6823);

		[CheckAssigned]
		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[CheckAssigned]
		[SerializeField]
		private UIPortraitTexture m_outcomeTexture;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_vbox;

		[SerializeField]
		[CheckAssigned]
		private Texture m_defaultBackground;

		[CheckAssigned]
		[SerializeField]
		private Texture m_unkownBackground;

		private void Awake()
		{
		}

		protected override void Render(ForgePopUpData _data)
		{
			UpdateLabel(_data);
			UpdateImage(_data);
			UpdateCost(_data);
		}

		public override void UpdateLayout()
		{
			EventDelegate.Add(m_vbox.OnReposition, OnAlignmentUpdate, true);
			m_vbox.RepositionNow = true;
		}

		private void UpdateCost(ForgePopUpData _data)
		{
			m_uiPopupResources.ConfigurateItemsCost(_data.Cost, false);
		}

		private void UpdateLabel(ForgePopUpData _data)
		{
			m_descriptionLabel.text = _data.Description;
		}

		private void UpdateImage(ForgePopUpData _data)
		{
			if (_data.Mode == ForgePopUpData.EMode.ReRoll)
			{
				m_outcomeTexture.mainTexture = m_defaultBackground;
				UIItemUtil.UpdateItem(_data.Element, false, m_outcomeTexture, false, string.Empty, 0f);
				return;
			}
			m_outcomeTexture.mainTexture = ((_data.Mode != ForgePopUpData.EMode.ReRoll) ? m_unkownBackground : m_defaultBackground);
			GameID colorGUID = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ItemQualityDescriptions[(int)_data.Quality].ColorGUID;
			ColorDef colorDefForColorGuid = UIItemUtil.GetColorDefForColorGuid(colorGUID);
			UIItemUtil.UpdateItemByColorDef(_data.GUID, colorDefForColorGuid.Color0, false, colorDefForColorGuid, m_outcomeTexture, false, UIItemUtil.ItemUVCoordType.ENone, false, 0f);
		}

		private void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}
	}
}
