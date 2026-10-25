using System;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class EarthTransitionManager
	{
		public Action OnEarthTransitionDone;

		private EraExtensions.EEarthPin m_targetPin;

		private bool m_isEarthZoomedTarget;

		public bool TransisionInProgres { get; private set; }

		public void CancelTransition(CameraMenuManager _cameraMenuManager, EarthSpinManager _earthSpinMgr)
		{
			TransisionInProgres = false;
			_cameraMenuManager.InstantChangeZoom(false);
			_earthSpinMgr.InstantChangePinTarget(m_targetPin);
		}

		public void SetEarthTransition(EraExtensions.EEarthPin _pinTarget, bool _isEarthZoomed)
		{
			m_targetPin = _pinTarget;
			m_isEarthZoomedTarget = _isEarthZoomed;
			TransisionInProgres = true;
		}

		public void UpdateTransition(CameraMenuManager _cameraMenuMgr, EarthSpinManager _earthSpinMgr)
		{
			if (!TransisionInProgres)
			{
				return;
			}
			if (m_targetPin != _earthSpinMgr.Current && _cameraMenuMgr.IsZoomed)
			{
				_cameraMenuMgr.IsZoomed = false;
			}
			else
			{
				if (_cameraMenuMgr.IsTweening)
				{
					return;
				}
				if (m_targetPin != _earthSpinMgr.Current)
				{
					_earthSpinMgr.Current = m_targetPin;
				}
				else
				{
					if (_earthSpinMgr.IsEarthRotating)
					{
						return;
					}
					if (_cameraMenuMgr.IsZoomed != m_isEarthZoomedTarget)
					{
						_cameraMenuMgr.IsZoomed = m_isEarthZoomedTarget;
						_earthSpinMgr.Current = m_targetPin;
					}
					else if (!_cameraMenuMgr.IsTweening && !_earthSpinMgr.IsEarthRotating)
					{
						TransisionInProgres = false;
						Action onEarthTransitionDone = OnEarthTransitionDone;
						if (onEarthTransitionDone != null)
						{
							onEarthTransitionDone();
						}
					}
				}
			}
		}
	}
}
