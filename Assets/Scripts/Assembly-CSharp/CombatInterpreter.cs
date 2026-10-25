using Assets.Scripts.Mission.Logic.Combat;
using SharedBaseLib.System;
using UbiCore.Acier;
using UbiCore.Messenger;
using UnityEngine;

public sealed class CombatInterpreter : IconInterpreter
{
	public AnimationCurve SelectMappingCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public static float SelectMinDistance = 1f;

	public static float SelectMaxDistance = 1.5f;

	public static float MoveDistance = 0.5f;

	public static float ParryDelay = 0.05f;

	private static bool m_hasPadInput;

	public static bool AutoAttack;

	private bool m_hasInput;

	private float m_setParryTime = float.MaxValue;

	private bool m_setParryValue;

	private float m_inputEnabledParryClock;

	public float StrafingFactor { get; private set; }

	private bool StrafingThisFrame { get; set; }

	public override bool InputEnabled
	{
		get
		{
			return base.InputEnabled && CombatController.Instance.PlayerParticipant != null;
		}
		set
		{
			StrafingThisFrame = false;
			StrafingFactor = 0f;
			base.InputEnabled = value;
			m_hasInput = false;
			m_setParryTime = float.MaxValue;
			m_setParryValue = false;
			if (!value)
			{
				m_inputEnabledParryClock = 0f;
			}
		}
	}

	public CombatInterpreter()
	{
		InputPriority = 1f;
		StrafingThisFrame = false;
		StrafingFactor = 0f;
	}

	public override IconMaterial GetDefaultActionIcon()
	{
		IconMaterial defaultActionIcon = base.GetDefaultActionIcon();
		if (defaultActionIcon != 0)
		{
			return defaultActionIcon;
		}
		return IconMaterial.InteractAttack;
	}

	public void SetParry(bool value, float delay = 0f)
	{
		CombatController instance = CombatController.Instance;
		if (m_inputEnabledParryClock < 1f)
		{
			delay = 0f;
		}
		if (delay <= 0f)
		{
			instance.PlayerParticipant.EnableParry = value;
			m_setParryTime = float.MaxValue;
			m_setParryValue = value;
			return;
		}
		float num = LogicMain.CurrentTime + delay;
		if (m_setParryValue == value)
		{
			m_setParryTime = Mathf.Min(m_setParryTime, num);
			return;
		}
		m_setParryValue = value;
		m_setParryTime = num;
	}

	public override bool OnTriggerDefaultAction()
	{
		if (base.OnTriggerDefaultAction())
		{
			return true;
		}
		CombatController instance = CombatController.Instance;
		instance.PlayerParticipant.QueueAction(CombatParticipant.EActionQueuedType.Attack);
		SetParry(false, 0f);
		return true;
	}

	public override bool OnPrepareDefaultAction()
	{
		if (base.OnPrepareDefaultAction())
		{
			return true;
		}
		SetParry(true, ParryDelay);
		return true;
	}

	public override bool OnCancelDefaultAction()
	{
		if (base.OnCancelDefaultAction())
		{
			return true;
		}
		SetParry(false, 0f);
		return true;
	}

	public override bool OnTouch(TouchEvent _event)
	{
		if (base.OnTouch(_event))
		{
			return true;
		}
		CombatController instance = CombatController.Instance;
		instance.FindTargetAtScreenPosition(instance.PlayerParticipant, _event.Position);
		SetParry(true, ParryDelay);
		return true;
	}

	public override bool OnSecondTap(TapEvent _event)
	{
		return OnTap(_event);
	}

	public override bool OnTap(TapEvent _event)
	{
		if (base.OnTap(_event))
		{
			return true;
		}
		CombatController instance = CombatController.Instance;
		instance.PlayerParticipant.QueueAction(CombatParticipant.EActionQueuedType.Attack);
		SetParry(false, 0f);
		return true;
	}

	public override bool OnDrag(DragEvent _event)
	{
		StrafingThisFrame |= _event.Phase != InputEvent.InputPhase.Ended;
		m_hasInput = _event.Phase != InputEvent.InputPhase.Ended;
		if (base.OnDrag(_event))
		{
			return true;
		}
		CombatController instance = CombatController.Instance;
		Vector3 position = instance.PlayerParticipant.Position;
		Vector3 forward = Camera.main.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Quaternion quaternion = Quaternion.LookRotation(forward);
		Vector3 aim = quaternion * new Vector3(_event.StickInput.x, 0f, _event.StickInput.y);
		if (aim.magnitude > 0f)
		{
			bool flag = instance.FindTargetInDirection(instance.PlayerParticipant, aim);
			Vector3 targetPosition = position + aim.normalized * (instance.StrafeDistanceStart + MoveDistance);
			NavMeshHit _hit;
			if (NavigationHelper.SafeRaycast(position, targetPosition, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				targetPosition = _hit.position;
			}
			instance.PlayerParticipant.MoveToPosition(targetPosition);
			SetParry(false, 0f);
			if (flag && _event.Phase == InputEvent.InputPhase.Ended)
			{
				instance.PlayerParticipant.QueueAction(CombatParticipant.EActionQueuedType.Attack);
			}
			else
			{
				instance.PlayerParticipant.CancelAction();
			}
		}
		else
		{
			instance.PlayerParticipant.CancelAction();
			if (_event.Phase == InputEvent.InputPhase.Ended)
			{
				SetParry(false, 0f);
			}
			else
			{
				SetParry(true, ParryDelay);
			}
		}
		return true;
	}

	public override bool OnSelect(SelectEvent _event)
	{
		StrafingThisFrame |= _event.Phase != InputEvent.InputPhase.Ended;
		m_hasInput = _event.Phase != InputEvent.InputPhase.Ended;
		if (base.OnSelect(_event))
		{
			return true;
		}
		CombatController instance = CombatController.Instance;
		Vector3 position = instance.PlayerParticipant.Position;
		Ray ray = Camera.main.ScreenPointToRay(_event.Position);
		Vector3 vector = ray.origin + ray.direction * ((position.y - ray.origin.y) / (0f - Mathf.Abs(ray.direction.y)));
		Vector3 vector2 = vector - position;
		Vector3 normalized = vector2.normalized;
		float num = SelectMappingCurve.Evaluate(Mathf.Clamp01((vector2.magnitude - SelectMinDistance) / (SelectMaxDistance - SelectMinDistance)));
		if (num > 0f)
		{
			bool flag = instance.FindTargetInDirection(instance.PlayerParticipant, normalized);
			instance.PlayerParticipant.MoveToPosition(position + normalized.normalized * (instance.StrafeDistanceStart + MoveDistance));
			SetParry(false, 0f);
			if (flag && _event.Phase == InputEvent.InputPhase.Ended)
			{
				instance.PlayerParticipant.QueueAction(CombatParticipant.EActionQueuedType.Attack);
			}
			else
			{
				instance.PlayerParticipant.CancelAction();
			}
		}
		else
		{
			instance.PlayerParticipant.CancelAction();
			if (_event.Phase == InputEvent.InputPhase.Ended)
			{
				SetParry(false, 0f);
			}
			else
			{
				SetParry(true, ParryDelay);
			}
		}
		return true;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
		Vector2 direction = gamepad.Direction;
		CombatController instance = CombatController.Instance;
		Vector3 position = instance.PlayerParticipant.Position;
		Vector3 forward = Camera.main.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		Quaternion quaternion = Quaternion.LookRotation(forward);
		Vector3 aim = quaternion * new Vector3(direction.x, 0f, direction.y);
		if (aim.magnitude > 0f)
		{
			instance.FindTargetInDirection(instance.PlayerParticipant, aim);
			instance.PlayerParticipant.MoveToPosition(position + aim.normalized * (instance.StrafeDistanceStart + MoveDistance));
			StrafingThisFrame = true;
			m_hasPadInput = true;
			SetParry(false, 0f);
		}
		else if (m_hasPadInput)
		{
			instance.PlayerParticipant.CancelAction();
			m_hasPadInput = false;
			SetParry(true, ParryDelay);
		}
		if (!m_hasInput && !m_hasPadInput && AutoAttack)
		{
			instance.PlayerParticipant.QueueAction(CombatParticipant.EActionQueuedType.Attack);
		}
		UpdateStrafingFactor();
		StrafingThisFrame = false;
		if (AcierInputHandler.Instance.FingerCount == 0 && !gamepad.ActionHeld)
		{
			SetParry(false, 0f);
		}
		if (LogicMain.CurrentTime > m_setParryTime)
		{
			SetParry(m_setParryValue, 0f);
		}
		if (InputEnabled)
		{
			m_inputEnabledParryClock = Mathf.Min(m_inputEnabledParryClock + LogicMain.FrameTime, 1f);
		}
	}

	private void UpdateStrafingFactor()
	{
		StrafingFactor += ((!StrafingThisFrame) ? (-1.5f) : 1f) * (Time.deltaTime / 5f);
		StrafingFactor = Mathf.Clamp01(StrafingFactor);
	}
}
