using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	public class UIContinousButton : MonoBehaviour
	{
		private class Threshold
		{
			public int Amount;

			public int Step;
		}

		public delegate void OnValueChangeHandler(int _delta);

		private const float m_updateFrequencey = 0.1f;

		private const int STEP_THRESHOLD = 24;

		private const int CLICK_INCREMENT = 1;

		private static readonly List<Threshold> s_thresholds = new List<Threshold>
		{
			new Threshold
			{
				Amount = 20,
				Step = 1
			},
			new Threshold
			{
				Amount = 40,
				Step = 2
			},
			new Threshold
			{
				Amount = 70,
				Step = 3
			},
			new Threshold
			{
				Amount = 150,
				Step = 4
			},
			new Threshold
			{
				Amount = 500,
				Step = 10
			}
		};

		private UIButtonProxy m_proxy;

		private UIExtendedButton m_button;

		private bool m_stopped;

		public bool isEnabled
		{
			get
			{
				if (m_proxy != null)
				{
					return m_proxy.isEnabled;
				}
				if (m_button != null)
				{
					return m_button.isEnabled;
				}
				return false;
			}
			set
			{
				if (m_proxy != null)
				{
					m_proxy.isEnabled = value;
				}
				else if (m_button != null)
				{
					m_button.isEnabled = value;
				}
			}
		}

		public event OnValueChangeHandler OnValueChange;

		public void Stop()
		{
			m_stopped = true;
		}

		private void Awake()
		{
			m_proxy = GetComponent<UIButtonProxy>();
			if (m_proxy == null)
			{
				m_button = GetComponent<UIExtendedButton>();
			}
		}

		private void OnEnable()
		{
			if (m_proxy != null)
			{
				EventDelegate.Add(m_proxy.onStateChange, OnStateChange);
				EventDelegate.Add(m_proxy.onClick, OnButtonClick);
			}
			else if (m_button != null)
			{
				EventDelegate.Add(m_button.onStateChange, OnStateChange);
				EventDelegate.Add(m_button.onClick, OnButtonClick);
			}
			m_stopped = false;
		}

		private void OnDisable()
		{
			if (m_proxy != null)
			{
				EventDelegate.Remove(m_proxy.onStateChange, OnStateChange);
				EventDelegate.Remove(m_proxy.onClick, OnButtonClick);
			}
			else if (m_button != null)
			{
				EventDelegate.Remove(m_button.onStateChange, OnStateChange);
				EventDelegate.Remove(m_button.onClick, OnButtonClick);
			}
		}

		private void OnButtonClick()
		{
			if (!m_stopped)
			{
				Trigger(1);
			}
		}

		private void OnStateChange()
		{
			UIButtonColor.State state = ((!(m_proxy != null)) ? m_button.state : m_proxy.ButtonState);
			if (state == UIButtonColor.State.Pressed)
			{
				StartCoroutine(co_update());
			}
			else
			{
				StopAllCoroutines();
			}
		}

		private IEnumerator co_update()
		{
			WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
			int total = 0;
			int stepCounter = 1;
			m_stopped = false;
			while (!m_stopped)
			{
				yield return waitForSeconds;
				int step = GetStepSize(total, stepCounter);
				total += step;
				stepCounter++;
				Trigger(step);
			}
		}

		private int GetStepSize(int _total, int _steps)
		{
			if (_steps < 24)
			{
				return 1;
			}
			for (int i = 0; i < s_thresholds.Count; i++)
			{
				if (_total < s_thresholds[i].Amount)
				{
					return s_thresholds[i].Step;
				}
			}
			return s_thresholds[s_thresholds.Count - 1].Step;
		}

		private void Trigger(int _value)
		{
			if (this.OnValueChange != null)
			{
				this.OnValueChange(_value);
			}
		}
	}
}
