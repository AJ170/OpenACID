using System;
using UnityEngine;

[Serializable]
public class CameraTransition
{
	public enum EType
	{
		EaseFunction = 0,
		None = 1
	}

	[SerializeField]
	private EType m_type;

	[SerializeField]
	private Interpolate.EaseType m_easeType = Interpolate.EaseType.EaseOutCubic;

	[SerializeField]
	private float m_easeDuration = 1.5f;

	public EType Type
	{
		get
		{
			return m_type;
		}
	}

	public Interpolate.EaseType EaseType
	{
		get
		{
			return m_easeType;
		}
	}

	public float EaseDuration
	{
		get
		{
			return m_easeDuration;
		}
	}

	public CameraTransition(EType _type, Interpolate.EaseType _easeType, float _easeDuration)
	{
		m_type = _type;
		m_easeType = _easeType;
		m_easeDuration = _easeDuration;
	}
}
