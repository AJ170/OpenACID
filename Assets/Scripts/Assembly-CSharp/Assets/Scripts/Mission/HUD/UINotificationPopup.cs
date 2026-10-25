using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UINotificationPopup : UIBase
	{
		private class NotificatioQueueConfigBase
		{
			public string Id;

			public string HeaderText;

			public string InfoText;

			public UnityResource TexResource;

			public UINotificationPopupBehaviour.ETextureType Type;
		}

		private class NotificatioQueueConfigAchievement : NotificatioQueueConfigBase
		{
			public int RankIconIndex;
		}

		private class NotificatioQueueConfigProgression : NotificatioQueueConfigBase
		{
			public string ResourceGuid;

			public long StartValue;

			public int EndValue;

			public long ProgressValue;

			public string TexResourcePath;
		}

		private UINotificationPopupBehaviour m_behaviour;

		private readonly Queue<NotificatioQueueConfigBase> m_notificatioQueue = new Queue<NotificatioQueueConfigBase>();

		private NotificatioQueueConfigBase m_currentNotification;

		private bool m_isShowingNotificationPaused;

		public bool IsShowingNotificationPaused
		{
			get
			{
				return m_isShowingNotificationPaused;
			}
			set
			{
				if (m_isShowingNotificationPaused != value)
				{
					m_isShowingNotificationPaused = value;
					if (value)
					{
						DelayCurrentNotification();
					}
				}
			}
		}

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Notification;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return NotificatioBehaviour;
			}
		}

		private UINotificationPopupBehaviour NotificatioBehaviour
		{
			get
			{
				return m_behaviour;
			}
			set
			{
				if (m_behaviour != null)
				{
					m_behaviour.Hidden -= OnNotificatioBehaviourOnHidden;
				}
				m_behaviour = value;
				if (m_behaviour != null)
				{
					m_behaviour.Hidden += OnNotificatioBehaviourOnHidden;
				}
			}
		}

		public override bool InitiallyVisible
		{
			get
			{
				return false;
			}
		}

		public override void OnUIReadyToUse()
		{
			Singleton<StateManager>.Instance.AchievementsManager.AchievementReached += OnAchievementReached;
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			base.IsActive = true;
			base.IsActiveOnSceneLoaded = true;
			Init();
			UbiCore.Utils.Singleton<LocalStorage>.Instance.OnCodexEntryUnlocked += OnCodexEntryUnlocked;
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			base.IsActive = false;
			base.IsInitialized = false;
			IsShowingNotificationPaused = true;
			NotificatioBehaviour = null;
			UbiCore.Utils.Singleton<LocalStorage>.Instance.OnCodexEntryUnlocked -= OnCodexEntryUnlocked;
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				DelayCurrentNotification();
			}
		}

		private void DelayCurrentNotification()
		{
			SetVisible(false);
			if (m_currentNotification != null)
			{
				ReEnqueueCurrentNotification(m_currentNotification);
			}
		}

		public override void OnMissionEnded()
		{
			base.OnMissionEnded();
			if (IsActive)
			{
				IsShowingNotificationPaused = true;
				if (m_currentNotification != null)
				{
					ReEnqueueCurrentNotification(m_currentNotification);
				}
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive)
			{
				SetVisible(false);
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (ShouldUpdate())
			{
				base.UpdateFrame(_deltaTime);
				NotificatioBehaviour.UpdateNotification();
				UpdateQueue();
			}
		}

		private void Init()
		{
			if (NotificatioBehaviour != null)
			{
				return;
			}
			if (Main.Instance.SceneLoader.InAnimus)
			{
				NotificatioBehaviour = Singleton<UIScreenManager>.Instance.SceneData.UINotificationPopupBehaviour;
			}
			else
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					NotificatioBehaviour = widgetByType as UINotificationPopupBehaviour;
				}
			}
			if (!(NotificatioBehaviour == null))
			{
				NotificatioBehaviour.Init();
				base.IsInitialized = true;
			}
		}

		private void ReEnqueueCurrentNotification(NotificatioQueueConfigBase _configBase)
		{
			if (_configBase == null)
			{
				return;
			}
			foreach (NotificatioQueueConfigBase item in m_notificatioQueue)
			{
				if (item.Type == _configBase.Type && item.Id == _configBase.Id)
				{
					return;
				}
			}
			m_notificatioQueue.Enqueue(_configBase);
		}

		private void ShowNotification(NotificatioQueueConfigBase _configBase)
		{
			foreach (NotificatioQueueConfigBase item in m_notificatioQueue)
			{
				if (item.Type == _configBase.Type && item.Id == _configBase.Id)
				{
					return;
				}
			}
			m_notificatioQueue.Enqueue(_configBase);
			if (ShouldUpdate())
			{
				UpdateQueue();
			}
		}

		private void UpdateQueue()
		{
			if (NotificatioBehaviour != null && !NotificatioBehaviour.gameObject.activeSelf && m_notificatioQueue.Count > 0)
			{
				m_currentNotification = m_notificatioQueue.Dequeue();
				int calculatedAnimationTime = CalculatedAnimationTime(m_currentNotification.InfoText);
				if (m_currentNotification is NotificatioQueueConfigAchievement)
				{
					NotificatioQueueConfigAchievement notificatioQueueConfigAchievement = m_currentNotification as NotificatioQueueConfigAchievement;
					NotificatioBehaviour.ShowNotification(notificatioQueueConfigAchievement.HeaderText, notificatioQueueConfigAchievement.InfoText, notificatioQueueConfigAchievement.Type, notificatioQueueConfigAchievement.TexResource, calculatedAnimationTime, notificatioQueueConfigAchievement.RankIconIndex);
				}
				else if (m_currentNotification is NotificatioQueueConfigProgression)
				{
					NotificatioQueueConfigProgression notificatioQueueConfigProgression = m_currentNotification as NotificatioQueueConfigProgression;
					NotificatioBehaviour.ShowNotification(notificatioQueueConfigProgression.HeaderText, notificatioQueueConfigProgression.StartValue, notificatioQueueConfigProgression.EndValue, notificatioQueueConfigProgression.ProgressValue, notificatioQueueConfigProgression.ResourceGuid, notificatioQueueConfigProgression.TexResourcePath, calculatedAnimationTime);
				}
				else
				{
					NotificatioBehaviour.ShowNotification(m_currentNotification.HeaderText, m_currentNotification.InfoText, m_currentNotification.Type, m_currentNotification.TexResource, calculatedAnimationTime);
				}
			}
		}

		private int CalculatedAnimationTime(string _message)
		{
			int result = 5;
			if (!string.IsNullOrEmpty(_message))
			{
				int num = _message.Split(' ').Length;
				result = 2;
				result += Mathf.FloorToInt((float)num / 2.1f);
				if (m_notificatioQueue.Count > 0)
				{
					result--;
				}
				result = Mathf.Max(5, result);
				result = Mathf.Min(result, 15);
			}
			return result;
		}

		public void CreateQuestNotificatioPopup(string _id, ProgressionQuests.RankQuests.Quest _quest, long _previousValue)
		{
			NotificatioQueueConfigProgression notificatioQueueConfigProgression = new NotificatioQueueConfigProgression();
			notificatioQueueConfigProgression.Id = _id;
			notificatioQueueConfigProgression.HeaderText = LocalizationManager.Instance.Get(_quest.titleLocaKey);
			notificatioQueueConfigProgression.Type = UINotificationPopupBehaviour.ETextureType.NormalTexture;
			notificatioQueueConfigProgression.TexResourcePath = "UI/Texture/aUI_img_daily_tracker";
			notificatioQueueConfigProgression.StartValue = _previousValue;
			notificatioQueueConfigProgression.EndValue = (int)_quest.numberOfTimesToPerform;
			notificatioQueueConfigProgression.ProgressValue = _quest.progress;
			NotificatioQueueConfigProgression configBase = notificatioQueueConfigProgression;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_unlock_achievement);
			ShowNotification(configBase);
		}

		public void CreateEventNotificatioPopup(EventsProgress.ChallengeEvent _eventProgress, long _previousValue)
		{
			EventProgressionChallengesManager eventProgressionChallengesManager = (EventProgressionChallengesManager)Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges);
			EventProgressionChallenges firstValidEventProgressionEvent = eventProgressionChallengesManager.GetFirstValidEventProgressionEvent();
			long challengeCount = _eventProgress.GetChallengeCount();
			NotificatioQueueConfigProgression notificatioQueueConfigProgression = new NotificatioQueueConfigProgression();
			notificatioQueueConfigProgression.Id = _eventProgress.challengeId;
			notificatioQueueConfigProgression.HeaderText = _eventProgress.GetChallengeHeaderText();
			notificatioQueueConfigProgression.Type = UINotificationPopupBehaviour.ETextureType.NormalTexture;
			notificatioQueueConfigProgression.ResourceGuid = firstValidEventProgressionEvent.EventProgressionData.LayoutTheme.NotificationImageGuiId;
			notificatioQueueConfigProgression.StartValue = _previousValue;
			notificatioQueueConfigProgression.EndValue = (int)_eventProgress.GetFormatedTimesToCompleteVaule();
			notificatioQueueConfigProgression.ProgressValue = challengeCount;
			NotificatioQueueConfigProgression configBase = notificatioQueueConfigProgression;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_unlock_achievement);
			ShowNotification(configBase);
		}

		private void OnAchievementReached(AchievementMapping.Definition _definition, int _rank)
		{
			AchievementDefinition achievementDefinition = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementDefinition(_definition.id);
			int rankIconIndex = _definition.ranks.Length - _rank;
			NotificatioQueueConfigAchievement notificatioQueueConfigAchievement = new NotificatioQueueConfigAchievement();
			notificatioQueueConfigAchievement.Id = _definition.id;
			notificatioQueueConfigAchievement.RankIconIndex = rankIconIndex;
			notificatioQueueConfigAchievement.HeaderText = LocalizationManager.Instance.Get(5631);
			notificatioQueueConfigAchievement.InfoText = LocalizationManager.Instance.Get(achievementDefinition.NameLocaKey.Key);
			notificatioQueueConfigAchievement.Type = UINotificationPopupBehaviour.ETextureType.Achievement;
			notificatioQueueConfigAchievement.TexResource = achievementDefinition.Icon;
			NotificatioQueueConfigAchievement configBase = notificatioQueueConfigAchievement;
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_unlock_achievement);
			ShowNotification(configBase);
		}

		private void OnCodexEntryUnlocked(GameID _gameID)
		{
			CodexDefinition item = Main.Instance.GameDB.GetItem<CodexDefinition>(_gameID);
			NotificatioQueueConfigBase notificatioQueueConfigBase = new NotificatioQueueConfigBase();
			notificatioQueueConfigBase.Id = _gameID.ToString();
			notificatioQueueConfigBase.HeaderText = LocalizationManager.Instance.Get(3804);
			notificatioQueueConfigBase.InfoText = LocalizationManager.Instance.Get(item.HeaderLocaKey.Key);
			notificatioQueueConfigBase.Type = UINotificationPopupBehaviour.ETextureType.NormalTexture;
			notificatioQueueConfigBase.TexResource = item.Thumbnail;
			NotificatioQueueConfigBase configBase = notificatioQueueConfigBase;
			ShowNotification(configBase);
		}

		private void OnNotificatioBehaviourOnHidden()
		{
			m_currentNotification = null;
		}

		private bool ShouldUpdate()
		{
			return !IsShowingNotificationPaused && NotificatioBehaviour != null && (!Main.Instance.SceneLoader.InAnimus || !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive || NotificatioBehaviour.gameObject.activeSelf);
		}
	}
}
