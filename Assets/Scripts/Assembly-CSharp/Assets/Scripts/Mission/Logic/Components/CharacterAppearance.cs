using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Database;
using Humanoids;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class CharacterAppearance : Component, IReadonlyCharacterAppearance, ICharacterAppearance
	{
		public HeadDef HeadDefinition { get; set; }

		public HeadDef.HeadMaterials HeadMaterials { get; set; }

		public UnityResource HeadAccessoireResource { get; set; }

		public Color Color0 { get; set; }

		public Color Color1 { get; set; }

		public override int Priority
		{
			get
			{
				return 25;
			}
		}

		public CharacterAppearance(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			NpcCharacter npcCharacter = Entity as NpcCharacter;
			if (npcCharacter != null)
			{
				NpcData.INpcDefinition definition = npcCharacter.NpcDefinition.Definition;
				HumanoidDef item = Main.Instance.GameDB.GetItem<HumanoidDef>(definition.HumanoidGameID);
				Singleton<HumanoidFactory>.Instance.RandomizeHumanoid(item, this, false);
				ColorDef item2 = Main.Instance.GameDB.GetItem<ColorDef>(definition.ColorSet);
				if (item2 != null)
				{
					Color0 = item2.Color0;
					Color1 = item2.Color1;
				}
				return;
			}
			AssassinCharacter assassinCharacter = Entity as AssassinCharacter;
			if (assassinCharacter == null)
			{
				return;
			}
			AvatarData.AvatarDefinition definition2 = assassinCharacter.AvatarDefinition.Definition;
			if (Assets.Scripts.Engine.Helpers.Verify(GameID.IsValid(assassinCharacter.AvatarDefinition.Definition.HeadGameID), "Player character without HEAD GUID!"))
			{
				HeadDefinition = Main.SafeGameDb.GetItem<HeadDef>(assassinCharacter.AvatarDefinition.Definition.HeadGameID);
				HeadMaterials = HeadDefinition.GetHeadMaterial(false);
				if (!GameID.IsValid(definition2.ColorSet))
				{
					HumanoidDef item3 = Main.SafeGameDb.GetItem<HumanoidDef>(definition2.HumanoidGameID);
					ColorTableDef item4 = Main.SafeGameDb.GetItem<ColorTableDef>(item3.ColorTable);
					ColorDef item5 = Main.SafeGameDb.GetItem<ColorDef>(item4.Colors[0]);
					Color0 = item5.Color0;
					Color1 = item5.Color1;
				}
				else
				{
					ColorDef item6 = Main.Instance.GameDB.GetItem<ColorDef>(definition2.ColorSet);
					if (item6 != null)
					{
						Color0 = item6.Color0;
						Color1 = item6.Color1;
					}
				}
			}
			else
			{
				HumanoidDef item7 = Main.SafeGameDb.GetItem<HumanoidDef>(assassinCharacter.CharacterDefinition.Definition.HumanoidGameID);
				Singleton<HumanoidFactory>.Instance.RandomizeHumanoid(item7, this, true);
			}
			if (GameID.IsValid(definition2.HeadAccessoireGameID))
			{
				ItemDef item8 = Main.SafeGameDb.GetItem<ItemDef>(definition2.HeadAccessoireGameID);
				if (!string.IsNullOrEmpty(item8.ItemPrefab.GetResourcePath()))
				{
					HeadAccessoireResource = item8.ItemPrefab;
				}
			}
		}
	}
}
