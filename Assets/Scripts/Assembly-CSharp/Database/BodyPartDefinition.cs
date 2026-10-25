using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("BodyPartDefinition")]
	public class BodyPartDefinition : DBEntry
	{
		public class BodyPartMaterials : DataSerializableBase
		{
			public UnityResource Material;

			public UnityResource Icon;

			public UnityResource Mask;

			public Vector2 IconOriginalSize;

			public BodyPartMaterials()
			{
				Material = new UnityResource();
				Icon = new UnityResource();
				Mask = new UnityResource();
			}

			public void ComputeSizesWrapped()
			{
			}

			protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
			{
				if (_s.Mode == EMode.EMode_Write)
				{
					ComputeSizesWrapped();
				}
				Material.Serialize("Material", _s, _customData);
				Icon.Serialize("Icon", _s, _customData);
				Mask.Serialize("Mask", _s, _customData);
				IconOriginalSize = _s.SerializeVector2("IconOrgSize", IconOriginalSize);
			}
		}

		public HumanoidDef.EAtlasPage AtlasPage;

		public UnityResource BodyPart;

		public int BaseMaterial;

		public List<BodyPartMaterials> MaterialVariations;

		public BodyPartDefinition(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			BodyPart = new UnityResource();
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			AtlasPage = (HumanoidDef.EAtlasPage)_s.SerializeEnum("AtlasPage", AtlasPage.GetType(), (int)AtlasPage);
			BodyPart.Serialize("BodyPart", _s, base.AssetBundleInfo);
			BaseMaterial = _s.SerializeInt("BaseMaterial", BaseMaterial);
			MaterialVariations = _s.SerializeList("MaterialVariations", MaterialVariations, base.AssetBundleInfo);
		}

		public override void ClearCache()
		{
			BodyPart.ClearCache();
			int count = MaterialVariations.Count;
			for (int i = 0; count > i; i++)
			{
				BodyPartMaterials bodyPartMaterials = MaterialVariations[i];
				bodyPartMaterials.Material.ClearCache();
				bodyPartMaterials.Icon.ClearCache();
			}
		}
	}
}
