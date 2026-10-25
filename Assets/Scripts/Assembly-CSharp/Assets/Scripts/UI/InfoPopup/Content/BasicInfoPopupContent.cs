using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Content
{
	public class BasicInfoPopupContent : InfoPopUpContentView<BasicInfoPopupContent.BasicInfoPopupData>
	{
		public class BasicInfoPopupData : InfoPopUpContentData
		{
			public InventoryElement Element;

			public ColorDef ItemColordef;

			public override UIInfoPopup.EContentType Type
			{
				get
				{
					return UIInfoPopup.EContentType.BasicContent;
				}
			}
		}

		[SerializeField]
		[CheckAssigned]
		private UIScrollView m_scrollView;

		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_imageTexture;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		public override UIPortraitTexture GetPortraitTexture()
		{
			return m_imageTexture;
		}

		protected override void Render(BasicInfoPopupData _data)
		{
			m_descriptionLabel.text = LocalizationManager.Instance.Get(_data.Element.descriptionlangKey.neutral);
			UIItemUtil.UpdateItem(_data.Element, true, m_imageTexture, _data.ItemColordef, false, 0f);
			m_scrollView.ResetPosition();
		}

		private void Awake()
		{
		}
	}
}
