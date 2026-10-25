using System;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup.Content
{
	public class EntitlementInfoPopupContent : InfoPopUpContentView<EntitlementInfoPopupContent.EntitlementInfoPopUpData>
	{
		public class EntitlementInfoPopUpData : InfoPopUpContentData
		{
			public string Description;

			public string TextureGuid;

			public override UIInfoPopup.EContentType Type
			{
				get
				{
					return UIInfoPopup.EContentType.Entitlement;
				}
			}
		}

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private UIExtendedLabel m_descriptionLabel;

		[SerializeField]
		private UITexture m_imageTexture;

		public override UIPortraitTexture GetPortraitTexture()
		{
			throw new NotImplementedException();
		}

		private void Awake()
		{
		}

		protected override void Render(EntitlementInfoPopUpData _data)
		{
			m_descriptionLabel.text = _data.Description;
			UIItemUtil.UpdateUITexture(_data.TextureGuid, m_imageTexture);
			m_scrollView.ResetPosition();
		}
	}
}
