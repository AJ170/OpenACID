using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.Database
{
	[Database("CodexDefinition")]
	public class CodexDefinition : DBEntry
	{
		public enum EEntryType
		{
			Characters = 0,
			Locations = 1,
			Story = 2,
			History = 3,
			EraHeader = 4,
			CategoryHeader = 5
		}

		public enum EStructureType
		{
			Entry = 0,
			CategoryOverview = 1,
			EraOverview = 2,
			StartingPage = 3
		}

		public enum ELayout
		{
			Banner = 0,
			LeftPicture = 1,
			Landscape = 2,
			Landscape_cutout = 3,
			Portrait = 4,
			Plain_Text = 5
		}

		public UnityResource Thumbnail { get; set; }

		public LocaKey HeaderLocaKey { get; set; }

		public LocaKey BodyLocaKey { get; set; }

		public int SortId { get; set; }

		public EEntryType EntryType { get; set; }

		public ELayout Layout { get; set; }

		public EStructureType StructureType { get; set; }

		public UnityResource AtlasTexture { get; set; }

		public Rect AtlasPosition { get; set; }

		public GameID ReqEntitlementID { get; set; }

		public CodexDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			Thumbnail = new UnityResource(_assetBundleInfo);
			SortId = 0;
			HeaderLocaKey = new LocaKey();
			BodyLocaKey = new LocaKey();
			AtlasTexture = new UnityResource(_assetBundleInfo);
			AtlasPosition = default(Rect);
			ReqEntitlementID = GameID.Invalid;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			UnityResource unityResource = new UnityResource(base.AssetBundleInfo);
			unityResource.Serialize("Picture", _s, base.AssetBundleInfo);
			Thumbnail.Serialize("Thumbail", _s, base.AssetBundleInfo);
			SortId = _s.SerializeInt("Order", SortId);
			HeaderLocaKey.Key = _s.SerializeInt("HeaderLocaKey", HeaderLocaKey.Key);
			BodyLocaKey.Key = _s.SerializeInt("BodyLocaKey", BodyLocaKey.Key);
			EntryType = (EEntryType)_s.SerializeEnum("EntryType", EntryType.GetType(), (int)EntryType);
			Layout = (ELayout)_s.SerializeEnum("Layout", Layout.GetType(), (int)Layout);
			StructureType = (EStructureType)_s.SerializeEnum("StructureType", StructureType.GetType(), (int)StructureType);
			AtlasTexture.Serialize("AtlasTexture", _s, base.AssetBundleInfo);
			AtlasPosition = _s.SerializeRect("AtlasPosition", AtlasPosition);
			ReqEntitlementID = SerializeGameID("ReqEntitlementID", _s, this, ReqEntitlementID);
		}

		public override void ClearCache()
		{
			AtlasTexture.ClearCache();
			Thumbnail.ClearCache();
		}
	}
}
