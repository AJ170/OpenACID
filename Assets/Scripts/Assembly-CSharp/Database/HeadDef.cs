using System;
using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("HeadDefinition")]
	public class HeadDef : DBEntry
	{
		[Flags]
		public enum EBodyTypes
		{
			Male = 1,
			Female = 2,
			Enemy = 4,
			Assassin = 8
		}

		public class HeadMaterials : DataSerializableBase
		{
			public UnityResource Material;

			public UnityResource Portrait;

			public HeadMaterials()
			{
				Material = new UnityResource();
				Portrait = new UnityResource();
			}

			protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
			{
				Material.Serialize("Material", _s, _customData);
				Portrait.Serialize("Portrait", _s, _customData);
			}
		}

		public EBodyTypes BodyTypes;

		public UnityResource BaseHead;

		public int BaseMaterial;

		public List<HeadMaterials> MaterialVariations;

		public HeadDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			BaseHead = new UnityResource(_assetBundleInfo);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			BodyTypes = (EBodyTypes)_s.SerializeEnum("BodyTypes", BodyTypes.GetType(), (int)BodyTypes);
			BaseHead.Serialize("BaseHead", _s, base.AssetBundleInfo);
			BaseMaterial = _s.SerializeInt("BaseMaterial", BaseMaterial);
			MaterialVariations = _s.SerializeList("MaterialVariations", MaterialVariations, base.AssetBundleInfo);
		}

		public HeadMaterials GetHeadMaterial(bool _random)
		{
			if (_random)
			{
				return MaterialVariations[UnityEngine.Random.Range(0, MaterialVariations.Count)];
			}
			return MaterialVariations[(BaseMaterial > -1 && BaseMaterial < MaterialVariations.Count) ? BaseMaterial : 0];
		}

		public override void ClearCache()
		{
			BaseHead.ClearCache();
			foreach (HeadMaterials materialVariation in MaterialVariations)
			{
				materialVariation.Material.ClearCache();
				materialVariation.Portrait.ClearCache();
			}
		}
	}
}
