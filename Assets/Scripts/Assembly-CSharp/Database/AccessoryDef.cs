using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("AccessoryDefinition")]
	public class AccessoryDef : DBEntry
	{
		public class Accessory : DataSerializableBase
		{
			public UnityResource Item;

			public Accessory()
			{
				Item = new UnityResource();
			}

			protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
			{
				Item.Serialize("Item", _s, _customData);
			}
		}

		public float NoAccessoryChance;

		public HumanoidDef.EAtlasPage AtlasPage;

		public List<Accessory> Accessories;

		public List<string> AccessoryMaterials;

		public AccessoryDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public UnityResource GetRandomHeadAccessoire()
		{
			if (Accessories != null && Random.Range(0f, 1f) > NoAccessoryChance)
			{
				return Accessories[Random.Range(0, Accessories.Count)].Item;
			}
			return null;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			AtlasPage = (HumanoidDef.EAtlasPage)_s.SerializeEnum("Atlas", AtlasPage.GetType(), (int)AtlasPage);
			NoAccessoryChance = _s.SerializeFloat("NoAccessoryChance", NoAccessoryChance);
			Accessories = _s.SerializeList("Accessories", Accessories, base.AssetBundleInfo);
			AccessoryMaterials = _s.SerializeListGeneric("AccessoryMaterials", AccessoryMaterials);
		}

		public override void ClearCache()
		{
			foreach (Accessory accessory in Accessories)
			{
				accessory.Item.ClearCache();
			}
		}
	}
}
