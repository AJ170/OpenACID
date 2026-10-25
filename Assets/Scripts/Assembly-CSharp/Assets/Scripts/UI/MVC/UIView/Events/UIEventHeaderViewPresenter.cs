using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Events
{
	public class UIEventHeaderViewPresenter : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_descriptionLabel;

		[CheckAssigned]
		[SerializeField]
		private UITexture m_eventTexture;

		public string HeaderDescription
		{
			set
			{
				m_descriptionLabel.text = value;
			}
		}

		public UITexture EventTexture
		{
			get
			{
				return m_eventTexture;
			}
		}
	}
}
