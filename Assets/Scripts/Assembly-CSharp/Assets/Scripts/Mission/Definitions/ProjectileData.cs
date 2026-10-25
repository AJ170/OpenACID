using System;
using AssetBundleData;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public class ProjectileData : UbiCore.Utils.Singleton<ProjectileData>
	{
		public enum EProjectileType
		{
			Invalid = 0,
			Dagger = 1,
			SplinterBomb = 2,
			SmokeBomb = 3,
			StickyBomb = 4,
			TripwireBomb = 5,
			JackInTheBox = 6,
			FanOfKnivesKnive = 7,
			BowArrow = 8,
			Count = 9
		}

		public interface IProjectileDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IReadonlyStatValueCollection
		{
			EProjectileType Type { get; }

			GameID EffectGameID { get; }
		}

		[Database("ProjectileDefinition")]
		public class ProjectileDefinition : EntityData.EntityDefinition, Data.IDefinition, DataWithStats.IDefinitionWithStats, EntityData.IEntityDefinition, IProjectileDefinition, IReadonlyStatValueCollection
		{
			public override Type InstanceType
			{
				get
				{
					return typeof(Projectile);
				}
			}

			public EProjectileType Type { get; private set; }

			public GameID EffectGameID { get; private set; }

			public override EntityTypes.EType EntityType
			{
				get
				{
					return EntityTypes.EType.Projectile;
				}
			}

			public ProjectileDefinition(AssetBundleInfo _assetBundleInfo)
				: base(ProjectileStats.StatsHelpers, _assetBundleInfo)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				Type = (EProjectileType)_s.SerializeEnum("ProjectileType", Type.GetType(), (int)Type);
				EffectGameID = SerializeGameID("EffectgameID", _s, this, EffectGameID);
				if (_s.Mode == EMode.EMode_Read && UbiCore.Utils.Singleton<ProjectileData>.IsAvailable)
				{
					UbiCore.Utils.Singleton<ProjectileData>.Instance.AddDefinition(this);
				}
			}
		}

		private readonly ProjectileDefinition[] m_Definitions = new ProjectileDefinition[9];

		public IProjectileDefinition GetDefinition(EProjectileType _type)
		{
			if (_type == EProjectileType.Invalid)
			{
				return null;
			}
			return m_Definitions[(int)_type];
		}

		private void AddDefinition(ProjectileDefinition _definition)
		{
			if (UbiCore.Utils.Singleton<DefinitionManager>.Instance.Status == DefinitionManager.EStatus.Fresh)
			{
				m_Definitions[(int)_definition.Type] = _definition;
			}
		}
	}
}
