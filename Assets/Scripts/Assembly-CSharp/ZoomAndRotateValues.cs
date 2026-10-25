using UnityEngine;

public class ZoomAndRotateValues : MonoBehaviour
{
	[SerializeField]
	public bool flip = true;

	[SerializeField]
	public float damping = 0.1f;

	[SerializeField]
	public float zoomDragDistance = 0.2f;

	[SerializeField]
	public float rotateDragDistance = 0.35f;

	public void CopyFrom(ZoomAndRotateValues _other)
	{
		flip = _other.flip;
		damping = _other.damping;
		zoomDragDistance = _other.zoomDragDistance;
		rotateDragDistance = _other.rotateDragDistance;
	}
}
