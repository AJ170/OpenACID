using System;
using UbiCore;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public class AutoInterpolation : AutoUpdateableBase
	{
		public enum EType
		{
			Linear = 0,
			Sinusoidal = 1
		}

		private float m_Parameter;

		private float m_Timer;

		public EType Type { get; set; }

		public float Speed { get; set; }

		public float StartDelay { get; set; }

		public float EndDelay { get; set; }

		public Action<float> Worker { get; set; }

		public float Value { get; private set; }

		public AutoInterpolation()
		{
			Speed = 1f;
		}

		public override void Start()
		{
			Reset();
			base.Start();
		}

		protected void Reset()
		{
			m_Parameter = 0f;
			m_Timer = 0f;
			Value = 0f;
		}

		protected override bool UpdateFrameInternal(float _gameTimeDelta)
		{
			if (Worker != null)
			{
				Worker(Value);
			}
			if (m_Parameter <= 0f)
			{
				m_Timer += _gameTimeDelta;
				if (!(m_Timer >= StartDelay))
				{
					return false;
				}
				_gameTimeDelta = m_Timer - StartDelay;
				m_Timer = 0f;
			}
			else if (m_Parameter >= 1f)
			{
				m_Timer += _gameTimeDelta;
				if (!(m_Timer >= EndDelay))
				{
					return false;
				}
				_gameTimeDelta -= m_Timer - EndDelay;
				m_Timer = 0f;
			}
			m_Parameter += _gameTimeDelta * Speed;
			m_Parameter = m_Parameter.Clamp(0f, 1f);
			UpdateValue();
			return m_Parameter >= 1f;
		}

		private void UpdateValue()
		{
			switch (Type)
			{
			case EType.Linear:
				Value = m_Parameter;
				break;
			case EType.Sinusoidal:
				Value = Interpolation.InterpolateSinusoidal(m_Parameter);
				break;
			}
		}
	}
}
