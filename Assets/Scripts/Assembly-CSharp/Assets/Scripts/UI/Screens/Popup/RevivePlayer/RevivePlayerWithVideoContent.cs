using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.RevivePlayer
{
	public class RevivePlayerWithVideoContent : PopUpContentView<RevivePlayerWithVideoContent.RevivePlayerWithVideoData>
	{
		public class RevivePlayerWithVideoData : PopUpContentData
		{
			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.RevivePlayerWithVideo;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_questionLabel;

		private void Awake()
		{
		}

		protected override void Render(RevivePlayerWithVideoData _data)
		{
			m_questionLabel.text = _data.Description;
		}
	}
}
