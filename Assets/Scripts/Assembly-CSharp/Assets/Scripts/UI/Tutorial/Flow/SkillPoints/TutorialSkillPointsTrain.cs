using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.SkillTreeScreen;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.SkillPoints
{
	internal class TutorialSkillPointsTrain : TutorialSkillPointsTrainBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialSkillPointsTrain";
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
			base.Activate();
			SkillTreeDetails skillTreeDetails = TutorialHelper.GetSkillTreeDetails(GetScreenId());
			skillTreeDetails.TrainOkInternalOverride = OnTrainOkPressed;
			skillTreeDetails.TrainCostOverride = new TrainingCost();
			skillTreeDetails.TrainCostOverride.Resources = new Resource[1];
			Resource resource = new Resource();
			resource.amount = 0L;
			resource.type = EResourceType.SoftCurrency;
			skillTreeDetails.TrainCostOverride.Resources[0] = resource;
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				SkillTreeDetails skillTreeDetails = TutorialHelper.GetSkillTreeDetails(GetScreenId());
				if (skillTreeDetails != null)
				{
					skillTreeDetails.TrainOkInternalOverride = null;
					skillTreeDetails.InstantTrainOkInternalOverride = null;
					skillTreeDetails.InstantTrainResourceConfigOverride = null;
					skillTreeDetails.UseProfileFacadeMock = false;
					skillTreeDetails.ForceUpdateData();
				}
			}
		}

		private void OnTrainOkPressed(SkillTreeData _data)
		{
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			SkillData skill = _data.GetSkill(_data.SelectedSkill);
			_data.UseProfileFacadeMock = true;
			SkillTreeProfileFacadeMock profileFacadeMock = _data.ProfileFacadeMock;
			AssassinSkillInTraining assassinSkillInTraining = new AssassinSkillInTraining();
			assassinSkillInTraining.skillClass = AssassinSkillInTraining.SkillClass.BaseClass;
			assassinSkillInTraining.skillType = skill.SkillType;
			assassinSkillInTraining.tierId = skill.TierId;
			Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			SkillTreeDetails skillTreeDetails = TutorialHelper.GetSkillTreeDetails(GetScreenId());
			profileFacadeMock.Init(TutorialHelper.GetFakeTrainSkillPoints(), TutorialHelper.GetFakeTrainSkillPointsSpent());
			skillTreeDetails.TrainAssassinDuringTutorial(delegate(IAsyncResult _asyncResult)
			{
				OnTrainFinished(_asyncResult, _data);
			});
		}

		private void OnTrainFinished(IAsyncResult _async, SkillTreeData _data)
		{
			_data.UseProfileFacadeMock = false;
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public override int GetHeaderText()
		{
			return 6834;
		}

		public override int GetDescriptionText()
		{
			return 6838;
		}
	}
}
