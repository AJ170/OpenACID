using Assets.Scripts.UI.InfoPopup;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UIInfoPopupViewPresenter : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIInfoPopup m_detailsPopup;

		[SerializeField]
		[CheckAssigned]
		private UIInfoPopupController m_controller;

		public UIInfoPopup InfoPopup
		{
			get
			{
				return m_detailsPopup;
			}
		}

		public void Close()
		{
			m_detailsPopup.Close();
		}

		protected override void OnAwakeUnity()
		{
			m_detailsPopup.OnBeforePopupCloseCallback += OnBeforePopupCloseCallback;
		}

		private void OnBeforePopupCloseCallback()
		{
			if (m_controller.IsInitialized)
			{
				m_controller.OnBeforePopupCloseCallback();
			}
		}

		protected override void OnDestroyUnity()
		{
			if (m_detailsPopup != null)
			{
				m_detailsPopup.OnBeforePopupCloseCallback -= OnBeforePopupCloseCallback;
			}
		}
	}
}
