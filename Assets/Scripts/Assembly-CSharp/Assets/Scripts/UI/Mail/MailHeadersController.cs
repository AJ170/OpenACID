using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Mail
{
	public class MailHeadersController : MonoBehaviour, IInitialized
	{
		public delegate void HandleMailSelected(MailHeader _header);

		[SerializeField]
		private GameObject m_helpText;

		[SerializeField]
		private UIExtendedWrapContent m_wrapper;

		[SerializeField]
		private Transform m_anchorTarget;

		[SerializeField]
		private GameObject m_loadingIndicator;

		[SerializeField]
		private UIExtendedLabel m_mailInventory;

		[SerializeField]
		private MailController m_mailController;

		private List<MailHeader> m_headers;

		private MailHeaderController m_current;

		private MailHeader m_currentHeader;

		private int m_selectedIndex;

		private UIRequestToken m_currentRequest;

		public MailHeaderController Current
		{
			get
			{
				return m_current;
			}
		}

		public event HandleMailSelected OnMailSelected;

		public void DeselectCurrentHeader()
		{
			if (m_current != null)
			{
				m_current.SetSelected(false);
			}
			m_current = null;
		}

		public void Initialize()
		{
			m_wrapper.OnItemCreated += OnItemCreated;
			m_wrapper.OnUpdateItem += OnUpdateItem;
			m_wrapper.Init();
		}

		public void Clear()
		{
			m_wrapper.OnItemCreated -= OnItemCreated;
			m_wrapper.OnUpdateItem -= OnUpdateItem;
		}

		public void ScreenDeactivate()
		{
			DeselectCurrentHeader();
			m_current = null;
			if (m_currentRequest != null)
			{
				m_currentRequest.Abandoned = true;
			}
			m_currentRequest = null;
		}

		public void UpdateData()
		{
			bool active = m_headers == null || m_headers.Count == 0;
			m_loadingIndicator.SetActive(active);
			m_currentRequest = Singleton<StateManager>.Instance.MailManager.GetMailHeaders(OnHeadersLoaded);
		}

		private void OnUpdateItem(Transform _transform, int _index)
		{
			MailHeaderController component = _transform.GetComponent<MailHeaderController>();
			bool selected = m_currentHeader == m_headers[_index];
			component.UpdateData(selected, m_headers[_index]);
			if (m_currentHeader == m_headers[_index])
			{
				m_current = component;
			}
		}

		private void OnItemCreated(Transform _transform)
		{
			MailHeaderController component = _transform.GetComponent<MailHeaderController>();
			EventDelegate.Add(component.OnMailClick, OnMailTapped);
			UIWidget component2 = _transform.GetComponent<UIWidget>();
			component2.leftAnchor.target = m_anchorTarget;
			component2.rightAnchor.target = m_anchorTarget;
			component.SetSelectedInstant(false);
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			m_loadingIndicator.SetActive(false);
			m_helpText.SetActive(false);
			m_selectedIndex = 0;
			Singleton<StateManager>.Instance.MailManager.OnHeaderRemoved += OnHeadersLoaded;
		}

		private void OnDisable()
		{
			Singleton<StateManager>.Instance.MailManager.OnHeaderRemoved -= OnHeadersLoaded;
			m_current = null;
			m_currentHeader = null;
		}

		private void OnHeadersLoaded(List<MailHeader> _headers)
		{
			m_loadingIndicator.SetActive(false);
			m_headers = _headers;
			UpdateList();
		}

		private void OnMailTapped()
		{
			MailHeaderController current = MailHeaderController.Current;
			if (!(m_current != null) || m_current.MailHeader == null || !(current != null) || current.MailHeader == null || !(m_current.MailHeader.headerId == current.MailHeader.headerId))
			{
				if (m_current != null)
				{
					m_current.SetSelected(false);
				}
				m_current = current;
				m_currentHeader = m_current.MailHeader;
				m_current.SetSelected(true);
				Trigger(this.OnMailSelected, m_currentHeader);
			}
		}

		private void UpdateList()
		{
			bool flag = m_headers == null || m_headers.Count == 0;
			if (flag)
			{
				m_wrapper.ItemCount = 0;
				m_selectedIndex = 0;
				if (m_mailController.Visible)
				{
					Trigger(this.OnMailSelected, null);
				}
			}
			else
			{
				int val = ((m_currentHeader != null) ? m_headers.FindIndex((MailHeader _x) => _x.headerId == m_currentHeader.headerId) : 0);
				m_selectedIndex = Math.Max(0, val);
				if (m_mailController.Visible)
				{
					m_currentHeader = m_headers[m_selectedIndex];
					Trigger(this.OnMailSelected, m_currentHeader);
				}
				m_wrapper.ItemCount = m_headers.Count;
			}
			m_wrapper.WrapScrollView.ResetPosition();
			m_helpText.SetActive(flag);
			if (!flag)
			{
				m_wrapper.SetFocusOnChildByIndex(m_selectedIndex);
			}
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("value", (m_headers != null) ? m_headers.Count : 0);
			stringMapper.Add("max_value", MailExtensions.MailLimit(EMailPriority.MailPriorityLow));
			StringMapper mapper = stringMapper;
			m_mailInventory.text = LocalizationManager.Instance.Get(5630, mapper);
		}

		public int GetMailIndexBySubjectLangKey(int _key)
		{
			int result = -1;
			for (int i = 0; i < m_headers.Count; i++)
			{
				if (m_headers[i].subjectLangKey.neutral == _key)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public void FocusMailHeaderByIndex(int _index)
		{
			m_wrapper.SetFocusOnChildByIndex(_index);
		}

		private void Trigger(HandleMailSelected _callback, MailHeader _header)
		{
			if (_callback != null)
			{
				_callback(_header);
			}
		}
	}
}
