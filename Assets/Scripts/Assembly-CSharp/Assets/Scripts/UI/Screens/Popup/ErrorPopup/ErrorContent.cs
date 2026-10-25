using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.ErrorPopup
{
	public class ErrorContent : PopUpContentView<ErrorContent.ErrorData>
	{
		public class ErrorData : PopUpContentData
		{
			public string Description;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.ErrorMessage;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_label;

		protected override void Render(ErrorData _data)
		{
			m_label.text = _data.Description;
		}

		private void Awake()
		{
		}
	}
}
