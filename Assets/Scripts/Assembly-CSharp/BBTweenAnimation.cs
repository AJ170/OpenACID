using System.Collections.Generic;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

public class BBTweenAnimation : MonoBehaviour
{
	[SerializeField]
	private Animation m_parentAnimation;

	[SerializeField]
	private Animation[] m_animations;

	[SerializeField]
	private float m_delay;

	[SerializeField]
	private bool m_reactivateParentOnFinish = true;

	[SerializeField]
	private bool m_deactivateOnFinish;

	[SerializeField]
	private bool m_playAnimations;

	private bool m_hasAnimationBeenPlayed;

	private bool m_shouldStartAnimation;

	private bool m_isAnimating;

	private string m_clip = string.Empty;

	private float m_animationStartedTime;

	private int m_startedAnimationIndex;

	private float m_speedBoost = 1f;

	private readonly List<EventDelegate> m_onAnimationsBegin = new List<EventDelegate>();

	private readonly List<EventDelegate> m_onAnimationsCompleted = new List<EventDelegate>();

	private readonly List<AnimationState> m_states = new List<AnimationState>();

	public List<EventDelegate> OnAnimationsCompleted
	{
		get
		{
			return m_onAnimationsCompleted;
		}
	}

	public List<EventDelegate> OnAnimationsBegin
	{
		get
		{
			return m_onAnimationsBegin;
		}
	}

	public float SpeedBoost
	{
		get
		{
			return m_speedBoost;
		}
		set
		{
			m_speedBoost = value;
		}
	}

	public bool IsFinishingAnimationInstantly { get; private set; }

	public bool IsDeactivating { get; private set; }

	public bool IgnoreParent { get; set; }

	public float Delay
	{
		get
		{
			return m_delay;
		}
		set
		{
			m_delay = Mathf.Max(value, 0f);
		}
	}

	public bool IsAnimating
	{
		get
		{
			return m_isAnimating;
		}
	}

	private bool IsWaitingForAnimationsToStart
	{
		get
		{
			return m_isAnimating && m_startedAnimationIndex < m_animations.Length;
		}
	}

	public void SetAnimationList(Animation[] _animations, string _clip = "")
	{
		m_clip = _clip;
		m_animations = _animations;
	}

	public void StartAnimation(bool _restart = false)
	{
		m_isAnimating = true;
		m_hasAnimationBeenPlayed = false;
		IsFinishingAnimationInstantly = false;
		m_startedAnimationIndex = 0;
		m_shouldStartAnimation = true;
		EventDelegate.Execute(OnAnimationsBegin);
		if (!m_shouldStartAnimation)
		{
			return;
		}
		m_animationStartedTime = RealTime.time;
		if (IgnoreParent || _restart)
		{
			return;
		}
		m_states.Clear();
		foreach (AnimationState item in m_parentAnimation)
		{
			if (item.enabled)
			{
				item.enabled = false;
				m_states.Add(item);
			}
		}
	}

	public void ReactivateParentAnimation()
	{
		foreach (AnimationState state in m_states)
		{
			state.enabled = true;
		}
		m_states.Clear();
	}

	public void DontStartAnimation()
	{
		m_hasAnimationBeenPlayed = true;
		m_shouldStartAnimation = false;
	}

	public void StopAnimationIfPlaying(bool _clearAnimationList = false)
	{
		if (m_isAnimating)
		{
			if (!IsWaitingForAnimationsToStart)
			{
				for (int i = 0; i < m_startedAnimationIndex; i++)
				{
					if (string.IsNullOrEmpty(m_clip))
					{
						m_animations[i].Stop();
					}
					else
					{
						m_animations[i].Stop(m_clip);
					}
				}
			}
			m_isAnimating = false;
			m_hasAnimationBeenPlayed = true;
			m_playAnimations = false;
			EventDelegate.Execute(OnAnimationsCompleted);
			if (m_reactivateParentOnFinish)
			{
				ReactivateParentAnimation();
			}
		}
		if (_clearAnimationList)
		{
			m_animations = new Animation[0];
		}
	}

	private void OnEnable()
	{
		IsDeactivating = false;
		SetAnimationPosition(0);
	}

	private void OnDisable()
	{
		IsDeactivating = true;
		SetAnimationPosition(1);
	}

	private void SetAnimationPosition(int _position)
	{
		IsFinishingAnimationInstantly = true;
		m_hasAnimationBeenPlayed = false;
		m_isAnimating = false;
		m_shouldStartAnimation = true;
		EventDelegate.Execute(OnAnimationsBegin);
		if (m_shouldStartAnimation)
		{
			for (int i = 0; i < m_animations.Length; i++)
			{
				m_animations[i].SampleAnimation(_position, m_clip);
			}
			EventDelegate.Execute(OnAnimationsCompleted);
			if (m_reactivateParentOnFinish)
			{
				ReactivateParentAnimation();
			}
		}
		IsFinishingAnimationInstantly = false;
	}

	private void Update()
	{
		if (!m_hasAnimationBeenPlayed && !m_isAnimating && m_playAnimations)
		{
			StartAnimation();
		}
		if (!m_isAnimating)
		{
			return;
		}
		if (IsWaitingForAnimationsToStart)
		{
			float time = RealTime.time;
			for (int i = m_startedAnimationIndex; i < m_animations.Length; i++)
			{
				if (time >= m_animationStartedTime + m_delay * (float)i)
				{
					if (string.IsNullOrEmpty(m_clip))
					{
						m_animations[i].Play();
					}
					else
					{
						m_animations[i].Play(m_clip);
					}
					m_startedAnimationIndex++;
				}
			}
		}
		bool flag = true;
		for (int j = 0; j < m_animations.Length; j++)
		{
			if (m_animations[j].isPlaying)
			{
				m_animations[j].UpdateAnimationSpeed(SpeedBoost);
				flag = false;
			}
		}
		if (flag && !IsWaitingForAnimationsToStart)
		{
			m_isAnimating = false;
			m_hasAnimationBeenPlayed = true;
			m_playAnimations = false;
			EventDelegate.Execute(OnAnimationsCompleted);
			if (m_reactivateParentOnFinish)
			{
				ReactivateParentAnimation();
			}
			if (m_deactivateOnFinish)
			{
				base.enabled = false;
			}
		}
	}
}
