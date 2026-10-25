using System;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public abstract class UIMissionListBase : UIMissionSubMenuBase
	{
		[SerializeField]
		protected UIExtendedWrapContent m_wrapperContainer;

		[SerializeField]
		protected UIRect m_itemAnchorTarget;

		[SerializeField]
		protected UIPanel m_scrollPanel;

		protected UIMissionItemBase m_currentSelected;

		private int m_previousIndex;

		private int m_currentSelectedIndex;

		public string CurrentSelectedId { get; set; }

		public int CurrentSelectedIndex
		{
			get
			{
				return m_currentSelectedIndex;
			}
			set
			{
				if (CurrentSelectedIndex != value)
				{
					m_previousIndex = m_currentSelectedIndex;
				}
				m_currentSelectedIndex = value;
			}
		}

		public UIMissionItemBase CurrentSelected
		{
			get
			{
				return m_currentSelected;
			}
			set
			{
				if (CurrentSelected != value && CurrentSelected != null && CurrentSelected.Index == CurrentSelectedIndex)
				{
					CurrentSelected.IsSelected = false;
				}
				if (value != null)
				{
					CurrentSelectedIndex = value.Index;
					CurrentSelectedId = value.ItemId;
				}
				m_currentSelected = value;
				OnCurrentSelectedChanged(m_previousIndex);
			}
		}

		public override void OnInit(EarthPinManager _earthPinManager)
		{
			base.OnInit(_earthPinManager);
			m_wrapperContainer.OnUpdateItem += OnWraperItemUpdate;
			m_wrapperContainer.OnItemCreated += OnWrapperItemCreated;
			m_wrapperContainer.MoveInstant = true;
			m_wrapperContainer.Init();
		}

		public void OnDestroy()
		{
			m_currentSelected = null;
			m_wrapperContainer.OnUpdateItem -= OnWraperItemUpdate;
			m_wrapperContainer.OnItemCreated -= OnWrapperItemCreated;
			if (m_wrapperContainer.Children == null)
			{
				return;
			}
			for (int i = 0; i < m_wrapperContainer.Children.Count; i++)
			{
				Transform transform = m_wrapperContainer.Children[i];
				if (transform != null)
				{
					UIMissionItemBase component = transform.GetComponent<UIMissionItemBase>();
					if (component != null)
					{
						component.OnItemClickedCallback = (Action<UIMissionItemBase>)Delegate.Remove(component.OnItemClickedCallback, new Action<UIMissionItemBase>(OnItemClicked));
					}
				}
			}
		}

		protected abstract void SetupEarthGlobeConnection();

		protected abstract void OnWraperItemUpdate(Transform _transform, int _index);

		protected void OnWrapperItemCreated(Transform _transform)
		{
			UIMissionItemBase component = _transform.GetComponent<UIMissionItemBase>();
			if (component != null)
			{
				component.UpdateItemAnchor(m_itemAnchorTarget);
				component.OnItemClickedCallback = (Action<UIMissionItemBase>)Delegate.Combine(component.OnItemClickedCallback, new Action<UIMissionItemBase>(OnItemClicked));
			}
		}

		protected virtual void OnItemClicked(UIMissionItemBase _selectedItem)
		{
			if (CurrentSelectedId == _selectedItem.ItemId)
			{
				OnActionButtonClicked(UIMissionActionButtonBar.EButtonType.Primary);
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_accept);
			}
			else
			{
				CurrentSelected = _selectedItem;
			}
		}

		protected virtual void OnCurrentSelectedChanged(int _previous)
		{
			SetupEarthGlobeConnection();
		}
	}
}
