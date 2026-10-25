using System;
using Assets.Scripts.Network.Support;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SupportScreen
{
	public class SupportScreenView : UIScreenChild
	{
		[CheckAssigned]
		[SerializeField]
		private UIButton m_backButton;

		[CheckAssigned]
		[SerializeField]
		private UIRect m_alignTarget;

		[SerializeField]
		[CheckAssigned]
		private UITable m_vBox;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_tabPrefab;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_supportDisabledLabel;

		[CheckAssigned]
		[SerializeField]
		private UIEventTrigger m_supportLabelTrigger;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_supportOfflineHeader;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onExit, OnScreenExit);
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onExit, OnScreenExit);
			}
		}

		private void OnScreenActivate()
		{
			if (Main.Instance.SupportEnabled)
			{
				m_supportDisabledLabel.gameObject.SetActive(false);
				m_supportOfflineHeader.SetActive(false);
				Array values = Enum.GetValues(typeof(ESupportQueue));
				foreach (object item in values)
				{
					ESupportQueue eSupportQueue = (ESupportQueue)(int)item;
					if (eSupportQueue != ESupportQueue.Refound)
					{
						CreateSupportTabView(eSupportQueue);
					}
				}
				m_vBox.repositionNow = true;
			}
			else
			{
				m_supportOfflineHeader.SetActive(true);
				m_supportDisabledLabel.gameObject.SetActive(true);
				EventDelegate.Add(m_supportLabelTrigger.onClick, OnClickedLink);
			}
		}

		private void OnScreenExit()
		{
			m_vBox.transform.ClearChildren();
			if (!Main.Instance.SupportEnabled)
			{
				EventDelegate.Remove(m_supportLabelTrigger.onClick, OnClickedLink);
			}
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_backButton.onClick, OnBackButtonClicked);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_backButton.onClick, OnBackButtonClicked);
		}

		private void OnClickedLink()
		{
			string urlAtPosition = m_supportDisabledLabel.GetUrlAtPosition(UICamera.lastWorldPosition);
			Application.OpenURL(urlAtPosition);
		}

		private void OnBackButtonClicked()
		{
			Singleton<UIScreenManager>.Instance.PopScreen();
			Singleton<UIScreenManager>.Instance.PushScreen(Singleton<UIScreenManager>.Instance.GetScreen(m_screen.m_previousScreen), null);
		}

		private void CreateSupportTabView(ESupportQueue _queueType)
		{
			GameObject gameObject = NGUITools.AddChild(m_vBox.gameObject, m_tabPrefab);
			UIWidget component = gameObject.GetComponent<UIWidget>();
			UIUtil.UpdateLeftAndRightAnchorsForTarget(component, m_alignTarget);
			SupportTabView component2 = gameObject.GetComponent<SupportTabView>();
			component2.Queue = _queueType;
		}

		public override void OnBackKey()
		{
			OnBackButtonClicked();
		}
	}
}
