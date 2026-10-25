using UnityEngine;

namespace Database
{
	public static class ItemHelper
	{
		public const string c_WeaponTag = "Weapon";

		public static GameObject CreateItemOrEffectFromDB(GameID _gameID, string _nameAndTag = null)
		{
			DBEntry item = Main.Instance.GameDB.GetItem<DBEntry>(_gameID);
			if (item != null)
			{
				if (item.GetType() == typeof(ItemDef))
				{
					return CreateItem((ItemDef)item, _nameAndTag);
				}
				if (item.GetType() == typeof(EffectPackage))
				{
					return EffectHelper.SpawnEffectInWorld(_gameID);
				}
			}
			return null;
		}

		public static GameObject CreateItem(ItemDef _def, string _nameAndTag = null)
		{
			GameObject gameObject = Object.Instantiate(_def.ItemPrefab.AccessObject<Object>()) as GameObject;
			if (_def.ItemType == ItemDef.EItemType.Weapon)
			{
				SetupLOD(gameObject, 0.05f);
				gameObject.tag = "Weapon";
			}
			return gameObject;
		}

		public static void SetupLOD(GameObject _go, float _cullDistance)
		{
			Renderer[] componentsInChildren = _go.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				LODGroup lODGroup = renderer.gameObject.AddComponent<LODGroup>();
				LOD[] array2 = new LOD[1];
				array2[0].renderers = new Renderer[1] { renderer };
				array2[0].screenRelativeTransitionHeight = _cullDistance;
				lODGroup.SetLODS(array2);
			}
		}
	}
}
