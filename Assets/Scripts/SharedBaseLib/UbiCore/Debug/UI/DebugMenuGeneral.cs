using System;
using System.Threading;
using SharedBaseLib.Core;
using UbiCore.CameraController;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuGeneral : DebugMenuItem
	{
		public enum EExternalToggle
		{
			NetworkLogging = 0
		}

		public delegate void OnToggleHandler(bool _value);

		private class Module
		{
			public readonly string name;

			public readonly string[] defines;

			public Module(string _name, string[] _defines)
			{
				name = _name;
				defines = _defines;
			}
		}

		private readonly BBDictionary<int, BBList<OnToggleHandler>> m_ToggleListeners = new BBDictionary<int, BBList<OnToggleHandler>>();

		private bool m_networkLoggingEnabled;

		private bool m_consoleEnabled;

		private bool m_instableFramerateEnabled;

		private int m_instableFramerateProbability = 100;

		private int m_instableFramerateSleepTimeMin = 50;

		private int m_instableFramerateSleepTimeMax = 50;

		private readonly BBList<Module> m_modules = new BBList<Module>();

		private void Awake()
		{
			BBList<string> bBList = new BBList<string>();
			bBList.Add("UNITY_RELEASE");
			RegisterModule("AcierLibraries/SharedBaseLib", bBList.ToArray());
		}

		public void RegisterToggleListener(EExternalToggle _toggle, OnToggleHandler _handler)
		{
			if (!m_ToggleListeners.ContainsKey((int)_toggle))
			{
				m_ToggleListeners.Add((int)_toggle, new BBList<OnToggleHandler>());
			}
			m_ToggleListeners[(int)_toggle].Add(_handler);
		}

		public void UnregisterToggleListener(EExternalToggle _toggle, OnToggleHandler _handler)
		{
			m_ToggleListeners[(int)_toggle].Remove(_handler);
		}

		private void TriggerToggleListener(EExternalToggle _toggle, bool _value)
		{
			BBList<OnToggleHandler> value = null;
			if (!m_ToggleListeners.TryGetValue((int)_toggle, out value))
			{
				return;
			}
			foreach (OnToggleHandler item in value)
			{
				item(_value);
			}
		}

		public void RegisterModule(string _name, string[] _defines)
		{
			m_modules.Add(new Module(_name, _defines));
		}

		private void Start()
		{
			base.enabled = false;
		}

		public override string GetName()
		{
			return "General";
		}

		public override void OnMenu()
		{
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			OnMenuExitGame();
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			OnMenuToggleConsole();
			OnMenuPause();
			OnMenuFreeCamera();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			OnMenuExceptions();
			OnMenuInstableFramerate();
			OnMenuBuildInformation();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private void OnMenuExitGame()
		{
			if (GUILayout.Button("Exit Game"))
			{
				Application.Quit();
				m_debugMenu.CloseDebugMenu();
			}
		}

		private void OnMenuToggleConsole()
		{
			GUILayout.BeginVertical("box");
			GUI.enabled = true;
			Toggle(ref m_consoleEnabled, "Display Console");
			bool value = m_networkLoggingEnabled;
			Toggle(ref value, "Log Network Result");
			if (value != m_networkLoggingEnabled)
			{
				m_networkLoggingEnabled = value;
				TriggerToggleListener(EExternalToggle.NetworkLogging, m_networkLoggingEnabled);
			}
			GUILayout.EndVertical();
		}

		private static void OnMenuPause()
		{
			GUILayout.BeginVertical("box");
			bool flag = Time.timeScale != 1f;
			GUILayout.Label("Scene Update (Force delta times to zero)");
			if (GUILayout.Button(flag ? "Play" : "Pause"))
			{
				Time.timeScale = (flag ? 1f : 0.001f);
			}
			GUILayout.EndVertical();
		}

		private void OnMenuFreeCamera()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Free Camera");
			Camera main = Camera.main;
			UbiCore.CameraController.CameraController cameraController = ((main != null) ? main.GetComponent<UbiCore.CameraController.CameraController>() : null);
			GUILayout.Label("Current CameraController: " + ((cameraController != null) ? cameraController.GetType().ToString() : "none"));
			if (GUILayout.Button("Toggle"))
			{
				GameObject.FindGameObjectWithTag("Main").BroadcastMessage("OnToggleFreeCamera");
			}
			GUILayout.EndVertical();
		}

		private void OnMenuExceptions()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Exceptions");
			if (GUILayout.Button("Application Exception"))
			{
				throw new ApplicationException();
			}
			GUILayout.Button("Null Reference");
			if (GUILayout.Button("Float Divide By Zero"))
			{
				float num = 0f;
				float num3 = 3.14f / num;
			}
			if (GUILayout.Button("Integer Divide By Zero"))
			{
				int num2 = 0;
				int num4 = 42 / num2;
			}
			if (GUILayout.Button("Stack Overflow"))
			{
				OverflowStack(1, 2, 3);
			}
			GUILayout.EndVertical();
		}

		private void OnMenuInstableFramerate()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Instable Framerate");
			Toggle(ref m_instableFramerateEnabled, "Enabled");
			if (m_instableFramerateEnabled)
			{
				float value = m_instableFramerateProbability;
				GUILayout.Label("Probability (1:" + m_instableFramerateProbability + ")");
				m_instableFramerateProbability = Mathf.FloorToInt(GUILayout.HorizontalSlider(value, 1f, 400f));
				value = m_instableFramerateSleepTimeMin;
				GUILayout.Label("Sleep Time Min (" + m_instableFramerateSleepTimeMin + " ms)");
				m_instableFramerateSleepTimeMin = Mathf.FloorToInt(GUILayout.HorizontalSlider(value, 1f, 400f));
				value = m_instableFramerateSleepTimeMax;
				GUILayout.Label("Sleep Time Max (" + m_instableFramerateSleepTimeMax + " ms)");
				m_instableFramerateSleepTimeMax = Mathf.FloorToInt(GUILayout.HorizontalSlider(value, 1f, 400f));
			}
			base.enabled = m_instableFramerateEnabled;
			GUILayout.EndVertical();
		}

		private void OnMenuBuildInformation()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Build Information");
			GUILayout.Space(16f);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Label("Module Name");
			foreach (Module module in m_modules)
			{
				GUILayout.Label(module.name);
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			GUILayout.Label("Defines");
			foreach (Module module2 in m_modules)
			{
				string text = string.Empty;
				for (int i = 0; i < module2.defines.Length; i++)
				{
					string text2 = module2.defines[i];
					text = text + text2 + ((i == module2.defines.Length - 1) ? string.Empty : ";");
				}
				GUILayout.Label(text);
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private int OverflowStack(int a, int b, int c)
		{
			return OverflowStack(c, b, a) + OverflowStack(b, c, a + 1);
		}

		public override void OnOpen()
		{
		}

		public override void OnApply()
		{
		}

		private void Update()
		{
			if (UnityEngine.Random.Range(0, m_instableFramerateProbability - 1) == 0)
			{
				Thread.Sleep(UnityEngine.Random.Range(m_instableFramerateSleepTimeMin, m_instableFramerateSleepTimeMax));
			}
		}
	}
}
