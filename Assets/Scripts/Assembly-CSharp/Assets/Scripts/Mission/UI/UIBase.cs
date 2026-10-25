using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using SharedBaseLib.System;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public abstract class UIBase : UISubSystemLayout, IUISubSystemLayout, IUiSubSystem
	{
		private UIRoot m_uiRoot;

		public abstract UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType { get; }

		public abstract UIBaseBehaviour Details { get; }

		public virtual bool IsVisible
		{
			get
			{
				return Details != null && Details.gameObject != null && Details.gameObject.activeInHierarchy;
			}
		}

		public virtual UIRoot UIMainRoot
		{
			get
			{
				if (m_uiRoot == null)
				{
					for (int i = 0; i < UIRoot.list.Count; i++)
					{
						UIRoot uIRoot = UIRoot.list[i];
						if (uIRoot.gameObject.layer == Layers.UILayer)
						{
							m_uiRoot = uIRoot;
							break;
						}
					}
				}
				return m_uiRoot;
			}
		}

		public bool IsActive { get; protected set; }

		public bool IsActiveOnSceneLoaded { get; protected set; }

		public virtual bool InitiallyVisible
		{
			get
			{
				return true;
			}
		}

		public bool IsInitialized { get; protected set; }

		public bool HasMissionEnded { get; private set; }

		public virtual void UpdateFrame(float _deltaTime)
		{
		}

		public virtual bool Tap(TapEvent _gesture)
		{
			return false;
		}

		public virtual bool IsOnUi(Vector2 _position)
		{
			return UICamera.isOverUI;
		}

		public virtual void OnSceneLoaded()
		{
			HasMissionEnded = false;
			IsActiveOnSceneLoaded = !Main.Instance.SceneLoader.InAnimus;
		}

		public virtual void OnSceneUnload()
		{
		}

		public virtual void OnCameraCreated()
		{
		}

		public virtual void OnSharedUILoaded()
		{
		}

		public virtual void OnUIAlignmentChanged(bool _isRightHanded)
		{
		}

		public virtual void OnUIReadyToUse()
		{
		}

		public virtual void SetVisible(bool _value)
		{
			if (Details != null)
			{
				Details.SetVisibility(_value);
			}
		}

		public virtual void OnMissionEnded()
		{
			HasMissionEnded = true;
		}

		public virtual void OnMissionOutroStarted()
		{
		}

		public virtual void OnMissionLoad()
		{
		}

		public virtual void OnMissionLoaded()
		{
			HasMissionEnded = false;
			IsActive = !Main.Instance.SceneLoader.InAnimus;
		}

		public virtual void OnMissionUnload()
		{
		}

		public virtual void OnMissionUnloaded()
		{
			IsActive = false;
			IsInitialized = false;
		}
	}
}
