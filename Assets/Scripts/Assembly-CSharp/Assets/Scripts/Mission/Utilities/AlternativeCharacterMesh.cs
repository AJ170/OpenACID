using System.Collections.Generic;
using Assets.Scripts.Database;
using Assets.Scripts.Engine.ExtensionMethods;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Database;
using Humanoids;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Utilities
{
	public class AlternativeCharacterMesh : MonoBehaviour
	{
		public bool PlayTransition = true;

		public ICharacterAppearance m_Appearance;

		private bool m_useAlternativeMesh;

		private LODGroup m_lodGroup;

		private List<GameObject> m_OriginalBodies = new List<GameObject>();

		private GameObject m_DisguiseBody;

		private List<SkinnedMeshRenderer> m_OriginalBodyRenderers = new List<SkinnedMeshRenderer>();

		private SkinnedMeshRenderer m_DisguiseBodyRenderer;

		private float m_state;

		private bool m_changing;

		private bool m_changed;

		public GameID HumanoidGameID { get; set; }

		public bool UseAlternativeMesh
		{
			get
			{
				return m_useAlternativeMesh;
			}
			set
			{
				if (m_useAlternativeMesh != value)
				{
					m_useAlternativeMesh = value;
					OnChangeMesh();
				}
			}
		}

		public AlternativeCharacterMesh()
		{
			HumanoidGameID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_alternativeCharacterMesh;
		}

		private void Start()
		{
			m_lodGroup = GetComponent<LODGroup>();
			if (m_lodGroup != null)
			{
				for (int i = 0; i < m_lodGroup.lodCount; i++)
				{
					Transform transform = base.transform.Find("Body_LOD" + i);
					if (transform != null)
					{
						m_OriginalBodies.Add(transform.gameObject);
						m_OriginalBodyRenderers.Add(transform.gameObject.GetComponent<SkinnedMeshRenderer>());
					}
				}
			}
			else
			{
				Transform transform2 = base.transform.Find("Body_LOD0");
				if (transform2 != null)
				{
					m_OriginalBodies.Add(transform2.gameObject);
					m_OriginalBodyRenderers.Add(transform2.gameObject.GetComponent<SkinnedMeshRenderer>());
				}
			}
			m_DisguiseBody = Singleton<HumanoidFactory>.Instance.AddHumanoidSkin(HumanoidGameID, base.gameObject, m_Appearance);
			if (m_DisguiseBody != null)
			{
				m_DisguiseBodyRenderer = m_DisguiseBody.GetComponent<SkinnedMeshRenderer>();
			}
			if (UseAlternativeMesh)
			{
				OnChangeMesh();
			}
			else
			{
				ToggleObjects();
			}
		}

		private void SetupNoiseGlow(Material _mat, Vector4 _data)
		{
			EntityWrapperComponent component = base.gameObject.GetComponent<EntityWrapperComponent>();
			if (!(component != null))
			{
				return;
			}
			Character character = component.TryGetEntity<Character>();
			if (character != null && character.Highlight != null)
			{
				character.Highlight.ConfigureMaterial(_mat, (!PlayerHelper.IsUsingEagleEye) ? Highlight.FX_MODE.FX_NOISEGLOW : Highlight.FX_MODE.FX_HIGHLIGHT_NOISEGLOW);
				_mat.SetVector("_NoiseGlow", _data);
				if (m_state > 1f)
				{
					character.Highlight.ConfigureMaterial(_mat, PlayerHelper.IsUsingEagleEye ? Highlight.FX_MODE.FX_HIGHLIGHT : Highlight.FX_MODE.FX_OFF);
					_mat.SetVector("_NoiseGlow", new Vector4(1f, 1f, 1f, 0f));
				}
			}
		}

		public void Update()
		{
			if (!m_changing)
			{
				return;
			}
			float num = EffectHelper.m_easeCurve.Evaluate(m_state);
			float num2 = EffectHelper.m_fadeCurve.Evaluate(m_state);
			Vector4 data = new Vector4(1f - 0.2f * num, 1f - 0.2f * num, 1f, m_state * num2);
			m_state += UbiCore.Utils.Singleton<Timer>.Instance.deltaTime * 0.5f;
			if (m_state > 1f)
			{
				m_changing = false;
			}
			foreach (SkinnedMeshRenderer originalBodyRenderer in m_OriginalBodyRenderers)
			{
				if (originalBodyRenderer != null)
				{
					SetupNoiseGlow(originalBodyRenderer.material, data);
				}
			}
			if ((bool)m_DisguiseBodyRenderer)
			{
				SetupNoiseGlow(m_DisguiseBodyRenderer.material, data);
				if (m_state > 0.4f && !m_changed)
				{
					ToggleObjects();
					m_changed = true;
				}
			}
		}

		private void OnChangeMesh()
		{
			if (PlayTransition)
			{
				m_changing = true;
				m_changed = false;
				m_state = 0f;
			}
			else
			{
				ToggleObjects();
				m_changed = true;
				m_changing = false;
				m_state = 1f;
			}
		}

		private void ToggleObjects()
		{
			if (m_lodGroup != null)
			{
				m_lodGroup.enabled = !UseAlternativeMesh;
			}
			foreach (GameObject originalBody in m_OriginalBodies)
			{
				originalBody.SetActive(!UseAlternativeMesh);
			}
			if (m_DisguiseBody != null)
			{
				m_DisguiseBody.SetActive(UseAlternativeMesh);
			}
			if (m_DisguiseBodyRenderer != null)
			{
				m_DisguiseBodyRenderer.enabled = UseAlternativeMesh;
			}
			EntityWrapperComponent component = base.gameObject.GetComponent<EntityWrapperComponent>();
			if (component != null)
			{
				Character character = component.TryGetEntity<Character>();
				if (character != null && character.Highlight != null)
				{
					character.Highlight.RescanGameObjects();
					character.Highlight.ApplyCurrentColor();
				}
			}
			Transform owner = HumanoidFactoryHelpers.FindNodeRecursive(base.gameObject.transform, "Head");
			foreach (GameObject child in owner.GetChildren())
			{
				child.SetActive(!UseAlternativeMesh);
			}
		}
	}
}
