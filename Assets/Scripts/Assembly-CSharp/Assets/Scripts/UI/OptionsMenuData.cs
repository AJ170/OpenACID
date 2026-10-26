using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours.Misc;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.LoadoutScreen;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class OptionsMenuData : UIScreenChild
	{
		[SerializeField]
		private GenericOptionsTab m_genericOptionsTabPrefab;

		[SerializeField]
		private UIGrid m_grid;

		[SerializeField]
		private OptionsMenuNavigation m_navigation;

		[SerializeField]
		private UIWidget m_naviagtionButtons;

		[SerializeField]
		private GameObject m_objectivesList;

		[SerializeField]
		private UISprite m_missionTypeIcon;

		[SerializeField]
		private UISprite m_missionTypeIconbackground;

		[SerializeField]
		private UIScrollView m_missionTableScrollView;

		[SerializeField]
		private UIPanel m_missionScrollViewPanel;

		[SerializeField]
		private UIObjectiveBehaviour Challenges;

		[SerializeField]
		private List<GameObject> m_challengeContainer = new List<GameObject>();

		[SerializeField]
		private UIObjectiveBehaviour Modifiers;

		[SerializeField]
		private UITable MissionTable;

		[SerializeField]
		private UICheckboxBehaviour MainGoal;

		[SerializeField]
		private GameObject AnimusTexture;

		[SerializeField]
		private UICheckboxBehaviour m_trackerMainGoal;

		[SerializeField]
		private UIObjectiveBehaviour m_trackerModifiers;

		[SerializeField]
		private List<GameObject> m_trackerGoalContainer = new List<GameObject>();

		[SerializeField]
		private UISprite m_trackerMissionTypeIcon;

		[SerializeField]
		private UISprite m_trackerMissionTypeIconbackground;

		private GenericOptionsTab m_everyplayTab;

		private GenericOptionsTab m_soundTab;

		private GenericOptionsTab m_controlsTab;

		private GenericOptionsTab m_gamepadTab;

		private GenericOptionsTab m_generalSettingsTab;

		private GenericOptionsTab m_supportTab;

		private GenericOptionsTab m_creditsTab;

		private GenericOptionsTab m_resetProfileTab;

		private UIMissionResultData m_missionResultData;

		private readonly List<GenericOptionsTab> m_allTabs = new List<GenericOptionsTab>();

		private bool m_updateData;

		private int m_repositionCounter;

		protected override void OnScreenInit()
		{
			m_navigation.InGame = !Main.Instance.SceneLoader.InAnimus;
			EventDelegate.Add(m_screen.onEnter, ScreenEnter);
			CreateElements();
			FillInData();
			SelfUpdate();
			m_navigation.GetActivated(m_allTabs, m_grid);
			m_objectivesList.SetActive(m_navigation.InGame);
			if (EveryplayManager.IsSupported)
			{
				EveryplayManager.RecordingStarted += OnRecordingStartedOrStopped;
				EveryplayManager.RecordingStopped += OnRecordingStartedOrStopped;
			}
		}

		protected override void OnScreenDestroy()
		{
			EveryplayManager.RecordingStarted -= OnRecordingStartedOrStopped;
			EveryplayManager.RecordingStopped -= OnRecordingStartedOrStopped;
			if (m_everyplayTab != null)
			{
				EventDelegate.Remove(m_everyplayTab.Button.onClick, OnRecordVideo);
			}
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onEnter, ScreenEnter);
			}
			for (int i = 0; i < m_allTabs.Count; i++)
			{
				m_allTabs[i].m_Icon.atlas = null;
				NGUITools.Destroy(m_allTabs[i].m_Icon.atlas);
			}
		}

		protected void ScreenEnter()
		{
			m_updateData = true;
			m_repositionCounter = 0;
			bool activeSelf = m_creditsTab.gameObject.activeSelf;
			bool flag = !m_navigation.InGame && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive;
			if (flag != activeSelf)
			{
				m_creditsTab.gameObject.SetActive(flag);
				m_grid.repositionNow = true;
			}
		}

		private void SelfUpdate()
		{
			if (m_soundTab.m_correspondingScreen == null)
			{
				m_soundTab.m_correspondingScreen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.SoundMenu);
				m_controlsTab.m_correspondingScreen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.ControlsMenu);
				m_generalSettingsTab.m_correspondingScreen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.GeneralSettingsMenu);
				m_supportTab.m_correspondingScreen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.SupportScreen);
				m_creditsTab.m_correspondingScreen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.CreditsScreen);
				m_gamepadTab.m_correspondingScreen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.GamepadScreen);
			}
			if (UIResetProfileHelper.IsEnabled)
			{
				m_resetProfileTab.gameObject.SetActive(!m_navigation.InGame);
			}
			if (m_navigation.InGame)
			{
				m_generalSettingsTab.gameObject.SetActive(false);
				m_gamepadTab.gameObject.SetActive(true);
				m_controlsTab.gameObject.SetActive(true);
				m_naviagtionButtons.gameObject.SetActive(true);
				m_creditsTab.gameObject.SetActive(false);
			}
			else
			{
				m_generalSettingsTab.gameObject.SetActive(true);
				m_gamepadTab.gameObject.SetActive(true);
				m_controlsTab.gameObject.SetActive(false);
				m_naviagtionButtons.gameObject.SetActive(false);
				m_supportTab.gameObject.SetActive(true);
				m_creditsTab.gameObject.SetActive(true);
			}
			m_objectivesList.SetActive(m_navigation.InGame);
			m_updateData = true;
		}

		private void CreateElements()
		{
			int num = 7;
			if (UIResetProfileHelper.IsEnabled)
			{
				num++;
			}
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = NGUITools.AddChild(m_grid.gameObject, m_genericOptionsTabPrefab.gameObject);
				GenericOptionsTab component = gameObject.GetComponent<GenericOptionsTab>();
				component.SetAnchoring(m_missionScrollViewPanel);
				m_allTabs.Add(component);
			}
			m_everyplayTab = m_allTabs[0];
			m_soundTab = m_allTabs[1];
			m_generalSettingsTab = m_allTabs[2];
			m_controlsTab = m_allTabs[3];
			m_gamepadTab = m_allTabs[4];
			m_supportTab = m_allTabs[5];
			m_creditsTab = m_allTabs[6];
			if (UIResetProfileHelper.IsEnabled)
			{
				m_resetProfileTab = m_allTabs[num - 1];
			}
			m_grid.repositionNow = true;
		}

		private void FillInData()
		{
			UIAtlas atlasByType = Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Shared);
			UIAtlas atlas = null;
			if (!m_navigation.InGame)
			{
				atlas = Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Animus);
			}
			if (m_navigation.InGame && EveryplayManager.IsSupported)
			{
				m_everyplayTab.UpdateData(atlasByType, "aUI_img_option_everyplay", LocalizationManager.Instance.Get(6484));
				EventDelegate.Add(m_everyplayTab.Button.onClick, OnRecordVideo);
				OnUpdateRecordVideoButton();
				m_everyplayTab.gameObject.SetActive(true);
			}
			else
			{
				m_everyplayTab.gameObject.SetActive(false);
			}
			m_soundTab.UpdateData(atlasByType, "aUI_img_option_sound", LocalizationManager.Instance.Get(2120));
			if (!m_navigation.InGame)
			{
				m_generalSettingsTab.UpdateData(atlas, "aUI_img_general_settings", LocalizationManager.Instance.Get(2128));
				m_supportTab.UpdateData(atlasByType, "aUI_img_option_support", LocalizationManager.Instance.Get(2129));
			}
			else
			{
				m_controlsTab.UpdateData(atlasByType, "aUI_img_option_controls", LocalizationManager.Instance.Get(2121));
				m_supportTab.gameObject.SetActive(false);
			}
			m_gamepadTab.UpdateData(atlasByType, "aUI_img_option_gamepad", LocalizationManager.Instance.Get(7792));
			if (!m_navigation.InGame)
			{
				m_creditsTab.UpdateData(atlas, "aUI_img_option_credits", LocalizationManager.Instance.Get(2130));
				m_creditsTab.m_changeScreenID = EScreenID.CreditsScreen;
			}
			else
			{
				m_creditsTab.gameObject.SetActive(false);
			}
			if (UIResetProfileHelper.IsEnabled)
			{
				m_resetProfileTab.UpdateData(atlas, "aUI_img_option_reset", LocalizationManager.Instance.Get(10307));
				EventDelegate.Add(m_resetProfileTab.Button.onClick, OnResetProfileClicked);
			}
			m_updateData = true;
		}

		public void Update()
		{
			if (m_updateData)
			{
				m_updateData = false;
				SetObjectivesData();
			}
		}

		private void SetObjectivesData()
		{
			AnimusTexture.SetActive(!m_navigation.InGame);
			if (m_navigation.InGame)
			{
				UpdateMissionGoals();
				UpdateMissionTracker();
			}
		}

		private void UpdateMissionTracker()
		{
			// A mission without objectives (offline hand-authored missions) has nothing to track.
			var collected =UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.CollectedObjectiveData;
			if (collected == null || collected.Count == 0 || collected[collected.Count - 1].ObjectivesBaseData == null || collected[collected.Count - 1].ObjectivesBaseData.Count == 0)
			{
				return;
			}
			MissionTracker.ObjectiveData objectiveData = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.CollectedObjectiveData.Last();
			IObjectiveBase objectiveBase = objectiveData.ObjectivesBaseData.First();
			for (int i = 0; i < m_trackerGoalContainer.Count; i++)
			{
				m_trackerGoalContainer[i].SetActive(!objectiveBase.ObjectiveData.HideDescription);
			}
			if (objectiveBase.ObjectiveData.HideDescription)
			{
				return;
			}
			ObjectiveBaseData objectiveData2 = objectiveBase.ObjectiveData;
			UICheckboxBehaviour.ECheckedState state = UIObjectiveBehaviour.GetState(objectiveData2.ID, true);
			m_trackerMainGoal.SetData(objectiveBase.Description, state);
			UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_trackerMissionTypeIcon, objectiveData2.TrackerIcon, MissionMetaDataManager.Instance.MissionGoals.IsElite, m_trackerMissionTypeIconbackground);
			List<UIMissionGoals.GoalData> list = new List<UIMissionGoals.GoalData>();
			IObjectiveBase objectiveBase2 = objectiveData.ObjectivesBaseData[0];
			string secondaryDescription = objectiveBase2.SecondaryDescription;
			if (!string.IsNullOrEmpty(secondaryDescription))
			{
				list.Add(new UIMissionGoals.GoalData
				{
					Name = secondaryDescription,
					ObjectiveId = objectiveBase2.ObjectiveData.ID
				});
			}
			for (int j = 1; j < objectiveData.ObjectivesBaseData.Count; j++)
			{
				IObjectiveBase objectiveBase3 = objectiveData.ObjectivesBaseData[j];
				if (!objectiveBase3.ObjectiveData.HideDescription)
				{
					list.Add(new UIMissionGoals.GoalData
					{
						Name = objectiveBase3.Description,
						ObjectiveId = objectiveBase3.ObjectiveData.ID
					});
				}
			}
			bool flag = list.Count > 0;
			m_trackerModifiers.SetActive(flag);
			if (flag)
			{
				m_trackerModifiers.SetData(list.ToArray(), RepositionTable, false, true);
			}
			foreach (GameObject item in m_trackerGoalContainer)
			{
				item.SetActive(true);
			}
			m_repositionCounter++;
			RepositionTable();
		}

		private void UpdateMissionGoals()
		{
			UIMissionGoals missionGoals = MissionMetaDataManager.Instance.MissionGoals;
			if (missionGoals != null)
			{
				List<UIMissionGoals.GoalData> list = missionGoals.Objectives.Where((UIMissionGoals.GoalData x) => !x.Hidden);
				if (list.Count > 0)
				{
					UICheckboxBehaviour.ECheckedState state = UIObjectiveBehaviour.GetState(list[0].ObjectiveId, true);
					MainGoal.SetData(list[0].Name, state);
				}
				m_missionResultData = new UIMissionResultData();
				m_missionResultData.UpdateChallengeData();
				if (m_missionResultData.ObjectiveType == EObjectiveType.Story_EObjectiveType || m_missionResultData.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType)
				{
					UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_missionTypeIcon, m_missionResultData.ObjectiveType, false, m_missionTypeIconbackground);
				}
				else
				{
					UIMissionUtil.UpdateMissionIconByMissionObjectiveType(m_missionTypeIcon, m_missionResultData.BaseType, missionGoals.IsElite, m_missionTypeIconbackground);
				}
				bool flag = missionGoals.Challenges != null && missionGoals.Challenges.Any((UIMissionGoals.GoalData x) => !x.Hidden);
				for (int i = 0; i < m_challengeContainer.Count; i++)
				{
					m_challengeContainer[i].SetActive(flag);
				}
				Challenges.SetActive(flag);
				if (flag)
				{
					m_repositionCounter++;
					Challenges.SetData(missionGoals.Challenges.ToArray(), RepositionTable, true);
				}
				int num = ((missionGoals.AdditionalObjectives != null) ? missionGoals.AdditionalObjectives.Where((UIMissionGoals.GoalData x) => !x.Hidden).Count : 0);
				bool flag2 = num > 0;
				Modifiers.SetActive(flag2);
				if (flag2)
				{
					m_repositionCounter++;
					Modifiers.SetData(missionGoals.AdditionalObjectives.ToArray(), RepositionTable, false, true);
				}
				if (m_repositionCounter == 0)
				{
					RepositionTable();
				}
			}
		}

		private void RepositionTable()
		{
			m_repositionCounter--;
			if (m_repositionCounter <= 0)
			{
				MissionTable.Reposition();
				m_missionTableScrollView.ResetPosition();
				MainGoal.gameObject.SetActive(true);
			}
		}

		private void OnUpdateRecordVideoButton()
		{
			m_everyplayTab.Button.Button.SetLabel((!EveryplayManager.IsRecording) ? 6485 : 6486);
		}

		private void OnRecordVideo()
		{
			EveryplayManager.ToggleRecording();
			if (EveryplayManager.IsRecording)
			{
				m_navigation.Deactivate();
			}
		}

		private void OnRecordingStartedOrStopped()
		{
			OnUpdateRecordVideoButton();
		}

		private void OnResetProfileClicked()
		{
			UIResetProfileHelper.OpenResetProfilePopup();
		}
	}
}
