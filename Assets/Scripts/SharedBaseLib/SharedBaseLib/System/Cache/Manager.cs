using System;
using System.Collections;
using System.IO;
using System.Threading;
using AssetBundleData;
using BB.Mobile.Net;
using BB.Mobile.Net.Http;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.Storage;
using UbiCore.Helpers;
using UbiCore.Utils;
using UnityEngine;

namespace SharedBaseLib.System.Cache
{
	public class Manager : Singleton<Manager>
	{
		public enum EItemType
		{
			Simple = 0,
			Zip = 1,
			Archive = 2
		}

		public enum EErrorReason
		{
			None = 0,
			HTTPError = 1,
			CacheFull = 2,
			OutOfDiskspace = 3
		}

		public const string LocalFilePrefix = "file://";

		private const string s_Dict = "catalogue.bin";

		private const ushort Version = 8;

		private const long MaxSize = 1932735283L;

		private const long DownloadChunk = 4194304L;

		private const string TagVersion = "version";

		public static readonly string BaseDir = SavedDataDir.GetPath() + "/cache/";

		public static readonly byte[] Buffer = new byte[131072];

		private readonly HttpClient mHttpClient = new HttpClient(null, null, false, 60000, 1);

		private readonly BBList<Item> m_Entities = new BBList<Item>();

		private readonly BBList<Item> m_EntitiesStatic = new BBList<Item>();

		public readonly SimpleDelegate ProgressDelegate;

		private ulong m_currentSize;

		private const string kCache = "Cache";

		private const string kItems = "Items";

		private const string kItem = "Item";

		private const string kType = "Type";

		public bool PotentialConnectionIssue { get; private set; }

		public string LastError { get; private set; }

		public EErrorReason ErrorReason { get; private set; }

		public Manager()
		{
			try
			{
				m_EntitiesStatic = ReadDictionary(GameFolders.GetRuntimeAssetsDictFilename(), GameFolders.GetAcierStaticDataPath());
			}
			catch (Exception)
			{
				m_EntitiesStatic.Clear();
			}
			try
			{
				m_Entities = ReadDictionary(BaseDir + "catalogue.bin", BaseDir);
				m_currentSize = Verify();
			}
			catch (Exception)
			{
				m_Entities.Clear();
				PrepareDirectory();
			}
			PotentialConnectionIssue = false;
			LastError = string.Empty;
			ProgressDelegate = new SimpleDelegate(this, new Type[2]
			{
				typeof(string),
				typeof(uint)
			});
		}

		private static void PrepareDirectory()
		{
			if (Directory.Exists(BaseDir))
			{
				string[] directories = Directory.GetDirectories(BaseDir);
				for (int i = 0; i < directories.Length; i++)
				{
					Directory.Delete(directories[i], true);
				}
				directories = Directory.GetFiles(BaseDir);
				foreach (string path in directories)
				{
					File.SetAttributes(path, FileAttributes.Normal);
					File.Delete(path);
				}
			}
			else
			{
				Directory.CreateDirectory(BaseDir);
			}
		}

		private ulong Verify()
		{
			BBList<string> _files = new BBList<string>();
			_files.Add(BaseDir + "catalogue.bin");
			ulong num = 0uL;
			try
			{
				int num2 = m_Entities.Count;
				int num3 = 0;
				while (num3 < num2)
				{
					Item item = m_Entities[num3];
					if (item.Verify(ref _files))
					{
						num += item.CurrentSize;
						num3++;
					}
					else
					{
						item.CleanUp();
						m_Entities.RemoveAt(num3);
						num2--;
					}
				}
			}
			catch (Exception)
			{
			}
			string[] files = Directory.GetFiles(BaseDir);
			foreach (string text in files)
			{
				if (!_files.Contains(text))
				{
					File.SetAttributes(text, FileAttributes.Normal);
					File.Delete(text);
				}
			}
			files = Directory.GetDirectories(BaseDir);
			for (int i = 0; i < files.Length; i++)
			{
				string[] files2 = Directory.GetFiles(files[i]);
				foreach (string text2 in files2)
				{
					if (!_files.Contains(text2))
					{
						File.SetAttributes(text2, FileAttributes.Normal);
						File.Delete(text2);
					}
				}
			}
			return num;
		}

		private void WriteDict()
		{
			string filename = BaseDir + "catalogue.bin";
			WriteDictionary(m_Entities, filename);
			m_currentSize = Verify();
		}

		private bool MakeRoom(ulong _requestedSize)
		{
			return true;
		}

		private void UpdateOrAddEntry(Item _newEntry)
		{
			Item item = m_Entities.Find((Item entry) => entry.BaseName == _newEntry.BaseName && entry.Revision != _newEntry.Revision);
			if (item != null)
			{
				m_currentSize -= item.CurrentSize;
				item.CleanUp();
				m_Entities.Remove(item);
			}
			ulong num = _newEntry.Size - _newEntry.CurrentSize;
			if (!MakeRoom(num))
			{
				LastError = string.Format("Unable to make enough room for Entity {0} - internal cache size exceeded!", _newEntry);
				ErrorReason = EErrorReason.CacheFull;
			}
			if (string.IsNullOrEmpty(LastError) && SharedMain.GetFreeDiskSpace() < num)
			{
				LastError = string.Format("Not enough disk space to store additional {0} bytes for {1}", num, _newEntry);
				ErrorReason = EErrorReason.OutOfDiskspace;
			}
			if (string.IsNullOrEmpty(LastError) && !m_Entities.Contains(_newEntry))
			{
				m_Entities.Add(_newEntry);
			}
			m_currentSize = 0uL;
			for (int i = 0; i < m_Entities.Count; i++)
			{
				m_currentSize += m_Entities[i].CurrentSize;
			}
			WriteDict();
		}

		public void Clear()
		{
			if (m_Entities.Count > 0)
			{
				ulong num = 0uL;
				int count = m_Entities.Count;
				foreach (Item entity in m_Entities)
				{
					num += entity.CurrentSize;
					entity.CleanUp();
				}
				m_Entities.Clear();
			}
			PrepareDirectory();
			WriteDict();
		}

		private int GetIndexFor(string _name, uint _revision)
		{
			string baseName = Item.GetBaseName(_name);
			return m_Entities.FindIndex((Item entity) => entity.BaseName == baseName && (entity.Revision == _revision || _revision == 0));
		}

		public Item GetEntry(string _name, uint _revision, bool _silently = false)
		{
			Item entryUnsafe = GetEntryUnsafe(_name, _revision);
			if (entryUnsafe != null && entryUnsafe.DownloadDone)
			{
				if (!_silently)
				{
					entryUnsafe.UpdateDate();
				}
				return entryUnsafe;
			}
			return null;
		}

		public Item GetEntryUnsafe(string _name, uint _revision)
		{
			int indexFor = GetIndexFor(_name, _revision);
			if (indexFor != -1)
			{
				return m_Entities[indexFor];
			}
			string baseName = Item.GetBaseName(_name);
			indexFor = m_EntitiesStatic.FindIndex((Item entity) => entity.BaseName == baseName && (entity.Revision == _revision || _revision == 0));
			if (indexFor != -1)
			{
				return m_EntitiesStatic[indexFor];
			}
			return null;
		}

		public void Remove(Item _ce)
		{
			if (_ce != null)
			{
				if (m_Entities.Remove(_ce))
				{
					_ce.CleanUp();
				}
				else
				{
					m_EntitiesStatic.Remove(_ce);
				}
				WriteDict();
			}
		}

		public IEnumerator PrepareFile(string _name, uint _revision, bool _forced, EItemType _type, ulong _exepectedSize)
		{
			bool flag = _forced;
			Item item = GetEntryUnsafe(_name, _revision);
			if (_forced && item != null)
			{
				m_currentSize -= item.CurrentSize;
				item.CleanUp();
				m_Entities.Remove(item);
				item = null;
			}
			if (!_forced && item != null && item.DownloadDone)
			{
				yield break;
			}
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			Uri val = new Uri(_name);
			// URLs are escaped (GameFolders.FixURL: spaces -> %20); the local file path is not.
			string srcFn = Uri.UnescapeDataString(_name.Substring("file://".Length, _name.Length - "file://".Length));
			PotentialConnectionIssue = false;
			LastError = string.Empty;
			ErrorReason = EErrorReason.None;
			Item entry = null;
			if (item != null)
			{
				entry = item;
			}
			else
			{
				switch (_type)
				{
				case EItemType.Simple:
					entry = new SimpleItem(_revision, _name, BaseDir);
					break;
				case EItemType.Zip:
					entry = new ZippedItem(_revision, _name, BaseDir);
					break;
				}
				entry.Location = Guid.NewGuid().ToString();
			}
			string targetFn = Path.Combine(BaseDir, entry.Location);
			if (val.IsFile)
			{
				bool done = false;
				FileInfo fileInfo = new FileInfo(srcFn);
				uint progress = (uint)((entry.Size != 0) ? fileInfo.Length : 0);
				if ((decimal)fileInfo.Length > (decimal)SharedMain.GetFreeDiskSpace())
				{
					LastError = "Not enough diskspace for file copy!";
					ErrorReason = EErrorReason.OutOfDiskspace;
				}
				else
				{
					Exception error = null;
					ThreadPool.QueueUserWorkItem(delegate
					{
						try
						{
							do
							{
								using (FileStream fileStream2 = File.OpenRead(srcFn))
								{
									using (FileStream fileStream3 = new FileStream(targetFn, FileMode.Append, FileAccess.Write))
									{
										Item item2 = entry;
										ulong size = (entry.CurrentSize = (ulong)fileStream2.Length);
										item2.Size = size;
										fileStream2.Seek(fileStream3.Length, SeekOrigin.Begin);
										int num2 = 0;
										int num3 = 4194304;
										while ((num2 = fileStream2.Read(Buffer, 0, Buffer.Length)) > 0)
										{
											fileStream3.Write(Buffer, 0, num2);
											entry.CurrentSize = (ulong)fileStream3.Position;
											progress = (uint)fileStream3.Position;
											num3 -= num2;
											if (num3 < 0)
											{
												break;
											}
										}
									}
								}
								UpdateOrAddEntry(entry);
							}
							while (!entry.DownloadDone && !SharedMain.IsShuttingDown);
						}
						catch (Exception ex5)
						{
							error = ex5;
						}
						done = true;
					});
					while (!done)
					{
						InvokeProgress(_name, progress);
						yield return wait;
					}
					if (error != null)
					{
						LastError = error.ToString();
						ErrorReason = EErrorReason.HTTPError;
					}
				}
			}
			else
			{
				if (_exepectedSize != 0L)
				{
					if (entry.Size != 0L && (entry.Size != _exepectedSize || entry.CurrentSize > _exepectedSize))
					{
						new FileInfo(targetFn);
						m_currentSize -= entry.CurrentSize;
						entry.CleanUp();
					}
					entry.Size = _exepectedSize;
				}
				UpdateOrAddEntry(entry);
				if (_exepectedSize == 0L || entry.CurrentSize != entry.Size)
				{
					IAsyncResult asyncResult = null;
					using (FileStream fileStream = new FileStream(targetFn, FileMode.Append, FileAccess.Write))
					{
						ulong position = (ulong)fileStream.Position;
						entry.CurrentSize = position;
						if (_exepectedSize != 0L)
						{
							if (!entry.DownloadDone)
							{
								ulong num = entry.Size - 1;
								if (_exepectedSize != 0L)
								{
									asyncResult = mHttpClient.BeginReadRangeRequest(val, (int)entry.CurrentSize, (int)num, null, new StreamPayload(fileStream, string.Empty, false), null, this, null);
								}
								while (!asyncResult.IsCompleted)
								{
									try
									{
										InvokeProgress(_name, (uint)fileStream.Position);
									}
									catch (Exception)
									{
									}
									yield return wait;
								}
							}
							else
							{
								InvokeProgress(_name, (uint)entry.CurrentSize);
							}
						}
						else
						{
							asyncResult = mHttpClient.BeginRequest(val, HttpVerb.GET, null, null, new StreamPayload(fileStream, string.Empty, false), null, this, null);
							while (!asyncResult.IsCompleted)
							{
								try
								{
									InvokeProgress(_name, (uint)fileStream.Position);
								}
								catch (Exception)
								{
								}
								yield return wait;
							}
							entry.Size = (ulong)fileStream.Position;
						}
					}
					SharedMain.FixFilePermissions(targetFn);
					if (asyncResult != null)
					{
						try
						{
							try
							{
								HttpResult httpResult = mHttpClient.EndRequest(asyncResult);
								if (!httpResult.Succeeded)
								{
									LastError = "Failed with to request file with HTTP Status Code " + httpResult.StatusCode;
									ErrorReason = EErrorReason.HTTPError;
								}
							}
							catch (MobileNetException ex3)
							{
								PotentialConnectionIssue = ex3.PotentialConnectionIssue;
								throw;
							}
						}
						catch (Exception)
						{
							LastError = "Failed with to request file with HTTP!";
							ErrorReason = EErrorReason.HTTPError;
						}
					}
					FileInfo fileInfo2 = new FileInfo(targetFn);
					entry.CurrentSize = (ulong)fileInfo2.Length;
					UpdateOrAddEntry(entry);
				}
			}
			if (string.IsNullOrEmpty(LastError))
			{
				IEnumerator ite = entry.Prepare(entry.Location, true);
				while (ite.MoveNext())
				{
					yield return new WaitForSeconds(1f);
				}
				if (entry.DownloadDone)
				{
					UpdateOrAddEntry(entry);
				}
				else
				{
					LastError = "Failed to Prepare downloaded file for use!";
				}
			}
		}

		public IEnumerator UpdateCRCs()
		{
			int activeThreads = 0;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			int i = 0;
			while (i < m_Entities.Count)
			{
				while (activeThreads >= 4)
				{
					yield return wait;
				}
				Item entry = m_Entities[i];
				int num;
				if (entry.DownloadDone)
				{
					lock (m_Entities)
					{
						num = activeThreads;
						activeThreads = num + 1;
					}
					ThreadPool.QueueUserWorkItem(delegate
					{
						entry.UpdateCRC();
						lock (m_Entities)
						{
							int num2 = activeThreads;
							activeThreads = num2 - 1;
						}
					});
				}
				num = i + 1;
				i = num;
			}
			while (activeThreads > 0)
			{
				yield return wait;
			}
			WriteDict();
		}

		public static EItemType GetCacheItemType(AssetBundleInfo _bundleInfo)
		{
			EItemType result = EItemType.Simple;
			AssetBundleInfo.EType type = _bundleInfo.Type;
			if (type == AssetBundleInfo.EType.RuntimeAssets || type == AssetBundleInfo.EType.SoundContainer)
			{
				result = EItemType.Zip;
			}
			return result;
		}

		private void InvokeProgress(string _url, uint _progress)
		{
			ProgressDelegate.Trigger(_url, _progress);
		}

		private static BBList<Item> ReadDictionary(string _filename, string _basePath)
		{
			if (!File.Exists(_filename))
			{
				return new BBList<Item>();
			}
			BBList<Item> bBList = new BBList<Item>();
			using (FileStream stream = File.OpenRead(_filename))
			{
				using (DataSerializerBase dataSerializerBase = new DataBinarySerializer(stream, EMode.EMode_Read))
				{
					dataSerializerBase.SerializeBeginNode("Cache");
					if (dataSerializerBase.SerializeUInt16("version", 0) != 8)
					{
						throw new Exception("CACHE: Cache version changed - clearing entries (forced reload!)");
					}
					dataSerializerBase.SerializeBeginNode("Items");
					int num = dataSerializerBase.SerializeItemCount();
					for (int i = 0; i < num; i++)
					{
						Item item = null;
						dataSerializerBase.SerializeBeginNode("Item");
						string text = dataSerializerBase.SerializeString("Type");
						if (text == typeof(SimpleItem).Name)
						{
							item = new SimpleItem(_basePath);
						}
						else if (text == typeof(ZippedItem).Name)
						{
							item = new ZippedItem(_basePath);
						}
						item.Serialize(dataSerializerBase);
						bBList.Add(item);
						dataSerializerBase.SerializeEndNode();
					}
					dataSerializerBase.SerializeEndNode();
					dataSerializerBase.SerializeEndNode();
					return bBList;
				}
			}
		}

		public static void WriteDictionary(BBList<Item> _entities, string _filename)
		{
			try
			{
				if (File.Exists(_filename))
				{
					SharedMain.FixFilePermissions(_filename);
				}
				using (FileStream stream = File.Create(_filename))
				{
					using (DataSerializerBase dataSerializerBase = new DataBinarySerializer(stream, EMode.EMode_Write))
					{
						dataSerializerBase.SerializeBeginNode("Cache");
						dataSerializerBase.SerializeUInt16("version", 8);
						dataSerializerBase.SerializeBeginNode("Items");
						dataSerializerBase.SerializeItemCount(_entities.Count);
						for (int i = 0; i < _entities.Count; i++)
						{
							dataSerializerBase.SerializeBeginNode("Item");
							dataSerializerBase.SerializeString("Type", _entities[i].GetType().Name);
							_entities[i].Serialize(dataSerializerBase);
							dataSerializerBase.SerializeEndNode();
						}
						dataSerializerBase.SerializeEndNode();
						dataSerializerBase.SerializeEndNode();
					}
				}
				SharedMain.FixFilePermissions(_filename);
			}
			catch (Exception)
			{
			}
		}
	}
}
