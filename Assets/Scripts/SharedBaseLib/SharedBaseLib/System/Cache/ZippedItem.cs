using System;
using System.Collections;
using System.IO;
using System.Threading;
using Ionic.Crc;
using Ionic.Zlib;

namespace SharedBaseLib.System.Cache
{
	public class ZippedItem : SimpleItem
	{
		public ZippedItem(string _baseDir)
			: base(_baseDir)
		{
		}

		public ZippedItem(uint _revision, string _name, string _baseDir)
			: base(_revision, _name, _baseDir)
		{
		}

		public override string ToString()
		{
			string cacheFile = base.CacheFile;
			object[] obj = new object[4]
			{
				(base.Name.Length > 64) ? base.Name.Substring(base.Name.Length - 64, 64) : base.Name,
				base.Revision,
				null,
				null
			};
			string text;
			if (cacheFile.Length <= 64)
			{
				text = cacheFile;
			}
			else
			{
				text = cacheFile.Substring(cacheFile.Length - 64, 64);
			}
			obj[2] = text;
			obj[3] = base.Size;
			return string.Format("ZIP {0} #{1} (aka {2}) {3} bytes", obj);
		}

		public override IEnumerator Prepare(string _tempFile, bool _deleteCacheFile)
		{
			Location = _tempFile;
			string targetFn = Guid.NewGuid().ToString();
			bool jobDone = false;
			string cacheFile = CacheFile;
			Exception error = null;
			CRC32 crcCalc = Item.ConstructCRC32Generator();
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					using (FileStream fileStream2 = File.OpenRead(base.CacheFile))
					{
						using (FileStream stream = File.Create(Path.Combine(m_baseDir, targetFn)))
						{
							using (ZlibStream zlibStream = new ZlibStream(stream, CompressionMode.Decompress))
							{
								int count;
								while ((count = fileStream2.Read(Manager.Buffer, 0, Manager.Buffer.Length)) > 0)
								{
									crcCalc.SlurpBlock(Manager.Buffer, 0, count);
									zlibStream.Write(Manager.Buffer, 0, count);
								}
							}
						}
					}
					if (_deleteCacheFile)
					{
						File.Delete(base.CacheFile);
					}
					Location = targetFn;
					SharedMain.FixFilePermissions(base.CacheFile);
				}
				catch (Exception ex)
				{
					error = ex;
				}
				jobDone = true;
			});
			while (!jobDone)
			{
				yield return 0;
			}
			CurrentSize = 0uL;
			if (error == null)
			{
				CRC = crcCalc.Crc32Result;
				bool flag = _deleteCacheFile;
				using (FileStream fileStream = File.OpenRead(CacheFile))
				{
					ZippedItem zippedItem = this;
					ulong currentSize = (Size = (uint)fileStream.Length);
					zippedItem.CurrentSize = currentSize;
				}
			}
		}
	}
}
