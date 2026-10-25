using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Extended Event Trigger")]
public class UIExtendedEventTrigger : MonoBehaviour
{
	public static UIExtendedEventTrigger Current;

	public List<EventDelegate> onDrag = new List<EventDelegate>();

	private void OnDrag(Vector2 _coordVector2)
	{
		Current = this;
		EventDelegate.Execute(onDrag);
		Current = null;
	}
}
