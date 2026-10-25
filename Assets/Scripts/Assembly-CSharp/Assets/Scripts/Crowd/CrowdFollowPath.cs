using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdFollowPath : MonoBehaviour
	{
		[HideInInspector]
		public CrowdBehavior CrowdBehavior;

		public void FollowPathInit()
		{
			NavMeshAgent agent = CrowdBehavior.Agent;
			agent.Resume();
			CrowdBehavior.CrowdDetectIsStuck.Reset(CrowdBehavior.Position);
			CrowdBehavior.RandomizeAvoidancePriority();
			agent.speed = CrowdBehavior.BaseSpeed;
			agent.stoppingDistance = 1f;
		}

		public void OnDrawGizmosSelected()
		{
			NavMeshAgent agent = CrowdBehavior.Agent;
			if (agent.hasPath)
			{
				NavMeshPath path = agent.path;
				Vector3[] corners = path.corners;
				Vector3 position = base.transform.position;
				foreach (Vector3 vector in corners)
				{
					position = vector;
				}
			}
		}

		public void FollowPathUpdate()
		{
			Animator animator = CrowdBehavior.Animator;
			NavMeshAgent agent = CrowdBehavior.Agent;
			float localDt = CrowdBehavior.LocalDt;
			Vector3 normalized = agent.desiredVelocity.normalized;
			if (normalized.sqrMagnitude > float.Epsilon)
			{
				CrowdBehavior.TargetRotation = Quaternion.LookRotation(normalized);
			}
			CrowdBehavior.TargetVelocity = Vector3.Lerp(CrowdBehavior.TargetVelocity, normalized * agent.speed, Damping.CalculateFactor(CrowdSettings.VelocityDamping, 1f, localDt));
			agent.velocity = Vector3.Lerp(agent.velocity, normalized * agent.speed, Damping.CalculateFactor(CrowdSettings.VelocityDamping, 1f, localDt));
			float currentSpeed = animator.GetCurrentSpeed();
			float num = Mathf.Min(agent.velocity.magnitude, agent.speed) / 6.27f;
			if (Mathf.Abs(currentSpeed - num) > 0.001f)
			{
				animator.SetCurrentSpeed(Mathf.Lerp(currentSpeed, num, Damping.CalculateFactor(CrowdSettings.AnimatorSpeedDamping, 1f, localDt)));
			}
		}
	}
}
