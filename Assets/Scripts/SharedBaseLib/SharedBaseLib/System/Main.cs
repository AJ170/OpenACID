namespace SharedBaseLib.System
{
	public static class Main
	{
		public static bool ms_initialized;

		public static void InitOnDemand()
		{
			if (!ms_initialized)
			{
				InitInternal();
				ms_initialized = true;
			}
		}

		private static void InitInternal()
		{
			Layers.Init();
			Tags.Init();
			NavMeshLayers.Init();
		}
	}
}
