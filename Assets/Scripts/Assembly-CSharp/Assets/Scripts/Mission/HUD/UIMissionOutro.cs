using System;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UIMissionOutro : UIBase
	{
		private UIMissionOutroBehaviour m_missionOutroBehaviour;

		private AnimationEventListener m_listener;

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.MissionOutro;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_missionOutroBehaviour;
			}
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				Init();
			}
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				if (m_listener != null)
				{
					AnimationEventListener listener = m_listener;
					listener.OnEvent = (AnimationEventListener.OnEventDelegate)Delegate.Remove(listener.OnEvent, new AnimationEventListener.OnEventDelegate(OnAnimationEvent));
				}
				base.IsInitialized = false;
				if (m_missionOutroBehaviour != null)
				{
					UnityEngine.Object.Destroy(m_missionOutroBehaviour);
				}
				m_missionOutroBehaviour = null;
				SetVisible(false);
			}
		}

		public override void OnMissionEnded()
		{
			base.OnMissionEnded();
			if (IsActive)
			{
				SetVisible(false);
			}
		}

		public void Init()
		{
			if (m_missionOutroBehaviour != null || !UbiCore.Utils.Singleton<MissionManager>.Instance.DoOutro)
			{
				return;
			}
			GameObject gameObject = Resources.Load("UI/Logos/MissionOutro") as GameObject;
			if (!(gameObject != null))
			{
				return;
			}
			UIRoot uIRoot = null;
			for (int i = 0; i < UIRoot.list.Count; i++)
			{
				UIRoot uIRoot2 = UIRoot.list[i];
				if (uIRoot2.gameObject.name == "UI InGame Root")
				{
					uIRoot = uIRoot2;
				}
			}
			if (!(uIRoot == null))
			{
				gameObject = NGUITools.AddChild(uIRoot.gameObject, gameObject);
				m_missionOutroBehaviour = gameObject.GetComponent<UIMissionOutroBehaviour>();
				if ((bool)m_missionOutroBehaviour)
				{
					m_missionOutroBehaviour.Init();
					base.IsInitialized = true;
				}
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive)
			{
				SetVisible(true);
				m_listener = Outro.Instance.GetComponent<AnimationEventListener>();
				if (m_listener != null)
				{
					AnimationEventListener listener = m_listener;
					listener.OnEvent = (AnimationEventListener.OnEventDelegate)Delegate.Combine(listener.OnEvent, new AnimationEventListener.OnEventDelegate(OnAnimationEvent));
				}
			}
		}

		private void OnAnimationEvent(string _desc)
		{
			if (!(m_missionOutroBehaviour == null))
			{
				if (_desc == "ShowLogo")
				{
					SetVisible(true);
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_tutorial_eaglescream_noducking);
				}
				else if (_desc == "HideLogo")
				{
					SetVisible(false);
				}
			}
		}
	}
}
