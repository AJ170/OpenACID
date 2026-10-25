using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxAssassinTab : TutorialGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialMysteryBoxAssassinTab";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25MysteryBox;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override void Activate()
		{
			m_buttonWidget = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager.GetButtonByHeaderType(UIButtonHeader.EHeaderType.Assassin).GetComponent<UIWidget>();
			base.Activate();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.TabUnlockScreen;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.AssassinSelection;
		}

		public override int GetHeaderText()
		{
			return 6956;
		}

		public override int GetDescriptionText()
		{
			return 6957;
		}
	}
}
