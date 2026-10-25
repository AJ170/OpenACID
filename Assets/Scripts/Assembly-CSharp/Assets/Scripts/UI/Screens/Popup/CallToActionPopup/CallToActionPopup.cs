using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Tracking;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.CallToActionPopup
{
	public class CallToActionPopup
	{
		private static List<KeyValuePair<GameID, DBEntry>> SortedDefinitions;

		public static bool LastMissionWasTutorial { get; private set; }

		public static bool LastMissionHadInventoryOverflow { get; private set; }

		public static bool LastMissionWasStoryFirstTry { get; private set; }

		static CallToActionPopup()
		{
			Reset();
		}

		public static void Show()
		{
			CTADefinitionBase cTADefinitionBase = null;
			if (!LastMissionWasTutorial && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive)
			{
				cTADefinitionBase = CheckTriggers(false);
			}
			if (cTADefinitionBase != null)
			{
				Popup(cTADefinitionBase);
			}
			else
			{
				Proceed();
			}
		}

		public static void ShowFromProfileUpdate()
		{
			CTADefinitionBase cTADefinitionBase = null;
			if (!LastMissionWasTutorial && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialActive)
			{
				cTADefinitionBase = CheckTriggers(true);
			}
			if (cTADefinitionBase != null)
			{
				Popup(cTADefinitionBase);
			}
			else
			{
				Proceed();
			}
		}

		public static void Show(CTADefinitionBase _definition)
		{
			Popup(_definition);
		}

		public static void ShowMessageLines()
		{
			if (LastMissionWasStoryFirstTry)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(8404), true);
			}
			LastMissionWasStoryFirstTry = false;
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.PopupQueue.Proceed();
		}

		private static CTADefinitionBase CheckTriggers(bool _fromProfile)
		{
			GetSortedDefinitions();
			for (int i = 0; i < SortedDefinitions.Count; i++)
			{
				CTADefinitionBase cTADefinitionBase = SortedDefinitions[i].Value as CTADefinitionBase;
				if (cTADefinitionBase.Enabled && (!_fromProfile || cTADefinitionBase.CheckOnProfileUpdate))
				{
					switch (cTADefinitionBase.CheckTriggerBase())
					{
					case CTADefinitionBase.EResult.Fullfilled:
						return cTADefinitionBase;
					default:
						return null;
					case CTADefinitionBase.EResult.Proceed:
						break;
					}
				}
			}
			return null;
		}

		public static List<KeyValuePair<GameID, DBEntry>> GetSortedDefinitions()
		{
			if (SortedDefinitions == null)
			{
				SortedDefinitions = Main.SafeGameDb.GetItems<CTADefinitionBase>();
				SortedDefinitions.Sort(CompareDefinition);
			}
			return SortedDefinitions;
		}

		private static int CompareDefinition(KeyValuePair<GameID, DBEntry> _a, KeyValuePair<GameID, DBEntry> _b)
		{
			CTADefinitionBase cTADefinitionBase = _a.Value as CTADefinitionBase;
			CTADefinitionBase cTADefinitionBase2 = _b.Value as CTADefinitionBase;
			return cTADefinitionBase.Priority.CompareTo(cTADefinitionBase2.Priority);
		}

		private static void Popup(CTADefinitionBase _definition)
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			instance.CTACounterSet(LocalStorage.CTACounterMissionLastCTA, instance.CTACounterGet(LocalStorage.CTACounterMission));
			if (!string.IsNullOrEmpty(_definition.Image.GetResourcePath()))
			{
				_definition.Image.GetObject<Texture2D>();
			}
			_definition.ShowPopup();
		}

		public static void Proceed(CTADefinitionBase _definition, bool _accept, bool _closed)
		{
			TrackingManager.AddCallToActionTrackPoint(_definition.Name, _accept, _closed);
			Proceed();
		}

		private static void Proceed()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.PopupQueue.Proceed();
			Reset();
		}

		public static void RecordMissionData()
		{
			LastMissionWasTutorial = Main.Instance.SceneLoader.CurrentMissionDescription.ObjectiveType == EObjectiveType.Tutorial_EObjectiveType;
			if (LastMissionWasTutorial)
			{
				return;
			}
			string baseName = Main.Instance.SceneLoader.CurrentMissionDescription.BaseName;
			LastMissionWasStoryFirstTry = Main.Instance.SceneLoader.CurrentMissionDescription.ObjectiveType == EObjectiveType.Story_EObjectiveType && !MissionsExtensions.HasStoryMissionBeenWonBefore(baseName) && UbiCore.Utils.Singleton<UiMain>.Instance.HasSucceededMissionOnce;
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			Assets.Scripts.Mission.Logic.Components.MissionResult missionResult = LogicMain.MissionResult;
			int num = instance.CTACounterAdd(LocalStorage.CTACounterMission, 1);
			if (num == int.MaxValue)
			{
				instance.CTACounterSet(LocalStorage.CTACounterMission, 0);
			}
			int playerDeathsCount = missionResult.PlayerDeathsCount;
			bool usedDoctor = missionResult.GetUsedHotspotCount(EHotspot.EHotspot_Doctor) > 0;
			bool flag = missionResult.UsedSkillsCount > 0;
			CTADefinitionDamageModule cTADefinitionDamageModule = FindDefinition<CTADefinitionDamageModule>();
			bool lowDamage = missionResult.RegularDamageDealtCount >= cTADefinitionDamageModule.DamageCount && missionResult.RegularDamageDealtSum / (float)missionResult.RegularDamageDealtCount <= cTADefinitionDamageModule.DamageThreshold;
			CTADefinitionDefenseModule cTADefinitionDefenseModule = FindDefinition<CTADefinitionDefenseModule>();
			bool lowHealth = missionResult.LowestBaseHealth / missionResult.LowestBaseHealthMax < cTADefinitionDefenseModule.HealthThreshold * 0.01f || playerDeathsCount > 0;
			instance.CTAMissionDataAdd(playerDeathsCount, usedDoctor, flag, lowDamage, lowHealth);
			if (flag)
			{
				instance.CTACounterAdd(LocalStorage.CTACounterUsedSkillsOutOfTutorial, 1);
			}
			ProfileManager profileManager = Singleton<StateManager>.Instance.ProfileManager;
			int level = profileManager.CurrentAssassin.level;
			int maxRankIdx = profileManager.Profile.maxRankIdx;
			HeroicRankMapping.Entry[] heroicRankMapping = Singleton<StateManager>.Instance.TemplateManager.HeroicRankMapping;
			if (maxRankIdx < heroicRankMapping.Length - 1 && level > heroicRankMapping[maxRankIdx].randomMissionsScalingLevelUpper)
			{
				if (instance.CTACounterGet(LocalStorage.CTACounterExceedingRankCurrent, -1) < maxRankIdx)
				{
					instance.CTACounterSet(LocalStorage.CTACounterExceedingRankCurrent, maxRankIdx);
					instance.CTACounterSet(LocalStorage.CTACounterExceedingRankNextAvail, 1);
				}
				else
				{
					instance.CTACounterAdd(LocalStorage.CTACounterExceedingRankNextAvail, 1);
				}
			}
		}

		public static void RecordMissionReward(MissionReward _reward)
		{
			LastMissionHadInventoryOverflow = _reward.fullInventory;
		}

		private static void Reset()
		{
			LastMissionWasTutorial = false;
			LastMissionHadInventoryOverflow = false;
			SortedDefinitions = null;
		}

		private static T FindDefinition<T>() where T : CTADefinitionBase
		{
			GetSortedDefinitions();
			T val = (T)null;
			for (int i = 0; i < SortedDefinitions.Count; i++)
			{
				T val2 = SortedDefinitions[i].Value as T;
				if (val2 != null)
				{
					if (!Assets.Scripts.Engine.Helpers.Verify(val == null, "Multiple CTAs of a type used with CallToActionPopup.FindDefinition."))
					{
						break;
					}
					val = val2;
				}
			}
			return val;
		}
	}
}
