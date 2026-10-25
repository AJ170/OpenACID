using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	public class CreditsScreenView : UIScreenChild
	{
		[SerializeField]
		private UIButton m_backButton;

		[SerializeField]
		private Transform m_alignTarget;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private CreditsWrapContent m_content;

		[SerializeField]
		private UITexture m_texture1;

		[SerializeField]
		private UITexture m_texture2;

		[SerializeField]
		private UITexture m_logo;

		[SerializeField]
		private float m_scrollSpeed = 100f;

		[SerializeField]
		private float m_scrollRestartWaitTime = 1f;

		[SerializeField]
		private float m_scrollAccelerateTime = 2f;

		[SerializeField]
		private float m_textureWaitTime = 5f;

		[SerializeField]
		private float m_textureFadeInTime = 1f;

		[SerializeField]
		private float m_textureFadeOutTime = 0.5f;

		[SerializeField]
		private float m_imageFadeTimeOffset = 0.5f;

		[SerializeField]
		private float m_imageFadeTime = 1f;

		[SerializeField]
		private float m_logoFadeTime = 1f;

		[SerializeField]
		private float m_initFadePosition = 0.05f;

		private float m_scrollWaitClock;

		private float m_scrollAccelerateClock = 1f;

		private float m_scrollSpeedActual;

		private bool m_screenActive;

		private int m_curTex;

		private int m_curMap;

		private readonly UITexture[] m_texture = new UITexture[2];

		private readonly bool[] m_loaded = new bool[2];

		private readonly bool[] m_loading = new bool[2];

		private float m_textureWait;

		private float m_textureFade;

		private float m_textureFadeTime = 1f;

		private float m_logoFade;

		private float m_imageFade;

		private BBList<UnityResource> m_maps;

		private BBList<Texture> m_loadedTextures;

		private float m_imageAlpha;

		private bool m_staticPopupWasEnabled;

		public float ScrollPosition
		{
			get
			{
				return m_scrollView.transform.localPosition.y;
			}
		}

		private void Update()
		{
			if (m_screenActive)
			{
				UpdateScrolling();
				UpdateImages();
				UpdateLogo();
			}
		}

		private void UpdateScrolling()
		{
			if (m_scrollView.isDragging)
			{
				return;
			}
			if (ScrollPosition < 0f - m_scrollView.panel.clipSoftness.y)
			{
				m_scrollView.ResetPosition();
				m_scrollWaitClock = 0f;
				m_scrollAccelerateClock = 1f;
			}
			if (m_scrollWaitClock > 0f)
			{
				m_scrollWaitClock -= Time.deltaTime;
				m_scrollAccelerateClock = 0f;
				return;
			}
			if (m_scrollAccelerateClock < 1f)
			{
				m_scrollAccelerateClock += Time.deltaTime / m_scrollAccelerateTime;
			}
			m_scrollAccelerateClock = Mathf.Clamp01(m_scrollAccelerateClock);
			float y = Mathf.SmoothStep(0f, 1f, m_scrollAccelerateClock) * m_scrollSpeedActual * Time.deltaTime;
			m_scrollView.MoveRelative(new Vector3(0f, y, 0f));
			if (ScrollPosition >= m_content.ExitScrollPosition)
			{
				OnBackButtonClicked();
			}
		}

		private void UpdateImages()
		{
			int num = 1 - m_curTex;
			if (!m_loading[num] && !m_loaded[num])
			{
				StartCoroutine(co_LoadTexture());
			}
			float fadeOffset = m_scrollAccelerateClock * m_scrollSpeedActual * (m_imageFadeTime + m_imageFadeTimeOffset);
			UpdateFade(ref m_imageFade, m_imageFadeTime, fadeOffset, false);
			m_imageAlpha = Mathf.SmoothStep(0f, 1f, m_imageFade);
			if (ScrollPosition <= 0f)
			{
				m_textureWait = m_textureWaitTime;
				m_textureFade = 0f;
			}
			if (!m_loaded[m_curTex])
			{
				m_textureWait = 0f;
			}
			if (m_textureWait > 0f || !m_loaded[num])
			{
				if (m_imageFade >= 1f && m_textureWait > 0f)
				{
					m_textureWait -= Time.deltaTime;
				}
				SetTextureAlpha(1f, 0f);
				return;
			}
			if (m_textureFade >= m_textureFadeTime)
			{
				SetTextureAlpha(0f, 1f);
				m_loaded[m_curTex] = false;
				m_curTex = num;
				m_textureWait = m_textureWaitTime;
				m_textureFade = 0f;
				return;
			}
			if (m_textureFade <= 0f)
			{
				m_textureFadeTime = m_textureFadeInTime + m_textureFadeOutTime;
				m_textureFade = 0f;
				SetTextureAlpha(1f, 0f);
				m_texture[m_curTex].depth = 1;
				m_texture[num].depth = 2;
			}
			else if (m_textureFade <= m_textureFadeInTime)
			{
				float next = Mathf.SmoothStep(0f, 1f, m_textureFade / m_textureFadeInTime);
				SetTextureAlpha(1f, next);
			}
			else
			{
				float current = Mathf.SmoothStep(1f, 0f, (m_textureFade - m_textureFadeInTime) / m_textureFadeOutTime);
				SetTextureAlpha(current, 1f);
			}
			if (m_imageFade >= 1f)
			{
				m_textureFade += Time.deltaTime;
			}
		}

		private void UpdateLogo()
		{
			float fadeOffset = (0f - m_scrollAccelerateClock) * m_scrollSpeedActual * m_logoFadeTime;
			if (UpdateFade(ref m_logoFade, m_logoFadeTime, fadeOffset, true))
			{
				float num = Mathf.SmoothStep(0f, 1f, m_logoFade);
				bool flag = num > 0.0019607844f;
				m_logo.gameObject.SetActive(flag);
				if (flag)
				{
					m_logo.color = ToColor(num);
				}
			}
		}

		private bool UpdateFade(ref float _fade, float _fadeTime, float _fadeOffset, bool _invert)
		{
			float num = ((!_invert) ? (-1f) : 1f);
			float scrollPosition = ScrollPosition;
			if (scrollPosition > m_initFadePosition * m_content.PanelHeight + _fadeOffset)
			{
				num *= -1f;
			}
			float num2 = _fade;
			_fade += num * Time.deltaTime / _fadeTime;
			_fade = Mathf.Clamp01(_fade);
			return _fade != num2;
		}

		protected override void OnScreenInit()
		{
			m_logo.mainTexture = UIImageLocalizationHelper.GetLogoForCurrentLanguage();
			EventDelegate.Add(m_screen.onEnter, OnScreenEnter);
			EventDelegate.Add(m_screen.onExit, OnScreenExit);
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onEnter, OnScreenEnter);
				EventDelegate.Remove(m_screen.onExit, OnScreenExit);
			}
		}

		private void OnScreenEnter()
		{
			AudioManager.Instance.EventManager.OnEnterCredits();
			UISceneData sceneData = Singleton<UIScreenManager>.Instance.SceneData;
			sceneData.UIMenuBars.Exclude();
			sceneData.BackButton.gameObject.SetActive(false);
			GameObject gameObject = sceneData.StaticPopup.gameObject;
			m_staticPopupWasEnabled = gameObject.activeSelf;
			gameObject.SetActive(false);
			m_content.Init();
			m_scrollView.ResetPosition();
			m_imageFade = 0f;
			m_logoFade = 0f;
			float num = (m_content.FontScale - 1f) * 0.3f + 1f;
			m_scrollSpeedActual = m_scrollSpeed * num;
			m_textureWait = 0f;
			m_textureFade = 0f;
			m_curTex = 0;
			m_curMap = 0;
			m_texture[0] = m_texture1;
			m_texture[1] = m_texture2;
			for (int i = 0; i < 2; i++)
			{
				m_loaded[i] = false;
				m_loading[i] = false;
				m_texture[i].color = ToColor(0f);
			}
			List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<RegionDefinition>();
			m_maps = new BBList<UnityResource>(items.Count);
			m_loadedTextures = new BBList<Texture>(items.Count);
			foreach (KeyValuePair<GameID, DBEntry> item in items)
			{
				RegionDefinition regionDefinition = item.Value as RegionDefinition;
				if (regionDefinition == null || regionDefinition.Hidden || regionDefinition.Globe || regionDefinition.Age == EEra.Common || regionDefinition.Status == DBEntry.EStatus.Obsolete)
				{
					continue;
				}
				for (int j = 0; j < regionDefinition.AppearanceOptions.Count; j++)
				{
					if (regionDefinition.AppearanceOptions[j].Map != null && !string.IsNullOrEmpty(regionDefinition.AppearanceOptions[j].Map.GetResourcePath()))
					{
						m_maps.Add(regionDefinition.AppearanceOptions[j].Map);
						m_loadedTextures.Add(null);
					}
				}
				m_maps.Add(regionDefinition.Map);
				m_loadedTextures.Add(null);
			}
			m_maps.Shuffle(new Random32((uint)UbiCore.Utils.Singleton<Timer>.Instance.realTime));
			m_screenActive = true;
		}

		private void OnScreenExit()
		{
			AudioManager.Instance.EventManager.OnLeaveCredits();
			m_content.Exit();
			UISceneData sceneData = Singleton<UIScreenManager>.Instance.SceneData;
			sceneData.UIMenuBars.Show();
			sceneData.BackButton.gameObject.SetActive(true);
			sceneData.StaticPopup.gameObject.SetActive(m_staticPopupWasEnabled);
			if (m_maps != null)
			{
				m_maps.Clear();
			}
			m_maps = null;
			for (int i = 0; i < 2; i++)
			{
				m_texture[i].ClearTexture();
			}
			if (m_loadedTextures != null)
			{
				for (int j = 0; j < m_loadedTextures.Count; j++)
				{
					if (m_loadedTextures[j] != null)
					{
						UnityEngine.Resources.UnloadAsset(m_loadedTextures[j]);
						m_loadedTextures[j] = null;
					}
				}
				m_loadedTextures = null;
			}
			if (Main.IsInitialized)
			{
				Main.Instance.RequestResourceUnload();
			}
			m_screenActive = false;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_backButton.onClick, OnBackButtonClicked);
			UIScrollView scrollView = m_scrollView;
			scrollView.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Combine(scrollView.onDragFinished, new UIScrollView.OnDragNotification(OnDragFinished));
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_backButton.onClick, OnBackButtonClicked);
			UIScrollView scrollView = m_scrollView;
			scrollView.onDragFinished = (UIScrollView.OnDragNotification)Delegate.Remove(scrollView.onDragFinished, new UIScrollView.OnDragNotification(OnDragFinished));
		}

		public override void OnBackKey()
		{
			OnBackButtonClicked();
		}

		private void OnBackButtonClicked()
		{
			EScreenID eScreenID = Singleton<UIScreenManager>.Instance.LastValidScreenId;
			switch (eScreenID)
			{
			case EScreenID.AssassinCreation:
				eScreenID = EScreenID.AssassinSelection;
				break;
			case EScreenID.MysteryBox:
				eScreenID = EScreenID.InventoryScreen;
				if (Singleton<StateManager>.Instance.ProfileManager.Profile.IsMysteryBoxOpen())
				{
					eScreenID = EScreenID.MysteryBox;
				}
				break;
			}
			Singleton<UIScreenManager>.Instance.ChangeScreen(eScreenID, new UITweenHandle(null, null));
		}

		private void OnDragFinished()
		{
			m_scrollWaitClock = m_scrollRestartWaitTime;
		}

		private IEnumerator co_LoadTexture()
		{
			int nextTex = 1 - m_curTex;
			m_loading[nextTex] = true;
			m_curMap++;
			if (m_curMap >= m_maps.Count)
			{
				m_curMap = 0;
			}
			UnityResource resource = m_maps[m_curMap];
			Texture mapTexture2 = null;
			if (m_loadedTextures[m_curMap] == null)
			{
				yield return Main.Instance.StartCoroutine(resource.PrepareAsync());
				mapTexture2 = resource.AccessObject<Texture>();
				m_loadedTextures[m_curMap] = mapTexture2;
			}
			else
			{
				mapTexture2 = m_loadedTextures[m_curMap];
			}
			m_texture[nextTex].mainTexture = mapTexture2;
			m_loading[nextTex] = false;
			m_loaded[nextTex] = m_screenActive;
		}

		private static Color ToColor(float _alpha)
		{
			return new Color(1f, 1f, 1f, _alpha);
		}

		private void SetTextureAlpha(float _current, float _next)
		{
			m_texture[m_curTex].color = ToColor(Mathf.Min(m_imageAlpha, _current));
			m_texture[1 - m_curTex].color = ToColor(Mathf.Min(m_imageAlpha, _next));
		}
	}
}
