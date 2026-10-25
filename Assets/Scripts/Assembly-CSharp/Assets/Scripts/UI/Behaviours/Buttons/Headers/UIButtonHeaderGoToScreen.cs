using Assets.Scripts.Tracking;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons.Headers
{
	public class UIButtonHeaderGoToScreen : UIButtonHeader
	{
		private const string c_selectedSprite = "aUI_btn_SpecialEvent_ON";

		private const string c_normalSprite = "aUI_btn_SpecialEvent";

		[CheckAssigned]
		[SerializeField]
		private UIExtendedButton m_button;

		[CheckAssigned]
		[SerializeField]
		private UIButtonLabelHandler m_labelHandler;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_highlightAlphaTween;

		[SerializeField]
		[CheckAssigned]
		private BoxCollider m_disabledCollider;

		public override bool IsEnabled
		{
			get
			{
				return m_button.isEnabled;
			}
			set
			{
				m_button.isEnabled = value;
				m_disabledCollider.enabled = !value;
			}
		}

		public override bool IsAnimating { get; protected set; }

		public override bool IsUnlocked
		{
			get
			{
				return m_isUnlocked;
			}
			set
			{
				if (m_isUnlocked != value)
				{
					ChangeLockedState(value, false);
				}
			}
		}

		public override void InitButton()
		{
			DeselectButton();
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnEvaluateButton);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenPopped, OnEvaluateButton);
		}

		protected override void OnDestroyButton()
		{
			base.OnDestroyButton();
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenChanged, OnEvaluateButton);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPopped, OnEvaluateButton);
		}

		protected override bool DoPress(bool _pressed)
		{
			if (base.HeaderType == EHeaderType.Events)
			{
				TrackingManager.OpenEventProgressionScreen("EventScreenButton");
			}
			return true;
		}

		private void OnEvaluateButton()
		{
			IsEnabled = base.ScreenChanger != null && !base.ScreenChanger.IsCurrentScreentActive;
		}

		public override void SelectButton()
		{
			if (IsUnlocked)
			{
				m_isSelected = true;
				m_button.normalSprite = "aUI_btn_SpecialEvent_ON";
				m_labelHandler.Label.color = new Color32(6, 88, 102, byte.MaxValue);
				m_labelHandler.Sprite.color = new Color32(6, 88, 102, byte.MaxValue);
				m_highlightAlphaTween.PlayForward();
			}
		}

		public override void DeselectButton()
		{
			m_isSelected = false;
			if (IsUnlocked)
			{
				m_button.normalSprite = "aUI_btn_SpecialEvent";
				m_labelHandler.Label.color = Color.white;
				m_labelHandler.Sprite.color = Color.white;
				m_highlightAlphaTween.PlayReverse();
			}
		}

		public override void ChangeLockedState(bool _isUnlocked, bool _changeInstantly, bool _forceUpdate = false)
		{
			IsAnimating = false;
			m_isUnlocked = _isUnlocked;
			IsEnabled = _isUnlocked;
			EventDelegate.Execute(base.OnUnlockChangeCompleted);
		}
	}
}
