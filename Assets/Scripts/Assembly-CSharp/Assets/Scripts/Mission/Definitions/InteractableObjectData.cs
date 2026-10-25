using System;
using System.Collections.ObjectModel;
using AcierProtocol;
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
	public class InteractableObjectData : UbiCore.Utils.Singleton<InteractableObjectData>, IDataManager<InteractableObjectData.ObjectType, InteractableObjectData.IInteractableObjectDefinition>
	{
		public class ObjectType : DataType<ObjectType, IInteractableObjectDefinition>
		{
			protected override IDataManager<ObjectType, IInteractableObjectDefinition> DataManager
			{
				get
				{
					return UbiCore.Utils.Singleton<InteractableObjectData>.Instance;
				}
			}

			public ObjectType(GameID _gameID)
				: base(_gameID)
			{
			}
		}

		public interface IInteractableObjectDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, MapGeneratorData.IMapGeneratorEntityDefinition, IReadonlyStatValueCollection
		{
			ObjectType Type { get; }

			EObjectCategory Category { get; }

			EObjectSubCategory SubCategory { get; }

			GameID ResourceGameID { get; }

			float InteractionDistance { get; }

			bool InteractionCanBeAborted { get; }

			bool InteractionIsSafe { get; }
		}

		[Database("InteractableObjectDefinition")]
		public class InteractableObjectDefinition : EntityData.EntityDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IInteractableObjectDefinition, MapGeneratorData.IMapGeneratorEntityDefinition, MapGeneratorData.IMapGeneratorEntityDefinitionAccess, IReadonlyStatValueCollection
		{
			protected EntityTypes.EType m_EntityType;

			public int MapWeight { get; set; }

			public int UnlockLevel { get; set; }

			public override EntityTypes.EType EntityType
			{
				get
				{
					return m_EntityType;
				}
			}

			public override Type InstanceType
			{
				get
				{
					return typeof(InteractableObject);
				}
			}

			public ObjectType Type { get; private set; }

			public EObjectCategory Category { get; protected set; }

			public EObjectSubCategory SubCategory { get; protected set; }

			public GameID ResourceGameID { get; protected set; }

			public float InteractionDistance { get; protected set; }

			public override bool SkipLogicUpdateAllowed
			{
				get
				{
					return true;
				}
			}

			public bool InteractionCanBeAborted
			{
				get
				{
					return Category != EObjectCategory.Codex_EObjectCategory && SubCategory != EObjectSubCategory.PigeonCoup;
				}
			}

			public bool InteractionIsSafe
			{
				get
				{
					return !InteractionCanBeAborted;
				}
			}

			public InteractableObjectDefinition(AssetBundleInfo _assetBundleInfo)
				: base(InteractableObjectStats.StatsHelpers, _assetBundleInfo)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				Category = (EObjectCategory)_s.SerializeEnum("Category", Category.GetType(), (int)Category);
				SubCategory = (EObjectSubCategory)_s.SerializeEnum("SubCategory", SubCategory.GetType(), (int)SubCategory);
				m_EntityType = (EntityTypes.EType)_s.SerializeEnum("EntityType", m_EntityType.GetType(), (int)m_EntityType);
				ResourceGameID = SerializeGameID("ResourceGameID", _s, this, ResourceGameID);
				InteractionDistance = _s.SerializeFloat("InteractionDistance", InteractionDistance);
				if (_s.Mode == EMode.EMode_Read)
				{
					PostConstructor();
				}
			}

			protected void PostConstructor()
			{
				Type = new ObjectType(base.GameID);
				MapGeneratorData.FillFromStats(this, m_StatValueCollection);
				if (UbiCore.Utils.Singleton<InteractableObjectData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<InteractableObjectData>.Instance.AddDefinition(this);
				}
			}

			virtual EEra MapGeneratorData.IMapGeneratorEntityDefinition.get_Age()
			{
				return base.Age;
			}

			virtual EEra MapGeneratorData.IMapGeneratorEntityDefinitionAccess.get_Age()
			{
				return base.Age;
			}

			virtual void MapGeneratorData.IMapGeneratorEntityDefinitionAccess.set_Age(EEra value)
			{
				base.Age = value;
			}
		}

		private readonly BBDictionary<GameID, InteractableObjectDefinition> m_DefinitionsByGameID = new BBDictionary<GameID, InteractableObjectDefinition>();

		private readonly BBDictionary<string, InteractableObjectDefinition> m_DefinitionsByName = new BBDictionary<string, InteractableObjectDefinition>();

		private readonly BBList<ObjectType> m_Types = new BBList<ObjectType>();

		private readonly ReadOnlyCollection<ObjectType> m_TypesAccess;

		public ReadOnlyCollection<ObjectType> Types
		{
			get
			{
				return m_TypesAccess;
			}
		}

		public InteractableObjectData()
		{
			m_TypesAccess = new ReadOnlyCollection<ObjectType>(m_Types);
		}

		private void AddDefinition(InteractableObjectDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_DefinitionsByGameID.Add(_definition.GameID, _definition);
				m_DefinitionsByName.Add(_definition.Name, _definition);
				m_Types.Add(_definition.Type);
			}
		}

		public IInteractableObjectDefinition GetDefinition(ObjectType _type)
		{
			InteractableObjectDefinition value;
			if (Assets.Scripts.Engine.Helpers.VerifyOrExit(m_DefinitionsByGameID.TryGetValue((GameID)_type, out value)))
			{
				return value;
			}
			return null;
		}

		public IInteractableObjectDefinition GetDefinitionByName(string _name)
		{
			InteractableObjectDefinition value;
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
	}
}
