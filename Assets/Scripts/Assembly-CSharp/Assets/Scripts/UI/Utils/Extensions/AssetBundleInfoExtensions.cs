using System;
using AcierNetwork;
using AcierNetwork.AssetBundles;
using AcierNetwork.Communication;
using AcierNetwork.Provider;
using AssetBundleData;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class AssetBundleInfoExtensions
	{
		public static void Provide(this AssetBundleInfo _assetBundleInfo, Action<bool> _callback)
		{
			if (AssetBundleManager.Instance.HasBundleLoaded(_assetBundleInfo))
			{
				if (_callback != null)
				{
					_callback(true);
				}
				return;
			}
			MissionProviderInput missionProviderInput = new MissionProviderInput();
			missionProviderInput.AssetBundles = new BBHashSet<AssetBundleInfo> { _assetBundleInfo };
			MissionProviderInput param = missionProviderInput;
			NetworkProvider.Instance.Request(delegate(QueryResult _result, MissionData _type)
			{
				if (_result.IsCompleted && _callback != null)
				{
					_callback(!_result.Failed);
				}
			}, param);
		}
	}
}
