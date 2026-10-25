using UbiCore.Debug.UI;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class EnvironmentMain : SharedMain
{
	public void Start()
	{
		if (Camera.main == null)
		{
			CreateFreeCamera();
		}
		base.gameObject.AddComponentIfNotExist<Rendering>();
		DebugMenu debugMenu = DebugMenu.Create(base.transform);
		debugMenu.Register<DebugMenuGeneral>();
		debugMenu.Register<DebugMenuRendering>();
		InputHandler.FindOrCreate<EnvironmentInputHandler>(base.transform);
		BroadcastMessage("OnPreInitialize", SendMessageOptions.DontRequireReceiver);
		BroadcastMessage("OnPostInitialize", SendMessageOptions.DontRequireReceiver);
	}
}
