using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion;
using UbiCore.Debug;
using UnityEngine;
using UnityEngine.AI;

public class ParcourPrediction : MonoBehaviour
{
	public bool HasInteraction;

	public ParcourHelper.EEnvironmentInteraction Interaction;

	public float DistanceToStart;

	public OffMeshLinkEntity Link;

	public eLinkType LinkType;

	public GameObject Object;

	public Entity Entity;

	public Vector3 CurrentPosition;

	public Vector3 CurrentDirection;

	public Vector3 StartPosition;

	public Vector3 StartDirection;

	public Vector3 EndPosition;

	public Vector3 EndDirection;

	public static float AnticipateDistance = 20f;

	public void OnDrawGizmos()
	{
		Color green = Color.green;
		if (!HasInteraction)
		{
			green = Color.red;
		}
	}

	public void Reset()
	{
		HasInteraction = false;
		DistanceToStart = 0f;
		Interaction = ParcourHelper.EEnvironmentInteraction.Invalid;
		Link = null;
		LinkType = eLinkType.Invalid;
		CurrentPosition = Vector3.zero;
		CurrentDirection = Vector3.forward;
		StartPosition = Vector3.zero;
		StartDirection = Vector3.forward;
		EndPosition = Vector3.zero;
		EndDirection = Vector3.forward;
		Object = null;
		Entity = null;
	}

	public void SetJump(Vector3 _targetPosition, ParcourHelper.EEnvironmentInteraction _interaction = ParcourHelper.EEnvironmentInteraction.JumpDown)
	{
		PlayerCharacter playerEntity = LogicMain.PlayerEntity;
		if (playerEntity != null)
		{
			NavMeshAgent navMeshAgent = playerEntity.UnityComponentCache.NavMeshAgent;
			if (!(navMeshAgent == null))
			{
				Vector3 position = navMeshAgent.transform.position;
				Vector3 endDirection = _targetPosition - position;
				endDirection.y = 0f;
				endDirection.Normalize();
				CurrentPosition = position;
				CurrentDirection = navMeshAgent.transform.forward;
				HasInteraction = true;
				DistanceToStart = 0f;
				Interaction = _interaction;
				Link = null;
				LinkType = eLinkType.Invalid;
				StartPosition = CurrentPosition;
				StartDirection = CurrentDirection;
				EndPosition = _targetPosition;
				EndDirection = endDirection;
				Object = null;
				Entity = null;
			}
		}
	}

	public void UpdateParcourPrediction(Vector3 _direction, float _preferredJumpLength = -1f)
	{
		PlayerCharacter playerEntity = LogicMain.PlayerEntity;
		if (playerEntity == null)
		{
			return;
		}
		NavMeshAgent navMeshAgent = playerEntity.UnityComponentCache.NavMeshAgent;
		if (navMeshAgent == null)
		{
			return;
		}
		Vector3 position = (CurrentPosition = navMeshAgent.transform.position);
		CurrentDirection = _direction;
		HasInteraction = false;
		AnimationTaskManager taskManager = LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager;
		if (taskManager != null)
		{
			IParkourAnimation parkourAnimation = taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) as IParkourAnimation;
			if (parkourAnimation != null)
			{
				position = parkourAnimation.Destination;
			}
		}
		NavMeshHit _hit;
		if (!NavigationHelper.CascadingSamplePosition(position, out _hit, navMeshAgent.areaMask))
		{
			return;
		}
		position = _hit.position;
		Profiling.Start("Update Parcour prediction", float.MaxValue);
		eLinkType _linkType = eLinkType.None;
		Vector3 vector = position;
		Vector3 vector2 = _direction;
		Vector3 _targetPosition = Vector3.zero;
		Vector3 _targetDirection = Vector3.zero;
		OffMeshLinkEntity _link = null;
		GameObject _targetObject = null;
		Entity _targetEntity = null;
		ParcourHelper.EEnvironmentInteraction eEnvironmentInteraction = ParcourHelper.EEnvironmentInteraction.Invalid;
		bool flag = (double)_direction.magnitude >= 0.5;
		if (eEnvironmentInteraction == ParcourHelper.EEnvironmentInteraction.Invalid)
		{
			eEnvironmentInteraction = ParcourHelper.CheckCivilianCovers(playerEntity, navMeshAgent, vector, ref _targetPosition, ref _targetEntity);
		}
		if (eEnvironmentInteraction == ParcourHelper.EEnvironmentInteraction.Invalid)
		{
			eEnvironmentInteraction = ParcourHelper.CheckAcrobatics(navMeshAgent, vector, vector2, out _linkType, out _link, out _targetPosition, out _targetDirection, out _targetObject, _preferredJumpLength);
		}
		if (eEnvironmentInteraction == ParcourHelper.EEnvironmentInteraction.Invalid && flag && NavigationHelper.SafeRaycast(vector, vector + vector2 * AnticipateDistance, out _hit, navMeshAgent.areaMask))
		{
			vector = _hit.position - vector2 * Mathf.Min(Mathf.Min(ParcourHelper.Acrobatics.EdgeDistance, ParcourHelper.Acrobatics.JumpEdgeDistance) * 0.9f, _hit.distance);
			if (NavigationHelper.CascadingSamplePosition(vector, out _hit, navMeshAgent.areaMask))
			{
				vector = _hit.position;
			}
			eEnvironmentInteraction = ParcourHelper.CheckAcrobatics(navMeshAgent, vector, vector2, out _linkType, out _link, out _targetPosition, out _targetDirection, out _targetObject, -1f);
			if (eEnvironmentInteraction == ParcourHelper.EEnvironmentInteraction.Invalid)
			{
				vector2 = -_hit.normal;
				eEnvironmentInteraction = ParcourHelper.CheckAcrobatics(navMeshAgent, vector, vector2, out _linkType, out _link, out _targetPosition, out _targetDirection, out _targetObject, -1f);
				if (eEnvironmentInteraction != ParcourHelper.EEnvironmentInteraction.Link)
				{
					eEnvironmentInteraction = ParcourHelper.EEnvironmentInteraction.Invalid;
				}
			}
		}
		if (eEnvironmentInteraction != 0)
		{
			HasInteraction = true;
			DistanceToStart = Vector3.Dot(vector - position, _direction.normalized);
			StartPosition = vector;
			StartDirection = vector2;
			Interaction = eEnvironmentInteraction;
			Link = _link;
			LinkType = _linkType;
			EndPosition = _targetPosition;
			EndDirection = _targetDirection;
			Object = _targetObject;
			Entity = _targetEntity;
		}
		Profiling.Stop();
	}
}
