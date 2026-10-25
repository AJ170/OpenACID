using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.UserConfirmationPopup
{
	public class UserConfirmationContent : PopUpContentView<UserConfirmationContent.ConfirmationData>
	{
		public class ConfirmationData : PopUpContentData
		{
			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.UserConfirmation;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_label;

		protected override void Render(ConfirmationData _data)
		{
			m_label.text = _data.Description;
		}

		private void Awake()
		{
		}
	}
}
