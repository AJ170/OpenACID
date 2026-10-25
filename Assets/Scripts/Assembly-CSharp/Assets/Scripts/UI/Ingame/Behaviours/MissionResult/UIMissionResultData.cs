using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.UI.LoadoutScreen;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Ingame.Behaviours.MissionResult
{
	public class UIMissionResultData
	{
		private MissionReward m_missionRewardData;

		private InventoryElement[] m_challengeInventoryElements;

		private InventoryElement[] m_extraInventoryElements;

		public bool HasValidRewardData
		{
			get
			{
				return m_missionRewardData != null;
			}
		}

		public EMissionKind BaseType { get; private set; }

		public EObjectiveType ObjectiveType { get; private set; }

		public int ChallengesCompleted { get; private set; }

		public InventoryElement[] Elements
		{
			get
			{
				return m_missionRewardData.elements;
			}
		}

		public MissionGoalLoot[] ChallengeLoots
		{
			get
			{
				return m_missionRewardData.challengeLoots;
			}
		}

		public UIMissionGoals MissionGoals { get; private set; }

		public int NumberOfChallenges { get; private set; }

		public int SkillPoints
		{
			get
			{
				return m_missionRewardData.skillPoints;
			}
		}

		public int SoftCurrencyMain { get; private set; }

		public int SoftCurrencyChallenge { get; private set; }

		public int SoftCurrencyExtra { get; private set; }

		public int HeroicCurrencyMain { get; private set; }

		public int HeroicCurrencyChallenge { get; private set; }

		public int HeroicCurrencyExtra { get; private set; }

		public int ExpMain { get; private set; }

		public int ExpChallenge { get; private set; }

		public UIMissionResultData()
		{
			MissionGoals = MissionMetaDataManager.Instance.MissionGoals;
			BaseType = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.Data.MissionKind;
			ObjectiveType = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.Data.ObjectiveType;
			Update();
		}

		public void SetMissionRewardData(MissionReward _value)
		{
			m_missionRewardData = _value;
			Update();
		}

		public bool HasResources()
		{
			return m_missionRewardData.resources != null;
		}

		public InventoryElement[] GetChallengeItems()
		{
			return m_challengeInventoryElements;
		}

		public InventoryElement[] GetExtraItems()
		{
			return m_extraInventoryElements;
		}

		public void UpdateChallengeData()
		{
			NumberOfChallenges = 0;
			ChallengesCompleted = 0;
			if (!UbiCore.Utils.Singleton<MissionManager>.IsAvailable)
			{
				return;
			}
			List<UIMissionGoals.GoalData> list = MissionGoals.Challenges.Where((UIMissionGoals.GoalData x) => !x.Hidden);
			ReadOnlyCollection<int> succeededChallenges = LogicMain.MissionResult.SucceededChallenges;
			NumberOfChallenges = list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				UIMissionGoals.GoalData goalData = list[i];
				for (int j = 0; j < succeededChallenges.Count; j++)
				{
					if (goalData.Id == succeededChallenges[j])
					{
						ChallengesCompleted++;
					}
				}
			}
		}

		private void Update()
		{
			ResetCurrencyRewards();
			UpdateChallengeData();
			UpdateCurrencyRewards();
			UpdateChallengeItemRewards();
		}

		private void UpdateChallengeItemRewards()
		{
			m_challengeInventoryElements = new InventoryElement[0];
			m_extraInventoryElements = new InventoryElement[0];
			if (m_missionRewardData == null)
			{
				return;
			}
			for (int i = 0; i < m_missionRewardData.challengeLoots.Length; i++)
			{
				MissionGoalLoot goalLoot = m_missionRewardData.challengeLoots[i];
				UIMissionGoals.GoalData goalData = MissionMetaDataManager.Instance.MissionGoals.Challenges.Where((UIMissionGoals.GoalData x) => x.Id == goalLoot.missionGoalId).FirstOrDefault((UIMissionGoals.GoalData x) => x.Id == goalLoot.missionGoalId);
				if (goalData == null)
				{
					continue;
				}
				if (goalData.Hidden)
				{
					m_extraInventoryElements = m_extraInventoryElements.AddRange(goalLoot.elements.elements);
					for (int j = 0; j < goalLoot.resources.Length; j++)
					{
						if (goalLoot.resources[j].resource.type == EResourceType.SoftCurrency)
						{
							SoftCurrencyExtra += (int)goalLoot.resources[j].resource.amount;
						}
						else if (goalLoot.resources[j].resource.type == EResourceType.SocialCurrency)
						{
							HeroicCurrencyExtra += (int)goalLoot.resources[j].resource.amount;
						}
					}
					continue;
				}
				m_challengeInventoryElements = m_challengeInventoryElements.AddRange(goalLoot.elements.elements);
				for (int k = 0; k < goalLoot.resources.Length; k++)
				{
					if (goalLoot.resources[k].resource.type == EResourceType.SoftCurrency)
					{
						SoftCurrencyChallenge += (int)goalLoot.resources[k].resource.amount;
					}
					else if (goalLoot.resources[k].resource.type == EResourceType.SocialCurrency)
					{
						HeroicCurrencyChallenge += (int)goalLoot.resources[k].resource.amount;
					}
				}
			}
		}

		private void UpdateCurrencyRewards()
		{
			if (m_missionRewardData == null)
			{
				return;
			}
			ExpMain += (int)m_missionRewardData.xpWithoutChallenge;
			ExpChallenge += (int)(m_missionRewardData.finalXp - ExpMain);
			SoftCurrencyExtra += (int)m_missionRewardData.scFromPickpocketing + (int)m_missionRewardData.scFromHireling;
			SoftCurrencyMain += (int)m_missionRewardData.scWithoutChallenge - SoftCurrencyExtra;
			HeroicCurrencyExtra += (int)m_missionRewardData.htFromPickpocketing + (int)m_missionRewardData.htFromHireling;
			HeroicCurrencyMain += (int)m_missionRewardData.htWithoutChallenge - HeroicCurrencyExtra;
			for (int i = 0; i < m_missionRewardData.resources.Length; i++)
			{
				Resource resource = m_missionRewardData.resources[i].resource;
				if (resource != null)
				{
					if (resource.type == EResourceType.SocialCurrency)
					{
						HeroicCurrencyExtra += (int)resource.amount;
					}
					else if (resource.type == EResourceType.SoftCurrency)
					{
						SoftCurrencyExtra += (int)resource.amount;
					}
				}
			}
			SoftCurrencyChallenge += (int)m_missionRewardData.finalSc - (SoftCurrencyMain + SoftCurrencyExtra);
			HeroicCurrencyChallenge += (int)m_missionRewardData.finalHt - (HeroicCurrencyMain + HeroicCurrencyExtra);
		}

		private void ResetCurrencyRewards()
		{
			ExpMain = 0;
			ExpChallenge = 0;
			SoftCurrencyExtra = 0;
			SoftCurrencyMain = 0;
			SoftCurrencyChallenge = 0;
			HeroicCurrencyExtra = 0;
			HeroicCurrencyMain = 0;
			HeroicCurrencyChallenge = 0;
		}
	}
}
