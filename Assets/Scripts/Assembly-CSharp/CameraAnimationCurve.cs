using UnityEngine;

public class CameraAnimationCurve : MonoBehaviour
{
	[SerializeField]
	public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(50f, 90f), new Keyframe(120f, 55f));
}
