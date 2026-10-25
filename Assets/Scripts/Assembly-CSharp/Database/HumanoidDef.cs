using System.Collections.Generic;
using AcierProtocol;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("HumanoidDefinition")]
	public class HumanoidDef : DBEntry, IReadonlyHumanoidDef
	{
		public enum ECharType
		{
			Player = 0,
			Enemy = 1,
			Male = 2,
			Female = 3,
			Courtesan = 4,
			Doctor = 5,
			SpecialNPC = 6
		}

		public enum EAtlasPage
		{
			None = 0,
			Enemy = 1,
			Male = 2,
			Female = 3
		}

		public enum EArmorType
		{
			LightLeather = 0,
			Leather = 1,
			BoiledLeather = 2,
			StuddedLeather = 3,
			Scalemail = 4,
			Brigandine = 5,
			Chainmail = 6,
			Plate = 7
		}

		private uint m_WeaponFlags;

		public UnityResource Body { get; set; }

		public UnityResource BodyPortrait { get; set; }

		public UnityResource BodyPortraitMask { get; set; }

		public Vector2 BodyPortraitOrgSize { get; private set; }

		public UnityResource BodyItemImage { get; set; }

		public UnityResource BodyItemImageMask { get; set; }

		public Vector2 BodyItemImageOrgSize { get; private set; }

		public UnityResource BodyItemShopImage { get; set; }

		public UnityResource BodyItemShopImageMask { get; set; }

		public Vector2 BodyItemShopImageOrgSize { get; private set; }

		public Vector2 ShopItemOffset { get; set; }

		public float ShopImageScaleFactor { get; set; }

		public Vector4 ShopItemUVPosition { get; set; }

		public Vector4 BannerUVPosition { get; set; }

		public float BannerImageScaleFactor { get; set; }

		public Color BodyColor0 { get; set; }

		public Color BodyColor1 { get; set; }

		public GameID Head { get; set; }

		public GameID ColorTable { get; set; }

		public GameID AccessoryTable { get; set; }

		public bool RandomColor { get; set; }

		public bool MaySitOnBench { get; set; }

		public bool RandomHeadMaterial { get; set; }

		public uint WeaponFlags
		{
			get
			{
				return m_WeaponFlags;
			}
			set
			{
				m_WeaponFlags = value & 0xFFFFFDFFu;
			}
		}

		public ECharType CharType { get; set; }

		public EAtlasPage AtlasPage { get; set; }

		public EArmorType ArmorType { get; set; }

		public List<string> BodyAtlasMaterials { get; set; }

		public HumanoidDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			Body = new UnityResource(_assetBundleInfo);
			BodyPortrait = new UnityResource(_assetBundleInfo);
			BodyPortraitMask = new UnityResource(_assetBundleInfo);
			BodyItemImage = new UnityResource(_assetBundleInfo);
			BodyItemImageMask = new UnityResource(_assetBundleInfo);
			BodyItemShopImage = new UnityResource(_assetBundleInfo);
			BodyItemShopImageMask = new UnityResource(_assetBundleInfo);
			ShopItemOffset = new Vector2(0f, 0f);
			ShopImageScaleFactor = 1f;
			ShopItemUVPosition = new Vector4(0f, 0f, 1f, 1f);
			BannerUVPosition = new Vector4(0f, 0f, 1f, 1f);
			BannerImageScaleFactor = 1f;
		}

		public static int GenerateBitMask(ECharType _type)
		{
			int num = 0;
			switch (_type)
			{
			case ECharType.Player:
				num |= 8;
				break;
			case ECharType.Enemy:
				num |= 4;
				break;
			case ECharType.Male:
				num |= 1;
				break;
			case ECharType.Female:
				num |= 2;
				break;
			}
			return num;
		}

		public List<GameID> GetHeadGameIds()
		{
			List<GameID> list = new List<GameID>();
			if (!GameID.IsValid(Head))
			{
				List<DBEntry> list2 = new List<DBEntry>(Main.SafeGameDb.GetSubDB(typeof(HeadDef)).Data.Values);
				int num = GenerateBitMask(CharType);
				int count = list2.Count;
				for (int i = 0; count > i; i++)
				{
					HeadDef headDef = list2[i] as HeadDef;
					if (headDef != null && ((uint)headDef.BodyTypes & (uint)num) != 0)
					{
						list.Add(headDef.GameID);
					}
				}
			}
			else
			{
				list.Add(Head);
			}
			return list;
		}

		public HeadDef GetHeadDef()
		{
			List<GameID> headGameIds = GetHeadGameIds();
			if (headGameIds.Count > 0)
			{
				return Main.SafeGameDb.GetItem<HeadDef>(headGameIds[Random.Range(0, headGameIds.Count)]);
			}
			return null;
		}

		public ColorTableDef GetColorTableDef()
		{
			if (ColorTable != null)
			{
				return Main.SafeGameDb.GetItem<ColorTableDef>(ColorTable);
			}
			return null;
		}

		public AccessoryDef GetAccessoryDef()
		{
			if (AccessoryTable != null)
			{
				return Main.SafeGameDb.GetItem<AccessoryDef>(AccessoryTable);
			}
			return null;
		}

		public override void ClearCache()
		{
			Body.ClearCache();
			BodyPortrait.ClearCache();
			BodyPortraitMask.ClearCache();
			BodyItemImage.ClearCache();
			BodyItemImageMask.ClearCache();
			BodyItemShopImage.ClearCache();
			BodyItemShopImageMask.ClearCache();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			RandomColor = _s.SerializeBool("RandomColor", RandomColor);
			MaySitOnBench = _s.SerializeBool("MaySitOnBench", MaySitOnBench);
			RandomHeadMaterial = _s.SerializeBool("RandomHeadMaterial", RandomHeadMaterial);
			CharType = (ECharType)_s.SerializeEnum("CharType", CharType.GetType(), (int)CharType);
			AtlasPage = (EAtlasPage)_s.SerializeEnum("AtlasPage", AtlasPage.GetType(), (int)AtlasPage);
			ArmorType = (EArmorType)_s.SerializeEnum("ArmorType", ArmorType.GetType(), (int)ArmorType);
			WeaponFlags = _s.SerializeUInt32("WeaponFlags", WeaponFlags);
			Body.Serialize("Body", _s, base.AssetBundleInfo);
			BodyPortrait.Serialize("BodyPortrait", _s, base.AssetBundleInfo);
			BodyPortraitMask.Serialize("BodyPortraitMask", _s, base.AssetBundleInfo);
			BodyPortraitOrgSize = _s.SerializeVector2("BodyPortraitOrgSize", BodyPortraitOrgSize);
			BodyItemImage.Serialize("BodyItemImage", _s, base.AssetBundleInfo);
			BodyItemImageMask.Serialize("BodyItemImageMask", _s, base.AssetBundleInfo);
			BodyItemImageOrgSize = _s.SerializeVector2("BodyItemImageOrgSize", BodyItemImageOrgSize);
			BodyItemShopImage.Serialize("BodyItemShopImage", _s, base.AssetBundleInfo);
			BodyItemShopImageMask.Serialize("BodyItemShopImageMask", _s, base.AssetBundleInfo);
			BodyItemShopImageOrgSize = _s.SerializeVector2("BodyItemShopImageOrgSize", BodyItemShopImageOrgSize);
			ShopItemOffset = _s.SerializeVector2("ShopItemOffset", ShopItemOffset);
			ShopImageScaleFactor = _s.SerializeFloat("ShopImageScaleFactor", ShopImageScaleFactor);
			ShopItemUVPosition = _s.SerializeVector4("ShopItemUVPosition", ShopItemUVPosition);
			BannerUVPosition = _s.SerializeVector4("BannerUVPosition", BannerUVPosition);
			BannerImageScaleFactor = _s.SerializeFloat("Banner1x1ImageScaleFactor", BannerImageScaleFactor);
			BodyColor0 = _s.SerializeColor("BodyColor0", BodyColor0);
			BodyColor1 = _s.SerializeColor("BodyColor1", BodyColor1);
			Head = SerializeGameID("Head", _s, this, Head);
			ColorTable = SerializeGameID("ColorTable", _s, this, ColorTable);
			AccessoryTable = SerializeGameID("AccessoryTable", _s, this, AccessoryTable);
			BodyAtlasMaterials = _s.SerializeListGeneric("BodyAtlasMaterials", BodyAtlasMaterials);
		}

	}
}
