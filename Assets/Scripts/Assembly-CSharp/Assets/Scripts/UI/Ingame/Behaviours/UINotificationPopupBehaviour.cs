using System;
using System.Collections;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.NGUIExtentions;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UINotificationPopupBehaviour : UIBaseBehaviour
	{
		public enum ETextureType
		{
			NormalTexture = 0,
			Achievement = 1
		}

		[SerializeField]
		private UIExtendedLabel m_headerLabel;

		[SerializeField]
		private UIExtendedLabel m_notificationLabel;

		[SerializeField]
		private UITexture m_normalTexture;

		[SerializeField]
		private UIAchievementTexture m_achievementTexture;

		[SerializeField]
		private UnityEngine.Animation m_animationClip;

		[SerializeField]
		private UIPlayAudio m_audioShowInfo;

		[SerializeField]
		private UIExtendedLabel m_progressText;

		[SerializeField]
		private UIWidget m_progressBarContainer;

		[SerializeField]
		private UIProgressBar m_progressBar;

		[SerializeField]
		private BBTweenProgressBar m_tweenProgressBar;

		[SerializeField]
		private BBTweenTickingCounterText m_tweenProgressText;

		[SerializeField]
		private Texture[] m_achievementRankTextures;

		private ETextureType m_textureType;

		private float m_showTime;

		private bool m_isWaitingForImageToLoad;

		private bool m_isFadingOut;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Notification;
			}
		}

		public event Action Hidden;

		public void ShowNotification(string _headerText, string _infoText, ETextureType _type, int _rank, Texture _texture, int _calculatedAnimationTime)
		{
			m_textureType = _type;
			m_headerLabel.text = _headerText;
			m_notificationLabel.text = _infoText;
			m_notificationLabel.gameObject.SetActive(true);
			m_showTime = _calculatedAnimationTime;
			m_isFadingOut = false;
			m_notificationLabel.gameObject.SetActive(true);
			m_progressBarContainer.gameObject.SetActive(false);
			Show();
			if (base.gameObject.activeInHierarchy)
			{
				UpdateTexture(_texture, _type, _rank);
			}
		}

		public void ShowNotification(string _headerText, string _infoText, ETextureType _type, UnityResource _texResource, int _calculatedAnimationTime, int _rank = 0)
		{
			m_textureType = _type;
			m_headerLabel.text = _headerText;
			m_notificationLabel.text = _infoText;
			m_showTime = _calculatedAnimationTime;
			m_isFadingOut = false;
			m_notificationLabel.gameObject.SetActive(true);
			m_progressBarContainer.gameObject.SetActive(false);
			Show();
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(UpdateNotificationTexture(_texResource, _type, _rank));
			}
		}

		public void ShowNotification(string _headerText, long _startValue, long _endValue, long _progress, string _resourceGuid, string _texResourcePath, int _calculatedAnimationTime)
		{
			m_textureType = ETextureType.NormalTexture;
			m_headerLabel.text = _headerText;
			m_showTime = _calculatedAnimationTime;
			m_isFadingOut = false;
			m_tweenProgressText.from = _startValue;
			m_tweenProgressText.to = _progress;
			m_tweenProgressText.StringSufix = " / " + LocalizationManager.Instance.GetInteger(_endValue);
			m_tweenProgressText.SetTweenToBeginning();
			m_tweenProgressBar.from = (float)_startValue / (float)_endValue;
			m_tweenProgressBar.to = (float)_progress / (float)_endValue;
			m_progressBar.value = (float)_startValue / (float)_endValue;
			m_progressBar.ForceUpdate();
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("value", LocalizationManager.Instance.GetInteger(_startValue));
			stringMapper.Add("max_value", LocalizationManager.Instance.GetInteger(_endValue));
			StringMapper mapper = stringMapper;
			string text = LocalizationManager.Instance.Get(5630, mapper);
			m_progressText.text = text;
			m_notificationLabel.gameObject.SetActive(false);
			m_progressBarContainer.gameObject.SetActive(true);
			Show();
			if (base.gameObject.activeInHierarchy)
			{
				if (string.IsNullOrEmpty(_resourceGuid))
				{
					UpdateNotificationTextureByResourcePath(_texResourcePath);
				}
				else
				{
					UpdateNotificationTextureByGuid(_resourceGuid);
				}
			}
		}

		public override void SetVisibility(bool _visible)
		{
			m_animationClip.Stop();
			m_visible = _visible;
			if (_visible)
			{
				m_animationClip.Rewind();
				m_animationClip.Play("NotificationHighlightAppear");
			}
			else
			{
				m_showTime = 0f;
				m_isFadingOut = true;
				m_animationClip.Play("NotificationHighlightDisappear");
			}
		}

		public void Update()
		{
			m_animationClip.UpdateAnimationSpeed();
		}

		public void UpdateNotification()
		{
			if (!base.gameObject.activeInHierarchy || m_isWaitingForImageToLoad)
			{
				return;
			}
			if (!UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
			{
				m_showTime -= RealTime.deltaTime;
				if (m_showTime < 0f && !m_isFadingOut)
				{
					SetVisibility(false);
				}
				else if (m_isFadingOut && !m_animationClip.isPlaying)
				{
					OnHidden();
					Exclude();
				}
			}
			else if (m_showTime <= 0f || m_isFadingOut)
			{
				OnHidden();
				Exclude();
			}
		}

		public void Init()
		{
			base.gameObject.GetComponent<UIPanel>().alpha = 0f;
			Exclude();
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void OnAbortAnimation()
		{
			if (m_showTime > 0f && !m_isFadingOut)
			{
				SetVisibility(false);
			}
		}

		protected void OnHidden()
		{
			Action hidden = this.Hidden;
			if (hidden != null)
			{
				hidden();
			}
		}

		private void Awake()
		{
		}

		private void OnDisabled()
		{
			OnHidden();
		}

		private void OnTextureUpdated(ItemDef _itemDef)
		{
			if (m_textureType == ETextureType.NormalTexture)
			{
				m_normalTexture.gameObject.SetActive(true);
			}
			else if (m_textureType == ETextureType.Achievement)
			{
				m_achievementTexture.gameObject.SetActive(true);
			}
			SetVisibility(true);
			if (m_audioShowInfo != null)
			{
				m_audioShowInfo.Play();
			}
			if (m_progressBarContainer.gameObject.activeSelf)
			{
				m_tweenProgressBar.StartTweenPlayForward();
				m_tweenProgressText.StartTweenPlayForward();
			}
			m_isWaitingForImageToLoad = false;
		}

		private void UpdateNotificationTextureByResourcePath(string _path)
		{
			m_isWaitingForImageToLoad = true;
			m_normalTexture.gameObject.SetActive(false);
			m_achievementTexture.gameObject.SetActive(false);
			if (!string.IsNullOrEmpty(_path))
			{
				Texture2D mainTexture = (Texture2D)Resources.Load(_path, typeof(Texture2D));
				m_normalTexture.mainTexture = mainTexture;
			}
			OnTextureUpdated(null);
		}

		private void UpdateNotificationTextureByGuid(string _guid)
		{
			m_isWaitingForImageToLoad = true;
			m_normalTexture.gameObject.SetActive(false);
			m_achievementTexture.gameObject.SetActive(false);
			if (!string.IsNullOrEmpty(_guid))
			{
				UIItemUtil.UpdateUITexture(_guid, m_normalTexture, OnTextureUpdated);
			}
			else
			{
				OnTextureUpdated(null);
			}
		}

		private IEnumerator UpdateNotificationTexture(UnityResource _resource, ETextureType _type, int _rankIconIndex = 0)
		{
			m_isWaitingForImageToLoad = true;
			m_normalTexture.gameObject.SetActive(false);
			m_achievementTexture.gameObject.SetActive(false);
			yield return Main.Instance.StartCoroutine(_resource.PrepareAsync());
			Texture texture = _resource.AccessObject<Texture>();
			UpdateTexture(texture, _type, _rankIconIndex);
		}

		private void UpdateTexture(Texture _texture, ETextureType _type, int _rankIconIndex)
		{
			if (_texture != null)
			{
				switch (_type)
				{
				case ETextureType.NormalTexture:
					m_normalTexture.mainTexture = _texture;
					break;
				case ETextureType.Achievement:
					m_achievementTexture.SetMaterialData(_texture, m_achievementRankTextures[_rankIconIndex], 0f);
					break;
				}
			}
			OnTextureUpdated(null);
		}
	}
}
