using System.Collections;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons.Headers
{
	public class UIButtonHeaderButton : UIButtonHeader
	{
		private const string c_unLockAnimation = "UnlockButton";

		private const string c_normalSprite = "aUI_btn_index_def";

		private const string c_lockedSprite = "aUI_btn_index_act";

		private const string c_selectedSprite = "aUI_btn_index_act";

		[SerializeField]
		[CheckAssigned]
		private TweenAlpha m_highLightAlphaTween;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_background;

		[CheckAssigned]
		[SerializeField]
		private Collider m_backgroundCollider;

		[CheckAssigned]
		[SerializeField]
		private TweenPosition m_tweenPosition;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_icon;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_label;

		[SerializeField]
		[CheckAssigned]
		private BoxCollider m_boxCollider;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_mainAnimation;

		private Vector3 m_clickedPosition;

		private Vector3 m_pressedPosition;

		public override bool IsEnabled
		{
			get
			{
				return m_boxCollider.enabled;
			}
			set
			{
				m_boxCollider.enabled = value;
				m_backgroundCollider.enabled = !value;
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
			m_clickedPosition = new Vector3(10f, 27.021f, 0f);
			m_pressedPosition = new Vector3(-10f, -27.021f, 0f);
			m_tweenPosition.duration = 0.12f;
			m_tweenPosition.animationCurve = GameObject.Find("UI Root").GetComponent<TweenPosition>().animationCurve;
			m_tweenPosition.to = m_pressedPosition;
			DeselectButton();
		}

		public override void SelectButton()
		{
			if (IsUnlocked)
			{
				m_isSelected = true;
				m_highLightAlphaTween.PlayForward();
				m_background.spriteName = "aUI_btn_index_act";
				m_icon.color = new Color32(6, 88, 102, byte.MaxValue);
				m_label.color = new Color32(6, 88, 102, byte.MaxValue);
				m_tweenPosition.to = m_clickedPosition;
				m_tweenPosition.ResetToBeginning();
				m_tweenPosition.PlayForward();
				m_mainAnimation.Stop();
			}
		}

		public override void DeselectButton()
		{
			m_isSelected = false;
			m_mainAnimation.Stop();
			if (IsUnlocked)
			{
				m_highLightAlphaTween.PlayReverse();
				m_background.spriteName = "aUI_btn_index_def";
				m_tweenPosition.PlayReverse();
				m_icon.color = Color.white;
				m_label.color = Color.white;
			}
		}

		public override void ChangeLockedState(bool _isUnlocked, bool _changeInstantly, bool _forceUpdate = false)
		{
			IsAnimating = false;
			bool flag = true;
			if (m_isUnlocked != _isUnlocked || _forceUpdate)
			{
				if (_isUnlocked)
				{
					m_background.spriteName = "aUI_btn_index_def";
					if (_changeInstantly || IsUnlocked)
					{
						OnInstantChangeAnimation(1, "UnlockButton");
					}
					else if (!IsUnlocked)
					{
						flag = false;
						StartCoroutine(OnStartChangeAnimation());
					}
				}
				else
				{
					m_background.spriteName = "aUI_btn_index_act";
					if (_changeInstantly || !IsUnlocked)
					{
						OnInstantChangeAnimation(0, "UnlockButton");
					}
					else if (IsUnlocked)
					{
						OnInstantChangeAnimation(0, "UnlockButton");
					}
				}
				m_isUnlocked = _isUnlocked;
				IsEnabled = _isUnlocked;
			}
			if (flag)
			{
				EventDelegate.Execute(base.OnUnlockChangeCompleted);
			}
		}

		protected override bool DoPress(bool _pressed)
		{
			m_tweenPosition.to = m_pressedPosition;
			if (_pressed)
			{
				m_tweenPosition.PlayForward();
			}
			else if (m_isSelected)
			{
				SelectButton();
			}
			else
			{
				m_tweenPosition.PlayReverse();
			}
			return true;
		}

		private void OnInstantChangeAnimation(int _factor, string _animationName)
		{
			m_mainAnimation.SampleAnimation(_factor, _animationName);
			IsAnimating = false;
		}

		private IEnumerator OnStartChangeAnimation()
		{
			IsAnimating = true;
			m_mainAnimation.Play("UnlockButton");
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_animusbar_unlock);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_mainAnimation.isPlaying)
			{
				yield return wait;
			}
			IsAnimating = false;
			EventDelegate.Execute(base.OnUnlockChangeCompleted);
		}
	}
}
