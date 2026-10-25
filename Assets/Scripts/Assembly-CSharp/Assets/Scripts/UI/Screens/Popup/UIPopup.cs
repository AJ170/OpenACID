using System;
using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Native;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Widget;
using Database;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup
{
	public class UIPopup : UIScreen
	{
		public enum EContentType
		{
			ErrorMessage = 0,
			Skills = 1,
			Item = 2,
			FriendHireling = 3,
			InventorySpace = 4,
			AssassinSlot = 5,
			ConvertCurrency = 6,
			Module = 7,
			EarlyAccess = 8,
			AppRating = 9,
			ThirdPartyAccount = 10,
			FacebookInvite = 11,
			RevivePlayer = 12,
			SocialMediaShareAchievement = 13,
			CallToAction = 14,
			Forge = 15,
			RefillEnergy = 16,
			TutorialMission = 17,
			UserConfirmation = 18,
			ChooseSecondClass = 19,
			RevivePlayerWithVideo = 20
		}

		public enum EContentAlignTarget
		{
			Heading = 0,
			CloseButton = 1,
			Top = 2,
			CloseButtonOverlay = 3
		}

		[Serializable]
		private sealed class PrefabHandle
		{
			public EContentType Type;

			public string PrefabPath;
		}

		private const int CONTENT_HEAD_OFFSET = 20;

		[SerializeField]
		[HideInInspector]
		private List<PrefabHandle> m_prefabs = new List<PrefabHandle>();

		[SerializeField]
		private GameObject m_buttonPrefab;

		[SerializeField]
		private GameObject m_timerPrefab;

		[SerializeField]
		private UIWidget m_BackgroundFrame;

		[SerializeField]
		protected UILabel Header;

		[SerializeField]
		protected UIWidget HeaderContainer;

		[SerializeField]
		protected UIAlignmentBox ButtonBar;

		[SerializeField]
		protected UIAlignmentBox TimerWithOneButtonBar;

		[SerializeField]
		protected UIAlignmentBox m_contentBox;

		[SerializeField]
		protected UIButtonProxy m_closeButton;

		[SerializeField]
		protected GameObject m_contentParent;

		[SerializeField]
		protected GameObject m_defaultFrame;

		[SerializeField]
		private UnityEngine.Animation m_openAnimation;

		[SerializeField]
		protected BBTweenAlpha m_popupTween;

		[SerializeField]
		[Range(1f, 500f)]
		private int m_buttonWidth = 310;

		[SerializeField]
		private int m_buttonDepth;

		[SerializeField]
		private FullscreenBlocker m_blocker;

		private PopUpContentView m_content;

		private PopUpConfiguration m_configuration;

		private PopUpConfiguration m_nextContent;

		private PopUpConfiguration.CloseButtonPressHandler m_closeHandler;

		private GameID m_closeButtonAudioControllerId = GameID.Invalid;

		private IAudioHandle m_closeButtonAudioHandle;

		private UIExtendedButton[] m_buttons;

		private List<EventDelegate> TimerRanOut = new List<EventDelegate>();

		private PopupTimerHandler m_timerHandler;

		private int m_delayColliderFixFrames;

		public EContentType CurrentContentType { get; set; }

		public FullscreenBlocker Blocker
		{
			get
			{
				return m_blocker;
			}
		}

		private GameID CloseButtonAudioControllerId
		{
			get
			{
				return m_closeButtonAudioControllerId;
			}
			set
			{
				if (AudioManager.IsAvailable && m_closeButtonAudioControllerId != value)
				{
					m_closeButtonAudioControllerId = value;
					if (m_closeButtonAudioHandle != null)
					{
						m_closeButtonAudioHandle.UnloadAllNowOrOnDone();
						m_closeButtonAudioHandle = null;
					}
					if (GameID.IsValid(m_closeButtonAudioControllerId))
					{
						m_closeButtonAudioHandle = AudioManager.Instance.CreateHandle(m_closeButtonAudioControllerId, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadAll);
						m_closeButtonAudioHandle.Load();
					}
				}
			}
		}

		public string this[EContentType _type]
		{
			get
			{
				PrefabHandle prefabHandle = m_prefabs.Find((PrefabHandle x) => x.Type == _type);
				return (prefabHandle == null) ? null : prefabHandle.PrefabPath;
			}
			set
			{
				PrefabHandle prefabHandle = m_prefabs.Find((PrefabHandle x) => x.Type == _type);
				if (prefabHandle != null)
				{
					prefabHandle.PrefabPath = value;
					return;
				}
				m_prefabs.Add(new PrefabHandle
				{
					Type = _type,
					PrefabPath = value
				});
			}
		}

		public virtual void Configure(PopUpConfiguration _conf)
		{
			if (Active && m_nextContent == null && _conf != null)
			{
				m_nextContent = _conf;
				EventDelegate.Remove(m_popupTween.onFinished, OnChangingConfigOnTweenFinished);
				EventDelegate.Add(m_popupTween.onFinished, OnChangingConfigOnTweenFinished, true);
				m_popupTween.PlayReverse();
				return;
			}
			m_nextContent = null;
			EventDelegate.Remove(m_popupTween.onFinished, OnChangingConfigOnTweenFinished);
			EventDelegate.Add(TimerRanOut, _conf.OnTimerRanOut);
			base.gameObject.SetActive(true);
			if (m_blocker != null)
			{
				m_blocker.gameObject.SetActive(false);
			}
			m_popupTween.SetTweenToBeginning();
			m_openAnimation.Stop();
			Clear();
			DoDeActivate();
			CurrentContentType = _conf.Data.Type;
			m_closeHandler = _conf.OnCloseButtonClicked;
			ConfigureCloseButton(_conf.ShowCloseButton, _conf.CloseButtonAudioControllerId);
			ConfigureHeading(_conf.Heading);
			ConfigButtons(_conf.Buttons, _conf.TimerDuration, _conf.Data.HasResourceContentDisplay);
			ConfigContent(_conf.Data, _conf);
			UpdateLayouts();
			m_configuration = _conf;
			m_delayColliderFixFrames = 2;
		}

		public UIExtendedButton GetButton(int _index)
		{
			if (m_buttons == null || _index >= m_buttons.Length)
			{
				return null;
			}
			return m_buttons[_index];
		}

		protected override void OnScreenAwoken()
		{
			if (NativeTools.IsTVDevice)
			{
			}
			base.OnScreenAwoken();
			base.Modal = true;
			base.CameraPosition = CameraMenuManager.EType.None;
			m_previousScreen = EScreenID.Invalid;
		}

		protected void Update()
		{
			if (Active && m_configuration != null)
			{
				if (AcierInputHandler.Instance != null && !AcierInputHandler.Instance.EnableGamepadInAnimus)
				{
					CheckInput();
				}
				if (m_delayColliderFixFrames == 0)
				{
					UIUtil.UpdateColliderIfTimerPaused();
					m_delayColliderFixFrames = -1;
				}
				else if (m_delayColliderFixFrames > 0)
				{
					m_delayColliderFixFrames--;
				}
			}
		}

		private void CheckInput()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if ((gamepad.AcceptUp || (gamepad.CloseUp && !m_configuration.ShowCloseButton)) && m_configuration != null && m_configuration.Buttons != null && m_configuration.Buttons.Length > 0)
			{
				OnButtonClick(m_configuration.Buttons[0]);
			}
			if (gamepad.CloseUp && m_configuration.ShowCloseButton)
			{
				OnCloseClicked();
			}
		}

		private void DeactivateAllButtons()
		{
			for (int i = 0; i < ButtonBar.transform.childCount; i++)
			{
				UIExtendedButton component = ButtonBar.transform.GetChild(i).GetComponent<UIExtendedButton>();
				if (component != null)
				{
					component.isEnabled = false;
				}
			}
		}

		protected virtual void UpdateLayouts()
		{
			EventDelegate.Add(ButtonBar.OnReposition, OnButtonLayoutFinished, true);
			ButtonBar.RepositionNow = true;
			if (TimerWithOneButtonBar != null)
			{
				TimerWithOneButtonBar.RepositionNow = true;
			}
		}

		protected void OnButtonLayoutFinished()
		{
			EventDelegate.Add(m_content.onLayoutFinished, OnContentLayoutFinished, true);
			m_content.UpdateLayout();
		}

		protected void OnContentLayoutFinished()
		{
			UIWidget component = m_contentParent.GetComponent<UIWidget>();
			component.height = (int)m_content.Size.y;
			EventDelegate.Add(m_contentBox.OnReposition, OnRepositionContentBox, true);
			m_contentBox.Reposition();
			m_popupTween.StartTweenPlayForward();
		}

		protected virtual void Clear()
		{
			Header.text = string.Empty;
			m_closeHandler = null;
			m_configuration = null;
			EventDelegate.Remove(m_closeButton.onClick, OnCloseClicked);
		}

		protected override void DoDeActivate()
		{
			if (m_content != null)
			{
				m_content.Purge();
			}
			m_content = null;
			if (ButtonBar != null)
			{
				for (int i = 0; i < ButtonBar.Children.Count; i++)
				{
					Transform transform = ButtonBar.Children[i];
					if (transform != null)
					{
						UIExtendedButton component = transform.GetComponent<UIExtendedButton>();
						if (component != null)
						{
							component.onClick.Clear();
						}
					}
				}
			}
			if (ButtonBar != null)
			{
				ButtonBar.transform.ClearChildren();
			}
			if (TimerWithOneButtonBar != null)
			{
				TimerWithOneButtonBar.transform.ClearChildren();
			}
			if (m_contentParent != null)
			{
				m_contentParent.transform.ClearChildren();
			}
		}

		protected override void DoExit()
		{
			base.DoExit();
			Clear();
		}

		private void ConfigButtons(PopUpButtonConfiguration[] _buttons, float _timerDuration = 0f, bool _addSpacer = false)
		{
			bool flag = _buttons.Length > 1 && (_addSpacer || _timerDuration > 0f);
			bool flag2 = _addSpacer && _buttons.Length > 1;
			bool flag3 = false;
			if (NativeTools.IsTVDevice && DevicePermissionHelper.ForceShowTimer && _buttons.Length <= 1)
			{
				flag3 = (flag = true);
				TimerWithOneButtonBar.StartAndEndWithPadding = true;
				TimerWithOneButtonBar.StrechWidget = false;
				TimerWithOneButtonBar.IgnoreChildren = true;
				TimerWithOneButtonBar.transform.ClearChildren();
			}
			TimerWithOneButtonBar.gameObject.SetActive(flag3);
			ButtonBar.StartAndEndWithPadding = true;
			ButtonBar.StrechWidget = false;
			ButtonBar.IgnoreChildren = true;
			ButtonBar.Sorted = flag;
			ButtonBar.transform.ClearChildren();
			m_buttons = new UIExtendedButton[_buttons.Length];
			if (flag3)
			{
				AddTimer(TimerWithOneButtonBar.gameObject, _timerDuration);
			}
			else if (flag)
			{
				if (_timerDuration > 0f)
				{
					AddTimer(ButtonBar.gameObject, _timerDuration);
				}
				else if (flag2)
				{
					GameObject gameObject = NGUITools.AddChild(ButtonBar.gameObject);
					gameObject.AddComponent<UIWidget>().width = 92;
					gameObject.name = "#2 MidSpacer";
				}
			}
			for (int i = 0; i < _buttons.Length; i++)
			{
				PopUpButtonConfiguration conf = _buttons[i];
				GameObject gameObject2 = NGUITools.AddChild(ButtonBar.gameObject, m_buttonPrefab);
				if (flag)
				{
					gameObject2.name = string.Format("#{0} {1}", (i == 0) ? 1 : 3, gameObject2.name);
				}
				UIExtendedButton component = gameObject2.GetComponent<UIExtendedButton>();
				m_buttons[i] = component;
				component.isEnabled = conf.IsEnabled;
				EventDelegate.Add(component.onClick, delegate
				{
					OnButtonClick(conf);
				});
				UIWidget component2 = gameObject2.GetComponent<UIWidget>();
				component2.width = m_buttonWidth;
				component2.depth = m_buttonDepth;
				ConfigureButton(conf, component);
			}
		}

		private void AddTimer(GameObject _gameObject, float _timerDuration)
		{
			m_timerHandler = NGUITools.AddChild(_gameObject, m_timerPrefab).GetComponent<PopupTimerHandler>();
			m_timerHandler.name = string.Format("#2 {0}", m_timerHandler.name);
			m_timerHandler.Init(_timerDuration, TimerRanOut);
		}

		private void OnButtonClick(PopUpButtonConfiguration _configuration)
		{
			if (m_configuration.DeactivateButtonsOnTap)
			{
				DeactivateAllButtons();
			}
			if (_configuration.OnButtonPressed != null)
			{
				_configuration.OnButtonPressed(_configuration.Data);
			}
		}

		private void ConfigureButton(PopUpButtonConfiguration _config, UIExtendedButton _button)
		{
			UIButtonSounds component = _button.GetComponent<UIButtonSounds>();
			if (component != null)
			{
				component.SetSuppressAudio(_config.SuppressAudio);
				component.ButtonIntent = _config.SoundIntent;
			}
			_button.Type = ((_config.ButtonType == PopUpButtonConfiguration.EButtonType.eSuggestedClick) ? EButtonTypes.Action : EButtonTypes.Normal);
			_button.isEnabled = _config.IsEnabled;
			UIButtonLabelHandler component2 = _button.GetComponent<UIButtonLabelHandler>();
			component2.SetText(_config.Label);
			if (!string.IsNullOrEmpty(_config.IconSpriteName))
			{
				component2.SetIcon(Singleton<UIScreenManager>.Instance.GetAtlasByType(_config.IconSpriteAtlas), _config.IconSpriteName);
			}
			else if (_config.Resource != null)
			{
				component2.ShowResource = true;
				component2.ResourceType = _config.Resource.type;
			}
			UIGamepadSelectable uIGamepadSelectable = _button.gameObject.AddComponentIfNotExist<UIGamepadSelectable>();
			uIGamepadSelectable.Priority = ((_config.ButtonType != PopUpButtonConfiguration.EButtonType.eSuggestedClick) ? 2 : 3);
		}

		private void OnRepositionContentBox()
		{
			m_content.OnLayoutFinished();
			UIUtil.UpdateColliderIfTimerPaused();
			if (base.ScreenId == EScreenID.UIPopUp)
			{
				m_openAnimation.Play("UIPopupAppear");
			}
			else
			{
				m_openAnimation.Play("UIPopupAppearBare");
			}
		}

		private void ConfigureHeading(string _heading)
		{
			if (string.IsNullOrEmpty(_heading))
			{
				HeaderContainer.gameObject.SetActive(false);
				return;
			}
			Header.text = _heading;
			HeaderContainer.gameObject.SetActive(true);
			Header.parent.ResetAnchors();
			Header.parent.UpdateAnchors();
		}

		private void ConfigContent(PopUpContentData _data, PopUpConfiguration _config)
		{
			string text = this[_data.Type];
			if (!string.IsNullOrEmpty(text))
			{
				GameObject gameObject = Resources.Load(text, typeof(GameObject)) as GameObject;
				UIWidget component = m_contentBox.GetComponent<UIWidget>();
				UIWidget component2 = gameObject.GetComponent<UIWidget>();
				component.width = component2.width;
				GameObject gameObject2 = NGUITools.AddChild(m_contentParent, gameObject);
				m_content = gameObject2.GetComponent<PopUpContentView>();
				m_content.SetData(_data);
				UIWidget component3 = gameObject2.GetComponent<UIWidget>();
				component3.leftAnchor.target = m_contentParent.transform;
				component3.rightAnchor.target = m_contentParent.transform;
				component3.leftAnchor.absolute = 0;
				component3.rightAnchor.absolute = 0;
				switch (_config.Aligntarget)
				{
				case EContentAlignTarget.Top:
					m_BackgroundFrame.topAnchor.absolute = Header.height;
					break;
				case EContentAlignTarget.Heading:
					m_BackgroundFrame.topAnchor.absolute = Header.height + 20;
					break;
				case EContentAlignTarget.CloseButton:
				{
					UIWidget component4 = m_closeButton.GetComponent<UIWidget>();
					m_BackgroundFrame.topAnchor.absolute = component4.height + Math.Abs(component4.topAnchor.absolute);
					break;
				}
				case EContentAlignTarget.CloseButtonOverlay:
					m_BackgroundFrame.topAnchor.absolute = 0;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void ConfigureCloseButton(bool _show, GameID _audioControllerId)
		{
			m_closeButton.gameObject.SetActive(_show);
			if (_show)
			{
				if (!GameID.IsValid(_audioControllerId))
				{
					_audioControllerId = DefaultAudioControllers.Close;
				}
				CloseButtonAudioControllerId = _audioControllerId;
				EventDelegate.Add(m_closeButton.onClick, OnCloseClicked);
				Header.parent.rightAnchor.target = m_closeButton.transform;
				Header.parent.rightAnchor.relative = 0f;
				Header.parent.rightAnchor.absolute = 0;
				UIGamepadSelectable uIGamepadSelectable = m_closeButton.Button.gameObject.AddComponentIfNotExist<UIGamepadSelectable>();
				uIGamepadSelectable.IsBackButton = true;
				uIGamepadSelectable.Priority = 1;
			}
			else
			{
				Header.parent.rightAnchor.target = m_BackgroundFrame.transform;
				Header.parent.rightAnchor.relative = 1f;
				Header.parent.rightAnchor.absolute = -20;
			}
		}

		private void OnCloseClicked()
		{
			if (m_closeButtonAudioHandle != null)
			{
				m_closeButtonAudioHandle.Play();
				CloseButtonAudioControllerId = GameID.Invalid;
			}
			if (m_closeHandler != null)
			{
				m_closeHandler();
			}
		}

		private void OnChangingConfigOnTweenFinished()
		{
			Configure(m_nextContent);
		}
	}
}
