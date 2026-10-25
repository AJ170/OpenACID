using System;
using UnityEngine;

[Serializable]
public class CombatValues
{
	[SerializeField]
	public float referenceMinThreshold = 0.1f;

	[SerializeField]
	public float referenceMaxThreshold = 2f;

	[SerializeField]
	public float minDistanceFromObstacles = 0.35f;

	[SerializeField]
	public float orientationLockTimeWhenReleaseRotation = 1.5f;

	public float maxTargetDistance = 4f;

	public void CopyFrom(CombatValues _other)
	{
		referenceMinThreshold = _other.referenceMinThreshold;
		referenceMaxThreshold = _other.referenceMaxThreshold;
		minDistanceFromObstacles = _other.minDistanceFromObstacles;
		orientationLockTimeWhenReleaseRotation = _other.orientationLockTimeWhenReleaseRotation;
		maxTargetDistance = _other.maxTargetDistance;
	}
}
