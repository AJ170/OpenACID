using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.AssassinNamingScreen;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow
{
	public class TutorialAssassinCreateButtonFunctionality : TutorialFlowBase
	{
		[SerializeField]
		private UICreateAssassinDetails m_assassinDetails;

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
				return "Assassin Create Button";
			}
		}

		protected void Awake()
		{
		}

		public override void Activate()
		{
			base.Activate();
			m_assassinDetails.OnAssassinCreateFailed += OnAssassinCreateFailed;
			m_assassinDetails.OnAssassinCreateSuccess += OnAssassinCreateSuccess;
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				if (m_assassinDetails != null)
				{
					m_assassinDetails.OnAssassinCreateFailed -= OnAssassinCreateFailed;
					m_assassinDetails.OnAssassinCreateSuccess -= OnAssassinCreateSuccess;
				}
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (m_assassinDetails != null)
			{
				m_assassinDetails.OnAssassinCreateFailed -= OnAssassinCreateFailed;
				m_assassinDetails.OnAssassinCreateSuccess -= OnAssassinCreateSuccess;
			}
		}

		private void OnAssassinCreateSuccess()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, OnFinishedCategory);
		}

		private void OnFinishedCategory(IAsyncResult _ar)
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.Mission, new UITweenHandle());
		}

		private void OnAssassinCreateFailed()
		{
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}
	}
}
