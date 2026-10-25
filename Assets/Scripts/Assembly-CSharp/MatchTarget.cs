using UnityEngine;

public class MatchTarget : MonoBehaviour
{
	public Transform TheTarget;

	private void Start()
	{
	}

	private void Update()
	{
		Transform transform = base.transform;
		transform.localPosition = TheTarget.position;
		transform.localRotation = TheTarget.rotation;
	}
}
