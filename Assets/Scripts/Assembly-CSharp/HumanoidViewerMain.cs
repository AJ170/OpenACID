using Assets.Scripts.Engine;
using SharedBaseLib.System;
using UnityEngine;

public class HumanoidViewerMain : MonoBehaviour
{
	private void Start()
	{
		SharedBaseLib.System.Main.InitOnDemand();
		LayerPresets.Init();
		QualitySettings.SetQualityLevel(5, true);
		Rendering rendering = base.gameObject.AddComponent<Rendering>();
		rendering.OnPreInitialize();
		rendering.OnPostInitialize();
	}

	private void Update()
	{
	}
}
