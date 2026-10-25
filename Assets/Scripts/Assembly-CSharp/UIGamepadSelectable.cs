using UnityEngine;

public class UIGamepadSelectable : MonoBehaviour
{
	public enum EMode
	{
		Autodetect = 0,
		Enabled = 1,
		Disabled = 2
	}

	public int Priority;

	public EMode Mode;

	public UIWidget Widget;

	public UIInput Input;

	public UIScrollView ScrollView;

	public bool IsBackButton;

	public string MessageEvent = "OnClick";

	public int PriorityRuntime { get; set; }

	public int PriorityCombined
	{
		get
		{
			return (Priority == 0) ? PriorityRuntime : Priority;
		}
	}

	public bool IsDropDownItem { get; set; }

	public void SimulateTap()
	{
		SendMessage(MessageEvent, SendMessageOptions.DontRequireReceiver);
	}
}
