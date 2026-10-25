using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UnityEngine;
using UnityEngine.AI;

namespace Humanoids
{
	public static class HumanoidFactoryHelpers
	{
		private class MaterialID
		{
			private static StringBuilder s_Stringbuilder = new StringBuilder(1024);

			private readonly uint m_hashCRC;

			private readonly uint m_source;

			private readonly HumanoidDef.EAtlasPage m_atlasPage;

			private readonly EEra m_age;

			internal string Name { get; private set; }

			internal MaterialID(string _source, HumanoidDef.EAtlasPage _atlasPage, EEra _age)
			{
				m_source = StringCRC32.Compute(_source);
				m_atlasPage = _atlasPage;
				m_age = _age;
				s_Stringbuilder.Length = 0;
				s_Stringbuilder.Append("Assets/RuntimeAssets/Character/Atlases/Materials");
				s_Stringbuilder.Append("/atlas_");
				s_Stringbuilder.Append(_age.ToListString(string.Empty));
				s_Stringbuilder.Append("_");
				s_Stringbuilder.Append(_atlasPage.ToString());
				s_Stringbuilder.Append("_");
				s_Stringbuilder.Append(Path.GetFileName(_source));
				if (!_source.EndsWith(".mat", StringComparison.OrdinalIgnoreCase))
				{
					s_Stringbuilder.Append(".mat");
				}
				Name = s_Stringbuilder.ToString().ToLower();
				m_hashCRC = StringCRC32.ComputeLowerCase(Name);
			}

			internal bool Equals(string _source, HumanoidDef.EAtlasPage _atlasPage, EEra _age)
			{
				return StringCRC32.Compute(_source) == m_source && m_atlasPage == _atlasPage && m_age == _age;
			}

			public override int GetHashCode()
			{
				return (int)m_hashCRC;
			}
		}

		public static bool isBuilder = false;

		private static readonly BBList<MaterialID> s_overrideNames = new BBList<MaterialID>();

		public static string ms_LayerName = "GroundSurface";

		private static BBDictionary<GameID, GameObject> m_GoCache = new BBDictionary<GameID, GameObject>();

		public static void CopyCapsuleCollider(CapsuleCollider _source, GameObject _dest)
		{
			CapsuleCollider capsuleCollider = _dest.AddComponent<CapsuleCollider>();
			capsuleCollider.center = _source.center;
			capsuleCollider.radius = _source.radius;
			capsuleCollider.height = _source.height;
			capsuleCollider.direction = _source.direction;
		}

		public static void CopyRigidbody(Rigidbody _source, GameObject _dest)
		{
			Rigidbody rigidbody = _dest.AddComponent<Rigidbody>();
			rigidbody.mass = _source.mass;
			rigidbody.drag = _source.drag;
			rigidbody.angularDrag = _source.angularDrag;
			rigidbody.useGravity = _source.useGravity;
			rigidbody.isKinematic = _source.isKinematic;
			rigidbody.interpolation = _source.interpolation;
			rigidbody.collisionDetectionMode = _source.collisionDetectionMode;
			rigidbody.constraints = _source.constraints;
		}

		public static Vector3 FindValidNavMeshPosition(string _layerName)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(new Vector3(0f, 0f, 0f), out hit, 1000f, 1 << NavMesh.GetAreaFromName(_layerName)))
			{
				return hit.position;
			}
			return new Vector3(0f, 0f, 0f);
		}

		public static void CopyNavMeshAgent(NavMeshAgent _source, GameObject _dest)
		{
			_dest.transform.position = FindValidNavMeshPosition(ms_LayerName);
			NavMeshAgent navMeshAgent = _dest.AddComponent<NavMeshAgent>();
			navMeshAgent.radius = _source.radius;
			navMeshAgent.speed = _source.speed;
			navMeshAgent.acceleration = _source.acceleration;
			navMeshAgent.angularSpeed = _source.angularSpeed;
			navMeshAgent.stoppingDistance = _source.stoppingDistance;
			navMeshAgent.autoTraverseOffMeshLink = _source.autoTraverseOffMeshLink;
			navMeshAgent.autoBraking = _source.autoBraking;
			navMeshAgent.autoRepath = _source.autoRepath;
			navMeshAgent.height = _source.height;
			navMeshAgent.baseOffset = _source.baseOffset;
			navMeshAgent.obstacleAvoidanceType = _source.obstacleAvoidanceType;
			navMeshAgent.avoidancePriority = _source.avoidancePriority;
			navMeshAgent.areaMask = _source.areaMask;
		}

		public static void CopyAnimatorData(Animator _source, Animator _dest)
		{
			_dest.runtimeAnimatorController = _source.runtimeAnimatorController;
			_dest.applyRootMotion = _source.applyRootMotion;
			_dest.updateMode = _source.updateMode;
			_dest.cullingMode = _source.cullingMode;
		}

		public static void CopyPrefabToTarget(GameObject _source, GameObject _dest)
		{
			Profiling.Start("CopyPrefabToTarget", float.MaxValue);
			CapsuleCollider component = _source.GetComponent<CapsuleCollider>();
			if ((bool)component)
			{
				CopyCapsuleCollider(component, _dest);
			}
			Rigidbody component2 = _source.GetComponent<Rigidbody>();
			if ((bool)component2)
			{
				CopyRigidbody(component2, _dest);
			}
			NavMeshAgent component3 = _source.GetComponent<NavMeshAgent>();
			if ((bool)component3)
			{
				CopyNavMeshAgent(component3, _dest);
			}
			Animator component4 = _source.GetComponent<Animator>();
			Animator component5 = _dest.GetComponent<Animator>();
			if (component4 != null && component5 != null)
			{
				Profiling.Start("CopyAnimatorData", float.MaxValue);
				CopyAnimatorData(component4, component5);
				Profiling.Stop();
			}
			Profiling.Start("AnimatorDataController", float.MaxValue);
			AnimatorDataController component6 = _source.GetComponent<AnimatorDataController>();
			if ((bool)component6)
			{
				AnimatorDataController.Copy(component6, _dest);
			}
			Profiling.Stop();
			Profiling.Start("AnimationController", float.MaxValue);
			AnimationController component7 = _source.GetComponent<AnimationController>();
			if ((bool)component7)
			{
				AnimationController.Copy(component7, _dest);
			}
			Profiling.Stop();
			Profiling.Start("EffectController", float.MaxValue);
			EffectController component8 = _source.GetComponent<EffectController>();
			if ((bool)component8)
			{
				EffectController.Copy(component8, _dest);
			}
			Profiling.Stop();
			Profiling.Start("AudioSource", float.MaxValue);
			AudioSource component9 = _source.GetComponent<AudioSource>();
			if (component9 != null)
			{
				AudioSource copy = _dest.AddComponent<AudioSource>();
				AudioSourceFactory.CopyFromTemplate(component9, copy);
			}
			Profiling.Stop();
			Transform transform = _source.transform;
			Transform transform2 = _dest.transform;
			int childCount = transform.childCount;
			for (int i = 0; childCount > i; i++)
			{
				Profiling.Start("Transforms", float.MaxValue);
				Transform child = transform.GetChild(i);
				Vector4 vector = child.localPosition;
				Quaternion localRotation = child.localRotation;
				Vector3 localScale = child.localScale;
				child.parent = transform2;
				child.localPosition = vector;
				child.localRotation = localRotation;
				child.localScale = localScale;
				Profiling.Stop();
			}
			Profiling.Stop();
		}

		public static Transform FindNodeRecursive(Transform _base, string _name)
		{
			if (_base.name == _name)
			{
				return _base;
			}
			int childCount = _base.childCount;
			Transform transform = null;
			for (int i = 0; childCount > i; i++)
			{
				transform = FindNodeRecursive(_base.GetChild(i), _name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		public static void PreloadEditorData()
		{
		}

		public static bool MaterialsCompatible(List<Material> _materials)
		{
			int count = _materials.Count;
			if (count > 1 && _materials[0] != null)
			{
				Texture texture = ((!_materials[0].HasProperty("_MainTex")) ? null : _materials[0].GetTexture("_MainTex"));
				Texture texture2 = ((!_materials[0].HasProperty("_BumpMap")) ? null : _materials[0].GetTexture("_BumpMap"));
				Texture texture3 = ((!_materials[0].HasProperty("_MaskTex")) ? null : _materials[0].GetTexture("_MaskTex"));
				if (texture == null || texture2 == null || texture3 == null)
				{
					return false;
				}
				for (int i = 0; count > i; i++)
				{
					Material material = _materials[i];
					if (material == null)
					{
						return false;
					}
					if (!material.HasProperty("_MainTex") || !(material.GetTexture("_MainTex") == texture))
					{
						return false;
					}
					if (!material.HasProperty("_BumpMap") || !(material.GetTexture("_BumpMap") == texture2))
					{
						return false;
					}
					if (!material.HasProperty("_MaskTex") || !(material.GetTexture("_MaskTex") == texture3))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public static string MaterialOverrideName(string _source, HumanoidDef.EAtlasPage _atlasPage, EEra _age)
		{
			foreach (MaterialID s_overrideName in s_overrideNames)
			{
				if (s_overrideName.Equals(_source, _atlasPage, _age))
				{
					return s_overrideName.Name;
				}
			}
			string empty = string.Empty;
			MaterialID materialID = new MaterialID(_source, _atlasPage, _age);
			s_overrideNames.Add(materialID);
			return materialID.Name;
		}

		public static string MaterialOverrideSafeName(Material _source, HumanoidDef.EAtlasPage _atlasPage, EEra _age)
		{
			if (!_source)
			{
				return null;
			}
			string name = _source.name;
			if (name.StartsWith("atlas_", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return MaterialOverrideName(name, _atlasPage, _age);
		}

		public static Material MaterialOverride(Material _source, HumanoidDef.EAtlasPage _atlasPage, EEra _age)
		{
			string text = MaterialOverrideSafeName(_source, _atlasPage, _age);
			if (text == null)
			{
				return _source;
			}
			Material material = null;
			if (!isBuilder)
			{
				Material _outMat = null;
				if (HumanoidData.TryFindMaterial(text, out _outMat))
				{
					return _outMat;
				}
			}
			if (material != null)
			{
				return material;
			}
			if (_atlasPage != 0)
			{
			}
			return _source;
		}

		public static Vector4 TextureOffset(Material _mat, string _tag)
		{
			Vector2 textureOffset = _mat.GetTextureOffset(_tag);
			Vector2 textureScale = _mat.GetTextureScale(_tag);
			return new Vector4(textureScale.x, textureScale.y, textureOffset.x, textureOffset.y);
		}

		public static void ClearGoCache()
		{
			m_GoCache = new BBDictionary<GameID, GameObject>();
		}

		public static bool TryGetCachedGo(GameID _id, out GameObject _go)
		{
			return m_GoCache.TryGetValue(_id, out _go);
		}

		public static void AddGoToCache(GameID _id, GameObject _object)
		{
			if (!m_GoCache.ContainsKey(_id))
			{
				m_GoCache.Add(_id, _object);
			}
		}
	}
}
