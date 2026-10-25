using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using Mission;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UIOnScreenMessage : UIBase
	{
		public class QueueMessage
		{
			public string Message = string.Empty;

			public bool IsValid = true;

			public EInfoEventType EventType = EInfoEventType.TextAnnotation;

			public InfoEvent.ETextAnnotationMode AnnotationMode;

			public Texture Texture;

			public int UniqueId = -1;

			public Action OnHide;
		}

		private UIOnScreenMessageBehaviour m_onScreenMessageBehaviour;

		private readonly BBSimpleList<QueueMessage> m_queuedMessagesQueue = new BBSimpleList<QueueMessage>();

		private bool m_isOutroRunning;

		private bool m_wasMessageVisibleBeforePause;

		private QueueMessage m_currentMessage;

		public bool IsQueueEmpty
		{
			get
			{
				return m_currentMessage == null;
			}
		}

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.OnScreenMessage;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return ScreenMessageBehaviour;
			}
		}

		public UIOnScreenMessageBehaviour ScreenMessageBehaviour
		{
			get
			{
				return m_onScreenMessageBehaviour;
			}
			set
			{
				if (m_onScreenMessageBehaviour != null)
				{
					m_onScreenMessageBehaviour.Hidden -= OnScreenMessageBehaviourOnHidden;
				}
				m_onScreenMessageBehaviour = value;
				if (m_onScreenMessageBehaviour != null)
				{
					m_onScreenMessageBehaviour.Hidden += OnScreenMessageBehaviourOnHidden;
				}
			}
		}

		private void OnScreenMessageBehaviourOnHidden()
		{
			if (m_currentMessage != null && m_currentMessage.OnHide != null)
			{
				m_currentMessage.OnHide();
			}
			m_currentMessage = null;
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			m_isOutroRunning = false;
			base.IsActive = true;
			base.IsActiveOnSceneLoaded = true;
			Init();
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Combine(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPauseChanged));
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			m_isOutroRunning = false;
			base.IsActive = false;
			base.IsInitialized = false;
			m_queuedMessagesQueue.Clear();
			ScreenMessageBehaviour = null;
			m_wasMessageVisibleBeforePause = false;
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Remove(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPauseChanged));
		}

		public override void OnMissionLoaded()
		{
			base.OnMissionLoaded();
			InitLogic();
		}

		public override void OnMissionUnload()
		{
			if (!IsActive)
			{
				base.OnMissionUnload();
				if (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<InfoEvent>(OnSendInfoEvent);
				}
			}
		}

		public override void OnMissionUnloaded()
		{
			base.OnMissionUnloaded();
			m_queuedMessagesQueue.Clear();
		}

		public override void OnMissionEnded()
		{
			base.OnMissionEnded();
			if (IsActive)
			{
				m_queuedMessagesQueue.Clear();
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive)
			{
				m_isOutroRunning = true;
				m_queuedMessagesQueue.Clear();
				SetVisible(false);
			}
		}

		public override bool Tap(TapEvent _gesture)
		{
			return true;
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			if (ScreenMessageBehaviour != null)
			{
				ScreenMessageBehaviour.UpdateScreenMessage();
			}
			UpdateQueue();
		}

		public void ShowMessage(string _message, bool _immediately = false, Action _callback = null)
		{
			ShowMessage(new QueueMessage
			{
				Message = _message,
				EventType = EInfoEventType.TextAnnotation,
				OnHide = _callback
			}, _immediately);
		}

		public void ClearQueue()
		{
			m_queuedMessagesQueue.Clear();
			m_currentMessage = null;
		}

		private void Init()
		{
			if (ScreenMessageBehaviour != null)
			{
				return;
			}
			if (Main.Instance.SceneLoader.InAnimus)
			{
				ScreenMessageBehaviour = Singleton<UIScreenManager>.Instance.SceneData.UIOnScreenMessage;
			}
			else
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					ScreenMessageBehaviour = widgetByType as UIOnScreenMessageBehaviour;
				}
			}
			if (!(ScreenMessageBehaviour == null))
			{
				ScreenMessageBehaviour.gameObject.SetActive(false);
				base.IsInitialized = true;
			}
		}

		private void InitLogic()
		{
			if (base.IsInitialized && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<InfoEvent>(OnSendInfoEvent);
			}
		}

		private void OnPauseChanged(Timer.EPauseState _pauseReason)
		{
			if (m_currentMessage == null || (m_currentMessage.AnnotationMode != 0 && m_currentMessage.AnnotationMode != InfoEvent.ETextAnnotationMode.ImageOpen && m_currentMessage.AnnotationMode != InfoEvent.ETextAnnotationMode.MessageOpen))
			{
				return;
			}
			switch (_pauseReason)
			{
			case Timer.EPauseState.Running:
				if (m_wasMessageVisibleBeforePause)
				{
					ScreenMessageBehaviour.gameObject.SetActive(true);
				}
				m_wasMessageVisibleBeforePause = false;
				break;
			case Timer.EPauseState.PauseGeneral:
			case Timer.EPauseState.PauseOptions:
			case Timer.EPauseState.PauseDialog:
				m_wasMessageVisibleBeforePause = ScreenMessageBehaviour.gameObject.activeSelf;
				ScreenMessageBehaviour.gameObject.SetActive(false);
				break;
			}
		}

		private void UpdateQueue()
		{
			EMissionState missionState = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState;
			bool flag = missionState == EMissionState.InProgress || missionState == EMissionState.Failed || missionState == EMissionState.Success || Main.Instance.SceneLoader.InAnimus;
			if (UbiCore.Utils.Singleton<Timer>.Instance.IsPaused() || !flag || !(ScreenMessageBehaviour != null) || ScreenMessageBehaviour.gameObject.activeSelf || m_queuedMessagesQueue.Count <= 0)
			{
				return;
			}
			m_currentMessage = m_queuedMessagesQueue[0];
			m_queuedMessagesQueue.RemoveAt(0);
			while (m_queuedMessagesQueue.Count > 0 && (m_currentMessage == null || !m_currentMessage.IsValid))
			{
				m_currentMessage = m_queuedMessagesQueue[0];
				m_queuedMessagesQueue.RemoveAt(0);
			}
			if (m_currentMessage != null && m_currentMessage.IsValid)
			{
				int displayTime = int.MaxValue;
				switch (m_currentMessage.AnnotationMode)
				{
				case InfoEvent.ETextAnnotationMode.ImageOpen:
					displayTime = 10;
					break;
				default:
					displayTime = CalculatedAnimationTime(m_currentMessage.Message);
					break;
				case InfoEvent.ETextAnnotationMode.MessageOpenExternal:
				case InfoEvent.ETextAnnotationMode.ImageOpenExternal:
					break;
				}
				ScreenMessageBehaviour.ShowMessage(m_currentMessage, displayTime);
			}
		}

		private int CalculatedAnimationTime(string _message)
		{
			int num = _message.Split(' ').Length;
			int num2 = 2;
			num2 += Mathf.FloorToInt((float)num / 2.1f);
			if (m_queuedMessagesQueue.Count > 0)
			{
				num2--;
			}
			num2 = Mathf.Max(5, num2);
			return Mathf.Min(num2, 15);
		}

		private void ShowMessage(QueueMessage _newMessage, bool _immediately)
		{
			if (ScreenMessageBehaviour == null || m_isOutroRunning || (ScreenMessageBehaviour.gameObject.activeSelf && IsDuplicateFreeText(_newMessage, m_currentMessage)))
			{
				return;
			}
			for (int num = m_queuedMessagesQueue.Count - 1; num >= 0; num--)
			{
				QueueMessage queueMessage = m_queuedMessagesQueue[num];
				if (queueMessage == null)
				{
					m_queuedMessagesQueue.RemoveAt(num);
				}
			}
			if (ScreenMessageBehaviour.gameObject.activeSelf && _newMessage != null && m_currentMessage != null && _newMessage.EventType != EInfoEventType.TextAnnotation && _newMessage.EventType == m_currentMessage.EventType)
			{
				for (int i = 0; i < m_queuedMessagesQueue.Count; i++)
				{
					QueueMessage queueMessage2 = m_queuedMessagesQueue[i];
					if (queueMessage2.IsValid)
					{
						ValidateOldMessages(queueMessage2, _newMessage);
					}
				}
				return;
			}
			for (int j = 0; j < m_queuedMessagesQueue.Count; j++)
			{
				QueueMessage queueMessage3 = m_queuedMessagesQueue[j];
				if (IsDuplicateFreeText(_newMessage, queueMessage3))
				{
					return;
				}
				if (queueMessage3.IsValid)
				{
					ValidateOldMessages(queueMessage3, _newMessage);
				}
			}
			if (_immediately)
			{
				m_queuedMessagesQueue.Insert(0, _newMessage);
				if (ScreenMessageBehaviour != null && ScreenMessageBehaviour.gameObject.activeSelf)
				{
					ScreenMessageBehaviour.gameObject.SetActive(false);
				}
			}
			else
			{
				m_queuedMessagesQueue.Add(_newMessage);
			}
			UpdateQueue();
		}

		private static bool IsDuplicateFreeText(QueueMessage _newMessage, QueueMessage _existingMessage)
		{
			if (_newMessage == null || _existingMessage == null)
			{
				return false;
			}
			return _newMessage.EventType == EInfoEventType.TextAnnotation && _newMessage.Message == _existingMessage.Message;
		}

		private void ShowMessage(InfoEvent _messageEvent, bool _immediately)
		{
			string message = UIOnScreenMessageBehaviour.GetMessage(_messageEvent);
			if (!string.IsNullOrEmpty(message) || _messageEvent.Texture != null)
			{
				ShowMessage(new QueueMessage
				{
					Message = message,
					EventType = _messageEvent.InfoType,
					AnnotationMode = _messageEvent.AnnotationMode,
					Texture = _messageEvent.Texture,
					UniqueId = _messageEvent.UniqueId
				}, _immediately);
			}
		}

		private void HideMessage(InfoEvent _messageEvent)
		{
			if (m_currentMessage != null && m_currentMessage.UniqueId == _messageEvent.UniqueId)
			{
				SetVisible(false);
				return;
			}
			int num = m_queuedMessagesQueue.FindIndex((QueueMessage _msg) => _msg.UniqueId == _messageEvent.UniqueId);
			if (num >= 0)
			{
				m_queuedMessagesQueue.RemoveAt(num);
			}
		}

		private void OnSendInfoEvent(InfoEvent _message)
		{
			if (!base.HasMissionEnded)
			{
				switch (_message.AnnotationMode)
				{
				case InfoEvent.ETextAnnotationMode.Invalid:
				case InfoEvent.ETextAnnotationMode.MessageOpen:
				case InfoEvent.ETextAnnotationMode.MessageOpenExternal:
				case InfoEvent.ETextAnnotationMode.ImageOpen:
				case InfoEvent.ETextAnnotationMode.ImageOpenExternal:
					ShowInfoEventMessage(_message);
					break;
				case InfoEvent.ETextAnnotationMode.MessageCloseExternal:
				case InfoEvent.ETextAnnotationMode.ImageCloseExternal:
					HideMessage(_message);
					break;
				case InfoEvent.ETextAnnotationMode.PopupOpenConfirm:
				case InfoEvent.ETextAnnotationMode.PopupOpenExternal:
				case InfoEvent.ETextAnnotationMode.PopupCloseExternal:
					break;
				}
			}
		}

		private void ShowInfoEventMessage(InfoEvent _message)
		{
			if (IsAllowedToShowMessage(_message))
			{
				string message = UIOnScreenMessageBehaviour.GetMessage(_message);
				if (!string.IsNullOrEmpty(message))
				{
					ShowMessage(_message, _message.Immediate);
				}
			}
		}

		private void ValidateOldMessages(QueueMessage _oldMessage, QueueMessage _newMessage)
		{
			switch (_newMessage.EventType)
			{
			case EInfoEventType.RestrictedAreaEnter:
				if (_oldMessage.EventType == EInfoEventType.RestrictedAreaLeave)
				{
					_oldMessage.IsValid = false;
				}
				break;
			case EInfoEventType.RestrictedAreaLeave:
				if (_oldMessage.EventType == EInfoEventType.RestrictedAreaEnter)
				{
					_oldMessage.IsValid = false;
				}
				break;
			case EInfoEventType.AnonymousLevel:
				if (_oldMessage.EventType == EInfoEventType.AlertedLevel || _oldMessage.EventType == EInfoEventType.ExposedLevel)
				{
					_oldMessage.IsValid = false;
				}
				break;
			case EInfoEventType.AlertedLevel:
				if (_oldMessage.EventType == EInfoEventType.AnonymousLevel || _oldMessage.EventType == EInfoEventType.ExposedLevel)
				{
					_oldMessage.IsValid = false;
				}
				break;
			case EInfoEventType.ExposedLevel:
				if (_oldMessage.EventType == EInfoEventType.AnonymousLevel || _oldMessage.EventType == EInfoEventType.AlertedLevel)
				{
					_oldMessage.IsValid = false;
				}
				break;
			case EInfoEventType.TailingTargetGetsAway:
				if (_oldMessage.EventType == EInfoEventType.TailingKeepLineOfSight)
				{
					_oldMessage.IsValid = false;
				}
				break;
			case EInfoEventType.TailingKeepLineOfSight:
			{
				EInfoEventType eventType = _oldMessage.EventType;
				if (eventType == EInfoEventType.TailingTargetGetsAway || eventType == EInfoEventType.TailingKeepLineOfSight)
				{
					_oldMessage.IsValid = false;
				}
				break;
			}
			case EInfoEventType.WrongChest:
			case EInfoEventType.CorrectChest:
			case EInfoEventType.DoctorUsed:
			case EInfoEventType.EagleVisionSuccess:
			case EInfoEventType.TextAnnotation:
			case EInfoEventType.CharacterAlarmed:
			case EInfoEventType.CharacterRejects:
				break;
			}
		}

		private bool IsAllowedToShowMessage(InfoEvent _info)
		{
			bool result = true;
			switch (_info.InfoType)
			{
			case EInfoEventType.RestrictedAreaEnter:
			case EInfoEventType.RestrictedAreaLeave:
			case EInfoEventType.AnonymousLevel:
			case EInfoEventType.AlertedLevel:
			case EInfoEventType.ExposedLevel:
				result = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIMiniMap>().IsVisible;
				break;
			}
			return result;
		}
	}
}
