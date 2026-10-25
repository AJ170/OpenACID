using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Content
{
	public class BasicWithItemCountInfoPopupContent : InfoPopUpContentView<BasicWithItemCountInfoPopupContent.BasicWithItemCountInfoPopupData>
	{
		public class BasicWithItemCountInfoPopupData : InfoPopUpContentData
		{
			public ECustomItemType CustomItemType;

			public StringId DescriptionlangKey;

			public int Value;

			public override UIInfoPopup.EContentType Type
			{
				get
				{
					return UIInfoPopup.EContentType.BasicContentWithCount;
				}
			}
		}

		[SerializeField]
		[CheckAssigned]
		private UIScrollView m_scrollView;

		[CheckAssigned]
		[SerializeField]
		private UIPortraitTexture m_imageTexture;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_valueLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		public override UIPortraitTexture GetPortraitTexture()
		{
			return m_imageTexture;
		}

		protected override void Render(BasicWithItemCountInfoPopupData _data)
		{
			Color32 shopNoQualityColor = UIDataMapper.UITextColors.ShopNoQualityColor;
			m_valueLabel.SetNumber(_data.Value);
			m_descriptionLabel.text = LocalizationManager.Instance.Get(_data.DescriptionlangKey);
			UIItemUtil.UpdateCustomItem(_data.CustomItemType, shopNoQualityColor, true, m_imageTexture, 0f);
			m_scrollView.ResetPosition();
		}

		private void Awake()
		{
		}
	}
}
