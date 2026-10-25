using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Generic
{
	public class AnimatedLineRenderer : MonoBehaviour
	{
		private const float s_duration = 0.4f;

		private static readonly Color s_startColor = Color.white;

		private static readonly Color s_endColor = new Color(1f, 1f, 1f, 1f);

		[SerializeField]
		private Transform m_lineStartAnchor;

		[SerializeField]
		private BBTweenPosition m_lineTweener;

		[SerializeField]
		private LineRenderer m_lineRenderer;

		private float m_alpha;

		private UIRect m_rect;

		private EarthPinBase m_earhPin;

		private void Awake()
		{
			m_lineTweener.worldSpace = true;
			m_lineRenderer.useWorldSpace = true;
		}

		public void StartLineAnimation(UIRect _rect, EarthPinBase _pin)
		{
			m_rect = _rect;
			m_earhPin = _pin;
			m_lineTweener.duration = 0.4f;
			m_lineTweener.from = m_lineStartAnchor.transform.position;
			m_lineTweener.to = m_earhPin.transform.position;
			m_lineTweener.StartTweenPlayForward();
			base.gameObject.SetActive(true);
		}

		public void StopLineAnimation()
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(false);
			}
		}

		private void Draw(Vector3 _startPosition, Vector3 _endPosition)
		{
			Color start = s_startColor;
			start.a *= m_alpha;
			Color end = s_endColor;
			end.a *= m_alpha;
			m_lineRenderer.SetColors(start, end);
			m_lineRenderer.SetPosition(0, _startPosition);
			m_lineRenderer.SetPosition(1, _endPosition);
		}

		private void LateUpdate()
		{
			if (m_earhPin != null)
			{
				Vector3 position = m_lineStartAnchor.transform.position;
				Vector3 zero = Vector3.zero;
				zero = ((!m_lineTweener.enabled) ? m_earhPin.transform.position : m_lineTweener.Target.transform.position);
				Draw(position, zero);
				UpdateLineAlpha();
			}
		}

		private void UpdateLineAlpha()
		{
			m_alpha = 1f;
			if (m_rect != null)
			{
				Vector3[] worldCorners = m_rect.worldCorners;
				Bounds bounds = new Bounds(worldCorners[0], Vector3.zero);
				for (int i = 1; i < 4; i++)
				{
					bounds.Encapsulate(worldCorners[i]);
				}
				float num = (m_lineStartAnchor.transform.position.y - bounds.min.y) / bounds.size.y;
				if (num < 0f || num > 1f)
				{
					m_alpha = 0f;
					return;
				}
				float f = Mathf.Min(num, 1f - num) * 14f;
				f = Mathf.Pow(f, 2f);
				m_alpha = Mathf.Clamp01(f);
			}
		}
	}
}
