using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.ScreenManager;
using SharedBaseLib.System;
using UbiCore.Acier;
using UbiCore.CameraController;
using UbiCore.Common;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class AcierInputHandler : InputHandler
{
	public const int MovementTouchIndex = 0;

	public const int LookTouchIndex = 1;

	public bool ShowParcourFeedback = true;

	public bool ShowPlayerSpeed;

	public bool EnableGamepadInAnimus = true;

	public bool EnableSecondTouchArea = true;

	public bool DisableSecondTouchAreaInCombat = true;

	public float SecondTouchMinX = 1f;

	public float SecondTouchMaxX = 0.2f;

	public float SecondTouchMinY;

	public float SecondTouchMaxY = -0.75f;

	public float InputSensitivity = 0.5f;

	public AnimationCurve InputSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(0.5f, 1f), new Keyframe(1f, 2f));

	public AnimationCurve DPISensitivityFactor = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	public bool EnableTouchFeedback = true;

	public bool EnableSelect;

	public float TapMaxDuration = 0.3f;

	public bool EnableStick = true;

	public AnimationCurve StickMappingCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public float StickDamping = 0.025f;

	public float StickMinDistance = 0.02f;

	public float StickMaxDistance = 0.035f;

	public bool DragStick;

	public bool EnableDoubleTap = true;

	public float DoubleTapMaxDelay = 0.2f;

	public float DoubleTapMaxDistance = 0.1f;

	private bool SimulateStylus;

	private static AcierInputHandler ms_instance;

	[SerializeField]
	private GameObject m_parcourFeedback;

	private List<IInterpreter> m_interpreters = new List<IInterpreter>();

	private IInterpreter m_activeInterpreter;

	private float m_lastTapTime = -1f;

	private TapEvent m_lastTapEvent;

	private bool m_cancelTouchNextUpdate;

	private bool m_moveToOutroTarget;

	private readonly TouchData[] m_touches = new TouchData[2]
	{
		new TouchData(),
		new TouchData()
	};

	public static AcierInputHandler Instance
	{
		get
		{
			return ms_instance;
		}
	}

	public UIGamepadHandler UIGamepadHandler { get; private set; }

	public GamepadWrapper Gamepad { get; private set; }

	public MoveInterpreter MoveInterpreter { get; private set; }

	public RooftopGardenInterpreter RooftopGardenInterpreter { get; private set; }

	public HaystackInterpreter HaystackInterpreter { get; private set; }

	public BenchInterpreter BenchInterpreter { get; private set; }

	public NpcInteractionInterpreter NpcInteractionInterpreter { get; private set; }

	public InteractWithObjectInterpreter InteractWithObjectInterpreter { get; private set; }

	public CombatInterpreter CombatInterpreter { get; private set; }

	public ParcourInterpreter ParcourInterpreter { get; private set; }

	[SerializeField]
	public IInterpreter ActiveInterpreter
	{
		get
		{
			return m_activeInterpreter;
		}
		private set
		{
			if (m_activeInterpreter != value)
			{
				if (m_activeInterpreter != null)
				{
					m_activeInterpreter.InputEnabled = false;
				}
				m_activeInterpreter = value;
				if (m_activeInterpreter != null)
				{
					m_activeInterpreter.InputEnabled = true;
				}
			}
		}
	}

	public bool IsInputEnabled
	{
		get
		{
			Camera main = Camera.main;
			if (main != null)
			{
				CameraController instance = AcierCameraController.Instance;
				if (instance == null || !instance.suppressesInteraction)
				{
					return base.isEnabled;
				}
			}
			return false;
		}
	}

	public int FingerCount { get; private set; }

	public int TouchDataCount
	{
		get
		{
			return m_touches.Length;
		}
	}

	public bool HasGamepadMoveInput
	{
		get
		{
			return Gamepad.Direction.sqrMagnitude > 0.0001f;
		}
	}

	public float PixelScaleFactor
	{
		get
		{
			float num = Mathf.Min(Screen.width, Screen.height);
			float dpi = Screen.dpi;
			float num2 = num;
			if (dpi > float.Epsilon)
			{
				float time = num / Screen.dpi;
				num2 /= Mathf.Max(0.01f, DPISensitivityFactor.Evaluate(time));
			}
			return num2;
		}
	}

	public float SensitivityFactor
	{
		get
		{
			return InputSensitivityCurve.Evaluate(InputSensitivity);
		}
	}

	public ParcourPrediction ParcourPrediction { get; private set; }

	public RoofEdgeDetection RoofEdgeDetection { get; private set; }

	public bool LeftHandedMode { get; set; }

	private void Awake()
	{
		ms_instance = this;
	}

	public TouchData GetTouchData(int _index)
	{
		return m_touches[_index];
	}

	public bool HasTouchInput(int _touchIndex, bool _checkMagnitude = true)
	{
		TouchData touchData = GetTouchData(_touchIndex);
		return touchData.state != 0 && (!_checkMagnitude || touchData.stickInput.sqrMagnitude > 0f);
	}

	public void RegisterInterpreter(IInterpreter _interpreter)
	{
		if (!m_interpreters.Contains(_interpreter))
		{
			m_interpreters.Add(_interpreter);
			m_interpreters.Sort((IInterpreter o, IInterpreter p) => -o.InputPriority.CompareTo(p.InputPriority));
		}
	}

	public void UnregisterInterpreter(IInterpreter _interpreter)
	{
		m_interpreters.Remove(_interpreter);
		m_interpreters.Sort((IInterpreter o, IInterpreter p) => -o.InputPriority.CompareTo(p.InputPriority));
	}

	public void DisableInput()
	{
		m_cancelTouchNextUpdate = true;
		ProcessTouchEvents();
		base.isEnabled = false;
	}

	public IconMaterial GetDefaultActionIcon()
	{
		for (int i = 0; i < m_interpreters.Count; i++)
		{
			IInterpreter interpreter = m_interpreters[i];
			if (interpreter.InputEnabled)
			{
				IconMaterial defaultActionIcon = interpreter.GetDefaultActionIcon();
				if (defaultActionIcon != 0)
				{
					return defaultActionIcon;
				}
			}
		}
		return IconMaterial.Invalid;
	}

	public bool TriggerDefaultAction()
	{
		for (int i = 0; i < m_interpreters.Count; i++)
		{
			IInterpreter interpreter = m_interpreters[i];
			if (interpreter.InputEnabled && interpreter.OnTriggerDefaultAction())
			{
				return true;
			}
		}
		return false;
	}

	public bool PrepareDefaultAction()
	{
		for (int i = 0; i < m_interpreters.Count; i++)
		{
			IInterpreter interpreter = m_interpreters[i];
			if (interpreter.InputEnabled && interpreter.OnPrepareDefaultAction())
			{
				return true;
			}
		}
		return false;
	}

	public bool CancelDefaultAction()
	{
		for (int i = 0; i < m_interpreters.Count; i++)
		{
			IInterpreter interpreter = m_interpreters[i];
			if (interpreter.InputEnabled && interpreter.OnCancelDefaultAction())
			{
				return true;
			}
		}
		return false;
	}

	public void StartOutro()
	{
		DisableInput();
		m_moveToOutroTarget = true;
		Messenger.Instance.Broadcast(new ParcourInputEvent(LogicMain.PlayerEntity.Rotation * Vector3.forward));
	}

	public void OnMissionLoaded()
	{
		UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<PlayerStateChangeEvent>(OnPlayerStateChange);
		MoveInterpreterDecals.Create();
		MoveInterpreter = new MoveInterpreter();
		RegisterInterpreter(MoveInterpreter);
		HaystackInterpreter = new HaystackInterpreter();
		RegisterInterpreter(HaystackInterpreter);
		RooftopGardenInterpreter = new RooftopGardenInterpreter();
		RegisterInterpreter(RooftopGardenInterpreter);
		BenchInterpreter = new BenchInterpreter();
		RegisterInterpreter(BenchInterpreter);
		NpcInteractionInterpreter = new NpcInteractionInterpreter();
		RegisterInterpreter(NpcInteractionInterpreter);
		InteractWithObjectInterpreter = new InteractWithObjectInterpreter();
		RegisterInterpreter(InteractWithObjectInterpreter);
		CombatInterpreter = new CombatInterpreter();
		RegisterInterpreter(CombatInterpreter);
		ParcourInterpreter = new ParcourInterpreter();
		RegisterInterpreter(ParcourInterpreter);
		ParcourPrediction = base.gameObject.AddComponentIfNotExist<ParcourPrediction>();
		ParcourPrediction.Reset();
		RoofEdgeDetection = base.gameObject.AddComponentIfNotExist<RoofEdgeDetection>();
		RoofEdgeDetection.Reset();
		ActiveInterpreter = MoveInterpreter;
	}

	public void OnMissionUnload()
	{
		UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<PlayerStateChangeEvent>(OnPlayerStateChange);
		m_moveToOutroTarget = false;
		ActiveInterpreter = null;
	}

	public void OnMissionUnloaded()
	{
		MoveInterpreter = null;
		RooftopGardenInterpreter = null;
		HaystackInterpreter = null;
		BenchInterpreter = null;
		NpcInteractionInterpreter = null;
		InteractWithObjectInterpreter = null;
		m_interpreters.Clear();
		MoveInterpreterDecals.Destroy();
	}

	private void Start()
	{
		m_touches[0].state = TouchData.ETouchState.None;
		m_touches[1].state = TouchData.ETouchState.None;
	}

	private void Update()
	{
		if (Main.Instance == null || Main.Instance.SceneLoader == null || Singleton<UIScreenManager>.Instance == null)
		{
			return;
		}
		if (m_moveToOutroTarget)
		{
			MoveToOutroTarget();
		}
		if (Gamepad == null)
		{
			Gamepad = base.gameObject.AddComponentIfNotExist<GamepadWrapper>();
		}
		if (UIGamepadHandler == null)
		{
			UIGamepadHandler uIGamepadHandler = Object.FindObjectOfType<UIGamepadHandler>();
			if (uIGamepadHandler == null)
			{
				GameObject gameObject = Object.Instantiate(UnityEngine.Resources.Load("UI/GamepadHandler/UIGamepadHandler") as GameObject);
				if (gameObject != null)
				{
					UIGamepadHandler = gameObject.GetComponent<UIGamepadHandler>();
					gameObject.transform.parent = base.transform;
				}
			}
			else
			{
				UIGamepadHandler = uIGamepadHandler;
			}
			UIGamepadHandler.m_gamepad = Gamepad;
		}
		if (UIGamepadHandler != null)
		{
			bool flag = false;
			SceneLoader sceneLoader = Main.Instance.SceneLoader;
			flag |= sceneLoader.CurrentMissionDescription != null && sceneLoader.CurrentMissionDescription.MissionType == EMissionType.Animus_EMissionType;
			flag |= Singleton<UIScreenManager>.Instance.CurrentScreen != null;
			flag |= Singleton<UIScreenManager>.Instance.CurrentModalScreen != null;
			flag |= UIDownloadScreen.IsLoadedAndFullyVisible;
			flag |= UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIMissionResult>().IsActive;
			flag &= EnableGamepadInAnimus;
			if (flag && !UIGamepadHandler.gameObject.activeSelf)
			{
				UIGamepadHandler.gameObject.SetActive(true);
			}
			if (!flag && UIGamepadHandler.gameObject.activeSelf)
			{
				UIGamepadHandler.gameObject.SetActive(false);
			}
		}
		if (ShowParcourFeedback)
		{
			if (m_parcourFeedback == null)
			{
				m_parcourFeedback = Object.Instantiate(UnityEngine.Resources.Load("Environment/Indicators/MoveInterpreter/ParcourFeedback/ParcourFeedback") as GameObject);
			}
		}
		else if (m_parcourFeedback != null)
		{
			Object.DestroyImmediate(m_parcourFeedback);
		}
		if (!IsInputEnabled)
		{
			return;
		}
		ProcessTouchEvents();
		for (int i = 0; i < m_interpreters.Count; i++)
		{
			IInterpreter interpreter = m_interpreters[i];
			if (interpreter.InputEnabled)
			{
				interpreter.OnUpdate();
			}
		}
	}

	private void OnPlayerStateChange(PlayerStateChangeEvent _event)
	{
		switch (_event.PlayerState)
		{
		case EPlayerState.Roaming:
		case EPlayerState.BeingChased:
			ActiveInterpreter = MoveInterpreter;
			break;
		case EPlayerState.Combat:
			ActiveInterpreter = CombatInterpreter;
			break;
		case EPlayerState.HidingInHaystack:
			ActiveInterpreter = HaystackInterpreter;
			break;
		case EPlayerState.HidingInRooftopGarden:
			ActiveInterpreter = RooftopGardenInterpreter;
			break;
		case EPlayerState.HidingOnBench:
			ActiveInterpreter = BenchInterpreter;
			break;
		case EPlayerState.HidingInPatrol:
			ActiveInterpreter = MoveInterpreter;
			break;
		case EPlayerState.NpcInteraction:
			ActiveInterpreter = NpcInteractionInterpreter;
			break;
		case EPlayerState.ObjectInteraction:
			ActiveInterpreter = InteractWithObjectInterpreter;
			break;
		case EPlayerState.Assassination:
			ActiveInterpreter = null;
			break;
		case EPlayerState.Parcour:
			ActiveInterpreter = ParcourInterpreter;
			break;
		case EPlayerState.Undefined:
			ActiveInterpreter = null;
			break;
		}
		Update();
	}

	private void MoveToOutroTarget()
	{
		LogicMain.PlayerEntity.SetSpeed(EMovementSpeed.WalkSlow);
		MoveInterpreter.MoveToTargetTapPosition(Outro.Instance.TargetPlayerPosition, Vector3.up, Outro.Instance.TargetPlayerForward, false);
		MoveInterpreterDecals.Instance.Enabled = false;
	}

	private void ProcessTouchEvents()
	{
		if (m_cancelTouchNextUpdate)
		{
			CancelAllTouches();
		}
		else
		{
			UpdateTouches();
		}
		for (int i = 0; i < 2; i++)
		{
			TouchData touchData = m_touches[i];
			EFingerId fingerId = (EFingerId)i;
			if (touchData.state != 0)
			{
				if (touchData.isOnUi)
				{
					HandleTouchUi(touchData, fingerId);
				}
				else
				{
					HandleTouchGame(touchData, fingerId);
				}
				if (touchData.phase == TouchPhase.Ended || touchData.phase == TouchPhase.Canceled)
				{
					touchData.state = TouchData.ETouchState.None;
				}
			}
		}
	}

	private void HandleTouchGame(TouchData touch, EFingerId fingerId)
	{
		UpdateStickInput(touch);
		switch (touch.state)
		{
		case TouchData.ETouchState.None:
			break;
		case TouchData.ETouchState.Touch:
			switch (touch.phase)
			{
			case TouchPhase.Began:
				DispatchTouch(new TouchEvent(touch.position, fingerId, FingerCount));
				break;
			case TouchPhase.Ended:
				DispatchTap(new TapEvent(touch.position, fingerId, FingerCount));
				break;
			default:
				if (EnableSelect && (UbiCore.Utils.Singleton<Timer>.Instance.realTime - touch.startTime > TapMaxDuration || (!EnableStick && touch.stickInput.SqrMagnitude() > 0f)))
				{
					touch.state = TouchData.ETouchState.Select;
					DispatchSelect(new SelectEvent(InputEvent.InputPhase.Started, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				}
				else if (EnableStick && (UbiCore.Utils.Singleton<Timer>.Instance.realTime - touch.startTime > TapMaxDuration || touch.stickInput.SqrMagnitude() > 0f))
				{
					touch.state = TouchData.ETouchState.Drag;
					DispatchDrag(new DragEvent(InputEvent.InputPhase.Started, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				}
				break;
			}
			break;
		case TouchData.ETouchState.Drag:
			switch (touch.phase)
			{
			case TouchPhase.Ended:
				DispatchDrag(new DragEvent(InputEvent.InputPhase.Ended, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				break;
			case TouchPhase.Canceled:
				DispatchDrag(new DragEvent(InputEvent.InputPhase.Ended, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				break;
			default:
				DispatchDrag(new DragEvent(InputEvent.InputPhase.Updated, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				break;
			}
			break;
		case TouchData.ETouchState.Select:
			switch (touch.phase)
			{
			case TouchPhase.Ended:
				DispatchSelect(new SelectEvent(InputEvent.InputPhase.Ended, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				break;
			case TouchPhase.Canceled:
				DispatchSelect(new SelectEvent(InputEvent.InputPhase.Ended, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				break;
			default:
				DispatchSelect(new SelectEvent(InputEvent.InputPhase.Updated, touch.delta, touch.position, touch.startPosition, touch.stickInput, fingerId, FingerCount));
				break;
			}
			break;
		}
	}

	private void HandleTouchUi(TouchData touch, EFingerId fingerId)
	{
		TouchData.ETouchState state = touch.state;
		if (state == TouchData.ETouchState.Touch)
		{
			switch (touch.phase)
			{
			case TouchPhase.Began:
				break;
			case TouchPhase.Ended:
				UbiCore.Utils.Singleton<UiMain>.Instance.Tap(new TapEvent(touch.position, fingerId, FingerCount));
				break;
			case TouchPhase.Moved:
			case TouchPhase.Stationary:
				break;
			}
		}
	}

	private void DispatchSelect(SelectEvent _event)
	{
		if (_event.FingerId == EFingerId.First)
		{
			for (int i = 0; i < m_interpreters.Count; i++)
			{
				IInterpreter interpreter = m_interpreters[i];
				if (interpreter.InputEnabled && interpreter.OnSelect(_event))
				{
					break;
				}
			}
			return;
		}
		for (int j = 0; j < m_interpreters.Count; j++)
		{
			IInterpreter interpreter2 = m_interpreters[j];
			if (interpreter2.InputEnabled && interpreter2.OnSecondSelect(_event))
			{
				break;
			}
		}
	}

	private void DispatchDrag(DragEvent _event)
	{
		if (_event.FingerId == EFingerId.First)
		{
			for (int i = 0; i < m_interpreters.Count; i++)
			{
				IInterpreter interpreter = m_interpreters[i];
				if (interpreter.InputEnabled && interpreter.OnDrag(_event))
				{
					break;
				}
			}
			return;
		}
		for (int j = 0; j < m_interpreters.Count; j++)
		{
			IInterpreter interpreter2 = m_interpreters[j];
			if (interpreter2.InputEnabled && interpreter2.OnSecondDrag(_event))
			{
				break;
			}
		}
	}

	private void DispatchTouch(TouchEvent _event)
	{
		if (_event.FingerId == EFingerId.First)
		{
			for (int i = 0; i < m_interpreters.Count; i++)
			{
				IInterpreter interpreter = m_interpreters[i];
				if (interpreter.InputEnabled && interpreter.OnTouch(_event))
				{
					break;
				}
			}
			return;
		}
		for (int j = 0; j < m_interpreters.Count; j++)
		{
			IInterpreter interpreter2 = m_interpreters[j];
			if (interpreter2.InputEnabled && interpreter2.OnSecondTouch(_event))
			{
				break;
			}
		}
	}

	private void DispatchTap(TapEvent _event)
	{
		if (_event.FingerId == EFingerId.First)
		{
			bool flag = false;
			if (m_lastTapEvent != null && EnableDoubleTap && ((_event.Position - m_lastTapEvent.Position) / PixelScaleFactor).magnitude < DoubleTapMaxDistance && UbiCore.Utils.Singleton<Timer>.Instance.realTime - m_lastTapTime < DoubleTapMaxDelay)
			{
				flag = true;
			}
			if (flag)
			{
				m_lastTapEvent = null;
				for (int i = 0; i < m_interpreters.Count; i++)
				{
					IInterpreter interpreter = m_interpreters[i];
					if (interpreter.InputEnabled && interpreter.OnDoubleTap(_event))
					{
						break;
					}
				}
				return;
			}
			m_lastTapEvent = _event;
			m_lastTapTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
			for (int j = 0; j < m_interpreters.Count; j++)
			{
				IInterpreter interpreter2 = m_interpreters[j];
				if (interpreter2.InputEnabled && interpreter2.OnTap(m_lastTapEvent))
				{
					break;
				}
			}
			return;
		}
		for (int k = 0; k < m_interpreters.Count; k++)
		{
			IInterpreter interpreter3 = m_interpreters[k];
			if (interpreter3.InputEnabled && interpreter3.OnSecondTap(_event))
			{
				break;
			}
		}
	}

	private bool IsOnUi(Vector2 _position)
	{
		Camera main = Camera.main;
		if (GUIUtility.hotControl != 0 || GUIUtility.hasModalWindow)
		{
			return true;
		}
		if (main != null && Physics.Raycast(main.ScreenPointToRay(_position), float.PositiveInfinity, Layers.NonInteractableUiLayerMask))
		{
			return true;
		}
		if (UbiCore.Utils.Singleton<UiMain>.Instance.IsOnUi(_position))
		{
			return true;
		}
		return false;
	}

	private void UpdateStickInput(TouchData _touch)
	{
		float pixelScaleFactor = PixelScaleFactor;
		Vector2 vector = (_touch.position - _touch.stickBasePosition) / pixelScaleFactor;
		float magnitude = vector.magnitude;
		Vector2 normalized = vector.normalized;
		float num = Mathf.Clamp01((magnitude - StickMinDistance) / (StickMaxDistance - StickMinDistance));
		if (DragStick && num >= 1f)
		{
			_touch.stickBasePosition = _touch.position - normalized * StickMaxDistance * pixelScaleFactor;
		}
		num = StickMappingCurve.Evaluate(num);
		Vector2 stickInput = _touch.stickInput;
		Vector2 vector2 = normalized * num;
		float num2 = Mathf.Min(StickDamping, num);
		_touch.stickInput = ((!(num2 <= 0f)) ? ((stickInput * num2 + vector2 * UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime) / (num2 + UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime)) : vector2);
	}

	private void UpdateTouches()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.PSP2)
		{
			UpdateMobileTouches();
		}
		else
		{
			UpdateSimulatedTouches();
		}
	}

	private void UpdateMobileTouches()
	{
		int touchCount = Input.touchCount;
		if (touchCount == 0)
		{
			UpdateStylusTouches();
			return;
		}
		FingerCount = touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = Input.touches[i];
			int num = 0;
			if (touch.phase == TouchPhase.Began)
			{
				if (EnableSecondTouchArea && IsInSecondTouchArea(touch.position) && (!DisableSecondTouchAreaInCombat || !CombatInterpreter.InputEnabled))
				{
					int num2 = 1;
					if (m_touches[num2].state != 0)
					{
						continue;
					}
					num = num2;
				}
				else if (m_touches[0].state == TouchData.ETouchState.None)
				{
					num = 0;
				}
				else
				{
					if (m_touches[1].state != 0)
					{
						continue;
					}
					num = 1;
				}
			}
			else if (m_touches[0].state != 0 && m_touches[0].fingerId == touch.fingerId)
			{
				num = 0;
			}
			else
			{
				if (m_touches[1].state == TouchData.ETouchState.None || m_touches[1].fingerId != touch.fingerId)
				{
					continue;
				}
				num = 1;
			}
			TouchData touchData = m_touches[num];
			if (touch.phase == TouchPhase.Began)
			{
				touchData.startPosition = touch.position;
				touchData.stickBasePosition = touchData.startPosition;
				touchData.startTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
				touchData.state = TouchData.ETouchState.Touch;
				touchData.stickInput = Vector2.zero;
				touchData.isOnUi = IsOnUi(touchData.startPosition);
				touchData.fingerId = touch.fingerId;
				touchData.position = touch.position;
				touchData.delta = Vector2.zero;
			}
			else
			{
				touchData.delta = touch.position - touchData.position;
				touchData.position = touch.position;
			}
			touchData.phase = touch.phase;
		}
		if (touchCount == 0)
		{
			CancelAllTouches();
		}
	}

	private void UpdateStylusTouches()
	{
		int num = 0;
		if (MouseButtonInUse(0))
		{
			if (EnableSecondTouchArea && IsInSecondTouchArea(Input.mousePosition) && (!DisableSecondTouchAreaInCombat || !CombatInterpreter.InputEnabled))
			{
				SimulateStylus = true;
				UpdateStandaloneTouch(1);
			}
			else
			{
				SimulateStylus = false;
				UpdateStandaloneTouch(0);
			}
			num = 1;
		}
		else
		{
			SimulateStylus = false;
			ReleaseStandaloneTouch(0);
			RestStandaloneTouch(1);
			num = 0;
		}
		FingerCount = num;
	}

	private bool IsInSecondTouchArea(Vector2 position)
	{
		float num = position.x / (float)Screen.width * 2f - 1f;
		float num2 = position.y / (float)Screen.height * 2f - 1f;
		float num3 = SecondTouchMinX;
		float num4 = SecondTouchMaxX;
		if (LeftHandedMode)
		{
			num3 *= -1f;
			num4 *= -1f;
		}
		float num5 = Mathf.Min(num3, num4);
		float num6 = Mathf.Max(num3, num4);
		float num7 = Mathf.Min(SecondTouchMinY, SecondTouchMaxY);
		float num8 = Mathf.Max(SecondTouchMinY, SecondTouchMaxY);
		return num >= num5 && num <= num6 && num2 >= num7 && num2 < num8;
	}

	private TouchPhase RateMouseButton(int _button)
	{
		if (Input.GetMouseButtonDown(_button))
		{
			return TouchPhase.Began;
		}
		if (Input.GetMouseButtonUp(_button))
		{
			return TouchPhase.Ended;
		}
		if (!Input.GetMouseButton(_button))
		{
			return TouchPhase.Canceled;
		}
		Vector2 vector = Input.mousePosition;
		if (m_touches[_button].position != vector)
		{
			return TouchPhase.Moved;
		}
		return TouchPhase.Stationary;
	}

	private bool MouseButtonInUse(int _button)
	{
		if (Input.GetMouseButtonDown(_button))
		{
			return true;
		}
		if (Input.GetMouseButtonUp(_button))
		{
			return true;
		}
		if (Input.GetMouseButton(_button))
		{
			return true;
		}
		return false;
	}

	private void UpdateSimulatedTouches()
	{
		int num = 0;
		if (MouseButtonInUse(1))
		{
			RestStandaloneTouch(0);
			UpdateStandaloneTouch(1);
			num = 2;
		}
		else if (MouseButtonInUse(0))
		{
			UpdateStandaloneTouch(0);
			num = 1;
		}
		else
		{
			ReleaseStandaloneTouch(0);
			ReleaseStandaloneTouch(1);
			num = 0;
		}
		FingerCount = num;
	}

	private void CancelAllTouches()
	{
		for (int i = 0; i < m_touches.Length; i++)
		{
			TouchData touchData = m_touches[i];
			if (touchData.state != 0)
			{
				touchData.phase = TouchPhase.Canceled;
			}
		}
		FingerCount = 0;
		m_cancelTouchNextUpdate = false;
	}

	private void ReleaseStandaloneTouch(int _button)
	{
		TouchData touchData = m_touches[_button];
		if (touchData.state != 0)
		{
			touchData.phase = TouchPhase.Canceled;
		}
	}

	private void RestStandaloneTouch(int _button)
	{
		TouchData touchData = m_touches[_button];
		touchData.phase = RateMouseButton(_button);
		if (touchData.phase == TouchPhase.Moved)
		{
			touchData.phase = TouchPhase.Stationary;
			touchData.stickBasePosition = touchData.startPosition;
			touchData.stickInput = Vector2.zero;
			touchData.delta = Vector2.zero;
		}
		else if (touchData.phase == TouchPhase.Began)
		{
			touchData.startPosition = Input.mousePosition;
			touchData.stickBasePosition = touchData.startPosition;
			touchData.startTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
			touchData.state = TouchData.ETouchState.Touch;
			touchData.stickInput = Vector2.zero;
			touchData.isOnUi = IsOnUi(touchData.startPosition);
		}
	}

	private void UpdateStandaloneTouch(int _button)
	{
		TouchData touchData = m_touches[_button];
		if (SimulateStylus)
		{
			touchData.phase = RateMouseButton(0);
		}
		else
		{
			touchData.phase = RateMouseButton(_button);
		}
		if (touchData.phase == TouchPhase.Stationary)
		{
			touchData.delta = Vector2.zero;
		}
		else if (touchData.phase == TouchPhase.Moved)
		{
			Vector2 vector = Input.mousePosition;
			touchData.delta = vector - m_touches[_button].position;
		}
		else if (touchData.phase == TouchPhase.Began)
		{
			touchData.startPosition = Input.mousePosition;
			touchData.stickBasePosition = touchData.startPosition;
			touchData.startTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
			touchData.state = TouchData.ETouchState.Touch;
			touchData.stickInput = Vector2.zero;
			touchData.isOnUi = IsOnUi(touchData.startPosition);
		}
		if (touchData.phase != TouchPhase.Canceled)
		{
			touchData.position = Input.mousePosition;
		}
	}
}
