using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Rankings
{
	public class UIRankHeaderView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_title;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_scoreTypeName;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_scoreIcon;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_scoreAlignmentBox;

		public void SetTitle(string _name)
		{
			m_title.text = _name;
		}

		public void SetScoreTypeName(string _typeName)
		{
			m_scoreTypeName.text = _typeName;
			m_scoreAlignmentBox.RepositionNow = true;
		}

		public void SetScoreIcon(string _spriteName)
		{
			m_scoreIcon.spriteName = _spriteName;
			m_scoreIcon.MakePixelPerfect();
			m_scoreAlignmentBox.RepositionNow = true;
		}

		public void SetColor(Color _selectedTextColor)
		{
			m_title.color = _selectedTextColor;
			m_scoreTypeName.color = _selectedTextColor;
			m_scoreIcon.color = _selectedTextColor;
		}
	}
}
