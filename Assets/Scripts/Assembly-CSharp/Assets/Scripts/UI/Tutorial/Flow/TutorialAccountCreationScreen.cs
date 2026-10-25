using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;

namespace Assets.Scripts.UI.Tutorial.Flow
{
	public class TutorialAccountCreationScreen : TutorialFlowBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatIntroMission;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override string Name
		{
			get
			{
				return "Account Creation Screen";
			}
		}

		public override void Activate()
		{
			base.Activate();
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.AccountCreation, new UITweenHandle(null, null));
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Exclude();
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.BackButton != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.BackButton.gameObject.SetActive(false);
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.BackButton != null)
				{
					Singleton<UIScreenManager>.Instance.SceneData.BackButton.gameObject.SetActive(true);
				}
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}
	}
}
