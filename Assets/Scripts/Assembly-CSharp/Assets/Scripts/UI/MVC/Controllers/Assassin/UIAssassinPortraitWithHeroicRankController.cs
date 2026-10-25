using AcierProtocol;
using Assets.Scripts.UI.MVC.UIView.Assassin;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinPortraitWithHeroicRankController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIAssassinPortraitView m_assassinPortraitView;

		public void UpdateAssassinPortrait(AcierProtocol.Assassin _assassin, int _heroicRank)
		{
			UIPortraitUtil.UpdatePortraitByColorDef(_assassin, m_assassinPortraitView.AssassinPortrait, true);
			UIPortraitUtil.UpdateHeroicPortraitByAssassin(null, _heroicRank, m_assassinPortraitView.AssassinHeroicPortrait);
		}

		public void UpdateAssassinPortrait(GameID _headId, GameID _humanoidId, GameID _colorId, GameID _itemId, bool _hideAssassin, int _heroicRank)
		{
			UIPortraitUtil.UpdatePortraitByColorDef(_headId, _humanoidId, _colorId, _itemId, m_assassinPortraitView.AssassinPortrait, true);
			m_assassinPortraitView.AssassinPortrait.alpha = ((!_hideAssassin) ? 1f : 0.01f);
			UIPortraitUtil.UpdateHeroicPortraitByAssassin(null, _heroicRank, m_assassinPortraitView.AssassinHeroicPortrait);
		}
	}
}
