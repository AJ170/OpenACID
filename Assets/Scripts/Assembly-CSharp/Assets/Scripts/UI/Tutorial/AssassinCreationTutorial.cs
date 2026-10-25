using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow;

namespace Assets.Scripts.UI.Tutorial
{
	public class AssassinCreationTutorial : TutorialFlowBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatAssassinCreation;
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
				return "Assassin creation";
			}
		}

		public override void Activate()
		{
			base.Activate();
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.gameObject.SetActive(false);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.AssassinSelection, new UITweenHandle(null, null));
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Exclude();
			SetTitleHeaderAnchor();
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
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.gameObject.SetActive(true);
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Show();
				ResetTitleHeaderAnchor();
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

		protected void OnDestroy()
		{
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
		}

		private void OnScreenChanged()
		{
			EScreenID currentScreenId = Singleton<UIScreenManager>.Instance.CurrentScreenId;
			if (currentScreenId == EScreenID.AssassinCreation)
			{
				if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.BackButton != null)
				{
					Singleton<UIScreenManager>.Instance.SceneData.BackButton.gameObject.SetActive(true);
				}
			}
			else if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.BackButton != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.BackButton.gameObject.SetActive(false);
			}
		}

		private void SetTitleHeaderAnchor()
		{
		}

		private void ResetTitleHeaderAnchor()
		{
		}
	}
}
