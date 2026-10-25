using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	internal class TutorialForgeReroll : TutorialInfoPopUpBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideForgeReroll.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeReroll";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Forge;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onModalScreenPopped, OnModalScreenPopped);
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			ItemInfoPopupHelper.OnReRollCallback -= new SimpleDelegate.CallbackFunction(OnReroll);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onModalScreenPopped, OnModalScreenPopped);
		}

		private void OnModalScreenPopped()
		{
			UIScreen previousModalScreen = Singleton<UIScreenManager>.Instance.PreviousModalScreen;
			UIPopup uIPopup = previousModalScreen as UIPopup;
			if (uIPopup != null)
			{
				if (uIPopup.CurrentContentType == UIPopup.EContentType.Forge)
				{
					OnModalScreenPoppedInternal();
					return;
				}
				ClearEffects();
				TidyUpClickWidget();
				PrepareClickWidget();
			}
		}

		protected virtual void OnModalScreenPoppedInternal()
		{
			OnRerollFinished(null);
		}

		protected override void OnClickInternal()
		{
			ClearEffects();
		}

		protected override void AddActionCallback()
		{
			ItemInfoPopupHelper.OnReRollCallback += new SimpleDelegate.CallbackFunction(OnReroll);
		}

		protected override void RemoveActionCallback()
		{
			ItemInfoPopupHelper.OnReRollCallback -= new SimpleDelegate.CallbackFunction(OnReroll);
		}

		private void OnReroll(object[] _params)
		{
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onModalScreenPopped, OnModalScreenPopped);
		}

		private void OnRerollFinished(IAsyncResult _ar)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return 6933;
		}
	}
}
