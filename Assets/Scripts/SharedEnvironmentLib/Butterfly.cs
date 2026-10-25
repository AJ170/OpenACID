using UnityEngine;

public class Butterfly : MonoBehaviour
{
	private Vector3 LastPosition;

	private void Start()
	{
		LastPosition = Vector3.zero;
		Animation component = GetComponent<Animation>();
		component["FlappyWings"].time = Random.Range(0f, component["FlappyWings"].length);
	}

	private void Update()
	{
		if (LastPosition != Vector3.zero)
		{
			Vector3 vector = base.transform.position - LastPosition;
			base.transform.LookAt(base.transform.position - vector);
		}
		LastPosition = base.transform.position;
	}
}
