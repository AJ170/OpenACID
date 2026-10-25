using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialButtonAndConfirmationBase : TutorialButtonClickBase
	{
		public override void Activate()
		{
			base.Activate();
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onModalScreenPopped, OnModalScreenPopped);
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
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onModalScreenPopped, OnModalScreenPopped);
		}

		private void OnModalScreenPopped()
		{
			UIScreen previousModalScreen = Singleton<UIScreenManager>.Instance.PreviousModalScreen;
			UIPopup uIPopup = previousModalScreen as UIPopup;
			if (uIPopup != null)
			{
				if (uIPopup.CurrentContentType == GetPopupType())
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
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		protected override void OnClickInternal()
		{
			ClearEffects();
		}

		protected abstract UIPopup.EContentType GetPopupType();
	}
}
