using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UIHeaderTitleViewPresenter : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_titleText;

		public string Title
		{
			set
			{
				m_titleText.text = value;
			}
		}
	}
}
