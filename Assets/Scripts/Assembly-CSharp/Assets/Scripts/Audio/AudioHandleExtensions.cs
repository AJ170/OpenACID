namespace Assets.Scripts.Audio
{
	public static class AudioHandleExtensions
	{
		public static void UnloadAllNowOrOnDone(this IAudioHandle _this)
		{
			if (_this != null)
			{
				_this.ChangeStoppedFinishedStrategy(EStoppedFinishedStrategy.UnloadAll);
				if (!_this.IsPlaying)
				{
					_this.Unload();
				}
				_this = null;
			}
		}

		public static string Name(this IAudioHandle _this)
		{
			if (AudioManager.AudioControllerSubDb.Data.ContainsKey(_this.AudioController.Id))
			{
				return AudioManager.AudioControllerSubDb.Data[_this.AudioController.Id].Name;
			}
			return string.Empty;
		}
	}
}
