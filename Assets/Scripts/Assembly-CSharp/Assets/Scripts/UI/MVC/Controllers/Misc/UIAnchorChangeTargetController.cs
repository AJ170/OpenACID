using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UIAnchorChangeTargetController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIAnchorChangeTargetViewPresenter m_anchorChangeTargetView;

		public void SetShortAnchorTarget()
		{
			UIRect shortAnchorTarget = m_anchorChangeTargetView.ShortAnchorTarget;
			UpdateBottomAnchor(shortAnchorTarget);
		}

		public void SetLongAnchorTarget()
		{
			UIRect longAnchorTarget = m_anchorChangeTargetView.LongAnchorTarget;
			UpdateBottomAnchor(longAnchorTarget);
		}

		private void UpdateBottomAnchor(UIRect _target)
		{
			UIRect anchorTarget = m_anchorChangeTargetView.AnchorTarget;
			_target.Update();
			anchorTarget.bottomAnchor.Set(_target.transform, 1f, 30f);
			anchorTarget.ResetAnchors();
			anchorTarget.UpdateAnchors();
			anchorTarget.Update();
		}
	}
}
