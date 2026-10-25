using UbiCore.CameraController;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenu : MonoBehaviour
	{
		public GUISkin m_skin;

		public Rect m_windowRect = new Rect(50f, 100f, 700f, 400f);

		public TextAsset m_buildVersion;

		private static DebugMenu m_instance;

		private bool m_showMenu;

		private bool m_inputWasEnabled;

		private bool m_cameraWasEnabled;

		private int m_selected;

		private Vector2 m_vScrollView = Vector2.zero;

		private Vector2 m_hScrollView = Vector2.zero;

		private const float m_spacingPx = 5f;

		private float m_scaleForDpi = 1f;

		public float ScaleForDpi
		{
			get
			{
				return m_scaleForDpi;
			}
			set
			{
				m_scaleForDpi = value;
			}
		}

		public static bool IsVisible
		{
			get
			{
				if (m_instance != null)
				{
					return m_instance.m_showMenu;
				}
				return false;
			}
		}

		private void Awake()
		{
			float dpi = Screen.dpi;
			if (dpi > 0f)
			{
				m_scaleForDpi = dpi / 150f;
			}
			if (!m_instance)
			{
				m_instance = this;
			}
		}

		public static DebugMenu Create(Transform _parent)
		{
			GameObject obj = new GameObject("DebugMenu");
			obj.transform.parent = _parent;
			return obj.AddComponent<DebugMenu>();
		}

		public T Register<T>() where T : DebugMenuItem
		{
			return base.gameObject.AddComponent<T>();
		}

		public void OnGUI()
		{
			GUISkin skin = GUI.skin;
			GUI.skin = m_skin;
			float scaleForDpi = m_scaleForDpi;
			GUIUtility.ScaleAroundPivot(new Vector2(scaleForDpi, scaleForDpi), Vector2.zero);
			GUI.BeginGroup(new Rect(0f, 0f, Screen.width, Screen.height));
			if (m_showMenu)
			{
				m_windowRect = new Rect(5f / m_scaleForDpi, 5f / m_scaleForDpi, ((float)Screen.width - 10f) / m_scaleForDpi, ((float)Screen.height - 10f) / m_scaleForDpi);
				GUILayout.Window(typeof(DebugMenu).GetHashCode(), m_windowRect, OnMenu, "Debug Menu");
			}
			GUI.EndGroup();
			GUI.skin = skin;
		}

		public void OpenDebugMenu()
		{
			m_showMenu = true;
			m_inputWasEnabled = InputHandler.ToggleAllHandlers(false);
			m_cameraWasEnabled = UbiCore.CameraController.CameraController.ToggleCameraController(false);
			DebugMenuItem[] components = GetComponents<DebugMenuItem>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].OnOpen();
				components[i].DebugMenu = this;
			}
		}

		private void OnMenu(int windowID)
		{
			m_hScrollView = GUILayout.BeginScrollView(m_hScrollView);
			GUILayout.BeginHorizontal(GUI.skin.box);
			DebugMenuItem[] components = GetComponents<DebugMenuItem>();
			string[] array = new string[components.Length];
			for (int i = 0; i < components.Length; i++)
			{
				array[i] = components[i].GetName();
			}
			m_selected = GUILayout.Toolbar(m_selected, array, GUILayout.MinWidth(300f));
			GUILayout.EndHorizontal();
			m_vScrollView = GUILayout.BeginScrollView(m_vScrollView, GUI.skin.box);
			if (m_selected < 0 || m_selected >= components.Length)
			{
				m_selected = -1;
			}
			else
			{
				GUI.enabled = true;
				components[m_selected].OnMenu();
				GUI.enabled = true;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndScrollView();
			GUILayout.BeginHorizontal(GUI.skin.box);
			if (m_buildVersion != null)
			{
				GUILayout.Label("Build " + m_buildVersion.text);
			}
			GUILayout.FlexibleSpace();
			bool flag = GUILayout.Button("Ok", GUILayout.MinWidth(80f));
			bool flag2 = GUILayout.Button("Apply", GUILayout.MinWidth(80f));
			bool flag3 = GUILayout.Button("Cancel", GUILayout.MinWidth(80f));
			if (flag || flag2)
			{
				for (int j = 0; j < components.Length; j++)
				{
					components[j].OnApply();
				}
			}
			if (flag || flag3)
			{
				CloseDebugMenu();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
		}

		public void CloseDebugMenu()
		{
			m_showMenu = false;
			InputHandler.ToggleAllHandlers(m_inputWasEnabled);
			UbiCore.CameraController.CameraController.ToggleCameraController(m_cameraWasEnabled);
		}
	}
}
