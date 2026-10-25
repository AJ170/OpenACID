using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Widget
{
	public class PopupTimerHandler : MonoBehaviour
	{
		[SerializeField]
		private UISprite m_circleFill;

		[SerializeField]
		private UIExtendedLabel m_timerValueLabel;

		private float m_timerValue;

		private List<EventDelegate> m_timerListener;

		private void Awake()
		{
		}

		public void Stop()
		{
			StopAllCoroutines();
		}

		public void Init(float _duration, List<EventDelegate> _timerListener)
		{
			m_timerValue = _duration;
			m_timerListener = _timerListener;
			StartCoroutine(CountDownRoutine(m_timerValue));
		}

		private IEnumerator CountDownRoutine(float _duration)
		{
			float currentValue = _duration;
			while (true)
			{
				currentValue -= Time.deltaTime / ((!(Time.timeScale > 0f)) ? 1f : Time.timeScale);
				m_timerValueLabel.text = Mathf.Round(currentValue).ToString();
				m_circleFill.fillAmount = currentValue / _duration;
				if (!(currentValue > 0f))
				{
					break;
				}
				yield return null;
			}
			EventDelegate.Execute(m_timerListener);
		}
	}
}
