using System;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialGoBackBase : TutorialClickBase
	{
		[SerializeField]
		private EScreenID m_BackScreenID = EScreenID.Invalid;

		public override void Activate()
		{
			Singleton<UIScreenManager>.Instance.SceneData.BackButton.StopBackTapEvent = true;
			m_buttonWidget = Singleton<UIScreenManager>.Instance.SceneData.BackButton.GetComponent<UIWidget>();
			base.Activate();
			ActivateBackButton(true);
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.SceneData.BackButton.OnBeforeTapBack, base.OnClick);
		}

		protected override void RemoveButtonFromEventDelegate()
		{
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.SceneData.BackButton.OnBeforeTapBack, base.OnClick);
		}

		protected override void OnClickInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, OnFinishedCallback);
		}

		private void OnFinishedCallback(IAsyncResult ar)
		{
			Singleton<UIScreenManager>.Instance.SceneData.BackButton.StopBackTapEvent = false;
			Singleton<UIScreenManager>.Instance.SceneData.BackButton.TriggerGoBack(m_BackScreenID);
		}
	}
}
