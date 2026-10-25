namespace Assets.Scripts.Audio
{
	public class PlayResult
	{
		private enum EPlayResult
		{
			Success = 0,
			SuccessPlayOnReadyScheduled = 1,
			SuccessPlayDelayed = 2,
			Failed = 50,
			FailedNoAvailableChannels = 51,
			FailedNotReadyToPlayYet = 52,
			FailedOmittedAlreadyPlaying = 53
		}

		public static readonly PlayResult Success = new PlayResult(EPlayResult.Success);

		public static readonly PlayResult SuccessPlayOnReadyScheduled = new PlayResult(EPlayResult.SuccessPlayOnReadyScheduled);

		public static readonly PlayResult SuccessPlayDelayed = new PlayResult(EPlayResult.SuccessPlayDelayed);

		public static readonly PlayResult Failed = new PlayResult(EPlayResult.Failed);

		public static readonly PlayResult FailedNoAvailableChannels = new PlayResult(EPlayResult.FailedNoAvailableChannels);

		public static readonly PlayResult FailedNotReadyToPlayYet = new PlayResult(EPlayResult.FailedNotReadyToPlayYet);

		public static readonly PlayResult FailedOmittedAlreadyPlaying = new PlayResult(EPlayResult.FailedOmittedAlreadyPlaying);

		private readonly EPlayResult m_ePlayResult;

		private PlayResult(EPlayResult _ePlayResult)
		{
			m_ePlayResult = _ePlayResult;
		}

		public override string ToString()
		{
			return m_ePlayResult.ToString();
		}

		public static implicit operator bool(PlayResult _playResult)
		{
			return _playResult.m_ePlayResult < Failed.m_ePlayResult;
		}

		public static implicit operator PlayResult(bool _value)
		{
			return (!_value) ? Failed : Success;
		}
	}
}
