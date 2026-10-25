using Assets.Scripts.UI.MVC.Models.Accordion;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.AccordionTab
{
	public class UIAccordionLeaderboardEntryTab : UIAccordionEntryTab
	{
		protected static readonly Color s_lockedBackgroundColor = new Color32(168, 182, 192, byte.MaxValue);

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_headerAlignmentBox;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_lockIcon;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_headerIcon;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_headerIconContainer;

		[SerializeField]
		[CheckAssigned]
		private BoxCollider m_boxCollider;

		private bool m_isLocked;

		public bool IsLocked
		{
			get
			{
				return m_isLocked;
			}
			set
			{
				m_isLocked = value;
				m_lockIcon.gameObject.SetActive(m_isLocked);
				m_boxCollider.enabled = !m_isLocked;
				if (m_isLocked)
				{
					OnDeactivateTab(true);
					m_highlightWidth.SetTweenToEnd();
					m_background.color = s_lockedBackgroundColor;
				}
				else
				{
					m_background.color = UIAccordionBaseTab.s_defaultBackgroundColor;
				}
			}
		}

		public override void SetDisplayData(UIAccordionCategoryBaseModel _data)
		{
			base.SetDisplayData(_data);
			IsLocked = _data.IsLocked;
			bool flag = !string.IsNullOrEmpty(_data.NameIconName);
			m_headerIconContainer.SetActive(flag);
			if (flag)
			{
				m_headerIcon.spriteName = _data.NameIconName;
			}
			m_headerIcon.MakePixelPerfect();
			m_headerAlignmentBox.RepositionNow = true;
		}

		protected override void OnActivateTab(bool _finishInstantly)
		{
			base.OnActivateTab(_finishInstantly);
			m_headerIcon.color = base.SelectedTextColor;
		}

		protected override void OnDeactivateTab(bool _finishInstantly)
		{
			base.OnDeactivateTab(_finishInstantly);
			m_headerIcon.color = base.DeselectedTextColor;
		}

		protected override void OnAwake()
		{
			base.OnAwake();
		}
	}
}
