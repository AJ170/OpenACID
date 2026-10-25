using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup
{
	public class UIInfoPopup : MonoBehaviour
	{
		public enum EContentType
		{
			InventoryItem = 0,
			Module = 1,
			Outfits = 2,
			MysteryBox = 3,
			Blueprint = 4,
			Entitlement = 5,
			BasicContent = 6,
			UnknownItem = 7,
			BasicContentWithCount = 8
		}

		[Serializable]
		private sealed class PrefabHandle
		{
			public EContentType Type;

			public string PrefabPath;

			public GameObject CachedInstance { get; set; }
		}

		public enum EPlacementSide
		{
			Auto = -1,
			Left = 0,
			Right = 1
		}

		private const string c_showInfoPopupName = "ShowInfoPopup";

		private const string c_hideInfoPopupName = "HideInfoPopup";

		public static BBList<UIInfoPopup> InfoPopupInstanceList = new BBList<UIInfoPopup>();

		[HideInInspector]
		[SerializeField]
		private List<PrefabHandle> m_prefabs = new List<PrefabHandle>();

		[SerializeField]
		private bool m_createArrow;

		[SerializeField]
		private UIWidget m_arrowContainer;

		[SerializeField]
		private UIWidget m_fadeAtBoarderContainer;

		[SerializeField]
		private bool m_fadeAtBorder = true;

		[SerializeField]
		private UIWidget m_backgroundFrame;

		[SerializeField]
		private UITexture m_qualityTexture;

		[SerializeField]
		private UIExtendedLabel m_headerText;

		[SerializeField]
		private UIWidget m_headerContainer;

		[SerializeField]
		private UIButtonProxy m_closeButton;

		[SerializeField]
		private UIWidget m_buttonBar;

		[SerializeField]
		private UIWidget m_contentParent;

		[SerializeField]
		private UIButtonProxy m_singleButton;

		[SerializeField]
		private UIButtonProxy m_secondaryButton;

		[SerializeField]
		private UIButtonProxy m_primaryButton;

		[SerializeField]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		private FullscreenBlocker m_blocker;

		private UIInfoPopupArrowHandler m_arrowHandler;

		private GameObject m_currentContentObject;

		private InfoPopUpContentView m_content;

		private InfoPopUpConfiguration m_configuration;

		private InfoPopUpConfiguration.CloseButtonPressHandler m_closeHandler;

		private int m_delayColliderFixFrames;

		private bool m_playShowAnimation;

		public bool IsClosing { get; private set; }

		public bool IsEnabled
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public bool IsCloseButtonShown
		{
			get
			{
				if (m_configuration != null)
				{
					return m_configuration.ShowCloseButton;
				}
				return false;
			}
		}

		public Transform IndicatorTarget { get; set; }

		public EContentType CurrentContentType { get; set; }

		public FullscreenBlocker Blocker
		{
			get
			{
				return m_blocker;
			}
		}

		public UIButtonProxy SingleButton
		{
			get
			{
				return m_singleButton;
			}
		}

		public UIButtonProxy SecondaryButton
		{
			get
			{
				return m_secondaryButton;
			}
		}

		public UIButtonProxy PrimaryButton
		{
			get
			{
				return m_primaryButton;
			}
		}

		public string this[EContentType _type]
		{
			get
			{
				PrefabHandle prefabHandle = m_prefabs.Find((PrefabHandle _handle) => _handle.Type == _type);
				return (prefabHandle == null) ? null : prefabHandle.PrefabPath;
			}
			set
			{
				PrefabHandle prefabHandle = m_prefabs.Find((PrefabHandle _handle) => _handle.Type == _type);
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

		public event Action OnPopupClosedCallback;

		public event Action OnBeforePopupCloseCallback;

		public event Action OnPopupOpenedCallback;

		private void Awake()
		{
			if (m_createArrow)
			{
				m_arrowHandler = base.gameObject.AddComponent<UIInfoPopupArrowHandler>();
				m_arrowHandler.Init(m_arrowContainer, m_fadeAtBorder, m_fadeAtBoarderContainer);
			}
		}

		private void OnEnable()
		{
			InfoPopupInstanceList.Add(this);
		}

		public void ShowArrows(bool _showArrows)
		{
			if (m_arrowHandler != null)
			{
				m_arrowHandler.IsActive = _showArrows;
			}
		}

		public UIPortraitTexture GetPortraitTexture()
		{
			return m_content.GetPortraitTexture();
		}

		public void ReActivateButtons()
		{
			if (m_configuration != null)
			{
				ConfigButtons(m_configuration.Buttons);
			}
		}

		public void Close()
		{
			if (IsEnabled)
			{
				SetButtonsActive(false);
				OnBeforePopupClose();
				IsClosing = true;
				if (base.gameObject.activeInHierarchy)
				{
					DisableWithAnimation();
				}
				else
				{
					DisableNow();
				}
			}
		}

		public void CloseWithDelayed(float _delayTime)
		{
			if (base.isActiveAndEnabled)
			{
				SetButtonsActive(false);
				StartCoroutine("CloseWithAnimationDelayed", _delayTime);
			}
			else
			{
				Close();
			}
		}

		public void CloseNow()
		{
			if (IsEnabled)
			{
				SetButtonsActive(false);
				DisableNow();
			}
		}

		public virtual void Configure(InfoPopUpConfiguration _conf)
		{
			m_playShowAnimation = !IsEnabled;
			IsEnabled = true;
			IsClosing = false;
			m_animation.Stop();
			StopCoroutine("WaitBeforeDisabling");
			StopCoroutine("CloseWithAnimationDelayed");
			Clear();
			m_configuration = _conf;
			m_delayColliderFixFrames = 2;
			CurrentContentType = _conf.Data.Type;
			m_closeHandler = _conf.OnCloseButtonClicked;
			ConfigureCloseButton(_conf.ShowCloseButton);
			ConfigureHeading(_conf.Heading);
			ConfigButtons(_conf.Buttons);
			ConfigContent(_conf.Data, _conf);
			if (_conf.Buttons == null || _conf.Buttons.Length == 0)
			{
				m_contentParent.transform.parent.GetComponent<UIRect>().Update();
				m_contentParent.bottomAnchor.Set(m_contentParent.transform.parent, 0f, 0f);
			}
			else
			{
				m_buttonBar.Update();
				m_contentParent.bottomAnchor.Set(m_buttonBar.transform, 1f, 0f);
			}
			m_contentParent.ResetAnchors();
			m_contentParent.UpdateAnchors();
			m_contentParent.Update();
			UpdateLayouts();
		}

		private GameObject GetContentObject(EContentType _type)
		{
			PrefabHandle prefabHandle = m_prefabs.Find((PrefabHandle _handle) => _handle.Type == _type);
			if (prefabHandle != null && !string.IsNullOrEmpty(prefabHandle.PrefabPath))
			{
				GameObject gameObject = prefabHandle.CachedInstance;
				if (gameObject == null)
				{
					GameObject prefab = Resources.Load(prefabHandle.PrefabPath) as GameObject;
					gameObject = (prefabHandle.CachedInstance = NGUITools.AddChild(m_contentParent.gameObject, prefab));
					if (gameObject != null)
					{
						int depth = GetComponent<UIPanel>().depth;
						GameObject gameObject3 = m_arrowContainer.transform.parent.gameObject;
						gameObject3.GetComponent<UIPanel>().depth += depth;
						InfoPopUpContentView component = gameObject.GetComponent<InfoPopUpContentView>();
						if (component != null)
						{
							component.UpdateChildPannelsDepth(depth);
						}
					}
				}
				return gameObject;
			}
			return null;
		}

		private void ConfigureHeading(string _heading)
		{
			if (string.IsNullOrEmpty(_heading))
			{
				m_headerContainer.gameObject.SetActive(false);
				return;
			}
			m_headerText.text = LocalizationManager.Instance.ClearNGUIFormat(_heading);
			m_headerContainer.gameObject.SetActive(true);
			m_headerText.parent.ResetAnchors();
			m_headerText.parent.UpdateAnchors();
		}

		private void ConfigContent(InfoPopUpContentData _data, InfoPopUpConfiguration _config)
		{
			GameObject contentObject = GetContentObject(_data.Type);
			m_currentContentObject = contentObject;
			m_currentContentObject.SetActive(true);
			m_content = m_currentContentObject.GetComponent<InfoPopUpContentView>();
			m_content.SetData(_data);
			UIWidget component = m_currentContentObject.GetComponent<UIWidget>();
			component.SetAnchor(m_contentParent.gameObject, 0, 0, 0, 0);
			m_qualityTexture.color = _config.QualityColor;
			m_backgroundFrame.color = _config.FrameColor;
			m_headerText.color = _config.QualityTextColor;
			if (m_arrowHandler != null)
			{
				m_arrowHandler.ConfigContent(_config, IndicatorTarget);
			}
		}

		private void ConfigureCloseButton(bool _show)
		{
			m_closeButton.gameObject.SetActive(_show);
			if (_show)
			{
				EventDelegate.Add(m_closeButton.onClick, OnCloseClicked);
				m_headerText.parent.rightAnchor.target = m_closeButton.transform;
				m_headerText.parent.rightAnchor.relative = 0f;
				UIGamepadSelectable uIGamepadSelectable = m_closeButton.Button.gameObject.AddComponentIfNotExist<UIGamepadSelectable>();
				uIGamepadSelectable.IsBackButton = true;
				uIGamepadSelectable.Priority = 2;
			}
			else
			{
				m_headerText.parent.rightAnchor.target = m_backgroundFrame.transform;
				m_headerText.parent.rightAnchor.relative = 1f;
			}
		}

		private void ConfigButtons(PopUpButtonConfiguration[] _buttons)
		{
			bool flag = _buttons != null && _buttons.Length > 0;
			m_buttonBar.gameObject.SetActive(flag);
			if (flag)
			{
				bool flag2 = _buttons.Length == 1;
				if (flag2)
				{
					ConfigureButton(_buttons[0], m_singleButton.Button);
				}
				else
				{
					ConfigureButton(_buttons[0], m_primaryButton.Button);
					ConfigureButton(_buttons[1], m_secondaryButton.Button);
				}
				m_singleButton.gameObject.SetActive(flag2);
				m_secondaryButton.gameObject.SetActive(!flag2);
				m_primaryButton.gameObject.SetActive(!flag2);
			}
		}

		private void ConfigureButton(PopUpButtonConfiguration _config, UIExtendedButton _button)
		{
			EventDelegate.Add(_button.onClick, delegate
			{
				OnButtonClick(_config);
			});
			_button.Type = ((_config.ButtonType == PopUpButtonConfiguration.EButtonType.eSuggestedClick) ? EButtonTypes.Action : EButtonTypes.Normal);
			_button.AllowDisabledClick = _config.IsDisabledClickAllowed;
			_button.isEnabled = _config.IsEnabled;
			UIButtonLabelHandler component = _button.GetComponent<UIButtonLabelHandler>();
			component.SetText(_config.Label);
			if (!string.IsNullOrEmpty(_config.IconSpriteName))
			{
				component.SetIcon(Singleton<UIScreenManager>.Instance.GetAtlasByType(_config.IconSpriteAtlas), _config.IconSpriteName);
			}
			else if (_config.Resource != null)
			{
				component.ShowResource = true;
				component.ResourceType = _config.Resource.type;
			}
			else
			{
				component.ShowIcon = false;
				component.ShowResource = false;
			}
			UIButtonSounds component2 = _button.GetComponent<UIButtonSounds>();
			component2.ButtonIntent = _config.SoundIntent;
			UIGamepadSelectable uIGamepadSelectable = _button.gameObject.AddComponentIfNotExist<UIGamepadSelectable>();
			uIGamepadSelectable.Priority = ((_config.ButtonType != PopUpButtonConfiguration.EButtonType.eSuggestedClick) ? 9 : 10);
		}

		private void UpdateLayouts()
		{
			EventDelegate.Add(m_content.onLayoutFinished, OnContentLayoutFinished, true);
			m_content.UpdateLayout();
		}

		private void OnContentLayoutFinished()
		{
			m_content.OnLayoutFinished();
			UIUtil.UpdateColliderIfTimerPaused();
			if (m_playShowAnimation)
			{
				m_animation.Play("ShowInfoPopup");
			}
			else
			{
				m_animation.SampleAnimation(1f, "ShowInfoPopup");
			}
			if (this.OnPopupOpenedCallback != null)
			{
				this.OnPopupOpenedCallback();
			}
		}

		private void Clear()
		{
			if (m_currentContentObject != null)
			{
				m_currentContentObject.SetActive(false);
			}
			m_currentContentObject = null;
			if (m_singleButton.Isinstanciated)
			{
				m_singleButton.Button.onClick.Clear();
			}
			if (m_secondaryButton.Isinstanciated)
			{
				m_secondaryButton.Button.onClick.Clear();
			}
			if (m_primaryButton.Isinstanciated)
			{
				m_primaryButton.Button.onClick.Clear();
			}
			m_closeHandler = null;
			m_configuration = null;
		}

		private void OnDisable()
		{
			InfoPopupInstanceList.Remove(this);
			Clear();
			if (m_content != null)
			{
				m_content.Purge();
			}
			m_content = null;
			IsEnabled = false;
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_prefabs.Count; i++)
			{
				m_prefabs[i].CachedInstance = null;
			}
			m_arrowHandler = null;
			IndicatorTarget = null;
			if (m_contentParent != null)
			{
				m_contentParent.transform.ClearChildren();
			}
		}

		private void Update()
		{
			if (m_configuration != null)
			{
				m_animation.UpdateAnimationSpeed();
				if (!AcierInputHandler.Instance.EnableGamepadInAnimus)
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
			if (gamepad.CloseUp && m_configuration != null && m_configuration.ShowCloseButton)
			{
				OnCloseClicked();
			}
		}

		private void SetButtonsActive(bool _active)
		{
			m_singleButton.isEnabled = _active;
			m_secondaryButton.isEnabled = _active;
			m_primaryButton.isEnabled = _active;
		}

		private void OnButtonClick(PopUpButtonConfiguration _configuration)
		{
			if (_configuration.OnButtonPressed != null)
			{
				_configuration.OnButtonPressed(_configuration.Data);
			}
		}

		private void OnCloseClicked()
		{
			Close();
			if (m_closeHandler != null)
			{
				m_closeHandler();
			}
		}

		private void DisableWithAnimation()
		{
			if (IsEnabled)
			{
				m_animation.Play("HideInfoPopup");
				StartCoroutine("WaitBeforeDisabling");
			}
		}

		private IEnumerator WaitBeforeDisabling()
		{
			while (m_animation.isPlaying)
			{
				yield return 0;
			}
			DisableNow();
		}

		public IEnumerator CloseWithAnimationDelayed(float _delay)
		{
			yield return new WaitForSeconds(_delay);
			Close();
		}

		private void DisableNow()
		{
			IsEnabled = false;
			IsClosing = false;
			SetButtonsActive(true);
			if (m_arrowHandler != null)
			{
				m_arrowHandler.IsActive = false;
			}
			OnPopupClosed();
		}

		private void OnPopupClosed()
		{
			Action onPopupClosedCallback = this.OnPopupClosedCallback;
			if (onPopupClosedCallback != null)
			{
				onPopupClosedCallback();
			}
		}

		private void OnBeforePopupClose()
		{
			Action onBeforePopupCloseCallback = this.OnBeforePopupCloseCallback;
			if (onBeforePopupCloseCallback != null)
			{
				onBeforePopupCloseCallback();
			}
		}
	}
}
