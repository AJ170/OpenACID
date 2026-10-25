using Database;
using UnityEngine;

namespace Assets.Scripts.Database
{
	public interface IReadonlyCharacterAppearance
	{
		HeadDef HeadDefinition { get; }

		HeadDef.HeadMaterials HeadMaterials { get; }

		UnityResource HeadAccessoireResource { get; }

		Color Color0 { get; }

		Color Color1 { get; }
	}
}
