using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Assassin
{
	public class UIAssassinPortraitView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIPortraitTexture m_assassinPortrait;

		[CheckAssigned]
		[SerializeField]
		private UIHeroicPortraitTexture m_assassinHeroicPortrait;

		public UIPortraitTexture AssassinPortrait
		{
			get
			{
				return m_assassinPortrait;
			}
		}

		public UIHeroicPortraitTexture AssassinHeroicPortrait
		{
			get
			{
				return m_assassinHeroicPortrait;
			}
		}
	}
}
