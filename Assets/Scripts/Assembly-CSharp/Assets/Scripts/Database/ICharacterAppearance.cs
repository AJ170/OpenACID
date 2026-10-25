using Database;
using UnityEngine;

namespace Assets.Scripts.Database
{
	public interface ICharacterAppearance : IReadonlyCharacterAppearance
	{
		new HeadDef HeadDefinition { get; set; }

		new HeadDef.HeadMaterials HeadMaterials { get; set; }

		new UnityResource HeadAccessoireResource { get; set; }

		new Color Color0 { get; set; }

		new Color Color1 { get; set; }
	}
}
