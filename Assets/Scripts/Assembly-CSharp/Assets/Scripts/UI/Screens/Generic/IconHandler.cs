using UnityEngine;

namespace Assets.Scripts.UI.Screens.Generic
{
	public abstract class IconHandler : MonoBehaviour
	{
		public delegate void SenderDelegate(IconHandler _sender, IconHandler _source);

		[SerializeField]
		private GameObject m_highlight;

		[SerializeField]
		private UITweener m_highlightAnim;

		[SerializeField]
		private UIToggle m_toggle;

		private bool m_activated;

		private bool m_isImmediateHighlightOn;

		public bool Empty
		{
			get
			{
				return Entry != null;
			}
		}

		public bool Enabled { get; set; }

		public object Entry { get; protected set; }

		public bool IsImmediateHighlightOn
		{
			get
			{
				return m_isImmediateHighlightOn;
			}
			set
			{
				m_highlightAnim.Sample((!value) ? 0f : 1f, false);
				m_isImmediateHighlightOn = value;
			}
		}

		public event SenderDelegate Deselected;

		public event SenderDelegate Selected;

		public void SetActivated(bool _value, IconHandler _source)
		{
			if (m_activated != _value)
			{
				m_activated = _value;
				m_toggle.value = m_activated;
				m_isImmediateHighlightOn = _value;
				if (m_activated)
				{
					InvokeSelected(_source);
				}
				else
				{
					InvokeDeselected(_source);
				}
			}
		}

		public void OnToggleChange()
		{
			if (Enabled)
			{
				bool flag = Entry != null && m_toggle.value;
				if (flag)
				{
					IsImmediateHighlightOn = false;
				}
				m_highlightAnim.Play(flag);
				SetActivated(m_toggle.value, null);
			}
		}

		public void Reset()
		{
			m_toggle.value = false;
		}

		public void Toggle(IconHandler _listItemHandler)
		{
			SetActivated(!m_activated, _listItemHandler);
		}

		protected virtual void OnClick()
		{
		}

		private void InvokeDeselected(IconHandler _source)
		{
			SenderDelegate deselected = this.Deselected;
			if (deselected != null)
			{
				deselected(this, _source);
			}
		}

		private void InvokeSelected(IconHandler _source)
		{
			SenderDelegate selected = this.Selected;
			if (selected != null)
			{
				selected(this, _source);
			}
		}

		protected virtual void Setup()
		{
			m_highlight.SetActive(true);
			m_highlightAnim.ResetToBeginning();
		}

		private void Awake()
		{
			Setup();
		}

		private void OnDisable()
		{
			if (m_toggle != null)
			{
				Reset();
			}
		}

		private void OnDestroy()
		{
			TearDown();
		}

		protected virtual void TearDown()
		{
		}
	}
}
