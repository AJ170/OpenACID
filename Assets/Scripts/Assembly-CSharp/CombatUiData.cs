using System;
using UnityEngine;

[Serializable]
public class CombatUiData
{
	public int m_ReferenceScreenHeight = 1536;

	public bool m_ShowHealthBars = true;

	public float m_HealthBar_HitTime = 0.3f;

	public float m_HealthBar_HitFalloffTime = 2.5f;

	public Vector2 m_HealthBar_Offset2d = new Vector2(0.48f, 0.17f);

	public Vector2 m_HealthBar_CustomScale = new Vector2(0.5f, 0.5f);

	public Interpolate.EaseType m_HealthBar_InterpolationType = Interpolate.EaseType.EaseOutCubic;

	public Vector2 m_Digits_Offset2d = new Vector2(50f, 40f);

	public float m_Digits_LifeTime = 1.4f;

	public float m_Digits_LootLifetime = 3f;

	public float m_Digits_FadeOutTime = 0.6f;

	public Vector2 m_Digits_TargetOffset2d = new Vector2(0f, 150f);

	public Interpolate.EaseType m_Digits_MoveInterpolation = Interpolate.EaseType.EaseOutCubic;

	public Interpolate.EaseType m_Digits_ScaleInterpolation = Interpolate.EaseType.EaseOutCubic;

	public Interpolate.EaseType m_Digits_FadeInterpolation = Interpolate.EaseType.EaseOutCubic;

	public float m_Digits_PositionBlendFactor = 1.5f;

	public Color m_Digits_ColorDefault = Color.white;

	public Color m_Digits_ColorHit = new Color(0.96f, 0.82f, 0f);

	public Color m_Digits_ColorCrit = new Color(1f, 0f, 0f);

	public Color m_Digits_ColorHeal = new Color(0.74f, 0.07f, 0f);

	public Color m_Digits_ColorTimer = Color.black;

	public Color m_Digits_ColorDodged = Color.white;

	public Color m_Digits_ColorDodgedMiss = Color.white;

	public Color m_Digits_ColorKillMoveDeflect = Color.white;

	public Color m_Digits_ColorCoins = Color.white;

	public Color m_Digits_ColorLockpicks = Color.white;

	public float m_Digits_BaseScaleFactor = 2.5f;

	public float m_Digits_ScaleDefault = 0.75f;

	public float m_Digits_ScaleHit = 0.5f;

	public float m_Digits_ScaleCrit = 0.75f;

	public float m_Digits_ScaleHeal = 0.5f;

	public float m_Digits_ScaleTimer = 0.75f;

	public float m_Digits_ScaleDodged = 0.75f;

	public float m_Digits_ScaleDodgedMiss = 0.75f;

	public float m_Digits_ScaleKillMoveDeflect = 0.75f;

	public float m_Digits_ScaleCoins = 0.9f;

	public float m_Digits_ScaleItems = 0.9f;

	public float m_Digits_ScaleLockpick = 0.5f;

	public string[] m_Digits_BackgroundHit = new string[3] { "background_damage_01", "background_damage_02", "background_damage_03" };

	public string m_Digits_BackgroundHeal = "background_heal";

	public string m_Digits_BackgroundTimer = "background_timer";

	public string m_Digits_BackgroundGeneral = "background_general";

	public Interpolate.EaseType m_Vignette_Ease = Interpolate.EaseType.EaseInOutExpo;

	public Color m_Vignette_Hit = new Color(0.93f, 0f, 0f);

	public float m_Vignette_Hit_Min = 0.4f;

	public float m_Vignette_Hit_Duration = 1.5f;

	public Vector2 m_Combo_Offset2d = new Vector2(70f, 10f);

	public float m_Combo_LifeTime = 1.4f;

	public float m_Combo_FadeOutTime = 0.6f;

	public Vector2 m_Combo_Movement2d = new Vector2(-15f, -30f);

	public Interpolate.EaseType m_Combo_MoveInterpolation = Interpolate.EaseType.EaseInOutExpo;

	public Interpolate.EaseType m_Combo_FadeInterpolation = Interpolate.EaseType.EaseOutQuint;

	public Color m_Combo_Color = new Color(0.93f, 0f, 0f);
}
