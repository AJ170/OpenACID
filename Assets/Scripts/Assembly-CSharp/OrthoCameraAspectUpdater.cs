using UnityEngine;

public class OrthoCameraAspectUpdater : MonoBehaviour
{
	private void Update()
	{
		base.transform.position = new Vector3((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, -2f);
		GetComponent<Camera>().orthographicSize = (float)Screen.height * 0.5f;
		GetComponent<Camera>().aspect = (float)Screen.width / (float)Screen.height;
	}
}
