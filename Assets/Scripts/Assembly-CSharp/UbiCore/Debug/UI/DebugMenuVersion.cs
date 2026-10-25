using AcierProtocol;
using Assets.Scripts;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Native;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuVersion : DebugMenuItem
	{
		private BuildDefinition m_buildDefinition;

		private ELanguages m_language;

		private string m_languageString;

		private GameObject m_uiIconcamera;

		private GameObject UIIconcamera
		{
			get
			{
				if (m_uiIconcamera == null)
				{
					m_uiIconcamera = GameObject.FindGameObjectWithTag("UiIconCamera");
				}
				return m_uiIconcamera;
			}
		}

		public override string GetName()
		{
			return "Version";
		}

		public void Initialize(BuildDefinition _buildDefinition)
		{
			m_buildDefinition = _buildDefinition;
			if (string.IsNullOrEmpty(m_languageString))
			{
				m_language = NativeTools.GetPreferredLanguage();
				m_languageString = NativeTools.Instance.GetPreferredLanguage();
			}
		}

		public override void OnMenu()
		{
			GUILayout.BeginHorizontal("Box");
			GUILayout.Label("NOTE: Select avatar & mission from the Mission tab!");
			GUILayout.EndHorizontal();
			if (m_buildDefinition != null)
			{
				GUILayout.BeginVertical();
				GUILayout.Label(m_buildDefinition.ToString(), GUILayout.Width(80f), GUILayout.ExpandWidth(true));
				GUILayout.EndVertical();
			}
			GUILayout.BeginVertical();
			if (!string.IsNullOrEmpty(QualitySettingsHelper.OriginalDeviceIdentifier))
			{
				GUILayout.Label("Original Device Id: " + QualitySettingsHelper.OriginalDeviceIdentifier, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			}
			if (Main.Instance.Startup.QualitySettings != null)
			{
				GUILayout.Label(Main.Instance.Startup.QualitySettings.ToString(QualitySettingsHelper.GetDeviceIdentifier()), GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			}
			else
			{
				GUILayout.Label("Unknown Device: " + QualitySettingsHelper.GetDeviceIdentifier(), GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			}
			GUILayout.Label("Graphics Device Version: " + SystemInfo.graphicsDeviceVersion, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			GUILayout.Label("Graphics Multi Threaded: " + SystemInfo.graphicsMultiThreaded, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			GUILayout.Label("NativeTools.GetPreferredLanguage() " + m_language, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			GUILayout.Label("NativeTools.Instance.GetPreferredLanguage() " + m_languageString, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			if (Main.Instance.Startup.Translation != null)
			{
				GUILayout.Label(Main.Instance.Startup.Translation.ToString(), GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			}
			else
			{
				GUILayout.Label("No TranslationDef selected", GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			string text = "UI: ";
			text = text + "\nResolution: " + Screen.currentResolution.ToString();
			text = text + "\nLayout Manager FamilyType: " + UbiCore.Utils.Singleton<UiMain>.Instance.LayoutManager.SelectedFamilyType;
			text = text + "\nLayout Manager DPI: " + UIUtil.GetLayoutDPI();
			text = text + "\nLayout Manager Inches: " + UIUtil.GetScreenInches();
			text = text + "\nLayout Scale Factor: " + UIUtil.CalculateScaleFactorFromDpi();
			GUILayout.Label(text, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			if (IconDisplay.ReadOnlyInstances != null)
			{
				GUILayout.BeginVertical();
				string text2 = "Icon Display: ";
				for (int i = 0; i < IconDisplay.ReadOnlyInstances.Count; i++)
				{
					if (IconDisplay.ReadOnlyInstances[i].gameObject.activeSelf)
					{
						string text3 = text2;
						text2 = text3 + "\nIconDisplay " + i + ": " + IconDisplay.ReadOnlyInstances[i].ToString();
					}
				}
				GUILayout.Label(text2, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
				if (UIIconcamera != null)
				{
					Camera component = UIIconcamera.GetComponent<Camera>();
					string text4 = "Icon camera: ";
					text4 = text4 + "\nPos: " + UIIconcamera.transform.position;
					text4 = text4 + "\nLocalScale: " + UIIconcamera.transform.localScale;
					text4 = text4 + "\nOrthographicSize: " + component.orthographicSize;
					text4 = text4 + "\nAspect: " + component.aspect;
					GUILayout.Label(text4, GUILayout.Width(80f), GUILayout.ExpandWidth(true));
				}
				GUILayout.EndVertical();
			}
			BBHashSet<GameObject> bBHashSet = new BBHashSet<GameObject>();
			FindStaticFlag(GameObject.Find("EnvStatic/ImportStatic"), bBHashSet);
			FindStaticFlag(GameObject.Find("EnvStatic/BakeStatic"), bBHashSet);
			GUILayout.Label("Has static batched gos: " + (bBHashSet.Count > 0));
			GUILayout.BeginVertical();
			StateManager instance = Singleton<StateManager>.Instance;
			if (instance != null && instance.ProfileManager != null)
			{
				ProfileManager profileManager = instance.ProfileManager;
				if (profileManager.Profile != null)
				{
					string text5 = "UserID: " + profileManager.Profile.id;
					text5 = text5 + "\nHashID: " + FriendUtil.ToHumanReadableUserHash(profileManager.Profile.userIdHash);
					text5 = text5 + "\nName: " + profileManager.Profile.userName;
					GUILayout.Label(text5, GUILayout.Width(200f), GUILayout.ExpandWidth(true));
				}
			}
			GUILayout.EndVertical();
		}

		public override void OnApply()
		{
		}

		public override void OnOpen()
		{
		}

		private static void FindStaticFlag(GameObject _go, BBHashSet<GameObject> _hashSet)
		{
			if (_go == null)
			{
				return;
			}
			foreach (Transform item in _go.transform)
			{
				if (item == null || item.gameObject == null)
				{
					continue;
				}
				Renderer[] componentsInChildren = item.GetComponentsInChildren<Renderer>();
				Renderer[] array = componentsInChildren;
				foreach (Renderer renderer in array)
				{
					if (renderer.isPartOfStaticBatch)
					{
						_hashSet.Add(item.gameObject);
					}
				}
				FindStaticFlag(item.gameObject, _hashSet);
			}
		}
	}
}
