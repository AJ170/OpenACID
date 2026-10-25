using UnityEngine;

namespace Assets.Scripts.UI.Screens.Generic
{
	public class ListItemHandler : MonoBehaviour
	{
		public delegate void SenderDelegate(ListItemHandler _source, IconHandler _sender);

		[SerializeField]
		protected IconHandler m_iconHandler;

		[SerializeField]
		protected ItemInfoHandler m_infoHandler;

		[SerializeField]
		protected UISprite m_levelBackground;

		private bool m_isSelected;

		public object Item { get; protected set; }

		public IconHandler Icon
		{
			get
			{
				return m_iconHandler;
			}
		}

		public bool Enabled
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public event SenderDelegate Selected;

		public event SenderDelegate Deselected;

		private void InvokeSelected(IconHandler _sender)
		{
			SenderDelegate selected = this.Selected;
			if (selected != null)
			{
				selected(this, _sender);
			}
		}

		private void InvokeDeselected(IconHandler _sender)
		{
			SenderDelegate deselected = this.Deselected;
			if (deselected != null)
			{
				deselected(this, _sender);
			}
		}

		public bool GetIsSelected()
		{
			return m_isSelected;
		}

		public void SetIsSelected(bool _value, IconHandler _sender, IconHandler _source, bool _instant)
		{
			if (_value == m_isSelected)
			{
				return;
			}
			m_isSelected = _value;
			if (_value)
			{
				if (_instant && _sender != null)
				{
					_sender.IsImmediateHighlightOn = true;
				}
				else
				{
					InvokeSelected(_sender);
				}
			}
			else if (_instant && _sender != null)
			{
				_sender.IsImmediateHighlightOn = false;
			}
			else
			{
				InvokeDeselected(_sender);
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			EnableEventListeners();
		}

		private void OnDisable()
		{
			DisableEventListeners();
		}

		private void EnableEventListeners()
		{
			m_infoHandler.Click += OnContainerClick;
			m_iconHandler.Selected += OnSelected;
			m_iconHandler.Deselected += OnDeselected;
			m_iconHandler.Enabled = true;
		}

		private void OnContainerClick(ItemInfoHandler _sender)
		{
			m_iconHandler.Toggle(m_iconHandler);
		}

		private void DisableEventListeners()
		{
			m_infoHandler.Click -= OnContainerClick;
			m_iconHandler.Selected -= OnSelected;
			m_iconHandler.Deselected -= OnDeselected;
		}

		protected virtual void OnSelected(IconHandler _sender, IconHandler _source)
		{
			SetIsSelected(true, _sender, _source, false);
		}

		private void OnDeselected(IconHandler _sender, IconHandler _source)
		{
			SetIsSelected(false, _sender, _source, false);
		}

		public void Reset()
		{
			m_isSelected = false;
			m_iconHandler.Reset();
		}
	}
}
