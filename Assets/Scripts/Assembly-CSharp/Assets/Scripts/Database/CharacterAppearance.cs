using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using Humanoids;
using UnityEngine;

namespace Assets.Scripts.Database
{
	public class CharacterAppearance : IReadonlyCharacterAppearance, ICharacterAppearance
	{
		public HeadDef HeadDefinition { get; set; }

		public HeadDef.HeadMaterials HeadMaterials { get; set; }

		public UnityResource HeadAccessoireResource { get; set; }

		public Color Color0 { get; set; }

		public Color Color1 { get; set; }

		public CharacterAppearance()
		{
		}

		public CharacterAppearance(HumanoidDef _humanoid, HeadDef _head)
		{
			HeadDefinition = _head;
			HeadMaterials = HeadDefinition.GetHeadMaterial(false);
			ColorTableDef item = Main.SafeGameDb.GetItem<ColorTableDef>(_humanoid.ColorTable);
			ColorDef item2 = Main.SafeGameDb.GetItem<ColorDef>(item.Colors[0]);
			Color0 = item2.Color0;
			Color1 = item2.Color1;
		}

		public CharacterAppearance(Assassin _assassin)
		{
			HeadDefinition = Main.Instance.GameDB.GetItem<HeadDef>(Main.Instance.GameDB.FindGameIdFromGuid(_assassin.headGuid));
			HeadMaterials = HeadDefinition.GetHeadMaterial(false);
			EquippedElement[] equippedElements = _assassin.equippedElements;
			foreach (EquippedElement equippedElement in equippedElements)
			{
				InventoryElement element = equippedElement.element;
				if (!element.IsItem())
				{
					continue;
				}
				ItemData itemData = element.itemData;
				GameID _gameId;
				if (itemData.category == Item.EItemCategory.Insignia && !string.IsNullOrEmpty(element.guid) && Main.Instance.GameDB.TryFindGameIdFromGuid(element.guid, out _gameId))
				{
					ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(_gameId);
					if (!string.IsNullOrEmpty(item.ItemPrefab.GetResourcePath()))
					{
						HeadAccessoireResource = item.ItemPrefab;
						break;
					}
				}
			}
			ColorDef item2 = Main.SafeGameDb.GetItem<ColorDef>(Main.SafeGameDb.FindGameIdFromGuid(_assassin.colorSetting));
			if (item2 == null)
			{
				HumanoidDef item3 = Main.Instance.GameDB.GetItem<HumanoidDef>(Main.Instance.GameDB.FindGameIdFromGuid(_assassin.humanoidGuid));
				ColorTableDef item4 = Main.SafeGameDb.GetItem<ColorTableDef>(item3.ColorTable);
				item2 = Main.SafeGameDb.GetItem<ColorDef>(item4.Colors[0]);
			}
			Color0 = item2.Color0;
			Color1 = item2.Color1;
		}

		public CharacterAppearance(AvatarData.IAvatarDefinition _definition)
		{
			if (Helpers.Verify(GameID.IsValid(_definition.HeadGameID), "Assassin character without HEAD GUID!"))
			{
				HeadDefinition = Main.SafeGameDb.GetItem<HeadDef>(_definition.HeadGameID);
				HeadMaterials = HeadDefinition.GetHeadMaterial(false);
				bool flag = false;
				if (GameID.IsValid(_definition.ColorSet))
				{
					ColorDef item = Main.SafeGameDb.GetItem<ColorDef>(_definition.ColorSet);
					if (item != null)
					{
						Color0 = item.Color0;
						Color1 = item.Color1;
						flag = true;
					}
				}
				if (!flag)
				{
					HumanoidDef item2 = Main.SafeGameDb.GetItem<HumanoidDef>(_definition.HumanoidGameID);
					ColorTableDef item3 = Main.SafeGameDb.GetItem<ColorTableDef>(item2.ColorTable);
					ColorDef item4 = Main.SafeGameDb.GetItem<ColorDef>(item3.Colors[0]);
					Color0 = item4.Color0;
					Color1 = item4.Color1;
				}
			}
			else
			{
				HumanoidDef item5 = Main.SafeGameDb.GetItem<HumanoidDef>(_definition.HumanoidGameID);
				Singleton<HumanoidFactory>.Instance.RandomizeHumanoid(item5, this, true);
			}
			if (GameID.IsValid(_definition.HeadAccessoireGameID))
			{
				ItemDef item6 = Main.SafeGameDb.GetItem<ItemDef>(_definition.HeadAccessoireGameID);
				if (!string.IsNullOrEmpty(item6.ItemPrefab.GetResourcePath()))
				{
					HeadAccessoireResource = item6.ItemPrefab;
				}
			}
		}
	}
}
