using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.SkillPoints
{
	public abstract class TutorialSkillPointsTrainBase : TutorialButtonAndConfirmationBase
	{
		[SerializeField]
		private UIWidget m_skillScrollArea;

		private int m_scrollPanelOldDepth;

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25SkillPoints;
			}
		}

		protected override void OnSetupInternal()
		{
			base.OnSetupInternal();
			if (m_skillScrollArea != null)
			{
				m_scrollPanelOldDepth = m_skillScrollArea.depth;
				m_skillScrollArea.depth = ((!(m_buttonPanel != null)) ? m_buttonWidget.depth : m_buttonPanel.depth);
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				if (m_skillScrollArea != null)
				{
					m_skillScrollArea.depth = m_scrollPanelOldDepth;
				}
			}
		}

		protected override void OnClickInternal()
		{
			ClearEffects();
		}

		protected override void OnModalScreenPoppedInternal()
		{
		}

		protected override UIPopup.EContentType GetPopupType()
		{
			return UIPopup.EContentType.Skills;
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideSkillPoints(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.AssassinSkillTree;
		}
	}
}
