using System;
using System.Collections.Generic;
using AcierProtocol;
using SharedBaseLib.Core;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuRendering : DebugMenuItem
	{
		private enum ReplacementMode
		{
			Original = 0,
			_16x16_truecolor = 1,
			_16x16_dxt1 = 2,
			_16x16_ehc = 3,
			_16x16_pvrtc = 4
		}

		private bool m_fpsEnabled;

		private bool m_collidersRedGreenEnabled;

		private bool m_collidersOpaqueEnabled;

		private bool m_postprocessingEnabled;

		private bool m_debugShadersEnabled;

		private bool m_fpsLimitEnabled;

		private FPS m_fps;

		private string m_visibilityTargetName = string.Empty;

		private float m_maxFar = 1200f;

		private float m_currentFar = 1f;

		private BBList<Transform> m_rootInactiveTransforms = new BBList<Transform>();

		private Mesh m_mesh;

		private Material m_opaqueMat;

		private Material m_clearMat;

		private Material m_frontMat;

		private Material m_backMat;

		private static Rect ms_windowRect = new Rect(20f, 20f, 120f, 50f);

		private static Vector2 ms_scrollPosition = Vector2.zero;

		private ReplacementMode m_replacementMode;

		private BBDictionary<Renderer, Material[]> m_restoreMaterials;

		public override string GetName()
		{
			return "Rendering";
		}

		public override void OnMenu()
		{
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			OnMenuSettings();
			OnMenuShaderLOD();
			OnMenuShadow();
			OnMenuPostEffects();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			OnMenuCamera();
			OnMenuClearColor();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			OnMenuVisibility();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private void OnMenuSettings()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Settings");
			OnMenuToggleFPS();
			OnMenuToggleColliders();
			OnMenuShaderReplacement();
			OnMenuLimitFPS();
			float lodBias = UnityEngine.QualitySettings.lodBias;
			GUILayout.Label("LOD Bias: " + lodBias * 100f + "%");
			GUI.changed = false;
			lodBias = GUILayout.HorizontalSlider(lodBias, 0.25f, 8f);
			if (GUI.changed)
			{
				UnityEngine.QualitySettings.lodBias = lodBias;
			}
			string[] names = UnityEngine.QualitySettings.names;
			int qualityLevel = UnityEngine.QualitySettings.GetQualityLevel();
			GUI.changed = false;
			int index = GUILayout.SelectionGrid(qualityLevel, names, 2);
			if (GUI.changed)
			{
				UnityEngine.QualitySettings.SetQualityLevel(index, true);
				GameObject.FindGameObjectWithTag("Main").BroadcastMessage("OnQualitySettingsChanged");
			}
			GUILayout.EndVertical();
		}

		private void OnMenuClearColor()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Skybox");
			RenderingSkyMesh renderingSkyMesh = UnityEngine.Object.FindObjectOfType<RenderingSkyMesh>();
			if (renderingSkyMesh != null)
			{
				Renderer component = renderingSkyMesh.GetComponent<Renderer>();
				if (GUILayout.Button((component.enabled ? "Hide" : "Show") + " Skybox renderer"))
				{
					component.enabled = !component.enabled;
				}
			}
			string[] texts = new string[4] { "default", "black", "cyan", "magenta" };
			int @int = PlayerPrefs.GetInt("__ClearColor", 0);
			GUI.changed = false;
			@int = GUILayout.SelectionGrid(@int, texts, 2);
			if (GUI.changed)
			{
				PlayerPrefs.SetInt("__ClearColor", @int);
				SharedMain.ApplyDefaultClearColor(Camera.main);
			}
			GUILayout.EndVertical();
		}

		private void OnMenuShadow()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Shadows");
			Light sceneLight = Rendering.Instance.Lighting.SceneLight;
			if (sceneLight != null)
			{
				float shadowBias = sceneLight.shadowBias;
				GUILayout.Label("Shadow Bias: " + shadowBias.ToString("0.00") + " units");
				GUI.changed = false;
				shadowBias = GUILayout.HorizontalSlider(shadowBias, 0f, 0.07f);
				if (GUI.changed)
				{
					sceneLight.shadowBias = shadowBias;
				}
			}
			float shadowDistance = UnityEngine.QualitySettings.shadowDistance;
			GUILayout.Label("Shadow Distance: " + shadowDistance.ToString("0.0") + " units");
			GUI.changed = false;
			shadowDistance = GUILayout.HorizontalSlider(shadowDistance, 0f, 60f);
			if (GUI.changed)
			{
				UnityEngine.QualitySettings.shadowDistance = shadowDistance;
				GameObject.FindGameObjectWithTag("Main").BroadcastMessage("OnQualitySettingsChanged");
			}
			string[] texts = new string[3] { "No Cascades", "2 Cascades", "4 Cascades" };
			int shadowCascades = UnityEngine.QualitySettings.shadowCascades;
			shadowCascades--;
			if (shadowCascades == 3)
			{
				shadowCascades = 2;
			}
			GUI.changed = false;
			shadowCascades = GUILayout.SelectionGrid(shadowCascades, texts, 2);
			if (GUI.changed)
			{
				if (shadowCascades == 2)
				{
					shadowCascades = 3;
				}
				UnityEngine.QualitySettings.shadowCascades = 1 + shadowCascades;
			}
			GUILayout.Label("Projection");
			ShadowProjection proj = UnityEngine.QualitySettings.shadowProjection;
			texts = Enum.GetNames(typeof(ShadowProjection));
			int selected = Array.FindIndex(texts, (string name) => name == proj.ToString());
			selected = GUILayout.SelectionGrid(selected, texts, 2);
			if (selected != (int)proj)
			{
				UnityEngine.QualitySettings.shadowProjection = (ShadowProjection)selected;
			}
			GUILayout.EndVertical();
		}

		private void OnMenuCamera()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Camera");
			if (Camera.main != null)
			{
				OnMenuToggleOcclusionCulling();
				Camera main = Camera.main;
				float fieldOfView = main.fieldOfView;
				GUILayout.Label("Field Of View: " + fieldOfView + "?");
				GUI.changed = false;
				fieldOfView = GUILayout.HorizontalSlider(fieldOfView, 50f, 90f);
				if (GUI.changed)
				{
					main.fieldOfView = fieldOfView;
				}
				GUILayout.Label("Near Clip: ");
				m_currentFar = GUILayout.HorizontalSlider(m_currentFar, 0f, 1f);
				main.farClipPlane = Mathf.Lerp(main.nearClipPlane + 0.1f, m_maxFar, m_currentFar);
				float nearClipPlane = Camera.main.nearClipPlane;
				float farClipPlane = Camera.main.farClipPlane;
				GUILayout.Label("Near: " + nearClipPlane + "m Far: " + farClipPlane + "m");
			}
			else
			{
				GUILayout.Label("Cannot find a camera tagged with \"MainCamera\" in the scene. Some options were hidden.");
			}
			GUILayout.EndVertical();
		}

		private void OnMenuShaderReplacement()
		{
			GUI.changed = false;
			Toggle(ref m_debugShadersEnabled, "Replace Shaders with debug versions");
			if (GUI.changed)
			{
				ToggleShaderDebugVersions(m_debugShadersEnabled);
			}
		}

		private void OnMenuLimitFPS()
		{
			GUI.changed = false;
			Toggle(ref m_fpsLimitEnabled, "Limit FPS 30");
			if (GUI.changed)
			{
				Application.targetFrameRate = (m_fpsLimitEnabled ? 30 : (-1));
			}
		}

		private void OnMenuToggleFPS()
		{
			GUI.changed = false;
			Toggle(ref m_fpsEnabled, "Display FPS");
			if (GUI.changed)
			{
				ToggleFpsDisplay();
			}
		}

		private void OnMenuToggleColliders()
		{
			GUI.changed = false;
			Toggle(ref m_collidersRedGreenEnabled, "Display Red/Green Colliders");
			Toggle(ref m_collidersOpaqueEnabled, "Display Opaque Colliders");
			RefreshEnabled();
		}

		private void RefreshEnabled()
		{
			base.enabled = m_collidersRedGreenEnabled || m_collidersOpaqueEnabled || m_postprocessingEnabled;
		}

		private void OnMenuToggleOcclusionCulling()
		{
			Camera main = Camera.main;
			if (!(main == null))
			{
				bool value = Camera.main.useOcclusionCulling;
				GUI.changed = false;
				Toggle(ref value, "Use Occlusion Culling");
				if (GUI.changed)
				{
					main.useOcclusionCulling = value;
				}
			}
		}

		private void StoreMaterials()
		{
			if (m_restoreMaterials == null)
			{
				m_restoreMaterials = new BBDictionary<Renderer, Material[]>();
				Renderer[] array = UnityEngine.Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
				foreach (Renderer renderer in array)
				{
					m_restoreMaterials.Add(renderer, renderer.sharedMaterials);
				}
			}
		}

		private void RestoreMaterials()
		{
			foreach (KeyValuePair<Renderer, Material[]> restoreMaterial in m_restoreMaterials)
			{
				restoreMaterial.Key.sharedMaterials = restoreMaterial.Value;
			}
			m_restoreMaterials = null;
		}

		private void ReplaceMaterials(string shaderName, string textureResourceName)
		{
			StoreMaterials();
			Renderer[] obj = UnityEngine.Object.FindObjectsOfType(typeof(Renderer)) as Renderer[];
			BBDictionary<string, Material> bBDictionary = new BBDictionary<string, Material>();
			Renderer[] array = obj;
			foreach (Renderer renderer in array)
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					string key = sharedMaterials[j].name + sharedMaterials[j].shader.name + sharedMaterials[j].ToString();
					if (bBDictionary.ContainsKey(key))
					{
						sharedMaterials[j] = bBDictionary[key];
						continue;
					}
					Texture2D texture2D = UnityEngine.Resources.Load(textureResourceName, typeof(Texture2D)) as Texture2D;
					Shader shader = (string.IsNullOrEmpty(shaderName) ? sharedMaterials[j].shader : Shader.Find(shaderName));
					Material material = new Material(shader);
					material.name = sharedMaterials[j].name;
					material.mainTexture = texture2D;
					if (material.HasProperty("_BumpMap"))
					{
						material.SetTexture("_BumpMap", texture2D);
					}
					if (material.HasProperty("_SpecTex"))
					{
						material.SetTexture("_SpecTex", texture2D);
					}
					bBDictionary.Add(key, material);
					sharedMaterials[j] = material;
				}
				renderer.sharedMaterials = sharedMaterials;
			}
		}

		private void OnMenuVisibility()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Visibility");
			m_visibilityTargetName = GUILayout.TextField(m_visibilityTargetName);
			GameObject gameObject = GameObject.Find(m_visibilityTargetName);
			if (gameObject != null)
			{
				Renderer component = gameObject.GetComponent<Renderer>();
				if ((bool)component)
				{
					component.useLightProbes = GUILayout.Toggle(component.useLightProbes, "Use lightprobes");
				}
				if (GUILayout.Button("Leave"))
				{
					if (gameObject.transform.parent != null)
					{
						VisibilitySetTargetName(gameObject.transform.parent);
					}
					else
					{
						m_visibilityTargetName = string.Empty;
					}
				}
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					Transform child = gameObject.transform.GetChild(i);
					OnMenuToggleParentTransform(child);
				}
			}
			else
			{
				Transform[] array = UnityEngine.Object.FindObjectsOfType<Transform>();
				foreach (Transform transform in array)
				{
					if (transform.parent == null)
					{
						OnMenuToggleParentTransform(transform);
					}
				}
				foreach (Transform item in new BBList<Transform>(m_rootInactiveTransforms))
				{
					if (item.parent == null)
					{
						OnMenuToggleParentTransform(item);
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			GUILayout.Label("Toggle Rendering Of Subsystems");
			GameObject gameObject2 = GameObject.Find("Camera Icon UI");
			Camera component2 = gameObject2.GetComponent<Camera>();
			if (gameObject2 != null && GUILayout.Button("Toggle UI " + (component2.enabled ? "Off" : "On")))
			{
				component2.enabled = !component2.enabled;
			}
			gameObject2 = GameObject.Find("BakeStatic");
			if (gameObject2 != null && GUILayout.Button("Disable BakeStatic " + (gameObject2.activeSelf ? "Off" : "On")))
			{
				GameObject obj = gameObject2;
				obj.SetActive(!obj.activeSelf);
			}
			gameObject2 = GameObject.Find("BakeScenario");
			if (gameObject2 != null && GUILayout.Button("Disable BakeScenario " + (gameObject2.activeSelf ? "Off" : "On")))
			{
				GameObject obj2 = gameObject2;
				obj2.SetActive(!obj2.activeSelf);
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			GUILayout.Label("Replace Materials");
			string[] names = Enum.GetNames(typeof(ReplacementMode));
			int selected = Array.FindIndex(names, (string name) => name == m_replacementMode.ToString());
			selected = GUILayout.SelectionGrid(selected, names, 2);
			if (selected != (int)m_replacementMode)
			{
				m_replacementMode = (ReplacementMode)selected;
				switch (m_replacementMode)
				{
				case ReplacementMode.Original:
					RestoreMaterials();
					break;
				case ReplacementMode._16x16_truecolor:
					ReplaceMaterials(null, "Debug/checkered_truecolor_16px");
					break;
				case ReplacementMode._16x16_dxt1:
					ReplaceMaterials(null, "Debug/checkered_dxt1_16px");
					break;
				case ReplacementMode._16x16_ehc:
					ReplaceMaterials(null, "Debug/checkered_etc_16px");
					break;
				case ReplacementMode._16x16_pvrtc:
					ReplaceMaterials(null, "Debug/checkered_pvrtc_16px");
					break;
				}
			}
			GUILayout.EndVertical();
		}

		private void OnMenuShaderLOD()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("ShaderLOD");
			RenderingSettings instance = RenderingSettings.Instance;
			int num = RenderingSettings.FloorShaderLOD(instance.shaderLOD);
			EQualityShaderLOD currentShaderLOD = (EQualityShaderLOD)num;
			string[] names = Enum.GetNames(typeof(EQualityShaderLOD));
			BBList<string> bBList = new BBList<string>(names);
			bBList.RemoveAt(0);
			names = bBList.ToArray();
			int selected = Array.FindIndex(names, (string name) => name == currentShaderLOD.ToString());
			selected = GUILayout.SelectionGrid(selected, names, 2);
			num = (int)Enum.Parse(typeof(EQualityShaderLOD), names[selected]);
			if (GUILayout.Toggle(instance.shaderLOD % 100 != 0, "Force Android"))
			{
				num--;
			}
			GUILayout.Label("maximumShaderLOD: " + num);
			instance.shaderLOD = num;
			GUILayout.EndVertical();
		}

		private void OnMenuPostEffects()
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Postprocessing");
			Toggle(ref m_postprocessingEnabled, "OnScreen Menu");
			RefreshEnabled();
			GameObject gameObject = GameObject.Find("Postprocessing");
			if (gameObject != null)
			{
				PostprocessingSettings[] componentsInChildren = gameObject.GetComponentsInChildren<PostprocessingSettings>(true);
				foreach (PostprocessingSettings postprocessingSettings in componentsInChildren)
				{
					bool value = postprocessingSettings.gameObject.activeSelf;
					GUI.changed = false;
					Toggle(ref value, postprocessingSettings.GetType().ToString());
					if (GUI.changed)
					{
						postprocessingSettings.gameObject.SetActive(value);
						Camera main = Camera.main;
						if (main != null)
						{
							Rendering.RefreshPostEffects(main);
						}
						CombinedPostEffect combinedPostEffect = ((main != null) ? main.GetComponent<CombinedPostEffect>() : null);
						if (combinedPostEffect != null)
						{
							combinedPostEffect.Refresh();
						}
					}
				}
			}
			GUILayout.EndVertical();
		}

		private void OnMenuToggleParentTransform(Transform t)
		{
			bool value = t.gameObject.activeInHierarchy;
			GUILayout.BeginHorizontal();
			Toggle(ref value, t.name);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Enter", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f)))
			{
				VisibilitySetTargetName(t);
			}
			GUILayout.EndHorizontal();
			if (t.parent == null && !value && value != t.gameObject.activeInHierarchy)
			{
				m_rootInactiveTransforms.Add(t);
			}
			if (t.parent == null && value && value != t.gameObject.activeInHierarchy)
			{
				m_rootInactiveTransforms.Remove(t);
			}
			t.gameObject.SetActive(value);
		}

		private void VisibilitySetTargetName(Transform t)
		{
			string text = t.name;
			Transform parent = t.parent;
			while (parent != null)
			{
				text = parent.name + "/" + text;
				parent = parent.parent;
			}
			m_visibilityTargetName = text;
		}

		private static void OnMenu(string _desc, ref Vector4 _c, float _min, float _max)
		{
			GUILayout.BeginVertical();
			GUILayout.Label(_desc);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Label("R: " + _c.x);
			_c.x = GUILayout.HorizontalSlider(_c.x, _min, _max, GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Label("G: " + _c.y);
			_c.y = GUILayout.HorizontalSlider(_c.y, _min, _max, GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Label("B: " + _c.z);
			_c.z = GUILayout.HorizontalSlider(_c.z, _min, _max, GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private static void OnMenu(string _desc, ref float _s, float _min, float _max)
		{
			GUILayout.BeginVertical();
			GUILayout.Label(_desc);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.Label(_s.ToString());
			_s = GUILayout.HorizontalSlider(_s, _min, _max, GUILayout.ExpandWidth(true));
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}

		private static void OnMenu(string _desc, ref int _i, int _min, int _max)
		{
			GUILayout.BeginVertical();
			GUILayout.Label(_desc);
			GUILayout.BeginVertical();
			GUILayout.Label(_i.ToString());
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				_i++;
			}
			if (GUILayout.Button("-"))
			{
				_i--;
			}
			_i = Mathf.Clamp(_i, _min, _max);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndVertical();
		}

		private static void OnMenuBloomSettings(BloomSettings bs)
		{
			GUILayout.BeginVertical("box");
			GUILayout.Label("Bloom");
			GUILayout.BeginVertical();
			OnMenu("Threshold", ref bs.threshold, 0f, 1f);
			OnMenu("Intensity", ref bs.intensity, 0f, 20f);
			OnMenu("Exponent", ref bs.exponent, 0f, 5f);
			OnMenu("Blur Size", ref bs.blurSize, 0f, 8f);
			OnMenu("Iterations", ref bs.blurIterations, 1, 4);
			GUILayout.EndVertical();
			GUILayout.EndVertical();
		}

		private static void OnMenuDesaturationSettings(DesaturationSettings ds)
		{
			GUILayout.BeginVertical("box");
			OnMenu("Desaturation", ref ds.desaturation, 0f, 1f);
			GUILayout.EndVertical();
		}

		private static void PostprocessingWindow(int _windowId)
		{
			ms_scrollPosition = GUILayout.BeginScrollView(ms_scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			BloomSettings bloomSettings = UnityEngine.Object.FindObjectOfType<BloomSettings>();
			if (bloomSettings != null)
			{
				OnMenuBloomSettings(bloomSettings);
			}
			DesaturationSettings desaturationSettings = UnityEngine.Object.FindObjectOfType<DesaturationSettings>();
			if (desaturationSettings != null)
			{
				OnMenuDesaturationSettings(desaturationSettings);
			}
			GUILayout.EndScrollView();
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
		}

		private void OnGUI()
		{
			if (m_postprocessingEnabled)
			{
				float width = (float)Screen.width * 0.5f;
				float height = (float)Screen.height * 0.5f;
				ms_windowRect.width = width;
				ms_windowRect.height = height;
				ms_windowRect = GUI.Window(0, ms_windowRect, PostprocessingWindow, "Postprocessing");
				Camera main = Camera.main;
				if (main != null)
				{
					Rendering.RefreshPostEffects(main);
				}
			}
		}

		public override void OnOpen()
		{
			m_fpsEnabled = m_fps != null && m_fps.gameObject.activeSelf;
		}

		public override void OnApply()
		{
			ToggleFpsDisplay();
		}

		private void Start()
		{
			ToggleFpsDisplay();
			base.enabled = false;
		}

		private void LateUpdate()
		{
			if (m_collidersOpaqueEnabled)
			{
				if (m_opaqueMat == null)
				{
					m_opaqueMat = new Material(Shader.Find("Hidden/Debug/MeshCollider (Opaque)"));
				}
				MeshCollider[] array = UnityEngine.Object.FindObjectsOfType<MeshCollider>();
				foreach (MeshCollider meshCollider in array)
				{
					Graphics.DrawMesh(meshCollider.sharedMesh, meshCollider.transform.localToWorldMatrix, m_opaqueMat, 0);
				}
			}
			if (m_collidersRedGreenEnabled)
			{
				MeshCollider[] array2 = UnityEngine.Object.FindObjectsOfType<MeshCollider>();
				if (m_frontMat == null)
				{
					m_frontMat = new Material(Shader.Find("Hidden/Debug/MeshCollider (Front)"));
				}
				MeshCollider[] array = array2;
				foreach (MeshCollider meshCollider2 in array)
				{
					Graphics.DrawMesh(meshCollider2.sharedMesh, meshCollider2.transform.localToWorldMatrix, m_frontMat, 0);
				}
				if (m_clearMat == null)
				{
					m_clearMat = new Material(Shader.Find("Hidden/Debug/MeshCollider (Clear Stencil)"));
				}
				if (m_backMat == null)
				{
					m_backMat = new Material(Shader.Find("Hidden/Debug/MeshCollider (Back)"));
				}
				if (m_mesh == null)
				{
					m_mesh = new Mesh();
					Vector3[] vertices = new Vector3[4]
					{
						new Vector3(-1f, -1f, 0f),
						new Vector3(1f, -1f, 0f),
						new Vector3(-1f, 1f, 0f),
						new Vector3(1f, 1f, 0f)
					};
					int[] triangles = new int[6] { 2, 1, 0, 1, 2, 3 };
					m_mesh.vertices = vertices;
					m_mesh.subMeshCount = 1;
					m_mesh.SetTriangles(triangles, 0);
				}
				Camera main = Camera.main;
				if ((bool)main)
				{
					float num = main.nearClipPlane + 0.1f;
					float num2 = Mathf.Tan(main.fieldOfView * ((float)Math.PI / 180f) * 0.5f) * num;
					Vector3 pos = main.transform.position + main.transform.rotation * new Vector3(0f, 0f, num);
					Quaternion rotation = main.transform.rotation;
					Vector3 s = new Vector3(num2 * main.aspect, num2, 1f);
					Matrix4x4 matrix = Matrix4x4.TRS(pos, rotation, s);
					Graphics.DrawMesh(m_mesh, matrix, m_clearMat, 0);
					Graphics.DrawMesh(m_mesh, matrix, m_backMat, 0);
				}
			}
		}

		private void ToggleShaderDebugVersions(bool _replaceWithDebug)
		{
			Renderer[] array = UnityEngine.Object.FindObjectsOfType<Renderer>();
			foreach (Renderer renderer in array)
			{
				for (int j = 0; j < renderer.sharedMaterials.Length; j++)
				{
					Material material = renderer.sharedMaterials[j];
					if (material != null && _replaceWithDebug)
					{
						Shader shader = Shader.Find(material.shader.name + "_Debug");
						if (shader != null)
						{
							material.shader = shader;
						}
					}
					else if (material != null && !_replaceWithDebug && material.shader.name.Contains("_Debug"))
					{
						Shader shader2 = Shader.Find(material.shader.name.Replace("_Debug", ""));
						if (shader2 != null)
						{
							material.shader = shader2;
						}
					}
				}
			}
		}

		private void ToggleFpsDisplay()
		{
			if (m_fpsEnabled)
			{
				if (m_fps == null)
				{
					CreateFrameRate();
				}
				m_fps.gameObject.SetActive(true);
			}
			else if (m_fps != null)
			{
				m_fps.gameObject.SetActive(false);
			}
		}

		private void CreateFrameRate()
		{
			GameObject gameObject = new GameObject("FPS");
			gameObject.transform.position = new Vector3(0.5f, 0.975f, 0f);
			UnityEngine.Resources.FindObjectsOfTypeAll(typeof(Font));
			m_fps = gameObject.AddComponent<FPS>();
			gameObject.transform.parent = base.transform;
		}
	}
}
