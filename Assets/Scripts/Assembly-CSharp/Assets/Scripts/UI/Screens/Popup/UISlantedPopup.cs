using System;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UbiCore;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup
{
	public class UISlantedPopup : MonoBehaviour
	{
		public enum ESide
		{
			Left = 0,
			Right = 1
		}

		public enum EState
		{
			OnScreen = 0,
			OffScreen = 1
		}

		public delegate void SlantedPopupEventDelegate(UISlantedPopup _popup);

		public delegate bool SlantedPopupEventDelegateWithResult(UISlantedPopup _popup);

		private static int s_maxNumberOfLines;

		private static readonly int[] s_rightAnchorOffsets;

		[SerializeField]
		private UIExtendedLabel m_Header;

		[SerializeField]
		private UIExtendedLabel m_Text;

		[SerializeField]
		private ESide m_Side;

		[SerializeField]
		private UIWidget m_Bottom;

		[SerializeField]
		private UIButtonProxy m_Button;

		[SerializeField]
		private UIWidget m_Container;

		[SerializeField]
		private int m_WidthIncrement;

		[SerializeField]
		private int m_MaxWidthIncrementSteps;

		[SerializeField]
		private GameObject m_background;

		[SerializeField]
		private UIPanel m_backgroundContainer;

		[SerializeField]
		private BBTweenPosition m_Tween;

		private BBTweenAlpha m_buttonTween;

		private Vector3 m_offScreenPosition;

		private Vector3 m_onScreenPosition;

		private int m_textLineCount;

		private DynamicSwipe m_swipeFx;

		private bool m_showButtonAfterMoveIn;

		private UISprite m_backgroundSprite;

		private bool m_isOverlayScreenHiden = true;

		public EState State { get; private set; }

		public event SlantedPopupEventDelegate OnMoveInFinished;

		public event SlantedPopupEventDelegate OnMoveOutFinished;

		public event SlantedPopupEventDelegateWithResult OnPopupDismissed;

		static UISlantedPopup()
		{
			s_maxNumberOfLines = 7;
			s_rightAnchorOffsets = new int[s_maxNumberOfLines];
			s_rightAnchorOffsets[0] = 74;
			s_rightAnchorOffsets[1] = 88;
			s_rightAnchorOffsets[2] = 104;
			s_rightAnchorOffsets[3] = 124;
			s_rightAnchorOffsets[4] = 138;
			s_rightAnchorOffsets[5] = 157;
			s_rightAnchorOffsets[6] = 175;
		}

		private void Awake()
		{
			State = EState.OffScreen;
			m_buttonTween = m_Button.GetComponent<BBTweenAlpha>();
			m_swipeFx = m_background.GetComponent<DynamicSwipe>();
			m_backgroundSprite = m_background.GetComponent<UISprite>();
			EventDelegate.Add(m_Tween.onFinished, OnTweenFinished);
			m_onScreenPosition = (m_offScreenPosition = base.transform.localPosition);
			EventDelegate.Add(m_Button.onClick, OnClick);
			m_Bottom.gameObject.SetActive(Main.Instance.SceneLoader.InAnimus);
			m_backgroundSprite.atlas = Singleton<UIScreenManager>.Instance.GetAtlasByType(Main.Instance.SceneLoader.InAnimus ? UIScreenManager.EAtlasType.Animus : UIScreenManager.EAtlasType.Shared);
		}

		public void SetHeaderTextLocalized(StringId _locaId)
		{
			if (m_Header.LocalizationKey != _locaId)
			{
				m_Header.LocalizationKey = _locaId;
				OnUpdateText(m_Header, LocalizationManager.Instance.Get(_locaId));
				UpdateSize();
			}
		}

		public void SetHeaderText(string _text)
		{
			if (m_Header.text != _text)
			{
				m_Header.text = _text;
				OnUpdateText(m_Header, _text);
				UpdateSize();
			}
		}

		public void SetDescriptionTextLocalized(StringId _locaId)
		{
			if (m_Text.LocalizationKey != _locaId)
			{
				m_Text.LocalizationKey = _locaId;
				m_textLineCount = OnUpdateText(m_Text, LocalizationManager.Instance.Get(_locaId));
				UpdateSize();
			}
		}

		public void SetDescriptionText(string _text)
		{
			if (m_Text.text != _text)
			{
				m_Text.text = _text;
				m_textLineCount = OnUpdateText(m_Text, _text);
				UpdateSize();
			}
		}

		public void SetButtonText(string _text)
		{
			if (_text != null)
			{
				m_Button.Button.SetLabel(_text);
			}
			m_showButtonAfterMoveIn = _text != null;
		}

		public void MoveIn()
		{
			m_isOverlayScreenHiden = UILoadingScreen.Instance.IsHidden && UIDownloadScreen.Instance.IsHidden;
			if (m_isOverlayScreenHiden)
			{
				State = EState.OnScreen;
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small);
				m_Tween.SetTweenToBeginning();
				m_Tween.PlayForward();
			}
		}

		public void MoveInInstant()
		{
			State = EState.OnScreen;
			m_isOverlayScreenHiden = true;
			m_Tween.SetTweenToEnd();
			if (m_showButtonAfterMoveIn)
			{
				m_Button.gameObject.SetActive(true);
				m_showButtonAfterMoveIn = false;
				m_buttonTween.StartTweenPlayForward();
			}
		}

		public void MoveOut()
		{
			State = EState.OffScreen;
			m_isOverlayScreenHiden = true;
			m_Tween.SetTweenToEnd();
			m_Tween.PlayReverse();
		}

		public void MoveOutInstant()
		{
			State = EState.OffScreen;
			m_isOverlayScreenHiden = true;
			m_Tween.SetTweenToBeginning();
			m_Button.gameObject.SetActive(false);
		}

		public void SetCallback(SlantedPopupEventDelegateWithResult _callback)
		{
			this.OnPopupDismissed = _callback;
		}

		private int OnUpdateText(UIExtendedLabel _label, string _newValue)
		{
			string final = string.Empty;
			bool flag = true;
			int num = 0;
			int num2 = 0;
			do
			{
				_label.ProcessText();
				_label.Wrap(_newValue, out final);
				flag = NGUIText.textTooLarge && num < m_MaxWidthIncrementSteps;
				num++;
				if (flag)
				{
					_label.width += m_WidthIncrement;
					num2 += m_WidthIncrement;
				}
			}
			while (flag);
			_label.width -= num2;
			m_Container.width += num2;
			_label.ForceUpdate(true);
			if (m_backgroundContainer.bottomAnchor.target == _label.transform)
			{
				m_backgroundContainer.bottomAnchor.absolute = -34;
				int num3 = _label.fontSize * NGUIText.wrapTextLineCount;
				if (_label.height > num3)
				{
					m_backgroundContainer.bottomAnchor.absolute = 0;
				}
				m_backgroundContainer.ForceUpdate(true);
			}
			return NGUIText.wrapTextLineCount;
		}

		private void Update()
		{
			if (!m_isOverlayScreenHiden)
			{
				MoveIn();
			}
		}

		private void UpdateSize()
		{
			int num = (m_textLineCount - 1).Clamp(0, s_maxNumberOfLines - 1);
			if (m_Side == ESide.Left)
			{
				m_Bottom.rightAnchor.absolute = -s_rightAnchorOffsets[num];
			}
			else
			{
				m_Bottom.leftAnchor.absolute = s_rightAnchorOffsets[num];
			}
			float offscreenPositionX = UIUtil.GetOffscreenPositionX(m_Container, m_Container.width, (m_Side != 0) ? UIUtil.ESide.Right : UIUtil.ESide.Left);
			m_offScreenPosition.x = offscreenPositionX;
			m_onScreenPosition.x = m_offScreenPosition.x + (float)(m_Container.width * ((m_Side == ESide.Left) ? 1 : (-1)));
			if (m_Side == ESide.Left)
			{
				m_offScreenPosition.x = (float)Math.Floor(m_offScreenPosition.x);
				m_onScreenPosition.x = (float)Math.Floor(m_onScreenPosition.x - 5f);
			}
			else
			{
				m_offScreenPosition.x = (float)Math.Ceiling(m_offScreenPosition.x);
				m_onScreenPosition.x = (float)Math.Ceiling(m_onScreenPosition.x + 5f);
			}
			m_Tween.from = m_offScreenPosition;
			m_Tween.to = m_onScreenPosition;
			m_Tween.SetTweenToBeginning();
		}

		private void OnTweenFinished()
		{
			if (State == EState.OnScreen)
			{
				if (this.OnMoveInFinished != null)
				{
					this.OnMoveInFinished(this);
				}
				PlayFx();
				if (m_showButtonAfterMoveIn)
				{
					m_Button.gameObject.SetActive(true);
					m_buttonTween.StartTweenPlayForward();
				}
			}
			else if (State == EState.OffScreen)
			{
				if (this.OnMoveOutFinished != null)
				{
					this.OnMoveOutFinished(this);
				}
				m_Button.gameObject.SetActive(false);
			}
			m_showButtonAfterMoveIn = false;
		}

		private void PlayFx()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_flash);
			m_swipeFx.Initialize(m_Side == ESide.Right, (int)m_backgroundContainer.baseClipRegion.z, (int)m_backgroundContainer.baseClipRegion.w);
			m_swipeFx.Animate();
		}

		private void OnClick()
		{
			if (this.OnPopupDismissed != null && this.OnPopupDismissed(this))
			{
				this.OnPopupDismissed = null;
			}
		}
	}
}
