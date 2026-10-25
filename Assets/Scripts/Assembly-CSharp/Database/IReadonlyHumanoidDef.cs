using System.Collections.Generic;
using AcierProtocol;
using UnityEngine;

namespace Database
{
	public interface IReadonlyHumanoidDef
	{
		UnityResource Body { get; }

		UnityResource BodyPortrait { get; }

		UnityResource BodyPortraitMask { get; }

		Vector2 BodyPortraitOrgSize { get; }

		UnityResource BodyItemImage { get; }

		UnityResource BodyItemImageMask { get; }

		Vector2 BodyItemImageOrgSize { get; }

		UnityResource BodyItemShopImage { get; }

		UnityResource BodyItemShopImageMask { get; }

		Vector2 BodyItemShopImageOrgSize { get; }

		Color BodyColor0 { get; }

		Color BodyColor1 { get; }

		GameID Head { get; }

		GameID ColorTable { get; }

		GameID AccessoryTable { get; }

		bool RandomColor { get; }

		bool MaySitOnBench { get; }

		bool RandomHeadMaterial { get; }

		uint WeaponFlags { get; }

		HumanoidDef.ECharType CharType { get; }

		HumanoidDef.EAtlasPage AtlasPage { get; }

		EEra Age { get; }

		List<string> BodyAtlasMaterials { get; }
	}
}
