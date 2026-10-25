using Assets.Scripts.UI.Interfaces;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.CameraManager
{
	public class UICameraManager : Singleton<UICameraManager>, IManager
	{
		public class CameraUsageData
		{
			public int LayerType = -1;

			public Camera Camera;

			public int UsageReference;
		}

		private readonly BBList<CameraUsageData> m_availableCameras = new BBList<CameraUsageData>();

		public bool IsInitialized { get; private set; }

		public void Initialize()
		{
			IsInitialized = true;
			for (int i = 0; i < m_availableCameras.Count; i++)
			{
				CameraUsageData data = m_availableCameras[i];
				ValidateCameraData(data);
			}
		}

		public void PurgeManager()
		{
			IsInitialized = false;
		}

		public CameraUsageData GetCameraDataByLayer(int _layerType)
		{
			for (int i = 0; i < m_availableCameras.Count; i++)
			{
				CameraUsageData cameraUsageData = m_availableCameras[i];
				if (cameraUsageData.LayerType == _layerType)
				{
					return cameraUsageData;
				}
			}
			return null;
		}

		public void RegisterCamera(Camera _camera)
		{
			CameraUsageData cameraUsageData = AddOrGetCameraData(_camera, _camera.gameObject.layer);
			cameraUsageData.Camera = _camera;
			ValidateCameraData(cameraUsageData);
		}

		public void UnRegisterCamera(int _layerType)
		{
			for (int i = 0; i < m_availableCameras.Count; i++)
			{
				CameraUsageData cameraUsageData = m_availableCameras[i];
				if (cameraUsageData.LayerType == _layerType)
				{
					cameraUsageData.Camera = null;
					m_availableCameras.RemoveAt(i);
					break;
				}
			}
		}

		public void ValidateCameraData(CameraUsageData _data)
		{
			if (IsInitialized && (bool)_data.Camera)
			{
				_data.Camera.gameObject.SetActive(_data.UsageReference > 0);
			}
		}

		public void RegisterCameraReference(int _layerType)
		{
			CameraUsageData cameraUsageData = AddOrGetCameraData(null, _layerType);
			cameraUsageData.UsageReference++;
			ValidateCameraData(cameraUsageData);
		}

		public void UnRegisterCameraReference(int _layerType)
		{
			CameraUsageData cameraDataByLayer = GetCameraDataByLayer(_layerType);
			if (cameraDataByLayer != null)
			{
				cameraDataByLayer.UsageReference--;
				ValidateCameraData(cameraDataByLayer);
			}
		}

		private CameraUsageData AddOrGetCameraData(Camera _camera, int _layerType)
		{
			CameraUsageData cameraUsageData = GetCameraDataByLayer(_layerType);
			if (cameraUsageData == null)
			{
				CameraUsageData cameraUsageData2 = new CameraUsageData();
				cameraUsageData2.Camera = _camera;
				cameraUsageData2.LayerType = _layerType;
				cameraUsageData2.UsageReference = 0;
				cameraUsageData = cameraUsageData2;
				m_availableCameras.Add(cameraUsageData);
			}
			return cameraUsageData;
		}
	}
}
