using Assets.Scripts.Mission.Logic;
using UbiCore.Acier;
using UbiCore.Messenger;
using UnityEngine;
using UnityEngine.AI;

public sealed class ParcourInterpreter : IconInterpreter
{
	private Vector2 m_controllerDirection;

	private bool m_controllerMoving;

	private bool m_enabled;

	public override bool InputEnabled
	{
		get
		{
			return m_enabled;
		}
		set
		{
			m_enabled = value;
			if (m_enabled && !LogicMain.PlayerEntity.GetIsApproaching())
			{
				m_controllerMoving = true;
			}
		}
	}

	public ParcourInterpreter()
	{
		InputPriority = 1f;
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
		Messenger.Instance.Broadcast(new ParcourInputEvent(IconInterpreter.NavMeshAgent.transform.forward));
		return true;
	}

	public override IconMaterial GetDefaultActionIcon()
	{
		IconMaterial defaultActionIcon = base.GetDefaultActionIcon();
		if (defaultActionIcon != 0)
		{
			return defaultActionIcon;
		}
		return IconMaterial.Invalid;
	}

	public override bool OnTriggerDefaultAction()
	{
		if (base.OnTriggerDefaultAction())
		{
			return true;
		}
		return true;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
		m_controllerDirection = gamepad.Direction;
		if (m_controllerDirection.sqrMagnitude > 0f)
		{
			m_controllerMoving = true;
			Messenger.Instance.Broadcast(new ParcourInputEvent(GetClimbDirectionFromStickInput(m_controllerDirection)));
		}
		else if (m_controllerMoving)
		{
			m_controllerMoving = false;
			Messenger.Instance.Broadcast(new ParcourInputEvent(Vector3.zero));
		}
	}

	private Vector3 GetClimbDirectionFromStickInput(Vector2 _input)
	{
		Vector3 forward = Camera.main.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		return Quaternion.LookRotation(forward) * new Vector3(_input.x, 0f, _input.y).normalized;
	}

	public override bool OnDrag(DragEvent _event)
	{
		if (base.OnDrag(_event))
		{
			return true;
		}
		if (_event.Phase == InputEvent.InputPhase.Ended)
		{
			Messenger.Instance.Broadcast(new ParcourInputEvent(Vector3.zero));
		}
		else
		{
			Messenger.Instance.Broadcast(new ParcourInputEvent(GetClimbDirectionFromStickInput(_event.StickInput)));
		}
		return true;
	}

	public override bool OnSelect(SelectEvent _event)
	{
		if (base.OnSelect(_event))
		{
			return true;
		}
		NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
		Vector3 position = navMeshAgent.transform.position;
		Ray ray = Camera.main.ScreenPointToRay(_event.Position);
		Vector3 vector = ray.origin + ray.direction * ((position.y - ray.origin.y) / (0f - Mathf.Abs(ray.direction.y)));
		Vector3 normalized = (vector - position).normalized;
		if (_event.Phase == InputEvent.InputPhase.Ended)
		{
			Messenger.Instance.Broadcast(new ParcourInputEvent(Vector3.zero));
		}
		else
		{
			Messenger.Instance.Broadcast(new ParcourInputEvent(normalized));
		}
		return true;
	}
}
