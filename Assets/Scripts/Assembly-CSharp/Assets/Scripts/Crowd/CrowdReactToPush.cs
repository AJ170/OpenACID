using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Crowd
{
	public class CrowdReactToPush : MonoBehaviour
	{
		public static bool EnablePushReaction = true;

		public static bool OnlyReactToPlayerPush = true;

		public static float MaxDistanceFromPlayer = 1.5f;

		public static float MaxDistanceForBigReaction = 0.35f;

		public static float PlayerVelocityThreshold = 0.2f;

		public static float PlayerVelocityThresholdSqr = PlayerVelocityThreshold * PlayerVelocityThreshold;

		public static float PlayerAdvancementFactor = 7f;

		public static float Cooldown = 3f;

		public static float MinPushStrength = 0.15f;

		public static float SmallPushStrength = 1f;

		public static float MediumPushStrength = 1.5f;

		public static float BigPushStrength = 2f;

		public static float FallPushStrength = float.MaxValue;

		public static float MaxSpeedForSmallPush = 1f;

		public static float MinPushVelocity = 0f;

		public static float PushStrengthPow = 1f;

		[HideInInspector]
		public CrowdBehavior CrowdBehavior;

		[HideInInspector]
		public Animator Animator;

		public Vector3 ReactionDirection;

		public float ReactionStrength;

		public bool ReactionNoticeable;

		private float m_reactionMinTime;

		public void ReactToPush(Vector3 _targetPosition, Vector3 _expectedVelocity, Vector3 _actualVelocity, Vector3 _facingDirection, float _desiredSpeed)
		{
			Animator animator = Animator;
			Vector3 vector = _actualVelocity.normalized;
			Vector3 normalized = _expectedVelocity.normalized;
			float value = Vector3.Dot(vector, normalized);
			Vector3 vector2 = _actualVelocity - _expectedVelocity * Mathf.Clamp01(value);
			float num = Mathf.Max(0f, vector2.magnitude - MinPushVelocity);
			ReactionDirection = vector2.normalized;
			bool flag = false;
			PlayerCharacter playerEntity = LogicMain.PlayerEntity;
			if (OnlyReactToPlayerPush && playerEntity != null)
			{
				Vector3 velocity = playerEntity.UnityComponentCache.NavMeshAgent.velocity;
				if (velocity.sqrMagnitude > PlayerVelocityThresholdSqr)
				{
					Vector3 vector3 = velocity * CrowdBehavior.LocalDt * PlayerAdvancementFactor;
					Vector3 vector4 = playerEntity.Position + vector3;
					float magnitude = (_targetPosition - vector4).magnitude;
					flag = magnitude < MaxDistanceFromPlayer;
					if (magnitude < MaxDistanceForBigReaction && num < MediumPushStrength)
					{
						float t = 1f - magnitude / MaxDistanceForBigReaction;
						num = Mathf.Lerp(MediumPushStrength, BigPushStrength, t);
						ReactionDirection = (_targetPosition - vector4).normalized;
						vector = ReactionDirection;
					}
				}
			}
			ReactionNoticeable = flag && num > MinPushStrength;
			if (flag && num >= SmallPushStrength && LogicMain.CurrentTime > m_reactionMinTime)
			{
				float num2 = 0f - Vector3.Dot(_facingDirection, vector);
				if (num >= FallPushStrength && LogicMain.CurrentTime > m_reactionMinTime)
				{
					animator.SetCurrentReactionDirectionZ(num2);
					animator.SetCurrentReactionType(ReactionType.PushBack);
					animator.SetCurrentStopAction(true);
					m_reactionMinTime = LogicMain.CurrentTime + Cooldown;
				}
				if (num >= BigPushStrength && LogicMain.CurrentTime > m_reactionMinTime)
				{
					animator.SetCurrentReactionType(ReactionType.StumbleBackwardHeavy);
					animator.SetTargetDirection((num2 > 0f) ? AnimationController.Direction.Front : AnimationController.Direction.Back);
					m_reactionMinTime = LogicMain.CurrentTime + Cooldown;
				}
				if (num >= MediumPushStrength && LogicMain.CurrentTime > m_reactionMinTime)
				{
					animator.SetCurrentReactionType(ReactionType.StumbleBackwardLight);
					animator.SetTargetDirection((num2 > 0f) ? AnimationController.Direction.Front : AnimationController.Direction.Back);
					m_reactionMinTime = LogicMain.CurrentTime + Cooldown;
				}
				if (num >= SmallPushStrength && _desiredSpeed < MaxSpeedForSmallPush && LogicMain.CurrentTime > m_reactionMinTime)
				{
					animator.SetTargetDirection(AnimationHelper.DetermineDirection6Ways(_facingDirection, _actualVelocity));
					animator.SetCurrentReactionType(ReactionType.GetPushedByWalk);
					m_reactionMinTime = LogicMain.CurrentTime + Cooldown;
				}
			}
		}

		public void ResetDelay()
		{
			m_reactionMinTime = LogicMain.CurrentTime + Cooldown;
		}
	}
}
