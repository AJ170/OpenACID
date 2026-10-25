using Assets.Scripts.Animation;
using SharedBaseLib.Core;
using UbiCore;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Crowd
{
	public class CrowdStopAtStation : MonoBehaviour
	{
		public static float CompletedCooldown = 20f;

		public static float RejectedCooldown = 5f;

		public static float CheckIntervalMin = 1f;

		public static float CheckIntervalMax = 5f;

		public static float SlowDownDamping = 0.5f;

		public static float RotateDamping = 0.3f;

		public static float AngleSensitivity = 10f;

		[HideInInspector]
		public CrowdBehavior CrowdBehavior;

		public Crowd.CrowdStationData StationData;

		public bool OnStation;

		public IdleType PlayingIdle = IdleType.Invalid;

		public float LeaveTime;

		public float MinEnterTime;

		public Vector3 TargetPosition = Vector3.zero;

		public void OnDrawGizmosSelected()
		{
			if (StationData == null)
			{
			}
		}

		public void Reset()
		{
			LeaveStation();
			MinEnterTime = 0f;
		}

		public IdleType PlayStationAnimation()
		{
			Animator animator = CrowdBehavior.Animator;
			IdleType[] array = ((!CrowdBehavior.IsFemale) ? CrowdSettings.MaleIdleTypes : CrowdSettings.FemaleIdleTypes);
			IdleType idleType = array[Random.Range(0, array.Length)];
			animator.SetCurrentIdleType(idleType);
			return idleType;
		}

		public void LeaveStation()
		{
			if (StationData != null)
			{
				StationData.Remove(CrowdBehavior);
				StationData = null;
				MinEnterTime = LogicMain.CurrentTime + CompletedCooldown;
				OnStation = false;
				PlayingIdle = IdleType.Invalid;
			}
		}

		public void StopAtStationInit()
		{
			if (StationData != null)
			{
				LeaveStation();
			}
			if (!(LogicMain.CurrentTime > MinEnterTime))
			{
				return;
			}
			NavMeshAgent agent = CrowdBehavior.Agent;
			Vector3 position = CrowdBehavior.Position;
			MinEnterTime = LogicMain.CurrentTime + Random.Range(CheckIntervalMin, CheckIntervalMax);
			Octree<Crowd.CrowdStationData> crowdStationIndex = Crowd.Instance.CrowdStationIndex;
			CrowdStaticCache.Stations.Clear();
			if (crowdStationIndex != null)
			{
				crowdStationIndex.GetObjects(CrowdStaticCache.Stations, position, 0f);
			}
			BBList<Crowd.CrowdStationData> stations = CrowdStaticCache.Stations;
			if (stations.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < stations.Count; i++)
			{
				Crowd.CrowdStationData crowdStationData = stations[i];
				CrowdStation station = crowdStationData.Station;
				if (!station.enabled || crowdStationData.CrowdOnStation.Count >= station.MaxCrowdOnStation)
				{
					continue;
				}
				Vector3 vector = position - station.CachedPosition;
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude > station.AttractRadius)
				{
					continue;
				}
				float num = Vector3.Angle(vector, station.CachedRotation * Vector3.forward) * Mathf.Sign(Vector3.Dot(vector, station.CachedRotation * Vector3.right));
				if (Mathf.Abs(num) > station.AttractAngle * 0.5f)
				{
					continue;
				}
				num = Mathf.Clamp(num, (0f - station.IdleAngle) * 0.5f, station.IdleAngle * 0.5f);
				Vector3 position2 = station.CachedPosition + Quaternion.Euler(0f, num, 0f) * station.CachedRotation * Vector3.forward * station.IdleRadius;
				NavMeshHit _hit;
				if (!NavigationHelper.CascadingSamplePosition(position2, out _hit, agent.areaMask))
				{
					continue;
				}
				position2 = _hit.position;
				if (IsSpotAtStationOccupied(crowdStationData, CrowdBehavior, position2))
				{
					continue;
				}
				NavMeshPath navMeshPath = new NavMeshPath();
				if (NavMesh.CalculatePath(position, position2, agent.areaMask, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
				{
					if (Random.value > station.AcceptChance)
					{
						MinEnterTime = LogicMain.CurrentTime + RejectedCooldown;
						break;
					}
					agent.SetPath(navMeshPath);
					agent.Resume();
					agent.stoppingDistance = 0.25f;
					agent.avoidancePriority = CrowdSettings.MaxAvoidancePriority;
					StationData = crowdStationData;
					StationData.Add(CrowdBehavior);
					LeaveTime = LogicMain.CurrentTime + Mathf.Lerp(station.MinIdleDuration, station.MaxIdleDuration, Random.value);
					TargetPosition = position2;
					break;
				}
			}
		}

		public bool StopAtStationUpdate()
		{
			return StopAtStationUpdateInternal();
		}

		public bool StopAtStationUpdateInternal()
		{
			Crowd.CrowdStationData stationData = StationData;
			if (stationData == null)
			{
				return false;
			}
			CrowdStation station = stationData.Station;
			if (station != null && !station.enabled)
			{
				LeaveStation();
				return false;
			}
			if (LogicMain.CurrentTime > LeaveTime)
			{
				LeaveStation();
				return false;
			}
			Vector3 position = CrowdBehavior.Position;
			NavMeshAgent agent = CrowdBehavior.Agent;
			float localDt = CrowdBehavior.LocalDt;
			Vector3 vector = station.CachedPosition - position;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float num = station.IdleRadius + agent.stoppingDistance;
			if (OnStation)
			{
				num += CrowdSettings.StayAtStationAddition;
			}
			bool flag = magnitude <= num;
			Vector3 b = Vector3.zero;
			if (agent.hasPath)
			{
				Vector3 vector2 = agent.velocity;
				float num2 = vector2.magnitude;
				Vector3 desiredVelocity = agent.desiredVelocity;
				float magnitude2 = desiredVelocity.magnitude;
				if (magnitude2 > num2)
				{
					vector2 = desiredVelocity;
					num2 = magnitude2;
				}
				float num3 = Mathf.Min(num2, agent.speed);
				if (!flag && num3 < CrowdSettings.StopSpeed)
				{
					Vector3 vector3 = ((!(num2 < MathHelper.Epsilon)) ? vector2 : vector);
					vector3.Normalize();
					b = vector3 * CrowdSettings.StopSpeed;
				}
				else
				{
					b = desiredVelocity.normalized * num3;
				}
			}
			CrowdBehavior.TargetVelocity = Vector3.Lerp(CrowdBehavior.TargetVelocity, b, Damping.CalculateFactor(CrowdSettings.VelocityDamping, 1f, localDt));
			agent.velocity = Damping.Damp(agent.velocity, CrowdBehavior.TargetVelocity, CrowdSettings.VelocityDamping, 1f, CrowdBehavior.LocalDt);
			if (!flag)
			{
				return GoToStationUpdate(magnitude);
			}
			return IsOnStationUptate();
		}

		private bool GoToStationUpdate(float _distanceToStation)
		{
			Crowd.CrowdStationData stationData = StationData;
			CrowdStation station = stationData.Station;
			NavMeshAgent agent = CrowdBehavior.Agent;
			Animator animator = CrowdBehavior.Animator;
			float localDt = CrowdBehavior.LocalDt;
			if (!agent.hasPath && !agent.pathPending)
			{
				LeaveStation();
				return false;
			}
			OnStation = false;
			PlayingIdle = IdleType.Invalid;
			float t = Mathf.Clamp01(1f - (_distanceToStation - station.IdleRadius) / (station.AttractRadius - station.IdleRadius));
			float to = Mathf.Lerp(CrowdBehavior.BaseSpeed, CrowdSettings.StopSpeed, t);
			agent.speed = Damping.Damp(agent.speed, to, SlowDownDamping, 1f, localDt);
			float currentSpeed = animator.GetCurrentSpeed();
			float num = Mathf.Min(agent.velocity.magnitude, agent.speed) / 6.27f;
			if (Mathf.Abs(currentSpeed - num) > 0.05f)
			{
				animator.SetCurrentSpeed(Mathf.Lerp(currentSpeed, num, Damping.CalculateFactor(CrowdSettings.AnimatorSpeedDamping, 1f, localDt)));
			}
			Vector3 normalized = agent.desiredVelocity.normalized;
			if (normalized.sqrMagnitude > float.Epsilon)
			{
				CrowdBehavior.TargetRotation = Damping.Damp(CrowdBehavior.TargetRotation, Quaternion.LookRotation(normalized), RotateDamping, 1f, localDt);
			}
			return true;
		}

		private bool IsOnStationUptate()
		{
			Crowd.CrowdStationData stationData = StationData;
			CrowdStation station = stationData.Station;
			Vector3 position = CrowdBehavior.Position;
			NavMeshAgent agent = CrowdBehavior.Agent;
			Animator animator = CrowdBehavior.Animator;
			float localDt = CrowdBehavior.LocalDt;
			if (!OnStation)
			{
				if (IsSpotAtStationOccupied(stationData, CrowdBehavior, position))
				{
					LeaveStation();
					return false;
				}
				OnStation = true;
				if (agent.hasPath)
				{
					agent.ResetPath();
				}
				CrowdBehavior.CrowdFollowCrowdGraph.Reset(stationData.NodeData ?? CrowdBehavior.CrowdFollowCrowdGraph.NodeData);
			}
			if (PlayingIdle != IdleType.Invalid && animator.GetCurrentIdleType() == IdleType.Neutral && animator.GetCurrentMovementState() == MovementState.Idle)
			{
				animator.SetCurrentIdleType(PlayingIdle);
			}
			if (PlayingIdle == IdleType.Invalid && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == AnimationStateHashes.State_LowProfile_Idle_Footm.NameHash)
			{
				PlayingIdle = PlayStationAnimation();
			}
			Vector3 vector = station.CachedPosition - position;
			vector.y = 0f;
			if (Vector3.Angle(CrowdBehavior.Rotation * Vector3.forward, vector) > AngleSensitivity)
			{
				CrowdBehavior.TargetRotation = Damping.Damp(CrowdBehavior.TargetRotation, Quaternion.LookRotation(vector), RotateDamping, 1f, localDt);
			}
			return true;
		}

		private static bool IsSpotAtStationOccupied(Crowd.CrowdStationData _stationData, CrowdBehavior _crowdBehavior, Vector3 _target)
		{
			NavMeshAgent agent = _crowdBehavior.Agent;
			for (int i = 0; i < _stationData.CrowdOnStation.Count; i++)
			{
				CrowdBehavior crowdBehavior = _stationData.CrowdOnStation[i];
				if (!(crowdBehavior == _crowdBehavior))
				{
					NavMeshAgent agent2 = crowdBehavior.Agent;
					Vector3 vector = ((!agent.hasPath) ? crowdBehavior.Position : agent2.destination);
					if ((vector - _target).magnitude < agent2.radius + agent.radius + CrowdSettings.StationSpotOccupiedAddition)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
