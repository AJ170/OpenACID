using System;
using AcierProtocol;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UINotorietyBarBehaviour : MonoBehaviour
	{
		private const float c_interpolationTime = 0.5f;

		public UIPanel Panel;

		public UISprite NotorietyBar;

		private float m_lastSetTime;

		private float m_currentValue;

		private float m_targetValue;

		private EAlertLevel m_currentAlertLevel;

		public void SetNotoriety(int _notoriety100, EAlertLevel _currentAlertLevel)
		{
			if (m_targetValue != (float)_notoriety100)
			{
				m_currentValue = GetInterpolatedValue();
				m_targetValue = _notoriety100;
				m_lastSetTime = UbiCore.Utils.Singleton<Timer>.Instance.time;
				m_currentAlertLevel = _currentAlertLevel;
			}
		}

		private float GetInterpolatedValue()
		{
			float val = UbiCore.Utils.Singleton<Timer>.Instance.time - m_lastSetTime;
			float num = Math.Min(val, 0.5f) / 0.5f;
			float num2 = m_targetValue - m_currentValue;
			return m_currentValue + num2 * num;
		}

		private void OnEnable()
		{
			ApplyNotoriety(0f);
		}

		private void Update()
		{
			if (m_currentValue != m_targetValue)
			{
				float interpolatedValue = GetInterpolatedValue();
				ApplyNotoriety(interpolatedValue);
				if (interpolatedValue == m_targetValue)
				{
					m_currentValue = m_targetValue;
				}
			}
		}

		private void ApplyNotoriety(float _notoriety100)
		{
			Panel.gameObject.SetActive(_notoriety100 > 0f);
			float num = 320f * _notoriety100 / 100f;
			Vector4 baseClipRegion = Panel.baseClipRegion;
			baseClipRegion.w = 146f + num;
			baseClipRegion.y = 145f - num / 2f;
			Panel.baseClipRegion = baseClipRegion;
			string spriteName = "minimap_notority_frame_01";
			if (m_currentAlertLevel == EAlertLevel.Alerted)
			{
				spriteName = "minimap_notority_frame_02";
			}
			else if (m_currentAlertLevel == EAlertLevel.Exposed)
			{
				spriteName = "minimap_notority_frame_03";
			}
			NotorietyBar.spriteName = spriteName;
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void SetVisibility(bool _visible)
		{
			base.gameObject.SetActive(_visible);
		}
	}
}
