using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialGotoBase : TutorialClickBase
	{
		[SerializeField]
		protected UIPanel PanelBlocker;

		public override void Activate()
		{
			base.Activate();
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
			if (PanelBlocker != null)
			{
				PanelBlocker.gameObject.SetActive(true);
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				if (PanelBlocker != null)
				{
					PanelBlocker.gameObject.SetActive(false);
				}
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
		}

		private void OnScreenChanged()
		{
			EScreenID currentScreenId = Singleton<UIScreenManager>.Instance.CurrentScreenId;
			EScreenID nextScreenId = GetNextScreenId();
			if (nextScreenId == EScreenID.Invalid || currentScreenId == nextScreenId)
			{
				OnScreenChangedInternal();
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
			}
		}

		protected virtual void OnScreenChangedInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		protected abstract EScreenID GetNextScreenId();

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
		}

		protected override void RemoveButtonFromEventDelegate()
		{
		}
	}
}
