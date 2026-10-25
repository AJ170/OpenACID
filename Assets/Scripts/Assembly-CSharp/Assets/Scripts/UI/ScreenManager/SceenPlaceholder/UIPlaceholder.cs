using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager.SceenPlaceholder
{
	[AddComponentMenu("UI/Placeholder")]
	public class UIPlaceholder : MonoBehaviour
	{
		[SerializeField]
		private UIPlaceholders.EPlaceholderType m_type;

		[SerializeField]
		private GameObject m_placeHolder;

		private Camera m_uiCamera;

		private Camera m_mainCamera;

		private Transform m_floorIsland;

		private Vector3 m_assassinStartPosition;

		public bool IsIsInitialized { get; private set; }

		public UIPlaceholders.EPlaceholderType PlaceholderType
		{
			get
			{
				return m_type;
			}
		}

		private void Init()
		{
			if (!IsIsInitialized)
			{
				m_uiCamera = Singleton<UIScreenManager>.Instance.SceneData.MainUICamera;
				CameraMenuManager cameraMenuManager = BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager;
				m_mainCamera = ((m_type != UIPlaceholders.EPlaceholderType.AssassinPreview) ? cameraMenuManager.Camera : cameraMenuManager.AssassinCamera);
				IsIsInitialized = true;
				if (m_type == UIPlaceholders.EPlaceholderType.Assassin)
				{
					m_floorIsland = BehaviourSingleton<EnvironmentBridgeMain>.Instance.FloorMesh;
				}
			}
		}

		private void Awake()
		{
			m_assassinStartPosition = base.transform.position;
			Init();
		}

		private void OnDestroy()
		{
			IsIsInitialized = false;
			m_uiCamera = null;
			m_mainCamera = null;
		}

		private void Update()
		{
			Init();
			if (!(m_uiCamera == null) && !(m_mainCamera == null))
			{
				Vector3 position = NGUIMath.WorldToLocalPoint(m_placeHolder.transform.position, m_uiCamera, m_mainCamera, null);
				base.transform.position = position;
				if (m_type == UIPlaceholders.EPlaceholderType.Assassin && base.transform.childCount > 0)
				{
					Vector3 vector = base.transform.position - m_assassinStartPosition;
					m_floorIsland.transform.position = Vector3.zero + vector;
				}
			}
		}
	}
}
