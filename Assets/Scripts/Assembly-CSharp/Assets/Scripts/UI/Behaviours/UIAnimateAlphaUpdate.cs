using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	[RequireComponent(typeof(UIRect))]
	[ExecuteInEditMode]
	public class UIAnimateAlphaUpdate : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.Animation m_animation;

		[Range(0f, 1f)]
		public float AnimatedAlpha = 1f;

		private bool m_hasAnimationStarted;

		private UIRect m_rectToAnimate;

		private UIRect RectToAnimate
		{
			get
			{
				if (m_rectToAnimate == null)
				{
					m_rectToAnimate = GetComponent<UIRect>();
				}
				return m_rectToAnimate;
			}
		}

		private void OnEnable()
		{
			m_hasAnimationStarted = false;
		}

		private void Update()
		{
			if (!Application.isPlaying || m_animation.isPlaying || m_hasAnimationStarted)
			{
				if (!m_hasAnimationStarted)
				{
					m_animation.Sample();
				}
				m_hasAnimationStarted = m_animation.isPlaying;
				RectToAnimate.alpha = AnimatedAlpha;
			}
		}
	}
}
