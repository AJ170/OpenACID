using System;
using System.Collections;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UnityEngine;

namespace UbiCore.Utils
{
	public class Timer : Singleton<Timer>
	{
		public enum EPauseState
		{
			Running = 0,
			PauseGeneral = 1,
			PauseOptions = 2,
			PauseDialog = 3
		}

		public delegate void OnScaleChangeHandler(float _new);

		public delegate void OnPauseChangeHandler(EPauseState _pauseReason);

		public delegate void OnResetHandler();

		internal class Occurrence
		{
			internal readonly Func<float, bool> callbackFunc;

			internal float lastExecTime;

			internal float delta;

			public bool ToBeRemoved { get; set; }

			internal Occurrence(Func<float, bool> _callback, float _delta, float _initlastExec)
			{
				callbackFunc = _callback;
				lastExecTime = _initlastExec;
				delta = _delta;
			}

			internal bool Eval(float _now)
			{
				float num = _now - lastExecTime;
				if (num > delta)
				{
					lastExecTime = _now;
					return callbackFunc(num);
				}
				return true;
			}
		}

		public const float s_PauseScale = 1E-05f;

		public const float s_DefaultFixedUpdateDelta = 0.02f;

		public const float s_MaxRealDelta = 0.33333f;

		public const float s_PauseDuration = 0.75f;

		private readonly BBList<Occurrence> m_RealtimeCallbacks = new BBList<Occurrence>();

		private readonly BBList<Occurrence> m_Callbacks = new BBList<Occurrence>();

		private float m_timeScale = 1f;

		private float m_pausedScale = 1f;

		private EPauseState m_paused;

		public float timeScale
		{
			get
			{
				if (IsPaused())
				{
					return Math.Max(m_timeScale * pauseTime / 0.75f, 1E-05f);
				}
				if (pauseTime > 0f)
				{
					float num = Math.Min(1.00001f - pauseTime / 0.75f, 1f);
					return m_timeScale * num;
				}
				return m_timeScale;
			}
			set
			{
				if (m_timeScale != value)
				{
					m_timeScale = value;
					OnScaleChangeHandler onScaleChanged = OnScaleChanged;
					if (onScaleChanged != null)
					{
						onScaleChanged(m_timeScale);
					}
				}
			}
		}

		public float deltaTime { get; private set; }

		public float time { get; private set; }

		public float realDeltaTime { get; private set; }

		public float realTime { get; private set; }

		public float smoothFactor { get; set; }

		public float pauseTime { get; set; }

		public int frameCount { get; private set; }

		public OnScaleChangeHandler OnScaleChanged { get; set; }

		public OnPauseChangeHandler OnPauseChanged { get; set; }

		public OnResetHandler OnReset { get; set; }

		public EPauseState PauseState
		{
			get
			{
				return m_paused;
			}
		}

		public Timer()
		{
			smoothFactor = 1f;
			Reset();
		}

		public void Reset()
		{
			m_RealtimeCallbacks.Clear();
			m_Callbacks.Clear();
			Pause(EPauseState.Running);
			realTime = Time.realtimeSinceStartup;
			time = 0f;
			Step();
			deltaTime = 0.15f;
			if (OnReset != null)
			{
				OnReset();
			}
		}

		public void Step()
		{
			Profiling.Start("Timer::Step()", float.MaxValue);
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			realDeltaTime = realtimeSinceStartup - realTime;
			if (realDeltaTime > 0.33333f)
			{
				realDeltaTime = 0.33333f;
				realTime = realtimeSinceStartup;
			}
			else
			{
				realTime += realDeltaTime;
			}
			if (pauseTime > 0f)
			{
				pauseTime -= realDeltaTime;
			}
			float num = realDeltaTime * timeScale;
			deltaTime += smoothFactor * (num - deltaTime);
			Time.timeScale = timeScale;
			time += deltaTime;
			frameCount = Time.frameCount;
			if (!IsPaused())
			{
				UpdateCallbacks(m_Callbacks, time);
			}
			UpdateCallbacks(m_RealtimeCallbacks, realTime);
			Profiling.Stop();
		}

		private void UpdateCallbacks(BBList<Occurrence> _callbacks, float _now)
		{
			int count = _callbacks.Count;
			for (int i = 0; i < count; i++)
			{
				Occurrence occurrence = _callbacks[i];
				if (!_callbacks[i].ToBeRemoved && !occurrence.Eval(_now))
				{
					_callbacks[i].ToBeRemoved = true;
				}
			}
			for (int num = _callbacks.Count - 1; num >= 0; num--)
			{
				if (_callbacks[num].ToBeRemoved)
				{
					_callbacks.RemoveAt(num);
				}
			}
		}

		public void AddCallback(Func<float, bool> callbackFunc, float _delta, bool _unscaled)
		{
			if (_unscaled)
			{
				m_RealtimeCallbacks.Add(new Occurrence(callbackFunc, _delta, realTime));
			}
			else
			{
				m_Callbacks.Add(new Occurrence(callbackFunc, _delta, time));
			}
		}

		public bool RemoveCallback(Func<float, bool> _callbackFunc)
		{
			int num = m_Callbacks.FindIndex((Occurrence occ) => occ.callbackFunc == _callbackFunc);
			if (num >= 0)
			{
				m_Callbacks[num].ToBeRemoved = true;
				return true;
			}
			num = m_RealtimeCallbacks.FindIndex((Occurrence occ) => occ.callbackFunc == _callbackFunc);
			if (num >= 0)
			{
				m_RealtimeCallbacks[num].ToBeRemoved = true;
				return true;
			}
			return false;
		}

		public static IEnumerator WaitForSecondsUnscaled(float _seconds)
		{
			float pauseEndTime = Time.realtimeSinceStartup + _seconds;
			while (Time.realtimeSinceStartup < pauseEndTime)
			{
				yield return 0;
			}
		}

		public void Pause(EPauseState _pauseReason)
		{
			if (PauseState != _pauseReason)
			{
				if (IsPaused())
				{
					m_pausedScale = timeScale;
				}
				m_paused = _pauseReason;
				pauseTime = 0.75f;
				OnPauseChangeHandler onPauseChanged = OnPauseChanged;
				if (onPauseChanged != null)
				{
					onPauseChanged(_pauseReason);
				}
				timeScale = (IsPaused() ? 1E-05f : m_pausedScale);
			}
		}

		public void Unpause()
		{
			Pause(EPauseState.Running);
		}

		public bool IsPaused()
		{
			return PauseState != EPauseState.Running;
		}
	}
}
