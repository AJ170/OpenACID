using System;
using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.HirelingScreen
{
	public class HirelingData : UIScreenChild
	{
		[SerializeField]
		private GameObject m_hirelingTabPrefab;

		[SerializeField]
		private HirelingDetail m_hirelingDetail;

		[SerializeField]
		private UIGrid m_tabContainer;

		[SerializeField]
		private GameObject startButton;

		[SerializeField]
		private UIWidget m_anchorContainer;

		[SerializeField]
		private UIFocusChildInScrollView m_focusChildInScrollView;

		private readonly List<HirelingTab> m_hirelingTabList = new List<HirelingTab>();

		private HirelingTab m_clickedTab;

		private HirelingAssassin[] m_hirelings;

		public List<HirelingTab> TabsList
		{
			get
			{
				return m_hirelingTabList;
			}
		}

		public HirelingTab CurrentSelectedTab
		{
			get
			{
				return m_clickedTab;
			}
		}

		public void UpdateHirelingData(string _hirelingId, Assassin _hirelingAssassin)
		{
			string text = ((!(m_clickedTab != null)) ? string.Empty : m_clickedTab.HirelingID);
			m_hirelings = Singleton<StateManager>.Instance.ProfileManager.Hirelings;
			int num = 0;
			for (int i = 0; i < m_hirelings.Length; i++)
			{
				HirelingAssassin hirelingAssassin = m_hirelings[i];
				if (hirelingAssassin.hirelingId == _hirelingId)
				{
					num = i;
					hirelingAssassin.itemPower = _hirelingAssassin.TotalItemPower();
					string _humanoidGuid = string.Empty;
					string _colorDefGuid = string.Empty;
					InventoryElement elementForSlot = _hirelingAssassin.GetElementForSlot(EInventorySlot.SlotOutfit);
					if (elementForSlot != null)
					{
						AvatarData.TryGetOutfitModification(elementForSlot, out _humanoidGuid, out _colorDefGuid);
					}
					if (string.IsNullOrEmpty(_humanoidGuid))
					{
						_humanoidGuid = _hirelingAssassin.humanoidGuid;
					}
					if (string.IsNullOrEmpty(_colorDefGuid))
					{
						_colorDefGuid = _hirelingAssassin.colorSetting;
					}
					hirelingAssassin.humanoidGuid = _humanoidGuid;
					hirelingAssassin.colorSetting = _colorDefGuid;
					InventoryElement elementForSlot2 = _hirelingAssassin.GetElementForSlot(EInventorySlot.SlotInsignia);
					if (elementForSlot2 != null)
					{
						hirelingAssassin.insigniaGuid = elementForSlot2.guid;
					}
					else
					{
						hirelingAssassin.insigniaGuid = string.Empty;
					}
					m_hirelings[i] = hirelingAssassin;
				}
			}
			Array.Sort(m_hirelings, HirelingComparer);
			HirelingAssassin hirelingAssassin2 = null;
			for (int j = 0; j < m_hirelings.Length; j++)
			{
				if (m_hirelings[j].hirelingId == _hirelingId)
				{
					hirelingAssassin2 = m_hirelings[j];
					break;
				}
			}
			m_hirelingDetail.UpdateTextureData(hirelingAssassin2);
			if (m_hirelings.Length > 0)
			{
				m_hirelingDetail.UpdateData(hirelingAssassin2);
			}
			for (int k = 0; k < m_hirelingTabList.Count; k++)
			{
				HirelingAssassin hirelingAssassin3 = m_hirelings[k];
				m_hirelingTabList[k].UpdateData(hirelingAssassin3);
				if (hirelingAssassin3.hirelingId == _hirelingId)
				{
					int num2 = k;
					bool flag = num != num2;
					if (m_clickedTab != null && flag)
					{
						m_clickedTab.StartDeselects(true);
					}
					m_clickedTab = m_hirelingTabList[k];
					if (flag)
					{
						m_clickedTab.StartSelections(true);
					}
				}
			}
			if (m_clickedTab != null)
			{
				m_focusChildInScrollView.FocusedObject = m_clickedTab.gameObject.transform;
			}
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onExit, OnScreenExit);
			m_focusChildInScrollView.MoveInstant = true;
			startButton.SetActive(false);
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onExit, OnScreenExit);
			}
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_hirelingTabList.Count; i++)
			{
				HirelingTab hirelingTab = m_hirelingTabList[i];
				if (hirelingTab != null && hirelingTab.gameObject != null)
				{
					UIButton uIEventTrigger = hirelingTab.UIEventTrigger;
					if (uIEventTrigger != null)
					{
						EventDelegate.Remove(uIEventTrigger.onClick, OnclickedTab);
					}
				}
				UnityEngine.Object.Destroy(hirelingTab);
			}
			m_hirelingTabList.Clear();
			m_clickedTab = null;
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged.Remove(UpdateHirelingDataReward);
		}

		private void OnScreenActivate()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.SetFirstAvailableStep();
			startButton.SetActive(false);
			m_hirelings = Singleton<StateManager>.Instance.ProfileManager.Hirelings;
			Array.Sort(m_hirelings, (HirelingAssassin _assassinA, HirelingAssassin _assassinB) => HirelingComparer(_assassinA, _assassinB));
			m_hirelingDetail.UpdateTextureData(GetFirstHireling());
			if (m_hirelings.Length > 0)
			{
				m_hirelingDetail.UpdateData(m_hirelings[0]);
			}
			StartCoroutine(CreateElements());
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged.Add(UpdateHirelingDataReward);
		}

		private void OnScreenExit()
		{
			for (int i = 0; i < m_hirelingTabList.Count; i++)
			{
				HirelingTab hirelingTab = m_hirelingTabList[i];
				if (hirelingTab != null && hirelingTab.gameObject != null)
				{
					UIButton component = hirelingTab.GetComponent<UIButton>();
					if (component != null)
					{
						EventDelegate.Remove(component.onClick, OnclickedTab);
					}
					UnityEngine.Object.Destroy(hirelingTab.gameObject);
				}
			}
			m_hirelings = null;
			m_hirelingTabList.Clear();
			Singleton<StateManager>.Instance.MissionSelectionManager.OnMissionStateChanged.Remove(UpdateHirelingDataReward);
		}

		private HirelingAssassin GetFirstHireling()
		{
			HirelingAssassin hirelingAssassin = null;
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsHirelingTutorialActive())
			{
				for (int i = 0; i < m_hirelings.Length; i++)
				{
					if (m_hirelings[i].classTypeId == Assassin.EClassTypeId.Berserker)
					{
						hirelingAssassin = m_hirelings[i];
						break;
					}
				}
				if (hirelingAssassin == null)
				{
					hirelingAssassin = m_hirelings[0];
				}
			}
			else
			{
				hirelingAssassin = m_hirelings[0];
			}
			return hirelingAssassin;
		}

		private IEnumerator CreateElements()
		{
			yield return 0;
			if (!(m_tabContainer != null))
			{
				yield break;
			}
			Transform anchorParentRight = m_anchorContainer.transform;
			Transform anchorParentLeft = m_anchorContainer.transform;
			for (int i = 0; i < m_hirelings.Length; i++)
			{
				HirelingAssassin hireling = m_hirelings[i];
				bool found = false;
				AvatarClassDefinitions.AvatarClass[] assassinClasses = Singleton<StateManager>.Instance.TemplateManager.AvatarDefinition.classes;
				foreach (AvatarClassDefinitions.AvatarClass avatarClass in assassinClasses)
				{
					if (avatarClass.classTypeId.Equals(hireling.classTypeId))
					{
						found = true;
						break;
					}
				}
				if (found)
				{
					GameObject newTab = NGUITools.AddChild(m_tabContainer.gameObject, m_hirelingTabPrefab);
					HirelingTab hirelingTab = newTab.GetComponent<HirelingTab>();
					UIWidget widget = newTab.GetComponent<UIWidget>();
					hirelingTab.HirelingIndex = i;
					widget.leftAnchor.target = anchorParentLeft;
					widget.rightAnchor.target = anchorParentRight;
					m_hirelingTabList.Add(hirelingTab);
					if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsHirelingTutorialActive())
					{
						break;
					}
				}
			}
			m_tabContainer.Reposition();
			StartCoroutine(FillData());
		}

		private IEnumerator FillData()
		{
			yield return 0;
			if (m_hirelings != null)
			{
				if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsHirelingTutorialActive())
				{
					ConnectHirelingToTab(GetFirstHireling(), 0);
				}
				else
				{
					for (int i = 0; i < m_hirelingTabList.Count; i++)
					{
						ConnectHirelingToTab(m_hirelings[m_hirelingTabList[i].HirelingIndex], i);
					}
				}
			}
			GetComponent<HirelingNavigation>().RemoteInitializeWhenDataIsReady(m_hirelingTabList);
			m_hirelingDetail.UpdateData(m_hirelingTabList[0].HirelingData.HirelingAssassinData);
			startButton.SetActive(true);
		}

		private int HirelingComparer(HirelingAssassin _hirelingAssassinA, HirelingAssassin _hirelingAssassinB)
		{
			if (_hirelingAssassinA.hirelingType != _hirelingAssassinB.hirelingType)
			{
				int num = ToSortingValue(_hirelingAssassinA.hirelingType);
				int value = ToSortingValue(_hirelingAssassinB.hirelingType);
				return num.CompareTo(value);
			}
			if (_hirelingAssassinA.itemPower != _hirelingAssassinB.itemPower)
			{
				return _hirelingAssassinB.itemPower.CompareTo(_hirelingAssassinA.itemPower);
			}
			if (_hirelingAssassinA.classTypeId != _hirelingAssassinB.classTypeId)
			{
				return _hirelingAssassinA.classTypeId.CompareTo(_hirelingAssassinB.classTypeId);
			}
			return _hirelingAssassinA.hirelingId.CompareTo(_hirelingAssassinB.hirelingId);
		}

		private int ToSortingValue(HirelingAssassin.EHirelingType _hirelingType)
		{
			switch (_hirelingType)
			{
			case HirelingAssassin.EHirelingType.Random:
				return 2;
			case HirelingAssassin.EHirelingType.RandomGenerated:
				return 3;
			case HirelingAssassin.EHirelingType.Friend:
				return 1;
			default:
				return 0;
			}
		}

		private void OnclickedTab()
		{
			m_clickedTab = UIButton.current.GetComponent<HirelingTab>();
			m_hirelingDetail.UpdateTextureData(m_clickedTab.HirelingData.HirelingAssassinData);
			m_hirelingDetail.UpdateData(m_clickedTab.HirelingData.HirelingAssassinData);
			Main.Instance.BuildDefinition.Hireling = m_clickedTab.HirelingID;
		}

		private void ConnectHirelingToTab(HirelingAssassin _hireling, int _tabIndex)
		{
			HirelingTab hirelingTab = m_hirelingTabList[_tabIndex];
			hirelingTab.UpdateData(_hireling);
			EventDelegate.Add(hirelingTab.UIEventTrigger.onClick, OnclickedTab);
		}

		private void UpdateHirelingDataReward(object[] _params)
		{
			m_hirelingDetail.UpdateRewardSeperately();
		}
	}
}
