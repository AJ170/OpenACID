using UnityEngine;

namespace Assets.Scripts.UI.CameraManager
{
	[AddComponentMenu("UI/Camera Owner")]
	public class UICameraOwner : MonoBehaviour
	{
		[SerializeField]
		private bool m_dontInAnimus = true;

		[SerializeField]
		private bool m_dontInGame = true;

		public bool DontDestroyCameraInAnimus
		{
			get
			{
				return m_dontInAnimus;
			}
		}

		public bool DontDestroyCameraInGame
		{
			get
			{
				return m_dontInGame;
			}
		}

		private void Awake()
		{
			Singleton<UICameraManager>.Instance.RegisterCamera(GetComponent<Camera>());
		}

		private void OnDestroy()
		{
			Singleton<UICameraManager>.Instance.UnRegisterCamera(base.gameObject.layer);
		}
	}
}
