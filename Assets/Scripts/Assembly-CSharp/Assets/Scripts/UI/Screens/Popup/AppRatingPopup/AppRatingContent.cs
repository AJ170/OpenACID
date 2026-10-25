using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.AppRatingPopup
{
	public class AppRatingContent : PopUpContentView<AppRatingContent.AppRatingData>
	{
		public class AppRatingData : PopUpContentData
		{
			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.AppRating;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_questionLabel;

		protected override void Render(AppRatingData _data)
		{
			m_questionLabel.text = _data.Description;
		}

		private void Awake()
		{
		}
	}
}
