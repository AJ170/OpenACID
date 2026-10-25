using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Database;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public abstract class UIUnitFrame<T> : UIBase where T : UIUnitFrameBehaviour
	{
		protected T UnitFrameBehaviour;

		public EntityId CurrentTargetId { get; protected set; }

		public override UIBaseBehaviour Details
		{
			get
			{
				return UnitFrameBehaviour;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Invalid;
			}
		}

		protected UIUnitFrame()
		{
			CurrentTargetId = EntityId.InvalidId;
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			if (base.IsActiveOnSceneLoaded)
			{
				Init();
			}
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			UnitFrameBehaviour = (T)null;
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<BuffUiEvent>(OnBuffUiEvent);
			}
		}

		public override void OnMissionUnload()
		{
			if (!((Object)UnitFrameBehaviour == (Object)null))
			{
				UnitFrameBehaviour.gameObject.SetActive(false);
			}
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				Hide();
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<BuffUiEvent>(OnBuffUiEvent);
				base.OnMissionUnloaded();
				base.IsInitialized = false;
			}
		}

		public override bool Tap(TapEvent _gesture)
		{
			return false;
		}

		public override void UpdateFrame(float _deltaTime)
		{
		}

		protected bool HasValidPortraitData(EntityId _entityId)
		{
			IReadonlyHumanoidDef humanoidDef = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetHumanoidDef(_entityId);
			if (humanoidDef == null)
			{
				return false;
			}
			return humanoidDef.BodyPortrait.GetObject<Texture2D>() != null;
		}

		protected void Hide()
		{
			SetVisible(false);
		}

		protected virtual void Show(EntityId _entityId)
		{
			CurrentTargetId = _entityId;
			UnitFrameBehaviour.SetPortraitData(_entityId);
		}

		private void Init()
		{
			base.IsInitialized = true;
			if (!((Object)UnitFrameBehaviour == (Object)null))
			{
				return;
			}
			UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
			if (widgetByType != null)
			{
				UnitFrameBehaviour = widgetByType as T;
				if ((Object)UnitFrameBehaviour != (Object)null)
				{
					UnitFrameBehaviour.Init();
				}
			}
		}

		private void OnBuffUiEvent(BuffUiEvent _message)
		{
			if (_message.Type != EBuffType.Invalid && _message.Target == CurrentTargetId)
			{
				if (_message.State == BuffUiEvent.EState.Active)
				{
					UnitFrameBehaviour.AddBuff(_message);
				}
				else
				{
					UnitFrameBehaviour.RemoveBuff(_message);
				}
			}
		}
	}
}
