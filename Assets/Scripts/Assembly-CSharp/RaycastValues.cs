using UnityEngine;

public class RaycastValues : MonoBehaviour
{
	[SerializeField]
	public float eyeHeight = 1.85f;

	[SerializeField]
	public float hideCharacterDistance = 0.45f;

	[SerializeField]
	public float hideCharacterFilterDistance = 0.1f;

	[SerializeField]
	public float hideCharacterRaycastDistance = 0.15f;

	[SerializeField]
	public float hideCharacterRaycastFilterDistance = 0.1f;

	[SerializeField]
	public float hideReferenceHeightOffset = 0.1f;

	[SerializeField]
	public float raycastStartDamping = 0.2f;

	[SerializeField]
	public float helperRadius = 0.25f;

	[SerializeField]
	public float helperMoveForward = 0.5f;
}
