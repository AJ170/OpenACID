using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("AchievementDefinition")]
	public class AchievementDefinition : DBEntry
	{
		public LocaKey NameLocaKey { get; private set; }

		public LocaKey DescriptionLocaKey { get; private set; }

		public UnityResource Icon { get; protected set; }

		public int UiPriority { get; protected set; }

		public string GameCenterId { get; protected set; }

		public string GooglePlayIdDevelopment { get; protected set; }

		public string GooglePlayIdShipping { get; protected set; }

		public AchievementDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			NameLocaKey = new LocaKey();
			DescriptionLocaKey = new LocaKey();
			Icon = new UnityResource(_assetBundleInfo);
			UiPriority = 1000;
		}

		public override void ClearCache()
		{
			Icon.ClearCache();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
			DescriptionLocaKey.Key = _s.SerializeInt("DescriptionLocaKey", DescriptionLocaKey.Key);
			Icon.Serialize("Icon", _s, base.AssetBundleInfo);
			UiPriority = _s.SerializeInt("UiPriority", UiPriority);
			GameCenterId = _s.SerializeString("GameCenterId", GameCenterId);
			GooglePlayIdDevelopment = _s.SerializeString("GooglePlayIdDevelopment", GooglePlayIdDevelopment);
			GooglePlayIdShipping = _s.SerializeString("GooglePlayIdShipping", GooglePlayIdShipping);
		}
	}
}
