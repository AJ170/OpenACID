using System;
using System.Collections.Generic;
using System.IO;
using AcierProtocol;
using AssetBundleData;
using Ionic.Zlib;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;

namespace Database
{
	public class GameDBSerializer
	{
		public enum SerializationFormat
		{
			kFormat_Xml = 0,
			kFormat_Binary = 1
		}

		public class AssetBundleDep
		{
			public short[] DepAssetBundleID;

			public short OwnAssetBundleID;
		}

		private const int kCurrentVersion = 1;

		private const string kIdentifier = "Identifier";

		private const string kIdentifierValue = "ACIDRAD";

		private const string kRuntimeAssetList = "RuntimeAssetList";

		private const string kRuntimeAsset = "RuntimeAsset";

		private const string kRuntimeAssetDep = "RuntimeAssetDep";

		private const string kRuntimeAssetData = "RuntimeAssetData";

		private const string kAssetBundleList = "AssetBundleList";

		private const string kRoot = "root";

		private const string kHeader = "Header";

		private const string kBuildTarget = "BuildTarget";

		private const string kBundleID = "BundleId";

		private const string kDepBundleIDs = "DepBundleIDs";

		private const string kType = "Type";

		private const string kVersion = "Version";

		private const string kAssetBundleAvailable = "AssetBundleAvailable";

		private const string kVariations = "Variations";

		private static void SerializeHeader(DataSerializerBase _s, string _buildTarget = null)
		{
			_s.SerializeBeginNode("Header");
			string strA = _s.SerializeIdentifier("Identifier", "ACIDRAD");
			if (string.Compare(strA, "ACIDRAD") != 0)
			{
			}
			int num = _s.SerializeInt("Version", 1);
			if (num != 1)
			{
			}
			string text = _s.SerializeString("BuildTarget", _buildTarget);
			_s.SerializeEndNode();
		}

		private static void WriteGameDBInternal(DataSerializerBase _s, string _buildTarget, MultiMap<short, AssetBundleInfo> _assetBundleInfos, Dictionary<DBEntry, AssetBundleDep> _gameDBEntries)
		{
			_s.SerializeBeginNode("root");
			SerializeHeader(_s, _buildTarget);
			WriteAssetBundleInfos(_assetBundleInfos, _s);
			WriteGameDBEntries(_gameDBEntries, _s);
			_s.SerializeEndNode();
		}

		public static void WriteGameDB(string _filename, string _buildTarget, MultiMap<short, AssetBundleInfo> _assetBundleInfos, Dictionary<DBEntry, AssetBundleDep> _gameDBEntries, SerializationFormat _format = SerializationFormat.kFormat_Binary)
		{
			try
			{
				if (_format == SerializationFormat.kFormat_Binary)
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (DataSerializerBase s = new DataBinarySerializer(memoryStream, EMode.EMode_Write))
						{
							WriteGameDBInternal(s, _buildTarget, _assetBundleInfos, _gameDBEntries);
							if (File.Exists(_filename))
							{
								File.SetAttributes(_filename, FileAttributes.Normal);
							}
							using (FileStream fileStream = new FileStream(_filename, FileMode.Create))
							{
								memoryStream.Position = 0L;
								using (ZlibStream zlibStream = new ZlibStream(fileStream, CompressionMode.Compress, CompressionLevel.BestCompression))
								{
									byte[] array = new byte[memoryStream.Length];
									memoryStream.Read(array, 0, array.Length);
									zlibStream.Write(array, 0, array.Length);
									zlibStream.Flush();
									fileStream.Flush();
									return;
								}
							}
						}
					}
				}
				using (FileStream fileStream2 = new FileStream(_filename, FileMode.Create))
				{
					using (DataXmlSerializer s2 = new DataXmlSerializer(fileStream2, EMode.EMode_Write))
					{
						WriteGameDBInternal(s2, _buildTarget, _assetBundleInfos, _gameDBEntries);
						fileStream2.Flush();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static void WriteAssetBundleInfos(MultiMap<short, AssetBundleInfo> _assetBundleInfos, DataSerializerBase _s)
		{
			_s.SerializeBeginNode("AssetBundleList");
			_s.SerializeItemCount(_assetBundleInfos.GetValueItemCount());
			foreach (short key in _assetBundleInfos.Keys)
			{
				foreach (AssetBundleInfo item in _assetBundleInfos[key])
				{
					item.Serialize(_s);
				}
			}
			_s.SerializeEndNode();
		}

		private static void WriteGameDBEntries(Dictionary<DBEntry, AssetBundleDep> _gameDBEntries, DataSerializerBase _s)
		{
			_s.SerializeBeginNode("RuntimeAssetList");
			_s.SerializeItemCount(_gameDBEntries.Count);
			foreach (KeyValuePair<DBEntry, AssetBundleDep> _gameDBEntry in _gameDBEntries)
			{
				_s.SerializeBeginNode("RuntimeAsset");
				_s.SerializeBeginNode("RuntimeAssetDep");
				_s.SerializeInt16("BundleId", _gameDBEntry.Value.OwnAssetBundleID);
				_s.SerializeArrayInt16("DepBundleIDs", _gameDBEntry.Value.DepAssetBundleID);
				_s.SerializeEndNode();
				_s.SerializeBeginNode("RuntimeAssetData");
				_s.SerializeType("Type", _gameDBEntry.Key.SerializedType);
				_gameDBEntry.Key.Serialize(_s);
				_s.SerializeEndNode();
				_s.SerializeEndNode();
			}
			_s.SerializeEndNode();
		}

		public static void ReadGameDBFromFile(string _filename, BBDictionary<int, AssetBundleInfo> _assetBundleInfos, EQualityAssetBundle _quality, BBDictionary<GameID, DBEntry> _dbEntries, SerializationFormat _format, MultiMap<int, AssetBundleInfo> _allAssetBundleInfos = null)
		{
			try
			{
				using (Stream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read))
				{
					ReadGameDBFromStream(stream, _assetBundleInfos, _quality, _dbEntries, _format, _allAssetBundleInfos);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static DataSerializerBase GetDataSerializer(SerializationFormat _format, Stream _stream, EMode _mode)
		{
			if (_format == SerializationFormat.kFormat_Binary)
			{
				return new DataBinarySerializer(_stream, _mode);
			}
			return new DataXmlSerializer(_stream, _mode);
		}

		private static void ReadGameDBFromStream(Stream _stream, BBDictionary<int, AssetBundleInfo> _assetBundleInfos, EQualityAssetBundle _quality, BBDictionary<GameID, DBEntry> _dbEntries, SerializationFormat _format, MultiMap<int, AssetBundleInfo> _allAssetBundleInfos)
		{
			try
			{
				if (_format == SerializationFormat.kFormat_Binary)
				{
					using (ZlibStream zlibStream = new ZlibStream(_stream, CompressionMode.Decompress))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							byte[] array = new byte[4096];
							int num = 0;
							do
							{
								num = zlibStream.Read(array, 0, array.Length);
								memoryStream.Write(array, 0, num);
							}
							while (num > 0);
							memoryStream.Position = 0L;
							using (DataSerializerBase dataSerializerBase = GetDataSerializer(_format, memoryStream, EMode.EMode_Read))
							{
								dataSerializerBase.SerializeBeginNode("root");
								SerializeHeader(dataSerializerBase);
								ReadAssetBundleInfos(dataSerializerBase, _assetBundleInfos, _quality, _allAssetBundleInfos);
								ReadGameDBEntries(dataSerializerBase, _assetBundleInfos, _dbEntries);
								dataSerializerBase.SerializeEndNode();
								return;
							}
						}
					}
				}
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					byte[] array2 = new byte[4096];
					int num2 = 0;
					do
					{
						num2 = _stream.Read(array2, 0, array2.Length);
						memoryStream2.Write(array2, 0, num2);
					}
					while (num2 > 0);
					memoryStream2.Position = 0L;
					using (DataSerializerBase dataSerializerBase2 = GetDataSerializer(_format, memoryStream2, EMode.EMode_Read))
					{
						dataSerializerBase2.SerializeBeginNode("root");
						SerializeHeader(dataSerializerBase2);
						ReadAssetBundleInfos(dataSerializerBase2, _assetBundleInfos, _quality, _allAssetBundleInfos);
						ReadGameDBEntries(dataSerializerBase2, _assetBundleInfos, _dbEntries);
						dataSerializerBase2.SerializeEndNode();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static void ReadGameDBEntries(DataSerializerBase _s, BBDictionary<int, AssetBundleInfo> _assetBundleInfos, BBDictionary<GameID, DBEntry> _dbEntries)
		{
			try
			{
				_s.SerializeBeginNode("RuntimeAssetList");
				int num = _s.SerializeItemCount();
				for (int i = 0; i < num; i++)
				{
					_s.SerializeBeginNode("RuntimeAsset");
					AssetBundleDep assetBundleDep = new AssetBundleDep();
					_s.SerializeBeginNode("RuntimeAssetDep");
					assetBundleDep.OwnAssetBundleID = _s.SerializeInt16("BundleId", 0);
					assetBundleDep.DepAssetBundleID = _s.SerializeArrayInt16("DepBundleIDs");
					_s.SerializeEndNode();
					_s.SerializeBeginNode("RuntimeAssetData");
					AssetBundleInfo assetBundleInfo = ((assetBundleDep.OwnAssetBundleID == -1) ? null : _assetBundleInfos[assetBundleDep.OwnAssetBundleID]);
					Type type = _s.SerializeType("Type");
					DBEntry dBEntry = CreateDBEntryFromStream(_s, type, assetBundleInfo, assetBundleDep.DepAssetBundleID);
					_dbEntries[dBEntry.GameID] = dBEntry;
					_s.SerializeEndNode();
					_s.SerializeEndNode();
				}
				_s.SerializeEndNode();
			}
			catch (Exception ex)
			{
				string text = ex.ToString();
				throw;
			}
		}

		private static void ReadAssetBundleInfos(DataSerializerBase _s, BBDictionary<int, AssetBundleInfo> _assetBundleInfos, EQualityAssetBundle _quality, MultiMap<int, AssetBundleInfo> _allAssetBundleInfos)
		{
			BBDictionary<int, AssetBundleInfo> bBDictionary = new BBDictionary<int, AssetBundleInfo>();
			_s.SerializeBeginNode("AssetBundleList");
			int num = _s.SerializeItemCount();
			for (int i = 0; i < num; i++)
			{
				AssetBundleInfo assetBundleInfo = new AssetBundleInfo();
				assetBundleInfo.Serialize(_s);
				if (_allAssetBundleInfos != null)
				{
					_allAssetBundleInfos.Add(assetBundleInfo.BundleId, assetBundleInfo);
				}
				if (assetBundleInfo.Quality == EQualityAssetBundle.High)
				{
					_assetBundleInfos.Add(assetBundleInfo.BundleId, assetBundleInfo);
				}
				else if (assetBundleInfo.Quality == EQualityAssetBundle.Low)
				{
					bBDictionary.Add(assetBundleInfo.BundleId, assetBundleInfo);
				}
			}
			_s.SerializeEndNode();
			if (_quality != EQualityAssetBundle.Low)
			{
				return;
			}
			foreach (KeyValuePair<int, AssetBundleInfo> item in bBDictionary)
			{
				_assetBundleInfos[item.Key] = item.Value;
			}
		}

		public static void ReadGameDBFromFile(string _filename, out AssetBundleInfo _assetBundleInfo)
		{
			_assetBundleInfo = null;
			try
			{
				using (Stream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read))
				{
					using (DataSerializerBase dataSerializerBase = new DataXmlSerializer(stream, EMode.EMode_Read))
					{
						dataSerializerBase.SerializeBeginNode("root");
						if (dataSerializerBase.SerializeBool("AssetBundleAvailable"))
						{
							_assetBundleInfo = new AssetBundleInfo();
							_assetBundleInfo.Serialize(dataSerializerBase);
						}
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static void ReadGameDBFromStream(Stream _stream, out List<DBEntry> _dbEntries, out AssetBundleInfo _assetBundleInfo)
		{
			_dbEntries = null;
			_assetBundleInfo = null;
			try
			{
				using (DataSerializerBase dataSerializerBase = new DataXmlSerializer(_stream, EMode.EMode_Read))
				{
					dataSerializerBase.SerializeBeginNode("root");
					if (dataSerializerBase.SerializeBool("AssetBundleAvailable"))
					{
						_assetBundleInfo = new AssetBundleInfo();
						_assetBundleInfo.Serialize(dataSerializerBase);
					}
					dataSerializerBase.SerializeBeginNode("Variations");
					int num = dataSerializerBase.SerializeItemCount();
					_dbEntries = new List<DBEntry>(num);
					for (int i = 0; i < num; i++)
					{
						dataSerializerBase.SerializeBeginNode("RuntimeAsset");
						Type type = dataSerializerBase.SerializeType("Type");
						DBEntry item = CreateDBEntryFromStream(dataSerializerBase, type, _assetBundleInfo, null);
						_dbEntries.Add(item);
						dataSerializerBase.SerializeEndNode();
					}
					dataSerializerBase.SerializeEndNode();
					dataSerializerBase.SerializeEndNode();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static void ReadGameDBFromString(string _content, out List<DBEntry> _dbEntries, out AssetBundleInfo _assetBundleInfo)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (StreamWriter streamWriter = new StreamWriter(memoryStream))
					{
						streamWriter.Write(_content);
						streamWriter.Flush();
						memoryStream.Position = 0L;
						ReadGameDBFromStream(memoryStream, out _dbEntries, out _assetBundleInfo);
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static void ReadGameDBFromFile(string _filename, out List<DBEntry> _dbEntries, out AssetBundleInfo _assetBundleInfo)
		{
			try
			{
				using (Stream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read))
				{
					ReadGameDBFromStream(stream, out _dbEntries, out _assetBundleInfo);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public static List<DBEntry> ReadGameDB(string _filename)
		{
			List<DBEntry> list = null;
			using (Stream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read))
			{
				using (DataSerializerBase dataSerializerBase = new DataXmlSerializer(stream, EMode.EMode_Read))
				{
					dataSerializerBase.SerializeBeginNode("root");
					dataSerializerBase.SerializeBeginNode("Variations");
					int num = dataSerializerBase.SerializeItemCount();
					list = new List<DBEntry>(num);
					for (int i = 0; i < num; i++)
					{
						dataSerializerBase.SerializeBeginNode("RuntimeAsset");
						Type type = dataSerializerBase.SerializeType("Type");
						DBEntry item = CreateDBEntryFromStream(dataSerializerBase, type, null, null);
						list.Add(item);
						dataSerializerBase.SerializeEndNode();
					}
					dataSerializerBase.SerializeEndNode();
					dataSerializerBase.SerializeEndNode();
					return list;
				}
			}
		}

		public static void WriteGameDB(string _filename, IEnumerable<DBEntry> _dbEntries, AssetBundleInfo _assetBundleInfo = null)
		{
			try
			{
				List<DBEntry> list = _dbEntries.ToList();
				list.Sort((DBEntry l, DBEntry r) => l.CompareTo(r));
				using (Stream stream = new FileStream(_filename, FileMode.Create))
				{
					using (DataSerializerBase dataSerializerBase = new DataXmlSerializer(stream, EMode.EMode_Write))
					{
						dataSerializerBase.SerializeBeginNode("root");
						dataSerializerBase.SerializeBool("AssetBundleAvailable", _assetBundleInfo != null);
						if (_assetBundleInfo != null)
						{
							_assetBundleInfo.Serialize(dataSerializerBase);
						}
						dataSerializerBase.SerializeBeginNode("Variations");
						dataSerializerBase.SerializeItemCount(list.Count);
						foreach (DBEntry item in list)
						{
							dataSerializerBase.SerializeBeginNode("RuntimeAsset");
							dataSerializerBase.SerializeType("Type", item.SerializedType);
							item.Serialize(dataSerializerBase);
							dataSerializerBase.SerializeEndNode();
						}
						dataSerializerBase.SerializeEndNode();
						dataSerializerBase.SerializeEndNode();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private static DBEntry CreateDBEntryFromStream(DataSerializerBase _s, Type _type, AssetBundleInfo _assetBundleInfo, short[] _depAssetBundleInfos)
		{
			DBEntry dBEntry = (DBEntry)Activator.CreateInstance(_type, _assetBundleInfo);
			dBEntry.DepAssetBundlesInfos = _depAssetBundleInfos;
			dBEntry.Serialize(_s);
			return dBEntry;
		}
	}
}
