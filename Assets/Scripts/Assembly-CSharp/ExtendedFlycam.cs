using UnityEngine;

public class ExtendedFlycam : MonoBehaviour
{
	public float cameraSensitivity = 5f;

	public float climbSpeed = 5f;

	public float normalMoveSpeed = 0.1f;

	public float slowMoveFactor = 0.25f;

	public float fastMoveFactor = 3f;

	private float rotationX;

	private float rotationY;

	private void Start()
	{
		Screen.lockCursor = false;
		Camera main = Camera.main;
		Transform transform = main.gameObject.transform;
		Vector3 localEulerAngles = transform.localEulerAngles;
		rotationX = localEulerAngles.x;
		rotationY = localEulerAngles.y;
	}

	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			Screen.lockCursor = true;
			Time.captureFramerate = 0;
			float deltaTime = Time.deltaTime;
			rotationX += Input.GetAxis("Mouse Y") * cameraSensitivity;
			rotationY += Input.GetAxis("Mouse X") * cameraSensitivity;
			rotationX = Mathf.Clamp(rotationX, -90f, 90f);
			base.transform.localRotation = Quaternion.AngleAxis(rotationY, Vector3.up);
			base.transform.localRotation *= Quaternion.AngleAxis(rotationX, Vector3.left);
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				base.transform.position += base.transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical");
				base.transform.position += base.transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal");
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				base.transform.position += base.transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical");
				base.transform.position += base.transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal");
			}
			else
			{
				base.transform.position += base.transform.forward * normalMoveSpeed * Input.GetAxis("Vertical");
				base.transform.position += base.transform.right * normalMoveSpeed * Input.GetAxis("Horizontal");
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position += base.transform.up * climbSpeed * deltaTime;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position -= base.transform.up * climbSpeed * deltaTime;
			}
		}
		else
		{
			Screen.lockCursor = false;
		}
	}
}
