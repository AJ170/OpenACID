using AcierProtocol;
using SharedBaseLib.Serialization;

namespace AssetBundleData
{
	public class AssetBundleInfo : IDataSerializable
	{
		public enum EType
		{
			Environment = 0,
			RuntimeAssets = 1,
			SoundContainer = 2
		}

		public const int BuildNumberDontCache = 0;

		private string m_toStringCache;

		public int BundleId { get; set; }

		public string URL { get; private set; }

		public int BuildNumber { get; private set; }

		public EType Type { get; private set; }

		public string SourceFilesHash { get; private set; }

		public string AssetBundleHash { get; private set; }

		public long AssetBundleFileSize { get; private set; }

		public bool EnforceLoading { get; private set; }

		public EQualityAssetBundle Quality { get; private set; }

		public AssetBundleInfo()
		{
		}

		public AssetBundleInfo(string _uri, EType _type, string _assetBundleHash, long _assetBundleFileSize, EQualityAssetBundle _quality, int _buildNumber = 0, bool _enforceLoading = false, string _sourceFilesHash = null)
		{
			Type = _type;
			URL = _uri;
			AssetBundleHash = _assetBundleHash;
			AssetBundleFileSize = _assetBundleFileSize;
			Quality = _quality;
			EnforceLoading = _enforceLoading;
			BuildNumber = _buildNumber;
			SourceFilesHash = _sourceFilesHash;
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(URL);
		}

		public override string ToString()
		{
			if (m_toStringCache == null)
			{
				m_toStringCache = string.Format("{0}_{1}_{2}", URL, Type, BuildNumber);
			}
			return m_toStringCache;
		}

		public void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			Serialize(_s);
		}

		public void Serialize(DataSerializerBase _s)
		{
			_s.SerializeBeginNode("AssetBundleInfo");
			BundleId = _s.SerializeInt32("BundleId", BundleId);
			URL = _s.SerializeString("URL", URL);
			BuildNumber = _s.SerializeInt32("BuildNumber", BuildNumber);
			Type = (EType)_s.SerializeEnum("Type", Type.GetType(), (int)Type);
			SourceFilesHash = _s.SerializeString("SourceFilesHash", SourceFilesHash);
			AssetBundleHash = _s.SerializeString("AssetBundleHash", AssetBundleHash);
			AssetBundleFileSize = _s.SerializeInt64("AssetBundleFileSize", AssetBundleFileSize);
			EnforceLoading = _s.SerializeBool("EnforceLoading", EnforceLoading);
			Quality = (EQualityAssetBundle)_s.SerializeEnum("Quality", Quality.GetType(), (int)Quality);
			_s.SerializeEndNode();
		}
	}
}
