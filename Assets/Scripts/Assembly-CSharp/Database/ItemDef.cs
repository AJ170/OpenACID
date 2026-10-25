using System;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("ItemDefinition")]
	public class ItemDef : DBEntry
	{
		public enum EItemType
		{
			Weapon = 0,
			Boots = 1,
			Shoulder = 2,
			Chest = 3,
			HiddenBlade = 4,
			Legs = 5,
			Insignia = 6,
			HC_Packages = 7,
			Premium = 8,
			Dye = 9,
			Decryption = 10,
			Shop_Categories = 11,
			Interactable = 12,
			Prop = 13,
			Consumables = 14,
			Social = 15,
			Advertisement = 16,
			LootPreviewIcon = 17,
			Entitlement = 18,
			Heroic_Shop = 19,
			Forge = 20,
			Heroic_Portrait = 21,
			CallToAction = 22
		}

		[Flags]
		public enum EWeaponTypes
		{
			Dagger = 1,
			Bow = 2,
			Crossbow = 4,
			Sword = 8,
			TwoHand = 0x10,
			Quiver = 0x20,
			Arrow = 0x40,
			Bolt = 0x80,
			Polearm = 0x100,
			NoWeapon = 0x200
		}

		public EItemType ItemType { get; set; }

		public UnityResource ItemPrefab { get; set; }

		public EWeaponTypes WeaponType { get; set; }

		public float ItemImageScaleFactor { get; set; }

		public Vector2 ItemOffset { get; set; }

		public Vector4 ItemUVPosition { get; set; }

		public float CtaImageScaleFactor { get; set; }

		public Vector2 CtaOffset { get; set; }

		public Vector4 CtaUVPosition { get; set; }

		public float BannerImageScaleFactor { get; set; }

		public Vector4 BannerUVPosition { get; set; }

		public bool BannerUseAlphaMask { get; set; }

		public Vector2 AlphaMaskOffset { get; set; }

		public UnityResource ItemImage { get; set; }

		public UnityResource ItemImageMask { get; set; }

		public UnityResource ItemPortraitImage { get; set; }

		public Vector2 ItemImageOrgSize { get; private set; }

		public bool UseDarkOldPriceColor { get; set; }

		public bool UseDarkPriceBackground { get; set; }

		public GameID BannerBackground1x1 { get; set; }

		public ItemDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			ItemPrefab = new UnityResource(_assetBundleInfo);
			ItemImage = new UnityResource(_assetBundleInfo);
			ItemImageMask = new UnityResource(_assetBundleInfo);
			ItemPortraitImage = new UnityResource(_assetBundleInfo);
			ItemImageScaleFactor = 1f;
			ItemOffset = new Vector2(0f, 0f);
			ItemUVPosition = new Vector4(0f, 0f, 1f, 1f);
			CtaImageScaleFactor = 1f;
			CtaOffset = new Vector2(0f, 0f);
			CtaUVPosition = new Vector4(0f, 0f, 1f, 1f);
			BannerUVPosition = new Vector4(0f, 0f, 1f, 1f);
			BannerUseAlphaMask = false;
			AlphaMaskOffset = new Vector2(0f, 0f);
			UseDarkOldPriceColor = false;
			UseDarkPriceBackground = false;
			BannerImageScaleFactor = 1f;
			BannerBackground1x1 = GameID.Invalid;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			ItemType = (EItemType)_s.SerializeEnum("ItemType", ItemType.GetType(), (int)ItemType);
			ItemPrefab.Serialize("ItemPrefab", _s, base.AssetBundleInfo);
			ItemImage.Serialize("ItemImage", _s, base.AssetBundleInfo);
			ItemImageMask.Serialize("ItemImageMask", _s, base.AssetBundleInfo);
			ItemPortraitImage.Serialize("ItemPortraitImage", _s, base.AssetBundleInfo);
			WeaponType = (EWeaponTypes)_s.SerializeEnum("WeaponType", WeaponType.GetType(), (int)WeaponType);
			ItemImageOrgSize = _s.SerializeVector2("ItemImageOrgSize", ItemImageOrgSize);
			ItemImageScaleFactor = _s.SerializeFloat("ItemImageScaleFactor", ItemImageScaleFactor);
			ItemOffset = _s.SerializeVector2("ItemOffset", ItemOffset);
			ItemUVPosition = _s.SerializeVector4("ItemUVPosition", ItemUVPosition);
			CtaImageScaleFactor = _s.SerializeFloat("CtaImageScaleFactor", CtaImageScaleFactor);
			CtaOffset = _s.SerializeVector2("CtaOffset", CtaOffset);
			CtaUVPosition = _s.SerializeVector4("CtaUVPosition", CtaUVPosition);
			UseDarkPriceBackground = _s.SerializeBool("UseDarkPriceBackground", UseDarkPriceBackground);
			UseDarkOldPriceColor = _s.SerializeBool("UseDarkOldPriceColor", UseDarkOldPriceColor);
			BannerUVPosition = _s.SerializeVector4("BannerUVPosition", BannerUVPosition);
			BannerImageScaleFactor = _s.SerializeFloat("Banner1x1ImageScaleFactor", BannerImageScaleFactor);
			BannerUseAlphaMask = _s.SerializeBool("BannerUseAlphaMask", BannerUseAlphaMask);
			AlphaMaskOffset = _s.SerializeVector2("AlphaMaskOffset", AlphaMaskOffset);
			BannerBackground1x1 = SerializeGameID("BannerBackground1x1", _s, this, BannerBackground1x1);
		}

		public override void ClearCache()
		{
			ItemPrefab.ClearCache();
			ItemImage.ClearCache();
			ItemImageMask.ClearCache();
			ItemPortraitImage.ClearCache();
		}
	}
}
