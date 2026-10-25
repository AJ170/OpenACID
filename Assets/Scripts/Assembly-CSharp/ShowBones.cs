using UnityEngine;

[ExecuteInEditMode]
public class ShowBones : MonoBehaviour
{
	private void drawbone(Transform t)
	{
		foreach (Transform item in t)
		{
			float num = 0.05f;
			Vector3 vector = new Vector3(num, 0f, 0f);
			Vector3 vector2 = new Vector3(0f, num, 0f);
			Vector3 vector3 = new Vector3(0f, 0f, num);
			vector = item.rotation * vector;
			vector2 = item.rotation * vector2;
			vector3 = item.rotation * vector3;
			Debug.DrawLine(t.position * 0.1f + item.position * 0.9f, t.position * 0.9f + item.position * 0.1f, Color.white, 0f, false);
			Debug.DrawLine(item.position, item.position + vector, Color.red, 0f, false);
			Debug.DrawLine(item.position, item.position + vector2, Color.green, 0f, false);
			Debug.DrawLine(item.position, item.position + vector3, Color.blue, 0f, false);
			drawbone(item);
		}
	}

	private void Update()
	{
		drawbone(base.transform);
	}
}
