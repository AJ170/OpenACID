using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIOnScreenMessageBehaviour : UIBaseBehaviour
	{
		[SerializeField]
		private UnityEngine.Animation m_labelAnimation;

		[SerializeField]
		private UnityEngine.Animation m_imageAnimation;

		[SerializeField]
		private UIExtendedLabel m_messageLabel;

		[SerializeField]
		private UITexture m_messageImage;

		private bool m_labelUsed;

		private bool m_imageUsed;

		private float m_showTime;

		private bool m_isFadingOut;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.OnScreenMessage;
			}
		}

		public event Action Hidden;

		public void ShowMessage(UIOnScreenMessage.QueueMessage _message, int _displayTime)
		{
			if (!string.IsNullOrEmpty(_message.Message) || !(_message.Texture == null))
			{
				m_showTime = _displayTime;
				m_imageAnimation.gameObject.SetActive(false);
				m_labelAnimation.gameObject.SetActive(false);
				m_labelUsed = !string.IsNullOrEmpty(_message.Message);
				if (m_labelUsed)
				{
					m_messageLabel.text = _message.Message;
				}
				m_imageUsed = _message.Texture != null;
				if (m_imageUsed)
				{
					m_messageImage.mainTexture = _message.Texture;
				}
				m_isFadingOut = false;
				SetVisibility(true);
			}
		}

		public override void SetVisibility(bool _visible)
		{
			base.SetVisibility(_visible);
			if (_visible)
			{
				base.gameObject.SetActive(true);
				StopAllCoroutines();
				if (m_labelUsed)
				{
					StartCoroutine(OnShowMessage(m_labelAnimation));
				}
				if (m_imageUsed)
				{
					StartCoroutine(OnShowMessage(m_imageAnimation));
				}
			}
			else
			{
				m_showTime = 0f;
				m_isFadingOut = true;
				OnHidden();
			}
		}

		private IEnumerator OnShowMessage(UnityEngine.Animation _animation)
		{
			UIWidget widget = _animation.gameObject.GetComponent<UIWidget>();
			_animation.gameObject.SetActive(true);
			_animation.SampleAnimation(0f, string.Empty);
			if (widget != null)
			{
				widget.alpha = 0f;
			}
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			yield return wait;
			if (widget != null)
			{
				widget.alpha = 1f;
			}
			_animation.PlayFirstClip();
			yield return new WaitForSeconds(0.6f);
			while (_animation.isPlaying)
			{
				yield return wait;
			}
		}

		public void UpdateScreenMessage()
		{
			if (base.gameObject.activeSelf && !UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
			{
				m_showTime -= RealTime.deltaTime;
				if (m_showTime < 0f && !m_isFadingOut)
				{
					SetVisibility(false);
					m_isFadingOut = true;
				}
			}
		}

		protected virtual void OnHidden()
		{
			if (this.Hidden != null)
			{
				this.Hidden();
			}
		}

		private void Update()
		{
			if (m_labelUsed)
			{
				m_labelAnimation.UpdateAnimationSpeed();
			}
			if (m_imageUsed)
			{
				m_imageAnimation.UpdateAnimationSpeed();
			}
		}

		public static string GetHeader(InfoEvent _info)
		{
			LocalizationManager instance = LocalizationManager.Instance;
			string empty = string.Empty;
			EInfoEventType infoType = _info.InfoType;
			if (infoType == EInfoEventType.TextAnnotation)
			{
				return instance.Get(_info.HeaderTextId, _info.StringMapper);
			}
			return instance.Get(3792);
		}

		public static string GetMessage(InfoEvent _info)
		{
			LocalizationManager instance = LocalizationManager.Instance;
			string result = string.Empty;
			switch (_info.InfoType)
			{
			case EInfoEventType.RestrictedAreaEnter:
				result = instance.Get(719);
				break;
			case EInfoEventType.RestrictedAreaLeave:
				result = instance.Get(1504);
				break;
			case EInfoEventType.AnonymousLevel:
				result = instance.Get(716);
				break;
			case EInfoEventType.AlertedLevel:
				result = instance.Get(717);
				break;
			case EInfoEventType.ExposedLevel:
				result = instance.Get(718);
				break;
			case EInfoEventType.DoctorUsed:
				result = instance.Get(1505);
				break;
			case EInfoEventType.CharacterAlarmed:
				result = instance.Get(2223);
				break;
			case EInfoEventType.CharacterRejects:
				result = instance.Get(2224);
				break;
			case EInfoEventType.WrongChest:
				result = instance.Get(4491);
				break;
			case EInfoEventType.TailingTargetGetsAway:
				result = instance.Get(4597);
				break;
			case EInfoEventType.TailingKeepLineOfSight:
				result = instance.Get(4598);
				break;
			case EInfoEventType.TextAnnotation:
				if (_info.TextId != -1)
				{
					result = instance.Get(_info.TextId, _info.StringMapper);
				}
				break;
			default:
				result = string.Format(instance.Get(4494), _info.InfoType);
				break;
			}
			return result;
		}
	}
}
