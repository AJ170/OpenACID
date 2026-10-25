using System.Collections.Generic;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AnimationController))]
public class AnimatorIK : MonoBehaviour
{
	public class PinData
	{
		public AvatarIKGoal AvatarIKGoal;

		public float StartTime;

		public float EndTime;

		public float Weight;

		public bool Enabled;

		public Vector3 Point;
	}

	public delegate bool GetIKPositionDelegate(Vector3 _bonePosition, ref Vector3 _groundPosition, ref Vector3 _groundNormal);

	public static bool PlaceOnCollider = false;

	private static readonly float MaxScanRange = 0.5f;

	private static readonly float HeightLimitMax = 0.4f;

	private static readonly float JitterRadiusXZ = 0.02f;

	private static readonly float GroundPenetrationSmoothDistance = 0.025f;

	public static float PinIKAnticipateTime = 0.2f;

	public static float PinIKAnticipateDamping = 0.5f;

	public static float PinIKReleaseDamping = 0.25f;

	private static LayerMask m_colliderMask = Layers.StaticCollidersLayersMaskPreset;

	private Animator m_animator;

	private AnimationController m_animationController;

	private Ray m_ray = new Ray(Vector3.zero, Vector3.zero);

	private float m_averageIKWeight;

	private float[] m_lastIKWeight = new float[4];

	private Vector3[] m_lastFootIKPositions = new Vector3[2]
	{
		Vector3.zero,
		Vector3.zero
	};

	public GetIKPositionDelegate GetIKPositionOverride;

	public List<PinData> PinDataList = new List<PinData>();

	public bool EnableFootIK { get; set; }

	private void Start()
	{
		m_animator = GetComponent<Animator>();
		m_animationController = GetComponent<AnimationController>();
		EnableFootIK = true;
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (m_animationController == null || m_animator == null)
		{
			return;
		}
		if (EnableFootIK && CheckIfIKIsNeeded())
		{
			TryPinFoot(AnimatorParameters.IKFootL, AvatarIKGoal.LeftFoot);
			TryPinFoot(AnimatorParameters.IKFootR, AvatarIKGoal.RightFoot);
		}
		else
		{
			m_averageIKWeight = 0f;
		}
		for (int i = 0; i < PinDataList.Count; i++)
		{
			PinData pinData = PinDataList[i];
			if (LogicMain.CurrentTime > pinData.EndTime)
			{
				float pinIKReleaseDamping = PinIKReleaseDamping;
				float t = LogicMain.FrameTime / (LogicMain.FrameTime + pinIKReleaseDamping);
				float num = (pinData.Weight = Mathf.Lerp(pinData.Weight, 0f, t));
				m_animator.SetIKPositionWeight(pinData.AvatarIKGoal, num);
				if (num < 0.01f)
				{
					PinDataList.RemoveAt(i);
					i--;
				}
				continue;
			}
			if (LogicMain.CurrentTime >= pinData.StartTime)
			{
				float num2 = Mathf.Lerp(0.3f, 0.1f, Mathf.Clamp01((LogicMain.CurrentTime - pinData.StartTime) / 0.3f));
				float num3 = LogicMain.FrameTime / (LogicMain.FrameTime + num2);
				Vector3 iKPosition = m_animator.GetIKPosition(pinData.AvatarIKGoal);
				Vector3 vector = pinData.Point - iKPosition;
				Vector3 vector2 = vector * num3;
				m_animator.transform.position += vector2;
			}
			float num4 = pinData.StartTime - PinIKAnticipateTime;
			if (LogicMain.CurrentTime >= num4)
			{
				float num5 = Mathf.Lerp(PinIKAnticipateDamping, 0f, Mathf.Clamp01((LogicMain.CurrentTime - num4) / PinIKAnticipateTime));
				float t2 = LogicMain.FrameTime / (LogicMain.FrameTime + num5);
				m_animator.SetIKPosition(pinData.AvatarIKGoal, pinData.Point);
				float value = (pinData.Weight = Mathf.Lerp(pinData.Weight, 1f, t2));
				m_animator.SetIKPositionWeight(pinData.AvatarIKGoal, value);
			}
		}
	}

	private bool CheckIfIKIsNeeded()
	{
		if (Camera.main == null)
		{
			return false;
		}
		if ((m_animationController.PreviousPosition - Camera.main.transform.position).sqrMagnitude > 100f)
		{
			return false;
		}
		return true;
	}

	private void TryPinFoot(int _weightParameter, AvatarIKGoal _ikgoal)
	{
		Vector3 iKPosition = m_animator.GetIKPosition(_ikgoal);
		Vector3 _groundPosition = Vector3.zero;
		Vector3 _groundNormal = Vector3.zero;
		float @float = m_animator.GetFloat(_weightParameter);
		m_averageIKWeight = Mathf.Lerp(m_averageIKWeight, @float, LogicMain.FrameTime / (LogicMain.FrameTime + 0.1f));
		bool num;
		if (GetIKPositionOverride != null)
		{
			num = !GetIKPositionOverride(iKPosition, ref _groundPosition, ref _groundNormal);
		}
		else
		{
			if (PlaceOnCollider && !GetColliderGround(iKPosition, ref _groundPosition, ref _groundNormal))
			{
				goto IL_00a9;
			}
			if (PlaceOnCollider)
			{
				goto IL_00d9;
			}
			num = !GetNavMeshGround(iKPosition, ref _groundPosition, ref _groundNormal);
		}
		if (num)
		{
			goto IL_00a9;
		}
		goto IL_00d9;
		IL_00a9:
		m_lastIKWeight[(int)_ikgoal] = 0f;
		m_animator.SetIKPositionWeight(_ikgoal, 0f);
		m_animator.SetIKRotationWeight(_ikgoal, 0f);
		return;
		IL_00d9:
		int num2 = ((_ikgoal != 0) ? 1 : 0);
		if (m_lastFootIKPositions[num2].DistanceXZ(_groundPosition) < JitterRadiusXZ)
		{
			float y = (_groundPosition.y = Mathf.Min(m_lastFootIKPositions[num2].y, _groundPosition.y));
			m_lastFootIKPositions[num2].y = y;
		}
		else
		{
			m_lastFootIKPositions[num2] = _groundPosition;
		}
		PinFoot(@float, _ikgoal, iKPosition, _groundPosition, _groundNormal);
	}

	public void PinFoot(float _baseWeight, AvatarIKGoal _ikgoal, Vector3 _bonePosition, Vector3 _groundPosition, Vector3 _groundNormal)
	{
		float b = _baseWeight;
		if (m_averageIKWeight > 0.1f && _groundPosition.y > _bonePosition.y)
		{
			b = Mathf.Lerp(m_lastIKWeight[(int)_ikgoal], 1f, LogicMain.FrameTime / (LogicMain.FrameTime + 0.05f));
		}
		b = Mathf.Lerp(m_lastIKWeight[(int)_ikgoal], b, LogicMain.FrameTime / (LogicMain.FrameTime + 0.05f));
		m_lastIKWeight[(int)_ikgoal] = b;
		if (_groundPosition.y < _bonePosition.y)
		{
			float num = _bonePosition.y - _groundPosition.y;
			_groundPosition.y += Mathf.Clamp(_bonePosition.y - m_animator.transform.position.y, 0f, HeightLimitMax) * Mathf.Clamp01(num / GroundPenetrationSmoothDistance);
		}
		Vector3 rhs = Vector3.Cross(Vector3.forward, _groundNormal);
		Vector3 forward = Vector3.Cross(_groundNormal, rhs);
		forward.Normalize();
		Quaternion goalRotation = m_animator.GetIKRotation(_ikgoal) * Quaternion.LookRotation(forward, _groundNormal);
		m_animator.SetIKPositionWeight(_ikgoal, b);
		m_animator.SetIKRotationWeight(_ikgoal, b);
		m_animator.SetIKPosition(_ikgoal, _groundPosition);
		m_animator.SetIKRotation(_ikgoal, goalRotation);
	}

	private bool GetNavMeshGround(Vector3 _bonePosition, ref Vector3 _groundPosition, ref Vector3 _groundNormal)
	{
		NavMeshHit hit;
		if (!NavMesh.SamplePosition(_bonePosition, out hit, MaxScanRange, NavMeshLayers.WalkableLayerMaskPreset))
		{
			return false;
		}
		_groundPosition = hit.position;
		_groundNormal = Vector3.up;
		if (base.gameObject == LogicMain.Player)
		{
			float num = 0.01f;
			NavMeshHit _hit;
			NavigationHelper.SafeRaycast(_groundPosition, _groundPosition + base.transform.forward * num, out _hit, NavMeshLayers.WalkableLayerMaskPreset);
			if (_hit.distance < float.MaxValue)
			{
				_groundPosition = _hit.position - base.transform.forward * Mathf.Min(_hit.distance, num);
				float y = _groundPosition.y;
				_groundPosition = _bonePosition;
				_groundPosition.y = y;
				return true;
			}
		}
		Vector3 vector = _groundPosition - _bonePosition;
		vector.y = 0f;
		float num2 = vector.magnitude;
		if (num2 < 0.01f)
		{
			return true;
		}
		Vector3 sourcePosition = _bonePosition;
		sourcePosition.y = hit.position.y + num2;
		NavMeshHit hit2;
		if (NavMesh.SamplePosition(sourcePosition, out hit2, MaxScanRange, NavMeshLayers.WalkableLayerMaskPreset))
		{
			Vector3 vector2 = hit2.position - _bonePosition;
			vector2.y = 0f;
			float magnitude = vector2.magnitude;
			if (magnitude < num2)
			{
				_groundPosition = hit2.position;
				_groundNormal = Vector3.up;
				num2 = magnitude;
			}
		}
		Vector3 sourcePosition2 = _bonePosition;
		sourcePosition2.y = hit.position.y - num2;
		NavMeshHit hit3;
		if (NavMesh.SamplePosition(sourcePosition2, out hit3, MaxScanRange, NavMeshLayers.WalkableLayerMaskPreset))
		{
			Vector3 vector3 = hit3.position - _bonePosition;
			vector3.y = 0f;
			float magnitude2 = vector3.magnitude;
			if (magnitude2 < num2)
			{
				_groundPosition = hit3.position;
				_groundNormal = Vector3.up;
			}
		}
		float y2 = _groundPosition.y;
		_groundPosition = _bonePosition;
		_groundPosition.y = y2;
		if (Mathf.Abs(_groundPosition.y - _bonePosition.y) > HeightLimitMax)
		{
			return false;
		}
		return true;
	}

	private bool GetColliderGround(Vector3 _bonePosition, ref Vector3 _groundPosition, ref Vector3 _groundNormal)
	{
		Vector3 origin = _bonePosition;
		origin.y += MaxScanRange;
		m_ray.origin = origin;
		m_ray.direction = Vector3.down;
		RaycastHit[] array = Physics.RaycastAll(m_ray, 2f * MaxScanRange, m_colliderMask);
		if (array.Length == 0)
		{
			return false;
		}
		float num = float.MaxValue;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			float num2 = Mathf.Abs(array[i].point.y - _bonePosition.y);
			if (num2 < num)
			{
				num = num2;
				_groundPosition = array[i].point;
				_groundNormal = array[i].normal;
				flag = true;
			}
		}
		if (!flag || Mathf.Abs(_groundPosition.y - _bonePosition.y) > HeightLimitMax)
		{
			return false;
		}
		return true;
	}
}
