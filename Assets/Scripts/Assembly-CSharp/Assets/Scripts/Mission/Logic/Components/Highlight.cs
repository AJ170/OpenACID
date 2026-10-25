using System;
using Assets.Scripts.Audio;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Highlight : FrameUpdateableComponent
	{
		public enum EHighlightLayer
		{
			Base = 0,
			Hiding = 1,
			Combat = 2,
			Selection = 3,
			Fleeing = 4
		}

		public enum ESelectionMode
		{
			Off = 0,
			On = 1,
			Enemy = 2,
			Objective = 3,
			Hiding = 4,
			Friendly = 5,
			Count = 6
		}

		public enum FX_MODE
		{
			FX_OFF = 0,
			FX_NOISEGLOW = 1,
			FX_HIGHLIGHT = 2,
			FX_HIGHLIGHT_NOISEGLOW = 3
		}

		public static bool ms_HighlightingActive = true;

		private readonly BBList<EntityId> m_relevantEntities = new BBList<EntityId>();

		private int[] m_highlightFrames = new int[Enum.GetValues(typeof(EHighlightLayer)).Length];

		private Renderer[] m_renderers;

		private static readonly Color[] s_selectionColors = new Color[6]
		{
			new Color(0f, 0f, 0f, 31f / 32f),
			new Color(0.5f, 0.5f, 0.5f, 27f / 32f),
			new Color(1f, 0f, 0f, 11f / 32f),
			new Color(1f, 1f, 0f, 19f / 32f),
			new Color(0f, 0f, 1f, 3f / 32f),
			new Color(0f, 0f, 1f, 3f / 32f)
		};

		private HighlightBehaviour m_highlightEffect;

		private bool m_isHighlightActive = true;

		private float m_state;

		private bool m_changing;

		private bool m_hiding;

		private bool m_WasAlive;

		private float m_FrameUpdateIntervalSeconds;

		public Type HightlightEffectType
		{
			get
			{
				return m_highlightEffect.GetType();
			}
		}

		public static Vector4 ObjectiveHighlightColor
		{
			get
			{
				return s_selectionColors[3];
			}
		}

		public override int Priority
		{
			get
			{
				return 18;
			}
		}

		public BBList<EntityId> RelevantEntities
		{
			get
			{
				return m_relevantEntities;
			}
		}

		public override float FrameUpdateIntervalSeconds
		{
			get
			{
				return m_FrameUpdateIntervalSeconds;
			}
		}

		public Highlight(IEntity _entity)
			: base(_entity)
		{
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			m_WasAlive = _wrapperComponent.Entity is NpcCharacter;
			RescanGameObjects();
			ApplyCurrentColor();
		}

		public void RescanGameObjects()
		{
			m_highlightEffect = GameEntity.GameObject.GetComponentLimitedRecursive<HighlightBehaviour>(2);
			if (!(m_highlightEffect is AnimusWallHighlightBehavior))
			{
				m_renderers = GameEntity.GameObject.GetComponentsInChildren<Renderer>();
			}
		}

		public override void Detach()
		{
			ApplyCurrentColor();
			m_renderers = null;
			base.Detach();
		}

		public void RemoveMode(EHighlightLayer _layer)
		{
			if (_layer == EHighlightLayer.Hiding)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ability_unblend);
			}
			m_highlightFrames[(int)_layer] = 0;
			ApplyCurrentColor();
		}

		public void SetMode(EHighlightLayer _layer, ESelectionMode _mode)
		{
			if (m_isHighlightActive)
			{
				if (_layer == EHighlightLayer.Hiding && _mode == ESelectionMode.Hiding)
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ability_blend);
				}
				m_highlightFrames[(int)_layer] = (int)_mode;
				ApplyCurrentColor();
			}
		}

		public void PlayAppearEffect()
		{
			if (m_highlightEffect != null)
			{
				m_highlightEffect.PlayAppearEffect();
			}
		}

		public void ApplyCurrentColor()
		{
			ESelectionMode highlightFrame = GetHighlightFrame();
			if (m_renderers != null)
			{
				if (highlightFrame == ESelectionMode.Hiding)
				{
					if (!m_hiding)
					{
						m_changing = true;
						m_state = 0f;
						m_hiding = true;
					}
				}
				else if (m_hiding)
				{
					m_changing = true;
					m_hiding = false;
					m_state = 0f;
				}
				for (int i = 0; i < m_renderers.Length; i++)
				{
					Renderer renderer = m_renderers[i];
					if (!(renderer != null))
					{
						continue;
					}
					Material[] materials = renderer.materials;
					foreach (Material material in materials)
					{
						material.SetColor("_RimColor", s_selectionColors[(int)highlightFrame]);
						material.SetColor("_TintColor", s_selectionColors[(int)highlightFrame]);
						material.SetInt("_ApplyRimIntensity", GameEntity.HighlightUsesRimColor ? 1 : 0);
						ESelectionMode eSelectionMode = highlightFrame;
						if (!ms_HighlightingActive && highlightFrame == ESelectionMode.On)
						{
							eSelectionMode = ESelectionMode.Off;
						}
						switch (eSelectionMode)
						{
						case ESelectionMode.Off:
							ConfigureMaterial(material, FX_MODE.FX_OFF);
							break;
						default:
							ConfigureMaterial(material, FX_MODE.FX_HIGHLIGHT);
							break;
						case ESelectionMode.Hiding:
							break;
						}
					}
					renderer.materials = materials;
				}
			}
			if (m_highlightEffect != null)
			{
				switch (highlightFrame)
				{
				case ESelectionMode.Off:
				case ESelectionMode.On:
					m_highlightEffect.Deactivate();
					break;
				case ESelectionMode.Objective:
					m_highlightEffect.Activate();
					break;
				}
			}
			ScheduleFrameUpdate(false);
		}

		private ESelectionMode GetHighlightFrame()
		{
			for (int num = m_highlightFrames.Length - 1; num >= 0; num--)
			{
				if (m_highlightFrames[num] != 0)
				{
					return (ESelectionMode)m_highlightFrames[num];
				}
			}
			return ESelectionMode.Off;
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			m_FrameUpdateIntervalSeconds = float.MaxValue;
		}

		public void Activate()
		{
			m_isHighlightActive = true;
			ApplyCurrentColor();
		}

		public void Deactivate()
		{
			m_isHighlightActive = false;
			for (int num = m_highlightFrames.Length - 1; num >= 0; num--)
			{
				m_highlightFrames[num] = 0;
			}
			ApplyCurrentColor();
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			if (m_WasAlive)
			{
				NpcCharacter npcCharacter = Entity as NpcCharacter;
				if (npcCharacter != null && npcCharacter.GetIsDead())
				{
					SetMode(EHighlightLayer.Base, ESelectionMode.Off);
				}
			}
			if (m_renderers != null && m_changing)
			{
				bool flag = false;
				m_state += UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime * 0.5f;
				m_changing = m_state < 1f;
				for (int i = 0; i < m_renderers.Length; i++)
				{
					Renderer renderer = m_renderers[i];
					if (renderer == null)
					{
						flag = true;
						continue;
					}
					Material[] materials = renderer.materials;
					foreach (Material material in materials)
					{
						float num = EffectHelper.m_easeCurve.Evaluate(m_state * 0.5f + ((!m_hiding) ? 0.5f : 0f));
						float num2 = EffectHelper.m_fadeCurve.Evaluate(m_state);
						if (!m_changing && !m_hiding)
						{
							ConfigureMaterial(material, FX_MODE.FX_OFF);
							if (PlayerHelper.IsUsingEagleEye)
							{
								ConfigureMaterial(material, FX_MODE.FX_HIGHLIGHT);
							}
						}
						else
						{
							ConfigureMaterial(material, FX_MODE.FX_NOISEGLOW);
							material.SetVector("_NoiseGlow", new Vector4(1f - 0.3f * num, 1f - 0.3f * num, 1f - 0.1f * num, m_state * num2));
						}
					}
					renderer.materials = materials;
				}
				if (flag)
				{
					RescanGameObjects();
				}
			}
			m_FrameUpdateIntervalSeconds = ((GetHighlightFrame() == ESelectionMode.Off && !m_changing) ? float.MaxValue : 0f);
			base.DoFrameUpdate(_deltaTime);
		}

		public void ConfigureMaterial(Material _mat, FX_MODE _mode)
		{
			switch (_mode)
			{
			case FX_MODE.FX_OFF:
				_mat.EnableKeyword("FX_OFF");
				_mat.DisableKeyword("FX_NOISEGLOW");
				_mat.DisableKeyword("FX_HIGHLIGHT");
				break;
			case FX_MODE.FX_NOISEGLOW:
				_mat.DisableKeyword("FX_OFF");
				_mat.EnableKeyword("FX_NOISEGLOW");
				_mat.DisableKeyword("FX_HIGHLIGHT");
				break;
			case FX_MODE.FX_HIGHLIGHT:
				_mat.DisableKeyword("FX_OFF");
				_mat.DisableKeyword("FX_NOISEGLOW");
				_mat.EnableKeyword("FX_HIGHLIGHT");
				break;
			case FX_MODE.FX_HIGHLIGHT_NOISEGLOW:
				_mat.DisableKeyword("FX_OFF");
				_mat.EnableKeyword("FX_NOISEGLOW");
				_mat.EnableKeyword("FX_HIGHLIGHT");
				break;
			}
		}
	}
}
