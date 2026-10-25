using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Models.Accordion;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.AccordionTab
{
	public abstract class UIAccordionBaseTab : MonoBehaviour
	{
		protected static readonly Color s_defaultBackgroundColor = new Color32(99, 186, byte.MaxValue, byte.MaxValue);

		private static readonly Color s_selectedTextColor = new Color32(6, 88, 102, byte.MaxValue);

		private static readonly Color s_deselectedTextColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		[SerializeField]
		protected UIExtendedLabel m_numberOfEntries;

		[SerializeField]
		protected UISprite m_newIndicator;

		[SerializeField]
		protected UISprite m_background;

		[SerializeField]
		protected UIExtendedLabel m_headerLabel;

		[SerializeField]
		protected BBTweenWidth m_highlightWidth;

		[SerializeField]
		protected BBTweenAlpha m_highlightAlpha;

		private bool m_activated;

		private bool m_hasUnread;

		public string Id { get; set; }

		public UIAccordionCategoryBaseModel ModelData { get; private set; }

		public bool IsActivated
		{
			get
			{
				return m_activated;
			}
			set
			{
				if (m_activated != value)
				{
					m_activated = value;
					if (m_activated)
					{
						OnActivateTab(false);
					}
					else
					{
						OnDeactivateTab(false);
					}
				}
			}
		}

		public bool HasUnread
		{
			get
			{
				return m_hasUnread;
			}
			set
			{
				m_hasUnread = value;
				m_newIndicator.gameObject.SetActive(m_hasUnread);
			}
		}

		public UIExtendedLabel NumberOfEntriesLabel
		{
			get
			{
				return m_numberOfEntries;
			}
		}

		public string TabName
		{
			get
			{
				return m_headerLabel.text;
			}
		}

		public Color SelectedTextColor
		{
			get
			{
				return (ModelData == null) ? s_selectedTextColor : ModelData.SelectedTextColor;
			}
		}

		public Color DeselectedTextColor
		{
			get
			{
				return (ModelData == null) ? s_deselectedTextColor : ModelData.DeselectedTextColor;
			}
		}

		public virtual void SetDisplayData(UIAccordionCategoryBaseModel _data)
		{
			ModelData = _data;
			SetDisplayData(_data.Id, _data.NameId, _data.IsUnread);
			if (m_activated)
			{
				OnActivateTab(true);
			}
			else
			{
				OnDeactivateTab(true);
			}
		}

		public virtual void SetDisplayData(string _id, StringId _headerLocaKey, bool _hasUnread)
		{
			Id = _id;
			m_headerLabel.LocalizationKey = _headerLocaKey;
			if (m_numberOfEntries != null)
			{
				m_numberOfEntries.text = string.Empty;
			}
			if (m_newIndicator != null)
			{
				m_newIndicator.gameObject.SetActive(_hasUnread);
			}
			m_background.color = s_defaultBackgroundColor;
		}

		public void UpdateTabAnchors(UIRect _anchor)
		{
			UIUtil.UpdateLeftAndRightAnchorsForTarget(GetComponent<UIRect>(), _anchor);
			UIRect component = m_highlightWidth.GetComponent<UIRect>();
			component.ResetAnchors();
			component.UpdateAnchors();
			component.Update();
			m_highlightWidth.ValidateTween();
		}

		public virtual void Init(bool _isActivated)
		{
			m_activated = _isActivated;
			if (m_activated)
			{
				OnActivateTab(true);
			}
			else
			{
				OnDeactivateTab(true);
			}
		}

		private void Awake()
		{
			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void OnActivateTab(bool _finishInstantly)
		{
			if (_finishInstantly)
			{
				m_highlightAlpha.SetTweenToEnd();
				m_highlightWidth.SetTweenToEnd();
			}
			else
			{
				m_highlightAlpha.StartTweenPlayForward();
				m_highlightWidth.StartTweenPlayForward();
			}
			m_headerLabel.color = SelectedTextColor;
			if (m_numberOfEntries != null)
			{
				m_numberOfEntries.color = SelectedTextColor;
			}
		}

		protected virtual void OnDeactivateTab(bool _finishInstantly)
		{
			if (_finishInstantly)
			{
				m_highlightAlpha.SetTweenToBeginning();
				m_highlightWidth.SetTweenToBeginning();
			}
			else
			{
				m_highlightAlpha.StartTweenPlayReverse();
				m_highlightWidth.StartTweenPlayReverse();
			}
			m_headerLabel.color = DeselectedTextColor;
			if (m_numberOfEntries != null)
			{
				m_numberOfEntries.color = DeselectedTextColor;
			}
		}
	}
}
