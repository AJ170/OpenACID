using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.LoadingScreen
{
	public class UILoadingScreenDetails : UIBaseBehaviour
	{
		public enum EState
		{
			None = 0,
			Loading = 1,
			WaitForTap = 2
		}

		public delegate void ScreenClosed();

		public delegate void TapScreen();

		public UITweener Tweener;

		public UITexture BackgroundTexture;

		public UITexture BackgroundTextureBlurred;

		public UIExtendedLabel MapName;

		public UIExtendedLabel MissionName;

		public UIExtendedLabel MissionDescription;

		public UIExtendedLabel LoadingStatus;

		public TweenAlpha SpinnerTweener;

		public TweenColor StatusColorTweener;

		public UISlider ProgressSlider;

		public UISprite MissionType;

		[SerializeField]
		private UISprite m_missionTypeContainer;

		[SerializeField]
		public UnityEngine.Animation m_loadingCompleteEffect;

		[SerializeField]
		public ParticleSystem m_particleSystem;

		private bool m_isPlaying;

		private float m_previousTimeSinceStartup;

		private float m_elapsedTime;

		private EState m_currentState;

		private bool m_tweenerDone;

		public TapScreen OnTapScreen;

		public ScreenClosed OnScreenClosed;

		public UITexture m_abstergoLogo;

		public float Progress
		{
			get
			{
				return (!(ProgressSlider != null)) ? 0f : ProgressSlider.value;
			}
			set
			{
				if (ProgressSlider != null)
				{
					ProgressSlider.value = value;
					ShowProgressBar(value > 0f);
				}
			}
		}

		public EState CurrentState
		{
			get
			{
				return m_currentState;
			}
			set
			{
				m_currentState = value;
				UpdateState();
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.LoadingScreen;
			}
		}

		public override bool FullyVisible
		{
			get
			{
				return CurrentState != 0 && m_tweenerDone;
			}
		}

		public void Open()
		{
			ResetAnimation();
			CurrentState = EState.Loading;
			base.gameObject.GetComponent<Collider>().enabled = true;
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
				if ((bool)Tweener)
				{
					m_tweenerDone = false;
					EventDelegate.Add(Tweener.onFinished, OnOpenTweenFinished, true);
					Tweener.delay = 0f;
					Tweener.PlayForward();
				}
				else
				{
					OnOpenTweenFinished();
				}
			}
		}

		public void Close(bool _delayed)
		{
			CurrentState = EState.None;
			base.gameObject.GetComponent<Collider>().enabled = false;
			if ((bool)Tweener)
			{
				EventDelegate.Add(Tweener.onFinished, OnCloseTweenFinished, true);
				if (_delayed)
				{
					Tweener.delay = 0.6f;
				}
				Tweener.PlayReverse();
			}
			else
			{
				OnCloseTweenFinished();
			}
		}

		public void UpdateScreen(MissionRootData _missionDesc, bool _isElite, bool _isLeavingGame)
		{
			if (_missionDesc == null)
			{
				ClearElements();
				SetDefaultBackground();
				m_abstergoLogo.alpha = 1f;
			}
			else
			{
				UpdateElements(_missionDesc, _isElite, _isLeavingGame);
			}
		}

		private void OnClick()
		{
			if (OnTapScreen != null)
			{
				OnTapScreen();
			}
		}

		private void OnOpenTweenFinished()
		{
			m_tweenerDone = true;
		}

		private void OnCloseTweenFinished()
		{
			base.gameObject.SetActive(false);
			ClearElements();
			if (OnScreenClosed != null)
			{
				OnScreenClosed();
			}
		}

		private void ShowProgressBar(bool _show)
		{
			if (ProgressSlider != null)
			{
				ProgressSlider.gameObject.SetActive(_show);
			}
		}

		private void Update()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = realtimeSinceStartup - m_previousTimeSinceStartup;
			m_previousTimeSinceStartup = realtimeSinceStartup;
			if (m_isPlaying)
			{
				m_elapsedTime += num;
				m_loadingCompleteEffect["Transition"].normalizedTime = m_elapsedTime / m_loadingCompleteEffect["Transition"].length;
				m_particleSystem.Simulate(num, true, false);
				if (m_loadingCompleteEffect["Transition"].normalizedTime > 2f)
				{
					m_isPlaying = false;
				}
			}
		}

		private void UpdateElements(MissionRootData _missionDesc, bool _isElite, bool _isLeavingGame)
		{
			string value = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			RegionDefinition regionDefinition = _missionDesc.GetRegionDefinition();
			RegionDefinition.AppearanceOption appearance = null;
			if (regionDefinition != null)
			{
				if (_missionDesc.ObjectiveType == EObjectiveType.Story_EObjectiveType || _missionDesc.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType)
				{
					UIMissionUtil.UpdateMissionIconByMissionObjectiveType(MissionType, _missionDesc.ObjectiveType, false, m_missionTypeContainer);
					MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
					if (missionSelectionManager != null && missionSelectionManager.CampaignMission != null)
					{
						appearance = regionDefinition.GetRegionAppearanceOption(missionSelectionManager.CampaignMission.regionAppearanceOption);
					}
				}
				else
				{
					UIMissionUtil.UpdateMissionIconByMissionObjectiveType(MissionType, _missionDesc.MissionKind, _isElite, m_missionTypeContainer);
				}
				m_missionTypeContainer.gameObject.SetActive(true);
				value = LocalizationManager.Instance.Get(regionDefinition.GetRegionNameLoca(appearance).Key);
				UpdateBackgroundTexture(regionDefinition.GetRegionMap(appearance).GetObject<Texture>());
			}
			else
			{
				SetDefaultBackground();
			}
			m_abstergoLogo.gameObject.SetActive(false);
			if (!_isLeavingGame)
			{
				empty2 = ((_missionDesc.ObjectiveType != EObjectiveType.Story_EObjectiveType && _missionDesc.ObjectiveType != EObjectiveType.Tutorial_EObjectiveType) ? LocalizationManager.Instance.Get(5547) : _missionDesc.LocaName(regionDefinition));
				if (_isElite)
				{
					empty2 = LocalizationManager.Instance.Get(8029, new StringMapper { { "elite_mission_name", empty2 } });
				}
				empty = _missionDesc.LocaBriefing();
			}
			else
			{
				value = LocalizationManager.Instance.Get(1830);
				if (UbiCore.Utils.Singleton<UiMain>.Instance.WasLastMissionSuccessful)
				{
					empty = _missionDesc.LocaDebriefing();
					empty2 = LocalizationManager.Instance.Get(1850);
				}
				else
				{
					empty2 = LocalizationManager.Instance.Get(1196);
					empty = ((_missionDesc.ObjectiveType != EObjectiveType.Tutorial_EObjectiveType) ? LocalizationManager.Instance.Get(2075) : LocalizationManager.Instance.Get(7766));
				}
			}
			SetLabel(MapName, value);
			SetLabel(MissionDescription, empty);
			SetLabel(MissionName, empty2);
			Progress = 0f;
		}

		private void SetLabel(UILabel _label, string _value)
		{
			bool active = _value != string.Empty;
			_label.text = _value;
			_label.gameObject.SetActive(active);
		}

		private void UpdateState()
		{
			bool flag = false;
			switch (m_currentState)
			{
			case EState.None:
				LoadingStatus.text = string.Empty;
				SpinnerTweener.gameObject.SetActive(false);
				break;
			case EState.Loading:
				LoadingStatus.text = LocalizationManager.Instance.Get(1414);
				SpinnerTweener.gameObject.SetActive(true);
				SpinnerTweener.from = 0f;
				SpinnerTweener.to = 1f;
				SpinnerTweener.RewindAndPlay();
				break;
			case EState.WaitForTap:
			{
				flag = true;
				ResetAnimation();
				m_loadingCompleteEffect.gameObject.SetActive(true);
				int num = ((!Main.Instance.SceneLoader.InAnimus) ? 1415 : 3791);
				LoadingStatus.text = LocalizationManager.Instance.Get(num);
				ShowProgressBar(false);
				m_loadingCompleteEffect.Play();
				m_isPlaying = true;
				SpinnerTweener.from = 1f;
				SpinnerTweener.to = 0f;
				SpinnerTweener.RewindAndPlay();
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			LoadingStatus.color = StatusColorTweener.from;
			StatusColorTweener.enabled = flag;
			if (flag)
			{
				StatusColorTweener.PlayForward();
			}
			Update();
		}

		private void ResetAnimation()
		{
			m_elapsedTime = 0f;
			m_previousTimeSinceStartup = Time.realtimeSinceStartup;
			m_loadingCompleteEffect["Transition"].normalizedTime = 0f;
			m_loadingCompleteEffect["Transition"].time = 0f;
			m_loadingCompleteEffect.Stop();
			m_particleSystem.Simulate(0f, true, true);
			m_loadingCompleteEffect.gameObject.SetActive(false);
		}

		private void ClearElements()
		{
			MapName.text = string.Empty;
			MissionName.text = string.Empty;
			MissionDescription.text = string.Empty;
			m_missionTypeContainer.gameObject.SetActive(false);
			Progress = 0f;
			m_abstergoLogo.alpha = 1f;
		}

		private void SetDefaultBackground()
		{
			Texture texture = null;
			RegionDefinition firstRegionDefinition = EEra.Common.GetFirstRegionDefinition();
			if (firstRegionDefinition != null)
			{
				texture = firstRegionDefinition.Map.GetObject<Texture>();
			}
			UpdateBackgroundTexture(texture);
		}

		private void UpdateBackgroundTexture(Texture _texture)
		{
			Texture mainTexture = BackgroundTexture.mainTexture;
			BackgroundTexture.mainTexture = _texture;
			BackgroundTextureBlurred.mainTexture = BackgroundTexture.mainTexture;
			if (mainTexture != null && mainTexture != _texture)
			{
				UnityEngine.Resources.UnloadAsset(mainTexture);
			}
		}
	}
}
