using UnityEngine;

public class retinaProButtonActivate : MonoBehaviour
{
	public GameObject target;

	public bool state = true;

	private void OnClick()
	{
		if (target != null)
		{
			NGUITools.SetActive(target, state);
			if (state)
			{
				retinaProUtil.sharedInstance.refreshAllWidgetsPixelPerfect(target);
			}
		}
	}
}
