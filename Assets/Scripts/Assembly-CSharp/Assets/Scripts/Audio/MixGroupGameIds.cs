using Database;
using UbiCore.Utils;

namespace Assets.Scripts.Audio
{
	public class MixGroupGameIds : UbiCore.Utils.Singleton<MixGroupGameIds>
	{
		public readonly GameID Explosions;

		public readonly GameID TapToStart;

		public readonly GameID WorldObjects;

		public readonly GameID Walla;

		public readonly GameID UINoDuck;

		public readonly GameID WeaponNoDuck;

		public readonly GameID Environment;

		public readonly GameID Default;

		public readonly GameID Music;

		public readonly GameID EagleVision;

		public readonly GameID Footsteps;

		public readonly GameID UI;

		public readonly GameID BFQuestButton;

		public readonly GameID Animus;

		public readonly GameID LeadofFaith;

		public readonly GameID Wounded;

		public readonly GameID Player;

		public readonly GameID NPC;

		public readonly GameID WeaponDuck;

		public MixGroupGameIds()
		{
			GameDB safeGameDb = Main.SafeGameDb;
			Explosions = safeGameDb.FindGameIdFromGuid("6b7caf01-1fc9-4953-81e8-eed58476d500");
			TapToStart = safeGameDb.FindGameIdFromGuid("2bf3bed3-d20c-401e-acd2-8b85b67729b8");
			WorldObjects = safeGameDb.FindGameIdFromGuid("d9bb17b5-91b7-44c2-85d8-c70d648d0796");
			Walla = safeGameDb.FindGameIdFromGuid("a5fce3af-0aef-4be4-84c5-3dd4587d826d");
			UINoDuck = safeGameDb.FindGameIdFromGuid("8e90bdc6-30f3-450f-8efb-dce5137ddff0");
			WeaponNoDuck = safeGameDb.FindGameIdFromGuid("a990f5bd-4a6d-4816-ac62-a2f2537f2d42");
			Environment = safeGameDb.FindGameIdFromGuid("abadc5a0-c9c1-4654-a11f-70dadd19ca18");
			Default = safeGameDb.FindGameIdFromGuid("4ca9566e-9ce6-44ed-b318-24331732d396");
			Music = safeGameDb.FindGameIdFromGuid("d34608f6-7d7b-4ed6-9db8-c6815eda5e2b");
			EagleVision = safeGameDb.FindGameIdFromGuid("c6c48173-40c3-4d37-9fa4-ab9459d50558");
			Footsteps = safeGameDb.FindGameIdFromGuid("292209a8-6ec1-4328-bc67-881a3debb3ed");
			UI = safeGameDb.FindGameIdFromGuid("fe5a9612-4fff-4094-bc34-b2138d160f69");
			BFQuestButton = safeGameDb.FindGameIdFromGuid("5239899e-3d59-45ff-96aa-adf2bc9fe61b");
			Animus = safeGameDb.FindGameIdFromGuid("47330829-2ab0-4883-8bbf-92346dda5ed0");
			LeadofFaith = safeGameDb.FindGameIdFromGuid("3dff080a-43f0-44f6-8534-f9ed0d57b811");
			Wounded = safeGameDb.FindGameIdFromGuid("975d85a7-5f5a-4a02-94d3-d73ee5633b14");
			Player = safeGameDb.FindGameIdFromGuid("c2ae9e76-3df2-4711-ad19-72fedebae65c");
			NPC = safeGameDb.FindGameIdFromGuid("db0b8bdf-a0a5-4fe9-8547-ed0b3ab532c1");
			WeaponDuck = safeGameDb.FindGameIdFromGuid("090288c4-f704-486c-b071-557d75530d60");
		}
	}
}
