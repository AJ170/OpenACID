using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using AssetBundleData;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.RuleSystem.Rules;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class SkillData : UbiCore.Utils.Singleton<SkillData>
	{
		public class SkillPreview
		{
			public enum EBonusType
			{
				Invalid = 0,
				Additive = 1,
				Multiplicative = 2
			}

			public StatTypes.EStatType StatType { get; private set; }

			public StatTypes.EStatType RuleStatType { get; private set; }

			public float Magnitude { get; private set; }

			public EBonusType BonusType { get; private set; }

			public SkillPreview(StatTypes.EStatType _statType, StatTypes.EStatType _ruleStatType, float _magnitude, EBonusType _bonusType)
			{
				StatType = _statType;
				RuleStatType = _ruleStatType;
				Magnitude = _magnitude;
				BonusType = _bonusType;
			}
		}

		public interface ISkillDefinition : Data.IDefinition
		{
			Enum<SkillTypes> Type { get; }

			int Rank { get; }

			Enum<SkillTypes> BaseSkillType { get; }

			int MaxRank { get; }

			Enum<RuleTypes> RuleType { get; }

			RuleData.IRuleDefinition RuleDefinition { get; }

			ETriggerFrequency Frequency { get; }

			ETriggerCategoryFlags Categories { get; }

			ESkillActivationType ActivationType { get; }

			bool Blocking { get; }

			bool ForceSkillWheel { get; }

			bool Abortable { get; }

			int Priority { get; }

			LocaKey NameLocaKey { get; }

			LocaKey DescriptionLocaKey { get; }

			UnityResource IconUp { get; }

			UnityResource IconDown { get; }
		}

		public interface IApproachSkillDefinition : Data.IDefinition, ISkillDefinition
		{
			EInteractionReason InteractionReason { get; }

			IconMaterial IconMaterial { get; }

			float MaxDistance { get; }

			float MinDistance { get; }

			float StartDistance { get; }

			float MinHeight { get; }

			float StartHeight { get; }

			EMovementSpeed Speed { get; }

			bool StopAfterApproaching { get; }

			bool RetainPathAfterApproaching { get; }

			bool KeepSelectionAfterApproaching { get; }

			TargetApproacher.EModeFlags ApproachingModeFlags { get; }
		}

		[Database("SkillDefinition")]
		public class SkillDefinition : Data.Definition, Data.IDefinition, ISkillDefinition
		{
			private Enum<SkillTypes> m_Type;

			private int m_Rank;

			private int m_MaxRank;

			private Enum<SkillTypes> m_BaseSkillType;

			private Enum<RuleTypes> m_RuleType;

			private ETriggerCategoryFlags m_Categories;

			private ESkillActivationType m_ActivationType;

			private int m_Priority;

			private bool m_Blocking;

			private bool m_Abortable;

			private bool m_ForceSkillWheel;

			public override bool Export
			{
				get
				{
					return m_Rank == -1;
				}
			}

			public Enum<SkillTypes> Type
			{
				get
				{
					return m_Type;
				}
			}

			public int Rank
			{
				get
				{
					return m_Rank;
				}
			}

			public Enum<SkillTypes> BaseSkillType
			{
				get
				{
					return m_BaseSkillType;
				}
			}

			public int MaxRank
			{
				get
				{
					return m_MaxRank;
				}
			}

			public Enum<RuleTypes> RuleType
			{
				get
				{
					return m_RuleType;
				}
			}

			public RuleData.IRuleDefinition RuleDefinition
			{
				get
				{
					return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(RuleType);
				}
			}

			public ETriggerFrequency Frequency
			{
				get
				{
					ESkillActivationType activationType = ActivationType;
					if (activationType == ESkillActivationType.Passive)
					{
						return ETriggerFrequency.Turnwise;
					}
					return ETriggerFrequency.Framewise;
				}
			}

			public LocaKey NameLocaKey { get; set; }

			public LocaKey DescriptionLocaKey { get; set; }

			public UnityResource IconUp { get; set; }

			public UnityResource IconDown { get; set; }

			public ETriggerCategoryFlags Categories
			{
				get
				{
					return m_Categories;
				}
			}

			public ESkillActivationType ActivationType
			{
				get
				{
					return m_ActivationType;
				}
			}

			public bool Blocking
			{
				get
				{
					return m_Blocking;
				}
			}

			public bool ForceSkillWheel
			{
				get
				{
					return m_ForceSkillWheel;
				}
			}

			public bool Abortable
			{
				get
				{
					return m_Abortable;
				}
			}

			public int Priority
			{
				get
				{
					return m_Priority;
				}
			}

			public override Type InstanceType
			{
				get
				{
					return null;
				}
			}

			public SkillDefinition(AssetBundleInfo _assetBundleInfo)
				: base(_assetBundleInfo)
			{
				IconUp = new UnityResource(_assetBundleInfo);
				IconDown = new UnityResource(_assetBundleInfo);
			}

			protected SkillDefinition(AssetBundleInfo _assetBundleInfo, SkillDefinition _definition, int _rank)
				: base(_assetBundleInfo, true, _definition)
			{
				m_Rank = _rank;
				m_MaxRank = _definition.m_MaxRank;
				m_Type = Enum<SkillTypes>.GetOrAdd((m_MaxRank <= 0) ? base.Name : FormatSkillName(base.Name, m_Rank));
				string name = "Skill_" + m_Type.ToString();
				m_RuleType = Enum<RuleTypes>.GetOrAdd(name);
				m_ActivationType = _definition.ActivationType;
				m_BaseSkillType = _definition.m_BaseSkillType;
				m_Categories = _definition.m_Categories;
				m_Priority = _definition.m_Priority;
				m_Abortable = _definition.m_Abortable;
				m_Blocking = _definition.m_Blocking;
				m_ForceSkillWheel = _definition.ForceSkillWheel;
				NameLocaKey = _definition.NameLocaKey;
				DescriptionLocaKey = _definition.DescriptionLocaKey;
				IconUp = _definition.IconUp;
				IconDown = _definition.IconDown;
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				m_ActivationType = (ESkillActivationType)_s.SerializeEnum("ActivationType", m_ActivationType.GetType(), (int)m_ActivationType);
				if (_s.Mode == EMode.EMode_Read)
				{
					switch (m_ActivationType)
					{
					case ESkillActivationType.TriggerSlot:
					case ESkillActivationType.SelectSlot:
					case ESkillActivationType.Passive:
						m_MaxRank = 4;
						break;
					default:
						m_MaxRank = 0;
						break;
					}
					m_Rank = -1;
				}
				m_Categories = (ETriggerCategoryFlags)_s.SerializeEnum("Categories", m_Categories.GetType(), (int)m_Categories);
				m_Priority = _s.SerializeInt("Priority", m_Priority);
				m_Abortable = _s.SerializeBool("Abortable", m_Abortable);
				m_ForceSkillWheel = _s.SerializeBool("ForceSkillWheel", m_ForceSkillWheel);
				m_Blocking = _s.SerializeBool("Blocking", m_Blocking);
				if (_s.Mode == EMode.EMode_Read)
				{
					m_MaxRank = _s.SerializeInt("MaxRank") - 1;
				}
				else
				{
					_s.SerializeInt("MaxRank", MaxRank + 1);
				}
				if (_s.Mode == EMode.EMode_Read)
				{
					NameLocaKey = new LocaKey();
					DescriptionLocaKey = new LocaKey();
				}
				m_BaseSkillType = Enum<SkillTypes>.GetOrAdd((m_MaxRank <= 0) ? base.Name : FormatSkillName(base.Name, 0));
				NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
				DescriptionLocaKey.Key = _s.SerializeInt("DescriptionLocaKey", DescriptionLocaKey.Key);
				IconUp.Serialize("IconUp", _s, base.AssetBundleInfo);
				IconDown.Serialize("IconDown", _s, base.AssetBundleInfo);
				if (_s.Mode == EMode.EMode_Read && GetType() == typeof(SkillDefinition))
				{
					FillRankDefinitions(base.AssetBundleInfo);
				}
			}

			public override void InitializePass1()
			{
				base.InitializePass1();
				RuleData.IRuleDefinition definition = UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(m_RuleType);
				if (definition != null)
				{
					((RuleData.RuleDefinition)definition).SetSkillType(m_Type);
				}
			}

			protected void FillRankDefinitions(AssetBundleInfo _assetBundleInfo)
			{
				if (!UbiCore.Utils.Singleton<SkillData>.IsAvailable)
				{
					return;
				}
				for (int i = 0; i <= m_MaxRank; i++)
				{
					SkillDefinition skillDefinition = Clone(_assetBundleInfo, i);
					UbiCore.Utils.Singleton<SkillData>.Instance.AddDefinition(skillDefinition);
					ApproachSkillDefinition approachSkillDefinition = skillDefinition as ApproachSkillDefinition;
					if (approachSkillDefinition != null)
					{
						UbiCore.Utils.Singleton<SkillData>.Instance.AddApproachSkill(approachSkillDefinition);
					}
				}
			}

			protected virtual SkillDefinition Clone(AssetBundleInfo _assetBundleInfo, int _rank)
			{
				return new SkillDefinition(_assetBundleInfo, this, _rank);
			}
		}

		public class ApproachSkillDefinition : SkillDefinition, Data.IDefinition, ISkillDefinition, IApproachSkillDefinition
		{
			private EInteractionReason m_InteractionReason;

			private IconMaterial m_IconMaterial;

			private EMovementSpeed m_Speed;

			private bool m_StopAfterApproaching;

			private bool m_RetainPathAfterApproaching;

			private bool m_KeepSelectionAfterApproaching;

			private TargetApproacher.EModeFlags m_ApproachingModeFlags;

			public EInteractionReason InteractionReason
			{
				get
				{
					return m_InteractionReason;
				}
			}

			public IconMaterial IconMaterial
			{
				get
				{
					return m_IconMaterial;
				}
			}

			public float MaxDistance
			{
				get
				{
					return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(RuleType).StatValueCollection.Get_float(StatTypes.EStatType.Rule_ApproachMaxDistance);
				}
			}

			public float MinDistance
			{
				get
				{
					return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(RuleType).StatValueCollection.Get_float(StatTypes.EStatType.Rule_ApproachMinDistance);
				}
			}

			public float StartDistance
			{
				get
				{
					return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(RuleType).StatValueCollection.Get_float(StatTypes.EStatType.Rule_ApproachStartDistance);
				}
			}

			public float MinHeight
			{
				get
				{
					return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(RuleType).StatValueCollection.Get_float(StatTypes.EStatType.Rule_ApproachMinHeight);
				}
			}

			public float StartHeight
			{
				get
				{
					return UbiCore.Utils.Singleton<RuleData>.Instance.GetDefinition(RuleType).StatValueCollection.Get_float(StatTypes.EStatType.Rule_ApproachStartHeight);
				}
			}

			public EMovementSpeed Speed
			{
				get
				{
					return m_Speed;
				}
			}

			public bool StopAfterApproaching
			{
				get
				{
					return m_StopAfterApproaching;
				}
			}

			public bool RetainPathAfterApproaching
			{
				get
				{
					return m_RetainPathAfterApproaching;
				}
			}

			public bool KeepSelectionAfterApproaching
			{
				get
				{
					return m_KeepSelectionAfterApproaching;
				}
			}

			public TargetApproacher.EModeFlags ApproachingModeFlags
			{
				get
				{
					return m_ApproachingModeFlags;
				}
			}

			public ApproachSkillDefinition(AssetBundleInfo _assetBundleInfo)
				: base(_assetBundleInfo)
			{
			}

			protected ApproachSkillDefinition(AssetBundleInfo _assetBundleInfo, ApproachSkillDefinition _definition, int _rank)
				: base(_assetBundleInfo, _definition, _rank)
			{
				m_InteractionReason = _definition.m_InteractionReason;
				m_IconMaterial = _definition.m_IconMaterial;
				m_Speed = _definition.m_Speed;
				m_StopAfterApproaching = _definition.StopAfterApproaching;
				m_RetainPathAfterApproaching = _definition.m_RetainPathAfterApproaching;
				m_KeepSelectionAfterApproaching = _definition.m_KeepSelectionAfterApproaching;
				m_ApproachingModeFlags = _definition.ApproachingModeFlags;
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				m_InteractionReason = (EInteractionReason)_s.SerializeEnum("InteractionReason", m_InteractionReason.GetType(), (int)m_InteractionReason);
				m_IconMaterial = (IconMaterial)_s.SerializeEnum("IconMaterial", m_IconMaterial.GetType(), (int)m_IconMaterial);
				m_Speed = (EMovementSpeed)_s.SerializeEnum("Speed", m_Speed.GetType(), (int)m_Speed);
				m_StopAfterApproaching = _s.SerializeBool("StopAfterApproaching", m_StopAfterApproaching);
				m_RetainPathAfterApproaching = _s.SerializeBool("RetainPathAfterApproaching", m_RetainPathAfterApproaching);
				m_KeepSelectionAfterApproaching = _s.SerializeBool("KeepSelectionAfterApproaching", m_KeepSelectionAfterApproaching);
				m_ApproachingModeFlags = (TargetApproacher.EModeFlags)_s.SerializeEnum("ApproachingModeFlags", m_ApproachingModeFlags.GetType(), (int)m_ApproachingModeFlags);
				if (_s.Mode == EMode.EMode_Read && GetType() == typeof(ApproachSkillDefinition))
				{
					FillRankDefinitions(base.AssetBundleInfo);
				}
			}

			protected override SkillDefinition Clone(AssetBundleInfo _assetBundleInfo, int _rank)
			{
				return new ApproachSkillDefinition(_assetBundleInfo, this, _rank);
			}
		}

		private static BBList<float> s_parameters = new BBList<float>();

		private readonly BBList<SkillDefinition> m_Definitions = new BBList<SkillDefinition>();

		private readonly ApproachSkillDefinition[] m_SkillDefinitionsByInteractionReason = new ApproachSkillDefinition[22];

		private readonly BBList<ISkillDefinition> m_ActiveSkillDefinitions = new BBList<ISkillDefinition>();

		private readonly ReadOnlyCollection<ISkillDefinition> m_ActiveSkillDefinitionsAccess;

		private readonly BBList<IApproachSkillDefinition> m_ApproachSkillDefinitions = new BBList<IApproachSkillDefinition>();

		private readonly ReadOnlyCollection<IApproachSkillDefinition> m_ApproachSkillDefinitionsAccess;

		private static readonly BBDictionary<string, SkillPreview> s_SkillPreviewCache = new BBDictionary<string, SkillPreview>();

		public ReadOnlyCollection<ISkillDefinition> ActiveSkillDefinitions
		{
			get
			{
				return m_ActiveSkillDefinitionsAccess;
			}
		}

		public ReadOnlyCollection<IApproachSkillDefinition> ApproachSkillDefinitions
		{
			get
			{
				return m_ApproachSkillDefinitionsAccess;
			}
		}

		public SkillData()
		{
			m_ActiveSkillDefinitionsAccess = new ReadOnlyCollection<ISkillDefinition>(m_ActiveSkillDefinitions);
			m_ApproachSkillDefinitionsAccess = new ReadOnlyCollection<IApproachSkillDefinition>(m_ApproachSkillDefinitions);
		}

		public static string FormatSkillName(string _baseName, int _rank)
		{
			return string.Format("{0}_{1:D2}", _baseName, _rank + 1);
		}

		public ISkillDefinition GetDefinition(Enum<SkillTypes> _type)
		{
			if (!_type.IsValid)
			{
				return null;
			}
			return m_Definitions[(int)_type];
		}

		public ISkillDefinition GetDefinition(string _guidString, int _rank)
		{
			GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_guidString);
			ISkillDefinition skillDefinition = Main.SafeGameDb.GetItem<DBEntry>(gameID) as ISkillDefinition;
			if (skillDefinition != null)
			{
				Enum<SkillTypes> type = new Enum<SkillTypes>((skillDefinition.MaxRank != 0) ? FormatSkillName(skillDefinition.Name, _rank - 1) : skillDefinition.Name);
				if (type.IsValid)
				{
					ISkillDefinition definition = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(type);
					if (definition != null)
					{
						return definition;
					}
				}
			}
			return null;
		}

		public Enum<SkillTypes> ParseSkillString(string _skillString, out float[] _parameters)
		{
			s_parameters.Clear();
			string[] array = _skillString.Split(new string[1] { "::" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 1; i < array.Length; i++)
			{
				float result;
				if (float.TryParse(array[i], out result))
				{
					s_parameters.Add(result);
				}
			}
			_parameters = s_parameters.ToArray();
			return _skillString.ToEnum<SkillTypes>();
		}

		public IApproachSkillDefinition GetBuffedApproachSkillDefinition(EInteractionReason _reason, ISkillsCarrier _skillsCarrier)
		{
			if (_reason == EInteractionReason.Invalid)
			{
				return null;
			}
			ApproachSkillDefinition approachSkillDefinition = m_SkillDefinitionsByInteractionReason[(int)_reason];
			object result;
			if (approachSkillDefinition == null)
			{
				IApproachSkillDefinition approachSkillDefinition2 = null;
				result = approachSkillDefinition2;
			}
			else
			{
				result = (IApproachSkillDefinition)_skillsCarrier.Skills.GetBuffedSkillDefinition(approachSkillDefinition.Type);
			}
			return (IApproachSkillDefinition)result;
		}

		public string MakeSkillString(string _skillGuid, float _effectValue)
		{
			GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_skillGuid);
			SkillDefinition item = Main.SafeGameDb.GetItem<SkillDefinition>(gameID);
			if (item != null)
			{
				return string.Format("{0}::{1}", item.Name, _effectValue);
			}
			return string.Empty;
		}

		public SkillPreview GetSkillPreview(string _skillGuid, float _effectValue, bool _doInversion)
		{
			return GetSkillPreview(MakeSkillString(_skillGuid, _effectValue), _doInversion);
		}

		public SkillPreview GetSkillPreview(string _skillString, bool _doInversion)
		{
			SkillPreview value = null;
			string key = _skillString + ((!_doInversion) ? string.Empty : "_inverted");
			if (s_SkillPreviewCache.TryGetValue(key, out value))
			{
				return value;
			}
			float[] _parameters;
			Enum<SkillTypes> skillType = ParseSkillString(_skillString, out _parameters);
			ISkillDefinition definition = skillType.GetDefinition();
			if (definition != null)
			{
				PassiveStatModifierRuleBase<PlayerCharacter> passiveStatModifierRuleBase = Activator.CreateInstance(definition.RuleDefinition.InstanceType, definition.RuleDefinition, _parameters) as PassiveStatModifierRuleBase<PlayerCharacter>;
				if (passiveStatModifierRuleBase != null && passiveStatModifierRuleBase.BuffGenerator != null)
				{
					INumberBuff numberBuff = null;
					BBList<IBuff> bBList = new BBList<IBuff>();
					passiveStatModifierRuleBase.BuffGenerator.MakeBuffs(bBList, null, null, _doInversion);
					for (int i = 0; i < bBList.Count; i++)
					{
						INumberBuff numberBuff2 = bBList[i] as INumberBuff;
						if (numberBuff2 != null && numberBuff2.IsActivePredicate == null)
						{
							numberBuff = numberBuff2;
							break;
						}
					}
					if (numberBuff != null)
					{
						SkillPreview.EBonusType bonusType = SkillPreview.EBonusType.Invalid;
						if (numberBuff is IAdditiveNumberBuff)
						{
							bonusType = SkillPreview.EBonusType.Additive;
						}
						else if (numberBuff is IMultiplicativeNumberBuff)
						{
							bonusType = SkillPreview.EBonusType.Multiplicative;
						}
						value = new SkillPreview(numberBuff.StatType, passiveStatModifierRuleBase.BuffGenerator.ValueRuleStat, numberBuff.Magnitude, bonusType);
					}
				}
			}
			s_SkillPreviewCache[key] = value;
			return value;
		}

		private void AddDefinition(SkillDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				int index = _definition.Type.Index;
				while (m_Definitions.Count <= index)
				{
					m_Definitions.Add(null);
				}
				m_Definitions[(int)_definition.Type] = _definition;
				if ((_definition.ActivationType & ESkillActivationType.Actives) == _definition.ActivationType)
				{
					m_ActiveSkillDefinitions.Add(_definition);
				}
			}
		}

		private void AddApproachSkill(ApproachSkillDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh && _definition.Rank == 0)
			{
				m_SkillDefinitionsByInteractionReason[(int)_definition.InteractionReason] = _definition;
				m_ApproachSkillDefinitions.Add(_definition);
				m_ApproachSkillDefinitions.Sort((IApproachSkillDefinition l, IApproachSkillDefinition r) => l.Priority.CompareTo(r.Priority));
			}
		}
	}
}
