using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Rankings
{
	public class UIRankShortDetailsView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_scoreValue;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_playerRank;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_mainAlignmentBox;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_titleColumn;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_valueColumn;

		private int m_waitForAlignmentCounter;

		public long ScoreValue
		{
			set
			{
				m_scoreValue.SetNumber(value);
			}
		}

		public int PlayerRank
		{
			set
			{
				m_playerRank.SetNumber(value);
			}
		}

		public void AlignDetails()
		{
			m_waitForAlignmentCounter = 0;
			m_waitForAlignmentCounter++;
			EventDelegate.Add(m_titleColumn.OnReposition, OnAlignColumns, true);
			m_waitForAlignmentCounter++;
			EventDelegate.Add(m_valueColumn.OnReposition, OnAlignColumns, true);
			m_titleColumn.Reposition();
			m_valueColumn.Reposition();
		}

		private void OnAlignColumns()
		{
			m_waitForAlignmentCounter--;
			if (m_waitForAlignmentCounter <= 0)
			{
				m_mainAlignmentBox.Reposition();
			}
		}

		public void OnPlayerNoRank()
		{
			m_playerRank.text = "-";
		}
	}
}
