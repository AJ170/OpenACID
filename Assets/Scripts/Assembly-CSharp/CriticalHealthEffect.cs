using System;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Network.Missions.Helpers;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class CriticalHealthEffect : MonoBehaviour
{
	public Color VignetteColor = Color.red;

	public float VignetteBlinkPeriod = 0.3f;

	public float SoundDampingDuration = 40f;

	private Vignette m_vignette;

	private bool m_wasCritical;

	private float m_clock;

	private float m_direction = -1f;

	private float m_alpha;

	private float m_lastAlpha;

	private float m_criticalStart;

	private bool m_isCritical;

	private float Alpha
	{
		get
		{
			return m_alpha;
		}
		set
		{
			if (m_lastAlpha < m_alpha && value < m_alpha)
			{
				bool flag = LogicMain.CurrentTime - m_criticalStart < SoundDampingDuration;
				AudioManager.Instance.Play((!flag) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_wounded_quiet : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_wounded);
			}
			m_lastAlpha = m_alpha;
			m_alpha = value;
		}
	}

	private bool IsCritical
	{
		get
		{
			return m_isCritical;
		}
		set
		{
			m_isCritical = value;
			if (!m_wasCritical && m_isCritical)
			{
				m_criticalStart = LogicMain.CurrentTime;
			}
		}
	}

	public void Start()
	{
		m_vignette = Camera.main.GetComponent<Vignette>();
	}

	private void LateUpdate()
	{
		if (!(m_vignette == null))
		{
			IsCritical = LogicMain.PlayerEntity.PlayerStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) <= 1f && UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.InProgress;
			if (IsCritical != m_wasCritical)
			{
				m_direction = ((!(m_clock < 1f)) ? (-1f) : 1f) * ((!IsCritical) ? (-1f) : 1f);
			}
			m_wasCritical = IsCritical;
			m_clock += m_direction * (LogicMain.FrameTime / Mathf.Max(0.5f * VignetteBlinkPeriod, 0.001f));
			if (IsCritical)
			{
				m_clock = MissionMathHelper.Repeat(m_clock, 2f);
			}
			else
			{
				m_clock = Mathf.Clamp(m_clock, 0f, 2f);
			}
			Alpha = -0.5f * Mathf.Cos(m_clock * (float)Math.PI) + 0.5f;
			m_vignette.SetVignetteColor(Vignette.Type.CritHealth, VignetteColor, Alpha);
		}
	}
}
