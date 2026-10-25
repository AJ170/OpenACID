using System;
using System.Collections;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public abstract class UIRecivedBase : MonoBehaviour
	{
		[SerializeField]
		private BBTweenSlerp m_slerpTween;

		[SerializeField]
		private UnityEngine.Animation m_animationFollow;

		[SerializeField]
		private UnityEngine.Animation m_animationArrived;

		[SerializeField]
		private GameObject m_itemFollow;

		[SerializeField]
		private GameObject m_itemArrived;

		[SerializeField]
		private BBTweenPosition m_referenceTween;

		public bool IsResourceReward { get; set; }

		private void Awake()
		{
			base.gameObject.SetActive(false);
			OnAwake();
		}

		protected abstract void OnAwake();

		public void DeactivateAnimation()
		{
			StopAllCoroutines();
			base.gameObject.SetActive(false);
		}

		public void StartAnimation(GameObject _startTarget, GameObject _endTarget, EventDelegate.Callback _animationFinished = null, bool _meetInCenter = false)
		{
			StopAllCoroutines();
			base.gameObject.SetActive(true);
			m_itemFollow.SetActive(true);
			if (m_itemArrived != null)
			{
				m_itemArrived.SetActive(false);
			}
			m_itemFollow.transform.localPosition = Vector3.zero;
			m_slerpTween.From = base.transform.InverseTransformPoint(_startTarget.transform.position);
			if (_meetInCenter)
			{
				m_referenceTween.from = m_slerpTween.From;
				m_referenceTween.to = new Vector3(0f, m_slerpTween.From.y, 0f);
				m_slerpTween.From = m_referenceTween.to;
			}
			m_slerpTween.To = base.transform.InverseTransformPoint(_endTarget.transform.position);
			if (m_itemArrived != null)
			{
				m_itemArrived.transform.localPosition = m_slerpTween.To;
			}
			Vector3 curveOffset = m_slerpTween.CurveOffset;
			curveOffset.x = Math.Abs(curveOffset.x);
			if (m_slerpTween.From.x > m_slerpTween.To.x)
			{
				curveOffset.x *= -1f;
			}
			m_slerpTween.CurveOffset = curveOffset;
			if (_animationFinished != null)
			{
				EventDelegate.Add(m_slerpTween.onFinished, _animationFinished, true);
			}
			m_slerpTween.SetTweenToBeginning();
			if (m_referenceTween != null)
			{
				m_referenceTween.SetTweenToBeginning();
			}
			StartAppearAnimation();
		}

		public void ForceStopAnimation()
		{
			base.gameObject.SetActive(false);
			m_slerpTween.SetTweenToBeginning();
			m_animationFollow.Rewind();
		}

		private void Start()
		{
			EventDelegate.Add(m_slerpTween.onFinished, OnTweenFinished);
		}

		private void OnDestroy()
		{
			EventDelegate.Remove(m_slerpTween.onFinished, OnTweenFinished);
		}

		private void OnDisable()
		{
			IsResourceReward = false;
		}

		private void StartAppearAnimation()
		{
			m_animationFollow.Play();
		}

		private IEnumerator AnimateArrived()
		{
			if (m_itemArrived != null)
			{
				m_itemArrived.SetActive(true);
			}
			m_animationArrived.Play("ItemArived");
			while (m_animationArrived.isPlaying)
			{
				yield return new WaitForEndOfFrame();
			}
			base.gameObject.SetActive(false);
		}

		private void OnTweenFinished()
		{
			if (m_animationArrived != null)
			{
				StartCoroutine(AnimateArrived());
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
