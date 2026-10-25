using System;
using AssetBundleData;
using Assets.Scripts.Mission.Logic.RuleSystem;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class RuleData : UbiCore.Utils.Singleton<RuleData>
	{
		public interface IRuleDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, IReadonlyStatValueCollection
		{
			Enum<RuleTypes> Type { get; }

			Enum<SkillTypes> SkillType { get; }

			GameID EffectPackageGameID { get; }

			Type ClassType { get; }
		}

		[Database("RuleDefinition")]
		public class RuleDefinition : DataWithStats.DefinitionWithStats, Data.IDefinition, DataWithStats.IDefinitionWithStats, IRuleDefinition, IReadonlyStatValueCollection
		{
			public static readonly IStatsHelper[] StatsHelpers = new IStatsHelper[1] { StatsHelper<RulesStatsHelper>.Instance };

			protected Type m_ClassType;

			protected Enum<RuleTypes> m_Type;

			private GameID m_EffectPackageGameID;

			private Enum<SkillTypes> m_SkillType;

			public Enum<RuleTypes> Type
			{
				get
				{
					return m_Type;
				}
			}

			public Enum<SkillTypes> SkillType
			{
				get
				{
					return m_SkillType;
				}
			}

			public GameID EffectPackageGameID
			{
				get
				{
					return m_EffectPackageGameID;
				}
				protected set
				{
					m_EffectPackageGameID = value;
				}
			}

			public Type ClassType
			{
				get
				{
					return m_ClassType;
				}
			}

			public override Type InstanceType
			{
				get
				{
					return m_ClassType;
				}
			}

			public RuleDefinition(AssetBundleInfo _assetBundleInfo)
				: base(StatsHelpers, _assetBundleInfo, true)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				EffectPackageGameID = SerializeGameID("EffectPackageGameID", _s, this, EffectPackageGameID);
				m_ClassType = _s.SerializeType("ClassType", m_ClassType);
				if (_s.Mode == EMode.EMode_Read)
				{
					PostConstructor();
				}
			}

			public override void InitializePass1()
			{
				base.InitializePass1();
				InitializeMutables();
			}

			public void SetSkillType(Enum<SkillTypes> _skillType)
			{
				m_SkillType = _skillType;
			}

			protected void PostConstructor()
			{
				m_Type = Enum<RuleTypes>.GetOrAdd(base.Name);
				if (UbiCore.Utils.Singleton<RuleData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<RuleData>.Instance.AddDefinition(this);
				}
			}
		}

		private readonly BBList<RuleDefinition> m_Definitions = new BBList<RuleDefinition>();

		public IRuleDefinition GetDefinition(Enum<RuleTypes> _type)
		{
			if (!_type.IsValid)
			{
				return null;
			}
			int num = (int)_type;
			if (num >= 0 && num < m_Definitions.Count)
			{
				return m_Definitions[num];
			}
			return null;
		}

		public IRuleDefinition GetDefinition(string _guidString)
		{
			GameID gameID = Main.SafeGameDb.FindGameIdFromGuid(_guidString);
			return Main.SafeGameDb.GetItem<DBEntry>(gameID) as IRuleDefinition;
		}

		private void AddDefinition(RuleDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				int index = _definition.Type.Index;
				while (m_Definitions.Count <= index)
				{
					m_Definitions.Add(null);
				}
				m_Definitions[(int)_definition.Type] = _definition;
			}
		}
	}
}
