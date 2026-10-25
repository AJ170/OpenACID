using UnityEngine;

public class MissionEndEffectConf : MonoBehaviour
{
	[SerializeField]
	private Color m_srcFogColor = Color.white;

	[SerializeField]
	private Color m_raysColor = Color.white;

	[SerializeField]
	private float m_duration = 4f;

	[SerializeField]
	private float m_eventDelay;

	[SerializeField]
	private float m_rayExp = 6f;

	[SerializeField]
	private float m_sceneExp = 4f;

	[SerializeField]
	private float m_skyExp = 4f;

	[SerializeField]
	private float m_zoneWidth = 20f;

	[SerializeField]
	private float m_zoneStart = 10f;

	public GameObject RaysPrefab;

	public SlowMotionEventData SlowMotionEventData;

	public Color SrcFogColor
	{
		get
		{
			return m_srcFogColor;
		}
	}

	public Color RaysColor
	{
		get
		{
			return m_raysColor;
		}
	}

	public float Duration
	{
		get
		{
			return m_duration;
		}
	}

	public float EventDelay
	{
		get
		{
			return m_eventDelay;
		}
	}

	public float RayExp
	{
		get
		{
			return m_rayExp;
		}
	}

	public float SceneExp
	{
		get
		{
			return m_sceneExp;
		}
	}

	public float SkyExp
	{
		get
		{
			return m_skyExp;
		}
	}

	public float ZoneWidth
	{
		get
		{
			return m_zoneWidth;
		}
	}

	public float ZoneStart
	{
		get
		{
			return m_zoneStart;
		}
	}
}
