using System;
using UnityEngine;

public class HumanoidViewerCamera : MonoBehaviour
{
	public float rotationSpeed = 10f;

	public float panSpeed = 1f;

	public float zoomValue = 30f;

	public GameObject target;

	private Vector3 positionfocusVec;

	private Vector3 focusVec;

	private Vector3 adjustFocusVec;

	private Vector3 originPos = Vector3.zero;

	private Vector3 nextPos = Vector3.zero;

	private Vector3 moveVec3 = Vector3.zero;

	private Vector3 cameradistanceVector = Vector3.zero;

	private Vector2 moveVec2 = Vector2.zero;

	private int x;

	private bool isPanning;

	private bool isRotating;

	private bool isZooming;

	private float zoomfactor;

	private float cameradistance;

	public GameObject objtest;

	private void Start()
	{
		Camera.main.transform.position = new Vector3(0f, 0f, 1f);
		objtest = new GameObject();
		objtest.transform.position = new Vector3(0f, 0f, 0f);
		target = objtest;
		base.transform.LookAt(target.transform.position);
		focusVec = target.transform.localPosition - base.gameObject.transform.localPosition;
		float z = Mathf.Sqrt(Mathf.Pow(focusVec.x, 2f) + Mathf.Pow(focusVec.y, 2f) + Mathf.Pow(focusVec.z, 2f)) - 5f;
		Camera.main.transform.Translate(0f, 0f, z);
	}

	private void Update()
	{
		if (target == null)
		{
			target = base.gameObject;
		}
		if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt))
		{
			isRotating = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			originPos = Vector3.zero;
			isRotating = false;
		}
		if (Input.GetMouseButton(2))
		{
			isPanning = true;
		}
		if (Input.GetMouseButtonUp(2))
		{
			originPos = Vector3.zero;
			isPanning = false;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			isZooming = true;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
		}
		if (isRotating)
		{
			isZooming = false;
			isPanning = false;
			if (originPos != Vector3.zero)
			{
				nextPos = Input.mousePosition;
				moveVec3 = nextPos - originPos;
				moveVec2.x = moveVec3.x;
				moveVec2.y = moveVec3.y;
				base.transform.RotateAround(target.transform.position, base.transform.right, (0f - moveVec2.y) * rotationSpeed * RealTime.deltaTime);
				base.transform.RotateAround(target.transform.position, target.transform.up, moveVec2.x * rotationSpeed * RealTime.deltaTime);
			}
			originPos = Input.mousePosition;
		}
		if (isPanning)
		{
			isRotating = false;
			isZooming = false;
			target = base.gameObject;
			if (originPos != Vector3.zero)
			{
				nextPos = Input.mousePosition;
				moveVec3 = nextPos - originPos;
				moveVec2.x = moveVec3.x * panSpeed * RealTime.deltaTime;
				moveVec2.y = moveVec3.y * panSpeed * RealTime.deltaTime;
				base.transform.LookAt(target.transform.position);
				base.transform.Translate(new Vector3(0f - moveVec2.x, 0f, 0f), Space.Self);
				base.transform.Translate(new Vector3(0f, 0f - moveVec2.y, 0f), Space.Self);
				objtest.transform.Translate(new Vector3(0f - moveVec2.x, 0f, 0f), Space.Self);
				target = objtest;
			}
			originPos = Input.mousePosition;
		}
		if (isZooming)
		{
			isPanning = false;
			isRotating = false;
			cameradistanceVector = Camera.main.transform.position - target.transform.position;
			cameradistance = (float)Math.Sqrt(cameradistanceVector.x * cameradistanceVector.x + cameradistanceVector.y * cameradistanceVector.y + cameradistanceVector.z * cameradistanceVector.z);
			zoomfactor = Input.mouseScrollDelta.y * zoomValue * RealTime.deltaTime * (cameradistance / 2f);
			Camera.main.transform.Translate(0f, 0f, zoomfactor);
		}
	}
}
