using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.CameraManager;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	[AddComponentMenu("UI/Screen")]
	public class UIScreen : MonoBehaviour
	{
		[SerializeField]
		private GameObject m_activatedObject;

		[SerializeField]
		private BBTargetedTween m_in;

		[SerializeField]
		private BBTargetedTween m_out;

		[SerializeField]
		private bool m_modal;

		[SerializeField]
		private bool m_showTutorial = true;

		[SerializeField]
		private bool m_showAssassinInfoBox;

		[SerializeField]
		private CameraMenuManager.EType m_cameraPosition;

		[SerializeField]
		private EScreenID m_id;

		[SerializeField]
		private bool m_showBackButton = true;

		private UITweenedScreenComponent[] m_elements;

		private Id<TrackPoint> m_trackId = Id<TrackPoint>.Invalid;

		public EScreenID m_previousScreen;

		public static UIScreen CurrentUIScreen;

		public List<EventDelegate> onInit = new List<EventDelegate>();

		public List<EventDelegate> onDestroy = new List<EventDelegate>();

		public List<EventDelegate> onEnter = new List<EventDelegate>();

		public List<EventDelegate> onSetup = new List<EventDelegate>();

		public List<EventDelegate> onExit = new List<EventDelegate>();

		public List<EventDelegate> onPrepareActivate = new List<EventDelegate>();

		public List<EventDelegate> onActivate = new List<EventDelegate>();

		public List<EventDelegate> onDeActivate = new List<EventDelegate>();

		public List<EventDelegate> onGainFocus = new List<EventDelegate>();

		public List<EventDelegate> onLooseFocus = new List<EventDelegate>();

		public List<EventDelegate> onScreenDeActivated = new List<EventDelegate>();

		public bool ShowBackButton
		{
			get
			{
				return m_showBackButton;
			}
			set
			{
				m_showBackButton = value;
				Singleton<UIScreenManager>.Instance.SceneData.BackButton.UpdateBackButtonVisibility();
			}
		}

		public bool IsActivated { get; private set; }

		public bool IsEntered { get; private set; }

		public CameraMenuManager.EType CameraPosition
		{
			get
			{
				return m_cameraPosition;
			}
			set
			{
				m_cameraPosition = value;
			}
		}

		public BBTargetedTween InTweener
		{
			get
			{
				return m_in;
			}
		}

		public BBTargetedTween OutTweener
		{
			get
			{
				return m_out;
			}
		}

		public bool ShowTutorial
		{
			get
			{
				return m_showTutorial;
			}
		}

		public string Name
		{
			get
			{
				return m_id.ToString();
			}
		}

		public bool Modal
		{
			get
			{
				return m_modal;
			}
			protected set
			{
				m_modal = value;
			}
		}

		public virtual bool Active
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public EScreenID ScreenId
		{
			get
			{
				return m_id;
			}
		}

		private void Awake()
		{
			m_elements = GetComponentsInChildren<UITweenedScreenComponent>();
			m_showBackButton = m_previousScreen != EScreenID.Invalid;
			OnScreenAwoken();
		}

		protected virtual void OnScreenAwoken()
		{
			if ((bool)InTweener)
			{
				InTweener.enabled = false;
			}
			if ((bool)OutTweener)
			{
				OutTweener.enabled = false;
			}
			base.gameObject.AddComponentIfNotExist<UICameraUser>();
		}

		private void Start()
		{
			Enter();
		}

		private void OnDisable()
		{
			OnScreenDisabled();
		}

		protected virtual void OnScreenDisabled()
		{
			if ((bool)m_activatedObject)
			{
				m_activatedObject.SetActive(true);
			}
		}

		public virtual bool CanDeactivate()
		{
			return true;
		}

		public virtual void Init()
		{
			CurrentUIScreen = this;
			EventDelegate.Execute(onInit);
			CurrentUIScreen = null;
		}

		public void Enter()
		{
			if (Singleton<UIScreenManager>.Instance.IsInitialized)
			{
				if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.BackButton != null)
				{
					Singleton<UIScreenManager>.Instance.SceneData.BackButton.UpdateBackButtonVisibility();
				}
				m_trackId = TrackingManager.OpenScreenTrack(Name);
				EventDelegate.Execute(onEnter);
				DoEnter();
				EventDelegate.Execute(onSetup);
				IsEntered = true;
				UpdateAssassinContainer();
			}
		}

		public void Exit()
		{
			if ((bool)m_activatedObject)
			{
				m_activatedObject.SetActive(false);
			}
			TrackingManager.CloseScreenTrack(m_trackId, Name);
			m_trackId = Id<TrackPoint>.Invalid;
			EventDelegate.Execute(onExit);
			DoExit();
			IsEntered = false;
		}

		public void UpdateAssassinContainer()
		{
			if (!m_modal && Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				if (m_showAssassinInfoBox)
				{
					Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Show();
				}
				else
				{
					Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Hide();
				}
			}
		}

		public void PrepareActivate()
		{
			EventDelegate.Execute(onPrepareActivate);
		}

		public void Activate()
		{
			if (m_cameraPosition != CameraMenuManager.EType.None && BehaviourSingleton<EnvironmentBridgeMain>.IsInitialized && BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager != null)
			{
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.Current = m_cameraPosition;
			}
			IsActivated = true;
			EventDelegate.Execute(onActivate);
			DoActivate();
		}

		public void DeActivate()
		{
			IsActivated = false;
			EventDelegate.Execute(onDeActivate);
			DoDeActivate();
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(co_DeActivateScreen());
			}
			else
			{
				EventDelegate.Execute(onScreenDeActivated);
			}
		}

		public void GainFocus()
		{
			EventDelegate.Execute(onGainFocus);
			DoGainFocus();
		}

		public void LooseFocus(UIScreen _old, UIScreen _new)
		{
			EventDelegate.Execute(onLooseFocus);
			DoLooseFocus();
		}

		public GameObject GetActivatedObject()
		{
			return m_activatedObject;
		}

		private void OnDestroy()
		{
			IsEntered = false;
			EventDelegate.Execute(onDestroy);
			ClearListeners();
			OnDestroyScreen();
		}

		private IEnumerator co_DeActivateScreen()
		{
			WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();
			for (int i = 0; i < m_elements.Length; i++)
			{
				if (m_elements[i].Phase == UITweenedScreenComponent.ETriggerPhase.OnScreenDeActivate && !m_elements[i].CanDeactivate)
				{
					yield return waitForFrame;
				}
			}
			EventDelegate.Execute(onScreenDeActivated);
		}

		private void ClearListeners()
		{
			onDestroy.Clear();
			onInit.Clear();
			onEnter.Clear();
			onExit.Clear();
			onActivate.Clear();
			onDeActivate.Clear();
			onGainFocus.Clear();
			onLooseFocus.Clear();
			onScreenDeActivated.Clear();
		}

		protected virtual void DoEnter()
		{
		}

		protected virtual void DoExit()
		{
		}

		protected virtual void DoActivate()
		{
		}

		protected virtual void DoDeActivate()
		{
		}

		protected virtual void DoGainFocus()
		{
		}

		protected virtual void DoLooseFocus()
		{
		}

		protected virtual void OnDestroyScreen()
		{
		}
	}
}
