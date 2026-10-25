using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Ingame.Behaviours;
using UbiCore.Common;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

public class CameraEvents : MonoBehaviour, IInterpreter
{
	private const float CameraDragAreaHeight = 0.25f;

	private MoveToEvent m_moveToEvent;

	private DragEvent m_cameraDragEvent;

	private DragEvent m_moveDragEvent;

	private TapEvent m_secondTapEvent;

	public static bool FlipXAxis;

	public static bool FlipYAxis;

	public static bool EnableDragArea;

	public static bool EnableSecondFingerDrag = true;

	public static bool StopMovementWhileDragging = true;

	public static float StopMovementWhileDraggingTimeThreshold = 0.3f;

	public static float StopMovementWhileDraggingDistanceThreshold = 0.5f;

	public static float StopMovementRotateMultiplier = 0.9f;

	public static bool RealignOnSecondFingerTap;

	public static float PadXSensitivity = 500f;

	public static float PadYSensitivity = 500f;

	private Vector2 m_firstTouchPosition;

	protected float m_stopMovingMaxTime;

	private bool m_stopMovement;

	private bool m_wasMoving;

	private bool m_hasGamePadCameraInput;

	[SerializeField]
	public float TapLockDuraction = 0.3f;

	private bool m_waitForStickReset;

	public bool InputEnabled { get; set; }

	public virtual float InputPriority { get; set; }

	public float RotateSensitivity
	{
		get
		{
			return (!StopMovementWhileDragging || m_stopMovement) ? 1f : StopMovementRotateMultiplier;
		}
	}

	public MoveToEvent moveToEvent
	{
		get
		{
			return m_moveToEvent;
		}
	}

	public DragEvent cameraDragEvent
	{
		get
		{
			return m_cameraDragEvent;
		}
	}

	public DragEvent moveDragEvent
	{
		get
		{
			return m_moveDragEvent;
		}
	}

	public TapEvent secondTapEvent
	{
		get
		{
			return m_secondTapEvent;
		}
	}

	public bool isMoving
	{
		get
		{
			return (AcierCameraController.Instance.CurrentPlayerState == EPlayerState.Roaming || AcierCameraController.Instance.CurrentPlayerState == EPlayerState.BeingChased || AcierCameraController.Instance.CurrentPlayerState == EPlayerState.HidingInPatrol) && AcierCameraController.NavMeshAgent.hasPath;
		}
	}

	public bool wasMoving
	{
		get
		{
			return m_wasMoving;
		}
	}

	public bool startedMoving
	{
		get
		{
			return !wasMoving && isMoving;
		}
	}

	public bool stoppedMoving
	{
		get
		{
			return wasMoving && !isMoving;
		}
	}

	public bool isTapMoving
	{
		get
		{
			return LastMoveToEventDispatcherType == EDispatcherType.Tap && AcierCameraController.NavMeshAgent.hasPath;
		}
	}

	public bool isDragMoving
	{
		get
		{
			return (LastMoveToEventDispatcherType == EDispatcherType.Drag || LastMoveToEventDispatcherType == EDispatcherType.Gamepad) && m_moveDragEvent != null;
		}
	}

	public bool startedTapMoving
	{
		get
		{
			return LastMoveToEventDispatcherType == EDispatcherType.Tap && m_moveToEvent != null;
		}
	}

	public bool startedDragMoving
	{
		get
		{
			return LastMoveToEventDispatcherType == EDispatcherType.Drag && m_moveToEvent != null && m_moveDragEvent != null && m_moveDragEvent.Phase == InputEvent.InputPhase.Started;
		}
	}

	public bool startedDragCamera
	{
		get
		{
			return m_cameraDragEvent != null && m_cameraDragEvent.Phase == InputEvent.InputPhase.Started;
		}
	}

	public bool isRotating
	{
		get
		{
			if (m_cameraDragEvent == null)
			{
				return false;
			}
			if (m_hasGamePadCameraInput)
			{
				return Vector3.SqrMagnitude(m_cameraDragEvent.Delta) > 1E-05f;
			}
			return Vector2.SqrMagnitude(m_cameraDragEvent.Position - m_firstTouchPosition) > 1E-05f;
		}
	}

	public EDispatcherType LastMoveToEventDispatcherType { get; private set; }

	public CameraEvents()
	{
		InputEnabled = true;
		InputPriority = 2f;
	}

	private void OnEnable()
	{
		AcierInputHandler.Instance.RegisterInterpreter(this);
		Messenger.Instance.AddListener<MoveToEvent>(OnMoveTo);
	}

	private void OnDisable()
	{
		Messenger.Instance.RemoveListener<MoveToEvent>(OnMoveTo);
		AcierInputHandler.Instance.UnregisterInterpreter(this);
	}

	private void OnMoveTo(MoveToEvent _event)
	{
		if (base.enabled)
		{
			m_moveToEvent = _event;
			LastMoveToEventDispatcherType = _event.Dispatcher;
		}
	}

	public bool OnTouch(TouchEvent _event)
	{
		m_stopMovingMaxTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime + StopMovementWhileDraggingTimeThreshold;
		m_stopMovement = _event.FingerCount >= 2 && m_stopMovement;
		m_firstTouchPosition = _event.Position;
		return false;
	}

	public bool OnTap(TapEvent _event)
	{
		if (m_stopMovement)
		{
			return true;
		}
		return false;
	}

	public bool OnDoubleTap(TapEvent _event)
	{
		return false;
	}

	public bool OnDrag(DragEvent _event)
	{
		if (EnableDragArea && _event.StartPosition.y > (float)Screen.height * 0.75f)
		{
			m_cameraDragEvent = _event;
			return m_stopMovement;
		}
		if (EnableSecondFingerDrag && m_stopMovement)
		{
			return true;
		}
		m_moveDragEvent = _event;
		return false;
	}

	public bool OnSelect(SelectEvent _event)
	{
		if (EnableDragArea && _event.StartPosition.y > (float)Screen.height * 0.75f)
		{
			m_cameraDragEvent = _event;
			return m_stopMovement;
		}
		if (EnableSecondFingerDrag && m_stopMovement)
		{
			return true;
		}
		m_moveDragEvent = _event;
		return false;
	}

	public IconMaterial GetDefaultActionIcon()
	{
		return IconMaterial.Invalid;
	}

	public bool OnTriggerDefaultAction()
	{
		return false;
	}

	public bool OnPrepareDefaultAction()
	{
		return false;
	}

	public bool OnCancelDefaultAction()
	{
		return false;
	}

	public bool OnSecondTouch(TouchEvent _event)
	{
		Vector2 vector = (_event.Position - m_firstTouchPosition) / AcierInputHandler.Instance.PixelScaleFactor;
		m_stopMovement = StopMovementWhileDragging && UbiCore.Utils.Singleton<Timer>.Instance.realTime <= m_stopMovingMaxTime && vector.magnitude <= StopMovementWhileDraggingDistanceThreshold;
		return false;
	}

	public bool OnSecondTap(TapEvent _event)
	{
		if (!RealignOnSecondFingerTap)
		{
			return false;
		}
		m_secondTapEvent = _event;
		return true;
	}

	public bool OnSecondDrag(DragEvent _event)
	{
		if (EnableSecondFingerDrag)
		{
			m_cameraDragEvent = _event;
		}
		return true;
	}

	public bool OnSecondSelect(SelectEvent _event)
	{
		if (EnableSecondFingerDrag)
		{
			m_cameraDragEvent = _event;
		}
		return false;
	}

	public bool OnSwipe(SwipeEvent _event)
	{
		return false;
	}

	public bool OnTwist(TwistEvent _event)
	{
		return false;
	}

	public bool OnPinch(PinchEvent _event)
	{
		return false;
	}

	public void OnUpdate()
	{
		GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
		Vector2 camera = gamepad.Camera;
		Vector2 vector = new Vector2(camera.x * PadXSensitivity, (0f - camera.y) * PadYSensitivity);
		if (camera.sqrMagnitude <= float.Epsilon)
		{
			m_waitForStickReset = false;
		}
		UISkills subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UISkills>();
		UISkillsWheelBehaviour uISkillsWheelBehaviour = subSystemByType.Details as UISkillsWheelBehaviour;
		if (uISkillsWheelBehaviour != null && uISkillsWheelBehaviour.IsOpen)
		{
			m_waitForStickReset = true;
		}
		m_hasGamePadCameraInput = camera.sqrMagnitude > 0f && !m_waitForStickReset;
		if (m_hasGamePadCameraInput)
		{
			m_cameraDragEvent = new DragEvent(InputEvent.InputPhase.Started, vector * LogicMain.FrameTime, Vector2.zero, Vector2.zero, camera, EFingerId.Second, 0);
		}
		if (gamepad.CameraRealignDown)
		{
			m_secondTapEvent = new TapEvent(Vector2.zero, EFingerId.Second, 0);
		}
	}

	public void ClearEvents()
	{
		m_wasMoving = isMoving;
		m_cameraDragEvent = null;
		m_moveDragEvent = null;
		m_moveToEvent = null;
		m_secondTapEvent = null;
	}
}
