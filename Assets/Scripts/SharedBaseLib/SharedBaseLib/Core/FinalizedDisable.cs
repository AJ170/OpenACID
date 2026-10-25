using System;

namespace SharedBaseLib.Core
{
	internal class FinalizedDisable : IDisposable
	{
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool _disposing)
		{
		}

		~FinalizedDisable()
		{
			Dispose(false);
		}
	}
}
