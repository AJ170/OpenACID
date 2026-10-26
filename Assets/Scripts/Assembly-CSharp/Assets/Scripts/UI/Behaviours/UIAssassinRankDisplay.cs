using System;
using System.Collections.Generic;
using System.Globalization;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Swiper;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Humanoids;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIAssassinRankDisplay : MonoBehaviour, IInitialized
	{
		public enum EDirection
		{
			Left = 0,
			Right = 1
		}

		public class AssassinHandle
		{
			public CharacterAppearance Appearance { get; set; }

			public bool IsPlaceHolder { get; set; }

			public Assassin RawAssassin { get; set; }
		}

		private static readonly StringId s_ClassNameStringId = 2116;

		private List<AssassinHandle> m_assassinList;

		private bool m_canCreateAssassins;

		private bool m_showSwitchAssassins = true;

		private bool m_canSwitchAssassins = true;

		private bool m_canSwitchRank = true;

		private UIDropDownConfiguration m_config;

		private bool m_createVisuals = true;

		private bool m_isBoxVisible;

		private int m_index;

		private bool m_isUpdating = true;

		[SerializeField]
		private UIExtendedLabel m_classLabel;

		[SerializeField]
		private List<GameObject> m_displayElements;

		[SerializeField]
		private UISprite m_plusIconSprite;

		[SerializeField]
		private UISprite m_levelBackground;

		[SerializeField]
		private UIExtendedLabel m_levelLabel;

		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		private UIExtendedButton m_nextButton;

		[SerializeField]
		private UIPageIndicator m_pageIndicator;

		[SerializeField]
		private UIHeroicPortraitTexture m_portrait;

		[SerializeField]
		private UIExtendedButton m_previousButton;

		[SerializeField]
		private UIProgressBar m_progressBar;

		[SerializeField]
		private UIExtendedButton m_rankButton;

		[SerializeField]
		private UIDropDown m_rankDropDown;

		[SerializeField]
		private GameObject m_rankGameObject;

		[SerializeField]
		private UIExtendedLabel m_xpLabel;

		public int AssassinCount
		{
			get
			{
				return m_assassinList.Count;
			}
		}

		public bool CanCreateAssassins
		{
			get
			{
				return m_canCreateAssassins;
			}
			set
			{
				m_canCreateAssassins = value;
				UpdateAssassinList();
				SetSwitchButtons();
			}
		}

		public bool CanSwitchAssassins
		{
			get
			{
				return m_canSwitchAssassins && m_showSwitchAssassins;
			}
			set
			{
				m_canSwitchAssassins = value;
			}
		}

		public bool ShowSwitchAssassins
		{
			get
			{
				return m_showSwitchAssassins;
			}
			set
			{
				m_showSwitchAssassins = value;
				SetSwitchButtons();
			}
		}

		public bool CanSwitchRank
		{
			get
			{
				return m_canSwitchRank;
			}
			set
			{
				m_canSwitchRank = value;
				SetRank();
				Close();
			}
		}

		public bool CreateVisuals
		{
			get
			{
				return m_createVisuals;
			}
			set
			{
				if (m_createVisuals != value)
				{
					m_createVisuals = value;
					UpdateAssassinList();
				}
			}
		}

		public AssassinHandle Current
		{
			get
			{
				return (Index < m_assassinList.Count) ? m_assassinList[Index] : null;
			}
		}

		public bool HasAssassins
		{
			get
			{
				return m_assassinList.Count > 0;
			}
		}

		public int Index
		{
			get
			{
				return m_index;
			}
			private set
			{
				if (value != m_index)
				{
					m_index = value;
					WriteAssassinState();
				}
			}
		}

		public AssassinHandle Next
		{
			get
			{
				return (Index + 1 < m_assassinList.Count) ? m_assassinList[Index + 1] : null;
			}
		}

		private bool IsBoxVisible
		{
			get
			{
				return m_isBoxVisible;
			}
			set
			{
				m_isBoxVisible = value;
				foreach (GameObject displayElement in m_displayElements)
				{
					if (displayElement != null)
					{
						displayElement.SetActive(value);
					}
				}
			}
		}

		public bool IsUpdating
		{
			get
			{
				return m_isUpdating;
			}
			set
			{
				m_isUpdating = value;
			}
		}

		public event Action<EDirection> OnSelectionChanged;

		public void Clear()
		{
			m_rankDropDown.Clear();
		}

		public void Open()
		{
			if (!(m_rankDropDown == null))
			{
				m_rankDropDown.SetState(UIDropDown.EState.Opened);
			}
		}

		public void Close()
		{
			if (!(m_rankDropDown == null))
			{
				m_rankDropDown.SetState(UIDropDown.EState.Closed);
			}
		}

		public void FocusValidAssassin(bool _newSlot, bool _gotoFirstEmptySlot)
		{
			int index = 0;
			if (_newSlot)
			{
				bool flag = Singleton<StateManager>.Instance.ProfileManager.Profile.CanBuyMoreAssassinSlots();
				index = m_assassinList.Count - ((!flag) ? 1 : 2);
			}
			else if (_gotoFirstEmptySlot)
			{
				for (int i = 0; i < m_assassinList.Count; i++)
				{
					if (m_assassinList[i].IsPlaceHolder)
					{
						index = i;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins.Length; j++)
				{
					if (Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins[j].id == Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId)
					{
						index = j;
						break;
					}
				}
			}
			Index = index;
			Initialize();
			InvokeOnSelectionChanged(EDirection.Left);
		}

		public void Initialize()
		{
			UpdateAssassinList();
			IsBoxVisible = Index <= Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinIndex;
			SetRank();
			SetAssassinData();
			SetXpProgress();
			UpdatePortraits();
		}

		public void OnDisable()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChanged);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Remove(OnProfileUpdate);
			Clear();
		}

		public void OnEnable()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChanged);
			Singleton<StateManager>.Instance.ProfileManager.OnProfileUpdate.Add(OnProfileUpdate);
			Initialize();
		}

		private void OnProfileUpdate(object[] _params)
		{
			Initialize();
		}

		public void OnNextClicked()
		{
			if (CanSwitchAssassins)
			{
				Index = Clamp(Index + 1, 0, m_assassinList.Count - 1);
				Initialize();
				InvokeOnSelectionChanged(EDirection.Right);
			}
		}

		public void OnPreviousClicked()
		{
			if (CanSwitchAssassins)
			{
				Index = Clamp(Index - 1, 0, m_assassinList.Count - 1);
				Initialize();
				InvokeOnSelectionChanged(EDirection.Left);
			}
		}

		public void Refresh()
		{
			Initialize();
		}

		public void Refresh(bool _placeHolder)
		{
			Initialize();
		}

		public void ShowAsValidLevel(bool _buttonEnabled)
		{
		}

		public bool ValidateAssassin()
		{
			UIAssassinContainer assassinContainer = Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer;
			if (assassinContainer.Display.Current != null && !assassinContainer.Display.Current.IsPlaceHolder)
			{
				return true;
			}
			for (int num = m_assassinList.Count - 1; num >= 0; num--)
			{
				if (!m_assassinList[num].IsPlaceHolder)
				{
					Index = num;
					Initialize();
					InvokeOnSelectionChanged(EDirection.Left);
					return false;
				}
			}
			return true;
		}

		private static int Clamp(int _value, int _min, int _max)
		{
			return Math.Max(Math.Min(_value, _max), _min);
		}

		private void Awake()
		{
			m_assassinList = new List<AssassinHandle>();
		}

		private void InvokeOnSelectionChanged(EDirection _direction)
		{
			Action<EDirection> onSelectionChanged = this.OnSelectionChanged;
			if (onSelectionChanged != null)
			{
				onSelectionChanged(_direction);
			}
		}

		private void OnAssassinChanged(object[] _params)
		{
			Initialize();
		}

		private void OnRankChanged(int _rank, UIDropDownItemConfiguration _configuration)
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank = _rank;
			m_rankButton.SetLabel(_configuration.Label);
			m_rankDropDown.Close();
			UpdatePortraits();
		}

		private void RefreshPageIndicator()
		{
			m_pageIndicator.SelectPage(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinIndex, Singleton<StateManager>.Instance.ProfileManager.AssassinCount);
		}

		private void SetAssassinData()
		{
			SetSwitchButtons();
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin != null)
			{
				m_nameLabel.text = currentAssassin.GetAssassinName();
				m_levelLabel.text = currentAssassin.level.ToString(CultureInfo.InvariantCulture);
				RefreshPageIndicator();
				if (currentAssassin.extendedClassTypeId != Assassin.EClassTypeId.None)
				{
					string value = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)currentAssassin.classTypeId].NameId);
					string value2 = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)currentAssassin.extendedClassTypeId].NameId);
					m_classLabel.text = LocalizationManager.Instance.Get(s_ClassNameStringId, new StringMapper
					{
						{ "Primary_Class_Name", value },
						{ "Secondary_Class_Name", value2 }
					});
				}
				else
				{
					m_classLabel.text = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)currentAssassin.classTypeId].NameId);
				}
			}
		}

		private void SetRank()
		{
			if (Assets.Scripts.Offline.OfflineMode.Enabled)
			{
				// Heroic ranks come from the server statics.
				return;
			}
			m_rankGameObject.SetActive(m_canSwitchRank);
			if (!m_canSwitchRank)
			{
				return;
			}
			m_config = new UIDropDownConfiguration
			{
				Items = new BBList<UIDropDownItemConfiguration>()
			};
			HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
			for (int i = 0; i < heroicRankMapping.Length; i++)
			{
				bool selected = Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank == i;
				HeroicRankDescription value;
				UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(i, out value);
				string label = LocalizationManager.Instance.Get(value.NameId);
				int rankIndex = i;
				Action<bool, UIDropDownItemConfiguration> onStateChangedFinished = delegate(bool _selected, UIDropDownItemConfiguration _configuration)
				{
					if (_selected)
					{
						OnRankChanged(rankIndex, _configuration);
					}
				};
				UIDropDownItemConfiguration uIDropDownItemConfiguration = new UIDropDownItemConfiguration();
				uIDropDownItemConfiguration.Data = rankIndex;
				uIDropDownItemConfiguration.Selected = selected;
				uIDropDownItemConfiguration.IconName = UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription[rankIndex].NormalIcon;
				uIDropDownItemConfiguration.IconNameDisabled = UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription[rankIndex].DisabledIcon;
				uIDropDownItemConfiguration.Label = label;
				uIDropDownItemConfiguration.OnStateChangedFinished = onStateChangedFinished;
				uIDropDownItemConfiguration.IsEnabled = EntitlementExtensions.IsHeroicRankGranted(i);
				UIDropDownItemConfiguration item = uIDropDownItemConfiguration;
				m_config.Items.Add(item);
			}
			UIDropDownItemConfiguration uIDropDownItemConfiguration2 = m_config.Items.Find((UIDropDownItemConfiguration _x) => _x.Selected);
			m_rankButton.SetLabel(uIDropDownItemConfiguration2.Label);
			m_rankDropDown.Initialize(m_config);
		}

		private void SetSwitchButtons()
		{
			bool flag = false;
			bool flag2 = false;
			if (m_assassinList != null)
			{
				flag = m_assassinList.Count > 1 && Index > 0;
				flag2 = m_assassinList.Count > 1 && Index < m_assassinList.Count - 1;
			}
			bool flag3 = Index + 1 >= Singleton<StateManager>.Instance.ProfileManager.AssassinCount + Singleton<StateManager>.Instance.ProfileManager.FreeAssassinSlots;
			m_previousButton.gameObject.SetActive(flag && ShowSwitchAssassins);
			m_nextButton.gameObject.SetActive(flag2 && ShowSwitchAssassins);
			m_plusIconSprite.gameObject.SetActive(flag3 && ShowSwitchAssassins);
		}

		private void SetXpProgress()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			string text = "0";
			float value = 0f;
			if (currentAssassin != null)
			{
				int num = UIUtil.FormatPercentValue(currentAssassin.GetXPFactor());
				value = (float)num / 100f;
				text = string.Format("{0} / {1}", LocalizationManager.Instance.GetInteger(currentAssassin.GetCurrentXPRelativeToCurrentLevel()), LocalizationManager.Instance.GetInteger(currentAssassin.GetCurrentLevelMaxXP()));
			}
			m_progressBar.value = value;
			m_xpLabel.text = text;
		}

		private void UpdateAssassinList()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			Assassin[] assassins = profileManager.Profile.assassins.assassins;
			int assassinCount = profileManager.AssassinCount;
			List<AssassinHandle> list = new List<AssassinHandle>(assassinCount);
			for (int i = 0; i < assassinCount; i++)
			{
				CharacterAppearance appearance = ((!CreateVisuals) ? null : new CharacterAppearance(assassins[i]));
				list.Add(new AssassinHandle
				{
					Appearance = appearance,
					RawAssassin = assassins[i],
					IsPlaceHolder = false
				});
			}
			if (CanCreateAssassins)
			{
				int num = (profileManager.Profile.CanBuyMoreAssassinSlots() ? 1 : 0);
				for (int j = 0; j < profileManager.FreeAssassinSlots + num; j++)
				{
					CharacterAppearance appearance2 = new CharacterAppearance();
					HumanoidDef item = Main.Instance.GameDB.GetItem<HumanoidDef>(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_silhouette);
					Singleton<HumanoidFactory>.Instance.RandomizeHumanoid(item, appearance2, false);
					list.Add(new AssassinHandle
					{
						Appearance = appearance2,
						RawAssassin = null,
						IsPlaceHolder = true
					});
				}
			}
			m_assassinList = list;
		}

		private void UpdatePortraits()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin != null)
			{
				UIPortraitUtil.UpdateHeroicPortraitByAssassin(currentAssassin, Singleton<StateManager>.Instance.MissionSelectionManager.ContractHeroicMissionRank, m_portrait);
				m_portrait.gameObject.SetActive(true);
			}
		}

		private void WriteAssassinState()
		{
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			profileManager.CurrentAssassinIndex = Clamp(Index, 0, profileManager.AssassinCount - 1);
			profileManager.SelectedAssassin = (Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Current.IsPlaceHolder ? null : Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.Current.RawAssassin.id);
		}
	}
}
