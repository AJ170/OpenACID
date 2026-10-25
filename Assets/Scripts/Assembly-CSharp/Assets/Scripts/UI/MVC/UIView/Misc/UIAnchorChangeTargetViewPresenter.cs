using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UIAnchorChangeTargetViewPresenter : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIRect m_anchorTarget;

		[SerializeField]
		[CheckAssigned]
		private UIRect m_shortAnchorTarget;

		[CheckAssigned]
		[SerializeField]
		private UIRect m_longAnchorTarget;

		[CheckAssigned]
		[SerializeField]
		private UIScrollView m_scrollView;

		public UIScrollView ScrollView
		{
			get
			{
				return m_scrollView;
			}
		}

		public UIRect AnchorTarget
		{
			get
			{
				return m_anchorTarget;
			}
		}

		public UIRect ShortAnchorTarget
		{
			get
			{
				return m_shortAnchorTarget;
			}
		}

		public UIRect LongAnchorTarget
		{
			get
			{
				return m_longAnchorTarget;
			}
		}
	}
}
