using System;
using System.Collections;
using System.IO;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace SharedBaseLib.System.Cache
{
	public class SimpleItem : Item
	{
		private const string TagLocation = "loc";

		private string m_location = string.Empty;

		protected string m_baseDir;

		public override string Location
		{
			get
			{
				return m_location;
			}
			internal set
			{
				m_location = value;
				base.CacheFile = Path.Combine(m_baseDir, Location);
			}
		}

		public SimpleItem(string _baseDir)
		{
			base.CacheFile = string.Empty;
			m_baseDir = _baseDir;
		}

		public SimpleItem(uint _revision, string _name, string _baseDir)
			: base(_revision, _name)
		{
			m_baseDir = _baseDir;
			Location = string.Empty;
		}

		internal override void CleanUp()
		{
			string cacheFile = base.CacheFile;
			if (File.Exists(cacheFile))
			{
				File.SetAttributes(cacheFile, FileAttributes.Normal);
				File.Delete(cacheFile);
			}
			base.CleanUp();
		}

		public override bool Verify(ref BBList<string> _files)
		{
			if (base.CurrentSize != 0)
			{
				FileInfo fileInfo = new FileInfo(base.CacheFile);
				if (!fileInfo.Exists)
				{
					return false;
				}
				ulong length = (ulong)fileInfo.Length;
				if (length != base.CurrentSize)
				{
					return false;
				}
				if (length > base.Size)
				{
					return false;
				}
			}
			_files.Add(base.CacheFile);
			return true;
		}

		public string CopyToCache(string _baseDir)
		{
			Location = Guid.NewGuid().ToString();
			File.Copy(Path.Combine(_baseDir, base.Name), Path.Combine(m_baseDir, Location));
			return Location;
		}

		public override IEnumerator Prepare(string _tempFile, bool _deleteCacheFile)
		{
			Location = _tempFile;
			CacheFile = Path.Combine(m_baseDir, Location);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			IEnumerator ite = UpdateCRCAsync();
			while (ite.MoveNext())
			{
				yield return wait;
			}
		}

		public override bool UpdateCRC()
		{
			int num = 0;
			try
			{
				using (FileStream input = File.OpenRead(base.CacheFile))
				{
					num = Item.ConstructCRC32Generator().GetCrc32(input);
				}
			}
			catch (Exception)
			{
			}
			if (base.CRC == 0)
			{
				base.CRC = num;
			}
			if (base.CRC != 0)
			{
				return base.CRC != num;
			}
			return false;
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
			return string.Format("{0} #{1} (aka {2}) {3} bytes", obj);
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Location = _s.SerializeString("loc", Location);
		}
	}
}
