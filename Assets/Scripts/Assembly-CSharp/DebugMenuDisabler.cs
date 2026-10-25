using System.Collections.Generic;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using UbiCore.Debug.UI;
using UnityEngine;

public class DebugMenuDisabler : DebugMenuItem
{
	private class CamCull
	{
		public Camera TheCamera;

		public int CullingMask;

		public CamCull(Camera _camera, int _mask)
		{
			TheCamera = _camera;
			CullingMask = _mask;
		}
	}

	private List<CamCull> myCameras = new List<CamCull>();

	private int m_ShaderDebug;

	public override string GetName()
	{
		return "Disabler";
	}

	public override void OnApply()
	{
	}

	public override void OnOpen()
	{
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.F11))
		{
			ScreenshotHacks();
		}
	}

	private static void ScreenshotHacks()
	{
		SetPlayerImmortal(true);
		SetOutlineVisibility(false);
		BarrierManager barrierManager = Object.FindObjectOfType<BarrierManager>();
		if (barrierManager != null)
		{
			SetBarrierVisibility(barrierManager, false);
		}
	}

	private static void SetPlayerImmortal(bool _state)
	{
		CombatController instance = CombatController.Instance;
		instance.DebugPlayerImmortal = _state;
	}

	private static void SetOutlineVisibility(bool enable_outline)
	{
		if (enable_outline != AssassinRendering.HasOutline())
		{
			if (enable_outline)
			{
				AssassinRendering.EnableOutline();
			}
			else
			{
				AssassinRendering.DisableOutline();
			}
		}
	}

	private static void SetBarrierVisibility(BarrierManager bm, bool barrier_state)
	{
		if (barrier_state != bm.IsVisible)
		{
			bm.SetVisible(barrier_state);
		}
	}

	private void SetUICameraVisibility(bool cam_toggle)
	{
		int mask = LayerMask.GetMask("UI", "UI3D", "UIShared", "UI_NonInteractable", "UI_Interactable");
		if (!cam_toggle)
		{
			Camera[] array = Object.FindObjectsOfType<Camera>();
			Camera[] array2 = array;
			foreach (Camera camera in array2)
			{
				myCameras.Add(new CamCull(camera, camera.cullingMask));
				camera.cullingMask &= ~mask;
			}
			return;
		}
		foreach (CamCull myCamera in myCameras)
		{
			if (myCamera.TheCamera != null)
			{
				myCamera.TheCamera.cullingMask = myCamera.CullingMask;
			}
		}
		myCameras.Clear();
	}

	public override void OnMenu()
	{
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		CombinedPostEffect combinedPostEffect = Object.FindObjectOfType<CombinedPostEffect>();
		if ((bool)combinedPostEffect)
		{
			combinedPostEffect.enabled = GUILayout.Toggle(combinedPostEffect.enabled, "Post Fx", "Button", GUILayout.Height(40f));
		}
		if (AcierInputHandler.Instance != null)
		{
			AcierInputHandler.Instance.ShowParcourFeedback = GUILayout.Toggle(AcierInputHandler.Instance.ShowParcourFeedback, "Parcour Feedback", "Button", GUILayout.Height(40f));
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		FindAndDisplayGameObject("BakeStatic");
		FindAndDisplayGameObject("Colliders");
		FindAndDisplayGameObject("MoodScenario/Wind_Animated");
		FindAndDisplayGameObject("ImportStatic/Wind_Animated");
		FindAndDisplayGameObject("Haystacks");
		FindAndDisplayGameObject("Birds");
		FindAndDisplayGameObject("Roof Gardens");
		GUILayout.EndHorizontal();
		GUILayout.Space(30f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Screenshot Mode Toggles");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		bool value = myCameras.Count == 0;
		value = GUILayout.Toggle(value, "UI Rendering", "Button", GUILayout.Height(40f));
		if (value != (myCameras.Count == 0))
		{
			SetUICameraVisibility(value);
		}
		BarrierManager barrierManager = Object.FindObjectOfType<BarrierManager>();
		if (barrierManager != null)
		{
			bool barrier_state = GUILayout.Toggle(barrierManager.IsVisible, "Animus Walls", "Button", GUILayout.Height(40f));
			SetBarrierVisibility(barrierManager, barrier_state);
		}
		bool outlineVisibility = GUILayout.Toggle(AssassinRendering.HasOutline(), "Character outline", "Button", GUILayout.Height(40f));
		SetOutlineVisibility(outlineVisibility);
		Highlight.ms_HighlightingActive = GUILayout.Toggle(Highlight.ms_HighlightingActive, "Highlighting", "Button", GUILayout.Height(40f));
		bool flag = Singleton<EffectManager>.Instance.HasCondition(EffectManager.ECondition.ReducedGore);
		bool flag2 = GUILayout.Toggle(flag, "Reduced Gore", "Button", GUILayout.Height(40f));
		if (flag != flag2)
		{
			if (flag2)
			{
				Singleton<EffectManager>.Instance.ActivateCondition(EffectManager.ECondition.ReducedGore);
			}
			else
			{
				Singleton<EffectManager>.Instance.RemoveCondition(EffectManager.ECondition.ReducedGore);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(30f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Destructive changes (unable to revert)");
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Terminate Lights (WTF!?)", GUILayout.Height(40f)))
		{
			KillTheLights();
		}
		GUILayout.BeginHorizontal();
		int num = m_ShaderDebug;
		if (GUILayout.Toggle(num == 0, "Full"))
		{
			num = 0;
		}
		if (GUILayout.Toggle(num == 1, "main"))
		{
			num = 1;
		}
		if (GUILayout.Toggle(num == 2, "lit"))
		{
			num = 2;
		}
		if (GUILayout.Toggle(num == 3, "rim"))
		{
			num = 3;
		}
		if (GUILayout.Toggle(num == 4, "spec"))
		{
			num = 4;
		}
		if (GUILayout.Toggle(num == 5, "smask"))
		{
			num = 5;
		}
		if (GUILayout.Toggle(num == 6, "nrm"))
		{
			num = 6;
		}
		if (GUILayout.Toggle(num == 7, "alb"))
		{
			num = 7;
		}
		if (GUILayout.Toggle(num == 8, "ao"))
		{
			num = 8;
		}
		if (GUILayout.Toggle(num == 9, "gloss"))
		{
			num = 9;
		}
		if (num != m_ShaderDebug)
		{
			Shader.SetGlobalInt("g_Debug", num);
			m_ShaderDebug = num;
		}
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Boost Ambient!!!!", GUILayout.Height(40f)))
		{
			RenderSettings.ambientLight = Color.white * 2f;
		}
		if (GUILayout.Button("DumpLightProbes", GUILayout.Height(40f)))
		{
			LightProbes lightProbes = LightmapSettings.lightProbes;
			int num2 = lightProbes.bakedProbes.Length;
			for (int i = 0; num2 > i * 27; i++)
			{
				string text = "LP(" + i + "):";
				int num3 = i * 27;
				for (int j = 0; j < 9; j++)
				{
					if (num3 + 2 >= lightProbes.bakedProbes.Length)
					{
						break;
					}
					string text2 = text;
					text = text2 + "{" + lightProbes.bakedProbes[num3].ToString() + ", " + lightProbes.bakedProbes[num3 + 1].ToString() + ", " + lightProbes.bakedProbes[num3 + 2].ToString() + "},\u00a0";
					num3 += 3;
				}
			}
		}
		GUILayout.EndVertical();
	}

	private static void KillTheLights()
	{
		Light[] array = Object.FindObjectsOfType<Light>();
		Light[] array2 = array;
		foreach (Light light in array2)
		{
			if (light.type != LightType.Directional)
			{
				Object.Destroy(light);
			}
			else
			{
				light.enabled = false;
			}
		}
	}

	public void FindAndDisplayGameObject(string _name)
	{
		GameObject gameObject = GameObject.Find(_name);
		if (!gameObject)
		{
			return;
		}
		int childCount = gameObject.transform.childCount;
		bool flag = childCount > 0 && gameObject.transform.GetChild(0).gameObject.activeSelf;
		bool flag2 = flag;
		flag = GUILayout.Toggle(flag, _name, "Button", GUILayout.Height(40f));
		if (flag2 != flag)
		{
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(i).gameObject;
				gameObject2.SetActive(flag);
			}
		}
	}
}
