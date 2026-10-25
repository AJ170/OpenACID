using AssetBundleData;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Definitions
{
	public static class EntityData
	{
		public interface IEntityDefinition : Data.IDefinition, DataWithStats.IDefinitionWithStats, IReadonlyStatValueCollection
		{
			EntityTypes.EType EntityType { get; }

			bool SkipLogicUpdateAllowed { get; }
		}

		public abstract class EntityDefinition : DataWithStats.DefinitionWithStats, Data.IDefinition, DataWithStats.IDefinitionWithStats, IEntityDefinition, IReadonlyStatValueCollection
		{
			public abstract EntityTypes.EType EntityType { get; }

			public virtual bool SkipLogicUpdateAllowed
			{
				get
				{
					return false;
				}
			}

			protected EntityDefinition(IStatsHelper[] _statsHelpers, AssetBundleInfo _assetBundleInfo)
				: base(_statsHelpers, _assetBundleInfo, true)
			{
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
			}
		}
	}
}
