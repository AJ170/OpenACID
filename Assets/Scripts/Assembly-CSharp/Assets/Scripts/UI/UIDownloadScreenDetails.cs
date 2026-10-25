using System;
using System.Collections;
using AcierNetwork;
using AcierNetwork.Communication;
using AcierNetwork.Provider;
using AssetBundleData;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Native;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtentions;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class UIDownloadScreenDetails : UIBaseBehaviour
	{
		public enum EState
		{
			None = 0,
			LoadingData = 1,
			LoadingMission = 2,
			WaitForTap = 3,
			Closing = 4
		}

		public class ImagePackage
		{
			public Texture Image;

			public DownloadScreenDef.EAlignment alignment;

			public UnityResource imageResource;

			public DownloadScreenDef.EColorType logoType;

			public DownloadScreenDef.EColorType privacyTextType;
		}

		public delegate void TapScreen();

		public delegate void ScreenClosed();

		public delegate void DownloadScreenClosed(bool fade);

		public TapScreen OnTapScreen;

		public ScreenClosed OnScreenClosed;

		public static DownloadScreenClosed DownloadScreenClosedEvent;

		[SerializeField]
		private GameObject m_progressSpinner;

		[SerializeField]
		private UILabel m_legalLabel;

		[SerializeField]
		private UISprite m_muteSprite;

		[SerializeField]
		private UISprite m_muteSpriteBackground;

		[SerializeField]
		private TweenAlpha m_downloadWidgetTween;

		[SerializeField]
		private UITexture m_TextureOne;

		[SerializeField]
		private UITexture m_SupportTexture;

		[SerializeField]
		private UITexture m_blackTexture;

		[SerializeField]
		private UITexture m_whiteTexture;

		[SerializeField]
		private TweenAlpha m_tweenOne;

		[SerializeField]
		private TweenAlpha m_blackLogo;

		[SerializeField]
		private TweenAlpha m_whiteLogo;

		[SerializeField]
		private UILabel m_downloadProgress;

		[SerializeField]
		private UILabel m_tipOfTheDay;

		[SerializeField]
		private TweenAlpha m_tipOfTheDayTween;

		[SerializeField]
		private BoxCollider m_tipOfTheDayGamePadCollider;

		[SerializeField]
		private UISprite m_tipOfTheDayBackground;

		[SerializeField]
		private UILabel m_statLabel;

		[SerializeField]
		private UILabel m_MB;

		[SerializeField]
		private UIWidget m_Logo;

		[SerializeField]
		private TweenColor m_TOSLink;

		[SerializeField]
		private int imageSwapTime = 8;

		[SerializeField]
		private int tipSwapTime = 30;

		private EState m_currentState;

		private int m_textureIndexOne;

		private int m_currentTip;

		private int m_tweensActive;

		private float m_imageCounter;

		private float m_tipCounter;

		private bool? m_blackLogoActive;

		private bool m_initialized;

		private bool m_isMusicAvailable;

		private readonly BBList<ImagePackage> imagePackageList = new BBList<ImagePackage>();

		private readonly BBList<int> m_tipsOfTheDayList = new BBList<int>();

		public static bool s_useTvShowPolicy;

		public bool CycleTips { get; set; }

		public bool TweenActive
		{
			get
			{
				return m_tweensActive != 0;
			}
			private set
			{
				if (value)
				{
					m_tweensActive++;
				}
				else
				{
					m_tweensActive--;
				}
			}
		}

		public string ScaleUnit
		{
			get
			{
				return m_MB.text;
			}
			set
			{
				m_MB.text = value;
			}
		}

		public string StatText
		{
			get
			{
				return m_statLabel.text;
			}
			set
			{
				m_statLabel.text = value;
			}
		}

		public string PrivacyPolicyUrl
		{
			get
			{
				return LocalizationManager.Instance.Get(3688);
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
				return UIScreenWidgetManager.EUiScreenWidget.DownloadScreen;
			}
		}

		private void Start()
		{
			m_downloadProgress.gameObject.SetActive(true);
			CycleTips = true;
			SetupMuteButton();
		}

		private void OnTweenFinished()
		{
			TweenActive = false;
		}

		public void OnLevelWasLoaded()
		{
			m_imageCounter = 0f - Time.unscaledDeltaTime;
		}

		private void Update()
		{
			if (UIDownloadScreen.Instance.ImageLoadingDone && !m_initialized && LocalizationManager.LanguageLoaded)
			{
				GetTips();
				imagePackageList.ClearIfNotEmpty();
				imagePackageList.AddRange(UIDownloadScreen.Instance.imagePackageList);
				m_initialized = true;
				m_textureIndexOne = UnityEngine.Random.Range(0, imagePackageList.Count - 1);
				m_currentTip = UnityEngine.Random.Range(0, m_tipsOfTheDayList.Count - 1);
				Texture logoForCurrentLanguage = UIImageLocalizationHelper.GetLogoForCurrentLanguage();
				m_whiteTexture.mainTexture = logoForCurrentLanguage;
				m_blackTexture.mainTexture = logoForCurrentLanguage;
				m_MB.alpha = 1f;
				m_Logo.alpha = 1f;
				SetNewImage(true);
				StartTipTween(SetTipOfTheDay);
			}
			if (m_initialized)
			{
				m_imageCounter += Time.unscaledDeltaTime;
				if (m_imageCounter > (float)imageSwapTime)
				{
					SetNewImage(false);
					m_imageCounter = 0f;
				}
				if (CycleTips)
				{
					m_tipCounter += Time.unscaledDeltaTime;
					if (m_tipCounter > (float)tipSwapTime)
					{
						StartTipTween(SetTipOfTheDay);
						m_tipCounter = 0f;
					}
				}
			}
			if (!m_isMusicAvailable && UIDownloadScreen.Instance.IsSoundReady)
			{
				m_isMusicAvailable = true;
				StartCoroutine(StartAudio());
			}
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			if (CurrentState == EState.WaitForTap)
			{
				UIGamepadHandler uIGamepadHandler = AcierInputHandler.Instance.UIGamepadHandler;
				if (!(uIGamepadHandler.CurrentSelection != null) && gamepad.ActionDown)
				{
					OnClick();
				}
			}
			if (m_isMusicAvailable && gamepad.PauseDown && UIGamepadHandler.CanHitCollider(m_muteSpriteBackground.gameObject))
			{
				OnMuteButtonRelease();
			}
			m_downloadProgress.gameObject.SetActive(!string.IsNullOrEmpty(UIDownloadScreen.Instance.ProgressValue) && UIDownloadScreen.Instance.ProgressValue != "0");
			m_downloadProgress.text = UIDownloadScreen.Instance.ProgressValue;
		}

		private IEnumerator StartAudio()
		{
			WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
			AssetBundleProviderInput input = new AssetBundleProviderInput
			{
				AssetBundles = new BBHashSet<AssetBundleInfo>()
			};
			foreach (AssetBundleInfo bi in Main.Instance.GameDB.GetAssetBundleInfos())
			{
				if (bi.Type == AssetBundleInfo.EType.SoundContainer)
				{
					input.AssetBundles.Add(bi);
				}
			}
			QueryResult result = null;
			NetworkProvider.Instance.Request(delegate(QueryResult _result, AssetBundleProviderResult _type)
			{
				result = _result;
			}, input);
			while (result == null || !result.IsCompleted)
			{
				yield return waitForSeconds;
			}
			UIPlayAudio plAudio = m_muteSprite.parent.gameObject.AddComponent<UIPlayAudio>();
			plAudio.AudioController = UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate;
			m_muteSprite.parent.gameObject.SetActive(true);
			AudioManager.Instance.EventManager.OnEnterDownloadScreen();
		}

		private void SetNewImage(bool _first)
		{
			ImagePackage activeImage = imagePackageList[m_textureIndexOne];
			activeImage.imageResource.ClearCache();
			m_textureIndexOne = (m_textureIndexOne + UnityEngine.Random.Range(0, imagePackageList.Count - 1)) % imagePackageList.Count;
			activeImage = imagePackageList[m_textureIndexOne];
			m_TextureOne.mainTexture = activeImage.imageResource.GetObject<Texture>();
			if (activeImage.privacyTextType == DownloadScreenDef.EColorType.Black)
			{
				SetAndPlayTweener(m_TOSLink, Color.white, Color.black);
			}
			else
			{
				SetAndPlayTweener(m_TOSLink, Color.black, Color.white);
			}
			if (activeImage.logoType == DownloadScreenDef.EColorType.Black && (!m_blackLogoActive.HasValue || m_blackLogoActive != true))
			{
				SetAndPlayTweener(m_whiteLogo, 1f, 0f);
				SetAndPlayTweener(m_blackLogo, 0f, 1f);
				m_blackLogoActive = true;
			}
			if (activeImage.logoType == DownloadScreenDef.EColorType.White && (!m_blackLogoActive.HasValue || m_blackLogoActive == true))
			{
				SetAndPlayTweener(m_whiteLogo, 0f, 1f);
				SetAndPlayTweener(m_blackLogo, 1f, 0f);
				m_blackLogoActive = false;
			}
			SetAlignment(m_TextureOne, activeImage.alignment);
			m_tweenOne.ResetToBeginning();
			if (EventDelegate.GetDelegate(m_tweenOne.onFinished, OnTweenFinished) == null)
			{
				TweenActive = true;
				EventDelegate.Add(m_tweenOne.onFinished, OnTweenFinished, true);
				EventDelegate.Add(m_tweenOne.onFinished, delegate
				{
					m_SupportTexture.mainTexture = m_TextureOne.mainTexture;
					SetAlignment(m_SupportTexture, activeImage.alignment);
					Main.Instance.RequestResourceUnload();
				}, true);
			}
			m_tweenOne.PlayForward();
		}

		private void SetAndPlayTweener(TweenAlpha _tweener, float _from, float _to)
		{
			if (_tweener.value != _to)
			{
				_tweener.from = _from;
				_tweener.to = _to;
				_tweener.ResetToBeginning();
				_tweener.PlayForward();
			}
		}

		private void SetAndPlayTweener(TweenColor _tweener, Color _from, Color _to)
		{
			if (_tweener.value != _to)
			{
				_tweener.from = _from;
				_tweener.to = _to;
				_tweener.ResetToBeginning();
				_tweener.PlayForward();
			}
		}

		public void StartTipTween(EventDelegate.Callback onFinished)
		{
			m_tipOfTheDayTween.from = 0f;
			m_tipOfTheDayTween.to = 1f;
			m_tipOfTheDayTween.ResetToBeginning();
			if (EventDelegate.GetDelegate(m_tipOfTheDayTween.onFinished, OnTweenFinished) == null)
			{
				TweenActive = true;
				EventDelegate.Add(m_tipOfTheDayTween.onFinished, OnTweenFinished, true);
			}
			EventDelegate.Add(m_tipOfTheDayTween.onFinished, onFinished, true);
			m_tipOfTheDayTween.PlayForward();
		}

		public void SetTipOfTheDayImpl(string _text)
		{
			if (!string.IsNullOrEmpty(_text))
			{
				m_tipOfTheDay.text = _text;
				m_tipOfTheDayTween.from = 1f;
				m_tipOfTheDayTween.to = 0f;
				m_tipOfTheDayTween.ResetToBeginning();
				if (m_tipOfTheDayBackground.alpha != 1f)
				{
					m_tipOfTheDayBackground.alpha = 1f;
				}
				if (EventDelegate.GetDelegate(m_tipOfTheDayTween.onFinished, OnTweenFinished) == null)
				{
					TweenActive = true;
					EventDelegate.Add(m_tipOfTheDayTween.onFinished, OnTweenFinished, true);
				}
				m_tipOfTheDayTween.PlayForward();
			}
		}

		private void SetTipOfTheDay()
		{
			int num = m_tipsOfTheDayList[m_currentTip];
			m_currentTip = (m_currentTip + UnityEngine.Random.Range(0, m_tipsOfTheDayList.Count - 1)) % m_tipsOfTheDayList.Count;
			SetTipOfTheDayImpl(LocalizationManager.Instance.Get(num));
		}

		private void GetTips()
		{
			m_tipsOfTheDayList.Clear();
			m_tipsOfTheDayList.Add(2351);
			m_tipsOfTheDayList.Add(2352);
			m_tipsOfTheDayList.Add(2353);
			m_tipsOfTheDayList.Add(2354);
			m_tipsOfTheDayList.Add(2355);
			m_tipsOfTheDayList.Add(2356);
			m_tipsOfTheDayList.Add(2357);
			m_tipsOfTheDayList.Add(2358);
			m_tipsOfTheDayList.Add(2359);
			m_tipsOfTheDayList.Add(2360);
			m_tipsOfTheDayList.Add(2361);
			m_tipsOfTheDayList.Add(2362);
			m_tipsOfTheDayList.Add(2363);
			m_tipsOfTheDayList.Add(2364);
			m_tipsOfTheDayList.Add(2365);
			m_tipsOfTheDayList.Add(2366);
			m_tipsOfTheDayList.Add(2367);
			m_tipsOfTheDayList.Add(2369);
			m_tipsOfTheDayList.Add(2370);
			m_tipsOfTheDayList.Add(2371);
			m_tipsOfTheDayList.Add(2372);
			m_tipsOfTheDayList.Add(2373);
			m_tipsOfTheDayList.Add(2374);
			m_tipsOfTheDayList.Add(2375);
			m_tipsOfTheDayList.Add(2376);
			m_tipsOfTheDayList.Add(2377);
			m_tipsOfTheDayList.Add(2378);
			m_tipsOfTheDayList.Add(2379);
			m_tipsOfTheDayList.Add(2380);
			m_tipsOfTheDayList.Add(2381);
			m_tipsOfTheDayList.Add(2382);
			m_tipsOfTheDayList.Add(2384);
			m_tipsOfTheDayList.Add(2385);
			m_tipsOfTheDayList.Add(2386);
			m_tipsOfTheDayList.Add(3807);
			m_tipsOfTheDayList.Add(7805);
			m_tipsOfTheDayList.Add(7806);
			m_tipsOfTheDayList.Add(7807);
		}

		private void SetAlignment(UITexture _tex, DownloadScreenDef.EAlignment _alignment)
		{
			switch (_alignment)
			{
			case DownloadScreenDef.EAlignment.Top:
				_tex.uvRect = new Rect(0f, -0.125f, 1f, 1f);
				_tex.pivot = UIWidget.Pivot.Top;
				break;
			case DownloadScreenDef.EAlignment.Center:
				_tex.uvRect = new Rect(0f, 0f, 1f, 1f);
				_tex.pivot = UIWidget.Pivot.Center;
				break;
			case DownloadScreenDef.EAlignment.Bottom:
				_tex.uvRect = new Rect(0f, 0.125f, 1f, 1f);
				_tex.pivot = UIWidget.Pivot.Bottom;
				break;
			}
		}

		private void Awake()
		{
			Texture splashScreenLogoForCurrentLanguage = UIImageLocalizationHelper.GetSplashScreenLogoForCurrentLanguage();
			m_SupportTexture.mainTexture = splashScreenLogoForCurrentLanguage;
			m_TextureOne.mainTexture = splashScreenLogoForCurrentLanguage;
		}

		public void Open()
		{
			CurrentState = EState.None;
			base.gameObject.GetComponent<Collider>().enabled = true;
			m_legalLabel.enabled = true;
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
				CurrentState = EState.None;
				if ((bool)m_downloadWidgetTween)
				{
					m_downloadWidgetTween.ResetToBeginning();
					m_downloadWidgetTween.delay = 0f;
					m_downloadWidgetTween.PlayForward();
				}
				SetupMuteButton();
			}
		}

		public void Close(bool _delayed)
		{
			CurrentState = EState.Closing;
			base.gameObject.GetComponent<Collider>().enabled = false;
			if ((bool)m_downloadWidgetTween)
			{
				EventDelegate.Add(m_downloadWidgetTween.onFinished, OnCloseTweenFinished, true);
				if (_delayed)
				{
					m_downloadWidgetTween.delay = 0.5f;
				}
				m_downloadWidgetTween.PlayReverse();
			}
			else
			{
				OnCloseTweenFinished();
			}
		}

		public void OnTOSTapped()
		{
			if (CurrentState != EState.Closing)
			{
				if (NativeTools.IsTVDevice || s_useTvShowPolicy)
				{
					NativeTools.ShowPrivacyPolicy(PrivacyPolicyUrl);
				}
				else
				{
					Application.OpenURL(PrivacyPolicyUrl);
				}
			}
		}

		public void OnWaitForTapClicked()
		{
			if (CurrentState == EState.WaitForTap)
			{
				OnClick();
			}
		}

		public void OnMuteButtonPress()
		{
			UpdateButtonBackground(true);
		}

		public void OnMuteButtonRelease()
		{
			if (UICamera.hoveredObject == m_muteSpriteBackground.gameObject)
			{
				SoundMenuNavigation.IsMute = !SoundMenuNavigation.IsMute;
				SetupMuteButton();
			}
			UpdateButtonBackground(false);
		}

		public void OnMuteDrag()
		{
			UpdateButtonBackground(UICamera.hoveredObject == m_muteSpriteBackground.gameObject);
		}

		private void SetupMuteButton()
		{
			bool isMute = SoundMenuNavigation.IsMute;
			m_muteSprite.spriteName = (isMute ? "aUI_icon_soundoff" : "aUI_icon_soundon");
			m_muteSprite.parent.gameObject.SetActive(m_isMusicAvailable && CurrentState == EState.LoadingData);
		}

		private void UpdateButtonBackground(bool _pressed)
		{
			m_muteSpriteBackground.spriteName = "hud_btn_toggle_" + ((!_pressed) ? "up" : "down");
		}

		private void OnClick()
		{
			if (OnTapScreen != null)
			{
				OnTapScreen();
			}
		}

		private void OnCloseTweenFinished()
		{
			base.gameObject.SetActive(false);
			m_SupportTexture.ClearTexture();
			m_TextureOne.ClearTexture();
			for (int i = 0; i < imagePackageList.Count; i++)
			{
				imagePackageList[i].imageResource.ClearCache();
			}
			imagePackageList.Clear();
			if (DownloadScreenClosedEvent != null)
			{
				DownloadScreenClosedEvent(true);
			}
			if (OnScreenClosed != null)
			{
				OnScreenClosed();
			}
		}

		private void UpdateState()
		{
			m_tipOfTheDayGamePadCollider.enabled = false;
			switch (m_currentState)
			{
			case EState.None:
				m_progressSpinner.SetActive(true);
				StatText = LocalizationManager.Instance.Get(4605);
				ScaleUnit = string.Empty;
				break;
			case EState.LoadingData:
				m_legalLabel.enabled = false;
				m_progressSpinner.SetActive(true);
				StatText = LocalizationManager.Instance.Get(3640);
				ScaleUnit = LocalizationManager.Instance.Get(4370);
				break;
			case EState.LoadingMission:
				m_legalLabel.enabled = false;
				m_progressSpinner.SetActive(true);
				StatText = LocalizationManager.Instance.Get(1414);
				ScaleUnit = LocalizationManager.Instance.Get(1708);
				break;
			case EState.WaitForTap:
				m_tipOfTheDayGamePadCollider.enabled = true;
				m_legalLabel.enabled = false;
				m_progressSpinner.SetActive(false);
				ScaleUnit = string.Empty;
				StatText = string.Empty;
				CycleTips = false;
				StartTipTween(delegate
				{
					SetTipOfTheDayImpl(LocalizationManager.Instance.Get(1415));
				});
				break;
			case EState.Closing:
				m_downloadWidgetTween.PlayForward();
				m_legalLabel.enabled = false;
				m_progressSpinner.SetActive(false);
				CycleTips = false;
				StatText = string.Empty;
				ScaleUnit = string.Empty;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			SetupMuteButton();
		}
	}
}
