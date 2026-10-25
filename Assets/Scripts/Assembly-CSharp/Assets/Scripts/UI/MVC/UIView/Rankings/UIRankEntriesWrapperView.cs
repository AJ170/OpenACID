using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Rankings
{
	public class UIRankEntriesWrapperView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedWrapContent m_wrapperContainer;

		[SerializeField]
		[CheckAssigned]
		private UIRect m_anchorTarget;

		public UIExtendedWrapContent Wrapper
		{
			get
			{
				return m_wrapperContainer;
			}
		}

		public UIRect AnchorTarget
		{
			get
			{
				return m_anchorTarget;
			}
		}

		public void SetItemCount(int _itemCount)
		{
			Wrapper.ItemCount = _itemCount;
		}

		public void ResetPosition()
		{
			Wrapper.WrapScrollView.ResetPosition();
		}
	}
}
