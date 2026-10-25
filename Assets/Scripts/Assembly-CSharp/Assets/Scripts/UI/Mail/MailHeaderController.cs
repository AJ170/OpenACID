using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class MailHeaderController : MonoBehaviour
	{
		private const float k_readAlpha = 0.8f;

		private const float k_unReadAlpha = 1f;

		private static readonly Color s_selectedColor = new Color(2f / 85f, 0.34509805f, 0.4f);

		private static readonly Color s_unselectedColor = Color.white;

		public List<EventDelegate> OnMailClick = new List<EventDelegate>();

		[SerializeField]
		private UIExtendedLabel m_senderLabel;

		[SerializeField]
		private UIExtendedLabel m_subjectLabel;

		[SerializeField]
		private UIExtendedLabel m_dateLabel;

		[SerializeField]
		private UISprite m_newIcon;

		[SerializeField]
		private BBTweenAlpha m_highlight;

		[SerializeField]
		private BBTweenScale m_scale;

		[SerializeField]
		private BBTweenAlpha m_activeBackground;

		[SerializeField]
		private BBTweenAlpha m_passiveBackground;

		[SerializeField]
		private UISprite m_passiveBackgroundWidget;

		[SerializeField]
		private BBTweenColor m_senderColor;

		[SerializeField]
		private BBTweenColor m_subjectColor;

		[SerializeField]
		private BBTweenColor m_dateColor;

		[SerializeField]
		private UIWidget m_widget;

		[SerializeField]
		private Font m_unreadFont;

		[SerializeField]
		private Font m_readFont;

		private BBTweenColor[] m_colorTweens;

		private Font[] m_fonts;

		private UIExtendedLabel[] m_labels;

		public static MailHeaderController Current { get; private set; }

		public MailHeader MailHeader { get; private set; }

		public bool IsSelected { get; private set; }

		private void Awake()
		{
			m_colorTweens = new BBTweenColor[3] { m_senderColor, m_subjectColor, m_dateColor };
			m_fonts = new Font[2] { m_readFont, m_unreadFont };
			m_labels = new UIExtendedLabel[3] { m_senderLabel, m_dateLabel, m_subjectLabel };
			m_scale.SetTweenToBeginning();
		}

		private void OnEnable()
		{
			MailCache.OnMailRead += OnMailRead;
		}

		private void OnDisable()
		{
			MailCache.OnMailRead -= OnMailRead;
		}

		private void OnMailRead(MailHeader _header, bool _read)
		{
			if (_header == MailHeader)
			{
				m_newIcon.gameObject.SetActive(!MailHeader.isRead);
			}
		}

		private void OnClick()
		{
			OnMailSelected();
		}

		private void OnMailSelected()
		{
			Current = this;
			EventDelegate.Execute(OnMailClick);
			Current = null;
		}

		public void UpdateData(bool _selected, MailHeader _header)
		{
			if (_header == null)
			{
				m_widget.alpha = 0f;
				return;
			}
			bool isRead = _header.isRead;
			m_widget.alpha = 1f;
			MailHeader = _header;
			m_senderLabel.text = _header.GetSenderName();
			m_newIcon.gameObject.SetActive(!isRead);
			m_subjectLabel.text = _header.GetSubject();
			m_passiveBackgroundWidget.alpha = ((!isRead) ? 1f : 0.8f);
			m_dateLabel.text = new UIDate(ServerTimeManager.ToLocalTime(_header.creationDate), UIDate.EDisplayType.Date).ToString();
			SetSelectedInstant(_selected);
		}

		public void SetSelected(bool _selected)
		{
			IsSelected = _selected;
			UpdateColors(_selected, MailHeader);
			m_scale.Play(_selected);
			m_highlight.Play(_selected);
			if (_selected)
			{
				m_activeBackground.StartTweenPlayForward();
				m_passiveBackground.StartTweenPlayReverse();
			}
			else
			{
				m_activeBackground.StartTweenPlayReverse();
				m_passiveBackground.StartTweenPlayForward();
			}
			for (int i = 0; i < m_colorTweens.Length; i++)
			{
				m_colorTweens[i].StartTweenPlayForward();
			}
			UpdateFont(_selected, MailHeader);
		}

		public void SetSelectedInstant(bool _selected)
		{
			IsSelected = _selected;
			Color color = GetColor(_selected, MailHeader);
			float factor = ((!_selected) ? 0f : 1f);
			m_scale.Sample(factor, false);
			m_highlight.Sample(factor, false);
			if (_selected)
			{
				m_activeBackground.SetTweenToEnd();
				m_passiveBackground.SetTweenToBeginning();
			}
			else
			{
				m_activeBackground.SetTweenToBeginning();
				m_passiveBackground.SetTweenToEnd();
			}
			for (int i = 0; i < m_colorTweens.Length; i++)
			{
				m_colorTweens[i].to = color;
				m_colorTweens[i].SetTweenToEnd();
			}
			UpdateFont(_selected, MailHeader);
		}

		private void UpdateColors(bool _selected, MailHeader _header)
		{
			Color color = GetColor(_selected, _header);
			for (int i = 0; i < m_colorTweens.Length; i++)
			{
				m_colorTweens[i].to = color;
			}
		}

		private static Color GetColor(bool _selected, MailHeader _header)
		{
			return (!_selected) ? s_unselectedColor : s_selectedColor;
		}

		private void UpdateFont(bool _selected, MailHeader _header)
		{
			int num = ((_header == null || !_header.isRead || _selected) ? 1 : 0);
			for (int i = 0; i < m_labels.Length; i++)
			{
				m_labels[i].trueTypeFont = m_fonts[num];
			}
		}
	}
}
