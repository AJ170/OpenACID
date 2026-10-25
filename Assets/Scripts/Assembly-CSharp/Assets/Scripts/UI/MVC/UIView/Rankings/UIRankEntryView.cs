using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Rankings
{
	public class UIRankEntryView : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_rank;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_playerName;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_score;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_relationIcon;

		public void SetPosition(long _rank)
		{
			m_rank.text = LocalizationManager.Instance.GetInteger(_rank);
		}

		public void SetPlayerName(string _name)
		{
			m_playerName.text = _name;
		}

		public void SetScore(long _rank)
		{
			m_score.text = LocalizationManager.Instance.GetInteger(_rank);
		}

		public void SetRelationIcon(string _icon)
		{
			bool flag = !string.IsNullOrEmpty(_icon);
			m_relationIcon.gameObject.SetActive(flag);
			if (flag)
			{
				m_relationIcon.spriteName = _icon;
				m_relationIcon.MakePixelPerfect();
			}
			m_relationIcon.Update();
		}

		public void SetColor(Color _color)
		{
			m_rank.color = _color;
			m_playerName.color = _color;
			m_score.color = _color;
			m_relationIcon.color = _color;
		}

		public override void Show()
		{
			base.ViewRoot.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.ViewRoot.gameObject.SetActive(false);
		}
	}
}
