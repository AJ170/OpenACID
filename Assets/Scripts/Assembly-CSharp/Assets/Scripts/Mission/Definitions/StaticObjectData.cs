using System;
using System.Collections.ObjectModel;
using AssetBundleData;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class StaticObjectData : UbiCore.Utils.Singleton<StaticObjectData>, IDataManager<StaticObjectData.ObjectType, StaticObjectData.IStaticObjectDefinition>
	{
		public interface IStaticObjectDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IReadonlyStatValueCollection
		{
			GameID StaticObjectGameID { get; }

			ObjectType Type { get; }
		}

		public class ObjectType : DataType<ObjectType, IStaticObjectDefinition>
		{
			protected override IDataManager<ObjectType, IStaticObjectDefinition> DataManager
			{
				get
				{
					return UbiCore.Utils.Singleton<StaticObjectData>.Instance;
				}
			}

			public ObjectType(GameID _gameID)
				: base(_gameID)
			{
			}
		}

		[Database("StaticObjectDefinition")]
		public class StaticObjectDefinition : EntityData.EntityDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IStaticObjectDefinition, IReadonlyStatValueCollection
		{
			protected EntityTypes.EType m_EntityType;

			public override Type InstanceType
			{
				get
				{
					return typeof(StaticObject);
				}
			}

			public GameID StaticObjectGameID { get; protected set; }

			public ObjectType Type { get; private set; }

			public override EntityTypes.EType EntityType
			{
				get
				{
					return m_EntityType;
				}
			}

			public StaticObjectDefinition(AssetBundleInfo _assetBundleInfo)
				: base(StaticObjectStats.StatsHelpers, _assetBundleInfo)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				m_EntityType = (EntityTypes.EType)_s.SerializeEnum("EntityType", m_EntityType.GetType(), (int)m_EntityType);
				StaticObjectGameID = SerializeGameID("StaticObjectGameID", _s, this, StaticObjectGameID);
				if (_s.Mode == EMode.EMode_Read)
				{
					PostConstructor();
				}
			}

			protected void PostConstructor()
			{
				Type = new ObjectType(base.GameID);
				if (UbiCore.Utils.Singleton<StaticObjectData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<StaticObjectData>.Instance.AddDefinition(this);
				}
			}
		}

		private readonly BBDictionary<GameID, StaticObjectDefinition> m_DefinitionsByGameID = new BBDictionary<GameID, StaticObjectDefinition>();

		private readonly BBDictionary<string, StaticObjectDefinition> m_DefinitionsByName = new BBDictionary<string, StaticObjectDefinition>();

		private readonly ReadOnlyCollection<ObjectType> m_TypesAccess;

		private readonly BBList<ObjectType> m_Types = new BBList<ObjectType>();

		public ReadOnlyCollection<ObjectType> Types
		{
			get
			{
				return m_TypesAccess;
			}
		}

		public StaticObjectData()
		{
			m_TypesAccess = new ReadOnlyCollection<ObjectType>(m_Types);
		}

		public IStaticObjectDefinition GetDefinition(ObjectType _type)
		{
			StaticObjectDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByGameID.TryGetValue((GameID)_type, out value)))
			{
				return value;
			}
			return null;
		}

		public IStaticObjectDefinition GetDefinitionByName(string _name)
		{
			StaticObjectDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByName.TryGetValue(_name, out value)))
			{
				return value;
			}
			return null;
		}

		public bool IsValid(ObjectType _type)
		{
			return m_DefinitionsByGameID.ContainsKey((GameID)_type);
		}

		private void AddDefinition(StaticObjectDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_DefinitionsByGameID.Add(_definition.GameID, _definition);
				m_DefinitionsByName.Add(_definition.Name, _definition);
				m_Types.Add(_definition.Type);
			}
		}
	}
}
