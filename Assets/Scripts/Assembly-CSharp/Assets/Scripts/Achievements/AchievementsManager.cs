using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Native;
using Assets.Scripts.UI.State;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.Achievements
{
	public class AchievementsManager
	{
		public delegate void CounterUpdatedHandler();

		public delegate void AchievementDataUpdatedHandler();

		public delegate void AchievementReachedHandler(AchievementMapping.Definition _definition, int _rank);

		public static readonly int AchievementCounterCount;

		private bool m_eventsCreated;

		private readonly BBHashSet<string> m_AchievementsCache = new BBHashSet<string>();

		private readonly AchievementCategoryData m_achievementCategoryRootData = new AchievementCategoryData();

		private readonly BBDictionary<string, AchievementCategoryData> m_idToAchievementCategoryData = new BBDictionary<string, AchievementCategoryData>();

		private readonly BBList<ulong> m_Counters = new BBList<ulong>(AchievementCounterCount);

		private readonly BBHashSet<string> m_ReportedAchievements = new BBHashSet<string>();

		private readonly BBDictionary<int, string> m_CounterToGcAchievement = new BBDictionary<int, string>();

		private readonly BBDictionary<string, AchievementMapping.Definition> m_GcAchievementToAcierAchievement = new BBDictionary<string, AchievementMapping.Definition>();

		private readonly BBDictionary<string, AchievementMapping.Definition> m_IdToAcierAchievement = new BBDictionary<string, AchievementMapping.Definition>();

		private readonly BBDictionary<string, AchievementCategoryMapping.Category> m_IdToAcierAchievementCategory = new BBDictionary<string, AchievementCategoryMapping.Category>();

		private bool m_ReadyToProcess;

		private bool m_ThirdPartyCountersAvailable;

		private static readonly BBList<string> s_CounterTypeNames;

		private static readonly BBHashSet<int> m_IngameCounterTypes;

		public AchievementCategoryData AchievementCategoryData
		{
			get
			{
				return m_achievementCategoryRootData;
			}
		}

		public BBDictionary<int, string> CounterToGcAchievement
		{
			get
			{
				return m_CounterToGcAchievement;
			}
		}

		public event CounterUpdatedHandler CountersUpdated;

		public event AchievementDataUpdatedHandler AchievementDataUpdated;

		public event AchievementReachedHandler AchievementReached;

		public AchievementsManager()
		{
			InitEvents();
		}

		static AchievementsManager()
		{
			m_IngameCounterTypes = new BBHashSet<int>(new int[43]
			{
				7, 8, 10, 12, 13, 14, 22, 34, 35, 36,
				37, 38, 39, 40, 41, 42, 43, 44, 45, 46,
				47, 48, 49, 50, 56, 57, 58, 59, 60, 62,
				63, 65, 66, 67, 85, 124, 129, 130, 131, 132,
				133, 136, 134
			});
			s_CounterTypeNames = new BBList<string>(Enum.GetNames(typeof(ECounterType)));
			Array values = Enum.GetValues(typeof(ECounterType));
			for (int i = 0; i < values.Length; i++)
			{
				AchievementCounterCount = Math.Max(AchievementCounterCount, (int)values.GetValue(i) + 1);
			}
		}

		public static ECounterType? ToCounterType(string _counter)
		{
			if (s_CounterTypeNames.Contains(_counter))
			{
				return (ECounterType)(int)Enum.Parse(typeof(ECounterType), _counter);
			}
			return null;
		}

		public void InitEvents()
		{
			if (!m_eventsCreated)
			{
				m_eventsCreated = true;
				StateManager instance = Singleton<StateManager>.Instance;
				UbiCore.Utils.Singleton<UiMain>.Instance.UIReadyToUse += OnReadyToProcess;
				instance.ProfileManager.OnProfileUserChanged += new SimpleDelegate.CallbackFunction(OnProfileUserChanged);
				EventDelegate.Add(instance.ProfileManager.onCountersUpdate, OnCountersUpdated);
				EventDelegate.Add(instance.ProfileManager.onAchievementUpdate, OnAchievementUpdate);
				for (int i = 0; i < AchievementCounterCount; i++)
				{
					m_Counters.Add(0uL);
				}
			}
		}

		public void InitStatics()
		{
			StateManager instance = Singleton<StateManager>.Instance;
			if (m_GcAchievementToAcierAchievement.Count != 0)
			{
				return;
			}
			AchievementMapping.Definition[] achievements = instance.TemplateManager.AchievementMapping.achievements;
			foreach (AchievementMapping.Definition definition in achievements)
			{
				GameID _gameId;
				if (!Main.Instance.GameDB.TryFindGameIdFromGuid(definition.id, out _gameId))
				{
					continue;
				}
				AchievementDefinition item = Main.Instance.GameDB.GetItem<AchievementDefinition>(_gameId);
				string text = ((!NativeTools.IsShipping()) ? item.GooglePlayIdDevelopment : item.GooglePlayIdShipping);
				if (!string.IsNullOrEmpty(text))
				{
					for (int j = 0; j < definition.ranks.Length; j++)
					{
						m_CounterToGcAchievement[(int)definition.ranks[j].conditions[0].counterType] = text;
					}
					m_GcAchievementToAcierAchievement[text] = definition;
				}
			}
			BBList<AchievementCategoryMapping.Category> bBList = new BBList<AchievementCategoryMapping.Category>();
			bBList.AddRange(instance.TemplateManager.AchievementCategoryMapping.categories);
			while (bBList.Count > 0)
			{
				AchievementCategoryMapping.Category category = bBList[0];
				m_IdToAcierAchievementCategory.Add(category.id, category);
				bBList.AddRange(category.subCategories);
				bBList.RemoveAt(0);
			}
			SetupAchievementCategoryData(m_achievementCategoryRootData, instance.TemplateManager.AchievementCategoryMapping.categories);
			for (int k = 0; k < instance.TemplateManager.AchievementMapping.achievements.Length; k++)
			{
				AchievementMapping.Definition definition2 = instance.TemplateManager.AchievementMapping.achievements[k];
				m_IdToAcierAchievement.Add(definition2.id, definition2);
				AchievementCategoryData achievementCategoryData = GetAchievementCategoryData(definition2.categoryId);
				if (achievementCategoryData != null)
				{
					achievementCategoryData.AddAchievementDef(definition2);
				}
			}
			m_achievementCategoryRootData.Sort();
			OnAchievementUpdate();
		}

		public void OnThirdPartyCountersAvailable()
		{
			if (!m_ThirdPartyCountersAvailable)
			{
				m_ThirdPartyCountersAvailable = true;
				UpdateThirdPartyCounters();
			}
		}

		public void OnMissionLoaded()
		{
			m_ReportedAchievements.Clear();
		}

		public AchievementCategoryData GetAchievementCategoryData(string _id)
		{
			AchievementCategoryData value = null;
			if (!string.IsNullOrEmpty(_id))
			{
				m_idToAchievementCategoryData.TryGetValue(_id, out value);
			}
			else
			{
				value = m_achievementCategoryRootData;
			}
			return value;
		}

		public AchievementCategoryMapping.Category GetAchievementCategoryMappingDefinition(string _id)
		{
			AchievementCategoryMapping.Category value;
			m_IdToAcierAchievementCategory.TryGetValue(_id, out value);
			return value;
		}

		public AchievementMapping.Definition GetAchievementMappingDefinition(string _id)
		{
			AchievementMapping.Definition value;
			m_IdToAcierAchievement.TryGetValue(_id, out value);
			return value;
		}

		public AchievementCategoryDefinition GetAchievementCategoryDefinition(string _id)
		{
			GameID _gameId;
			if (Main.Instance.GameDB.TryFindGameIdFromGuid(_id, out _gameId))
			{
				return Main.Instance.GameDB.GetItem<AchievementCategoryDefinition>(_gameId);
			}
			return null;
		}

		public AchievementCategoryDefinition GetAchievementCategoryDefinition(AchievementCategoryMapping.Category _mappingDefinition)
		{
			return GetAchievementCategoryDefinition(_mappingDefinition.id);
		}

		public AchievementDefinition GetAchievementDefinition(string _id)
		{
			GameID _gameId;
			if (Main.Instance.GameDB.TryFindGameIdFromGuid(_id, out _gameId))
			{
				return Main.Instance.GameDB.GetItem<AchievementDefinition>(_gameId);
			}
			return null;
		}

		public AchievementDefinition GetAchievementDefinition(AchievementMapping.Definition _mappingDefinition)
		{
			return GetAchievementDefinition(_mappingDefinition.id);
		}

		public ulong GetCounterValue(ECounterType _type)
		{
			if ((int)_type < m_Counters.Count)
			{
				return m_Counters[(int)_type];
			}
			return 0uL;
		}

		public void CheckIngameAchievements(ECounterType _counterType, IList<ulong> _counters, IList<ulong> _prevCounters)
		{
			IterateFulfilledAchievements(_counters, _prevCounters, OnIngameFulfilledAchievement);
			UpdateThirdPartcyCounterFromMission(_counterType, _counters[(int)_counterType]);
		}

		private void SetupAchievementCategoryData(AchievementCategoryData _data, AchievementCategoryMapping.Category[] _category)
		{
			foreach (AchievementCategoryMapping.Category category in _category)
			{
				AchievementCategoryDefinition achievementCategoryDefinition = GetAchievementCategoryDefinition(category.id);
				if (Helpers.Verify(achievementCategoryDefinition != null, "SetupAchievementCategoryData: Invalid AchievementCategoryDefinition - can't find {0} in local GameDB", category.id))
				{
					AchievementCategoryData achievementCategoryData = new AchievementCategoryData();
					achievementCategoryData.CategoryId = category.id;
					if (category.subCategories != null && category.subCategories.Length > 0)
					{
						SetupAchievementCategoryData(achievementCategoryData, category.subCategories);
					}
					m_idToAchievementCategoryData.Add(achievementCategoryData.CategoryId, achievementCategoryData);
					_data.SubCategories.Add(achievementCategoryData);
				}
			}
		}

		public double GetFulfilledPercentage(string _gameCenterId, long _counter)
		{
			AchievementMapping.Definition value;
			if (m_GcAchievementToAcierAchievement.TryGetValue(_gameCenterId, out value))
			{
				AchievementMapping.Condition condition = value.ranks[0].conditions[0];
				return Math.Min(100.0, 100.0 * (double)_counter / (double)condition.goal);
			}
			return 0.0;
		}

		public int GetCounterValue(string _gameCenterId, float _percentage)
		{
			AchievementMapping.Definition value;
			if (m_GcAchievementToAcierAchievement.TryGetValue(_gameCenterId, out value))
			{
				AchievementMapping.Condition condition = value.ranks[0].conditions[0];
				return (int)(_percentage * (float)condition.goal);
			}
			return 0;
		}

		public AchievementMapping.Definition GetAchievementMappingDefinitionByGcId(string _gameCenterId)
		{
			AchievementMapping.Definition value = null;
			m_GcAchievementToAcierAchievement.TryGetValue(_gameCenterId, out value);
			return value;
		}

		public AchievementDefinition GetAchievementDefinitionByGcId(string _gameCenterId)
		{
			return GetAchievementDefinition(GetAchievementMappingDefinitionByGcId(_gameCenterId));
		}

		public string GetGameCenterID(string _gameCenterId)
		{
			return GetAchievementDefinitionByGcId(_gameCenterId).GameCenterId;
		}

		private void OnReadyToProcess()
		{
			if (!m_ReadyToProcess)
			{
				m_ReadyToProcess = true;
				ProcessCounters();
			}
		}

		private void OnProfileUserChanged(object[] _params)
		{
			if (m_ReadyToProcess)
			{
				OnReadyToProcess();
			}
		}

		private void UpdateThirdPartcyCounterFromMission(ECounterType _counterType, ulong _currentCount)
		{
			if (Helpers.Verify(m_IngameCounterTypes.Contains((int)_counterType)))
			{
				UpdateThirdPartyCounter(_counterType, _currentCount);
			}
		}

		private void UpdateThirdPartyCounter(ECounterType _counterType, ulong _currentCount)
		{
			string value;
			if (ClientAchievementsManager.IsAvailable() && ClientAchievementsManager.IsSignedIn() && m_CounterToGcAchievement.TryGetValue((int)_counterType, out value) && !string.IsNullOrEmpty(value) && _currentCount != 0)
			{
				double fulfilledPercentage = GetFulfilledPercentage(value, (int)_currentCount);
				ClientAchievementsManager.UpdateAchievementProgressPoints(value, fulfilledPercentage);
			}
		}

		private void OnIngameFulfilledAchievement(AchievementMapping.Definition _definition, int _rank)
		{
			InvokeAchievementReached(_definition, _rank);
		}

		private void OnAchievementUpdate()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			if (profile == null)
			{
				return;
			}
			m_achievementCategoryRootData.ClearDynamicData();
			for (int i = 0; i < profile.achievements.entries.Length; i++)
			{
				Achievement achievement = profile.achievements.entries[i];
				AchievementMapping.Definition achievementMappingDefinition = GetAchievementMappingDefinition(achievement.definitionId);
				if (achievementMappingDefinition != null)
				{
					AchievementCategoryData achievementCategoryData = GetAchievementCategoryData(achievementMappingDefinition.categoryId);
					if (achievementCategoryData != null)
					{
						achievementCategoryData.AddAchievements(achievement);
					}
				}
			}
			m_achievementCategoryRootData.UpdateProgress();
			AchievementDataUpdatedHandler achievementDataUpdated = this.AchievementDataUpdated;
			if (achievementDataUpdated != null)
			{
				achievementDataUpdated();
			}
		}

		private void OnCountersUpdated()
		{
			for (int i = 0; i < m_Counters.Count; i++)
			{
				m_Counters[i] = 0uL;
			}
			Counter[] counters = Singleton<StateManager>.Instance.ProfileManager.Profile.counters.counters;
			for (int j = 0; j < counters.Length; j++)
			{
				int num = (int)counters[j].ECounterTypeOrdinal;
				if (num < m_Counters.Count)
				{
					if (counters[j].num < 0)
					{
						m_Counters[num] = 0uL;
					}
					else
					{
						m_Counters[num] = (ulong)counters[j].num;
					}
				}
			}
			UpdateThirdPartyCounters();
			ProcessCounters();
		}

		private void UpdateThirdPartyCounters()
		{
			if (m_ThirdPartyCountersAvailable)
			{
				for (int i = 0; i < m_Counters.Count; i++)
				{
					UpdateThirdPartyCounter((ECounterType)i, m_Counters[i]);
				}
			}
		}

		private void ProcessCounters()
		{
			if (!m_ReadyToProcess)
			{
				return;
			}
			m_AchievementsCache.Clear();
			IterateFulfilledAchievements(m_Counters, null, OnServerFulfilledAchievement);
			BBList<string> bBList = new BBList<string>(m_AchievementsCache.Count);
			foreach (string item in m_AchievementsCache)
			{
				bBList.Add(item);
			}
			if (bBList.Count > 0)
			{
				Singleton<StateManager>.Instance.ProfileManager.ReachNewAchievements(bBList);
			}
			CounterUpdatedHandler countersUpdated = this.CountersUpdated;
			if (countersUpdated != null)
			{
				countersUpdated();
			}
		}

		private void OnServerFulfilledAchievement(AchievementMapping.Definition _definition, int _rank)
		{
			m_AchievementsCache.Add(_definition.id);
			if (!m_IngameCounterTypes.Contains((int)_definition.ranks[_rank - 1].conditions[0].counterType))
			{
				InvokeAchievementReached(_definition, _rank);
			}
		}

		private void InvokeAchievementReached(AchievementMapping.Definition _definition, int _rank)
		{
			if (!m_ReportedAchievements.Contains(_definition.id + "_r" + _rank))
			{
				m_ReportedAchievements.Add(_definition.id + "_r" + _rank);
				AchievementReachedHandler achievementReached = this.AchievementReached;
				if (achievementReached != null)
				{
					achievementReached(_definition, _rank);
				}
			}
		}

		private void IterateFulfilledAchievements(IList<ulong> _counters, IList<ulong> _prevCounters, Action<AchievementMapping.Definition, int> _action)
		{
			AchievementMapping.Definition[] achievements = Singleton<StateManager>.Instance.TemplateManager.AchievementMapping.achievements;
			foreach (AchievementMapping.Definition definition in achievements)
			{
				for (int j = 0; j < definition.ranks.Length; j++)
				{
					for (int k = 0; k < 1; k++)
					{
						AchievementMapping.RankDefinition rankDefinition = definition.ranks[j];
						AchievementMapping.Condition condition = rankDefinition.conditions[k];
						int counterType = (int)condition.counterType;
						if (counterType >= _counters.Count || (_prevCounters != null && counterType >= _prevCounters.Count) || _counters[counterType] < (ulong)condition.goal || (_prevCounters != null && _prevCounters[counterType] >= (ulong)condition.goal))
						{
							continue;
						}
						bool flag = false;
						if (Singleton<StateManager>.Instance.ProfileManager.Profile != null)
						{
							Achievement[] entries = Singleton<StateManager>.Instance.ProfileManager.Profile.achievements.entries;
							for (int l = 0; l < entries.Length; l++)
							{
								if (!(entries[l].definitionId == definition.id))
								{
									continue;
								}
								for (int m = 0; m < entries[l].ranks.Length; m++)
								{
									AchievementRank achievementRank = entries[l].ranks[m];
									if (achievementRank.idx == rankDefinition.idx)
									{
										if (achievementRank.state != 0)
										{
											flag = true;
										}
										break;
									}
								}
								break;
							}
						}
						if (!flag && GetAchievementDefinition(definition) != null)
						{
							_action(definition, rankDefinition.idx);
						}
					}
				}
			}
		}
	}
}
