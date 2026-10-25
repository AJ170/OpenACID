using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Contracts
{
	internal class TutorialContractsTab : TutorialGotoMissionSubMenu
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialContractsTab";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Contracts;
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
			m_buttonWidget = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager.GetButtonByHeaderType(UIButtonHeader.EHeaderType.MissionsContracts).GetComponent<UIWidget>();
			base.Activate();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.TabUnlockScreen;
		}

		protected override MissionSelectionManager.EMissionMenu GetNextSubMenu()
		{
			return MissionSelectionManager.EMissionMenu.ContractMissions;
		}

		public override int GetHeaderText()
		{
			return 7136;
		}

		public override int GetDescriptionText()
		{
			return 7137;
		}
	}
}
