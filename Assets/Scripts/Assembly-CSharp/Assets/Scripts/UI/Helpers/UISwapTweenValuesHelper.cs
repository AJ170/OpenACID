using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers
{
	public class UISwapTweenValuesHelper : MonoBehaviour
	{
		[SerializeField]
		private BBTargetedTween[] m_tweener;

		[SerializeField]
		private bool m_isSwaped;

		[SerializeField]
		private float m_durationOffset;

		public bool IsValuesSwaped { get; private set; }

		public float DurationOffset
		{
			get
			{
				return m_durationOffset;
			}
			set
			{
				UpdateDurationValues(0f - m_durationOffset);
				m_durationOffset = value;
				UpdateDurationValues(m_durationOffset);
			}
		}

		private void Awake()
		{
			m_isSwaped = false;
			IsValuesSwaped = false;
		}

		public void SwapValues()
		{
			IsValuesSwaped = !IsValuesSwaped;
			for (int i = 0; i < m_tweener.Length; i++)
			{
				m_tweener[i].SwapToAndFromValues();
			}
		}

		public void UpdateDurationValues(float _offset)
		{
			for (int i = 0; i < m_tweener.Length; i++)
			{
				m_tweener[i].duration += _offset;
			}
		}
	}
}
