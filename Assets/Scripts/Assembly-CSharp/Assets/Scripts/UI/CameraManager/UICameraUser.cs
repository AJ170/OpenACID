using UnityEngine;

namespace Assets.Scripts.UI.CameraManager
{
	[AddComponentMenu("UI/Camera User")]
	public class UICameraUser : MonoBehaviour
	{
		private int m_registerdLayer = -1;

		private void OnEnable()
		{
			m_registerdLayer = base.gameObject.layer;
			Singleton<UICameraManager>.Instance.RegisterCameraReference(m_registerdLayer);
		}

		private void OnDisable()
		{
			Singleton<UICameraManager>.Instance.UnRegisterCameraReference(m_registerdLayer);
			m_registerdLayer = -1;
		}
	}
}
