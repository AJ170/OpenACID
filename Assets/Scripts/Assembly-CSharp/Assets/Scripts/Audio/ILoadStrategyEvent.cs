using System;
using SharedBaseLib.Core;

namespace Assets.Scripts.Audio
{
	public abstract class ILoadStrategyEvent
	{
		private static readonly Type[] s_delegateTypes = new Type[1] { typeof(SRawSoundLoadDef) };

		public readonly SimpleDelegate OnIsReadyToPlay;

		public readonly SimpleDelegate OnLoaded;

		protected ILoadStrategyEvent()
		{
			OnIsReadyToPlay = new SimpleDelegate(this, s_delegateTypes);
			OnLoaded = new SimpleDelegate(this, s_delegateTypes);
		}
	}
}
