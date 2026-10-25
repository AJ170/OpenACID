using System;
using UnityEngine;

[Serializable]
public class ScriptValues
{
	public enum Reference
	{
		Player = 0,
		Target = 1,
		PlayerRoot = 2,
		TargetRoot = 3
	}

	[SerializeField]
	public Reference cameraReference;

	[SerializeField]
	public Vector3 cameraOffset = new Vector3(0f, 1.8f, 0f);

	[SerializeField]
	public bool useCameraBone;

	[SerializeField]
	public HumanBodyBones cameraBone = HumanBodyBones.Spine;

	[SerializeField]
	public float cameraDamping = 0.1f;

	[SerializeField]
	public Vector3 cameraVelocity = Vector3.zero;

	[SerializeField]
	public Reference lookAtReference = Reference.Target;

	[SerializeField]
	public Vector3 lookAtOffset = new Vector3(0f, 0f, 0f);

	[SerializeField]
	public bool useLookAtBone;

	[SerializeField]
	public HumanBodyBones lookAtBone = HumanBodyBones.Spine;

	[SerializeField]
	public float lookAtDamping = 0.1f;

	[SerializeField]
	public Vector3 lookAtVelocity = Vector3.zero;

	[SerializeField]
	public float angularStartSpeed;

	[SerializeField]
	public float angularTargetSpeed;

	[SerializeField]
	public float angularDamping = 0.5f;
}
