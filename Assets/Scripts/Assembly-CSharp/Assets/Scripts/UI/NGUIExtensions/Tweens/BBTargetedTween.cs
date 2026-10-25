using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	public abstract class BBTargetedTween : UITweener
	{
		[SerializeField]
		private GameObject m_target;

		[SerializeField]
		private BBTargetedTween m_templateTween;

		[SerializeField]
		protected bool m_incrementValueWithFrom;

		[SerializeField]
		private bool m_isDeactivated;

		[SerializeField]
		private UITweener m_refTween;

		[SerializeField]
		private bool m_hideTargetOnPrepareTween;

		[SerializeField]
		private int m_startOrder;

		[SerializeField]
		private float m_delayModifier;

		[SerializeField]
		private bool m_forceActiveOnPlay = true;

		[HideInInspector]
		public List<EventDelegate> OnBeforeTweenStart = new List<EventDelegate>();

		private bool m_isTweenValidationAllowed = true;

		protected bool m_isInitialized;

		public bool IsTweenValidationAllowed
		{
			get
			{
				return m_isTweenValidationAllowed;
			}
			set
			{
				m_isTweenValidationAllowed = value;
			}
		}

		public UITweener ReferenceTween
		{
			get
			{
				return m_refTween;
			}
			set
			{
				m_refTween = value;
			}
		}

		public int StartOrder
		{
			get
			{
				return m_startOrder;
			}
			set
			{
				m_startOrder = value;
				delay = m_delayModifier * (float)m_startOrder;
			}
		}

		public bool IsDeactivated
		{
			get
			{
				return m_isDeactivated;
			}
			set
			{
				m_isDeactivated = value;
			}
		}

		public GameObject Target
		{
			get
			{
				return (!(m_target == null)) ? m_target : base.gameObject;
			}
			set
			{
				m_target = value;
				ClearCache();
			}
		}

		public bool ActivateOnPlay
		{
			get
			{
				return m_forceActiveOnPlay;
			}
			set
			{
				m_forceActiveOnPlay = value;
			}
		}

		public virtual void ValidateTween()
		{
			if (!IsTweenValidationAllowed)
			{
				return;
			}
			Init();
			if (m_templateTween != null)
			{
				CopyFrom(m_templateTween);
				return;
			}
			if (m_refTween != null)
			{
				style = m_refTween.style;
				ignoreTimeScale = m_refTween.ignoreTimeScale;
				animationCurve = m_refTween.animationCurve;
			}
			if (m_startOrder > 0)
			{
				delay = m_delayModifier * (float)m_startOrder;
			}
		}

		public virtual void CopyFrom(BBTargetedTween _template)
		{
			m_incrementValueWithFrom = _template.m_incrementValueWithFrom;
			m_isDeactivated = _template.m_isDeactivated;
			m_hideTargetOnPrepareTween = _template.m_hideTargetOnPrepareTween;
			m_refTween = _template.m_refTween;
			m_startOrder = _template.m_startOrder;
			m_delayModifier = _template.m_delayModifier;
			style = _template.style;
			ignoreTimeScale = _template.ignoreTimeScale;
			animationCurve = _template.animationCurve;
			duration = _template.duration;
			delay = _template.delay;
			tweenGroup = _template.tweenGroup;
		}

		[ContextMenu("Play Forward")]
		public void StartTweenPlayForward()
		{
			if (m_forceActiveOnPlay)
			{
				Target.gameObject.SetActive(true);
			}
			TriggerOnBeforeStart();
			SetTweenToBeginning();
			base.enabled = false;
			PlayForward();
		}

		[ContextMenu("Play Reverse")]
		public void StartTweenPlayReverse()
		{
			if (m_forceActiveOnPlay)
			{
				Target.gameObject.SetActive(true);
			}
			TriggerOnBeforeStart();
			SetTweenToEnd();
			base.enabled = false;
			PlayReverse();
		}

		public void PrepareTweenForStart()
		{
			base.enabled = false;
			if (!m_isDeactivated && m_hideTargetOnPrepareTween)
			{
				Target.gameObject.SetActive(false);
			}
		}

		public void SetTweenToBeginning()
		{
			base.enabled = false;
			if (!m_isDeactivated)
			{
				base.tweenFactor = 0f;
				Sample(base.tweenFactor, false);
				if (m_hideTargetOnPrepareTween)
				{
					Target.gameObject.SetActive(true);
				}
			}
		}

		public void SetTweenToEnd()
		{
			base.enabled = false;
			base.tweenFactor = 1f;
			Sample(base.tweenFactor, false);
			if (!m_isDeactivated && m_hideTargetOnPrepareTween)
			{
				Target.gameObject.SetActive(true);
			}
		}

		public abstract void SwapToAndFromValues();

		protected abstract void ClearCache();

		private void Awake()
		{
			Init();
		}

		private void OnDestroy()
		{
			ClearCache();
			OnBeforeTweenStart.Clear();
		}

		protected virtual void Init()
		{
			m_isInitialized = true;
		}

		protected void OnValidate()
		{
			ClearCache();
		}

		private void TriggerOnBeforeStart()
		{
			if (OnBeforeTweenStart != null)
			{
				List<EventDelegate> onBeforeTweenStart = OnBeforeTweenStart;
				OnBeforeTweenStart = new List<EventDelegate>();
				EventDelegate.Execute(onBeforeTweenStart);
				for (int i = 0; i < onBeforeTweenStart.Count; i++)
				{
					EventDelegate.Add(OnBeforeTweenStart, onBeforeTweenStart[i]);
				}
				onBeforeTweenStart = null;
			}
		}
	}
}
