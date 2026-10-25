using System;
using Assets.Scripts.Tracking;
using InControl;
using UnityEngine;

public class GamepadWrapper : MonoBehaviour
{
	public const float InputTimeout = 10f;

	public const float DeadZone = 0.19f;

	public const float DeadZoneSq = 0.0361f;

	private string m_previousLoadedLevel = string.Empty;

	private float m_inputClock;

	public bool HasLeftStick
	{
		get
		{
			return InputManager.ActiveDevice.HasControl(InputControlType.LeftStickUp) && InputManager.ActiveDevice.HasControl(InputControlType.LeftStickDown) && InputManager.ActiveDevice.HasControl(InputControlType.LeftStickRight) && InputManager.ActiveDevice.HasControl(InputControlType.LeftStickLeft);
		}
	}

	public bool HasInput { get; private set; }

	public bool HadInput
	{
		get
		{
			return m_inputClock > 0f;
		}
	}

	public bool HadInputInLevel { get; private set; }

	public Vector2 Direction { get; private set; }

	public Vector2 Select { get; private set; }

	public Vector2 SelectAnimus { get; private set; }

	public Vector2 Camera { get; private set; }

	public Vector2 SkillWheel { get; private set; }

	public bool AcceptHeld { get; private set; }

	public bool AcceptDown { get; private set; }

	public bool AcceptUp { get; private set; }

	public bool CloseDown { get; private set; }

	public bool CloseUp { get; private set; }

	public bool ScrollTabLeftDown { get; private set; }

	public bool ScrollTabRightDown { get; private set; }

	public bool SkillWheelHeld { get; private set; }

	public bool SkillWheelReleased { get; private set; }

	public bool ActionHeld { get; private set; }

	public bool FreeAimHeld { get; private set; }

	public bool FreeAimDown { get; private set; }

	public bool FreeAimUp { get; private set; }

	public bool PauseDown { get; private set; }

	public bool CameraRealignDown { get; private set; }

	public bool CameraRealignReleased { get; private set; }

	public bool CameraRealignHold { get; private set; }

	public bool ActionUp { get; private set; }

	public bool ActionDown { get; private set; }

	public bool SwitchProfileDown { get; private set; }

	public bool SwitchProfileUp { get; private set; }

	public bool ShowObjectivesUp { get; private set; }

	public event Action OnControllerConnected;

	protected void Start()
	{
		m_inputClock = 0f;
		InputManager.OnDeviceAttached += OnDeviceAttached;
	}

	protected void OnDestroy()
	{
		InputManager.OnDeviceAttached -= OnDeviceAttached;
	}

	private void OnDeviceAttached(InputDevice _obj)
	{
		if (this.OnControllerConnected != null)
		{
			this.OnControllerConnected();
		}
	}

	protected void Update()
	{
		HasInput = false;
		Vector2 vector = GetAxis2D(InputManager.ActiveDevice.LeftStick);
		bool hasInput = HasInput;
		Vector2 axis2D = GetAxis2D(InputManager.ActiveDevice.DPad);
		if (!HasLeftStick)
		{
			vector = axis2D;
		}
		Direction = ((!hasInput && CombatController.IsActive) ? axis2D : vector);
		Select = ((!hasInput) ? axis2D : vector);
		SelectAnimus = ((!hasInput) ? axis2D : vector);
		Camera = GetAxis2D(InputManager.ActiveDevice.RightStick);
		SkillWheel = GetAxis2D(InputManager.ActiveDevice.RightStick, true);
		AcceptHeld = GetButtonHeld(InputControlType.Action1);
		AcceptDown = GetButtonDown(InputControlType.Action1);
		AcceptUp = GetButtonUp(InputControlType.Action1);
		CloseDown = GetButtonDown(InputControlType.Action2);
		CloseUp = GetButtonUp(InputControlType.Action2);
		ScrollTabLeftDown = GetButtonDown(InputControlType.LeftTrigger);
		ScrollTabRightDown = GetButtonDown(InputControlType.RightTrigger);
		SkillWheelHeld = GetButtonHeld(InputControlType.RightBumper);
		SkillWheelReleased = GetButtonUp(InputControlType.RightBumper);
		ActionHeld = GetButtonHeld(InputControlType.Action1);
		ActionDown = GetButtonDown(InputControlType.Action1);
		ActionUp = GetButtonUp(InputControlType.Action1);
		FreeAimHeld = GetButtonHeld(InputControlType.Action3);
		FreeAimDown = GetButtonDown(InputControlType.Action3);
		FreeAimUp = GetButtonUp(InputControlType.Action3);
		PauseDown = GetButtonDown(InputControlType.Command);
		CameraRealignDown = GetButtonDown(InputControlType.LeftBumper);
		CameraRealignHold = GetButtonHeld(InputControlType.LeftBumper);
		CameraRealignReleased = GetButtonUp(InputControlType.LeftBumper);
		SwitchProfileDown = GetButtonDown(InputControlType.Action2);
		SwitchProfileUp = GetButtonUp(InputControlType.Action2);
		ShowObjectivesUp = GetButtonUp(InputControlType.RightTrigger);
		if (HasInput)
		{
			m_inputClock = 10f;
		}
		else
		{
			m_inputClock -= Time.deltaTime;
		}
		HadInputInLevel |= HasInput;
	}

	protected void OnLevelWasLoaded(int _level)
	{
		if (Application.loadedLevelName != "Empty")
		{
			m_previousLoadedLevel = Application.loadedLevelName;
			return;
		}
		if (HadInputInLevel && m_previousLoadedLevel != string.Empty)
		{
			if (m_previousLoadedLevel == "AnimusGlobe")
			{
				TrackingManager.AddGamePadTrackPoint("Animus");
			}
			else
			{
				TrackingManager.AddGamePadTrackPoint("Mission");
			}
		}
		m_inputClock = 0f;
		HadInputInLevel = false;
	}

	private Vector2 GetAxis2D(TwoAxisInputControl _inputControl, bool _invertY = false)
	{
		Vector2 result = new Vector2(_inputControl.X, _inputControl.Y * (float)((!_invertY) ? 1 : (-1)));
		HasInput |= result.sqrMagnitude > 0.0361f;
		return result;
	}

	private bool GetButtonDown(InputControlType _name)
	{
		bool wasPressed = InputManager.ActiveDevice.GetControl(_name).WasPressed;
		HasInput |= wasPressed;
		return wasPressed;
	}

	private bool GetButtonUp(InputControlType _name)
	{
		return InputManager.ActiveDevice.GetControl(_name).WasReleased;
	}

	private bool GetButtonHeld(InputControlType _name)
	{
		bool isPressed = InputManager.ActiveDevice.GetControl(_name).IsPressed;
		HasInput |= isPressed;
		return isPressed;
	}
}
