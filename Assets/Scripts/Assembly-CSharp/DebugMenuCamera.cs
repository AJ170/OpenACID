using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Mission.Camera;
using UbiCore.Debug.UI;
using UnityEngine;

public class DebugMenuCamera : DebugMenuItem
{
	private class DebuggingFrame
	{
		public Vector3 position;

		public Quaternion rotation;

		public Vector3 targetRaycastPosition;

		public Vector3 raycastPosition;

		public List<DebuggingSample> samples = new List<DebuggingSample>();
	}

	private class DebuggingSample
	{
		public Color color;

		public Vector3 position;

		public Quaternion rotation;
	}

	private class GameplayStateCache
	{
		public float timeScaleUnpaused;

		public GameObject gameplayCamera;
	}

	private AcierCameraController m_cameraController;

	private bool m_showCameraAnimationBindings;

	private Vector2 m_cameraAnimationBindingsPosition;

	private bool m_ShowCameraStack;

	private string m_CamPosStr = string.Empty;

	private string m_CamDirStr = string.Empty;

	private bool m_recording;

	private int m_debugPositionOrRotation;

	private bool m_frameOrSamples;

	private bool m_allOrOne;

	private bool m_moveCamera;

	private bool m_showRaycastStart;

	private int m_index;

	private List<DebuggingFrame> m_debuggingFrames = new List<DebuggingFrame>();

	private GameplayStateCache m_gameplayStateCache = new GameplayStateCache();

	private bool m_screenshotGameStateEnabled;

	private GameObject m_screenshotCamera;

	public override string GetName()
	{
		return "Camera";
	}

	public override void OnMenu()
	{
		if (m_cameraController == null)
		{
			m_cameraController = UnityEngine.Object.FindObjectOfType<AcierCameraController>();
			OnMenuNoController();
		}
		else
		{
			OnMenuEditController();
		}
		OnMenuToggleMissionInfoGUI();
		OnCustomCameraKeyMapper();
	}

	private void OnMenuNoController()
	{
		if (GUILayout.Button("Switch to New Camera System"))
		{
			m_cameraController = AcierCameraController.Create();
		}
	}

	private bool OnVector3Editor(string _label, ref string _inputPrinted, ref Vector3 _input)
	{
		Vector3 vector = default(Vector3);
		string[] array = _inputPrinted.Split(';', ' ');
		if (array.Length == 3)
		{
			vector.x = float.Parse(array[0]);
			vector.y = float.Parse(array[1]);
			vector.z = float.Parse(array[2]);
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label(_label, GUILayout.Width(300f));
		_inputPrinted = GUILayout.TextField(_inputPrinted, GUILayout.Width(300f));
		if (GUILayout.Button("Get", GUILayout.Width(50f)))
		{
			_inputPrinted = string.Format("{0}; {1}; {2}", _input.x, _input.y, _input.z);
		}
		bool result = false;
		GUILayout.EndHorizontal();
		return result;
	}

	private void OnMenuEditController()
	{
		Vector3 _input = AcierCameraController.Instance.LastCameraPosition;
		if (OnVector3Editor("Camera Position:", ref m_CamPosStr, ref _input))
		{
			AcierCameraController.Instance.Camera.gameObject.transform.position = _input;
		}
		Vector3 _input2 = AcierCameraController.Instance.LastCameraForward;
		if (OnVector3Editor("Camera Direction:", ref m_CamDirStr, ref _input2))
		{
			AcierCameraController.Instance.Camera.gameObject.transform.forward = _input2;
		}
		GUI.changed = false;
		AcierCameraController.Prototyping = GUILayout.Toggle(AcierCameraController.Prototyping, "Prototyping camera", "Button");
		if (GUI.changed)
		{
			AcierCameraController.Create();
		}
		if (AcierCameraController.Prototyping)
		{
			Toggle(ref CameraStateBase_Florent.FlipZoomDirection, "Flip zoom direction");
		}
		ZoomAndRotateValues zoomAndRotateValues = AcierCameraController.Instance.GlobalValues.m_zoomAndRotateValues;
		Slider("Rotation sensitivity (lower = faster)", zoomAndRotateValues.rotateDragDistance, out zoomAndRotateValues.rotateDragDistance, 0.01f, 2f);
		Slider("Zoom sensitivity (lower = faster)", zoomAndRotateValues.zoomDragDistance, out zoomAndRotateValues.zoomDragDistance, 0.01f, 2f);
		GUILayout.Label(string.Format("Active camera state: {0} ({1})", m_cameraController.CurrentStateType, m_cameraController.CurrentState));
		CameraStateOrbit cameraStateOrbit = m_cameraController.CurrentState as CameraStateOrbit;
		if (cameraStateOrbit != null)
		{
			GUILayout.BeginVertical("box");
			Toggle(ref CameraStateBase.Debugging, "Draw debugging");
			Toggle(ref CameraEvents.EnableDragArea, "Enable drag area");
			Toggle(ref CameraEvents.EnableSecondFingerDrag, "Enable second finger drag");
			Toggle(ref CameraEvents.RealignOnSecondFingerTap, "Realign On Second Finger Tap");
			Toggle(ref CameraEvents.StopMovementWhileDragging, "Stop movement while dragging");
			if (CameraEvents.StopMovementWhileDragging)
			{
				GUILayout.BeginVertical("Box");
				Slider("Max time between touches (s):", CameraEvents.StopMovementWhileDraggingTimeThreshold, out CameraEvents.StopMovementWhileDraggingTimeThreshold, 0f, 2f);
				Slider("Max distance between touches (screen height %):", CameraEvents.StopMovementWhileDraggingDistanceThreshold, out CameraEvents.StopMovementWhileDraggingDistanceThreshold, 0f, 2f);
				Slider("Rotation speed multiplier:", CameraEvents.StopMovementRotateMultiplier, out CameraEvents.StopMovementRotateMultiplier, 0f, 2f);
				GUILayout.EndHorizontal();
			}
			Slider("Pad X sensitivity", CameraEvents.PadXSensitivity, out CameraEvents.PadXSensitivity, 0f, 10000f);
			Slider("Pad Y sensitivity:", CameraEvents.PadYSensitivity, out CameraEvents.PadYSensitivity, 0f, 10000f);
			Slider("Auto realign multiplier", AcierCameraController.RealignSpeedMultiplier, out AcierCameraController.RealignSpeedMultiplier, 0f, 10f);
			Toggle(ref AcierCameraController.CanGoBehindObjects, "Can go behind objects");
			Toggle(ref AcierCameraController.CullCharacter, "Cull character when too close");
			Toggle(ref AcierCameraController.ShowCharacterOutline, "Show character outline");
			GUILayout.EndVertical();
		}
		CameraStateBase_Florent cameraStateBase_Florent = m_cameraController.CurrentState as CameraStateBase_Florent;
		if (cameraStateBase_Florent != null)
		{
			Toggle(ref CameraStateBase.Debugging, "Draw debugging");
			Toggle(ref CameraStateBase_Florent.GrabMoveInput, "Stop movement while dragging");
			CameraStateInfo(cameraStateBase_Florent, cameraStateBase_Florent.StateType.ToString());
		}
		OnMenuDataFilter();
	}

	private void Slider(string _title, float _in, out float _out, float _min, float _max)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(_title, GUILayout.Width(300f));
		float.TryParse(GUILayout.TextField(_in.ToString("F2"), 10, GUILayout.Width(50f)), out _out);
		_out = GUILayout.HorizontalSlider(_in, _min, _max);
		GUILayout.EndHorizontal();
	}

	private void OnMenuDataFilter()
	{
		float width = 300f;
		float width2 = 50f;
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Position damping:", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(AcierCameraController.AgentDataFilter.PositionDamping.ToString("F2"), 10, GUILayout.Width(width2)), out AcierCameraController.AgentDataFilter.PositionDamping);
		AcierCameraController.AgentDataFilter.PositionDamping = GUILayout.HorizontalSlider(AcierCameraController.AgentDataFilter.PositionDamping, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Position jitter radius:", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(AcierCameraController.AgentDataFilter.PositionJitterRadius.ToString("F2"), 10, GUILayout.Width(width2)), out AcierCameraController.AgentDataFilter.PositionJitterRadius);
		AcierCameraController.AgentDataFilter.PositionJitterRadius = GUILayout.HorizontalSlider(AcierCameraController.AgentDataFilter.PositionJitterRadius, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Rotation damping:", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(AcierCameraController.AgentDataFilter.RotationDamping.ToString("F2"), 10, GUILayout.Width(width2)), out AcierCameraController.AgentDataFilter.RotationDamping);
		AcierCameraController.AgentDataFilter.RotationDamping = GUILayout.HorizontalSlider(AcierCameraController.AgentDataFilter.RotationDamping, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Rotation jitter radius:", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(AcierCameraController.AgentDataFilter.RotationJitterAngle.ToString("F2"), 10, GUILayout.Width(width2)), out AcierCameraController.AgentDataFilter.RotationJitterAngle);
		AcierCameraController.AgentDataFilter.RotationJitterAngle = GUILayout.HorizontalSlider(AcierCameraController.AgentDataFilter.RotationJitterAngle, 0f, 90f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Velocity damping:", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(AcierCameraController.AgentDataFilter.VelocityDamping.ToString("F2"), 10, GUILayout.Width(width2)), out AcierCameraController.AgentDataFilter.VelocityDamping);
		AcierCameraController.AgentDataFilter.VelocityDamping = GUILayout.HorizontalSlider(AcierCameraController.AgentDataFilter.VelocityDamping, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	public override void OnOpen()
	{
	}

	public override void OnApply()
	{
	}

	private void OnMenuToggleMissionInfoGUI()
	{
		GUI.enabled = true;
		bool value = m_ShowCameraStack;
		Toggle(ref value, "Show Stack Info");
		m_ShowCameraStack = value;
	}

	private void OnCustomCameraKeyMapper()
	{
		GUILayout.Space(10f);
		GUILayout.BeginVertical("box");
		Toggle(ref m_showCameraAnimationBindings, "Show Custom Camera Animation:");
		if (m_showCameraAnimationBindings)
		{
			GUILayout.BeginVertical(GUILayout.MinHeight(200f));
			CameraCustomAnimationHelper cameraCustomAnimationHelper = Main.Instance.CameraCustomAnimationHelper;
			m_cameraAnimationBindingsPosition = GUILayout.BeginScrollView(m_cameraAnimationBindingsPosition);
			StringBuilder stringBuilder = new StringBuilder(string.Empty);
			for (int i = 0; i < cameraCustomAnimationHelper.KeyMapping.Length; i++)
			{
				AnimationClip animationByIndex = cameraCustomAnimationHelper.GetAnimationByIndex(i);
				if (animationByIndex == null)
				{
					break;
				}
				string arg = cameraCustomAnimationHelper.KeyMapping[i];
				stringBuilder.AppendFormat("{0} : {1}\n", arg, animationByIndex.name);
			}
			GUILayout.Label(stringBuilder.ToString());
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
		}
		GUILayout.EndVertical();
	}

	public void CameraStateInfo(CameraStateBase_Florent _state, string _label)
	{
		float width = 150f;
		float width2 = 50f;
		GUILayout.BeginVertical("box");
		GUILayout.Label(_label);
		GUILayout.Space(10f);
		GUILayout.Label("Leading");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Distance (m)", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.LeadDistance.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LeadDistance);
		_state.LeadDistance = GUILayout.HorizontalSlider(_state.LeadDistance, 0f, 10f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Damping", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.LeadDamping.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LeadDamping);
		_state.LeadDamping = GUILayout.HorizontalSlider(_state.LeadDamping, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Position");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Damping", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.PositionDamping.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionDamping);
		_state.PositionDamping = GUILayout.HorizontalSlider(_state.PositionDamping, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Normal", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.PositionOffset.x.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffset.x);
		float.TryParse(GUILayout.TextField(_state.PositionOffset.y.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffset.y);
		float.TryParse(GUILayout.TextField(_state.PositionOffset.z.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffset.z);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoomed out", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.PositionOffsetForward.x.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffsetForward.x);
		float.TryParse(GUILayout.TextField(_state.PositionOffsetForward.y.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffsetForward.y);
		float.TryParse(GUILayout.TextField(_state.PositionOffsetForward.z.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffsetForward.z);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoomed in", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.PositionOffsetBack.x.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffsetBack.x);
		float.TryParse(GUILayout.TextField(_state.PositionOffsetBack.y.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffsetBack.y);
		float.TryParse(GUILayout.TextField(_state.PositionOffsetBack.z.ToString("F2"), 10, GUILayout.Width(width2)), out _state.PositionOffsetBack.z);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("LookAt");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Damping:", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.LookAtDamping.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtDamping);
		_state.LookAtDamping = GUILayout.HorizontalSlider(_state.LookAtDamping, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Normal", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.LookAtOffset.x.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffset.x);
		float.TryParse(GUILayout.TextField(_state.LookAtOffset.y.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffset.y);
		float.TryParse(GUILayout.TextField(_state.LookAtOffset.z.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffset.z);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoomed out", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.LookAtOffsetForward.x.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffsetForward.x);
		float.TryParse(GUILayout.TextField(_state.LookAtOffsetForward.y.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffsetForward.y);
		float.TryParse(GUILayout.TextField(_state.LookAtOffsetForward.z.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffsetForward.z);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoomed in", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.LookAtOffsetBack.x.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffsetBack.x);
		float.TryParse(GUILayout.TextField(_state.LookAtOffsetBack.y.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffsetBack.y);
		float.TryParse(GUILayout.TextField(_state.LookAtOffsetBack.z.ToString("F2"), 10, GUILayout.Width(width2)), out _state.LookAtOffsetBack.z);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.Label("Fov");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Normal (deg)", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.Fov.ToString("F2"), 10, GUILayout.Width(width2)), out _state.Fov);
		_state.Fov = GUILayout.HorizontalSlider(_state.Fov, 0f, 180f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoomed out (deg)", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.FovForward.ToString("F2"), 10, GUILayout.Width(width2)), out _state.FovForward);
		_state.FovForward = GUILayout.HorizontalSlider(_state.FovForward, 0f, 180f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoomed in (deg)", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.FovBack.ToString("F2"), 10, GUILayout.Width(width2)), out _state.FovBack);
		_state.FovBack = GUILayout.HorizontalSlider(_state.FovBack, 0f, 180f);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		_state.AutoRealign = GUILayout.Toggle(_state.AutoRealign, "Align with player orientation");
		if (_state.AutoRealign)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Damping", GUILayout.Width(width));
			float.TryParse(GUILayout.TextField(_state.RealignDamping.ToString("F2"), 10, GUILayout.Width(width2)), out _state.RealignDamping);
			_state.RealignDamping = GUILayout.HorizontalSlider(_state.RealignDamping, 0f, 1f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Cutoff angle", GUILayout.Width(width));
			float.TryParse(GUILayout.TextField(_state.MaxAngleToRealign.ToString("F2"), 10, GUILayout.Width(width2)), out _state.MaxAngleToRealign);
			_state.MaxAngleToRealign = GUILayout.HorizontalSlider(_state.MaxAngleToRealign, 0f, 180f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Speed at angle 0", GUILayout.Width(width));
			float.TryParse(GUILayout.TextField(_state.MaxRealignSpeed.ToString("F2"), 10, GUILayout.Width(width2)), out _state.MaxRealignSpeed);
			_state.MaxRealignSpeed = GUILayout.HorizontalSlider(_state.MaxRealignSpeed, 0f, 1f);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Speed at cutoff angle", GUILayout.Width(width));
			float.TryParse(GUILayout.TextField(_state.MinRealignSpeed.ToString("F2"), 10, GUILayout.Width(width2)), out _state.MinRealignSpeed);
			_state.MinRealignSpeed = GUILayout.HorizontalSlider(_state.MinRealignSpeed, 0f, 1f);
			GUILayout.EndHorizontal();
		}
		_state.RealignIfLOSIsBlocked = GUILayout.Toggle(_state.RealignIfLOSIsBlocked, "Realign when line of sight is blocked");
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Zoom sensitivity", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.ZoomDragDistance.ToString("F2"), 10, GUILayout.Width(width2)), out _state.ZoomDragDistance);
		_state.ZoomDragDistance = GUILayout.HorizontalSlider(_state.ZoomDragDistance, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Rotate sensitivity", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.RotateDragDistance.ToString("F2"), 10, GUILayout.Width(width2)), out _state.RotateDragDistance);
		_state.RotateDragDistance = GUILayout.HorizontalSlider(_state.RotateDragDistance, 0f, 1f);
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Camera cut distance (m)", GUILayout.Width(width));
		float.TryParse(GUILayout.TextField(_state.MinDistanceToCut.ToString("F2"), 10, GUILayout.Width(width2)), out _state.MinDistanceToCut);
		_state.MinDistanceToCut = GUILayout.HorizontalSlider(_state.MinDistanceToCut, 0f, 50f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	public void OnGUI()
	{
		if (!m_ShowCameraStack || !(AcierCameraController.Instance != null))
		{
			return;
		}
		Rect position = new Rect(170f, 140f, Screen.width - 340, Screen.height - 280);
		List<AcierCameraController.StackedState> stack = AcierCameraController.Instance.Stack;
		string text = "Wanted State: " + CameraStateHelper.WantedState.ToString() + Environment.NewLine + "Recording: " + m_recording + " (F3) Clear (F4)" + Environment.NewLine + Environment.NewLine;
		if (m_debuggingFrames.Count > 0)
		{
			text = text + ((m_debugPositionOrRotation != 0) ? "Rotation" : "Position") + " (F5)" + Environment.NewLine;
			text = text + ((!m_frameOrSamples) ? "Output" : "Systems") + " (F6)" + Environment.NewLine;
			text = ((!m_allOrOne) ? (text + "All (F7)" + Environment.NewLine) : (text + "One (F7)" + Environment.NewLine));
			string text2 = text;
			text = text2 + "Camera: " + m_moveCamera + " (F8)" + Environment.NewLine;
			if (m_allOrOne || m_moveCamera)
			{
				text2 = text;
				text = text2 + Environment.NewLine + "Index: " + m_index + "/" + (m_debuggingFrames.Count - 1) + Environment.NewLine;
			}
			text2 = text;
			text = text2 + "Raycast: " + ((!m_showRaycastStart) ? "Off" : "On ") + " (F9)" + Environment.NewLine;
		}
		GUI.Label(position, text);
		position.y += 150f;
		foreach (AcierCameraController.StackedState item in stack)
		{
			string text3 = string.Concat("State: ", item.CameraState.StateType, " Requested: ", item.WantedWeight, " Weight: ", item.GrantedWeight, Environment.NewLine);
			GUI.contentColor = item.DebuggingColor;
			GUI.Label(position, text3);
			position.y += 16f;
		}
		GUI.contentColor = Color.white;
	}

	private void SwitchActiveCamera(GameObject _oldCam, GameObject _newCam)
	{
		_oldCam.gameObject.SetActive(false);
		_newCam.gameObject.SetActive(true);
		Rendering.RefreshPostEffects(_newCam.GetComponent<Camera>());
	}

	private void ToggleScreenshotGameState()
	{
		m_screenshotGameStateEnabled = !m_screenshotGameStateEnabled;
		if (m_screenshotGameStateEnabled)
		{
			m_gameplayStateCache.timeScaleUnpaused = LogicMain.GlobalTimeScale;
			LogicMain.GlobalTimeScale = 0f;
			m_gameplayStateCache.gameplayCamera = Camera.main.gameObject;
			if (m_screenshotCamera == null)
			{
				m_screenshotCamera = Main.Instance.CreateSharedCamera();
				m_screenshotCamera.name = "ScreenshotCamera";
			}
			m_screenshotCamera.transform.position = m_gameplayStateCache.gameplayCamera.transform.position;
			m_screenshotCamera.transform.rotation = m_gameplayStateCache.gameplayCamera.transform.rotation;
			m_screenshotCamera.GetComponent<Camera>().fieldOfView = m_gameplayStateCache.gameplayCamera.GetComponent<Camera>().fieldOfView;
			InputHandler.ToggleAllHandlers(false);
			SwitchActiveCamera(m_gameplayStateCache.gameplayCamera, m_screenshotCamera);
		}
		else
		{
			LogicMain.GlobalTimeScale = m_gameplayStateCache.timeScaleUnpaused;
			SwitchActiveCamera(m_screenshotCamera, m_gameplayStateCache.gameplayCamera);
			InputHandler.ToggleAllHandlers(true);
		}
	}

	private void LateUpdate()
	{
		if (AcierCameraController.Instance != null)
		{
			UpdateDebuggingUtilities();
		}
	}

	private void UpdateDebuggingUtilities()
	{
		AcierCameraController.Instance.DontRemoveNotContributing = m_recording || m_debuggingFrames.Count > 0;
		if (Input.GetKeyDown(KeyCode.F3))
		{
			m_recording = !m_recording;
			if (m_recording)
			{
				m_moveCamera = false;
				m_debuggingFrames.Clear();
				AcierCameraController.Instance.RemoveNonContributingStates();
			}
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			m_debuggingFrames.Clear();
			AcierCameraController.Instance.RemoveNonContributingStates();
		}
		if (Input.GetKeyDown(KeyCode.F5))
		{
			m_debugPositionOrRotation = ++m_debugPositionOrRotation % 2;
		}
		if (Input.GetKeyDown(KeyCode.F6))
		{
			m_frameOrSamples = !m_frameOrSamples;
		}
		if (Input.GetKeyDown(KeyCode.F7))
		{
			m_allOrOne = !m_allOrOne;
		}
		if (Input.GetKeyDown(KeyCode.F8))
		{
			m_moveCamera = !m_moveCamera;
		}
		if (Input.GetKeyDown(KeyCode.F9))
		{
			m_showRaycastStart = !m_showRaycastStart;
		}
		if (Input.GetKeyDown(KeyCode.F10))
		{
			ToggleScreenshotGameState();
		}
		if (m_recording)
		{
			DebuggingFrame debuggingFrame = new DebuggingFrame();
			debuggingFrame.position = AcierCameraController.Instance.transform.position;
			debuggingFrame.rotation = AcierCameraController.Instance.transform.rotation;
			debuggingFrame.targetRaycastPosition = AcierCameraController.Instance.RaycastPosition;
			debuggingFrame.raycastPosition = AcierCameraController.Instance.RaycastPosition;
			List<AcierCameraController.StackedState> stack = AcierCameraController.Instance.Stack;
			for (int i = 0; i < stack.Count; i++)
			{
				AcierCameraController.StackedState stackedState = stack[i];
				ICameraState cameraState = stackedState.CameraState;
				if (stackedState.GrantedWeight > 0f)
				{
					DebuggingSample debuggingSample = new DebuggingSample();
					debuggingSample.rotation = cameraState.CurrentRotation;
					debuggingSample.position = cameraState.CurrentPosition;
					debuggingSample.color = stackedState.DebuggingColor;
					debuggingFrame.samples.Add(debuggingSample);
				}
			}
			m_debuggingFrames.Add(debuggingFrame);
		}
		bool key = Input.GetKey(KeyCode.LeftControl);
		int num = ((!Input.GetKey(KeyCode.LeftShift)) ? 1 : 10);
		if ((!key && Input.GetKey(KeyCode.KeypadPlus)) || (key && Input.GetKeyDown(KeyCode.KeypadPlus)))
		{
			m_index += num;
		}
		if ((!key && Input.GetKey(KeyCode.KeypadMinus)) || (key && Input.GetKeyDown(KeyCode.KeypadMinus)))
		{
			m_index -= num;
		}
		for (int j = 0; j < m_debuggingFrames.Count; j++)
		{
			DebuggingFrame debuggingFrame2 = m_debuggingFrames[j];
			m_index = Mathf.Clamp(m_index, 0, m_debuggingFrames.Count - 1);
			if (m_moveCamera && j == m_index)
			{
				AcierCameraController.Instance.transform.position = debuggingFrame2.position;
				AcierCameraController.Instance.transform.rotation = debuggingFrame2.rotation;
			}
			if (m_allOrOne && j != m_index)
			{
				continue;
			}
			if (((!m_showRaycastStart || m_allOrOne) && j != m_index) || Physics.CheckSphere(debuggingFrame2.targetRaycastPosition, AcierCameraController.Instance.MinDistanceToWall, AcierCameraController.Instance.LayerMask))
			{
			}
			if (!m_frameOrSamples)
			{
				if (m_debugPositionOrRotation != 0)
				{
					DrawRotation(debuggingFrame2.position, debuggingFrame2.rotation, (!m_allOrOne && j != m_index) ? 0.05f : 0.5f);
				}
			}
			else if (j == m_index && debuggingFrame2.samples.Count > 1 && m_debugPositionOrRotation != 0)
			{
				DrawRotation(debuggingFrame2.position, debuggingFrame2.rotation, 0.5f);
			}
			for (int k = 0; k < debuggingFrame2.samples.Count; k++)
			{
				DebuggingSample debuggingSample2 = debuggingFrame2.samples[k];
				if (m_frameOrSamples)
				{
					if (m_debugPositionOrRotation != 0)
					{
						DrawRotation(debuggingSample2.position, debuggingSample2.rotation, (!m_allOrOne && j != m_index) ? 0.05f : 0.5f);
					}
				}
				else if (j == m_index && debuggingFrame2.samples.Count > 1 && m_debugPositionOrRotation != 0)
				{
					DrawRotation(debuggingSample2.position, debuggingSample2.rotation, 0.5f);
				}
			}
		}
	}

	private void DrawRotation(Vector3 _position, Quaternion _rotation, float _length)
	{
		Vector3 vector = _rotation * Vector3.right;
		Vector3 vector2 = _rotation * Vector3.up;
		Vector3 vector3 = _rotation * Vector3.forward;
		Debug.DrawLine(_position, _position + vector * _length, Color.red);
		Debug.DrawLine(_position, _position + vector2 * _length, Color.green);
		Debug.DrawLine(_position, _position + vector3 * _length, Color.blue);
	}
}
