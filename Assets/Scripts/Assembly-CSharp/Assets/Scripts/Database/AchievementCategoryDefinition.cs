using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("AchievementCategoryDefinition")]
	public class AchievementCategoryDefinition : DBEntry
	{
		public LocaKey NameLocaKey { get; private set; }

		public int UiPriority { get; protected set; }

		public AchievementCategoryDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			NameLocaKey = new LocaKey();
			UiPriority = 1000;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
			UiPriority = _s.SerializeInt("UiPriority", UiPriority);
		}
	}
}
