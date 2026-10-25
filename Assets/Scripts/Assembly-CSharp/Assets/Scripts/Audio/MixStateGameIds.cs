using Database;
using UbiCore.Utils;

namespace Assets.Scripts.Audio
{
	public class MixStateGameIds : UbiCore.Utils.Singleton<MixStateGameIds>
	{
		public readonly GameID EagleVision;

		public readonly GameID HigherGround;

		public readonly GameID WallaFade;

		public readonly GameID SlowMotion;

		public readonly GameID Wounded;

		public readonly GameID Combat;

		public readonly GameID Chase;

		public readonly GameID Loading;

		public readonly GameID Adcolony;

		public readonly GameID PlayerDie;

		public readonly GameID DebriefScreen;

		public readonly GameID Pause;

		public readonly GameID PausePopUp;

		public readonly GameID LevelStartCameraPan;

		public MixStateGameIds()
		{
			GameDB safeGameDb = Main.SafeGameDb;
			EagleVision = safeGameDb.FindGameIdFromGuid("c6988f0f-9c2b-4860-9e61-80057b4ce173");
			HigherGround = safeGameDb.FindGameIdFromGuid("ce0e7ef4-df00-4c99-962d-6152c68634c8");
			WallaFade = safeGameDb.FindGameIdFromGuid("a6c7312c-790e-47e1-b62c-6a040b76e6e2");
			SlowMotion = safeGameDb.FindGameIdFromGuid("29873199-79c1-42f1-a1ba-5549a27a7457");
			Wounded = safeGameDb.FindGameIdFromGuid("2adbc43f-662a-496e-8cdf-77f67e8b433c");
			Combat = safeGameDb.FindGameIdFromGuid("2ef48a84-900d-476c-9a60-8716d3d7f6ea");
			Chase = safeGameDb.FindGameIdFromGuid("bacbd088-1089-4f70-b439-64842bb2a4a6");
			Loading = safeGameDb.FindGameIdFromGuid("5fad5a52-7c9b-465b-b193-db5985ffb4cc");
			Adcolony = safeGameDb.FindGameIdFromGuid("b81d1cf7-8046-4f0e-a1c2-fe4d36d7f77c");
			PlayerDie = safeGameDb.FindGameIdFromGuid("0ad83cfa-d5f3-48f3-8a12-6ec927409848");
			DebriefScreen = safeGameDb.FindGameIdFromGuid("170ecd1d-d4f5-4613-8926-03ac2af4cf9c");
			Pause = safeGameDb.FindGameIdFromGuid("5609db68-b7a7-447c-a2d4-546b34ddf5c7");
			PausePopUp = safeGameDb.FindGameIdFromGuid("8c8956aa-42d1-4eb8-b0f1-e9a02029d0ce");
			LevelStartCameraPan = safeGameDb.FindGameIdFromGuid("ea32edb8-7791-4d1b-a2ac-9b307056de6a");
		}
	}
}
