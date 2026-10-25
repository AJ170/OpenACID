using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Rankings
{
	public class UIRankEntryDetailsView : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_leaderboardRank;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_playerName;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_assassinName;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_assassinClasses;

		public void SetLeaderboardRank(int _rank, Color _color)
		{
			m_leaderboardRank.text = LocalizationManager.Instance.GetInteger(_rank);
			m_leaderboardRank.color = _color;
		}

		public void SetPlayerName(string _playerName, Color _color)
		{
			m_playerName.text = _playerName;
			m_playerName.color = _color;
		}

		public void SetAssassinName(string _name)
		{
			m_assassinName.text = _name;
		}

		public void SetAssassinClasses(string _assassinClasses)
		{
			m_assassinClasses.text = _assassinClasses;
		}
	}
}
