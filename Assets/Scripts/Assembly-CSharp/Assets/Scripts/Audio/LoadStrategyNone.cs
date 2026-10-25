using SharedBaseLib.Core;

namespace Assets.Scripts.Audio
{
	public class LoadStrategyNone : LoadStrategyBase
	{
		public override ELoadStrategy ELoadStrategy
		{
			get
			{
				return ELoadStrategy.LoadNone;
			}
		}

		public LoadStrategyNone(BBSimpleList<SRawSoundLoadDef> _list)
			: base(_list)
		{
			AudioResourceManager.Instance.OnIsReadyToPlay += new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
			AudioResourceManager.Instance.OnLoaded += new SimpleDelegate.CallbackFunction(OnLoadedCallback);
		}

		~LoadStrategyNone()
		{
			AudioResourceManager.Instance.OnIsReadyToPlay -= new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
			AudioResourceManager.Instance.OnLoaded -= new SimpleDelegate.CallbackFunction(OnLoadedCallback);
		}

		public override void Load()
		{
		}

		public override void Unload()
		{
		}
	}
}
