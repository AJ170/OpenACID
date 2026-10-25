using UnityEngine;

public class RenderingSkyMesh : RenderingSky
{
	public override void Initialize()
	{
		Camera.main.clearFlags = CameraClearFlags.Color;
	}

	private void LateUpdate()
	{
		Camera camera = Camera.main;
		if (camera == null && !Application.isEditor)
		{
			camera = Camera.current;
		}
		if (camera != null)
		{
			Align(camera);
		}
	}

	public void Align(Camera c)
	{
		float num = c.farClipPlane * Mathf.Sqrt(1f / 3f) * 1.8f;
		base.transform.localScale = new Vector3(num, num, num);
		base.transform.localPosition = c.transform.position;
	}
}
