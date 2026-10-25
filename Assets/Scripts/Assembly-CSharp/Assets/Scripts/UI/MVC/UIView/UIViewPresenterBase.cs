using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView
{
	public class UIViewPresenterBase : MonoBehaviour
	{
		[SerializeField]
		private UIRect m_view;

		public bool IsInitialized { get; set; }

		public UIRect ViewRoot
		{
			get
			{
				return m_view;
			}
		}

		public virtual void Show()
		{
		}

		public virtual void Hide()
		{
		}

		protected virtual void OnAwakeUnity()
		{
		}

		protected virtual void OnStartUnity()
		{
		}

		protected virtual void OnDestroyUnity()
		{
		}

		protected virtual void OnEnableUnity()
		{
		}

		protected virtual void OnDisableUnity()
		{
		}

		private void Awake()
		{
			m_view = m_view ?? GetComponent<UIRect>();
			OnAwakeUnity();
			IsInitialized = true;
		}

		private void Start()
		{
			OnStartUnity();
		}

		private void OnDestroy()
		{
			IsInitialized = false;
			OnDestroyUnity();
		}

		private void OnEnable()
		{
			OnEnableUnity();
		}

		private void OnDisable()
		{
			OnDisableUnity();
		}
	}
}
