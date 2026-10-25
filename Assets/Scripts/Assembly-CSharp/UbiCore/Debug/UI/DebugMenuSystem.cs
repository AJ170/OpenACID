using System;
using System.Collections.Generic;
using System.IO;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup.AppRatingPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuSystem : DebugMenuItem
	{
		private SystemDebugOutput m_DebugOutput;

		private readonly string[] ErrorCodeNames = Enum.GetNames(typeof(ErrorCode));

		private readonly string[] ErrorCategoryNames = Enum.GetNames(typeof(ErrorCategory));

		private Vector2 m_exceptionScrollPosition;

		private int m_selectedExceptionIndex;

		private bool m_showExceptionSelectionBox;

		private int m_selectedExceptionCatIndex;

		private bool m_showExceptionCatSelectionBox;

		private bool m_forceDuringSync;

		private string m_errorCount = string.Empty;

		private Vector2 m_atlasPosition;

		private int m_selectedDeviceIndex;

		private bool m_showDeviceSelectionBox;

		private Vector2 m_selectedDeviceScrollPosition;

		private string[] m_deviceIds = new string[23]
		{
			"iPad2,*", "iPad3,1_*", "iPad3,2_*", "iPad3,4_*", "iPad3,5_*", "iPad3,6_*", "iPad4,*", "iPad5,*", "iPod5,*", "iPhone7,*",
			"iPad3,3_*", "iPhone4,*", "iPhone5,*", "iPhone6,*", "iPhone8,*", "Intel\\(R\\) Core*", "Auto_Quality_1", "Auto_Quality_2", "Auto_Quality_3", "Auto_Quality_4",
			"Auto_Quality_5", "samsung SM-N910G", "samsung GT-N8010"
		};

		public override string GetName()
		{
			return "System";
		}

		public override void OnApply()
		{
		}

		public override void OnMenu()
		{
			GUILayout.BeginVertical(GUI.skin.box);
			OnMenuToggleShowFrameTimeGraphAnnotations();
			OnMenuToggleShowFrameTimeGraph();
			OnMenuToggleGCStats();
			OnMenuToggleFreeMemoryPages();
			OnMenuToggleFreeMemoryBytes();
			OnMenuToggleUsedMemoryBytes();
			OnMenuToggleCPUStats();
			OnMenuToggleUIAlignment();
			OnMenuHideFleeButton();
			OnMenuForceShowSkillWheel();
			OnMenuToggleShowShopOptions();
			OnMenuToggleUseTVOpenURL();
			OnMenuForceLodForMap();
			OnMenuForceException();
			OnNetworkInfo();
			OnScrollview();
			OnAtlasInfo();
			OnLabelInfo();
			OnShowCameraDebugInfo();
			OnQualitySettings();
			GUILayout.EndVertical();
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(GUI.skin.box);
			if (BBCollectionsTracker.Enabled && GUILayout.Button("Dump Collection tracking data"))
			{
				try
				{
					DateTime now = DateTime.Now;
					string path = Main.LoadSavedDataPath() + "/CollectionTracker_" + string.Format("{0:}{1:00}{2:00}_{3:00}{4:00}{5:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second) + ".log";
					using (StreamWriter writer = File.CreateText(path))
					{
						BBCollectionsTracker.Dump(writer, Assets.Scripts.Engine.Helpers.GetBuildType());
					}
				}
				catch (Exception)
				{
				}
			}
			if (GUILayout.Button("Clear LocalStorage"))
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.Clear();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(GUI.skin.box);
			GUI.enabled = Singleton<UIScreenManager>.Instance.CurrentModalScreen == null;
			if (GUILayout.Button("App Rating: Simulate mission"))
			{
				AppRatingPopup.LastMissionWasSuccess = true;
				AppRatingPopup.Show();
			}
			object obj = UbiCore.Utils.Singleton<LocalStorage>.Instance.GPGetValue(LocalStorage.GPKeyAppRatingMissions);
			string text = ((!(obj is long)) ? "not set" : ((long)obj).ToString());
			GUILayout.Label("MissionCounter: " + text);
			GUILayout.EndHorizontal();
			GUI.enabled = true;
		}

		private void OnScrollview()
		{
			GUIStyle style = GUI.skin.GetStyle("Label");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Scrollview Momentum", style, GUILayout.Width(300f));
			GUILayout.Space(10f);
			if (GUILayout.Button("-1", GUILayout.Width(80f)))
			{
				UIScrollView.s_momentumOffset -= 1f;
			}
			GUILayout.Label((UIDataMapper.GlobalScrollViewMomentum + UIScrollView.s_momentumOffset).ToString(), style, GUILayout.Width(100f));
			if (GUILayout.Button("+1", GUILayout.Width(80f)))
			{
				UIScrollView.s_momentumOffset += 1f;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Damp Strength", style, GUILayout.Width(300f));
			GUILayout.Space(10f);
			if (GUILayout.Button("-1", GUILayout.Width(80f)))
			{
				UIScrollView.s_dampstrength -= 1f;
			}
			GUILayout.Label(UIScrollView.s_dampstrength.ToString(), style, GUILayout.Width(100f));
			if (GUILayout.Button("+1", GUILayout.Width(80f)))
			{
				UIScrollView.s_dampstrength += 1f;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Drag end delay", style, GUILayout.Width(300f));
			GUILayout.Space(10f);
			if (GUILayout.Button("-0.01f", GUILayout.Width(80f)))
			{
				UIScrollView.s_delayPressedWhenDraging -= 0.01f;
			}
			GUILayout.Label(UIScrollView.s_delayPressedWhenDraging.ToString(), style, GUILayout.Width(100f));
			if (GUILayout.Button("+0.01f", GUILayout.Width(80f)))
			{
				UIScrollView.s_delayPressedWhenDraging += 0.01f;
			}
			GUILayout.EndHorizontal();
		}

		private void OnShowCameraDebugInfo()
		{
			GUILayout.Space(10f);
			GUILayout.BeginVertical("box", GUILayout.MinHeight(100f));
			GUILayout.Label("UI Camera Debug:");
			bool value = NGUIDebug.debugRaycast;
			Toggle(ref value, "Active");
			NGUIDebug.debugRaycast = value;
			GUILayout.EndVertical();
		}

		private void OnLabelInfo()
		{
			GUILayout.Space(10f);
			GUILayout.BeginVertical("box", GUILayout.MinHeight(100f));
			GUILayout.Label("Translation:");
			bool value = LocalizationManager.Instance.ShowStringId;
			Toggle(ref value, "Show string id");
			LocalizationManager.Instance.ShowStringId = value;
			GUILayout.Label("Label Info:");
			value = UILabel.ShowLabelInfo;
			Toggle(ref value, "Show Label Info");
			UILabel.ShowLabelInfo = value;
			GUILayout.EndVertical();
		}

		private void OnAtlasInfo()
		{
			GUILayout.Space(10f);
			GUILayout.BeginVertical("box", GUILayout.MinHeight(200f));
			GUILayout.Label("Loaded Atlases:");
			UIAtlas[] loadedAtlases = UIAtlasHelper.GetLoadedAtlases();
			m_atlasPosition = GUILayout.BeginScrollView(m_atlasPosition);
			UIAtlas[] array = loadedAtlases;
			foreach (UIAtlas uIAtlas in array)
			{
				if (uIAtlas.replacement != null)
				{
					GUILayout.Label(uIAtlas.replacement.GetDisplayName());
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}

		private void OnQualitySettings()
		{
			bool changed = GUI.changed;
			GUI.changed = false;
			string qualityDeviceProfileName = UbiCore.Utils.Singleton<LocalStorage>.Instance.QualityDeviceProfileName;
			bool flag = GUILayout.Toggle(!string.IsNullOrEmpty(qualityDeviceProfileName), "Activate Override Quality".ToString());
			if (flag)
			{
				GUILayout.BeginVertical("Box", GUILayout.MinWidth(300f));
				m_selectedDeviceScrollPosition = GUILayout.BeginScrollView(m_selectedDeviceScrollPosition, GUILayout.MinHeight(200f));
				m_selectedDeviceIndex = GUILayout.SelectionGrid(m_selectedDeviceIndex, m_deviceIds, 1, "toggle");
				GUILayout.EndScrollView();
				GUILayout.EndVertical();
			}
			if (GUI.changed)
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.QualityDeviceProfileName = ((!flag) ? string.Empty : m_deviceIds[m_selectedDeviceIndex]);
			}
			GUI.changed = changed;
		}

		public override void OnOpen()
		{
			if (m_DebugOutput == null)
			{
				GameObject gameObject = new GameObject("SystemDebugOutput");
				m_DebugOutput = gameObject.AddComponent<SystemDebugOutput>();
				EventDelegate.Add(Singleton<StateManager>.Instance.ProfileManager.onForceExceptionChange, OnForceExceptionChanged);
				OnForceExceptionChanged();
			}
		}

		public void OnDestroy()
		{
			EventDelegate.Remove(Singleton<StateManager>.Instance.ProfileManager.onForceExceptionChange, OnForceExceptionChanged);
		}

		private void OnMenuToggleShowFrameTimeGraph()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowFrameTimeGraph;
				Toggle(ref value, "Show Frame Time Graph");
				m_DebugOutput.ShowFrameTimeGraph = value;
			}
		}

		private void OnMenuToggleShowFrameTimeGraphAnnotations()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowFrameTimeGraphAnnotations;
				Toggle(ref value, "Show Frame Time Graph Annotations");
				m_DebugOutput.ShowFrameTimeGraphAnnotations = value;
			}
		}

		private void OnMenuToggleGCStats()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null)
			{
				bool value = m_DebugOutput.ShowGCStats;
				Toggle(ref value, "Show GC stats");
				m_DebugOutput.ShowGCStats = value;
			}
		}

		private void OnMenuToggleFreeMemoryPages()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null && Application.platform == RuntimePlatform.IPhonePlayer)
			{
				bool value = m_DebugOutput.ShowFreeMemoryPages;
				Toggle(ref value, "Show Free Memory Pages");
				m_DebugOutput.ShowFreeMemoryPages = value;
			}
		}

		private void OnMenuToggleFreeMemoryBytes()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null && Application.platform == RuntimePlatform.IPhonePlayer)
			{
				bool value = m_DebugOutput.ShowFreeMemoryBytes;
				Toggle(ref value, "Show Free Memory Bytes");
				m_DebugOutput.ShowFreeMemoryBytes = value;
			}
		}

		private void OnMenuToggleUsedMemoryBytes()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null && Application.platform == RuntimePlatform.IPhonePlayer)
			{
				bool value = m_DebugOutput.ShowUsedMemoryBytes;
				Toggle(ref value, "Show Used Memory Bytes");
				m_DebugOutput.ShowUsedMemoryBytes = value;
			}
		}

		private void OnMenuToggleCPUStats()
		{
			GUI.enabled = true;
			if (m_DebugOutput != null && Application.platform == RuntimePlatform.IPhonePlayer)
			{
				bool value = m_DebugOutput.ShowCPUStats;
				Toggle(ref value, "Show CPU stats");
				m_DebugOutput.ShowCPUStats = value;
			}
		}

		private void OnMenuToggleUIAlignment()
		{
			GUI.enabled = true;
		}

		private void OnMenuHideFleeButton()
		{
			GUI.enabled = true;
			bool value = UiMain.UIHideFleeOption;
			Toggle(ref value, "Hide Fleebutton in combat");
			UiMain.UIHideFleeOption = value;
		}

		private void OnMenuForceShowSkillWheel()
		{
			GUI.enabled = true;
			bool value = UiMain.UIForceShowSkillWheelInAnimus;
			Toggle(ref value, "Force show skillwheel in animus");
			UiMain.UIForceShowSkillWheelInAnimus = value;
		}

		private void OnMenuToggleShowShopOptions()
		{
			GUI.enabled = true;
		}

		private void OnMenuToggleUseTVOpenURL()
		{
			GUI.enabled = true;
		}

		private void OnMenuForceLodForMap()
		{
			GUILayout.BeginHorizontal("box");
			GUILayout.Label("Force LOD");
			int num = -2;
			if (GUILayout.Button("Disable"))
			{
				num = -1;
			}
			if (GUILayout.Button("Lod 0"))
			{
				num = 0;
			}
			if (GUILayout.Button("Lod 1"))
			{
				num = 1;
			}
			if (num != -2)
			{
				GameObject[] array = new GameObject[2]
				{
					GameObject.Find("EnvScenario"),
					GameObject.Find("EnvStatic")
				};
				GameObject[] array2 = array;
				foreach (GameObject gameObject in array2)
				{
					if (gameObject != null)
					{
						LODGroup[] componentsInChildren = gameObject.GetComponentsInChildren<LODGroup>();
						LODGroup[] array3 = componentsInChildren;
						foreach (LODGroup lODGroup in array3)
						{
							lODGroup.ForceLOD(num);
						}
					}
				}
			}
			GUILayout.EndHorizontal();
		}

		private void OnNetworkInfo()
		{
			GUI.enabled = true;
			GUILayout.BeginVertical("Box");
			GUILayout.Label("Network Information");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Session Token:");
			GUILayout.Label(Singleton<StateManager>.Instance.ProfileManager.SessionToken);
			if (GUILayout.Button("Copy", GUILayout.MaxWidth(150f)))
			{
				ToClipBoard(Singleton<StateManager>.Instance.ProfileManager.SessionToken);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Request Token:");
			GUILayout.Label(Singleton<StateManager>.Instance.ProfileManager.RequestToken);
			if (GUILayout.Button("Copy", GUILayout.MaxWidth(150f)))
			{
				ToClipBoard(Singleton<StateManager>.Instance.ProfileManager.RequestToken);
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private void OnMenuForceException()
		{
			GUI.enabled = true;
			GUILayout.BeginVertical("Box");
			GUILayout.Label("Simulate request errors (for the next x requests)");
			GUILayout.BeginHorizontal();
			m_showExceptionCatSelectionBox = GUILayout.Toggle(m_showExceptionCatSelectionBox, ((!m_showExceptionCatSelectionBox) ? ">" : "^") + " Error Category (" + ErrorCategoryNames[m_selectedExceptionCatIndex] + ")", "button", GUILayout.Width(300f));
			List<string> list = ErrorCodeNames.ToList();
			list.Insert(0, "None");
			m_showExceptionSelectionBox = GUILayout.Toggle(m_showExceptionSelectionBox, ((!m_showExceptionSelectionBox) ? ">" : "^") + " Error Code (" + list[m_selectedExceptionIndex] + ")", "button", GUILayout.Width(300f));
			GUILayout.Label("count");
			string text = GUILayout.TextField(m_errorCount);
			int result;
			if (text.Length == 0)
			{
				m_errorCount = string.Empty;
			}
			else if (int.TryParse(text, out result))
			{
				m_errorCount = text;
			}
			m_forceDuringSync = GUILayout.Toggle(m_forceDuringSync, "force during sync.");
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (m_showExceptionCatSelectionBox)
			{
				DrawExceptionSelector(false);
			}
			if (m_showExceptionSelectionBox)
			{
				if (!m_showExceptionCatSelectionBox)
				{
					GUILayout.Space(300f);
				}
				DrawExceptionSelector(true);
			}
			GUILayout.EndHorizontal();
			bool flag = GUI.enabled;
			GUI.enabled = m_errorCount.Length > 0;
			if (GUILayout.Button("Set"))
			{
				int errorCode = ((m_selectedExceptionIndex != 0) ? ((int)Enum.Parse(typeof(ErrorCode), ErrorCodeNames[m_selectedExceptionIndex - 1])) : (-1));
				int errorCategory = (int)Enum.Parse(typeof(ErrorCategory), ErrorCategoryNames[m_selectedExceptionCatIndex]);
				int result2;
				if (int.TryParse(m_errorCount, out result2))
				{
					Singleton<StateManager>.Instance.ProfileManager.ForceException(errorCategory, errorCode, result2, m_forceDuringSync);
					OnForceExceptionChanged();
				}
			}
			GUI.enabled = flag;
			GUILayout.EndVertical();
		}

		private void DrawExceptionSelector(bool drawCodes)
		{
			GUILayout.BeginVertical("Box", GUILayout.MinWidth(300f));
			m_exceptionScrollPosition = GUILayout.BeginScrollView(m_exceptionScrollPosition, GUILayout.MinHeight(200f));
			if (drawCodes)
			{
				List<string> list = ErrorCodeNames.ToList();
				list.Insert(0, "None");
				string[] texts = list.ToArray();
				int selectedExceptionIndex = m_selectedExceptionIndex;
				m_selectedExceptionIndex = GUILayout.SelectionGrid(m_selectedExceptionIndex, texts, 1, "toggle");
				if (selectedExceptionIndex != m_selectedExceptionIndex)
				{
					m_showExceptionSelectionBox = false;
				}
			}
			else
			{
				int selectedExceptionCatIndex = m_selectedExceptionCatIndex;
				m_selectedExceptionCatIndex = GUILayout.SelectionGrid(m_selectedExceptionCatIndex, ErrorCategoryNames, 1, "toggle");
				if (selectedExceptionCatIndex != m_selectedExceptionCatIndex)
				{
					m_showExceptionCatSelectionBox = false;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}

		private void ToClipBoard(string _copy)
		{
		}

		private void OnForceExceptionChanged()
		{
			int forceErrorCode = Singleton<StateManager>.Instance.ProfileManager.ForceErrorCode;
			m_selectedExceptionIndex = ((forceErrorCode >= 0) ? (Array.IndexOf(ErrorCodeNames, ((ErrorCode)forceErrorCode).ToString()) + 1) : 0);
			m_errorCount = Singleton<StateManager>.Instance.ProfileManager.ForceErrorCount.ToString();
			m_forceDuringSync = Singleton<StateManager>.Instance.ProfileManager.ForceErrorDuringSync;
		}
	}
}
