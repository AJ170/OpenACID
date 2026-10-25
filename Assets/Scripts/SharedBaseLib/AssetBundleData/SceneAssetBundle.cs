using System;
using System.IO;
using AcierProtocol;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;

namespace AssetBundleData
{
	public class SceneAssetBundle : IDataSerializable
	{
		public BBList<AssetBundleInfo> m_assetBundleInfos = new BBList<AssetBundleInfo>();

		public string Name { get; set; }

		public AssetBundleInfo GetAssetBundleInfos(EQualityAssetBundle _quality)
		{
			for (int i = 0; i < m_assetBundleInfos.Count; i++)
			{
				AssetBundleInfo assetBundleInfo = m_assetBundleInfos[i];
				if (assetBundleInfo.Quality == _quality)
				{
					return assetBundleInfo;
				}
			}
			return m_assetBundleInfos[0];
		}

		public void AddAssetBundleInfo(AssetBundleInfo _bundleInfo)
		{
			for (int i = 0; i < m_assetBundleInfos.Count; i++)
			{
				if (m_assetBundleInfos[i].Quality == _bundleInfo.Quality)
				{
					m_assetBundleInfos[i] = _bundleInfo;
					return;
				}
			}
			m_assetBundleInfos.Add(_bundleInfo);
		}

		public bool HasAssetBundleInfo(EQualityAssetBundle _quality)
		{
			for (int i = 0; i < m_assetBundleInfos.Count; i++)
			{
				if (m_assetBundleInfos[i].Quality == _quality)
				{
					return true;
				}
			}
			return false;
		}

		public void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			Serialize(_s);
		}

		public void Serialize(DataSerializerBase _s)
		{
			_s.SerializeBeginNode("SceneInfo");
			Name = _s.SerializeString("Name", Name);
			m_assetBundleInfos = _s.SerializeList("AssetBundleInfos", m_assetBundleInfos);
			if (string.IsNullOrEmpty(Name) || m_assetBundleInfos.Count == 0)
			{
				throw new Exception("Invalid AssetBundleInfo in XML!");
			}
			_s.SerializeEndNode();
		}

		public static SceneAssetBundle ReadPackage(string _filename)
		{
			SceneAssetBundle sceneAssetBundle = new SceneAssetBundle();
			using (Stream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read))
			{
				using (DataSerializerBase dataSerializerBase = new DataXmlSerializer(stream, EMode.EMode_Read))
				{
					dataSerializerBase.SerializeBeginNode("root");
					sceneAssetBundle.Serialize(dataSerializerBase);
					dataSerializerBase.SerializeEndNode();
					return sceneAssetBundle;
				}
			}
		}

		public void WritePackage(string _filename)
		{
			if (File.Exists(_filename))
			{
				SharedMain.FixFilePermissions(_filename);
			}
			using (FileStream fileStream = new FileStream(_filename, FileMode.Create))
			{
				using (DataSerializerBase dataSerializerBase = new DataXmlSerializer(fileStream, EMode.EMode_Write, false))
				{
					dataSerializerBase.SerializeBeginNode("root");
					Serialize(dataSerializerBase);
					dataSerializerBase.SerializeEndNode();
					fileStream.Flush();
				}
			}
		}
	}
}
