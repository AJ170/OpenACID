using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Humanoids
{
	public class HumanoidFactory : Singleton<HumanoidFactory>
	{
		private class PreloadCacheEntry
		{
			public RuntimeAnimatorController Controller;

			public GameObject Go;

			public PreloadCacheEntry(RuntimeAnimatorController _controller, GameObject _go)
			{
				Controller = _controller;
				Go = _go;
			}
		}

		private GameDB m_GameDb;

		private GameObject m_CacheBase;

		private BBDictionary<int, PreloadCacheEntry> m_PreloadCache;

		private GameObject CacheBase
		{
			get
			{
				if (m_CacheBase == null)
				{
					m_CacheBase = GameObject.Find("CharacterCacheBase");
					if (m_CacheBase == null)
					{
						m_CacheBase = new GameObject("CharacterCacheBase");
					}
				}
				return m_CacheBase;
			}
		}

		public HumanoidFactory()
		{
			Reset();
		}

		public void EmptyCache()
		{
			if (m_PreloadCache != null)
			{
				foreach (int key in m_PreloadCache.Keys)
				{
					PreloadCacheEntry value;
					if (m_PreloadCache.TryGetValue(key, out value))
					{
						Object.Destroy(value.Go);
					}
				}
				m_PreloadCache.Clear();
			}
			m_PreloadCache = null;
		}

		public void Reset()
		{
			SetupGameDb();
			EmptyCache();
			m_PreloadCache = new BBDictionary<int, PreloadCacheEntry>();
			if ((bool)m_CacheBase)
			{
				m_CacheBase.name = "DeletedCacheBase";
				Object.Destroy(m_CacheBase);
			}
			m_CacheBase = null;
		}

		public bool PreCacheHumanoidFromType(NpcData.NpcType _npcType, GameID _overridePrefab)
		{
			NpcData.INpcDefinition definition = UbiCore.Utils.Singleton<NpcData>.Instance.GetDefinition(_npcType);
			HumanoidDef item = Main.Instance.GameDB.GetItem<HumanoidDef>(definition.HumanoidGameID);
			return PreCacheHumanoidFromDefinition(item, _overridePrefab);
		}

		public bool PreCacheHumanoidFromDB(GameID _humandoidGameID, GameID _overridePrefab)
		{
			HumanoidDef item = m_GameDb.GetItem<HumanoidDef>(_humandoidGameID);
			if (item == null)
			{
				m_GameDb.Dump();
				return false;
			}
			return PreCacheHumanoidFromDefinition(item, _overridePrefab);
		}

		public bool PreCacheHumanoidFromDefinition(HumanoidDef _def, GameID _overridePrefab)
		{
			int humanoidHash = GetHumanoidHash(_def, null, _overridePrefab);
			if (m_PreloadCache.ContainsKey(humanoidHash))
			{
				return true;
			}
			GameObject gameObject = CreateHumanoidInternal(_def, null, _overridePrefab);
			if (gameObject == null)
			{
				return false;
			}
			gameObject.SetActive(false);
			gameObject.transform.ParentTo(CacheBase);
			Animator component = gameObject.GetComponent<Animator>();
			RuntimeAnimatorController controller = null;
			if (component != null)
			{
				controller = component.runtimeAnimatorController;
				component.runtimeAnimatorController = null;
			}
			PreloadCacheEntry value = new PreloadCacheEntry(controller, gameObject);
			m_PreloadCache.Add(humanoidHash, value);
			return true;
		}

		public void RandomizeHumanoid(HumanoidDef _def, ICharacterAppearance _appearance, bool _chooseFirstColor)
		{
			HumanoidFactoryHelpers.PreloadEditorData();
			RandomizeHead(_def, _appearance);
			RandomizeAccessoire(_def, _appearance);
			RandomizeColor(_def, _appearance, _chooseFirstColor);
		}

		public void RandomizeHead(HumanoidDef _def, ICharacterAppearance _appearance)
		{
			_appearance.HeadDefinition = _def.GetHeadDef();
			_appearance.HeadMaterials = ((_appearance.HeadDefinition == null) ? null : _appearance.HeadDefinition.GetHeadMaterial(_def.RandomHeadMaterial));
		}

		public void RandomizeAccessoire(HumanoidDef _def, ICharacterAppearance _appearance)
		{
			if (_def.AccessoryTable != null && _def.GetAccessoryDef() != null)
			{
				_appearance.HeadAccessoireResource = _def.GetAccessoryDef().GetRandomHeadAccessoire();
			}
			else
			{
				_appearance.HeadAccessoireResource = null;
			}
		}

		public void RandomizeColor(HumanoidDef _def, ICharacterAppearance _appearance, bool _chooseFirstColor)
		{
			if (_def.RandomColor)
			{
				if (_def.ColorTable != null)
				{
					ColorTableDef item = m_GameDb.GetItem<ColorTableDef>(_def.ColorTable);
					if (item != null)
					{
						if (_chooseFirstColor)
						{
							ColorDef item2 = m_GameDb.GetItem<ColorDef>(item.Colors[0]);
							_appearance.Color0 = item2.Color0;
							_appearance.Color1 = item2.Color1;
						}
						else
						{
							Color _color;
							Color _color2;
							item.GetRandomColors(m_GameDb, out _color, out _color2);
							_appearance.Color0 = _color;
							_appearance.Color1 = _color2;
						}
						return;
					}
				}
				_appearance.Color0 = Color.magenta;
				_appearance.Color1 = Color.magenta;
			}
			else
			{
				_appearance.Color0 = _def.BodyColor0;
				_appearance.Color1 = _def.BodyColor1;
			}
		}

		public IEnumerator PreloadHumanoidFromDefinitionAsync(HumanoidDef _def, ICharacterAppearance _appearance)
		{
			yield return Main.Instance.StartCoroutine(_def.Body.PrepareAsync());
			HeadDef head_def = GetHeadDef(_def, _appearance);
			if (head_def != null && head_def.BaseHead != null)
			{
				yield return Main.Instance.StartCoroutine(head_def.BaseHead.PrepareAsync());
			}
			yield return Main.Instance.StartCoroutine(_appearance.HeadMaterials.Material.PrepareAsync());
		}

		public GameObject CreateHumanoidFromDB(GameID _humandoidGameID, bool _receiveShadows, ICharacterAppearance _appearance, string _nameAndTag = null, GameID _overridePrefab = null)
		{
			HumanoidDef item = m_GameDb.GetItem<HumanoidDef>(_humandoidGameID);
			if (item == null)
			{
				m_GameDb.Dump();
				return null;
			}
			return CreateHumanoidFromDefinition(item, _receiveShadows, _appearance, _nameAndTag, _overridePrefab);
		}

		public GameObject CreateHumanoidFromDefinition(HumanoidDef _def, bool _receiveShadows, ICharacterAppearance _appearance, string _nameAndTag = null, GameID _overridePrefab = null)
		{
			Profiling.Start("HumanoidFactory::CreateHumanoidFromDefinition", float.MaxValue);
			if (_appearance == null)
			{
				Profiling.Start("Setting Appearance", float.MaxValue);
				CharacterAppearance characterAppearance = new CharacterAppearance();
				RandomizeHumanoid(_def, characterAppearance, true);
				_appearance = characterAppearance;
				Profiling.Stop();
			}
			HumanoidFactoryHelpers.PreloadEditorData();
			EEra age = _def.Age;
			int humanoidHash = GetHumanoidHash(_def, _appearance, _overridePrefab);
			PreloadCacheEntry value;
			GameObject gameObject;
			if (!m_PreloadCache.TryGetValue(humanoidHash, out value))
			{
				gameObject = CreateHumanoidInternal(_def, _appearance, _overridePrefab, _receiveShadows);
			}
			else
			{
				if (value.Go == null || value.Controller == null)
				{
				}
				Profiling.Start("Instantiate", float.MaxValue);
				gameObject = Object.Instantiate(value.Go);
				Profiling.Stop();
				Animator component = gameObject.GetComponent<Animator>();
				if (component != null)
				{
					component.runtimeAnimatorController = value.Controller;
				}
				AdjustAppearance(_def, _appearance, gameObject);
			}
			ApplyAccessoire(_appearance, _def, gameObject, _receiveShadows);
			SetTinting(_appearance, gameObject);
			gameObject.SetLayerRecursively(LayerMask.NameToLayer("Character"));
			Profiling.Start("Activate", float.MaxValue);
			if (gameObject.transform.position.DistanceSquared(new Vector3(0f, 0f, 0f)) < 1f)
			{
				gameObject.transform.position = HumanoidFactoryHelpers.FindValidNavMeshPosition(HumanoidFactoryHelpers.ms_LayerName);
			}
			gameObject.SetActive(true);
			Profiling.Stop();
			Profiling.Stop();
			return gameObject;
		}

		public GameObject AddHumanoidSkin(GameID _gameID, GameObject _target, ICharacterAppearance _appearance)
		{
			GameObject gameObject = CreateHumanoidFromDB(_gameID, false, _appearance);
			Transform transform = gameObject.transform.FindChild("Body_LOD0");
			if (transform != null)
			{
				transform.parent = _target.transform;
				transform.name = "Disguise";
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				GameObject gameObject2 = transform.gameObject;
				Object.Destroy(gameObject);
				Animator component = _target.GetComponent<Animator>();
				if (component != null)
				{
					component.Rebind();
				}
				return gameObject2;
			}
			return null;
		}

		public void DestroyHumanoid(GameObject _go)
		{
			if (Application.isPlaying)
			{
				Renderer[] componentsInChildren = _go.GetComponentsInChildren<Renderer>();
				if (componentsInChildren != null)
				{
					int num = componentsInChildren.Length;
					for (int i = 0; num > i; i++)
					{
						Material[] materials = componentsInChildren[i].materials;
						if (materials != null)
						{
							int num2 = materials.Length;
							for (int j = 0; num2 > j; j++)
							{
								Object.Destroy(materials[j]);
							}
						}
					}
				}
				Object.Destroy(_go);
			}
			else
			{
				Object.DestroyImmediate(_go);
			}
		}

		private void SetupGameDb()
		{
			m_GameDb = Main.SafeGameDb;
		}

		private GameObject PrepareBody(HumanoidDef _def, GameID _overridePrefab = null)
		{
			Profiling.Start("HumanoidFactory::PrepareBody", float.MaxValue);
			HumanoidInfo info = HumanoidData.GetInfo(_def.CharType);
			GameObject prefab = info.GetPrefab(_overridePrefab);
			GameObject @object = _def.Body.GetObject<GameObject>();
			GameObject gameObject = null;
			if (@object != null)
			{
				gameObject = Object.Instantiate(@object);
				HumanoidFactoryHelpers.CopyPrefabToTarget(prefab, gameObject);
			}
			if (prefab != null)
			{
				Object.DestroyImmediate(prefab);
			}
			Profiling.Stop();
			return gameObject;
		}

		private GameObject PrepareHead(HumanoidDef _def, ICharacterAppearance _appearance)
		{
			Profiling.Start("HumanoidFactory::PrepareHead", float.MaxValue);
			HeadDef headDef = GetHeadDef(_def, _appearance);
			GameObject result = null;
			if (headDef != null && headDef.BaseHead != null && headDef.BaseHead.GetObject<Object>() != null)
			{
				result = Object.Instantiate(headDef.BaseHead.AccessObject<GameObject>());
			}
			Profiling.Stop();
			return result;
		}

		private HeadDef GetHeadDef(HumanoidDef _def, ICharacterAppearance _appearance)
		{
			if (_appearance != null && _appearance.HeadDefinition != null)
			{
				return _appearance.HeadDefinition;
			}
			if (_def.Head != null)
			{
				return m_GameDb.GetItem<HeadDef>(_def.Head);
			}
			return null;
		}

		private int GetHumanoidHash(HumanoidDef _def, ICharacterAppearance _appearance = null, GameID _overridePrefab = null)
		{
			int hashCode = _def.GameID.GetHashCode();
			int num = 0;
			int num2 = 0;
			HeadDef headDef = GetHeadDef(_def, _appearance);
			if (headDef != null)
			{
				num = headDef.GameID.GetHashCode();
			}
			if (_overridePrefab != null)
			{
				num2 = _overridePrefab.GetHashCode();
			}
			return hashCode ^ num ^ num2;
		}

		private GameObject CreateHumanoidInternal(HumanoidDef _def, ICharacterAppearance _appearance = null, GameID _overridePrefab = null, bool _receiveShadows = false)
		{
			GameObject gameObject = PrepareBody(_def, _overridePrefab);
			if (gameObject == null)
			{
				return null;
			}
			GameObject gameObject2 = PrepareHead(_def, _appearance);
			Material headMaterial = null;
			if (gameObject2 != null)
			{
				Renderer componentInChildren = gameObject2.GetComponentInChildren<Renderer>();
				componentInChildren.receiveShadows = _receiveShadows;
				headMaterial = componentInChildren.sharedMaterial;
			}
			if (_appearance != null && _appearance.HeadMaterials != null)
			{
				Material @object = _appearance.HeadMaterials.Material.GetObject<Material>();
				if (@object != null)
				{
					headMaterial = @object;
				}
				else
				{
					Debug.LogError("Failed to load head material for " + _def.Name + " and " + _appearance.HeadDefinition.Name);
				}
			}
			if (gameObject2 != null)
			{
				CombineBodyAndHead(_def, gameObject, gameObject2, headMaterial, _receiveShadows);
			}
			else
			{
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				int num = componentsInChildren.Length;
				for (int i = 0; num > i; i++)
				{
					componentsInChildren[i].receiveShadows = _receiveShadows;
					SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i] as SkinnedMeshRenderer;
					skinnedMeshRenderer.sharedMaterial = new Material(HumanoidFactoryHelpers.MaterialOverride(skinnedMeshRenderer.sharedMaterial, _def.AtlasPage, _def.Age.Fix()));
					SplitColorMesh(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMesh.vertexCount);
				}
			}
			LODGroup component = gameObject.GetComponent<LODGroup>();
			if (component != null)
			{
				component.RecalculateBounds();
			}
			Object.DestroyImmediate(gameObject2);
			ApplyLightProbes(gameObject);
			return gameObject;
		}

		public static void ApplyLightProbes(GameObject _theHumanoid)
		{
			Animator component = _theHumanoid.GetComponent<Animator>();
			Transform bone = null;
			if (component != null)
			{
				bone = component.GetBoneTransform(HumanBodyBones.Chest);
			}
			Rendering.ConfigureLightProbes(_theHumanoid, bone);
		}

		private static void CombineBodyAndHead(HumanoidDef _def, GameObject the_body, GameObject the_head, Material _headMaterial, bool _receiveShadows = false)
		{
			Renderer[] componentsInChildren = the_body.GetComponentsInChildren<Renderer>();
			Renderer[] componentsInChildren2 = the_head.GetComponentsInChildren<Renderer>();
			Transform transform = the_body.transform;
			List<Material> list = new List<Material>();
			int num = componentsInChildren.Length;
			for (int i = 0; num > i; i++)
			{
				componentsInChildren[i].receiveShadows = _receiveShadows;
				componentsInChildren2[i].receiveShadows = _receiveShadows;
				SkinnedMeshRenderer skinnedMeshRenderer = componentsInChildren[i] as SkinnedMeshRenderer;
				SkinnedMeshRenderer skinnedMeshRenderer2 = componentsInChildren2[i] as SkinnedMeshRenderer;
				CombineInstance[] array = new CombineInstance[skinnedMeshRenderer.sharedMesh.subMeshCount + 1];
				int num2 = 0;
				for (int j = 0; skinnedMeshRenderer.sharedMesh.subMeshCount > j; j++)
				{
					array[num2].mesh = skinnedMeshRenderer.sharedMesh;
					array[num2].subMeshIndex = j;
					array[num2].transform = Matrix4x4.identity;
					num2++;
				}
				array[num2].mesh = skinnedMeshRenderer2.sharedMesh;
				array[num2].transform = Matrix4x4.identity;
				Mesh mesh = new Mesh();
				list.Clear();
				Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;
				int num3 = sharedMaterials.Length;
				for (int k = 0; num3 > k; k++)
				{
					list.Add(new Material(HumanoidFactoryHelpers.MaterialOverride(sharedMaterials[k], _def.AtlasPage, _def.Age.Fix())));
				}
				list.Add(new Material(HumanoidFactoryHelpers.MaterialOverride(_headMaterial, _def.AtlasPage, _def.Age.Fix())));
				bool flag = HumanoidFactoryHelpers.MaterialsCompatible(list);
				List<Transform> list2 = new List<Transform>();
				list2.AddRange(skinnedMeshRenderer.bones);
				list2.AddRange(skinnedMeshRenderer2.bones);
				mesh.CombineMeshes(array, flag);
				mesh.name = _def.Name + "_MergedBodyHead_LOD" + i;
				mesh.RecalculateBounds();
				SplitColorMesh(mesh, skinnedMeshRenderer.sharedMesh.vertexCount);
				Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
				skinnedMeshRenderer.sharedMesh = mesh;
				Transform[] array2 = new Transform[list2.Count];
				for (int l = 0; list2.Count > l; l++)
				{
					if (l < skinnedMeshRenderer.bones.Length)
					{
						array2[l] = list2[l];
					}
					else
					{
						array2[l] = HumanoidFactoryHelpers.FindNodeRecursive(transform, list2[l].name);
					}
				}
				skinnedMeshRenderer.bones = array2;
				if (flag)
				{
					skinnedMeshRenderer.sharedMaterial = list[0];
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					skinnedMeshRenderer.GetPropertyBlock(materialPropertyBlock);
					materialPropertyBlock.AddVector("_MainTex_Face", HumanoidFactoryHelpers.TextureOffset(list[1], "_MainTex"));
					materialPropertyBlock.AddVector("_BumpMap_Face", HumanoidFactoryHelpers.TextureOffset(list[1], "_BumpMap"));
					materialPropertyBlock.AddVector("_MaskTex_Face", HumanoidFactoryHelpers.TextureOffset(list[1], "_MaskTex"));
					skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
				}
				else
				{
					skinnedMeshRenderer.sharedMaterials = list.ToArray();
				}
				skinnedMeshRenderer.gameObject.name = "Body_LOD" + i;
			}
		}

		private static void SplitColorMesh(Mesh _mergedMesh, int _bodyVertexCount)
		{
			Color32[] array = ((_mergedMesh.colors32 != null && _mergedMesh.colors32.Length >= _mergedMesh.vertexCount) ? _mergedMesh.colors32 : new Color32[_mergedMesh.vertexCount]);
			int vertexCount = _mergedMesh.vertexCount;
			for (int i = 0; vertexCount > i; i++)
			{
				array[i].a = (byte)((i >= _bodyVertexCount) ? 255u : 0u);
			}
			_mergedMesh.colors32 = array;
		}

		private void SetTinting(ICharacterAppearance _appearance, GameObject _go)
		{
			Renderer[] componentsInChildren = _go.GetComponentsInChildren<Renderer>(true);
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				int num = renderer.materials.Length;
				Material[] materials = renderer.materials;
				for (int j = 0; j < num; j++)
				{
					Material material = materials[j];
					if (material != null && material.HasProperty("_Color0") && material.HasProperty("_Color1"))
					{
						materials[j].SetColor("_Color0", _appearance.Color0);
						materials[j].SetColor("_Color1", _appearance.Color1);
					}
				}
			}
		}

		private void ApplyAccessoire(ICharacterAppearance _appearance, HumanoidDef _def, GameObject _go, bool _receiveShadows)
		{
			if (_appearance.HeadAccessoireResource != null)
			{
				AccessoryDef accessoryDef = _def.GetAccessoryDef();
				Profiling.Start("HumanoidFactory::ApplyAccessoire()", float.MaxValue);
				GameObject gameObject = Object.Instantiate(_appearance.HeadAccessoireResource.GetObject<GameObject>());
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				int num = componentsInChildren.Length;
				for (int i = 0; i < num; i++)
				{
					Renderer renderer = componentsInChildren[i];
					renderer.receiveShadows = _receiveShadows;
					Material material = ((accessoryDef != null) ? HumanoidFactoryHelpers.MaterialOverride(renderer.sharedMaterial, accessoryDef.AtlasPage, accessoryDef.Age.Fix()) : renderer.sharedMaterial);
					renderer.sharedMaterial = ((!(material == renderer.sharedMaterial)) ? material : renderer.material);
				}
				Transform transform = HumanoidFactoryHelpers.FindNodeRecursive(_go.transform, "Head");
				if (transform != null)
				{
					gameObject.transform.parent = transform;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
				}
				ApplyLightProbes(_go);
				Profiling.Stop();
			}
		}

		internal void AdjustAppearance(HumanoidDef _def, ICharacterAppearance _appearance, GameObject _goHumanoid)
		{
			if (!(_goHumanoid != null))
			{
				return;
			}
			Profiling.Start("HumanoidFactory::AdjustAppearance()", float.MaxValue);
			Renderer[] componentsInChildren = _goHumanoid.GetComponentsInChildren<Renderer>(true);
			if (componentsInChildren != null)
			{
				int num = componentsInChildren.Length;
				for (int i = 0; num > i; i++)
				{
					Renderer renderer = componentsInChildren[i];
					bool flag = false;
					int num2 = renderer.materials.Length;
					for (int j = 0; j < num2; j++)
					{
						Material material = renderer.materials[j];
						if (material != null && material.HasProperty("_Color0") && material.HasProperty("_Color1"))
						{
							material.SetColor("_Color0", _appearance.Color0);
							material.SetColor("_Color1", _appearance.Color1);
						}
					}
					if (renderer.gameObject.name.Contains("Body_LOD"))
					{
						flag = true;
					}
					if (_appearance.HeadMaterials == null)
					{
						continue;
					}
					Material material2 = _appearance.HeadMaterials.Material.AccessObject<Material>();
					if (!(material2 != null))
					{
						continue;
					}
					Material material3 = HumanoidFactoryHelpers.MaterialOverride(material2, _def.AtlasPage, _def.Age.Fix());
					if (!(material3 != null) || !flag)
					{
						continue;
					}
					if (_def.AtlasPage != 0)
					{
						MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
						materialPropertyBlock.AddVector("_MainTex_Face", HumanoidFactoryHelpers.TextureOffset(material3, "_MainTex"));
						materialPropertyBlock.AddVector("_BumpMap_Face", HumanoidFactoryHelpers.TextureOffset(material3, "_BumpMap"));
						materialPropertyBlock.AddVector("_MaskTex_Face", HumanoidFactoryHelpers.TextureOffset(material3, "_MaskTex"));
						renderer.SetPropertyBlock(materialPropertyBlock);
					}
					else
					{
						Material[] sharedMaterials = renderer.sharedMaterials;
						if (sharedMaterials.Length > 0)
						{
							sharedMaterials[1] = material3;
						}
						renderer.sharedMaterials = sharedMaterials;
					}
				}
			}
			Profiling.Stop();
		}
	}
}
