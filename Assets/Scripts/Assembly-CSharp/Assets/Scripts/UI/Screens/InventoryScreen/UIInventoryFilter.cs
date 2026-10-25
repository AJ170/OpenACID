using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIInventoryFilter : MonoBehaviour, IInitialized<UIInventoryScreenData>
	{
		[SerializeField]
		private UIExtendedButton m_filterButton;

		[SerializeField]
		private UIDropDown m_filterDropDown;

		private UIInventoryScreenData m_data;

		private UIDropDownConfiguration m_config;

		private EFilter m_filter;

		private static bool IsSpecificTutorialActive
		{
			get
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25Forge) || UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25Equip) || UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs25MysteryBox) || UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(ETutorialCategory.TutCatMs26Modules);
			}
		}

		public event Action OnFilterOpen;

		public event Action OnFilterClose;

		public void Initialize(UIInventoryScreenData _data)
		{
			m_data = _data;
			m_config = new UIDropDownConfiguration
			{
				Items = new BBList<UIDropDownItemConfiguration>
				{
					new UIDropDownItemConfiguration
					{
						FilterType = EFilter.All,
						Selected = (m_filter == EFilter.All),
						IconName = "aUI_icon_filter_all",
						Label = LocalizationManager.Instance.Get(6705),
						OnStateChangedFinished = delegate(bool _selected, UIDropDownItemConfiguration _configuration)
						{
							if (_selected)
							{
								OnFilterSelected(EFilter.All, _configuration);
							}
						},
						IsEnabled = true
					},
					new UIDropDownItemConfiguration
					{
						FilterType = EFilter.Equipment,
						Selected = (m_filter == EFilter.Equipment),
						IconName = "aUI_icon_filter_equipment",
						Label = LocalizationManager.Instance.Get(6706),
						OnStateChangedFinished = delegate(bool _selected, UIDropDownItemConfiguration _configuration)
						{
							if (_selected)
							{
								OnFilterSelected(EFilter.Equipment, _configuration);
							}
						},
						IsEnabled = true
					},
					new UIDropDownItemConfiguration
					{
						FilterType = EFilter.Modules,
						Selected = (m_filter == EFilter.Modules),
						IconName = "aUI_icon_filter_module",
						Label = LocalizationManager.Instance.Get(6707),
						OnStateChangedFinished = delegate(bool _selected, UIDropDownItemConfiguration _configuration)
						{
							if (_selected)
							{
								OnFilterSelected(EFilter.Modules, _configuration);
							}
						},
						IsEnabled = true
					}
				}
			};
			UIDropDownItemConfiguration uIDropDownItemConfiguration = m_config.Items.Find((UIDropDownItemConfiguration _x) => _x.Selected);
			m_filterButton.SetLabel(uIDropDownItemConfiguration.Label);
			m_filterDropDown.Initialize(m_config);
			m_filterButton.isEnabled = !IsSpecificTutorialActive;
			UpdateScreenState(m_data.State);
			m_data.OnStateChanged += OnScreenStateChanged;
		}

		public void Clear()
		{
			m_data.OnStateChanged -= OnScreenStateChanged;
			m_data = null;
			m_filterDropDown.Clear();
			m_config = null;
			m_filter = EFilter.All;
		}

		public void Close()
		{
			m_filterDropDown.Close();
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_filterButton.onClick, OnFilterClicked);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_filterButton.onClick, OnFilterClicked);
		}

		private void OnScreenStateChanged(UIInventoryScreenData.StateChangeData _stateChangeData)
		{
			UpdateScreenState(_stateChangeData.NewState);
		}

		private void OnFilterClicked()
		{
			m_filterDropDown.Toggle();
			switch (m_filterDropDown.State)
			{
			case UIDropDown.EState.Closed:
				if (this.OnFilterClose != null)
				{
					this.OnFilterClose();
				}
				break;
			case UIDropDown.EState.Opened:
				if (this.OnFilterOpen != null)
				{
					this.OnFilterOpen();
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnFilterSelected(EFilter _filter, UIDropDownItemConfiguration _configuration)
		{
			m_filter = _filter;
			switch (_filter)
			{
			case EFilter.All:
				m_data.CurrentFilter = ItemFilters.FilterAll;
				break;
			case EFilter.Equipment:
				m_data.CurrentFilter = ItemFilters.FilterEquipment;
				break;
			case EFilter.Modules:
				m_data.CurrentFilter = ItemFilters.FilterModules;
				break;
			}
			m_filterButton.SetLabel(_configuration.Label);
			m_filterDropDown.Close();
		}

		private void UpdateScreenState(UIInventoryScreenData.EScreenState _newState)
		{
			switch (_newState)
			{
			case UIInventoryScreenData.EScreenState.Inventory:
				m_filterButton.isEnabled = !IsSpecificTutorialActive;
				OnFilterSelected(EFilter.All, m_config.Items.Find((UIDropDownItemConfiguration _x) => _x.FilterType == m_filter));
				UpdateSelectedFilterButton(EFilter.All);
				break;
			case UIInventoryScreenData.EScreenState.Details:
				m_filterButton.isEnabled = false;
				m_filterDropDown.Close();
				break;
			case UIInventoryScreenData.EScreenState.Forge:
				m_filterButton.isEnabled = false;
				m_data.CurrentFilter = ItemFilters.FilterForgeItems;
				m_filterDropDown.Close();
				UpdateSelectedFilterButton(EFilter.All);
				break;
			}
		}

		private void UpdateSelectedFilterButton(EFilter _filter)
		{
			if (m_config == null || m_config.Items == null)
			{
				return;
			}
			UIDropDownItemConfiguration uIDropDownItemConfiguration = null;
			for (int i = 0; i < m_config.Items.Count; i++)
			{
				UIDropDownItemConfiguration uIDropDownItemConfiguration2 = m_config.Items[i];
				uIDropDownItemConfiguration2.Selected = _filter == uIDropDownItemConfiguration2.FilterType;
				if (uIDropDownItemConfiguration2.Selected)
				{
					uIDropDownItemConfiguration = uIDropDownItemConfiguration2;
				}
			}
			if (uIDropDownItemConfiguration != null)
			{
				m_filterButton.SetLabel(uIDropDownItemConfiguration.Label);
			}
			m_filterDropDown.Initialize(m_config);
		}
	}
}
