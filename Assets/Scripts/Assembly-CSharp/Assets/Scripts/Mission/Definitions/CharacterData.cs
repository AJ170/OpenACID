using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.Definitions
{
	public static class CharacterData
	{
		public interface ICharacterDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IRulesCollectionCarrier, IReadonlyStatValueCollection
		{
			GameID HumanoidGameID { get; }

			GameID ColorSet { get; }

			IdleType DefaultIdleType { get; }

			IdleType TransitionIdleType { get; }

			string DisplayNameFallback { get; }

			ReadOnlyCollection<int> PortraitDisplayNameLocaKeys { get; }

			Vector3 DetectionCastOffset { get; }

			Vector3 DefaultProjectileSpawnOffset { get; }

			ReadOnlyCollection<RulesCollection.RuleParamPair> ComplementedRuleDefinitions { get; }

			IdleType GetIdleType(ENpcIdleCategory _category);

			AnimatorStateMachineStatic.StateHashValue GetStateHash(IdleType _idleType);
		}

		public abstract class CharacterDefinition : EntityData.EntityDefinition, ICharacterDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IRulesCollectionCarrier, IReadonlyStatValueCollection
		{
			private readonly BBList<RulesCollection.RuleParamPair> m_ComplementedRuleDefinitions = new BBList<RulesCollection.RuleParamPair>();

			private readonly ReadOnlyCollection<RulesCollection.RuleParamPair> m_ComplementedRuleDefinitionsAccess;

			private readonly BBList<int> m_PortraitDisplayNameLocaKeys = new BBList<int>();

			private readonly ReadOnlyCollection<int> m_PortraitDisplayNameLocaKeysAccess;

			public GameID HumanoidGameID { get; set; }

			public GameID ColorSet { get; set; }

			public string DisplayNameFallback { get; private set; }

			public ReadOnlyCollection<int> PortraitDisplayNameLocaKeys
			{
				get
				{
					return m_PortraitDisplayNameLocaKeysAccess;
				}
			}

			public Vector3 DetectionCastOffset { get; protected set; }

			public Vector3 DefaultProjectileSpawnOffset { get; protected set; }

			public ReadOnlyCollection<RulesCollection.RuleParamPair> ComplementedRuleDefinitions
			{
				get
				{
					return m_ComplementedRuleDefinitionsAccess;
				}
			}

			public abstract IdleType DefaultIdleType { get; }

			public abstract IdleType TransitionIdleType { get; }

			public RulesCollection RulesCollection { get; private set; }

			public override bool SkipLogicUpdateAllowed
			{
				get
				{
					return EntityType == EntityTypes.EType.CrowdCharacter;
				}
			}

			protected CharacterDefinition(IStatsHelper[] _statsHelpers, AssetBundleInfo _assetBundleInfo)
				: base(_statsHelpers, _assetBundleInfo)
			{
				m_ComplementedRuleDefinitionsAccess = new ReadOnlyCollection<RulesCollection.RuleParamPair>(m_ComplementedRuleDefinitions);
				m_PortraitDisplayNameLocaKeysAccess = new ReadOnlyCollection<int>(m_PortraitDisplayNameLocaKeys);
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				HumanoidGameID = SerializeGameID("HumanoidGameID", _s, this, HumanoidGameID);
				ColorSet = SerializeGameID("ColorSet", _s, this, ColorSet);
			}

			public abstract IdleType GetIdleType(ENpcIdleCategory _category);

			public virtual AnimatorStateMachineStatic.StateHashValue GetStateHash(IdleType _idleType)
			{
				return GetStateHashStatic(_idleType);
			}

			public override void InitializePass1()
			{
				base.InitializePass1();
				ComplementRuleDefinitions();
			}

			public override void InitializePass2()
			{
				base.InitializePass2();
			}

			protected void ComplementRuleDefinitions()
			{
				string[] valuesCopy_string = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_Skills).GetValuesCopy_string();
				for (int i = 0; i < valuesCopy_string.Length; i++)
				{
					string text = valuesCopy_string.GetValue(i).ToString();
					if (!string.IsNullOrEmpty(text))
					{
						float[] _parameters;
						Enum<SkillTypes> skillType = UbiCore.Utils.Singleton<SkillData>.Instance.ParseSkillString(text, out _parameters);
						if (skillType.IsValid)
						{
							SkillData.ISkillDefinition definition = skillType.GetDefinition();
							AddToComplementedRuleCollection(text, definition, _parameters);
						}
					}
				}
				IReadonlyStatCollectionValue readonlyStatCollectionValue = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_Rules);
				for (int j = 0; j < readonlyStatCollectionValue.Count; j++)
				{
					RuleData.IRuleDefinition definition2 = readonlyStatCollectionValue.Get_string(j).ToEnum<RuleTypes>().GetDefinition();
					m_ComplementedRuleDefinitions.Add(new RulesCollection.RuleParamPair
					{
						RuleDefinition = definition2
					});
				}
			}

			internal void BuildRulesCollection()
			{
				if (RulesCollection == null)
				{
					RulesCollection = new RulesCollection(m_ComplementedRuleDefinitions);
				}
			}

			protected virtual void PostConstructor()
			{
				DisplayNameFallback = m_StatValueCollection.Get_String(StatTypes.EStatType.Entity_DisplayNameFallback).ToString();
				IReadonlyStatCollectionValue readonlyStatCollectionValue = m_StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_PortraitDisplayNameLocaKeys);
				for (int i = 0; i < readonlyStatCollectionValue.Count; i++)
				{
					m_PortraitDisplayNameLocaKeys.Add(readonlyStatCollectionValue.Get_int(i));
				}
				DetectionCastOffset = m_StatValueCollection.Get_Vector3(StatTypes.EStatType.Character_DetectionCastOffset);
				DefaultProjectileSpawnOffset = m_StatValueCollection.Get_Vector3(StatTypes.EStatType.Character_DefaultProjectileSpawnOffset);
			}

			protected virtual void AddToComplementedRuleCollection(string _skillStr, SkillData.ISkillDefinition _skillDefinition, IList<float> _parameter)
			{
				AddToComplementedRuleCollectionInternal(_skillDefinition, _parameter);
			}

			protected void AddToComplementedRuleCollectionInternal(SkillData.ISkillDefinition _skillDefinition, IList<float> _parameter)
			{
				for (int i = 0; i <= _skillDefinition.MaxRank; i++)
				{
					RuleData.IRuleDefinition ruleDefinition = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(_skillDefinition.BaseSkillType.Plus(i)).RuleDefinition;
					m_ComplementedRuleDefinitions.Add(new RulesCollection.RuleParamPair
					{
						RuleDefinition = ruleDefinition,
						Parameters = _parameter
					});
				}
			}
		}

		private static readonly AnimatorStateMachineStatic.StateHashValue[] m_idleStateHashes;

		static CharacterData()
		{
			m_idleStateHashes = new AnimatorStateMachineStatic.StateHashValue[108];
			m_idleStateHashes[0] = AnimationStateHashes.State_HighProfile_Idle_Footl;
			m_idleStateHashes[1] = AnimationStateHashes.State_LowProfile_Idle_Footm;
			m_idleStateHashes[2] = AnimationStateHashes.State_LowProfile_Idle_Footm;
			m_idleStateHashes[3] = AnimationStateHashes.State_Npc_Idle_Count;
			m_idleStateHashes[4] = AnimationStateHashes.State_Npc_Idle_Sit_Crossed_Legs;
			m_idleStateHashes[5] = AnimationStateHashes.State_Npc_Idle_StandAndWaitMale_01;
			m_idleStateHashes[6] = AnimationStateHashes.State_Npc_Idle_StandAndTalkMale_01;
			m_idleStateHashes[7] = AnimationStateHashes.State_Npc_Idle_StandAndArgumentMale_02;
			m_idleStateHashes[8] = AnimationStateHashes.State_Npc_Idle_StandActArrogantMale_01;
			m_idleStateHashes[9] = AnimationStateHashes.State_Npc_Idle_SittingOnGroundTalkingMale;
			m_idleStateHashes[10] = AnimationStateHashes.State_Npc_Idle_AdvertisingGoodsMale;
			m_idleStateHashes[11] = AnimationStateHashes.State_LowProfile_Idle_Footm;
			m_idleStateHashes[13] = AnimationStateHashes.State_Npc_Idle_Searching_Base;
			m_idleStateHashes[105] = AnimationStateHashes.State_Npc_Idle_StandAndWaitFemale_Base;
			m_idleStateHashes[106] = AnimationStateHashes.State_LowProfile_Idle_Footm;
			m_idleStateHashes[107] = AnimationStateHashes.State_Npc_Idle_StandAndListenFemale_Base;
			m_idleStateHashes[16] = AnimationStateHashes.State_Npc_Idle_Cheer_Joy_01;
			m_idleStateHashes[12] = AnimationStateHashes.State_DoctorIdle_DoctorWait_1;
		}

		public static AnimatorStateMachineStatic.StateHashValue GetStateHashStatic(IdleType _idleType)
		{
			switch (_idleType)
			{
			case IdleType.CapturedNpcMale:
				return AnimationStateHashes.States_CapturedNpcIdle[LogicMain.Random.Rand(0, AnimationStateHashes.States_CapturedNpcIdle.Length - 1)];
			case IdleType.Shocked:
				return AnimationStateHashes.States_Idle_Shocked_Loop[LogicMain.Random.Rand(0, AnimationStateHashes.States_Idle_Shocked_Loop.Length - 1)];
			default:
				return m_idleStateHashes[(int)_idleType];
			}
		}
	}
}
