using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.SkillTreeScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.SkillPoints
{
	internal class TutorialSkillPointsSelectSkill : TutorialClickBase
	{
		[SerializeField]
		private SkillButtonHandler m_skillButtonHandler;

		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialSkillPointsSelectSkill";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25SkillPoints;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
			EventDelegate.Add(m_skillButtonHandler.onSelect, base.OnClick);
		}

		protected override void RemoveButtonFromEventDelegate()
		{
			EventDelegate.Remove(m_skillButtonHandler.onSelect, base.OnClick);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideSkillPoints(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.AssassinSkillTree;
		}

		public override int GetHeaderText()
		{
			return 6834;
		}

		public override int GetDescriptionText()
		{
			return 6837;
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (m_skillButtonHandler != null)
			{
				EventDelegate.Remove(m_skillButtonHandler.onSelect, base.OnClick);
			}
		}
	}
}
