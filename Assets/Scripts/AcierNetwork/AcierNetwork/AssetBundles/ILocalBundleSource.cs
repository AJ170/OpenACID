using System;

namespace AcierNetwork.AssetBundles
{
	// Serves bundles without downloading or opening bundle files: in the editor the imported bundle assets are
	// loaded straight from the project (Assets.Scripts.Offline.LocalBundles). Set AssetBundleManager.LocalSource.
	public interface ILocalBundleSource
	{
		bool Contains(string _bundleUrl);

		UnityEngine.Object LoadAsset(string _bundleUrl, string _name, Type _type);
	}
}
