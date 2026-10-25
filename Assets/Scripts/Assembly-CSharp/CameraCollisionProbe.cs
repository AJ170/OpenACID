using UnityEngine;

public class CameraCollisionProbe : MonoBehaviour
{
	public Camera Camera;

	public AcierCameraController CameraController;

	public Rigidbody Rigidbody;

	public float Damping = 0.2f;

	public float MaxDistance = 1f;

	public float ForceMultiplier = 1000f;

	public float ForceSlow = 50f;

	public bool UseVelocity = true;

	private void Start()
	{
	}

	public void Teleport(Vector3 _newPosition)
	{
		Rigidbody.MovePosition(_newPosition);
	}

	private void FixedUpdate()
	{
		if (Camera.main != null && Camera == null)
		{
			Camera = Camera.main;
		}
		if (Camera.main != null && CameraController == null)
		{
			CameraController = AcierCameraController.Instance;
			CameraController.CollisionProbe = this;
		}
		if (Rigidbody == null)
		{
			Rigidbody = GetComponent<Rigidbody>();
		}
		if (Camera == null || CameraController == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		Transform transform = base.transform;
		Vector3 position = transform.position;
		Vector3 vector = CameraController.RaycastPosition - position;
		Vector3 normalized = vector.normalized;
		float magnitude = vector.magnitude;
		if (magnitude > MaxDistance)
		{
			Teleport(CameraController.RaycastPosition);
			return;
		}
		if (!UseVelocity)
		{
			RaycastHit hitInfo;
			bool flag = Rigidbody.SweepTest(normalized, out hitInfo, magnitude);
			float t = deltaTime / (Mathf.Lerp(Damping, 0f, magnitude / MaxDistance) + deltaTime);
			Rigidbody.MovePosition(Vector3.Lerp(position, position + normalized * ((!flag) ? magnitude : hitInfo.distance), t));
			Rigidbody.velocity = Vector3.zero;
		}
		else
		{
			Rigidbody.AddForce(-Rigidbody.velocity * ForceSlow * deltaTime, ForceMode.VelocityChange);
			Rigidbody.AddForce(normalized * magnitude * magnitude * ForceMultiplier * deltaTime, ForceMode.VelocityChange);
		}
		Rigidbody.angularVelocity = Vector3.zero;
	}
}
