using UnityEngine;

public class VisibilityBroadcaster : MonoBehaviour
{
	public GameObject TheTarget;

	public void OnBecameVisible()
	{
		TheTarget.SendMessage("OnBecameVisible");
	}

	public void OnBecameInvisible()
	{
		TheTarget.SendMessage("OnBecameInvisible");
	}
}
