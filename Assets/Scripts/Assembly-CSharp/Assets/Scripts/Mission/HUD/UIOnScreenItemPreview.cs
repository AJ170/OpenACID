using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UIOnScreenItemPreview : UIBase
	{
		public class QueueItem
		{
			public InventoryElement Item;

			public GameID GameID;

			public Color LootQualityColor;

			public int LootCount;

			public string LootName;
		}

		private UIOnScreenItemPreviewBehaviour m_onScreenBehaviour;

		private readonly BBSimpleList<QueueItem> m_queuedMessagesQueue = new BBSimpleList<QueueItem>();

		private QueueItem m_currentItem;

		public bool IsQueueEmpty
		{
			get
			{
				return m_currentItem == null;
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
				return UIScreenWidgetManager.EUiScreenWidget.OnScreenPreview;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return ScreenBehaviour;
			}
		}

		public UIOnScreenItemPreviewBehaviour ScreenBehaviour
		{
			get
			{
				return m_onScreenBehaviour;
			}
			set
			{
				if (m_onScreenBehaviour != null)
				{
					m_onScreenBehaviour.Hidden -= OnBehaviourHidden;
				}
				m_onScreenBehaviour = value;
				if (m_onScreenBehaviour != null)
				{
					m_onScreenBehaviour.Hidden += OnBehaviourHidden;
				}
			}
		}

		private void OnBehaviourHidden()
		{
			m_currentItem = null;
		}

		public override void OnSceneLoaded()
		{
			base.OnSceneLoaded();
			base.IsActive = true;
			base.IsActiveOnSceneLoaded = true;
			Init();
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			base.IsActive = false;
			base.IsInitialized = false;
			m_queuedMessagesQueue.Clear();
			ScreenBehaviour = null;
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
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatUiEvent>(OnCombatUiEvent);
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
			UpdateQueue();
		}

		public void ShowMessage(GameID _lootGameID, Color _lootQualityColor, string _lootName, int _count)
		{
			ValidateAndAddItemToQuest(new QueueItem
			{
				GameID = _lootGameID,
				LootQualityColor = _lootQualityColor,
				LootName = _lootName,
				LootCount = _count
			});
		}

		public void ShowMessage(InventoryElement _element)
		{
			ValidateAndAddItemToQuest(new QueueItem
			{
				Item = _element
			});
		}

		public void ClearQueue()
		{
			m_queuedMessagesQueue.Clear();
			m_currentItem = null;
		}

		private void Init()
		{
			if (!(ScreenBehaviour != null) && !Main.Instance.SceneLoader.InAnimus)
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					ScreenBehaviour = widgetByType as UIOnScreenItemPreviewBehaviour;
				}
				if (!(ScreenBehaviour == null))
				{
					ScreenBehaviour.gameObject.SetActive(false);
					base.IsInitialized = true;
				}
			}
		}

		private void InitLogic()
		{
			if (base.IsInitialized && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatUiEvent>(OnCombatUiEvent);
			}
		}

		private void UpdateQueue()
		{
			EMissionState missionState = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState;
			bool flag = missionState == EMissionState.InProgress || missionState == EMissionState.Failed || missionState == EMissionState.Success || Main.Instance.SceneLoader.InAnimus;
			if (!UbiCore.Utils.Singleton<Timer>.Instance.IsPaused() && flag && ScreenBehaviour != null && !ScreenBehaviour.gameObject.activeSelf && m_queuedMessagesQueue.Count > 0)
			{
				m_currentItem = m_queuedMessagesQueue[0];
				m_queuedMessagesQueue.RemoveAt(0);
				if (m_currentItem != null)
				{
					ScreenBehaviour.ShowMessage(m_currentItem);
				}
			}
		}

		public void CreateMessagesDebug(MissionGoalLoot _goalLoot)
		{
			CreateMessagesByMissionGoalLoot(_goalLoot);
		}

		private void OnCombatUiEvent(CombatUiEvent _message)
		{
			if (!base.HasMissionEnded && _message.Type == CombatUiEvent.EType.MissionGoalLoot)
			{
				CreateMessagesByMissionGoalLoot(_message.MissionGoalLoot);
			}
		}

		private void CreateMessagesByMissionGoalLoot(MissionGoalLoot _goalLoot)
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			int heroicMissionRank = Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank;
			InventoryElement inventoryElement = profile.GatInventoryElementByCustomType(ECustomLootTemplateType.LockPick);
			GameID lootGameID = Main.Instance.GameDB.FindGameIdFromGuid(inventoryElement.guid);
			int requiredLockPicks = EntitlementExtensions.GetHeroicRankMapping(heroicMissionRank).requiredLockPicks;
			string lootName = LocalizationManager.Instance.Get(9701);
			ShowMessage(lootGameID, UIDataMapper.UITextColors.LockPickQualityColor, lootName, -requiredLockPicks);
			for (int i = 0; i < _goalLoot.elements.elements.Length; i++)
			{
				InventoryElement element = _goalLoot.elements.elements[i];
				ShowMessage(element);
			}
			for (int j = 0; j < _goalLoot.resources.Length; j++)
			{
				ResourceFromTemplate resourceFromTemplate = _goalLoot.resources[j];
				ResourceDescription value;
				if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)resourceFromTemplate.resource.type, out value))
				{
					Color itemQualityColor = UIItemUtil.GetItemQualityColor(resourceFromTemplate.quality);
					string resourceLocaName = resourceFromTemplate.resource.GetResourceLocaName();
					ShowMessage(value.ItemGUID, itemQualityColor, resourceLocaName, (int)resourceFromTemplate.resource.amount);
				}
			}
		}

		private void ValidateAndAddItemToQuest(QueueItem _newItem)
		{
			if (!(ScreenBehaviour == null))
			{
				m_queuedMessagesQueue.Add(_newItem);
				UpdateQueue();
			}
		}
	}
}
