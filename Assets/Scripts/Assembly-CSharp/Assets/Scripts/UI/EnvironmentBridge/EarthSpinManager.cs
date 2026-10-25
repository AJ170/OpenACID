using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class EarthSpinManager : MonoBehaviour
	{
		public delegate void EarthRotateDelegate(EraExtensions.EEarthPin _eEarthCurrent, EraExtensions.EEarthPin _eEarthOld, float _progress);

		private IAudioHandle m_earthSpinAudio;

		private List<SmoothPositionTransition> m_transitions;

		private SmoothPositionTransition m_rotateTransition;

		private EraExtensions.EEarthPin m_current;

		private EraExtensions.EEarthPin m_old;

		private EarthGlobeData m_globeData;

		private bool m_isEarthRotating;

		public EarthPinBase UIPin { get; set; }

		public bool IsEarthRotating
		{
			get
			{
				return m_isEarthRotating;
			}
		}

		public EraExtensions.EEarthPin Current
		{
			get
			{
				return m_current;
			}
			set
			{
				m_old = m_current;
				m_current = value;
				bool flag = m_old != m_current;
				SetEarthPin(m_current, m_old);
				if (flag)
				{
					BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.UpdateCameraLocation();
				}
			}
		}

		public float CurrentZoomOffset
		{
			get
			{
				EarthGlobeData.EarthPinLocation pinLocation = BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthGlobeData.GetPinLocation(Current);
				if (pinLocation == null)
				{
					return 0f;
				}
				EarthPinOffset component = pinLocation.EarthPin.GetComponent<EarthPinOffset>();
				return component.ZoomOffset;
			}
		}

		public event EarthRotateDelegate OnEarthRotate;

		public void Init(EarthGlobeData _globeData)
		{
			m_globeData = _globeData;
			m_transitions = new List<SmoothPositionTransition>();
		}

		public void LoadAndEnableAudio()
		{
			m_earthSpinAudio = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
			m_earthSpinAudio.Load();
		}

		public void UpdateGlobe()
		{
			for (int num = m_transitions.Count - 1; num >= 0; num--)
			{
				if (m_transitions[num].IsDone())
				{
					if (m_rotateTransition == m_transitions[num])
					{
						if (this.OnEarthRotate != null)
						{
							this.OnEarthRotate(m_current, m_old, 1f);
						}
						m_isEarthRotating = false;
					}
					m_transitions.RemoveAt(num);
				}
				else
				{
					m_transitions[num].Step();
				}
			}
			if (m_isEarthRotating && m_rotateTransition != null && this.OnEarthRotate != null)
			{
				this.OnEarthRotate(m_current, m_old, m_rotateTransition.Progress);
			}
		}

		private void OnDestroy()
		{
			if (m_earthSpinAudio != null)
			{
				m_earthSpinAudio.Unload();
			}
		}

		private void SetEarthPin(EraExtensions.EEarthPin _newEarthPin, EraExtensions.EEarthPin _oldEarthPin)
		{
			EarthGlobeData.EarthPinLocation pinLocation = BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthGlobeData.GetPinLocation(_newEarthPin);
			if (pinLocation != null)
			{
				if (UILoadingScreen.Instance.IsHidden && m_earthSpinAudio != null && m_old != m_current)
				{
					m_earthSpinAudio.Stop();
					m_earthSpinAudio.Play();
				}
				m_isEarthRotating = true;
				m_transitions.Clear();
				Vector3 posStart = ((m_rotateTransition == null) ? BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthGlobeData.GetPinLocation(_oldEarthPin).LookAt : m_rotateTransition.CurrentPosition);
				EarthPinOffset component = pinLocation.EarthPin.GetComponent<EarthPinOffset>();
				Vector2 vector = pinLocation.LookAt;
				Vector3 currentCameraPosition = BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.CurrentCameraPosition;
				currentCameraPosition -= m_globeData.EarthGlobe.transform.position;
				currentCameraPosition.z -= component.ZoomOffset;
				float num = Mathf.Atan2(currentCameraPosition.x, currentCameraPosition.z) * 57.29578f;
				Vector3 posTarget = new Vector3(num + 180f, vector.x - component.PinOffsetY, vector.y + 180f - component.PinOffsetX);
				float num2 = posTarget.x % 360f - posStart.x % 360f;
				float num3;
				if (num2 > 0f)
				{
					num3 = 360f - num2;
				}
				else
				{
					num3 = Mathf.Abs(num2);
					num2 = 360f - num3;
				}
				if (num2 > 180f)
				{
					posTarget.x = posStart.x - num3;
				}
				else
				{
					posTarget.x = posStart.x + num2;
				}
				m_rotateTransition = new SmoothPositionTransition(new EarthLookAtTransition(m_globeData, posStart), posTarget, 150f);
				m_transitions.Add(m_rotateTransition);
				if (UIPin != null)
				{
					m_transitions.Add(new SmoothPositionTransition(new PinTransition(pinLocation.EarthPin.transform, UIPin.transform, posStart, m_globeData.GameCamera, m_globeData.UiMainCamera, m_rotateTransition), pinLocation.LookAt, 150f));
					UIPin.gameObject.SetActive(m_globeData.EarthPins.ContainsKey((int)m_current));
				}
				UpdateGlobe();
			}
		}

		public void InstantChangePinTarget(EraExtensions.EEarthPin _targetPin)
		{
			Current = _targetPin;
			for (int num = m_transitions.Count - 1; num >= 0; num--)
			{
				m_transitions[num].InstantFinishTransition();
				if (m_rotateTransition == m_transitions[num])
				{
					m_isEarthRotating = false;
				}
			}
			m_transitions.Clear();
			if (m_isEarthRotating && m_rotateTransition != null && this.OnEarthRotate != null)
			{
				this.OnEarthRotate(m_current, m_old, m_rotateTransition.Progress);
			}
		}
	}
}
