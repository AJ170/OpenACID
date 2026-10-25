using Assets.Scripts.UI.MVC.Models.Rankings;
using Assets.Scripts.UI.MVC.UIView.Rankings;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankHeaderController : UIControllerBase
	{
		protected static readonly Color s_selectedTextColor = new Color32(6, 88, 102, byte.MaxValue);

		[CheckAssigned]
		[SerializeField]
		private UIRankHeaderView m_rankHeaderView;

		public void UpdateRankHeader(UIRankHeaderBaseModel _data)
		{
			m_rankHeaderView.SetTitle(_data.Name);
			m_rankHeaderView.SetScoreTypeName(_data.ScoreName);
			m_rankHeaderView.SetScoreIcon(_data.ScoreIcon);
			m_rankHeaderView.SetColor(s_selectedTextColor);
		}
	}
}
