using System.Collections.Generic;
using Assets.Scripts.Mission.Camera;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.AI;

public abstract class CameraStateScripted : CameraStateRaycast
{
	internal sealed class SortObject
	{
		public ScriptValues Values { get; private set; }

		public float Angle { get; private set; }

		public bool Hit { get; private set; }

		public SortObject(ScriptValues _values, float _angle, bool _hit)
		{
			Values = _values;
			Angle = _angle;
			Hit = _hit;
		}
	}

	[SerializeField]
	public ScriptValues[] m_valuesList;

	protected ScriptValues m_values;

	protected Quaternion m_orientation;

	protected Transform m_cameraBone;

	protected Transform m_lookAtBone;

	protected Vector3 m_agentPosition;

	protected Vector3 m_targetPosition;

	protected Vector3 m_cameraTargetPosition;

	protected Vector3 m_cameraDampedPosition;

	protected Vector3 m_cameraOffset;

	protected Vector3 m_lookAtTargetPosition;

	protected Vector3 m_lookAtDampedPosition;

	protected Vector3 m_lookAtOffset;

	protected float m_angle;

	protected float m_angularSpeed;

	protected Vector3 m_raycastStart;

	private GameObject m_cachedTarget;

	private GameObject cachedTarget
	{
		get
		{
			if (m_cachedTarget != null)
			{
				return m_cachedTarget;
			}
			m_cachedTarget = FindTarget();
			return m_cachedTarget;
		}
	}

	public override bool Outlines
	{
		get
		{
			return false;
		}
	}

	private void FindAndVerifyBone(bool useBone, ScriptValues.Reference reference, HumanBodyBones boneId, out Transform bone)
	{
		if (useBone)
		{
			bone = FindBone(reference, boneId);
			Verify(bone, reference, boneId);
		}
		else
		{
			bone = null;
		}
	}

	private Transform FindBone(ScriptValues.Reference reference, HumanBodyBones boneId)
	{
		GameObject obj;
		switch (reference)
		{
		case ScriptValues.Reference.PlayerRoot:
			return AcierCameraController.NavMeshAgent.transform;
		case ScriptValues.Reference.TargetRoot:
			return m_cachedTarget.transform;
		case ScriptValues.Reference.Player:
			obj = AcierCameraController.NavMeshAgent.gameObject;
			break;
		default:
			obj = m_cachedTarget;
			break;
		}
		GameObject gameObject = obj;
		return gameObject.GetComponent<Animator>().GetBoneTransform(boneId);
	}

	private void Verify(Transform bone, ScriptValues.Reference reference, HumanBodyBones boneId)
	{
		if (bone == null)
		{
			Debug.LogError(string.Concat("Camera state '", GetType(), "' Cannot find bone: ", boneId, " of ", reference));
		}
	}

	public override void StartState(ICameraState _previous)
	{
		base.StartState(_previous);
		FetchValues();
		Vector3 forward = m_agentPosition - m_targetPosition;
		forward.y = 0f;
		forward.Normalize();
		if (forward.sqrMagnitude > 1E-05f)
		{
			m_orientation = Quaternion.LookRotation(forward);
		}
		List<SortObject> list = new List<SortObject>();
		ScriptValues[] valuesList = m_valuesList;
		foreach (ScriptValues scriptValues in valuesList)
		{
			Transform bone;
			FindAndVerifyBone(scriptValues.useCameraBone, scriptValues.cameraReference, scriptValues.cameraBone, out bone);
			Transform bone2;
			FindAndVerifyBone(scriptValues.useLookAtBone, scriptValues.lookAtReference, scriptValues.lookAtBone, out bone2);
			if ((scriptValues.useCameraBone && bone == null) || (scriptValues.useLookAtBone && bone2 == null))
			{
				continue;
			}
			Vector3 vector = CalculateTargetPosition(scriptValues.cameraReference, scriptValues.cameraOffset, bone);
			Vector3 vector2 = CalculateTargetPosition(scriptValues.lookAtReference, scriptValues.lookAtOffset, bone2);
			Vector3 vector3 = vector - vector2;
			if (!(vector3.sqrMagnitude <= 0.001f))
			{
				Quaternion a = Quaternion.LookRotation(-vector3);
				Vector3 forward2 = ((_previous != null) ? base.transform.forward : AcierCameraController.NavMeshAgent.transform.forward);
				forward2.y = 0f;
				forward2.Normalize();
				if (!(forward2.sqrMagnitude <= 1E-05f))
				{
					float angle = Quaternion.Angle(a, Quaternion.LookRotation(forward2));
					bool hit = Physics.Raycast(vector2, vector3.normalized, vector3.magnitude, AcierCameraController.Instance.LayerMask);
					list.Add(new SortObject(scriptValues, angle, hit));
				}
			}
		}
		list.Sort(CompareValues);
		if (list.Count > 0)
		{
			m_values = list.ToArray()[0].Values;
		}
		else
		{
			m_values = new ScriptValues();
			Debug.LogError(string.Concat("There is no valid setup which could be used to animate the camera for state '", GetType(), "'"));
		}
		m_cameraBone = ((!m_values.useCameraBone) ? null : FindBone(m_values.cameraReference, m_values.cameraBone));
		m_lookAtBone = ((!m_values.useLookAtBone) ? null : FindBone(m_values.lookAtReference, m_values.lookAtBone));
		CalculateCameraTargetPosition();
		CalculateLookAtTargetPosition();
		m_cameraDampedPosition = m_cameraTargetPosition;
		m_lookAtDampedPosition = m_lookAtTargetPosition;
		m_raycastStart = m_lookAtTargetPosition;
		m_angularSpeed = m_values.angularStartSpeed;
	}

	private int CompareValues(SortObject _left, SortObject _right)
	{
		int num = _left.Hit.CompareTo(_right.Hit);
		if (num != 0)
		{
			return num;
		}
		return _left.Angle.CompareTo(_right.Angle);
	}

	public override void EndState()
	{
		m_cachedTarget = null;
	}

	protected abstract GameObject FindTarget();

	private void FetchValues()
	{
		m_agentPosition = AcierCameraController.NavMeshAgent.transform.position;
		m_targetPosition = ((!(cachedTarget != null) || !(cachedTarget.transform != null)) ? m_agentPosition : cachedTarget.transform.position);
	}

	protected Vector3 CalculateTargetPosition(ScriptValues.Reference reference, Vector3 offset, Transform bone)
	{
		Vector3 vector = ((!(bone != null)) ? ((reference != 0) ? m_targetPosition : m_agentPosition) : bone.position);
		return vector + m_orientation * offset;
	}

	protected void CalculateCameraTargetPosition()
	{
		m_cameraTargetPosition = CalculateTargetPosition(m_values.cameraReference, m_values.cameraOffset, m_cameraBone);
	}

	protected void DampCameraPosition()
	{
		m_cameraDampedPosition = Damping.Damp(m_cameraDampedPosition, m_cameraTargetPosition, m_values.cameraDamping, 1f);
	}

	protected void CalculateLookAtTargetPosition()
	{
		m_lookAtTargetPosition = CalculateTargetPosition(m_values.lookAtReference, m_values.lookAtOffset, m_lookAtBone);
	}

	protected void DampLookAtPosition()
	{
		m_lookAtDampedPosition = Damping.Damp(m_lookAtDampedPosition, m_lookAtTargetPosition, m_values.lookAtDamping, 1f);
	}

	protected override Vector3 CalculateRaycastStart()
	{
		return m_raycastStart;
	}

	public override void UpdateState()
	{
		FetchValues();
		CalculateCameraTargetPosition();
		DampCameraPosition();
		CalculateLookAtTargetPosition();
		DampLookAtPosition();
		m_cameraOffset += m_values.cameraVelocity * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		m_lookAtOffset += m_values.lookAtVelocity * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		Vector3 vector = m_cameraDampedPosition + m_orientation * m_cameraOffset;
		Vector3 vector2 = m_lookAtDampedPosition + m_orientation * m_lookAtOffset;
		Vector3 vector3 = vector - m_lookAtDampedPosition;
		m_angularSpeed = Damping.Damp(m_angularSpeed, m_values.angularTargetSpeed, m_values.angularDamping, 1f);
		m_angle += m_angularSpeed * UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		m_outputPosition = Quaternion.Euler(0f, m_angle, 0f) * vector3 + vector2;
		if ((vector2 - m_outputPosition).sqrMagnitude > 1E-05f)
		{
			m_outputRotation = Quaternion.LookRotation(vector2 - m_outputPosition);
		}
		if (CameraStateBase.Debugging)
		{
		}
		base.UpdateState();
	}
}
