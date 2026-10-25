using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialGotoMissionSubMenu : TutorialClickBase
	{
		[SerializeField]
		protected UIPanel PanelBlocker;

		public override void Activate()
		{
			base.Activate();
			Singleton<StateManager>.Instance.MissionSelectionManager.OnSubMenuChanged += new SimpleDelegate.CallbackFunction(OnSubMenuChanged);
			if (PanelBlocker != null)
			{
				PanelBlocker.gameObject.SetActive(true);
			}
		}

		public override void Deactivate()
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.OnSubMenuChanged -= new SimpleDelegate.CallbackFunction(OnSubMenuChanged);
			if (IsActive)
			{
				base.Deactivate();
				if (PanelBlocker != null)
				{
					PanelBlocker.gameObject.SetActive(false);
				}
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			Singleton<StateManager>.Instance.MissionSelectionManager.OnSubMenuChanged -= new SimpleDelegate.CallbackFunction(OnSubMenuChanged);
		}

		private void OnSubMenuChanged(params object[] _params)
		{
			if (Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType == GetNextSubMenu())
			{
				Singleton<StateManager>.Instance.MissionSelectionManager.OnSubMenuChanged -= new SimpleDelegate.CallbackFunction(OnSubMenuChanged);
				OnSubMenuChangedInternal();
			}
		}

		protected virtual void OnSubMenuChangedInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		protected abstract MissionSelectionManager.EMissionMenu GetNextSubMenu();

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
		}

		protected override void RemoveButtonFromEventDelegate()
		{
		}
	}
}
