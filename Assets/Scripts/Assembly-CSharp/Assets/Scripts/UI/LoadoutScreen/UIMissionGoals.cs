using System.Collections.Generic;
using System.Text.RegularExpressions;
using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.UI.Localization;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.UI.LoadoutScreen
{
	public class UIMissionGoals
	{
		public class GoalData
		{
			public long ObjectiveId { get; set; }

			public string Name { get; set; }

			public int Id { get; set; }

			public bool Hidden { get; set; }

			public bool Forced { get; set; }
		}

		private int m_missionLevel;

		public bool IsElite { get; private set; }

		public List<GoalData> Challenges { get; private set; }

		public List<GoalData> AdditionalObjectives { get; private set; }

		public List<GoalData> Objectives { get; private set; }

		public Dictionary<string, int> RolledNames { get; private set; }

		public Dictionary<string, int> RolledPortraitNames { get; private set; }

		public UIMissionGoals(MissionGoals _goals, bool _isElite)
		{
			if (_goals == null)
			{
				return;
			}
			Clear();
			if (_goals.missionGoals == null || _goals.missionGoals.Length == 0)
			{
				return;
			}
			m_missionLevel = _goals.missionLevel;
			IsElite = _isElite;
			MissionGoals.MissionGoal[] missionGoals = _goals.missionGoals;
			foreach (MissionGoals.MissionGoal missionGoal in missionGoals)
			{
				if (missionGoal.elite && !_isElite)
				{
					continue;
				}
				List<GoalData> list = ParseGoals(missionGoal);
				switch (missionGoal.goalType)
				{
				case EMissionGoal.Objective_EMissionGoal:
				{
					List<GoalData> list3 = list;
					for (int k = 0; k < list3.Count; k++)
					{
						if (k == 0 && Objectives.Count == 0)
						{
							Objectives.Insert(0, list3[k]);
						}
						else
						{
							AdditionalObjectives.Insert(0, list3[k]);
						}
					}
					break;
				}
				case EMissionGoal.Modifier_EMissionGoal:
				{
					List<GoalData> list2 = list;
					for (int j = 0; j < list2.Count; j++)
					{
						AdditionalObjectives.Add(list2[j]);
					}
					break;
				}
				case EMissionGoal.Challenge_EMissionGoal:
					Challenges.AddRange(list);
					break;
				}
			}
		}

		public UIMissionGoals()
		{
			Clear();
		}

		private StringMapper CreateMapper(IEnumerable<MissionGoals.MissionGoal.MappingEntry> _mapping)
		{
			StringMapper stringMapper = new StringMapper();
			foreach (MissionGoals.MissionGoal.MappingEntry item in _mapping)
			{
				switch (item.key)
				{
				case EObjectiveBoundWildcards.Available_EObjectiveBoundWildcards:
					stringMapper.Add("available", item.valueInt);
					break;
				case EObjectiveBoundWildcards.ChallengerCount_EObjectiveBoundWildcards:
					stringMapper.Add("ChallengerCount", item.valueInt);
					break;
				case EObjectiveBoundWildcards.Count_EObjectiveBoundWildcards:
					stringMapper.Add("Count", item.valueInt);
					stringMapper.Add("kill_amount", item.valueInt);
					stringMapper.Add("kill_amount-1", item.valueInt - 1);
					stringMapper.Add("kill_amount+1", item.valueInt + 1);
					stringMapper.Add("Limit", item.valueInt);
					break;
				case EObjectiveBoundWildcards.TimeInSeconds_EObjectiveBoundWildcards:
					stringMapper.Add("TimeInSeconds", item.valueInt);
					stringMapper.Add("Seconds", item.valueInt % 60);
					stringMapper.Add("Minutes", item.valueInt / 60);
					break;
				case EObjectiveBoundWildcards.UnitGuids_EObjectiveBoundWildcards:
				{
					BBDictionary<string, int> bBDictionary = new BBDictionary<string, int>();
					string[] valueStrings = item.valueStrings;
					foreach (string text in valueStrings)
					{
						if (text == null)
						{
							continue;
						}
						int localizedLocaId = CharacterHelper.GetLocalizedLocaId(text, CharacterHelper.ECharacterNameType.Plain);
						RolledNames[text] = localizedLocaId;
						int localizedLocaId2 = CharacterHelper.GetLocalizedLocaId(text, CharacterHelper.ECharacterNameType.PortraitWithLevel);
						RolledPortraitNames[text] = localizedLocaId2;
						if (localizedLocaId > 0)
						{
							string text2 = LocalizationManager.Instance.Get(localizedLocaId, new StringMapper { { "enemy_lvl", m_missionLevel } });
							if (!bBDictionary.ContainsKey(text2))
							{
								bBDictionary.Add(text2, 0);
							}
							BBDictionary<string, int> bBDictionary2;
							BBDictionary<string, int> bBDictionary3 = (bBDictionary2 = bBDictionary);
							string key;
							string key2 = (key = text2);
							int num = bBDictionary2[key];
							bBDictionary3[key2] = num + 1;
						}
					}
					string value = ConcatCountedList(bBDictionary);
					stringMapper.Add("Units", value);
					stringMapper.Add("type_name", value);
					if (bBDictionary.Count > 0)
					{
						stringMapper.Add("Unit", bBDictionary.GetEnumerator().Current.Key);
					}
					break;
				}
				case EObjectiveBoundWildcards.StaticObjectGuid_EObjectiveBoundWildcards:
				{
					string valueString = item.valueString;
					if (valueString != null)
					{
						int localizedObjectLocaId = CharacterHelper.GetLocalizedObjectLocaId(valueString, CharacterHelper.ECharacterNameType.Plain);
						RolledNames[valueString] = localizedObjectLocaId;
						int localizedObjectLocaId2 = CharacterHelper.GetLocalizedObjectLocaId(valueString, CharacterHelper.ECharacterNameType.PortraitWithLevel);
						RolledPortraitNames[valueString] = localizedObjectLocaId2;
					}
					break;
				}
				case EObjectiveBoundWildcards.SkillName_EObjectiveBoundWildcards:
					stringMapper.Add("Name", item.valueString);
					break;
				case EObjectiveBoundWildcards.Notoriety_EObjectiveBoundWildcards:
					stringMapper.Add("Notoriety", item.valueAlertLevel);
					break;
				case EObjectiveBoundWildcards.Range_EObjectiveBoundWildcards:
					stringMapper.Add("Range", item.valueFloat);
					stringMapper.Add("Distance", item.valueFloat);
					break;
				}
			}
			return stringMapper;
		}

		private static string ConcatCountedList(BBDictionary<string, int> _rolledNames)
		{
			string text = string.Empty;
			foreach (KeyValuePair<string, int> _rolledName in _rolledNames)
			{
				if (text != string.Empty)
				{
					text += ", ";
				}
				if (_rolledName.Value > 1)
				{
					text = text + _rolledName.Value + " ";
				}
				text += _rolledName.Key;
			}
			return text;
		}

		private List<GoalData> ParseGoals(MissionGoals.MissionGoal _missonGoal)
		{
			List<GoalData> list = new List<GoalData>();
			string description = LocalizationHelper.ParseStringToLocaKey(_missonGoal.description.neutral.ToString(), CreateMapper(_missonGoal.mapping));
			description = ParseConditionalParts(description, _missonGoal.mapping);
			list.Add(new GoalData
			{
				Name = description,
				ObjectiveId = _missonGoal.objectiveId,
				Id = _missonGoal.id,
				Hidden = _missonGoal.hidden,
				Forced = _missonGoal.forced
			});
			return list;
		}

		private string ParseConditionalParts(string _description, MissionGoals.MissionGoal.MappingEntry[] _mapping)
		{
			Regex regex = new Regex("\\[(?<Inverse>!?)(?<Status>Multi)(?<Caption>[^\\]]+)\\]");
			EObjectiveBoundWildcards[] counterKeys = new EObjectiveBoundWildcards[2]
			{
				EObjectiveBoundWildcards.ChallengerCount_EObjectiveBoundWildcards,
				EObjectiveBoundWildcards.Count_EObjectiveBoundWildcards
			};
			if (!_mapping.Any((MissionGoals.MissionGoal.MappingEntry x) => counterKeys.Contains(x.key)))
			{
				return _description;
			}
			MatchCollection matchCollection = regex.Matches(_description);
			if (matchCollection.Count == 0)
			{
				return _description;
			}
			MissionGoals.MissionGoal.MappingEntry mappingEntry = _mapping.FirstOrDefault((MissionGoals.MissionGoal.MappingEntry g) => g.key == EObjectiveBoundWildcards.ChallengerCount_EObjectiveBoundWildcards);
			MissionGoals.MissionGoal.MappingEntry mappingEntry2 = _mapping.FirstOrDefault((MissionGoals.MissionGoal.MappingEntry g) => g.key == EObjectiveBoundWildcards.Count_EObjectiveBoundWildcards);
			int num = ((mappingEntry == null) ? mappingEntry2.valueInt : mappingEntry.valueInt);
			bool flag = num == 1;
			for (int i = 0; i < matchCollection.Count; i++)
			{
				Match match = matchCollection[i];
				bool flag2 = match.Groups["Inverse"].Value == "!";
				bool flag3 = flag;
				if (flag2)
				{
					flag3 = !flag3;
				}
				_description = regex.Replace(_description, (!flag3) ? "${Caption}" : string.Empty, 1);
			}
			return _description;
		}

		public void Clear(bool _isDebug = false)
		{
			RolledNames = new Dictionary<string, int>();
			RolledPortraitNames = new Dictionary<string, int>();
			Objectives = new List<GoalData>();
			AdditionalObjectives = new List<GoalData>();
			Challenges = new List<GoalData>();
			if (_isDebug)
			{
				Objectives.Add(new GoalData
				{
					Name = "WARNING: Local mission file loaded. Do not QA, for proper use ask for help."
				});
			}
		}
	}
}
