using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Native;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Ingame.Implementation;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Generic;
using Assets.Scripts.UI.Screens.ModuleScreen;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.ErrorPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UbiPlayground;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug.UI;
using UbiCore.Utils;
using UnityEngine;

public class DebugMenuPlayerProfile : DebugMenuItem
{
	private class EntitlementsData
	{
		public string OriginalName;

		public int Guid;

		public string GuidIds;
	}

	private GUIStyle m_styleLabelBold;

	private List<int> m_increaseList = new List<int> { 1, 5, 10, 100, 1000, 10000, 100000, 1000000, 10000000 };

	private List<bool> m_increaseButtonBools = new List<bool>();

	private int m_selectedResourceIndex = 1;

	private bool m_showResourceSelectionBox;

	private int m_resource;

	private Vector2 m_resourceScrollPosition = Vector2.zero;

	private string m_currentAssassinId = "Invalid";

	private int m_level;

	private int m_exp;

	private int m_skillpoints;

	private int m_energy;

	private int m_itemLevelMin = 1;

	private int m_itemLevelMax = 3;

	private string m_quality = Quality.EQuality.Common.ToString();

	private string m_itemType = string.Empty;

	private string m_lootTemplateNameToCheatAdd = string.Empty;

	private string m_rollingCode = string.Empty;

	private string m_copyProfileId = string.Empty;

	private int m_eventScore;

	private readonly List<int> m_eventCounter = new List<int>();

	private int m_selectedCounterIndex;

	private bool m_showCounterSelectionBox;

	private int m_counter;

	private Vector2 m_counterScrollPosition = Vector2.zero;

	private string[] m_counterNames;

	private int m_moduleCount = 1;

	private int m_moduleQuality = 1;

	private int m_moduleRemainingTime = 30;

	private bool m_showModuleTypeSelectionBox;

	private int m_selectedModuleTypeIndex;

	private Vector2 m_moduleTypeScrollPosition = Vector2.zero;

	private string[] m_moduleTypeNames;

	private string m_achievementId = string.Empty;

	private int m_achievementRank = 1;

	private int m_selectedAchievementIndex;

	private Vector2 m_achievementScrollPosition = Vector2.zero;

	private string[] m_achievementNames;

	private EMysteryBoxType m_mysteryBoxType = EMysteryBoxType.MysteryBoxItemType;

	private string m_activeTimeMystery = string.Empty;

	private int m_selectedCustomLootIndex;

	private bool m_showCustomLootSelectionBox;

	private Vector2 m_customLootScrollPosition = Vector2.zero;

	private string[] m_customLootNames;

	private Queue<string> m_tempEntitlemetnIds = new Queue<string>();

	private string[] m_entitlementNames;

	private EntitlementsData[] m_entitlementsData;

	private Vector2 m_entitlementScrollPosition = Vector2.zero;

	private int m_selectedEntitlementIndex;

	private string m_dailyRewardParam1 = string.Empty;

	private bool m_dailyRewardCreateNewChain;

	private string m_missionQuestParam1 = string.Empty;

	private static bool m_doPolupateEliteMissions;

	private string m_pushNotificationTitle = string.Empty;

	private string m_pushNotificationMessage = string.Empty;

	private string m_pushNotificationKey = string.Empty;

	private long m_pushNotificationDuration = 30L;

	private EPushNotificationGroup m_selectedPushNotificationGroup;

	private string[] m_categories = new string[0];

	private BBDictionary<string, Action[]> m_cheatMenuDrawMethods = new BBDictionary<string, Action[]>();

	private int m_selectedCategoryIndex;

	private bool m_overrideSocialNetworkAvailability;

	private string[] CounterNames
	{
		get
		{
			if (m_counterNames == null)
			{
				m_counterNames = Enum.GetNames(typeof(ECounterType));
				Array.Sort(m_counterNames, (string _nameA, string _nameB) => string.Compare(_nameA, _nameB, StringComparison.Ordinal));
			}
			return m_counterNames;
		}
	}

	private string[] ModuleTypeNames
	{
		get
		{
			if (m_moduleTypeNames == null)
			{
				string[] names = Enum.GetNames(typeof(EModuleType));
				string[] array = new string[names.Length + 1];
				array[0] = "<random>";
				for (int i = 1; i < array.Length; i++)
				{
					array[i] = names[i - 1];
				}
				m_moduleTypeNames = array;
			}
			return m_moduleTypeNames;
		}
	}

	private string[] AchievementNames
	{
		get
		{
			if (m_achievementNames == null)
			{
				AchievementMapping.Definition[] achievements = Singleton<StateManager>.Instance.TemplateManager.AchievementMapping.achievements;
				m_achievementNames = new string[Math.Max(1, achievements.Length)];
				if (achievements.Length == 0)
				{
					m_achievementNames[0] = "<none>";
				}
				else
				{
					for (int i = 0; i < achievements.Length; i++)
					{
						AchievementDefinition achievementDefinition = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementDefinition(achievements[i]);
						if (achievementDefinition != null)
						{
							m_achievementNames[i] = achievementDefinition.Name;
						}
					}
				}
			}
			return m_achievementNames;
		}
	}

	private string[] CustomLootNames
	{
		get
		{
			if (m_customLootNames == null)
			{
				m_customLootNames = Enum.GetNames(typeof(ECustomLootTemplateType));
				Array.Sort(m_customLootNames, (string _nameA, string _nameB) => string.Compare(_nameA, _nameB, StringComparison.Ordinal));
			}
			return m_customLootNames;
		}
	}

	private EntitlementsData[] EntitlementInfos
	{
		get
		{
			if (m_entitlementsData == null)
			{
				BBDictionary<GameID, DBEntry> data = Main.Instance.GameDB.GetSubDB(typeof(EntitlementDefinition)).Data;
				m_entitlementsData = new EntitlementsData[data.Count];
				int num = 0;
				foreach (KeyValuePair<GameID, DBEntry> item in data)
				{
					m_entitlementsData[num] = new EntitlementsData
					{
						Guid = item.Value.GameID.Guid,
						GuidIds = item.Value.GameID.GuidString,
						OriginalName = item.Value.Name
					};
					num++;
				}
				Array.Sort(m_entitlementsData, (EntitlementsData _nameA, EntitlementsData _nameB) => string.Compare(_nameA.OriginalName, _nameB.OriginalName, StringComparison.Ordinal));
			}
			return m_entitlementsData;
		}
	}

	private string[] EntitlementNames
	{
		get
		{
			if (m_entitlementNames == null)
			{
				m_entitlementNames = new string[EntitlementInfos.Length];
				for (int i = 0; i < EntitlementInfos.Length; i++)
				{
					bool flag = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(EntitlementInfos[i].Guid);
					m_entitlementNames[i] = EntitlementInfos[i].OriginalName + " ID: " + EntitlementInfos[i].GuidIds + ((!flag) ? string.Empty : " -> Owned");
				}
			}
			return m_entitlementNames;
		}
	}

	private bool IsDataStoreInitialized
	{
		get
		{
			if (Main.Instance != null && Main.Instance.IsNetworkEnabled)
			{
				return Singleton<StateManager>.Instance.ProfileManager != null;
			}
			return false;
		}
	}

	private void Awake()
	{
		FacebookManager.OverrideIsFacebookAvailable = m_overrideSocialNetworkAvailability;
		WeiboManager.OverrideIsWeiboAvailable = m_overrideSocialNetworkAvailability;
		UbiPlaygroundManager.OverrideIsUplayAvailable = m_overrideSocialNetworkAvailability;
		m_increaseButtonBools.Add(true);
		for (int i = 1; i < m_increaseList.Count; i++)
		{
			m_increaseButtonBools.Add(false);
		}
		SetupCheatMenu();
	}

	private void SetupCheatMenu()
	{
		AddCheatMenuCategory("Profile", new Action[2]
		{
			delegate
			{
				DrawPlayerProfile();
			},
			delegate
			{
				DrawProfile();
			}
		});
		AddCheatMenuCategory("Entitlements", new Action[1]
		{
			delegate
			{
				DrawEntitlements();
			}
		});
		AddCheatMenuCategory("Items & Reward", new Action[2]
		{
			delegate
			{
				DrawItems();
			},
			delegate
			{
				DrawModules();
			}
		});
		AddCheatMenuCategory("Mission & Quests", new Action[2]
		{
			delegate
			{
				DrawMissions();
			},
			delegate
			{
				DrawMissionQuest();
			}
		});
		AddCheatMenuCategory("Tutorial", new Action[1]
		{
			delegate
			{
				DrawTutorial();
			}
		});
		AddCheatMenuCategory("Achievements", new Action[1]
		{
			delegate
			{
				DrawAchievements();
			}
		});
		AddCheatMenuCategory("Push", new Action[1]
		{
			delegate
			{
				DrawPushNotifications();
			}
		});
		AddCheatMenuCategory("Native Tools", new Action[1]
		{
			delegate
			{
				DrawNativeTools();
			}
		});
		AddCheatMenuCategory("Advertisement", new Action[1]
		{
			delegate
			{
				DrawAdColony();
			}
		});
		AddCheatMenuCategory("Event", new Action[1]
		{
			delegate
			{
				DrawEventProgressionMenu();
			}
		});
		AddCheatMenuCategory("SocialNetwork", new Action[1]
		{
			delegate
			{
				DrawSocialNetworkMenu();
			}
		});
	}

	private void AddCheatMenuCategory(string _name, Action[] _actions)
	{
		m_categories = m_categories.Add(_name);
		m_cheatMenuDrawMethods[_name] = _actions;
	}

	private int GetIncreaseCount()
	{
		int num = 0;
		for (int i = 0; i < m_increaseList.Count; i++)
		{
			if (m_increaseButtonBools[i])
			{
				num += m_increaseList[i];
			}
		}
		return num;
	}

	public override string GetName()
	{
		return "Cheat";
	}

	public override void OnApply()
	{
		if (!IsDataStoreInitialized)
		{
			return;
		}
		if (m_level != 0)
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.LEVEL_UP, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_level
			});
		}
		if (m_exp != 0)
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.ADD_XP, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_exp
			});
		}
		if (m_skillpoints != 0)
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.ADD_SKILLPOINTS, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_skillpoints
			});
		}
		if (m_eventScore != 0)
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.SET_EVENT_SCORE, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_eventScore
			});
		}
		if (m_eventCounter.Count > 0)
		{
			EventsProgress eventsProgress = Singleton<StateManager>.Instance.ProfileManager.Profile.eventsProgress;
			if (eventsProgress != null)
			{
				for (int i = 0; i < m_eventCounter.Count; i++)
				{
					int num = m_eventCounter[i];
					if (num > 0)
					{
						ECounterType challengeCounter = eventsProgress.challengeEvents[i].challengeCounter;
						Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.INC_COUNTER, m_currentAssassinId, new CheatParameters
						{
							Param1 = num,
							Param2 = (int)challengeCounter
						});
					}
				}
			}
		}
		if (m_energy != 0)
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.DEPLETE_ENERGY, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_energy
			});
		}
		if (m_resource != 0)
		{
			Array values = Enum.GetValues(typeof(EResourceType));
			if (m_selectedResourceIndex < values.Length)
			{
				Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.ADD_RESOURCES, m_currentAssassinId, new CheatParameters
				{
					Param1 = m_resource,
					Param2 = (int)values.GetValue(m_selectedResourceIndex)
				});
			}
		}
		if (m_counter != 0 && m_selectedCounterIndex < CounterNames.Length)
		{
			ECounterType param = (ECounterType)(int)Enum.Parse(typeof(ECounterType), CounterNames[m_selectedCounterIndex]);
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.INC_COUNTER, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_counter,
				Param2 = (int)param
			});
		}
		Reset();
	}

	public override void OnOpen()
	{
		Reset();
	}

	public override void OnMenu()
	{
		if (m_styleLabelBold == null)
		{
			m_styleLabelBold = new GUIStyle(GUI.skin.GetStyle("Label"));
			m_styleLabelBold.fontStyle = FontStyle.Bold;
		}
		GUILayout.BeginHorizontal();
		m_selectedCategoryIndex = GUILayout.SelectionGrid(m_selectedCategoryIndex, m_categories, 1, GUILayout.Width(150f));
		GUILayout.BeginVertical();
		DrawActiveCategory();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	private void DrawFacebookMenu()
	{
		GUILayout.BeginVertical();
		FacebookManager.FacebookHandler.DrawDebugMenu();
		GUILayout.EndVertical();
	}

	private void DrawHeaderIncreaseButtons()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Settings");
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal("Box");
		for (int i = 0; i < m_increaseList.Count; i++)
		{
			m_increaseButtonBools[i] = GUILayout.Toggle(m_increaseButtonBools[i], m_increaseList[i].ToString());
		}
		GUILayout.FlexibleSpace();
		GUILayout.Label("Increase/Decrease sum: " + GetIncreaseCount());
		GUILayout.EndHorizontal();
		GUILayout.EndHorizontal();
	}

	private void DrawIncDecField(string _label, ref int _value)
	{
		GUIStyle style = ((_value == 0) ? GUI.skin.GetStyle("Label") : m_styleLabelBold);
		GUILayout.BeginHorizontal();
		if (!string.IsNullOrEmpty(_label))
		{
			GUILayout.Label(_label, style, GUILayout.Width(300f));
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("-" + GetIncreaseCount(), GUILayout.Width(60f)))
		{
			_value -= GetIncreaseCount();
		}
		GUILayout.Label(_value.ToString(), style, GUILayout.Width(100f));
		if (GUILayout.Button("+" + GetIncreaseCount(), GUILayout.Width(60f)))
		{
			_value += GetIncreaseCount();
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("0", GUILayout.Width(80f)))
		{
			_value = 0;
		}
		GUILayout.EndHorizontal();
	}

	private void DrawSelectAssassinDropDown()
	{
		Assassin[] assassins = Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins;
		if (assassins.Length <= 0)
		{
			GUILayout.Label("You have no assassins. Go create some =)");
			return;
		}
		string[] array = new string[assassins.Length];
		for (int i = 0; i < assassins.Length; i++)
		{
			Assassin assassin = assassins[i];
			string text = assassin.GetAssassinName() + " (Level:" + assassin.level + ") (Id:" + assassin.id + ")";
			array[i] = text.Substring(0, Mathf.Min(text.Length, 120));
		}
		int num = assassins.FindIndex((Assassin _o) => _o.id == m_currentAssassinId);
		if (num == -1)
		{
			m_currentAssassinId = assassins[0].id;
		}
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Select assassin");
		num = GUILayout.SelectionGrid(num, array, 1, "toggle");
		GUILayout.EndVertical();
		if (GUI.changed && num != -1)
		{
			m_currentAssassinId = assassins[num].id;
		}
	}

	private void DrawResourceSelector()
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(300f));
		m_resourceScrollPosition = GUILayout.BeginScrollView(m_resourceScrollPosition, GUILayout.MinHeight(200f));
		string[] names = Enum.GetNames(typeof(EResourceType));
		int selectedResourceIndex = m_selectedResourceIndex;
		m_selectedResourceIndex = GUILayout.SelectionGrid(m_selectedResourceIndex, names, 1, "toggle");
		if (selectedResourceIndex != m_selectedResourceIndex)
		{
			m_showResourceSelectionBox = false;
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawAchievementSelector()
	{
		GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true));
		m_achievementScrollPosition = GUILayout.BeginScrollView(m_achievementScrollPosition, GUILayout.MinHeight(200f));
		string[] achievementNames = AchievementNames;
		int selectedAchievementIndex = m_selectedAchievementIndex;
		m_selectedAchievementIndex = GUILayout.SelectionGrid(m_selectedAchievementIndex, achievementNames, 1, "toggle");
		if (selectedAchievementIndex != m_selectedAchievementIndex)
		{
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawCounterSelector()
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(300f));
		m_counterScrollPosition = GUILayout.BeginScrollView(m_counterScrollPosition, GUILayout.MinHeight(200f));
		string[] array = new string[CounterNames.Length];
		for (int i = 0; i < CounterNames.Length; i++)
		{
			array[i] = CounterNames[i] + "(" + Singleton<StateManager>.Instance.ProfileManager.Profile.GetCounterAmountByType((ECounterType)(int)Enum.Parse(typeof(ECounterType), CounterNames[i])) + ")";
		}
		int selectedCounterIndex = m_selectedCounterIndex;
		m_selectedCounterIndex = GUILayout.SelectionGrid(m_selectedCounterIndex, array, 1, "toggle");
		if (selectedCounterIndex != m_selectedCounterIndex)
		{
			m_showCounterSelectionBox = false;
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawCustomLootSelector()
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(300f));
		m_customLootScrollPosition = GUILayout.BeginScrollView(m_customLootScrollPosition, GUILayout.MinHeight(200f));
		int selectedCustomLootIndex = m_selectedCustomLootIndex;
		m_selectedCustomLootIndex = GUILayout.SelectionGrid(m_selectedCustomLootIndex, CustomLootNames, 1, "toggle");
		if (selectedCustomLootIndex != m_selectedCustomLootIndex)
		{
			m_showCustomLootSelectionBox = false;
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawModuleTypeSelector()
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(300f));
		m_moduleTypeScrollPosition = GUILayout.BeginScrollView(m_moduleTypeScrollPosition, GUILayout.MinHeight(200f));
		int selectedModuleTypeIndex = m_selectedModuleTypeIndex;
		m_selectedModuleTypeIndex = GUILayout.SelectionGrid(m_selectedModuleTypeIndex, ModuleTypeNames, 1, "toggle");
		if (selectedModuleTypeIndex != m_selectedModuleTypeIndex)
		{
			m_showModuleTypeSelectionBox = false;
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawActiveCategory()
	{
		string text = m_categories[m_selectedCategoryIndex];
		if (IsDataStoreInitialized)
		{
			GUILayout.BeginVertical(text, "window");
			Action[] array = m_cheatMenuDrawMethods[text];
			Action[] array2 = array;
			foreach (Action action in array2)
			{
				action();
			}
			GUILayout.EndVertical();
		}
	}

	private void DrawEventProgressionMenu()
	{
		GUILayout.BeginVertical("Box");
		DrawHeaderIncreaseButtons();
		EventsProgress eventsProgress = Singleton<StateManager>.Instance.ProfileManager.Profile.eventsProgress;
		if (eventsProgress != null)
		{
			for (int i = 0; i < eventsProgress.challengeEvents.Length; i++)
			{
				EventsProgress.ChallengeEvent challengeEvent = eventsProgress.challengeEvents[i];
				if (i >= m_eventCounter.Count)
				{
					m_eventCounter.Add(0);
				}
				string label = challengeEvent.challengeCounter.ToString() + "(" + Singleton<StateManager>.Instance.ProfileManager.Profile.GetCounterAmountByType(challengeEvent.challengeCounter) + ")";
				int _value = m_eventCounter[i];
				DrawIncDecField(label, ref _value);
				m_eventCounter[i] = _value;
			}
			GUILayout.Space(10f);
			DrawIncDecField("Event Score", ref m_eventScore);
			if (GUILayout.Button("Reset Event progression"))
			{
				Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters(), _cheatType: CheatRequest.CheatType.RESET_PROGRESSION_EVENTS_PROGRESS, _assassinId: string.Empty);
			}
		}
		else
		{
			GUILayout.Label("No Event is running");
		}
		GUILayout.EndVertical();
	}

	private void DrawPlayerProfile()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Predefined Berserker"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param3 = "GFX Preset Berserker"
			}, _cheatType: CheatRequest.CheatType.ADD_PREDEFINED_ASSASSIN, _assassinId: string.Empty);
		}
		if (GUILayout.Button("Add Predefined Shadowblade"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param3 = "GFX Preset Shadowblade"
			}, _cheatType: CheatRequest.CheatType.ADD_PREDEFINED_ASSASSIN, _assassinId: string.Empty);
		}
		if (GUILayout.Button("Add Predefined Trickster"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param3 = "GFX Preset Trickster"
			}, _cheatType: CheatRequest.CheatType.ADD_PREDEFINED_ASSASSIN, _assassinId: string.Empty);
		}
		if (GUILayout.Button("Add Predefined Thief"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param3 = "GFX Preset Thief"
			}, _cheatType: CheatRequest.CheatType.ADD_PREDEFINED_ASSASSIN, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Clear Mission history"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param3 = "GFX Preset Berserker"
			}, _cheatType: CheatRequest.CheatType.CLEAR_MISSION_HISTORY, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		DrawHeaderIncreaseButtons();
		DrawSelectAssassinDropDown();
		DrawIncDecField("Level", ref m_level);
		DrawIncDecField("Experience", ref m_exp);
		DrawIncDecField("Skillpoints", ref m_skillpoints);
		DrawIncDecField("Energy", ref m_energy);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reset Second Class"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.RESET_SECOND_CLASS, m_currentAssassinId);
			RestartApp();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Assassins Selection Screen"))
		{
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.AssassinSelection, new UITweenHandle(null, null));
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add Friend"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = 1
			}, _cheatType: CheatRequest.CheatType.ADD_FRIEND, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		string[] names = Enum.GetNames(typeof(EResourceType));
		m_showResourceSelectionBox = GUILayout.Toggle(m_showResourceSelectionBox, ((!m_showResourceSelectionBox) ? ">" : "^") + " Resource(" + names[m_selectedResourceIndex] + ")", "button", GUILayout.Width(300f));
		DrawIncDecField(null, ref m_resource);
		GUILayout.EndHorizontal();
		if (m_showResourceSelectionBox)
		{
			DrawResourceSelector();
		}
		GUILayout.BeginHorizontal();
		long counterAmountByType = Singleton<StateManager>.Instance.ProfileManager.Profile.GetCounterAmountByType((ECounterType)(int)Enum.Parse(typeof(ECounterType), CounterNames[m_selectedCounterIndex]));
		m_showCounterSelectionBox = GUILayout.Toggle(m_showCounterSelectionBox, ((!m_showCounterSelectionBox) ? ">" : "^") + " Counter(" + CounterNames[m_selectedCounterIndex] + "[" + counterAmountByType + "])", "button", GUILayout.Width(300f));
		DrawIncDecField(null, ref m_counter);
		GUILayout.EndHorizontal();
		if (m_showCounterSelectionBox)
		{
			DrawCounterSelector();
		}
		GUILayout.EndVertical();
		GUILayout.Space(10f);
	}

	private void DrawSocialNetworkMenu()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.BeginVertical("Box");
		if (GUILayout.Button((!m_overrideSocialNetworkAvailability) ? "OVERRIDE social network availability" : "DO NOT override Social Network availability"))
		{
			m_overrideSocialNetworkAvailability = !m_overrideSocialNetworkAvailability;
			FacebookManager.OverrideIsFacebookAvailable = m_overrideSocialNetworkAvailability;
			WeiboManager.OverrideIsWeiboAvailable = m_overrideSocialNetworkAvailability;
			UbiPlaygroundManager.OverrideIsUplayAvailable = m_overrideSocialNetworkAvailability;
		}
		GUILayout.EndVertical();
		if (m_overrideSocialNetworkAvailability)
		{
			GUILayout.Space(30f);
			GUILayout.BeginVertical("Box");
			GUILayout.Label("Is Facebook enabled: " + FacebookManager.IsEnabledForTesting);
			if (GUILayout.Button((!FacebookManager.IsEnabledForTesting) ? "Enable FB" : "Disable FB"))
			{
				FacebookManager.IsEnabledForTesting = !FacebookManager.IsEnabledForTesting;
			}
			GUILayout.Space(20f);
			GUILayout.Label("Is Weibo enabled: " + WeiboManager.IsEnabledForTesting);
			if (GUILayout.Button((!WeiboManager.IsEnabledForTesting) ? "Enable Weibo" : "Disable Weibo"))
			{
				WeiboManager.IsEnabledForTesting = !WeiboManager.IsEnabledForTesting;
			}
			if (WeiboManager.IsEnabledForTesting)
			{
				GUILayout.Label("Is Weibo initialized: " + WeiboManager.IsInitialized);
				if (!WeiboManager.IsInitialized && GUILayout.Button("Initialize Weibo"))
				{
					WeiboManager.Init();
				}
			}
			GUILayout.Space(20f);
			GUILayout.Label("Is Uplay enabled: " + UbiPlaygroundManager.IsEnabledForTesting);
			if (GUILayout.Button((!UbiPlaygroundManager.IsEnabledForTesting) ? "Enable Uplay" : "Disable Uplay"))
			{
				UbiPlaygroundManager.IsEnabledForTesting = !UbiPlaygroundManager.IsEnabledForTesting;
			}
			GUILayout.Space(20f);
			GUILayout.Label("IsLinkedToProfileForTesting: " + UbiPlaygroundManager.IsLinkedToProfileForTesting);
			if (GUILayout.Button((!UbiPlaygroundManager.IsLinkedToProfileForTesting) ? "Fake link profile to Uplay" : "Fake unlink profile from Uplay"))
			{
				UbiPlaygroundManager.IsLinkedToProfileForTesting = !UbiPlaygroundManager.IsLinkedToProfileForTesting;
			}
			GUILayout.EndVertical();
		}
		GUILayout.EndVertical();
	}

	private void DrawMissions()
	{
		GUILayout.BeginHorizontal("Box");
		if (GUILayout.Button("Repopulate generated missions"))
		{
			object obj;
			if (m_doPolupateEliteMissions)
			{
				CheatParameters cheatParameters = new CheatParameters();
				cheatParameters.Param1 = 1;
				obj = cheatParameters;
			}
			else
			{
				obj = null;
			}
			CheatParameters @params = (CheatParameters)obj;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.REPOPULATE_MISSIONS, string.Empty, @params);
		}
		m_doPolupateEliteMissions = GUILayout.Toggle(m_doPolupateEliteMissions, "Populate with elite missions");
		if (GUILayout.Button("Reset elite timer"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.RESET_ELITE_MISSION_TIMER, string.Empty);
		}
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Repopulate hirelings list"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.REPOPULATE_HIRELINGS, string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.EndVertical();
		GUILayout.Space(10f);
	}

	private void DrawModules()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Count");
		string s = GUILayout.TextField(m_moduleCount.ToString());
		int.TryParse(s, out m_moduleCount);
		GUILayout.EndHorizontal();
		DrawIncDecField("Module quality (1 worst, 5 best, 0 random)", ref m_moduleQuality);
		m_moduleQuality = Mathf.Clamp(m_moduleQuality, 0, 5);
		GUILayout.BeginHorizontal();
		m_showModuleTypeSelectionBox = GUILayout.Toggle(m_showModuleTypeSelectionBox, ((!m_showModuleTypeSelectionBox) ? ">" : "^") + ModuleTypeNames[m_selectedModuleTypeIndex], "button", GUILayout.Width(300f));
		GUILayout.EndHorizontal();
		if (m_showModuleTypeSelectionBox)
		{
			DrawModuleTypeSelector();
		}
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add modules"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = m_moduleCount,
				Param2 = m_moduleQuality,
				Param3 = ((m_selectedModuleTypeIndex > 0) ? ModuleTypeNames[m_selectedModuleTypeIndex] : string.Empty)
			}, _cheatType: CheatRequest.CheatType.ADD_MODULES, _assassinId: string.Empty);
		}
		if (GUILayout.Button("Deactivate module (current Assassin)"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.DEACTIVATE_MODULE, m_currentAssassinId);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Remaining time (in s) or mission count");
		string s2 = GUILayout.TextField(m_moduleRemainingTime.ToString());
		int.TryParse(s2, out m_moduleRemainingTime);
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Set for active module of selected assassin"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.MODULE_SET_REMAINING_TIME, m_currentAssassinId, new CheatParameters
			{
				Param1 = m_moduleRemainingTime
			}, RescheduleModuleNotifications);
		}
		GUILayout.EndVertical();
		GUILayout.Space(10f);
	}

	private void DrawItems()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Remove all items"))
		{
			UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemRemoveAll();
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = 1
			}, _cheatType: CheatRequest.CheatType.CLEAR_ALL_ITEMS, _assassinId: string.Empty);
			EventDelegate.Execute(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange);
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Clear GP Entries in Local Storage"))
		{
			UbiCore.Utils.Singleton<LocalStorage>.Instance.GPRemoveAll();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Space(10f);
		if (GUILayout.Button("Unlock all Codexentries"))
		{
			UnlockAllCodex();
			if (Main.Instance.SceneLoader.InAnimus)
			{
				Singleton<UIScreenManager>.Instance.PurgeManager();
				Singleton<StateManager>.Instance.ProfileManager.EnableUpdates = true;
				Main.Instance.LoadMission(BuildDefinition.AnimusMission, null, false);
				m_debugMenu.CloseDebugMenu();
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Choose Quality for Item/Mystery Box");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Common"))
		{
			m_quality = Quality.EQuality.Common.ToString();
		}
		if (GUILayout.Button("Uncommon"))
		{
			m_quality = Quality.EQuality.Uncommon.ToString();
		}
		if (GUILayout.Button("Mundane"))
		{
			m_quality = Quality.EQuality.Mundane.ToString();
		}
		if (GUILayout.Button("Rare"))
		{
			m_quality = Quality.EQuality.Rare.ToString();
		}
		if (GUILayout.Button("Perfect"))
		{
			m_quality = Quality.EQuality.Perfect.ToString();
		}
		if (GUILayout.Button("Random"))
		{
			m_quality = string.Empty;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Choose Item Type");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Arms"))
		{
			m_itemType = Item.EItemCategory.Arms.ToString();
		}
		if (GUILayout.Button("Weapon"))
		{
			m_itemType = "weapon";
		}
		if (GUILayout.Button("Boots"))
		{
			m_itemType = Item.EItemCategory.Boots.ToString();
		}
		if (GUILayout.Button("Chest"))
		{
			m_itemType = Item.EItemCategory.Chest.ToString();
		}
		if (GUILayout.Button("Insignia"))
		{
			m_itemType = Item.EItemCategory.Insignia.ToString();
		}
		if (GUILayout.Button("Legs"))
		{
			m_itemType = Item.EItemCategory.Legs.ToString();
		}
		if (GUILayout.Button("Outfit"))
		{
			m_itemType = Item.EItemCategory.Outfit.ToString();
		}
		if (GUILayout.Button("Shoulder"))
		{
			m_itemType = Item.EItemCategory.Shoulder.ToString();
		}
		if (GUILayout.Button("Random"))
		{
			m_itemType = string.Empty;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add " + ((!string.IsNullOrEmpty(m_quality.ToString())) ? m_quality.ToString() : "Random") + ((!string.IsNullOrEmpty(m_itemType.ToString())) ? (" " + m_itemType) : " item")))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = m_itemLevelMin,
				Param2 = m_itemLevelMax,
				Param3 = m_quality,
				Param4 = m_itemType
			}, _cheatType: CheatRequest.CheatType.ADD_ITEM, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Choose Mystery Box Type");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Mystery Box Type : Item"))
		{
			m_mysteryBoxType = EMysteryBoxType.MysteryBoxItemType;
		}
		if (GUILayout.Button("Mystery Box Type: Booty"))
		{
			m_mysteryBoxType = EMysteryBoxType.MysteryBoxBootyChestType;
		}
		if (GUILayout.Button("Mystery Box Type: Movie"))
		{
			m_mysteryBoxType = EMysteryBoxType.MysteryBoxMovieType;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Add : " + m_quality + " MysteryBox with " + m_mysteryBoxType))
		{
			string param;
			switch (m_mysteryBoxType)
			{
			case EMysteryBoxType.MysteryBoxMovieType:
				param = "MovieChestCommon";
				break;
			case EMysteryBoxType.MysteryBoxBootyChestType:
				param = "BootyChestCommon";
				break;
			default:
				param = string.Concat("T01_Mystery Box_", m_mysteryBoxType, "_L01_", m_quality);
				break;
			}
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = 1,
				Param2 = 0,
				Param3 = param
			}, _cheatType: CheatRequest.CheatType.INSTANTIATE_LOOT_TEMPLATE, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Set active time of current mystery box chain (in sec)");
		GUILayout.EndVertical();
		GUILayout.BeginVertical("Box");
		m_activeTimeMystery = GUILayout.TextArea(m_activeTimeMystery);
		GUILayout.EndVertical();
		GUILayout.BeginVertical("Box");
		int result = 0;
		if (GUILayout.Button("Set") && int.TryParse(m_activeTimeMystery, out result))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = result
			}, _cheatType: CheatRequest.CheatType.MYSTERY_BOX_SET_TIME, _assassinId: string.Empty);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		string text = CustomLootNames[m_selectedCustomLootIndex];
		string text2 = ((!m_showCustomLootSelectionBox) ? ">" : "^");
		string text3 = text2 + " Loot Type(" + text;
		m_showCustomLootSelectionBox = GUILayout.Toggle(m_showCustomLootSelectionBox, text3, "button", GUILayout.Width(300f));
		if (m_showCustomLootSelectionBox)
		{
			DrawCustomLootSelector();
		}
		GUILayout.Space(5f);
		if (GUILayout.Button("Add " + text))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = m_itemLevelMin,
				Param2 = m_itemLevelMax,
				Param3 = m_quality,
				Param5 = text
			}, _cheatType: CheatRequest.CheatType.ADD_ITEM, _assassinId: string.Empty);
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		string text4 = ((!string.IsNullOrEmpty(m_quality.ToString())) ? m_quality.ToString() : "Random");
		GUILayout.Label("Outcome: " + text4);
		GUILayout.EndHorizontal();
		DrawIncDecField("Item Level Min", ref m_itemLevelMin);
		DrawIncDecField("Item Level Max", ref m_itemLevelMax);
		m_itemLevelMin = Mathf.Clamp(m_itemLevelMin, 1, 30);
		m_itemLevelMax = Mathf.Clamp(m_itemLevelMax, 3, 30);
		if (m_itemLevelMin > m_itemLevelMax)
		{
			m_itemLevelMax = m_itemLevelMin;
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label("Enter LootTemplate name to add");
		m_lootTemplateNameToCheatAdd = GUILayout.TextField(m_lootTemplateNameToCheatAdd);
		if (GUILayout.Button("Add loot (from template)"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters
			{
				Param1 = 1,
				Param2 = 0,
				Param3 = m_lootTemplateNameToCheatAdd
			}, _cheatType: CheatRequest.CheatType.INSTANTIATE_LOOT_TEMPLATE, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		ItemInfoHandler.s_ShowRollingCode = GUILayout.Toggle(ItemInfoHandler.s_ShowRollingCode, "Show rolling code", GUILayout.Width(200f));
		if (GUILayout.Button("Roll by code:", GUILayout.Width(150f)))
		{
			Singleton<StateManager>.Instance.ProfileManager.SimulateLootRoll(m_rollingCode, false);
		}
		m_rollingCode = GUILayout.TextField(m_rollingCode, GUILayout.Width(300f));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
	}

	private void DrawProfile()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Leaderboard");
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Refresh Leaderboard"))
		{
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_params: new CheatParameters(), _cheatType: CheatRequest.CheatType.REPOPULATE_LEADER_BOARDS, _assassinId: string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Profile");
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reset player profile (no confirmation!)"))
		{
			UbiCore.Utils.Singleton<LocalStorage>.Instance.Clear();
			if (Singleton<StateManager>.Instance.ProfileManager.AbandonUserProfile())
			{
				MailCache.Clear();
				RestartApp();
			}
		}
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("ProfileId:", GUILayout.ExpandWidth(false));
		m_copyProfileId = GUILayout.TextField(m_copyProfileId);
		long result = 0L;
		string text = "USER::";
		if (long.TryParse(m_copyProfileId, out result) && result > 0)
		{
			int num = 19 - m_copyProfileId.Length;
			for (int i = 0; i < num; i++)
			{
				text += '0';
			}
			text += m_copyProfileId;
			string id = Singleton<StateManager>.Instance.ProfileManager.Profile.id;
			int num2 = id.LastIndexOf("::");
			text = ((num2 < 0) ? string.Empty : (text + id.Substring(num2)));
		}
		else
		{
			text = string.Empty;
		}
		bool flag = GUI.enabled;
		GUI.enabled = text.Length != 0;
		if (GUILayout.Button("Copy Profile"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param3 = text;
			CheatParameters params2 = cheatParameters;
			AsyncCallback callback = delegate(IAsyncResult _response)
			{
				AsyncErrorResult asyncErrorResult = _response as AsyncErrorResult;
				if (asyncErrorResult == null || asyncErrorResult.Exception == null)
				{
					RestartApp();
				}
			};
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.COPY_USER_PROFILE, string.Empty, params2, callback);
		}
		GUI.enabled = flag;
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
	}

	private void UnlockAllCodex()
	{
		List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<CodexDefinition>();
		for (int i = 0; i < items.Count; i++)
		{
			CodexDefinition codexDefinition = (CodexDefinition)items[i].Value;
			if (codexDefinition.Status == DBEntry.EStatus.Production)
			{
				UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexUnlockEntry(codexDefinition.GameID);
			}
		}
	}

	private static void DrawTutorial()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Tutorial");
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		UITutorialManager tutorialManager = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager;
		if (GUILayout.Button("Reinit", GUILayout.MaxWidth(100f)))
		{
			tutorialManager.InitFromProfile();
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.CLEAR_TUTORIAL_CATEGORIES, string.Empty);
		}
		TutorialCategory tutorialCategory = null;
		for (int i = tutorialManager.CurrentCategory.Index; i < tutorialManager.Categories.Length; i++)
		{
			tutorialCategory = tutorialManager.Categories[i];
			if (!tutorialCategory.Finished)
			{
				break;
			}
		}
		if (GUILayout.Button("Finish All", GUILayout.MaxWidth(100f)))
		{
			tutorialManager.FinishTutorial();
		}
		if (tutorialCategory != null && !tutorialCategory.Finished)
		{
			ETutorialCategory category = tutorialCategory.Category;
			if (GUILayout.Button(string.Format("Finish {0}", category)))
			{
				tutorialManager.FinishCategory(category, null);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void RescheduleModuleNotifications(IAsyncResult _result)
	{
		IAsyncDataResult<CheatResponse> asyncDataResult = _result as IAsyncDataResult<CheatResponse>;
		CheatResponse cheatResponse = asyncDataResult.Result();
		InventoryElement elementForSlot = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.GetElementForSlot(EInventorySlot.SlotModule);
		if (elementForSlot != null && elementForSlot.IsModule())
		{
			long num = elementForSlot.moduleData.ComputeRemainingMilliSeconds() / 1000;
			PushNotificationsProvider.ScheduleNotificationsForNewlyActivatedModule(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.id, elementForSlot.moduleData);
		}
	}

	private static void RestartApp()
	{
		DebugMenu componentInChildren = Main.Instance.transform.GetComponentInChildren<DebugMenu>();
		if (componentInChildren != null)
		{
			componentInChildren.CloseDebugMenu();
		}
		Singleton<UIScreenManager>.Instance.ShowPopUpScreen(new PopUpConfiguration
		{
			Data = new ErrorContent.ErrorData
			{
				Description = "Press ok to quit app so changes can take effect. (Editor must be restarted manually)"
			},
			Heading = "User profile deleted",
			Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = "OK",
					OnButtonPressed = delegate
					{
						UIUtil.QuitApplication();
					}
				}
			}
		});
	}

	private void DrawEntitlements()
	{
		GUILayout.BeginHorizontal("Box");
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Reset Insignia+Outfit slot unlocked"))
		{
			UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID, false);
			UbiCore.Utils.Singleton<LocalStorage>.Instance.SetEntitlementShown(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID, false);
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Grant all"))
		{
			Singleton<StateManager>.Instance.ProfileManager.SaveLastProfileDateBeforeMission();
			CheatParameters @params = new CheatParameters();
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.GRANT_ALL_ENTITLEMENTS, string.Empty, @params);
		}
		GUILayout.Space(10f);
		string text = ((m_tempEntitlemetnIds.Count != 0) ? ("Stop Wait Counter: " + m_tempEntitlemetnIds.Count) : "Grant Main Campaign");
		if (GUILayout.Button(text))
		{
			if (m_tempEntitlemetnIds.Count == 0)
			{
				Singleton<StateManager>.Instance.ProfileManager.SaveLastProfileDateBeforeMission();
				CampaignCatalog.Campaign campaignById = Singleton<StateManager>.Instance.TemplateManager.CampaignCatalog.GetCampaignById("ntt_5572b4db");
				for (int i = 0; i < campaignById.reqEntitlements.Length; i++)
				{
					if (!Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(campaignById.reqEntitlements[i]))
					{
						m_tempEntitlemetnIds.Enqueue(campaignById.reqEntitlements[i]);
					}
				}
				for (int j = 0; j < campaignById.missions.Length; j++)
				{
					MissionsAtLocation missionsAtLocation = campaignById.missions[j];
					for (int k = 0; k < missionsAtLocation.missions.Length; k++)
					{
						MissionInfo missionInfo = missionsAtLocation.missions[k];
						StoryMissionRewardMapping.Entry missionReward = missionInfo.GetMissionReward();
						StoryMissionRewardMapping.Entry.RankEntry currentRankNormalRewards = missionReward.GetCurrentRankNormalRewards();
						if (currentRankNormalRewards != null && !currentRankNormalRewards.lootTemplates.HasEntitlementsFomLootTemplates())
						{
							string[] entitlementsFromLootTemplate = currentRankNormalRewards.lootTemplates.GetEntitlementsFromLootTemplate();
							for (int l = 0; l < entitlementsFromLootTemplate.Length; l++)
							{
								m_tempEntitlemetnIds.Enqueue(entitlementsFromLootTemplate[l]);
							}
						}
						for (int m = 0; m < missionInfo.reqEntitlements.Length; m++)
						{
							if (!Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(missionInfo.reqEntitlements[m]))
							{
								m_tempEntitlemetnIds.Enqueue(missionInfo.reqEntitlements[m]);
							}
						}
					}
				}
				UnlockEntitlementsRecursive();
			}
			else
			{
				m_tempEntitlemetnIds.Clear();
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Reset All Entitlement Was Effect Shown Status"))
		{
			UbiCore.Utils.Singleton<LocalStorage>.Instance.ResetAllEntitlementShown();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
		GUILayout.Label("Selected Entitlement (" + EntitlementNames[m_selectedEntitlementIndex] + ")");
		if (GUILayout.Button("Grant Entitlement"))
		{
			Singleton<StateManager>.Instance.ProfileManager.SaveLastProfileDateBeforeMission();
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = EntitlementInfos[m_selectedEntitlementIndex].Guid;
			CheatParameters params2 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.GRANT_ENTITLEMENT, string.Empty, params2, delegate
			{
				m_entitlementNames = null;
			});
		}
		if (GUILayout.Button("Remove Entitlement"))
		{
			RemoveEntitlement(EntitlementInfos[m_selectedEntitlementIndex].Guid);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		m_entitlementScrollPosition = GUILayout.BeginScrollView(m_entitlementScrollPosition, GUILayout.MinHeight(200f));
		m_selectedEntitlementIndex = GUILayout.SelectionGrid(m_selectedEntitlementIndex, EntitlementNames, 1, "toggle");
		GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
	}

	private void RemoveEntitlement(int _guid)
	{
		Singleton<StateManager>.Instance.ProfileManager.SaveLastProfileDateBeforeMission();
		CheatParameters cheatParameters = new CheatParameters();
		cheatParameters.Param1 = EntitlementInfos[m_selectedEntitlementIndex].Guid;
		CheatParameters @params = cheatParameters;
		Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.REMOVE_ENTITLEMENT, string.Empty, @params, delegate
		{
			m_entitlementNames = null;
		});
	}

	private void UnlockEntitlementsRecursive()
	{
		if (m_tempEntitlemetnIds.Count != 0)
		{
			string _guid = m_tempEntitlemetnIds.Dequeue().ToLowerInvariant();
			if (string.IsNullOrEmpty(_guid) || Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(_guid))
			{
				UnlockEntitlementsRecursive();
			}
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = GameID.ComputeGuidHash(ref _guid);
			CheatParameters @params = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.GRANT_ENTITLEMENT, string.Empty, @params, delegate
			{
				m_entitlementNames = null;
				UnlockEntitlementsRecursive();
			});
		}
	}

	private void DrawAchievements()
	{
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Achievements");
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Selected Achievement (" + AchievementNames[m_selectedAchievementIndex] + ")", GUILayout.Width(300f));
		m_achievementId = Singleton<StateManager>.Instance.TemplateManager.AchievementMapping.achievements[m_selectedAchievementIndex].id;
		GUILayout.Label("rank idx");
		string text = GUILayout.TextField(m_achievementRank.ToString());
		int result;
		if (text.Length == 0)
		{
			m_achievementRank = 1;
		}
		else if (int.TryParse(text, out result))
		{
			m_achievementRank = result;
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		DrawAchievementSelector();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reach achievement"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = m_achievementRank;
			cheatParameters.Param3 = m_achievementId;
			CheatParameters @params = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.REACH_ACHIEVEMENT, string.Empty, @params);
		}
		if (GUILayout.Button("Unreach achievement"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = m_achievementRank;
			cheatParameters.Param3 = m_achievementId;
			CheatParameters params2 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.UNREACH_ACHIEVEMENT, string.Empty, params2);
		}
		if (GUILayout.Button("Reset achievement"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param3 = m_achievementId;
			CheatParameters params3 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.RESET_ACHIEVEMENT, string.Empty, params3);
		}
		if (GUILayout.Button("Set counters for achievement"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = m_achievementRank;
			cheatParameters.Param3 = m_achievementId;
			CheatParameters params4 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.SET_COUNTERS_FOR_ACHIEVEMENT, string.Empty, params4);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Unclaim achievement"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = m_achievementRank;
			cheatParameters.Param3 = m_achievementId;
			CheatParameters params5 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.UNCLAIM_ACHIEVEMENT, string.Empty, params5);
		}
		if (GUILayout.Button("Unread achievement"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = m_achievementRank;
			cheatParameters.Param3 = m_achievementId;
			CheatParameters params6 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.UNREAD_ACHIEVEMENT, string.Empty, params6);
		}
		if (GUILayout.Button("Reach all"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = 0;
			CheatParameters params7 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.REACH_OR_UNREACH_ALL_ACHIEVEMENTS, string.Empty, params7);
		}
		if (GUILayout.Button("Unreach all"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = 1;
			CheatParameters params8 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.REACH_OR_UNREACH_ALL_ACHIEVEMENTS, string.Empty, params8);
			ClientAchievementsManager.ResetAllAchievements();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Reset Client-Based Achievements"))
		{
			ClientAchievementsManager.ResetAllAchievements();
		}
		if (GUILayout.Button("Show Client-Based Achievements"))
		{
			ClientAchievementsManager.ShowAchievements();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void DrawMissionQuest()
	{
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Mission Quest");
		GUILayout.Space(10f);
		bool flag = false;
		long num = 0L;
		UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
		if (profile != null)
		{
			ProgressionQuests.RankQuests questByRank = profile.GetQuestByRank(1);
			if (questByRank != null)
			{
				num = (long)questByRank.expiresAtTimeStamp;
			}
		}
		int result = 0;
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Current TimeStamp: " + num);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Rank (\"Set Progress\") or Millisec (\"Move timestamps\")", GUILayout.Width(300f));
		m_missionQuestParam1 = GUILayout.TextField(m_missionQuestParam1, GUILayout.Width(100f));
		flag = int.TryParse(m_missionQuestParam1, out result);
		GUILayout.EndHorizontal();
		bool flag2 = GUI.enabled;
		GUI.enabled = flag;
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Set Progress"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = result;
			CheatParameters @params = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.PROGRESSION_QUESTS_PERFORM, string.Empty, @params);
		}
		if (GUILayout.Button("Move Timestamps"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param1 = result;
			CheatParameters params2 = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.PROGRESSION_QUESTS_EXPIRE, string.Empty, params2);
		}
		GUILayout.EndHorizontal();
		GUI.enabled = flag2;
		GUILayout.EndVertical();
	}

	private void DrawPushNotifications()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("message (optional)");
		m_pushNotificationMessage = GUILayout.TextField(m_pushNotificationMessage);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("title (optional, not working on all devices)");
		m_pushNotificationTitle = GUILayout.TextField(m_pushNotificationTitle);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Send test push notification"))
		{
			CheatParameters cheatParameters = new CheatParameters();
			cheatParameters.Param3 = m_pushNotificationMessage;
			cheatParameters.Param4 = m_pushNotificationTitle;
			CheatParameters @params = cheatParameters;
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.SEND_TEST_PUSH_NOTIFICATION, string.Empty, @params);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginVertical("Box");
		GUILayout.Space(20f);
		GUILayout.Label("LOCAL PUSH NOTIFICATIONS");
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("title:");
		m_pushNotificationTitle = GUILayout.TextField(m_pushNotificationTitle);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Message:");
		m_pushNotificationTitle = GUILayout.TextField(m_pushNotificationTitle);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Duration in seconds:");
		m_pushNotificationDuration = long.Parse(GUILayout.TextField(m_pushNotificationDuration.ToString()));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Alert key:");
		m_pushNotificationKey = GUILayout.TextField(m_pushNotificationKey);
		GUILayout.EndHorizontal();
		int num = Array.IndexOf(Enum.GetValues(typeof(EPushNotificationGroup)), m_selectedPushNotificationGroup);
		if (num == -1)
		{
			m_selectedPushNotificationGroup = EPushNotificationGroup.SkillTrainingGroup;
		}
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Select push notification group(no effect on iOS):");
		num = GUILayout.SelectionGrid(num, Enum.GetNames(typeof(EPushNotificationGroup)), 1, "toggle") + 1;
		GUILayout.EndVertical();
		if (GUI.changed && num != -1 && Enum.IsDefined(typeof(EPushNotificationGroup), num))
		{
			m_selectedPushNotificationGroup = (EPushNotificationGroup)num;
		}
		if (GUILayout.Button("Schedule local push notification"))
		{
			PushNotificationsProvider.ScheduleLocalNotificationWithInterval((m_pushNotificationDuration > 0) ? m_pushNotificationDuration : 60, (!string.IsNullOrEmpty(m_pushNotificationTitle)) ? m_pushNotificationTitle : "Title", (!string.IsNullOrEmpty(m_pushNotificationMessage)) ? m_pushNotificationMessage : "Message", (!string.IsNullOrEmpty(m_pushNotificationKey)) ? m_pushNotificationKey : m_selectedPushNotificationGroup.ToString(), m_selectedPushNotificationGroup);
		}
		GUILayout.EndVertical();
		GUILayout.EndVertical();
	}

	private void DrawNativeTools()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Bundle ID: " + NativeTools.BundleIdentifier);
		GUILayout.Space(10f);
		string deviceUniqueId = NativeTools.DeviceUniqueId;
		GUILayout.Label("Device unique ID: " + deviceUniqueId);
		if (GUILayout.Button("Copy device ID to clipboard", GUILayout.MaxWidth(300f)))
		{
			NativeTools.CopyToClipboard(deviceUniqueId);
			NativeTools.ShowAlertView("Copy To Clipboard", "Device unique ID was copied!");
		}
		GUILayout.Space(10f);
		string advertisingID = NativeTools.AdvertisingID;
		GUILayout.Label("Advertising ID: " + advertisingID);
		if (GUILayout.Button("Copy advertising ID to clipboard", GUILayout.MaxWidth(300f)))
		{
			NativeTools.CopyToClipboard(advertisingID);
			NativeTools.ShowAlertView("Copy To Clipboard", "Advertising ID was copied!");
		}
		GUILayout.EndVertical();
	}

	private void DrawAdColony()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Label("ADCOLONY");
		GUILayout.Label("IsAvailable: " + AdColonyManager.IsAvailable);
		GUILayout.Space(30f);
		GUILayout.BeginVertical("Box");
		GUILayout.Label("OverrideAdColonyAvailability: " + AdColonyManager.OverrideAdColonyAvailability);
		if (GUILayout.Button((!AdColonyManager.OverrideAdColonyAvailability) ? "Override AdColony Availability" : "Remove Override AdColony Availability"))
		{
			AdColonyManager.OverrideAdColonyAvailability = !AdColonyManager.OverrideAdColonyAvailability;
			if (!AdColonyManager.OverrideAdColonyAvailability)
			{
				AdColonyManager.EnableAdColonyOverride = false;
			}
		}
		GUILayout.Label("EnableAdColonyOverride: " + AdColonyManager.EnableAdColonyOverride);
		if (AdColonyManager.OverrideAdColonyAvailability && GUILayout.Button((!AdColonyManager.EnableAdColonyOverride) ? "Enable AdColony" : "Disable AdColony"))
		{
			AdColonyManager.EnableAdColonyOverride = !AdColonyManager.EnableAdColonyOverride;
		}
		GUILayout.EndVertical();
		GUILayout.Space(30f);
		GUILayout.Label("IsVideoAvailable: " + AdColonyManager.IsVideoAvailable);
		if (AdColonyManager.IsVideoAvailable)
		{
			GUILayout.Space(30f);
			if (GUILayout.Button("[SHOP] Show AdColony Video"))
			{
				AdColonyManager.Instance.ShowAdColonyVideo(AdColonyManager.ERewardType.MysteryBox);
			}
			if (GUILayout.Button("[SHOP] Show Video - showPopupBeforeAndAfter"))
			{
				AdColonyManager.Instance.ShowAdColonyVideo(AdColonyManager.ERewardType.MysteryBox, true, true);
			}
			GUILayout.Space(30f);
			if (GUILayout.Button("[REVIVE] Show AdColony Video"))
			{
				AdColonyManager.Instance.ShowAdColonyVideo(AdColonyManager.ERewardType.Revive);
			}
			if (GUILayout.Button("[REVIVE] Show Video - showPopupBeforeAndAfter"))
			{
				AdColonyManager.Instance.ShowAdColonyVideo(AdColonyManager.ERewardType.Revive, true, true);
			}
		}
		GUILayout.EndVertical();
	}

	private void Reset()
	{
		m_level = 0;
		m_exp = 0;
		m_skillpoints = 0;
		m_energy = 0;
		m_resource = 0;
		m_counter = 0;
		m_eventScore = 0;
		m_eventCounter.Clear();
		m_entitlementNames = null;
	}
}
