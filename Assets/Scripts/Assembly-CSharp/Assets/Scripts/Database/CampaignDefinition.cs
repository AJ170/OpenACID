using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Database
{
	[Database("CampaignDefinition")]
	public class CampaignDefinition : DBEntry
	{
		public LocaKey NameLocaKey { get; private set; }

		public LocaKey DescriptionLocaKey { get; private set; }

		public UnityResource Banner { get; protected set; }

		public GameID BackgroundGameId { get; protected set; }

		public LocaKey UnlockRewardLocaLine1 { get; private set; }

		public LocaKey UnlockRewardLocaLine2 { get; private set; }

		public LocaKey UnlockExtraRewardLocaLine1 { get; private set; }

		public LocaKey UnlockExtraRewardLocaLine2 { get; private set; }

		public CampaignDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			NameLocaKey = new LocaKey();
			DescriptionLocaKey = new LocaKey();
			Banner = new UnityResource(_assetBundleInfo);
			UnlockRewardLocaLine1 = new LocaKey();
			UnlockRewardLocaLine2 = new LocaKey();
			UnlockExtraRewardLocaLine1 = new LocaKey();
			UnlockExtraRewardLocaLine2 = new LocaKey();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			NameLocaKey.Key = _s.SerializeInt("NameLocaKey", NameLocaKey.Key);
			DescriptionLocaKey.Key = _s.SerializeInt("DescriptionLocaKey", DescriptionLocaKey.Key);
			Banner.Serialize("Banner", _s, base.AssetBundleInfo);
			BackgroundGameId = SerializeGameID("BackgroundGameId", _s, this, BackgroundGameId);
			UnlockRewardLocaLine1.Key = _s.SerializeInt("UnlockRewardLocaLine1", UnlockRewardLocaLine1.Key);
			UnlockRewardLocaLine2.Key = _s.SerializeInt("UnlockRewardLocaLine2", UnlockRewardLocaLine2.Key);
			UnlockExtraRewardLocaLine1.Key = _s.SerializeInt("UnlockExtraRewardLocaLine1", UnlockExtraRewardLocaLine1.Key);
			UnlockExtraRewardLocaLine2.Key = _s.SerializeInt("UnlockExtraRewardLocaLine2", UnlockExtraRewardLocaLine2.Key);
		}

		public override void ClearCache()
		{
			Banner.ClearCache();
		}
	}
}
