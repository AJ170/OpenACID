using System;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public static class EntityTypes
	{
		public enum EType
		{
			Invalid = 0,
			Entity = 1,
			GameLogic = 2,
			OffMeshLinkEntity = 3,
			GameEntity = 4,
			Patrol = 5,
			SpawnPoint = 6,
			Character = 7,
			AssassinCharacter = 8,
			HirelingCharacter = 9,
			PlayerCharacter = 10,
			NpcCharacter = 11,
			EnemyCharacter = 12,
			CrowdCharacter = 13,
			InteractionCharacter = 14,
			AwareInteractionCharacter = 15,
			Object = 16,
			Projectile = 17,
			InteractibleObject = 18,
			HideoutObject = 19,
			Haystack = 20,
			Codex = 21,
			InteractibleMissionObject = 22,
			Bench = 23,
			RooftopGarden = 24,
			Pigeonry = 25,
			StaticObject = 26,
			StalkingZone = 27,
			Count = 28
		}

		public static readonly Type[] EntityClassTypes;

		static EntityTypes()
		{
			EntityClassTypes = new Type[28];
			EntityClassTypes[1] = typeof(Entity);
			EntityClassTypes[2] = typeof(GameLogic);
			EntityClassTypes[3] = typeof(OffMeshLinkEntity);
			EntityClassTypes[4] = typeof(GameEntity);
			EntityClassTypes[5] = typeof(Patrol);
			EntityClassTypes[7] = typeof(Character);
			EntityClassTypes[8] = typeof(AssassinCharacter);
			EntityClassTypes[9] = typeof(HirelingCharacter);
			EntityClassTypes[10] = typeof(PlayerCharacter);
			EntityClassTypes[11] = typeof(NpcCharacter);
			EntityClassTypes[12] = typeof(EnemyCharacter);
			EntityClassTypes[13] = typeof(CrowdCharacter);
			EntityClassTypes[14] = typeof(InteractionCharacter);
			EntityClassTypes[15] = typeof(AwareInteractionCharacter);
			EntityClassTypes[16] = typeof(Object);
			EntityClassTypes[17] = typeof(Projectile);
			EntityClassTypes[18] = typeof(InteractableObject);
			EntityClassTypes[20] = typeof(Haystack);
			EntityClassTypes[21] = typeof(Codex);
			EntityClassTypes[6] = typeof(SpawnPoint);
			EntityClassTypes[22] = typeof(InteractableMissionObject);
			EntityClassTypes[23] = typeof(Bench);
			EntityClassTypes[24] = typeof(RooftopGarden);
			EntityClassTypes[25] = typeof(Pigeonry);
			EntityClassTypes[26] = typeof(StaticObject);
			EntityClassTypes[27] = typeof(StalkingZone);
		}

		public static Type GetEntityClassType(EType _type)
		{
			return EntityClassTypes[(int)_type];
		}
	}
}
