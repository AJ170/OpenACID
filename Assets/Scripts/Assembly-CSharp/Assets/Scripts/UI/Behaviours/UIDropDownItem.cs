using System;
using System.Collections;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIDropDownItem : MonoBehaviour
	{
		private const string k_selectClipName = "Select";

		private const string k_deSelectClipName = "DeSelect";

		private static readonly Color s_disabledColor = Color.white;

		private static readonly Color s_enabledColor = new Color(2f / 85f, 0.34509805f, 0.4f);

		private static readonly Color s_disabledIconColor = new Color(4f / 51f, 4f / 51f, 4f / 51f);

		private static readonly Color s_enabledIconColor = Color.white;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_iconSprite;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_label;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedButton m_button;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		[CheckAssigned]
		private BBTweenColor m_textTweenColor;

		[CheckAssigned]
		[SerializeField]
		private BBTweenColor m_iconTweenColor;

		[SerializeField]
		private UISprite m_disabledShadow;

		private string m_currentClip = string.Empty;

		public UIDropDownItemConfiguration Data { get; private set; }

		public bool Selected { get; private set; }

		public bool IsEnabled
		{
			get
			{
				return m_button.isEnabled;
			}
			set
			{
				m_button.isEnabled = value;
			}
		}

		public string Label
		{
			set
			{
				m_label.text = value;
			}
		}

		public string Icon
		{
			set
			{
				m_iconSprite.spriteName = value;
				m_iconSprite.MakePixelPerfect();
			}
		}

		public event Action<UIDropDownItem> OnClicked;

		public void UpdateItem(UIDropDownItemConfiguration _data, UIRect _anchorTarget)
		{
			bool flag = _data.IsEnabled || _data.IconNameDisabled == null;
			Data = _data;
			Icon = ((!flag) ? _data.IconNameDisabled : _data.IconName);
			Label = _data.Label;
			IsEnabled = _data.IsEnabled;
			if (m_disabledShadow != null)
			{
				m_disabledShadow.gameObject.SetActive(true);
			}
			if (_data.IconNameDisabled != null)
			{
				m_iconTweenColor.from = s_enabledIconColor;
			}
			m_iconTweenColor.from = ((Data.IconNameDisabled == null) ? s_disabledColor : s_enabledIconColor);
			m_iconTweenColor.to = ((Data.IconNameDisabled == null) ? s_enabledColor : s_enabledIconColor);
			m_iconTweenColor.ResetToBeginning();
			Select(_data.Selected, true);
			UIWidget component = GetComponent<UIWidget>();
			component.leftAnchor.Set(_anchorTarget.transform, 0f, 0f);
			component.rightAnchor.Set(_anchorTarget.transform, 1f, 0f);
		}

		public void Select(bool _select, bool _instant, Action<UIDropDownItem, bool> _callback = null)
		{
			if (_select != Selected || _instant)
			{
				Selected = _select;
				ChangeVisibleState((!_select) ? "DeSelect" : "Select", _instant, _callback);
			}
		}

		private void Awake()
		{
			Selected = false;
			m_textTweenColor.from = s_disabledColor;
			m_textTweenColor.to = s_enabledColor;
			m_iconTweenColor.from = s_disabledIconColor;
			m_iconTweenColor.to = s_enabledIconColor;
			m_textTweenColor.ResetToBeginning();
			m_iconTweenColor.ResetToBeginning();
			m_animation.SampleAnimation(0f, string.Empty);
		}

		private void OnEnable()
		{
			m_button.isEnabled = Data != null && Data.IsEnabled;
			EventDelegate.Add(m_button.onClick, OnButtonClicked);
		}

		private void OnDisable()
		{
			m_button.isEnabled = false;
			EventDelegate.Remove(m_button.onClick, OnButtonClicked);
			StopAllCoroutines();
			m_currentClip = string.Empty;
		}

		private void OnButtonClicked()
		{
			if (this.OnClicked != null)
			{
				this.OnClicked(this);
			}
		}

		private void ChangeVisibleState(string _clip, bool _instant, Action<UIDropDownItem, bool> _callback)
		{
			if (_instant)
			{
				m_animation.SampleAnimation(1f, _clip);
			}
			else if (!string.IsNullOrEmpty(m_currentClip))
			{
				m_animation.SampleAnimation(0f, m_currentClip);
			}
			else
			{
				StartCoroutine(co_changeState(_clip));
			}
			if (_callback != null)
			{
				_callback(this, Selected);
			}
		}

		private IEnumerator co_changeState(string _clip)
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			m_currentClip = _clip;
			m_animation.Play(_clip);
			while (m_animation.IsPlaying(_clip))
			{
				yield return wait;
			}
			m_currentClip = string.Empty;
		}

		[ContextMenu("Select")]
		private void DoSelect()
		{
			Select(true, true);
		}

		[ContextMenu("De Select")]
		private void DoDeSelect()
		{
			Select(false, true);
		}
	}
}
