using System;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.Interaction;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Generic
{
	public abstract class ListHandler : MonoBehaviour
	{
		public delegate void SenderDelegate(ListItemHandler _sender);

		[SerializeField]
		protected UIExtendedWrapContent m_wrapper;

		[SerializeField]
		protected UIScrollBar m_scrollBar;

		[SerializeField]
		private int m_rightMargin = 70;

		protected int TotalSpace;

		public virtual ListItemHandler SelectedHandler { get; protected set; }

		public event SenderDelegate SelectionChange;

		public event Action<Transform, int> ItemUpdate;

		protected void SetSelectedHandler(ListItemHandler _sender, IconHandler _source)
		{
			if (!(_sender == SelectedHandler))
			{
				if (SelectedHandler != null)
				{
					SelectedHandler.Reset();
				}
				SelectedHandler = _sender;
				if (SelectedHandler != null)
				{
					SelectedHandler.SetIsSelected(true, _sender.Icon, _source, false);
				}
				OnSelectionChange(SelectedHandler);
			}
		}

		protected virtual void OnSelectionChange(ListItemHandler _sender)
		{
			SenderDelegate selectionChange = this.SelectionChange;
			if (selectionChange != null)
			{
				selectionChange(_sender);
			}
		}

		public virtual void OnScreenInit()
		{
			m_wrapper.OnItemCreated += OnItemCreated;
			m_wrapper.OnUpdateItem += OnItemUpdated;
			m_wrapper.Init();
		}

		public virtual void OnScreenActivated()
		{
		}

		protected virtual void OnItemUpdated(Transform _transform, int _index)
		{
			if (this.ItemUpdate != null)
			{
				this.ItemUpdate(_transform, _index);
			}
		}

		private void OnItemCreated(Transform _transform)
		{
			ListItemHandler component = _transform.GetComponent<ListItemHandler>();
			component.Selected += OnItemSelected;
			component.Deselected += OnItemDeselected;
			UIWidget component2 = _transform.GetComponent<UIWidget>();
			component2.leftAnchor.target = base.transform;
			component2.rightAnchor.target = base.transform;
			component2.rightAnchor.absolute = -m_rightMargin;
		}

		public void DeselectItem()
		{
			RemoveSelection();
			SetSelectedHandler(null, null);
		}

		public void RemoveSelection()
		{
			if (SelectedHandler != null)
			{
				SelectedHandler.SetIsSelected(false, null, null, false);
			}
		}

		private void Awake()
		{
		}

		private void OnItemSelected(ListItemHandler _source, IconHandler _sender)
		{
			SetSelectedHandler(_source, _sender);
		}

		private void OnItemDeselected(ListItemHandler _source, IconHandler _sender)
		{
			if (!(SelectedHandler != _source))
			{
				SetSelectedHandler(null, null);
			}
		}

		private void OnDestroy()
		{
			if (m_wrapper != null)
			{
				m_wrapper.OnItemCreated -= OnItemCreated;
				m_wrapper.OnUpdateItem -= OnItemUpdated;
			}
		}

		public virtual void ForceUpdate(bool _resetScrollbar)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(m_scrollBar != null, "Working on an uninitialized ListHandler!"))
			{
				return;
			}
			float value = m_scrollBar.value;
			if (_resetScrollbar)
			{
				m_scrollBar.value = 0f;
				if (Assets.Scripts.Engine.Helpers.Verify(m_wrapper != null && m_wrapper.WrapScrollView != null, "Working on an uninitialized ListHandler.m_wrapper!"))
				{
					m_wrapper.WrapScrollView.ResetPosition();
				}
			}
			else
			{
				m_scrollBar.value = value;
			}
		}

		public void ReloadScreen()
		{
			ForceUpdate(true);
		}

		public virtual void OnScreenDestroy()
		{
		}
	}
}
