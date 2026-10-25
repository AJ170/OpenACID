using System;
using UnityEngine;

[Serializable]
public class ParkourStep
{
	public enum eParkourStepType
	{
		Jump = 0,
		Balance = 1
	}

	public Vector3 StartPosition;

	public Vector3 StartTangent;

	public Vector3 EndPosition;

	public Vector3 EndTangent;

	public eParkourStepType StepType;
}
