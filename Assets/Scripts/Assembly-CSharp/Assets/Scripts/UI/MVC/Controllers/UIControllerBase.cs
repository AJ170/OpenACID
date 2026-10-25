using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers
{
	public abstract class UIControllerBase : MonoBehaviour
	{
		public bool IsInitialized { get; protected set; }

		public bool IsActive { get; set; }

		public virtual void Show()
		{
			IsActive = true;
		}

		public virtual void Hide()
		{
			IsActive = false;
		}

		public virtual void OnControllerClicked()
		{
		}

		public virtual void OnChanged(UIControllerBase _changedController)
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

		protected virtual void OnUpdateUnity()
		{
		}

		private void Awake()
		{
			OnAwakeUnity();
		}

		private void Start()
		{
			OnStartUnity();
		}

		private void OnDestroy()
		{
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

		private void Update()
		{
			OnUpdateUnity();
		}
	}
}
