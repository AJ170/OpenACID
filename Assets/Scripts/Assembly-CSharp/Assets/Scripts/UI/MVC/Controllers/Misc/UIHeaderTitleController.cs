using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UIHeaderTitleController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIHeaderTitleViewPresenter m_headerTitleView;

		public void UpdateTitle(string _title)
		{
			m_headerTitleView.Title = _title;
			base.IsInitialized = true;
			Show();
		}
	}
}
