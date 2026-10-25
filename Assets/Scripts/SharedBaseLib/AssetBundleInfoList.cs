using AssetBundleData;
using SharedBaseLib.Serialization;

public class AssetBundleInfoList
{
	private AssetBundleInfo[] m_assetBundleInfos;

	public AssetBundleInfoList(AssetBundleInfo[] _assetBundleInfos)
	{
		m_assetBundleInfos = _assetBundleInfos;
	}

	public AssetBundleInfoList()
	{
	}

	public AssetBundleInfo[] GetAssetBundleInfos()
	{
		return m_assetBundleInfos;
	}

	public void Serialize(DataSerializerBase _s)
	{
		_s.SerializeBeginNode("AssetBundleInfoDict");
		m_assetBundleInfos = _s.SerializeArray("AssetBundleInfos", m_assetBundleInfos);
		_s.SerializeEndNode();
	}
}
