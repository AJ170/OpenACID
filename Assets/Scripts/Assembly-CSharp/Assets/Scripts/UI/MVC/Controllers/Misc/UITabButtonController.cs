using System;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Models.Misc;
using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UITabButtonController : UIControllerBase
	{
		private static readonly Color32 s_timerSelectedColor = UIDataMapper.UITextColors.TimerColor;

		private static readonly Color32 s_timerDeselectedColor = new Color32(99, 97, 99, byte.MaxValue);

		[SerializeField]
		[CheckAssigned]
		private UITabButtonViewPresenter m_viewPresenter;

		[SerializeField]
		[CheckAssigned]
		private UITabButtonHeaderController m_headerController;

		private Action<UITabButtonController> m_onTabClicked;

		private bool m_isSelected;

		private bool m_isEnabled;

		private DateTime m_eventEndDate;

		public UITabButtonModel Model { get; set; }

		public UITabButtonHeaderController HeaderController
		{
			get
			{
				return m_headerController;
			}
		}

		public Action<UITabButtonController> OnClicked
		{
			get
			{
				return m_onTabClicked;
			}
			set
			{
				m_onTabClicked = value;
			}
		}

		public bool IsSelected
		{
			get
			{
				return m_isSelected;
			}
			set
			{
				if (m_isSelected != value)
				{
					m_isSelected = value;
					if (m_isSelected)
					{
						m_headerController.Show();
						m_viewPresenter.TimerColor = s_timerSelectedColor;
					}
					else
					{
						m_headerController.Hide();
						m_viewPresenter.TimerColor = s_timerDeselectedColor;
					}
					m_viewPresenter.IsSelected = m_isSelected;
				}
			}
		}

		public bool IsEnabled
		{
			get
			{
				return m_isEnabled;
			}
			set
			{
				if (m_isEnabled != value)
				{
					m_isEnabled = value;
					UpdateEnableState();
				}
			}
		}

		public void Initialize(UITabButtonModel _model)
		{
			Show();
			base.IsInitialized = true;
			m_isSelected = true;
			IsSelected = false;
			m_headerController.Initialize(_model);
			UpdateController(_model);
			OnUpdateUnity();
		}

		public void UpdateController(UITabButtonModel _model)
		{
			Model = _model;
			m_isEnabled = Model.IsEnabled;
			UpdateEnableState();
			m_viewPresenter.Title = LocalizationManager.Instance.Get(Model.TitleLoca);
			if (Model.SubTitleLoca.Valid)
			{
				m_viewPresenter.SubTitle = LocalizationManager.Instance.Get(Model.SubTitleLoca);
			}
			m_viewPresenter.SubTitleVisibility = Model.SubTitleLoca.Valid;
			m_eventEndDate = ServerTimeManager.ToUtcTime(Model.EndTimeUTC);
			if (m_headerController != null)
			{
				m_headerController.UpdateController(_model);
			}
		}

		public void DeselectHeaderItem()
		{
			if (m_headerController != null)
			{
				m_headerController.DeselectRewardItem();
			}
		}

		public void OnClickTab()
		{
			if (!IsSelected)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate);
				IsSelected = true;
				Action<UITabButtonController> onTabClicked = m_onTabClicked;
				if (onTabClicked != null)
				{
					m_onTabClicked(this);
				}
			}
		}

		public override void Show()
		{
			base.Show();
			m_viewPresenter.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			m_viewPresenter.gameObject.SetActive(false);
		}

		protected override void OnDisableUnity()
		{
			base.OnDisableUnity();
			DeselectHeaderItem();
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			double num = 0.0;
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			num = (m_eventEndDate - utcNow).TotalSeconds;
			if (num > 0.0)
			{
				m_viewPresenter.Timer = UIDuration.FromSeconds((long)num, true).ToString();
			}
			m_viewPresenter.TimerContainerVisibility = num > 0.0;
		}

		private void UpdateEnableState()
		{
			if (!m_isEnabled)
			{
				IsSelected = false;
			}
			m_viewPresenter.IsColliderEnabled = m_isEnabled;
		}
	}
}
